Imports SysproQ.Entity
Imports SYSPROWCFServicesClientLibrary40

Public Class Post
    Implements IDisposable

    Private Property Username As String
    Private Property UserPW As String
    Private Property Company As String
    Private Property CompanyPW As String
    Private _businessObject As String
    Private _xmlIn As String
    Private _xmlParams As String
    Private _xmlOut As String
    'Public Qry As New SysproQueryService.queryclass
    'Public Tran As New SysproTransService.transactionclass
    'Public Util As New SysproUtilitiesService.Logon_With_String()
    Public _wcf As SYSPROWCFServicesClient
    Public _wcfPrima As SYSPROWCFServicesPrimitiveClient

    Public _transactionXmlOut As New SysproPostXmlOutResult
    Public msgHelper As New MessagingHelper

    Private signinInfo As SysproSignInObj
    Private _sorDetails As List(Of SorDetail)
    Private _postedOrder As List(Of SorDetail)
    Private _omaster As SorMaster
    Private _actionType As String
    Private _trnMessage As String = Nothing
    Private _soLines As List(Of SoLines)
    Private _kitItems As List(Of Kit)
    Private _ChildNodesToCancel As List(Of SorDetail)
    Private _warehouseComponents As List(Of InvWarehouse)
    Private _kitItemstoUpdate As List(Of SorDetail)

    Public ReadOnly Property PostResult As SysproPostXmlOutResult
        Get
            Return _transactionXmlOut
        End Get
    End Property

    Public ReadOnly Property TrnMessage As String
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

    Public Sub New(loginInfo As SysproSignInObj, ByVal businessObj As String, ByVal xmlIn As String,
                   ByVal xmlParam As String, ActionType As String, slns As List(Of SoLines), kititems As List(Of Kit))
        _businessObject = businessObj
        _xmlIn = xmlIn
        _xmlParams = xmlParam
        _xmlOut = Nothing
        _transactionXmlOut = New SysproPostXmlOutResult
        signinInfo = loginInfo
        _actionType = ActionType
        _soLines = slns
        _kitItems = kititems
    End Sub

    Public Sub New(loginInfo As SysproSignInObj, ByVal businessObj As String, ByVal xmlIn As String,
                   ByVal xmlParam As String, soDetails As List(Of SorDetail))
        '_businessObject = businessObj
        '_xmlIn = xmlIn
        '_xmlParams = xmlParam
        '_xmlOut = Nothing
        '_transactionXmlOut = New SysproPostXmlOutResult
        'signinInfo = loginInfo
        '_sorDetails = soDetails

    End Sub

    Public Function Excecute() As Boolean
        Try
            With signinInfo
                _wcf = New SYSPROWCFServicesClient("net.tcp://localhost:20000/SYSPROWCFService/Rest",
                                                   SYSPROWCFBinding.NetTcp, .Username, .UserPassWord, .Company, .CompanyPassword)
            End With

            _transactionXmlOut.XmlOut = _wcf.TransactionPost(_businessObject, _xmlParams, _xmlIn)

        Catch ex As Exception
            AppendTrnMessage("Syspro Posting Failure" & vbCrLf & vbCrLf & "Exception: " & msgHelper.GetFullMessage(ex))
        End Try

        Return _transactionXmlOut.XmlOut IsNot Nothing
    End Function

    Public Function ConfirmXmlOut(Optional salesorder As String = "") As SysproPostXmlOutResult
        Dim returnObj As SysproPostXmlOutResult = Nothing
        If _transactionXmlOut.XmlOut IsNot Nothing Then
            returnObj = New SysproPostXmlOutResult
            returnObj.XmlOut = _transactionXmlOut.XmlOut
            If _businessObject = "SORTOI" Then
                If _actionType = "A" Then
                    'Query db to check if sales order items were succefully reserved
                    If CheckSalesOrderPost(salesorder) Then
                        returnObj.Successful = True
                        If _trnMessage Is Nothing Then
                            'Error would have occured in posting
                            GetOtherErrors(returnObj)
                        End If
                    Else
                        'Error would have occured in posting
                        GetOtherErrors(returnObj)
                    End If
                Else
                    'Action was Cancel Order or lines
                    CheckBoForErrors(returnObj)
                    If Not returnObj.ErrorsFound Then
                        GetCancellationResult(salesorder)
                        GetOtherErrors(returnObj)
                    End If
                End If
            End If
        End If
        Return returnObj
    End Function

    Private Sub processErrorMessages(xdoc As XDocument, ByRef returnObj As SysproPostXmlOutResult)
        Dim itemsFound = xdoc.Descendants("Item")
        If itemsFound.Count > 0 Then
            For Each item In itemsFound
                Try
                    Dim vStatus As String = item.Element("ValidationStatus").Element("Status").Value
                    If vStatus = "Failed" Then
                        _transactionXmlOut.ErrorsFound = True
                        returnObj.ErrorsFound = True
                        Dim stockcode As String = ""
                        Dim LineNumber As String = ""
                        Dim errorDescription As String = ""
                        'Get line Number
                        LineNumber = item.Element("ValidationStatus").Element("SalesOrderDetails").Element("SalesOrderLine").Value
                        errorDescription = item.Element("salesorderline").Element("ErrorDescription").Value
                        'get the stockcode value from the order details object
                        Dim ln As Decimal = CDec(LineNumber)
                        If _sorDetails IsNot Nothing Then
                            Dim d = _sorDetails.Where(Function(c) c.SalesOrderLine = ln).FirstOrDefault
                            If d IsNot Nothing Then
                                stockcode = d.MStockCode
                            End If
                        End If
                        AppendTrnMessage("Line " & LineNumber & " Stockcode: " & stockcode & " Failed with Error: " & errorDescription)
                        returnObj.ErrorMessages.Add("Line " & LineNumber & " Stockcode: " & stockcode & " Failed with Error: " & errorDescription)
                    End If
                Catch ex As Exception

                End Try
            Next
        End If
    End Sub

    Private Sub processSuccessMessages(xdoc As XDocument, ByRef returnObj As SysproPostXmlOutResult)
        Dim itemsFound = xdoc.Descendants("Item")
        If itemsFound.Count > 0 Then
            For Each item In itemsFound
                Try
                    Dim vStatus As String = item.Element("ValidationStatus").Element("Status").Value
                    If vStatus = "Successful" Then
                        _transactionXmlOut.Successful = True
                        returnObj.Successful = True
                        Dim stockcode As String = ""
                        Dim LineNumber As String = ""
                        'Get line Number
                        LineNumber = item.Element("ValidationStatus").Element("SalesOrderDetails").Element("SalesOrderLine").Value
                        'get the stockcode value from the order details object
                        Dim ln As Decimal = CDec(LineNumber)
                        If _sorDetails IsNot Nothing Then
                            Dim d = _sorDetails.Where(Function(c) c.SalesOrderLine = ln).FirstOrDefault
                            If d IsNot Nothing Then
                                stockcode = d.MStockCode
                            End If
                        End If
                        AppendTrnMessage("Line " & LineNumber & " Stockcode: " & stockcode & " was Successfully Reserved.")
                        returnObj.OtherMessages.Add("Line " & LineNumber & " Stockcode: " & stockcode & " was Successfully Reserved.")
                    End If
                Catch ex As Exception

                End Try
            Next
        End If
    End Sub

    Private Function CheckSalesOrderPost(salesorder As String) As Boolean
        Dim b As New Query
        Dim result As Boolean
        'Fill the created sales order
        _omaster = b.FillSorMaster(salesorder)
        _postedOrder = b.FillSorDetails(salesorder)
        If _omaster IsNot Nothing Then
            If _omaster.CancelledFlag <> "Y" Then
                If _postedOrder.Count > 0 Then
                    'Need to fomrat headers
                    For Each item In _postedOrder
                        'Check if item is a Kit item
                        'If Kit then get all components. If any commponents are not reserved then reverse this kit item and all its components and mark all as not ok.
                        result = True
                        If Trim(item.MBomFlag) = "" Then
                            'this means item is not BomStructure ite and is a single item
                            ValidateItemQtyPosted(item)
                        Else
                            ' item Is BomStructure item
                            If Trim(item.MBomFlag) = "P" Then
                                'this is Parent Item in Bom Structure
                                'Validate all its Children and process Accordingly
                                result = ProcessBomItems(item)
                            End If
                        End If
                    Next
                Else
                    AppendTrnMessage("Could not identify items for created  order.")
                End If
            Else
                AppendTrnMessage("This is a cancelled order.")
            End If
        Else
            AppendTrnMessage("An Error occured.")
        End If
        Return result
    End Function

    Private Sub ValidateItemQtyPosted(item As SorDetail)
        Dim msg As String
        'this means item is not BomStructure ite and is a single item
        If item.QtyReserved > 0 Then
            If item.MOrderQty = item.QtyReserved Then 'OK
                msg = ("Stock code " & item.MStockCode & " Quantity " & CInt(item.QtyReserved) & " Reserve Successful")
                Dim x = FormatResult(item, msg, "OK")
                AppendTrnMessage(x.ToString)
            Else 'POK
                msg = ("Stock code " & item.MStockCode & " Quantity " & CInt(item.QtyReserved) & " Reserve Partial")
                Dim x = FormatResult(item, msg, "POK")
                AppendTrnMessage(x.ToString)
            End If
        ElseIf item.MBackOrderQty > 0 Then
            msg = ("Stock code " & item.MStockCode & " Quantity " & CInt(item.MBackOrderQty) & " Reserve Fail")
            Dim x = FormatResult(item, msg, "NOK")
            AppendTrnMessage(x.ToString)
        End If
    End Sub

    Private Function ProcessBomItems(parentItem As SorDetail) As Boolean
        If ValidateParentBomItemPostQty(parentItem) Then
            Return True
        End If
        Return False
    End Function

