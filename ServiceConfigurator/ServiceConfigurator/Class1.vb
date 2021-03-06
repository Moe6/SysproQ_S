﻿Imports System
Imports System.Xml
Imports System.Xml.Serialization
Imports System.IO
Imports Microsoft.VisualBasic
Public Class Class1
    ' The XmlRootAttribute allows you to set an alternate name
    ' (PurchaseOrder) for the XML element and its namespace. By
    ' default, the XmlSerializer uses the class name. The attribute
    ' also allows you to set the XML namespace for the element. Lastly,
    ' the attribute sets the IsNullable property, which specifies whether
    ' the xsi:null attribute appears if the class instance is set to
    ' a null reference. 
    <XmlRootAttribute("PurchaseOrder",
     Namespace:="http://www.cpandl.com", IsNullable:=False)>
    Public Class PurchaseOrder
        Public ShipTo As Address
        Public OrderDate As String
        ' The XmlArrayAttribute changes the XML element name
        ' from the default of "OrderedItems" to "Items". 
        <XmlArrayAttribute("Items")>
        Public OrderedItems() As OrderedItem
        Public SubTotal As Decimal
        Public ShipCost As Decimal
        Public TotalCost As Decimal
    End Class

    Public Class Address
        ' The XmlAttribute instructs the XmlSerializer to serialize the 
        ' Name field as an XML attribute instead of an XML element (the 
        ' default behavior). 
        <XmlAttribute()>
        Public Name As String
        Public Line1 As String

        ' Setting the IsNullable property to false instructs the
        ' XmlSerializer that the XML attribute will not appear if
        ' the City field is set to a null reference. 
        <XmlElementAttribute(IsNullable:=False)>
        Public City As String
        Public State As String
        Public Zip As String
    End Class

    Public Class OrderedItem
        Public ItemName As String
        Public Description As String
        Public UnitPrice As Decimal
        Public Quantity As Integer
        Public LineTotal As Decimal

        ' Calculate is a custom method that calculates the price per item
        ' and stores the value in a field. 
        Public Sub Calculate()
            LineTotal = UnitPrice * Quantity
        End Sub
    End Class

    Public Class Test
        Public Shared Sub Main()
            ' Read and write purchase orders.
            Dim t As New Test()
            t.CreatePO("po.xml")
            t.ReadPO("po.xml")
        End Sub

        Private Sub CreatePO(filename As String)
            ' Creates an instance of the XmlSerializer class;
            ' specifies the type of object to serialize.
            Dim serializer As New XmlSerializer(GetType(PurchaseOrder))
            Dim writer As New StreamWriter(filename)
            Dim po As New PurchaseOrder()

            ' Creates an address to ship and bill to.
            Dim billAddress As New Address()
            billAddress.Name = "Teresa Atkinson"
            billAddress.Line1 = "1 Main St."
            billAddress.City = "AnyTown"
            billAddress.State = "WA"
            billAddress.Zip = "00000"
            ' Set ShipTo and BillTo to the same addressee.
            po.ShipTo = billAddress
            po.OrderDate = System.DateTime.Now.ToLongDateString()

            ' Creates an OrderedItem.
            Dim i1 As New OrderedItem()
            i1.ItemName = "Widget S"
            i1.Description = "Small widget"
            i1.UnitPrice = CDec(5.23)
            i1.Quantity = 3
            i1.Calculate()

            ' Inserts the item into the array.
            Dim items(0) As OrderedItem
            items(0) = i1
            po.OrderedItems = items
            ' Calculates the total cost.
            Dim subTotal As New Decimal()
            Dim oi As OrderedItem
            For Each oi In items
                subTotal += oi.LineTotal
            Next oi
            po.SubTotal = subTotal
            po.ShipCost = CDec(12.51)
            po.TotalCost = po.SubTotal + po.ShipCost
            ' Serializes the purchase order, and close the TextWriter.
            serializer.Serialize(writer, po)
            writer.Close()
        End Sub

        Protected Sub ReadPO(filename As String)
            ' Creates an instance of the XmlSerializer class;
            ' specifies the type of object to be deserialized.
            Dim serializer As New XmlSerializer(GetType(PurchaseOrder))
            ' If the XML document has been altered with unknown
            ' nodes or attributes, handles them with the
            ' UnknownNode and UnknownAttribute events.
            AddHandler serializer.UnknownNode, AddressOf serializer_UnknownNode
            AddHandler serializer.UnknownAttribute, AddressOf _
            serializer_UnknownAttribute

            ' A FileStream is needed to read the XML document.
            Dim fs As New FileStream(filename, FileMode.Open)
            ' Declare an object variable of the type to be deserialized.
            Dim po As PurchaseOrder
            ' Uses the Deserialize method to restore the object's state 
            ' with data from the XML document. 
            po = CType(serializer.Deserialize(fs), PurchaseOrder)
            ' Reads the order date.
            Console.WriteLine(("OrderDate: " & po.OrderDate))

            ' Reads the shipping address.
            Dim shipTo As Address = po.ShipTo
            ReadAddress(shipTo, "Ship To:")
            ' Reads the list of ordered items.
            Dim items As OrderedItem() = po.OrderedItems
            Console.WriteLine("Items to be shipped:")
            Dim oi As OrderedItem
            For Each oi In items
                Console.WriteLine((ControlChars.Tab & oi.ItemName &
                ControlChars.Tab &
                    oi.Description & ControlChars.Tab & oi.UnitPrice &
                    ControlChars.Tab &
                    oi.Quantity & ControlChars.Tab & oi.LineTotal))
            Next oi
            ' Reads the subtotal, shipping cost, and total cost.
            Console.WriteLine((ControlChars.Cr & New String _
            (ControlChars.Tab, 5) &
            " Subtotal" & ControlChars.Tab & po.SubTotal & ControlChars.Cr &
            New String(ControlChars.Tab, 5) & " Shipping" & ControlChars.Tab &
            po.ShipCost & ControlChars.Cr & New String(ControlChars.Tab, 5) &
            " Total" & New String(ControlChars.Tab, 2) & po.TotalCost))
        End Sub

        Protected Sub ReadAddress(a As Address, label As String)
            ' Reads the fields of the Address.
            Console.WriteLine(label)
            Console.Write((ControlChars.Tab & a.Name & ControlChars.Cr &
            ControlChars.Tab & a.Line1 & ControlChars.Cr & ControlChars.Tab &
            a.City & ControlChars.Tab & a.State & ControlChars.Cr &
            ControlChars.Tab & a.Zip & ControlChars.Cr))
        End Sub

        Protected Sub serializer_UnknownNode(sender As Object, e As _
        XmlNodeEventArgs)
            Console.WriteLine(("Unknown Node:" & e.Name &
            ControlChars.Tab & e.Text))
        End Sub

        Protected Sub serializer_UnknownAttribute(sender As Object,
        e As XmlAttributeEventArgs)
            Dim attr As System.Xml.XmlAttribute = e.Attr
            Console.WriteLine(("Unknown attribute " & attr.Name & "='" &
            attr.Value & "'"))
        End Sub 'serializer_UnknownAttribute
    End Class 'Test
End Class
