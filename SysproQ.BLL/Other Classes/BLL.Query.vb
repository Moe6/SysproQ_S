Imports SysproQ.Entity

Public Class Query
    Public Sub New()

    End Sub

    Public Function FillSorDetails(so As String) As List(Of SorDetail)
        Using dal As New DAL.Query
            Return dal.FillSalesOrderDetails(so)
        End Using
    End Function
    Public Function FillSorMaster(so As String) As SorMaster
        Using dal As New DAL.Query
            Return dal.FillOrderMaster(so)
        End Using
    End Function
    Public Function FillWarehouseByCity(city As String) As InvWhLookUp
        Using d As New DAL.Query
            Return d.FillWarehouseByCity(city)
        End Using
    End Function

    Public Function FillBomStructure(stockcode As String) As List(Of BomStructure)
        Using d As New DAL.Query
            Return d.FillComponents(stockcode)
        End Using
    End Function

    Public Function FillInvMaster(stockCode As String) As InvMaster
        Using d As New DAL.Query
            Return d.FillInvMaster(stockCode)
        End Using
    End Function

    Public Function FillInvWarehouse(stockcode As String, wh As String) As InvWarehouse
        Using d As New DAL.Query
            Return d.FillInveWarehouse(stockcode, wh)
        End Using
    End Function
End Class

