Imports SysproQ.Entity
Imports SysproQ.DAL
Public Class Update
    Public Sub Update(obj As ArCustomer)
        Using dal As New DAL.Update
            dal.Update(obj)
        End Using
    End Sub
    Public Function Save() As Boolean
        Using dal As New DAL.Update
            Return dal.Save
        End Using
    End Function
End Class