#Region "BOM Structure Validation"

    Private Function IsKitItem(stockcode As String) As Boolean
        If _kitItems IsNot Nothing Then
            Dim found = _kitItems.Where(Function(c) c.StockCode = stockcode)
            Return found IsNot Nothing
        End If
        Return False
    End Function

    Private Function ValidateParentBomItemPostQty(parentItem As SorDetail) As Boolean
        'Get All Child Nodes for Parent Bom Item
        _kitItemstoUpdate = New List(Of SorDetail)
        Dim fComps = _kitItems.Where(Function(c) c.ComponentOf = parentItem.MStockCode).ToList

        'Get all the child Nodes for this Kit Parent item
        For Each comp In fComps
            Dim found = _postedOrder.Where(Function(c) c.MStockCode = comp.StockCode And c.SalesOrderLine > parentItem.SalesOrderInitLine _
                And Trim(c.MBomFlag) = "C").FirstOrDefault
            If found IsNot Nothing Then
                _kitItemstoUpdate.Add(found)
            End If
        Next

        If _kitItemstoUpdate.Count > 0 Then
            'Add parent item for processing
            _kitItemstoUpdate.Add(parentItem)
            If ValidateAvailableQtyForComponnents(_kitItemstoUpdate) Then
                If ReserveComponents(_kitItemstoUpdate) Then
                    If UpdateWarehouseAllocation(_kitItemstoUpdate) Then
                        If Update() Then
                            ParentItemSuccessMessges(parentItem)
                            Return True
                        End If
                    End If
                End If
            End If
        End If
        ParentItemFailMessges(parentItem)
        Return False
    End Function

    Private Function VerifyPostOfChildNodes(ls As List(Of SorDetail)) As Boolean
        Dim verified As Boolean = True
        If ls.Any(Function(c) c.QtyReserved <> c.MOrderQty) Then
            verified = False
        End If
        Return verified
    End Function

    Private Sub ParentItemFailMessges(item As SorDetail)
        Dim msg As String
        msg = ("Stock code " & item.MStockCode & " Quantity " & CInt(item.MOrderQty) & " Reserve Fail for All or some components of this kit item.")
        Dim x = FormatResult(item, msg, "NOK")
        AppendTrnMessage(x.ToString)
    End Sub

    Private Sub ParentItemSuccessMessges(item As SorDetail)
        Dim msg As String
        msg = ("Stock code " & item.MStockCode & " Quantity " & CInt(item.QtyReserved) & " Reserve Successful")
        Dim x = FormatResult(item, msg, "OK")
        AppendTrnMessage(x.ToString)
    End Sub

