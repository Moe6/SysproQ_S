Imports SysproQ.Entity

Public Class SORTRA
    Implements IBusinessObjects
    Private obj As List(Of SortraObj)
    Private _trnMessage As String
    Private itemsCount As Integer
    Private _signInInfo As SysproSignInObj
    ' Private _msgs As New System.Text.StringBuilder
    Private _soDetails As List(Of SorDetail)
    Public Property TotalSuccess As Boolean
    Public Property PartSuccess As Boolean
    Public Property TotalFail As Boolean
    Public Property itemsPostSuccess As List(Of SortraObj)
    Public Property itemsPostFail As List(Of SortraObj)
    Public Property itemsPostWarning As List(Of SortraObj)
    Private _salesOrder As String
    Public ReadOnly Property TrnMessage As String Implements IBusinessObjects.TrnMessage
        Get
            Return _trnMessage
        End Get
    End Property

    Private Sub AppendTrnMessage(msg As String)
        If _trnMessage IsNot Nothing Then
            _trnMessage &= Environment.NewLine
        End If
        _trnMessage &= msg
    End Sub

#Region "Perpare Things"
    Public Sub New(SignInObj As SysproSignInObj, SortraData As List(Of SortraObj))
        obj = SortraData
    End Sub

    Public Sub New(signinIfo As SysproSignInObj, xmlin As String)
        'create sortraObj using salesorder number
        GetSalesOrder(xmlin)
        'Query Db for sales order data
        fillsalesOrderDetails(_salesOrder)
        If _soDetails.Count > 0 Then
            createSortraObj(xmlin)
            itemsCount = obj.Count
            _signInInfo = signinIfo
        End If

    End Sub
    Private Function GetSalesOrder(xmlin As String) As Boolean
        Dim xl As XElement
        xl = XElement.Parse(xmlin)
        _salesOrder = xl.Element("SalesOrder").Value
        If _salesOrder IsNot Nothing Then
            Return True
        End If
        Return False
    End Function
    Private Sub createSortraObj(xmlin As String)
        'Object must link stockcode from xmlin with sodetails on stockcode 
        'and get xmlin qty and details line numbers
        Dim doc As XElement = ParseXmlin(xmlin)
        Dim detailInfo = doc.Descendants("StockLine")
        obj = New List(Of SortraObj)
        If detailInfo.Count > 0 Then
            For Each detail In detailInfo
                Dim line = _soDetails.Where(Function(c) c.MStockCode = detail.Element("StockCode").Value).FirstOrDefault
                If line IsNot Nothing Then
                    Dim sora As New SortraObj
                    sora.SalesOrder = line.SalesOrder
                    sora.Line = line.SalesOrderLine
                    sora.StockCode = line.MStockCode 'opitional
                    'Make qty equal to xmlin Qty
                    sora.Qty = detail.Element("Qty").Value
                    obj.Add(sora)
                Else
                    AppendTrnMessage("Stock code " & detail.Element("StockCode").Value & " could not be identified on the sales order.")
                End If
            Next
        Else
            AppendTrnMessage("No Stock Line elements found in xml.")
        End If

        'For Each line In _soDetails
        '    Dim sora As New SortraObj
        '    sora.SalesOrder = line.SalesOrder
        '    sora.Line = line.SalesOrderLine
        '    sora.StockCode = line.MStockCode 'opitional
        '    sora.Qty = line.QtyReserved
        '    obj.Add(sora)
        'Next
    End Sub
    Private Function ParseXmlin(xmlin As String) As XElement
        Dim parsedXml As XElement
        parsedXml = XElement.Parse(xmlin)
        Return parsedXml
    End Function
    Private Sub fillsalesOrderDetails(so As String)
        Dim b As New BLL.Query
        _soDetails = b.FillSorDetails(so)
    End Sub
#End Region

