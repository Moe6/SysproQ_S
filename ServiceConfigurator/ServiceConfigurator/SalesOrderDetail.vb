Imports System.Xml
Imports System.Xml.Serialization

<Serializable()> Public Class StockLine
    Public Property PoLine As Integer
    Public Property StockCode As String
    Public Property LineAction As String
    Public Property Qty As Decimal
    Public Property Price As Decimal
    Public Property City As String
End Class
'Public Class SalesOrderDetail

'End Class