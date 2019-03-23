Partial Public Class SORTOI
    Public Class OrderDetailDataObject

        Public Sub New()
            'LineActionType = GetLineActionType(Action)
            'StockCode = String.Format("{0}/{1}", obj.ItemNo, obj.ID)
            'StockDescription = GetDescriptionFormatted(obj.Description)
            'OrderQty = obj.QtyToMake
            'Price = obj.LineUnitPrice
            'UserDefined = obj.Seq
            ''static values 
            'Warehouse = "FG"
            'OrderUom = "EA"
            'PriceUom = "EA"
            'NsProductClass = "NON"
        End Sub

        Public Property LineActionType As String
        Public Property StockCode As String
        Public Property StockDescription As String
        Public Property Warehouse As String
        Public Property OrderQty As String
        Public Property OrderUom As String
        Public Property Price As String
        Public Property PriceUom As String
        Public Property UserDefined As String
        Public Property CustomerPoLine As String

        'Private Shared Function GetLineActionType(action As SysproActionType) As String
        '    Select Case action
        '        Case SysproActionType.Add
        '            Return "A"
        '        Case SysproActionType.Change
        '            Return "C"
        '        Case SysproActionType.Cancel
        '            Return "D"
        '        Case Else
        '            Return ""
        '    End Select
        'End Function

        Private Shared Function GetDescriptionFormatted(desc As String) As String
            'check for ampersand sign
            If InStr(desc, "&") <> 0 Then
                desc = desc.Replace("&", "&amp;")
            End If
            'only allow for 30 characters
            If desc.Length > 30 Then
                desc = desc.Substring(0, 30)
            End If
            Return desc
        End Function

    End Class
End Class