#Region "Handle Qty Updates and Validation"

    Private Function ValidateAvailableQtyForComponnents(components As List(Of SorDetail)) As Boolean
        Dim validCount As Integer = 0
        _warehouseComponents = New List(Of InvWarehouse)
        For Each item In components.Where(Function(c) c.MBomFlag = "C").ToList
            Dim b As New Query
            Dim warehouse = b.FillInvWarehouse(item.MStockCode, item.MWarehouse)
            If warehouse IsNot Nothing Then
                If (warehouse.QtyOnHand - warehouse.QtyAllocated) >= item.MOrderQty Then
                    validCount += 1
                    'Add warehouse component to list for update if all items successful
                    _warehouseComponents.Add(warehouse)
                End If
            End If
        Next
        Return validCount = components.Count - 1
    End Function

    Private Function ReserveComponents(components As List(Of SorDetail)) As Boolean
        'Update Reserved Qty for Kit Component items including the Kit Item
        For Each item In components
            item.QtyReserved = item.MOrderQty
            item.QtyReservedShip = item.MOrderQty
            item.MBackOrderQty = 0
            item.MShipQty = 0
        Next
        Return True
    End Function

    Private Function UpdateWarehouseAllocation(components As List(Of SorDetail)) As Boolean
        'Update warehouse Alllocated Qty for each of the Componets reserved
        Dim itemCount As Integer = 0
        For Each component In components.Where(Function(c) c.MBomFlag = "C").ToList
            Dim found = _warehouseComponents.Where(Function(c) c.StockCode = component.MStockCode And c.Warehouse = component.MWarehouse).FirstOrDefault
            If found IsNot Nothing Then
                found.QtyAllocated += component.MOrderQty
                itemCount += 1
            End If
        Next
        Return itemCount = components.Where(Function(c) c.MBomFlag = "C").Count
    End Function

    Private Function Update() As Boolean
        Dim bll As New Update
        bll.Update(_warehouseComponents)
        bll.Update(_kitItemstoUpdate)
        If bll.Save Then
            Return True
        End If
        Return False
    End Function

