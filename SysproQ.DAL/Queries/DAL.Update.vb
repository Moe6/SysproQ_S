Imports System.Data.Entity
Imports SysproQ.Entity
Imports System.Data.SqlClient
Imports SysproQ.Entity.Enums
Public Class Update
    Implements IDisposable

    Private _db As SysproEntities
    Private _recordCount As Integer
    Private _trnMessage As String = ""

    Public Sub New()
        _db = New SysproEntities
    End Sub

    Public ReadOnly Property TrnMessage As String
        Get
            Return _trnMessage
        End Get
    End Property

    Private Sub AppendMessage(msg As String)
        If _trnMessage IsNot Nothing Then
            _trnMessage &= Environment.NewLine
        End If
        _trnMessage &= msg
    End Sub

    Public Sub Update(obj As ArCustomer)
        If _db.ArCustomers.Any(Function(c) c.Customer = obj.Customer) Then
            SetObjectModified(obj)
        Else
            SetObjectAdded(obj)
        End If
    End Sub

    Public Function Save() As Boolean
        Dim trnSuccess As Integer
        Try
            trnSuccess = _db.SaveChanges
            If Not trnSuccess > 0 Then
                Dim er = _db.GetValidationErrors
                If er IsNot Nothing Then
                    For Each item In er
                        AppendMessage(item.ValidationErrors.ToString())
                    Next
                End If
            End If
        Catch ex As Exception
            Dim H As New MessagingHelper
            Dim M = H.GetFullMessage(ex)
            AppendMessage(M)
        End Try
        Return trnSuccess > 0
    End Function

    Private Sub SetObjectModified(Of T)(obj As T)
        _db.Entry(obj).State = EntityState.Modified
        _recordCount += 1
    End Sub

    Private Sub SetObjectAdded(Of T As Class)(newItem As T)
        _db.[Set](Of T)().Add(newItem)
        _recordCount += 1
    End Sub

    Public Function UpdateSQL(strSQL As String) As Boolean
        Dim con = New SqlConnection("data source=.;initial catalog=SysproCompanyC;persist security info=False;user id=sa;password=P@$$w0rd")
        Dim cmd As New SqlCommand
        Dim adp As New SqlDataAdapter
        Dim rowsupdated As Integer
        Try
            cmd.Connection = con
            con.Open()
            cmd.CommandText = strSQL
            rowsupdated = cmd.ExecuteNonQuery()
        Catch ex As Exception
            Dim H As New MessagingHelper
            Dim M = H.GetFullMessage(ex)
            AppendMessage(M)
        Finally
            con.Close()
        End Try
        Return rowsupdated = 2
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
