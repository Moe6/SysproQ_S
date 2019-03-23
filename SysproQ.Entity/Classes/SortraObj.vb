Public Class SortraObj
    Public Property SalesOrder As String
    Public Property Line As Integer
    'Public Property Customer As String
    Public Property StockCode As String
    'Public Property Warehouse As String
    Public Property Qty As Decimal
    ' Public Property Lot As Decimal
End Class

Public Class SoLines
    Public Property SalesOrder As String
    Public Property Warehouse As String
    Public Property PoLine As String
    Public Property StockCode As String
    Public Property LineAction As String
    Public Property virtualLine As Integer
End Class