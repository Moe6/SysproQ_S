Imports SysproQ.Entity
Imports System.Data.SqlClient
Imports System.Data.Entity

Public Class Query
    Implements IDisposable
    Private _trnmsg As String

    Public ReadOnly Property TrnMessage As String
        Get
            Return _trnmsg
        End Get
    End Property

    Private Sub AppendTrnMessage(msg As String)
        If _trnmsg IsNot Nothing Then
            _trnmsg &= Environment.NewLine
        End If
        _trnmsg &= msg
    End Sub

    Private _db As SysproEntities

    Public Sub New()

        _db = New SysproEntities
    End Sub

    Public Function FillSalesOrderDetails(so As String) As List(Of SorDetail)
        'Return _db.SorDetails.Where(Function(c) c.SalesOrder.EndsWith(so)).ToList
        Return _db.SorDetails.Where(Function(c) c.SalesOrder = so).ToList
    End Function
    Public Function FillOrderMaster(so As String) As SorMaster
        'Return _db.SorMasters.Where(Function(c) c.SalesOrder.EndsWith(so)).FirstOrDefault
        Return _db.SorMasters.Where(Function(c) c.SalesOrder = so).FirstOrDefault
    End Function

    Public Function FillWarehouseByCity(city As String) As InvWhLookUp
        Using _db2 As New SysproQEntities
            Return _db2.InvWhLookUps.Where(Function(c) c.City = city).FirstOrDefault
        End Using
    End Function
    Public Function FillCustomer(cust As String) As ArCustomer
        Return _db.ArCustomers.Where(Function(c) c.Customer = cust).FirstOrDefault
    End Function

    Public Function GetCustomerSQl(cust As String) As Boolean
        Dim con = New SqlConnection("data source=.;initial catalog=SysproCompanyC;persist security info=False;user id=sa;password=P@$$w0rd")
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader
        Try
            cmd.Connection = con
            con.Open()
            cmd.CommandText = "Select * from ArCustomer Where Customer = '" & cust & "'"
            reader = cmd.ExecuteReader
            If reader.HasRows Then Return True

        Catch ex As Exception
            Dim H As New MessagingHelper
            Dim M = H.GetFullMessage(ex)
            AppendTrnMessage(M)
        Finally
            con.Close()
        End Try
        Return False
    End Function

    Public Function FillInvMaster(StockCode As String) As InvMaster
        Return _db.InvMasters.Where(Function(c) c.StockCode = StockCode).FirstOrDefault
    End Function

    Public Function FillComponents(StockCode As String) As List(Of BomStructure)
        Return _db.BomStructures.Where(Function(c) c.ParentPart = StockCode).ToList
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

End Class