#End Region



#End Region


    Private Function FormatResult(item As SorDetail, msg As String, result As String) As XElement
        Return <StockLine>
                   <Line><%= item.SalesOrderLine %></Line>
                   <Post2Result><%= msg %></Post2Result>
                   <StockCode><%= item.MStockCode %></StockCode>
                   <QtyReserved><%= item.QtyReserved %></QtyReserved>
                   <QtyFailed><%= item.MBackOrderQty %></QtyFailed>
                   <Status><%= result %></Status>
               </StockLine>
    End Function

    Private Function FormatCancelResult(sln As SoLines, msg As String, result As String) As XElement
        Return <StockLine>
                   <StockCode><%= sln.StockCode %></StockCode>
                   <Post2Result><%= msg %></Post2Result>
                   <Status><%= result %></Status>
               </StockLine>
    End Function

    Private Function FormatOtherResultMsg(msg As String) As XElement
        Return <SysproErrorMessage>
                   <Error><%= msg %></Error>
                   <Status><%= "NOK" %></Status>
               </SysproErrorMessage>
    End Function

    Private Sub GetOtherErrors(ByRef returnObj As SysproPostXmlOutResult)
        'read xml data
        Dim xDoc = XDocument.Parse(_transactionXmlOut.XmlOut)
        'get any errors if exists
        Dim errortags = xDoc.Descendants("ErrorDescription")
        If errortags.Count > 0 Then
            returnObj.ErrorsFound = True
            _transactionXmlOut.ErrorsFound = True
            For Each et As XElement In errortags
                _transactionXmlOut.ErrorMessages.Add(Trim(et.Value.ToString))
                'Format error result for out
                Dim r = FormatOtherResultMsg(et.Value.ToString)
                AppendTrnMessage(r.ToString)
                returnObj.ErrorMessages.Add(Trim(et.Value.ToString))
            Next
        End If
        'get any warnings if exists
        Dim warningtags = xDoc.Descendants("WarningMessages")
        If warningtags.Count > 0 Then
            returnObj.WarningsFound = True
            _transactionXmlOut.WarningsFound = True
            For Each wt In warningtags
                _transactionXmlOut.WarningMessages.Add(wt.Value.ToString)
                'Format warning msg for out
                Dim r = FormatOtherResultMsg(wt.Value.ToString)
                AppendTrnMessage(r.ToString)
                returnObj.WarningMessages.Add(wt.Value.ToString)
            Next
        End If
        'AppendTrnMessage("<StockLine><Status>NOK</Status></StockLine>")
    End Sub

    Private Function CheckBoForErrors(ByRef returnObj As SysproPostXmlOutResult) As Boolean
        'read xml data
        Dim xDoc = XDocument.Parse(_transactionXmlOut.XmlOut)
        'get any errors if exists
        Dim errortags = xDoc.Descendants("ErrorDescription")
        If errortags.Count > 0 Then
            returnObj.ErrorsFound = True
            _transactionXmlOut.ErrorsFound = True
            For Each et As XElement In errortags
                _transactionXmlOut.ErrorMessages.Add(Trim(et.Value.ToString))
                'Format error result for out
                Dim r = FormatOtherResultMsg(et.Value.ToString)
                AppendTrnMessage(r.ToString)
                returnObj.ErrorMessages.Add(Trim(et.Value.ToString))
            Next
        End If
        'get any warnings if exists
        Dim warningtags = xDoc.Descendants("WarningMessages")
        If warningtags.Count > 0 Then
            returnObj.WarningsFound = True
            _transactionXmlOut.WarningsFound = True
            For Each wt In warningtags
                _transactionXmlOut.WarningMessages.Add(wt.Value.ToString)
                'Format warning msg for out
                Dim r = FormatOtherResultMsg(wt.Value.ToString)
                AppendTrnMessage(r.ToString)
                returnObj.WarningMessages.Add(wt.Value.ToString)
            Next
        End If
        Return True
    End Function

    Private Sub GetCancellationResult(salesorder As String)
        'Fill the created sales order
        Dim b As New BLL.Query
        _omaster = b.FillSorMaster(salesorder)
        _postedOrder = b.FillSorDetails(salesorder)

        If _omaster IsNot Nothing Then
            'check order status
            If _omaster.CancelledFlag = "Y" Then
                'Order was cancelled, no need to check lines
                AppendTrnMessage(FormatOrderCancelMsg("Whole Order Cancelled").ToString)
            Else
                'If _postedOrder.Count > 0 Then
                For Each item In _soLines
                    'PoLine passed for cancellation should be equivalent to the So Line number
                    Dim ln As Decimal = CDec(item.PoLine)
                    If _postedOrder IsNot Nothing Then
                        If Not _postedOrder.Where(Function(c) c.MStockCode = item.StockCode And c.SalesOrderLine = ln).Any Then
                            'if entry is not found it means it has been canceled
                            AppendTrnMessage(FormatCancelResult(item, "Cancelled", "OK").ToString)
                        Else
                            'entry found therefore was not cancelled
                            AppendTrnMessage(FormatCancelResult(item, "Not Cancelled", "NOK").ToString)
                        End If
                    Else
                        'if no entries found  it means it has been canceled
                        AppendTrnMessage(FormatCancelResult(item, "Cancelled", "OK").ToString)
                    End If
                Next
            End If
        End If
    End Sub

    Private Function FormatOrderCancelMsg(msg As String) As XElement
        Return <Order>
                   <Post2Result><%= msg %></Post2Result>
                   <Status><%= "OK" %></Status>
               </Order>
    End Function

    Private Function ParseXmlin(xmlin As String) As XElement
        Dim parsedXml As XElement
        parsedXml = XElement.Parse(xmlin)
        Return parsedXml
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
    End Sub
