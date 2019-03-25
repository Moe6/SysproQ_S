Imports System.Text
Imports System.Xml.Serialization
Imports System.IO
Imports System.Xml
Imports SysproQ.BLL
Imports SysproQ.Entity
Imports S.DAL

Partial Public Class Form1
    Inherits DevExpress.XtraBars.Ribbon.RibbonForm
    Private _OrderHeader As SalesOrderHeader.OrderHeader
    Private _orderDetails As List(Of StockLine)
    Private _trnMsg As String

    Shared Sub New()
        DevExpress.UserSkins.BonusSkins.Register()
        DevExpress.Skins.SkinManager.EnableFormSkins()
    End Sub

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub AppendTrnMessage(msg As String)
        If _trnmsg IsNot Nothing Then
            _trnMsg &= Environment.NewLine
        Else
            _trnMsg = msg
        End If

    End Sub

    Private Sub btnNew_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnNew.ItemClick
        _orderDetails = New List(Of StockLine)
        _OrderHeader = New SalesOrderHeader.OrderHeader
        With _OrderHeader
            .ActionType = "A"
            .Customer = "WERTY"
            .PO = "TEST"
            .SalesOrder = "700085"
        End With
        BindingSource1.DataSource = _OrderHeader
        BindingSource2.DataSource = _orderDetails
        VGridControl1.Refresh()
        GridView1.RefreshData()

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        GridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True
        GridView1.OptionsBehavior.ReadOnly = False
        GridView1.OptionsBehavior.Editable = True
    End Sub

    Private Sub addNewRowInGroupMode(ByVal View As DevExpress.XtraGrid.Views.Grid.GridView)
        'Get the handle of the source data row 
        'The row will provide group column values for a new row 
        Dim rowHandle As Integer = View.GetDataRowHandleByGroupRowHandle(View.FocusedRowHandle)
        'Store group column values 
        Dim groupValues() As Object = Nothing
        Dim groupColumnCount As Integer = View.GroupedColumns.Count
        Dim i As Integer
        If groupColumnCount > 0 Then
            ReDim groupValues(groupColumnCount - 1)
            For i = 0 To groupColumnCount - 1
                groupValues(i) = View.GetRowCellValue(rowHandle, View.GroupedColumns(i))
            Next
        End If
        'Add a new row 
        View.AddNewRow()
        'Get the handle of the new row 
        Dim newRowHandle As Integer = View.FocusedRowHandle
        Dim newRow As Object = View.GetRow(newRowHandle)
        'Set cell values corresponding to group columns 
        If groupColumnCount > 0 Then
            For i = 0 To groupColumnCount - 1
                View.SetRowCellValue(newRowHandle, View.GroupedColumns(i), groupValues(i))
            Next
        End If
        'Accept the new row 
        'The row moves to a new position according to the current group settings 
        View.UpdateCurrentRow()
        'Locate the new row 
        For i = 0 To View.DataRowCount
            If View.GetRow(i).Equals(newRow) Then
                View.FocusedRowHandle = i
                Exit For
            End If
        Next
    End Sub

    Private Sub AddNewRecords(ByRef obj As List(Of StockLine))
        Dim line As Integer = 1
        Dim price As Decimal = 60
        Dim qty As Decimal = 2
        Dim stockcode As String = "KIT100"
        Dim wh As String = "GABORONE"
        If obj IsNot Nothing Then
            If obj.Count = 1 Then
                Dim sl = obj.LastOrDefault
                line = sl.PoLine + 1
                price = sl.Price + 1
                qty = sl.Qty + 1
                stockcode = "KIT105"
                wh = "GABORONE"
            ElseIf obj.Count > 1 Then
                Dim sl = obj.LastOrDefault
                line = sl.PoLine + 1
                price = sl.Price + 1
                qty = sl.Qty + 1
                stockcode = "NEWCODE"
                wh = "GABORONE"
            End If
        Else
            obj = New List(Of StockLine)
        End If

        Dim ar As New StockLine With {.PoLine = line, .Qty = qty, .Price = price, .StockCode = stockcode, .City = wh, .LineAction = "A"}
        obj.Add(ar)
    End Sub
    Private Sub btnAddLine_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnAddLine.ItemClick
        Dim obj As New List(Of StockLine)
        If BindingSource2.DataSource IsNot Nothing Then
            obj = TryCast(BindingSource2.DataSource, List(Of StockLine))

        End If
        AddNewRecords(obj)
        BindingSource2.DataSource = obj
        GridView1.RefreshData()
        ' addNewRowInGroupMode(GridView1)
    End Sub

    Private Sub btnPost_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnPost.ItemClick
        Dim wcfService As New ServiceReference1.Service1Client
        Dim returnedMsg As String = ""
        Me.Validate()

        Dim strIn As String = ""
        _trnMsg = Nothing
        Try
            _OrderHeader = DirectCast(BindingSource1.Current, SalesOrderHeader.OrderHeader)
            _orderDetails = DirectCast(BindingSource2.DataSource, List(Of StockLine))
            _OrderHeader.StockLine = _orderDetails

            'serialize object here to xml for posting to web service
            'Serialize and send to text string
            strIn = SerializeToMemory(_OrderHeader, GetType(SalesOrderHeader.OrderHeader))
            'strIn = CreateXmlin(_OrderHeader, _orderDetails)
            'SerializeToFile(_OrderHeader)


            Using dal As New S.DAL.Query
                Dim q = dal.FillInvMaster("KIT100")
                If q IsNot Nothing Then
                    Console.WriteLine($"{q.StockCode}")
                End If
            End Using



            Dim bll As New SysproQ.BLL.Query
            Dim x = bll.FillInvMaster("KIT100")
            If x IsNot Nothing Then
                MsgBox(x.StockCode)
            End If
            'Do Post 
            returnedMsg = wcfService.Post2(strIn)

            AppendTrnMessage(returnedMsg)
            AppendTrnMessage(wcfService.TrnMessage)

            If _trnMsg IsNot Nothing Then
                MsgBox(_trnMsg)
            End If

        Catch ex As Exception
            MsgBox(GetFullMessage(ex), vbCritical)
        End Try

    End Sub

