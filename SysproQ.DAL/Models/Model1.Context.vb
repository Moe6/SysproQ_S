﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated from a template.
'
'     Manual changes to this file may cause unexpected behavior in your application.
'     Manual changes to this file will be overwritten if the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports SysproQ.Entity

Partial Public Class SysproEntities
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=SysproEntities")
        Dim ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance
    End Sub

    Protected Overrides Sub OnModelCreating(modelBuilder As DbModelBuilder)
        Throw New UnintentionalCodeFirstException()
    End Sub

    Public Overridable Property ArCustomers() As DbSet(Of ArCustomer)
    Public Overridable Property BomStructures() As DbSet(Of BomStructure)
    Public Overridable Property SorDetails() As DbSet(Of SorDetail)
    Public Overridable Property SorMasters() As DbSet(Of SorMaster)
    Public Overridable Property InvMasters() As DbSet(Of InvMaster)
    Public Overridable Property InvWarehouses() As DbSet(Of InvWarehouse)

End Class