#Region "BO things"
    'Commented out becasue I decided to post eahc line at a time
    Private Function CreateXmlIn() As String Implements IBusinessObjects.CreateXmlIn
        Dim xmlin = <PostSorAllocateReserved xmlns:xsd="http://www.w3.org/2001/XMLSchema-instance" xsd:noNamespaceSchemaLocation="SORTRADOC.XSD">
                        <%= CreateDetailitems() %>
                    </PostSorAllocateReserved>
        Return xmlin.ToString
    End Function

    Private Function CreateDetailitems() As XElement()
        Dim elements(itemsCount - 1) As XElement
        For i As Integer = 0 To itemsCount - 1
            elements(i) = CreateItemLineElement(obj(i))
        Next
        Return elements
    End Function

    Private Function CreateXmlIn2(line As SortraObj) As String Implements IBusinessObjects.CreateXmlIn2
        Dim xmlin = <PostSorAllocateReserved xmlns:xsd="http://www.w3.org/2001/XMLSchema-instance" xsd:noNamespaceSchemaLocation="SORTRADOC.XSD">
                        <%= CreateDetailitems(line) %>
                    </PostSorAllocateReserved>
        Return xmlin.ToString
    End Function

    Private Function CreateDetailitems(line As SortraObj) As XElement()
        Dim element(1) As XElement
        For i As Integer = 0 To itemsCount - 1
            element(i) = CreateItemLineElement(line)
        Next
        Return element
    End Function
    Private Function CreateItemLineElement(item As SortraObj) As XElement
        Return <Item>
                   <Customer/>
                   <SalesOrder><%= item.SalesOrder %></SalesOrder>
                   <SalesOrderLine><%= item.Line %></SalesOrderLine>
                   <StockCode><%= item.StockCode %></StockCode>
                   <Quantity><%= item.Qty %></Quantity>
                   <UnitOfMeasure/>
                   <Units/>
                   <Pieces/>
                   <Lot/>
                   <Serials>
                       <SerialNumber/>
                       <SerialQuantity/>
                       <SerialUnits/>
                       <SerialPieces/>
                   </Serials>
                   <Bins/>
               </Item>
    End Function

    Public Function GetName() As String Implements IBusinessObjects.GetName
        Return "SORTRA"
    End Function

    Public Function ValidateInput() As Boolean Implements IBusinessObjects.ValidateInput
        Throw New NotImplementedException()
    End Function

    Private Function CreateXmlParams(validateOnly As Boolean) As String Implements IBusinessObjects.CreateXmlParams
        Dim params = <PostSorAllocateReserved xmlns:xsd="http://www.w3.org/2001/XMLSchema-instance" xsd:noNamespaceSchemaLocation="SORTRA.XSD">
                         <Parameters>
                             <IgnoreWarnings>N</IgnoreWarnings>
                             <ApplyIfEntireDocumentValid>N</ApplyIfEntireDocumentValid>
                             <ValidateOnly>N</ValidateOnly>
                             <IgnoreAutoDepletion>N</IgnoreAutoDepletion>
                             <AccumulateAllocatedQuantity>N</AccumulateAllocatedQuantity>
                         </Parameters>
                     </PostSorAllocateReserved>
        Return params.ToString
    End Function

    Public Sub PostPerItem()
        'itemsPostFail = New List(Of SortraObj)
        'itemsPostSuccess = New List(Of SortraObj)
        'itemsPostWarning = New List(Of SortraObj)
        'Dim successCount As Integer

        'For Each item In obj
        '    Dim xmlin As String = CreateXmlIn2(item)
        '    Dim xmlparam As String = CreateXmlParams(False)
        '    Dim postResult As SysproPostXmlOutResult
        '    Dim po As New Post(_signInInfo, GetName, xmlin, xmlparam, "A")

        '    postResult = po.ConfirmXmlOut()
        '    If postResult IsNot Nothing Then
        '        'Check for messages in Xml out
        '        If postResult.WarningsFound Then
        '            ' If postResult.WarningMessages.Any(Function(c) c.ToUpper = "") Then
        '            For Each errm In postResult.WarningMessages
        '                AppendTrnMessage(errm.ToString & vbNewLine)
        '            Next
        '            '  End If
        '        ElseIf postResult.ErrorsFound Then
        '            'If postResult.ErrorMessages.Any(Function(c) c.ToUpper = "") Then
        '            For Each errm In postResult.ErrorMessages
        '                AppendTrnMessage("Fail")
        '                AppendTrnMessage(errm.ToString & vbNewLine)
        '                itemsPostFail.Add(item)
        '            Next
        '            'End If
        '        ElseIf postResult.Successful Then
        '            AppendTrnMessage("Success")
        '            successCount += 1
        '            itemsPostSuccess.Add(item)
        '        End If
        '    Else
        '        'MsgBox(po.TrnMessage, ServiceNotification)
        '    End If
        'Next

        ''Evaluate Result
        'If successCount = itemsCount Then
        '    TotalSuccess = True
        'ElseIf successCount > 0 Then
        '    PartSuccess = True
        'Else
        '    TotalFail = True
        'End If

    End Sub

    Public Function Post() As Boolean
        Dim xmlin As String = CreateXmlIn()
        Dim xmlparam As String = CreateXmlParams(False)
        Dim postResult As SysproPostXmlOutResult
        Dim result As Boolean
        If itemsCount > 0 Then
            Using po As New Post(_signInInfo, GetName, xmlin, xmlparam, _soDetails)
                po.Excecute()
                postResult = po.ConfirmXmlOut()
                If postResult IsNot Nothing Then
                    'Check for messages in Xml out
                    If postResult.WarningsFound Then
                        For Each errm In postResult.WarningMessages
                            AppendTrnMessage(errm.ToString & vbNewLine)
                        Next
                    End If

                    If postResult.ErrorsFound Then
                        For Each errm In postResult.ErrorMessages
                            AppendTrnMessage("Fail")
                            AppendTrnMessage(errm.ToString & vbNewLine)
                        Next
                    End If

                    If postResult.Successful Then
                        For Each m In postResult.OtherMessages
                            AppendTrnMessage(m)
                        Next
                        AppendTrnMessage("Success")
                        result = True
                    End If
                Else
                    AppendTrnMessage("Sortra Post result returned failure")
                End If
            End Using
        Else
            AppendTrnMessage("Could not identify sales order items.")
        End If
        Return result
    End Function

#Region "Template"
    Private Function sortraOuttemplat() As XElement
        Return <postsorallocatereserved xmlns:xsd="http://www.w3.org/2001/XMLSchema-instance" Language="05" Language2="EN" CssStyle="" DecFormat="1" DateFormat="01" Role="01" Version="6.1.000" OperatorPrimaryRole=" " xsd:noNamespaceSchemaLocation="SORTRAOUT.XSD">
                   <Item>
                       <ValidationStatus>
                           <SalesOrderDetails>
                               <SalesOrder>000876</SalesOrder>
                               <SalesOrderLine>00001</SalesOrderLine>
                           </SalesOrderDetails>
                           <Status>Successful</Status>
                           <Reason/>
                       </ValidationStatus>
                       <ItemNumber>000001</ItemNumber>
                   </Item>
                   <StatusOfItems>
                       <ItemsProcessed>000001</ItemsProcessed>
                       <ItemsInvalid>000000</ItemsInvalid>
                   </StatusOfItems>
               </postsorallocatereserved>
    End Function
#End Region


#End Region



End Class
