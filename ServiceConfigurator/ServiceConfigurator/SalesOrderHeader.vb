Imports System.Xml
Imports System.Xml.Serialization
<XmlRoot("Order",
     Namespace:="http://www.cpandl.com", IsNullable:=False)>
Public Class SalesOrderHeader
    Public Class OrderHeader
        Public Property SalesOrder As String
        Public Property PO As String
        Public Property Customer As String
        Public Property ActionType As String
        ' The XmlArrayAttribute changes the XML element name
        ' from the default of "??" to "OrderDetails". 
        <XmlArrayAttribute("OrderDetails")>
        Public Property StockLine As List(Of StockLine)
    End Class


End Class
