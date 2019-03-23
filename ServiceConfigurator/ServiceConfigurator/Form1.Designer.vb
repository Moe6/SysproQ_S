Partial Public Class Form1
    ''' <summary>
    ''' Required designer variable.
    ''' </summary>
    Private components As System.ComponentModel.IContainer = Nothing

    ''' <summary>
    ''' Clean up any resources being used.
    ''' </summary>
    ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso (components IsNot Nothing) Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

#Region "Windows Form Designer generated code"

    ''' <summary>
    ''' Required method for Designer support - do not modify
    ''' the contents of this method with the code editor.
    ''' </summary>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.ribbonControl1 = New DevExpress.XtraBars.Ribbon.RibbonControl()
        Me.btnNew = New DevExpress.XtraBars.BarButtonItem()
        Me.btnAddLine = New DevExpress.XtraBars.BarButtonItem()
        Me.btnPost = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem1 = New DevExpress.XtraBars.BarButtonItem()
        Me.ribbonPage1 = New DevExpress.XtraBars.Ribbon.RibbonPage()
        Me.ribbonPageGroup1 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.RibbonPageGroup3 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.VGridControl1 = New DevExpress.XtraVerticalGrid.VGridControl()
        Me.BindingSource1 = New System.Windows.Forms.BindingSource(Me.components)
        Me.rowSalesOrder = New DevExpress.XtraVerticalGrid.Rows.EditorRow()
        Me.rowPO1 = New DevExpress.XtraVerticalGrid.Rows.EditorRow()
        Me.rowCustomer1 = New DevExpress.XtraVerticalGrid.Rows.EditorRow()
        Me.rowActionType = New DevExpress.XtraVerticalGrid.Rows.EditorRow()
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl()
        Me.BindingSource2 = New System.Windows.Forms.BindingSource(Me.components)
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.colPoLine = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colWarehouse = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colStockCode = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colLineAction = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colQty = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPrice = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.LayoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        Me.rowPO = New DevExpress.XtraVerticalGrid.Rows.EditorRow()
        Me.rowCustomer = New DevExpress.XtraVerticalGrid.Rows.EditorRow()
        Me.BarHeaderItem1 = New DevExpress.XtraBars.BarHeaderItem()
        CType(Me.ribbonControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.VGridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSource1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSource2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ribbonControl1
        '
        Me.ribbonControl1.ExpandCollapseItem.Id = 0
        Me.ribbonControl1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.ribbonControl1.ExpandCollapseItem, Me.btnNew, Me.btnAddLine, Me.btnPost, Me.BarButtonItem1, Me.BarHeaderItem1})
        Me.ribbonControl1.Location = New System.Drawing.Point(0, 0)
        Me.ribbonControl1.MaxItemId = 6
        Me.ribbonControl1.Name = "ribbonControl1"
        Me.ribbonControl1.Pages.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPage() {Me.ribbonPage1})
        Me.ribbonControl1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2010
        Me.ribbonControl1.Size = New System.Drawing.Size(974, 143)
        '
        'btnNew
        '
        Me.btnNew.Caption = "Create New Order"
        Me.btnNew.Id = 1
        Me.btnNew.ImageOptions.Image = CType(resources.GetObject("btnNew.ImageOptions.Image"), System.Drawing.Image)
        Me.btnNew.ImageOptions.LargeImage = CType(resources.GetObject("btnNew.ImageOptions.LargeImage"), System.Drawing.Image)
        Me.btnNew.Name = "btnNew"
        '
        'btnAddLine
        '
        Me.btnAddLine.Caption = "Add New Line"
        Me.btnAddLine.Id = 2
        Me.btnAddLine.ImageOptions.Image = CType(resources.GetObject("btnAddLine.ImageOptions.Image"), System.Drawing.Image)
        Me.btnAddLine.ImageOptions.LargeImage = CType(resources.GetObject("btnAddLine.ImageOptions.LargeImage"), System.Drawing.Image)
        Me.btnAddLine.Name = "btnAddLine"
        '
        'btnPost
        '
        Me.btnPost.Caption = "Post"
        Me.btnPost.Id = 3
        Me.btnPost.ImageOptions.Image = CType(resources.GetObject("btnPost.ImageOptions.Image"), System.Drawing.Image)
        Me.btnPost.ImageOptions.LargeImage = CType(resources.GetObject("btnPost.ImageOptions.LargeImage"), System.Drawing.Image)
        Me.btnPost.Name = "btnPost"
        '
        'BarButtonItem1
        '
        Me.BarButtonItem1.Caption = "Delete Line"
        Me.BarButtonItem1.Id = 4
        Me.BarButtonItem1.ImageOptions.Image = CType(resources.GetObject("BarButtonItem1.ImageOptions.Image"), System.Drawing.Image)
        Me.BarButtonItem1.ImageOptions.LargeImage = CType(resources.GetObject("BarButtonItem1.ImageOptions.LargeImage"), System.Drawing.Image)
        Me.BarButtonItem1.Name = "BarButtonItem1"
        '
        'ribbonPage1
        '
        Me.ribbonPage1.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.ribbonPageGroup1, Me.RibbonPageGroup3})
        Me.ribbonPage1.Name = "ribbonPage1"
        '
        'ribbonPageGroup1
        '
        Me.ribbonPageGroup1.ItemLinks.Add(Me.BarHeaderItem1)
        Me.ribbonPageGroup1.ItemLinks.Add(Me.btnNew)
        Me.ribbonPageGroup1.ItemLinks.Add(Me.BarHeaderItem1)
        Me.ribbonPageGroup1.ItemLinks.Add(Me.btnPost)
        Me.ribbonPageGroup1.ItemLinks.Add(Me.BarHeaderItem1)
        Me.ribbonPageGroup1.Name = "ribbonPageGroup1"
        '
        'RibbonPageGroup3
        '
        Me.RibbonPageGroup3.ItemLinks.Add(Me.BarHeaderItem1)
        Me.RibbonPageGroup3.ItemLinks.Add(Me.BarButtonItem1)
        Me.RibbonPageGroup3.ItemLinks.Add(Me.BarHeaderItem1)
        Me.RibbonPageGroup3.ItemLinks.Add(Me.btnAddLine)
        Me.RibbonPageGroup3.ItemLinks.Add(Me.BarHeaderItem1)
        Me.RibbonPageGroup3.Name = "RibbonPageGroup3"
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.VGridControl1)
        Me.LayoutControl1.Controls.Add(Me.GridControl1)
        Me.LayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutControl1.Location = New System.Drawing.Point(2, 2)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        Me.LayoutControl1.Size = New System.Drawing.Size(970, 427)
        Me.LayoutControl1.TabIndex = 1
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'VGridControl1
        '
        Me.VGridControl1.DataSource = Me.BindingSource1
        Me.VGridControl1.Location = New System.Drawing.Point(12, 12)
        Me.VGridControl1.Name = "VGridControl1"
        Me.VGridControl1.Rows.AddRange(New DevExpress.XtraVerticalGrid.Rows.BaseRow() {Me.rowSalesOrder, Me.rowPO1, Me.rowCustomer1, Me.rowActionType})
        Me.VGridControl1.Size = New System.Drawing.Size(946, 124)
        Me.VGridControl1.TabIndex = 5
        '
        'rowSalesOrder
        '
        Me.rowSalesOrder.Name = "rowSalesOrder"
        Me.rowSalesOrder.Properties.Caption = "Sales Order"
        Me.rowSalesOrder.Properties.FieldName = "SalesOrder"
        '
        'rowPO1
        '
        Me.rowPO1.Name = "rowPO1"
        Me.rowPO1.Properties.Caption = "PO"
        Me.rowPO1.Properties.FieldName = "PO"
        '
        'rowCustomer1
        '
        Me.rowCustomer1.Name = "rowCustomer1"
        Me.rowCustomer1.Properties.Caption = "Customer"
        Me.rowCustomer1.Properties.FieldName = "Customer"
        '
        'rowActionType
        '
        Me.rowActionType.Name = "rowActionType"
        Me.rowActionType.Properties.Caption = "Action Type"
        Me.rowActionType.Properties.FieldName = "ActionType"
        '
        'GridControl1
        '
        Me.GridControl1.DataSource = Me.BindingSource2
        Me.GridControl1.Location = New System.Drawing.Point(12, 140)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.MenuManager = Me.ribbonControl1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.Size = New System.Drawing.Size(946, 275)
        Me.GridControl1.TabIndex = 4
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'BindingSource2
        '
        Me.BindingSource2.DataSource = GetType(ServiceConfigurator.StockLine)
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colPoLine, Me.colWarehouse, Me.colStockCode, Me.colLineAction, Me.colQty, Me.colPrice})
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.[True]
        '
        'colPoLine
        '
        Me.colPoLine.FieldName = "PoLine"
        Me.colPoLine.Name = "colPoLine"
        Me.colPoLine.Visible = True
        Me.colPoLine.VisibleIndex = 1
        '
        'colWarehouse
        '
        Me.colWarehouse.Caption = "Warehouse"
        Me.colWarehouse.FieldName = "City"
        Me.colWarehouse.Name = "colWarehouse"
        Me.colWarehouse.Visible = True
        Me.colWarehouse.VisibleIndex = 0
        '
        'colStockCode
        '
        Me.colStockCode.FieldName = "StockCode"
        Me.colStockCode.Name = "colStockCode"
        Me.colStockCode.Visible = True
        Me.colStockCode.VisibleIndex = 3
        '
        'colLineAction
        '
        Me.colLineAction.Caption = "Line Action"
        Me.colLineAction.FieldName = "LineAction"
        Me.colLineAction.Name = "colLineAction"
        Me.colLineAction.Visible = True
        Me.colLineAction.VisibleIndex = 2
        '
        'colQty
        '
        Me.colQty.FieldName = "Qty"
        Me.colQty.Name = "colQty"
        Me.colQty.Visible = True
        Me.colQty.VisibleIndex = 4
        '
        'colPrice
        '
        Me.colPrice.FieldName = "Price"
        Me.colPrice.Name = "colPrice"
        Me.colPrice.Visible = True
        Me.colPrice.VisibleIndex = 5
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.CustomizationFormText = "LayoutControlGroup1"
        Me.LayoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.LayoutControlGroup1.GroupBordersVisible = False
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2})
        Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(970, 427)
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.GridControl1
        Me.LayoutControlItem1.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 128)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Size = New System.Drawing.Size(950, 279)
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextVisible = False
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.VGridControl1
        Me.LayoutControlItem2.CustomizationFormText = "LayoutControlItem2"
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.Size = New System.Drawing.Size(950, 128)
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextVisible = False
        '
        'PanelControl1
        '
        Me.PanelControl1.Controls.Add(Me.LayoutControl1)
        Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControl1.Location = New System.Drawing.Point(0, 143)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(974, 431)
        Me.PanelControl1.TabIndex = 2
        '
        'rowPO
        '
        Me.rowPO.Name = "rowPO"
        Me.rowPO.Properties.Caption = "PO"
        Me.rowPO.Properties.FieldName = "PO"
        '
        'rowCustomer
        '
        Me.rowCustomer.Name = "rowCustomer"
        Me.rowCustomer.Properties.Caption = "Customer"
        Me.rowCustomer.Properties.FieldName = "Customer"
        '
        'BarHeaderItem1
        '
        Me.BarHeaderItem1.Caption = "        "
        Me.BarHeaderItem1.Id = 5
        Me.BarHeaderItem1.Name = "BarHeaderItem1"
        '
        'Form1
        '
        Me.AllowFormGlass = DevExpress.Utils.DefaultBoolean.[False]
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(974, 574)
        Me.Controls.Add(Me.PanelControl1)
        Me.Controls.Add(Me.ribbonControl1)
        Me.Name = "Form1"
        Me.Ribbon = Me.ribbonControl1
        Me.Text = "Post Sales Order"
        CType(Me.ribbonControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.VGridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSource1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSource2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private ribbonControl1 As DevExpress.XtraBars.Ribbon.RibbonControl
    Private ribbonPage1 As DevExpress.XtraBars.Ribbon.RibbonPage
    Private ribbonPageGroup1 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents VGridControl1 As DevExpress.XtraVerticalGrid.VGridControl
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents LayoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents btnNew As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents btnAddLine As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BindingSource2 As BindingSource
    Friend WithEvents rowPO As DevExpress.XtraVerticalGrid.Rows.EditorRow
    Friend WithEvents rowCustomer As DevExpress.XtraVerticalGrid.Rows.EditorRow
    Friend WithEvents colPoLine As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colStockCode As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colQty As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPrice As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents btnPost As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BindingSource1 As BindingSource
    Friend WithEvents rowSalesOrder As DevExpress.XtraVerticalGrid.Rows.EditorRow
    Friend WithEvents rowPO1 As DevExpress.XtraVerticalGrid.Rows.EditorRow
    Friend WithEvents rowCustomer1 As DevExpress.XtraVerticalGrid.Rows.EditorRow
    Friend WithEvents rowActionType As DevExpress.XtraVerticalGrid.Rows.EditorRow
    Friend WithEvents colWarehouse As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colLineAction As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents BarButtonItem1 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents RibbonPageGroup3 As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents BarHeaderItem1 As DevExpress.XtraBars.BarHeaderItem
End Class
