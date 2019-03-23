Imports SysproQ.Entity
Imports System.Text
Partial Public Class SORTOI
    Private _Xmlin As String
    Private passedXml As XElement
    Private OrderHdr As New List(Of OrderHeaderDataObject)
    Private orderDetails As New List(Of OrderDetailDataObject)
    Private _PostOut As New SysproPostXmlOutResult
    Private _signInInfo As SysproSignInObj
    Private _errmsg As New StringBuilder
    Private _msg As String
    Private _actionType As String
    Public ReadOnly Property PostResult As Enums.PostResults
    Private _solines As List(Of SoLines)
    Public ReadOnly Property TrnMessage As String
        Get
            Return _msg
        End Get
    End Property
    Private Sub AppendTrnMessage(msg As String)
        If _msg IsNot Nothing Then
            _msg &= Environment.NewLine
        End If
        _msg &= msg
    End Sub

    Public Sub New(Xmlin As String, signIn As SysproSignInObj, actionType As String, slns As List(Of SoLines))
        _Xmlin = Xmlin
        _actionType = actionType
        _solines = slns
        With signIn
            _signInInfo = New SysproSignInObj(.Username, .UserPassWord, .Company, .CompanyPassword)
        End With
    End Sub

    Public Function processXmlIn(salesorder As String) As Enums.PostResults
        passedXml = ParseXmlin()
        LoadDataIntoSalesObject()
        If CheckItemsToCancelValid() Then
            If CheckForOrder() Then
                If ValidateItemsWarehouse() Then
                    Dim p As New SORTOI.ProcessPost(OrderHdr.FirstOrDefault, orderDetails, _solines)
                    _PostOut = p.Execute(_signInInfo, salesorder, _actionType)
                    AppendTrnMessage(p.TrnMessage)
                Else
                    AppendTrnMessage(_errmsg.ToString)
                End If
            Else
                AppendTrnMessage(OrderFailMsg(_errmsg.ToString).ToString)
            End If
        Else
            AppendTrnMessage(_errmsg.ToString)
        End If
        Return Enums.PostResults.Success
    End Function
    Private Function OrderFailMsg(msg) As XElement
        Return <Order>
                   <Post2Result><%= msg %></Post2Result>
                   <Status><%= "NOK" %></Status>
               </Order>
    End Function
    Private Function ParseXmlin() As XElement
        Dim foreignXml As XElement
        foreignXml = XElement.Parse(_Xmlin)
        Return foreignXml
    End Function

    Private Function CheckItemsToCancelValid() As Boolean
        'Check validity of lines that need to be cancelled if order header action type is C
        Dim proceed As Boolean = True
        If _actionType = "C" Then
            Dim b As New Query
            Dim found = b.FillSorDetails(OrderHdr.FirstOrDefault.SalesOrder)
            If found IsNot Nothing Then
                For Each item In _solines
                    If item.LineAction = "D" Then
                        If Not found.Where(Function(c) c.MStockCode = item.StockCode And c.SalesOrderLine = item.PoLine).Any Then
                            _errmsg.AppendLine(FormatLineMsg(item, "Line and Stock Code match is Not valid ").ToString)
                            proceed = False
                        End If
                    Else
                        _errmsg.AppendLine(FormatLineMsg(item, "Line Action " & item.LineAction & " Not Valid for Cancellation").ToString)
                        proceed = False
                    End If
                Next
            Else
                proceed = False
            End If
        End If
        Return proceed
    End Function

    Private Function FormatLineMsg(obj As SoLines, msg As String) As XElement
        Return <StockLine>
                   <StockCode><%= obj.StockCode %></StockCode>
                   <Line><%= obj.PoLine %></Line>
                   <Post2Result><%= msg %></Post2Result>
                   <Status><%= "NOK" %></Status>
               </StockLine>
    End Function

    Private Function ValidateItemsWarehouse() As Boolean
        'Check if the City entered was matched to a warehouse
        Dim canProceed As Boolean = True
        If _actionType = "A" Then
            For Each d In orderDetails
                If d.Warehouse Is Nothing Then
                    canProceed = False
                    _errmsg.AppendLine(FormatDetailLineMsg(d, "Warehouse for City " & d.UserDefined & " not found. Transaction Cancelled").ToString)
                End If
            Next
        End If
        Return canProceed
    End Function
    Private Function FormatDetailLineMsg(obj As OrderDetailDataObject, msg As String) As XElement
        Return <StockLine>
                   <StockCode><%= obj.StockCode %></StockCode>
                   <Line><%= obj.CustomerPoLine %></Line>
                   <Post2Result><%= msg %></Post2Result>
                   <Status><%= "NOK" %></Status>
               </StockLine>
    End Function
    Private Function CheckForOrder() As Boolean
        Dim proceed As Boolean = True
        Select Case _actionType
            Case "A"
                Dim b As New BLL.Query
                Dim found = b.FillSorMaster(OrderHdr.FirstOrDefault.SalesOrder)
                If found IsNot Nothing Then
                    _errmsg.AppendLine("This Order already exists, therefore Order cannot be processed")
                    proceed = False
                End If
            Case "D"
                Dim b As New BLL.Query
                Dim found = b.FillSorMaster(OrderHdr.FirstOrDefault.SalesOrder)
                If found Is Nothing Then
                    _errmsg.AppendLine("Order not found, transaction cannot be completed.")
                    proceed = False
                ElseIf found IsNot Nothing Then
                    If found.CancelledFlag = "Y" Then
                        _errmsg.AppendLine("This order was already Cancelled, transaction cannot be completed.")
                        proceed = False
                    End If
                End If
        End Select
        Return proceed
    End Function
    Private Sub LoadDataIntoSalesObject()
        'Create the Objects to post to Syspro
        Dim doc As XElement = passedXml
        Dim ordHd As New OrderHeaderDataObject
        With ordHd
            .SalesOrder = doc.Element("SalesOrder").Value
            .CustomerPoNumber = doc.Element("PO").Value
            .Customer = doc.Element("Customer").Value
            .OrderActionType = doc.Element("ActionType").Value
            .OrderDate = Now.Date.ToString("yyyy-MM-dd")
        End With
        OrderHdr.Add(ordHd)
        'Load Detail    
        Dim detailInfo = doc.Descendants("StockLine")
        For Each detail In detailInfo
            Dim ordDet As New OrderDetailDataObject
            With ordDet
                .StockCode = detail.Element("StockCode").Value
                .OrderQty = detail.Element("Qty").Value
                .Price = detail.Element("Price").Value
                .Warehouse = GetWarehouse(detail.Element("City").Value)
                .LineActionType = detail.Element("LineAction").Value
                .CustomerPoLine = detail.Element("PoLine").Value
                .UserDefined = detail.Element("City").Value
            End With
            orderDetails.Add(ordDet)
        Next
    End Sub
    Private Function GetWarehouse(city As String) As String
        Dim b As New BLL.Query
        Dim ct = b.FillWarehouseByCity(city)
        If ct IsNot Nothing Then
            Return ct.Warehouse
        Else
            Return Nothing
        End If
    End Function
    Private Function BillingXmlInTemplate() As XElement
        'THis is the xml format that I expect to be passed by thi billing system as a string
        'Then converted to an xml doc
        'then read the xml doc and pass data to my Order objects 
        'Then  read order classes and pass dat to Syspro Xml BO
        'Then Post to Syspro.

        Return <Order>
                   <OrderHeader>
                       <SalesOrder>50005</SalesOrder>
                       <CustomerPoNumber>C1000</CustomerPoNumber>
                       <Customer>610001</Customer>
                       <OrderActionType>A</OrderActionType>
                   </OrderHeader>
                   <OrderDetails>
                       <StockLine>
                           <CustomerPoLine>1</CustomerPoLine>
                           <StockCode>0101</StockCode>
                           <OrderQty>5</OrderQty>
                           <Price>40000</Price>
                       </StockLine>
                       <StockLine>
                           <CustomerPoLine>2</CustomerPoLine>
                           <StockCode>0103</StockCode>
                           <OrderQty>20</OrderQty>
                           <Price>500000</Price>
                       </StockLine>
                   </OrderDetails>
               </Order>

    End Function

End Class
