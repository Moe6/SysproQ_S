Partial Public Class SORTOI
    Public Class OrderHeaderDataObject
        'Public Sub New()
        '    Warehouse = "E"
        '    OrderType = "C"
        'End Sub

        Public Sub New()
            'OrderActionType = GetOrderActionType(Action)
            'SalesOrder = order.Quote 'IIf(order.SalesOrder IsNot Nothing, order.SalesOrder, Nothing)
            'CustomerPoNumber = GetCustomerPoFormatted(order.CustomerPO)
            'NewCustomerPoNumber = GetCustomerPoFormatted(order.NewPurchaseOrderNo)
            'Customer = order.ClientCode
            'OrderDate = GetOrderDateFormatted(order.GetOrderDate)
            'ShipAddress1 = order.DeliveryAddress1
            'ShipAddress2 = order.DeliveryAddress2
            'ShipAddress3 = order.DeliveryAddress3
            'ShipPostalCode = order.DeliveryPostCode
            'DeliveryCharge = order.DeliveryCharge
            'Warehouse = "E"
            'OrderType = "C"
        End Sub

        Public Property OrderActionType As String
        Public Property SalesOrder As String
        Public Property CustomerPoNumber As String
        Public Property NewCustomerPoNumber As String
        Public Property Customer As String
        Public Property OrderDate As String
        Public Property ShipAddress1 As String
        Public Property ShipAddress2 As String
        Public Property ShipAddress3 As String
        Public Property ShipPostalCode As String
        Public Property Warehouse As String
        Public Property OrderType As String
        Public Property DeliveryCharge As Decimal

        Private Function GetOrderDateFormatted(od As DateTime) As String
            Return od.ToString("yyyy-MM-dd")
        End Function

        'Private Function GetOrderActionType(action As SysproActionType) As String
        '    Select Case action
        '        Case SysproActionType.Add
        '            Return "A"
        '        Case SysproActionType.Change
        '            Return "C"
        '        Case SysproActionType.Cancel
        '            Return "D"
        '        Case Else
        '            Return ""
        '    End Select
        'End Function
    End Class
End Class
