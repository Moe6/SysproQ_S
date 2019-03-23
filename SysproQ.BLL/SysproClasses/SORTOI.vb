Imports System.Collections.Generic
Imports System.Linq
Imports SysproQ.Entity
Imports System.Xml.Linq

Partial Public Class SORTOI
    Public Class ProcessPost
        Implements IBusinessObjects
        Private _masterData As OrderHeaderDataObject
        Private _detailData As List(Of OrderDetailDataObject)
        Private _trnMessage As String
        Public msgHelper As New Entity.MessagingHelper
        Private _transactionType As TransactionType
        Private _soLines As List(Of SoLines)
        Private _KitItems As List(Of Kit)

        Private Sub AppendTrnMessage(msg As String)
            If _trnMessage IsNot Nothing Then
                _trnMessage &= Environment.NewLine
            End If
            _trnMessage &= msg
        End Sub

        Public ReadOnly Property TrnMessage As String Implements IBusinessObjects.TrnMessage
            Get
                Return _trnMessage
            End Get
        End Property

        Private Enum TransactionType
            SalesOrderCreateMaintain
            OrderMasterOnlyMaintain
        End Enum

        Public Sub New(orderHdr As OrderHeaderDataObject, orderDetail As List(Of OrderDetailDataObject), solns As List(Of SoLines))
            _masterData = orderHdr
            _detailData = orderDetail
            _soLines = solns
            FillKitItems()
        End Sub

        Private Function CreateXmlIn2(obj As SortraObj) As String Implements IBusinessObjects.CreateXmlIn2
            Return Nothing
        End Function

        Private Function CreateTransmissionHeaderElement() As XElement
            Return <TransmissionHeader>
                       <TransmissionReference></TransmissionReference>
                       <SenderCode></SenderCode>
                       <ReceiverCode></ReceiverCode>
                       <DatePrepared><%= Date.Now.ToString("yyyy-MM-dd") %></DatePrepared>
                       <TimePrepared><%= Date.Now.ToString("HH:mm") %></TimePrepared>
                   </TransmissionHeader>
        End Function

        Private Function CreateOrderMasterElement() As XElement
            Dim _header As XElement = Nothing
            Try
                Dim header = <OrderHeader>
                                 <OrderActionType><%= _masterData.OrderActionType %></OrderActionType>
                                 <Supplier/>
                                 <Customer><%= _masterData.Customer %></Customer>
                                 <OrderDate><%= _masterData.OrderDate %></OrderDate>
                                 <InvoiceTerms></InvoiceTerms>
                                 <Currency></Currency>
                                 <ShippingInstrs></ShippingInstrs>
                                 <CustomerName></CustomerName>
                                 <ShipAddress1><%= _masterData.ShipAddress1 %></ShipAddress1>
                                 <ShipAddress2><%= _masterData.ShipAddress2 %></ShipAddress2>
                                 <ShipAddress3><%= _masterData.ShipAddress3 %></ShipAddress3>
                                 <ShipAddress4></ShipAddress4>
                                 <ShipAddress5></ShipAddress5>
                                 <ShipPostalCode><%= _masterData.ShipPostalCode %></ShipPostalCode>
                                 <Email></Email>
                                 <OrderDiscPercent1>0</OrderDiscPercent1>
                                 <OrderDiscPercent2>0</OrderDiscPercent2>
                                 <OrderDiscPercent3>0</OrderDiscPercent3>
                                 <Warehouse><%= _masterData.Warehouse %></Warehouse>
                                 <SpecialInstrs></SpecialInstrs>
                                 <SalesOrder><%= _masterData.SalesOrder %></SalesOrder>
                                 <OrderType><%= _masterData.OrderType %></OrderType>
                                 <MultiShipCode></MultiShipCode>
                                 <AlternateReference/>
                                 <Salesperson></Salesperson>
                                 <Branch></Branch>
                                 <Area></Area>
                                 <RequestedShipDate></RequestedShipDate>
                                 <InvoiceNumberEntered></InvoiceNumberEntered>
                                 <InvoiceDateEntered></InvoiceDateEntered>
                                 <OrderComments></OrderComments>
                                 <Nationality/>
                                 <DeliveryTerms/>
                                 <TransactionNature/>
                                 <TransportMode/>
                                 <ProcessFlag/>
                                 <TaxExemptNumber/>
                                 <TaxExemptionStatus></TaxExemptionStatus>
                                 <GstExemptNumber/>
                                 <GstExemptionStatus></GstExemptionStatus>
                                 <CompanyTaxNumber/>
                                 <CancelReasonCode/>
                                 <DocumentFormat/>
                                 <State/>
                                 <CountyZip/>
                                 <City/>
                                 <eSignature></eSignature>
                             </OrderHeader>
                _header = header
            Catch ex As Exception
                BusinessObjectsHelper.AppendTrnMessage(_trnMessage, String.Format("CreateOrderMasterElement Exception:{0}{1}",
                                                     Environment.NewLine, msgHelper.GetFullMessage(ex)))
            End Try
            Return _header
        End Function

        Private Function CreateOrderDetails() As XElement()
            Dim elements(_detailData.Count - 1) As XElement
            For i As Integer = 0 To _detailData.Count - 1
                Select Case _detailData(i).LineActionType
                    Case "D"
                        'Cancel stock Line
                        elements(i) = CreateStockCancelLineElement(_detailData(i))
                    Case Else
                        elements(i) = CreateStockLineElement(_detailData(i))
                End Select

            Next
            Return elements
        End Function

        Private Function CreateStockLineElement(item As OrderDetailDataObject) As XElement
            Return <StockLine>
                       <CustomerPoLine><%= item.CustomerPoLine %></CustomerPoLine>
                       <LineActionType><%= item.LineActionType %></LineActionType>
                       <LineCancelCode/>
                       <StockCode><%= item.StockCode %></StockCode>
                       <StockDescription/>
                       <Warehouse><%= item.Warehouse %></Warehouse>
                       <CustomersPartNumber/>
                       <OrderQty><%= item.OrderQty %></OrderQty>
                       <OrderUom>EA</OrderUom>
                       <Price><%= item.Price %></Price>
                       <PriceUom>EA</PriceUom>
                       <PriceCode/>
                       <AlwaysUsePriceEntered/>
                       <Units/>
                       <Pieces/>
                       <ProductClass/>
                       <LineDiscPercent1>0</LineDiscPercent1>
                       <LineDiscPercent2>0</LineDiscPercent2>
                       <LineDiscPercent3>0</LineDiscPercent3>
                       <AlwaysUseDiscountEntered>N</AlwaysUseDiscountEntered>
                       <CommissionCode/>
                       <LineShipDate/>
                       <LineDiscValue>0</LineDiscValue>
                       <LineDiscValFlag/>
                       <OverrideCalculatedDiscount/>
                       <UserDefined/>
                       <NonStockedLine/>
                       <NsProductClass/>
                       <NsUnitCost/>
                       <UnitMass/>
                       <UnitVolume/>
                       <StockTaxCode/>
                       <StockNotTaxable/>
                       <StockFstCode/>
                       <StockNotFstTaxable/>
                       <AllocationAction>A</AllocationAction>
                       <ConfigPrintInv/>
                       <ConfigPrintDel/>
                       <ConfigPrintAck/>
                       <TariffCode/>
                       <LineMultiShipCode/>
                       <SupplementaryUnitsFactor/>
                       <ReserveStock>Y</ReserveStock>
                       <ReserveStockRequestAllocs>Y</ReserveStockRequestAllocs>
                   </StockLine>
        End Function

        Private Function CreateStockCancelLineElement(item As OrderDetailDataObject) As XElement
            Return <StockLine>
                       <CustomerPoLine><%= item.CustomerPoLine %></CustomerPoLine>
                       <LineActionType><%= item.LineActionType %></LineActionType>
                       <LineCancelCode>01</LineCancelCode>
                       <StockCode><%= item.StockCode %></StockCode>
                       <StockDescription/>
                       <Warehouse><%= item.Warehouse %></Warehouse>
                       <CustomersPartNumber/>
                       <OrderQty><%= item.OrderQty %></OrderQty>
                       <OrderUom>EA</OrderUom>
                       <Price><%= item.Price %></Price>
                       <PriceUom>EA</PriceUom>
                       <PriceCode/>
                       <AlwaysUsePriceEntered/>
                       <Units/>
                       <Pieces/>
                       <ProductClass/>
                       <LineDiscPercent1>0</LineDiscPercent1>
                       <LineDiscPercent2>0</LineDiscPercent2>
                       <LineDiscPercent3>0</LineDiscPercent3>
                       <AlwaysUseDiscountEntered>N</AlwaysUseDiscountEntered>
                       <CommissionCode/>
                       <LineShipDate/>
                       <LineDiscValue>0</LineDiscValue>
                       <LineDiscValFlag/>
                       <OverrideCalculatedDiscount/>
                       <UserDefined/>
                       <NonStockedLine/>
                       <NsProductClass/>
                       <NsUnitCost/>
                       <UnitMass/>
                       <UnitVolume/>
                       <StockTaxCode/>
                       <StockNotTaxable/>
                       <StockFstCode/>
                       <StockNotFstTaxable/>
                       <AllocationAction>A</AllocationAction>
                       <ConfigPrintInv/>
                       <ConfigPrintDel/>
                       <ConfigPrintAck/>
                       <TariffCode/>
                       <LineMultiShipCode/>
                       <SupplementaryUnitsFactor/>
                       <ReserveStock>Y</ReserveStock>
                       <ReserveStockRequestAllocs>Y</ReserveStockRequestAllocs>
                   </StockLine>
        End Function

        Public Function CreateXmlParams(validateOnly As Boolean) As String Implements IBusinessObjects.CreateXmlParams
            Dim xmlparams = <?xml version="1.0" encoding="Windows-1252"?>
                            <SalesOrders xmlns:xsd="http://www.w3.org/2001/XMLSchema-instance" xsd:noNamespaceSchemaLocation="SORTOI.XSD">
                                <Parameters>
                                    <InBoxMsgReqd>Y</InBoxMsgReqd>
                                    <Process>IMPORT</Process>
                                    <CustomerToUse/>
                                    <WarehouseListToUse/>
                                    <TypeOfOrder>ORD</TypeOfOrder>
                                    <OrderStatus>4</OrderStatus>
                                    <MinimumDaysToShip>4</MinimumDaysToShip>
                                    <AllowNonStockItems>Y</AllowNonStockItems>
                                    <AcceptOrdersIfNoCredit>Y</AcceptOrdersIfNoCredit>
                                    <AcceptEarlierShipDate>N</AcceptEarlierShipDate>
                                    <OperatorToInform>ADMIN</OperatorToInform>
                                    <CreditFailMessage>No credit available</CreditFailMessage>
                                    <ValidProductClassList/>
                                    <ShipFromDefaultBin>N</ShipFromDefaultBin>
                                    <AllowDuplicateOrderNumbers>Y</AllowDuplicateOrderNumbers>
                                    <CheckForCustomerPoNumbers>N</CheckForCustomerPoNumbers>
                                    <AllowInvoiceInformationEntry/>
                                    <AlwaysUsePriceEntered/>
                                    <AllowZeroPrice>Y</AllowZeroPrice>
                                    <AllowChangeToZeroPrice></AllowChangeToZeroPrice>
                                    <AddStockSalesOrderText>N</AddStockSalesOrderText>
                                    <AddDangerousGoodsText>N</AddDangerousGoodsText>
                                    <UseStockDescSupplied/>
                                    <ValidateShippingInstrs/>
                                    <AllocationAction>A</AllocationAction>
                                    <IgnoreWarnings>N</IgnoreWarnings>
                                    <AddAttachedServiceCharges></AddAttachedServiceCharges>
                                    <StatusInProcess></StatusInProcess>
                                    <WarnIfCustomerOnHold>N</WarnIfCustomerOnHold>
                                    <AcceptKitOptional>N</AcceptKitOptional>
                                    <AllowBackOrderForPartialHold></AllowBackOrderForPartialHold>
                                    <AllowBackOrderForSuperseded></AllowBackOrderForSuperseded>
                                    <OverrideCustomerBackOrder></OverrideCustomerBackOrder>
                                    <UseMasterAccountForCustomerPartNo></UseMasterAccountForCustomerPartNo>
                                    <ApplyLeadTimeCalculation></ApplyLeadTimeCalculation>
                                    <ApplyParentDiscountToComponents>N</ApplyParentDiscountToComponents>
                                    <AllowManualOrderNumberToBeUsed>N</AllowManualOrderNumberToBeUsed>
                                    <ReserveStock>Y</ReserveStock>
                                    <ReserveStockRequestAllocs>Y</ReserveStockRequestAllocs>
                                    <AllowBackOrderForNegativeMerchLine>N</AllowBackOrderForNegativeMerchLine>
                                </Parameters>
                            </SalesOrders>
            Return xmlparams.ToString
        End Function

        Public Function CreateXmlIn() As String Implements IBusinessObjects.CreateXmlIn
            Return CreateOrderMasterAndDetailXmlIn()
        End Function

        Public Function CreateOrderMasterChangeOnlyXmlIn() As String
            Dim xmlin = <?xml version="1.0" encoding="Windows-1252"?>
                        <SalesOrders xmlns:xsd="http://www.w3.org/2001/XMLSchema-instance" xsd:noNamespaceSchemaLocation="SORTOIDOC.XSD">
                            <%= CreateTransmissionHeaderElement() %>
                            <Orders>
                                <%= CreateOrderMasterElement() %>
                            </Orders>
                        </SalesOrders>
            Return xmlin.ToString
        End Function

        ''' <summary>
        ''' Create or maintain all item lines supplied
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' 
        Private Function CreateOrderMasterAndDetailXmlIn() As String
            Dim xmlin = <SalesOrders xmlns:xsd="http://www.w3.org/2001/XMLSchema-instance" xsd:noNamespaceSchemaLocation="SORTOIDOC.XSD">
                            <%= CreateTransmissionHeaderElement() %>
                            <Orders>
                                <%= CreateOrderMasterElement() %>
                                <OrderDetails>
                                    <%= CreateOrderDetails() %>
                                </OrderDetails>
                            </Orders>
                        </SalesOrders>
            Return xmlin.ToString
        End Function

        Public Function GetName() As String Implements IBusinessObjects.GetName
            Return "SORTOI"
        End Function

        Public Function ValidateInput() As Boolean Implements IBusinessObjects.ValidateInput
            'todo SORTOI validation module 
            Dim valid As Boolean = False


            Return valid
        End Function

        Public Function Execute(sigInfo As SysproSignInObj, salesorder As String, actiontype As String) As SysproPostXmlOutResult
            Dim postResult As SysproPostXmlOutResult = Nothing
            Dim xmlin As String = CreateXmlIn()
            Dim xmlParam As String = CreateXmlParams(False)
            'Dim P As New Post(sigInfo, "SORTOI", xmlin, xmlParam, actiontype, _soLines, _KitItems)
            Using p As New Post(sigInfo, "SORTOI", xmlin, xmlParam, actiontype, _soLines, _KitItems)
                'Process Post
                If p.Excecute() Then
                    'Read returned result
                    postResult = p.ConfirmXmlOut(salesorder)
                    AppendTrnMessage(p.TrnMessage)
                Else
                    AppendTrnMessage("Sales order creation failed.")
                    AppendTrnMessage(p.TrnMessage)
                End If
            End Using

            Return postResult
        End Function

        Private Function FillKitItems() As Boolean
            _KitItems = New List(Of Kit)
            For Each item In _detailData
                FillItemData(item.StockCode)
            Next
            Return True
        End Function

        Private Sub FillItemData(stockcode As String)
            Dim bll As New BLL.Query
            Dim bomstruct = bll.FillBomStructure(stockcode)
            If bomstruct IsNot Nothing Then
                For Each item In bomstruct
                    Dim comp = New Kit
                    With comp
                        .ComponentOf = stockcode
                        .ItemType = ComponetType.SubItem
                        .StockCode = item.Component
                    End With
                    _KitItems.Add(comp)
                Next
            End If
        End Sub
    End Class

End Class