#End Region


    'Private Function GetTransationReferenceValue(xDoc As XDocument, bo As Enums.BusinessObjectUsed) As String
    '    Dim reference As String = Nothing
    '    Select Case bo
    '        Case Enums.BusinessObjectUsed.PORTOI
    '            'purchase order number
    '            reference = xDoc.<postpurchaseorders>.<Order>.<Key>.<PurchaseOrder>.Value
    '        Case Enums.BusinessObjectUsed.PORTOR
    '            'grn number 
    '            reference = xDoc.<postpurchaseorderreceipts>.<Item>.<Receipt>.<Grn>.Value
    '        Case Enums.BusinessObjectUsed.SORTIC
    '            'invoice number
    '            reference = xDoc.<postsalesorderinvoice>.<Item>.<Key>.<InvoiceNumber>.Value
    '        Case Enums.BusinessObjectUsed.SORTOI
    '            'reserve stock
    '            reference = xDoc.<PostSorAllocateReserved>.<item>.<key>.<SalesOrder>.Value
    '    End Select
    '    Return reference
    'End Function

End Class

Public Class SysproPostXmlOutResult

    Public Sub New()
        ErrorsFound = False
        WarningsFound = False
        Successful = False
        ErrorMessages = New List(Of String)
        WarningMessages = New List(Of String)
        OtherMessages = New List(Of String)
        Reference = Nothing
        itemsRejectedWithWarnings = Nothing
        itemsProcessedWithWarnings = Nothing
        ItemsProcessed = Nothing
    End Sub

    Public Property XmlOut As String
    Public Property ErrorsFound As Boolean
    Public Property ErrorMessages As List(Of String)
    Public Property WarningsFound As Boolean
    Public Property WarningMessages As List(Of String)
    Public Property OtherMessages As List(Of String)
    Public Property itemsRejectedWithWarnings As String
    Public Property itemsProcessedWithWarnings As String
    Public Property Successful As Boolean
    Public Property Reference As String
    Public Property ItemsProcessed As String

End Class

Public Class SysproUtilityObject
    Public Property Username As String
    Public Property UserPW As String
    Public Property Company As String
    Public Property CompanyPW As String
    'Private Sub New()
    'End Sub
    Public Sub New(SignInInfo As SysproSignInObj)
        With SignInInfo
            Me.Username = .Username
            Me.UserPW = .UserPassWord
            Me.Company = .Company
            Me.CompanyPW = .CompanyPassword
        End With

    End Sub
End Class

Public Class SysproSignInObj
    Public Property Username As String
    Public Property UserPassWord As String
    Public Property Company As String
    Public Property CompanyPassword As String
    Public Sub New(ByVal username As String, ByVal userpassword As String, ByVal company As String, ByVal companypassword As String)
        Me.Username = username
        Me.UserPassWord = userpassword
        Me.Company = company
        Me.CompanyPassword = companypassword
    End Sub
End Class