#Region "Xmlin build"
    Private Function CreateXmlin(hdr As SalesOrderHeader.OrderHeader, detail As List(Of StockLine)) As String
        'create hdr element
        'create detail element
        Dim xmlin = <?xml version="1.0" encoding="Windows-1252"?>
                    <Order>
                        <%= createHeaderElment(hdr) %>
                        <OrderDetails><%= CreateDetailElement(detail) %></OrderDetails>
                    </Order>

        'serialise xml
        Dim fname As String = "C:\Temp\dd.xml"

        Dim serializer As New XmlSerializer(GetType(XDocument))
        Dim writer As New StreamWriter(fname)
        serializer.Serialize(writer, xmlin)
        Return xmlin.ToString
    End Function

    Private Function createHeaderElment(hdr As SalesOrderHeader.OrderHeader) As XElement
        Return <OrderHeader>
                   <SalesOrder><%= hdr.SalesOrder %></SalesOrder>
                   <CustomerPoNumber><%= hdr.PO %></CustomerPoNumber>
                   <Customer><%= hdr.Customer %></Customer>
                   <OrderActionType><%= hdr.ActionType %></OrderActionType>
               </OrderHeader>
    End Function
    Private Function CreateDetailElement(details As List(Of StockLine)) As XElement()
        Dim xe(details.Count) As XElement
        Dim count As Integer = 0
        For Each item In details
            xe(count) = CreateDetailsLine(item)
            count += 1
        Next
        Return xe

    End Function
    Private Function CreateDetailsLine(detail As StockLine) As XElement
        Return <StockLine>
                   <CustomerPoLine><%= detail.PoLine %></CustomerPoLine>
                   <StockCode><%= detail.StockCode %></StockCode>
                   <OrderQty><%= detail.Qty %></OrderQty>
                   <Price><%= detail.Price %></Price>
                   <LineAction><%= detail.LineAction %></LineAction>
                   <Warehouse><%= detail.City %></Warehouse>
               </StockLine>
    End Function
#End Region

#Region "Xml Serialization Deserialization"
    Private Sub SerializeToFile(obj As SalesOrderHeader.OrderHeader)
        Dim serializer As New XmlSerializer(GetType(SalesOrderHeader.OrderHeader))
        Dim writer As New StreamWriter(Application.LocalUserAppDataPath & "so.xml")
        Try
            serializer.Serialize(writer, obj)
        Catch ex As Exception
            MsgBox(GetFullMessage(ex), vbCritical)
        Finally
            writer.Close()
        End Try

    End Sub

    Private Function SerializeToMemory(ByVal Obj As Object, ByVal ObjType As System.Type) As String
        Dim serializer As New XmlSerializer(ObjType)
        Dim memStream As MemoryStream
        memStream = New MemoryStream()
        Dim Writer As XmlTextWriter
        Writer = New XmlTextWriter(memStream, Encoding.UTF8)
        Writer.Namespaces = True

        Try
            serializer.Serialize(Writer, Obj)
        Catch ex As Exception
            MsgBox(ex.Message, vbCritical)
        Finally
            Writer.Close()
            memStream.Close()
        End Try
        Dim xml As String
        xml = Encoding.UTF8.GetString(memStream.GetBuffer())
        xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)))
        xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1))
        Return xml
    End Function

    Public Shared Function FromXml(ByVal Xml As String, ByVal ObjType As System.Type) As Object
        Dim ser As XmlSerializer
        ser = New XmlSerializer(ObjType)
        Dim stringReader As StringReader
        stringReader = New StringReader(Xml)
        Dim xmlReader As XmlTextReader
        xmlReader = New XmlTextReader(stringReader)
        Dim obj As Object
        obj = ser.Deserialize(xmlReader)
        xmlReader.Close()
        stringReader.Close()
        Return obj
    End Function

    'Private Function ReadSerialisedFile(filename As String) As String
    '    ' Creates an instance of the XmlSerializer class;
    '    ' specifies the type of object to be deserialized.
    '    Dim serializer As New XmlSerializer(GetType(SalesOrderHeader))
    '    ' If the XML document has been altered with unknown
    '    ' nodes or attributes, handles them with the
    '    ' UnknownNode and UnknownAttribute events.
    '    AddHandler serializer.UnknownNode, AddressOf serializer_UnknownNode
    '    AddHandler serializer.UnknownAttribute, AddressOf _
    '    serializer_UnknownAttribute

    '    ' A FileStream is needed to read the XML document.
    '    Dim fs As New FileStream(filename, FileMode.Open)
    '    ' Declare an object variable of the type to be deserialized.
    '    Dim so As SalesOrderHeader
    '    ' Uses the Deserialize method to restore the object's state 
    '    ' with data from the XML document. 
    '    so = CType(serializer.Deserialize(fs), SalesOrderHeader)

    'End Function

    Protected Sub serializer_UnknownNode(sender As Object, e As _
   XmlNodeEventArgs)
        Console.WriteLine(("Unknown Node:" & e.Name &
        ControlChars.Tab & e.Text))
    End Sub

    Protected Sub serializer_UnknownAttribute(sender As Object,
    e As XmlElementEventArgs)
        Dim attr As System.Xml.XmlElement = e.Element
        Console.WriteLine(("Unknown attribute " & attr.Name & "='" &
        attr.Value & "'"))
    End Sub 'serializer_UnknownAttribute
    Protected Sub serializer_UnknownAttribute(sender As Object,
     e As XmlAttributeEventArgs)
        Dim attr As System.Xml.XmlAttribute = e.Attr
        Console.WriteLine(("Unknown attribute " & attr.Name & "='" &
        attr.Value & "'"))
    End Sub 'serializer_UnknownAttribute
#End Region

    Private Function GetFullMessage(ex As Exception) As String
        Dim strBuild As New System.Text.StringBuilder
        If ex IsNot Nothing Then
            strBuild.Append("Message:" & Environment.NewLine & ex.Message & Environment.NewLine & Environment.NewLine)
            'Get Inner Exception
            If ex.InnerException IsNot Nothing Then
                If ex.InnerException.Message IsNot Nothing Then
                    strBuild.Append("Inner Exception:" & Environment.NewLine & ex.InnerException.Message _
                                    & Environment.NewLine & Environment.NewLine)
                End If
                If ex.InnerException.StackTrace IsNot Nothing Then
                    strBuild.Append("Inner Exception Stack:" & Environment.NewLine & ex.InnerException.StackTrace _
                                    & Environment.NewLine & Environment.NewLine)
                ElseIf ex.StackTrace IsNot Nothing Then
                    strBuild.Append("Initial Stack Trace:" & Environment.NewLine & ex.StackTrace)
                End If
                'Get main stack trace
            ElseIf ex.StackTrace IsNot Nothing Then
                strBuild.Append("Initial Stack Trace:" & Environment.NewLine & ex.StackTrace)

            End If
        End If

        Return strBuild.ToString
    End Function

    Private Sub BarButtonItem1_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        Dim obj As New List(Of StockLine)
        If BindingSource2.DataSource IsNot Nothing Then
            obj = TryCast(BindingSource2.DataSource, List(Of StockLine))
            If GridView1.FocusedRowHandle > -1 Then
                obj.RemoveAt(GridView1.FocusedRowHandle)
                BindingSource2.DataSource = obj
                GridView1.RefreshData()
            End If

        End If
    End Sub

    Private Sub BarHeaderItem1_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarHeaderItem1.ItemClick

    End Sub
End Class
