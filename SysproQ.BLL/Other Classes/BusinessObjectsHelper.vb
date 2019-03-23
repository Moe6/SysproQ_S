Module BusinessObjectsHelper
    Sub AppendTrnMessage(ByRef currentMessage As String, ByVal messageToAdd As String)
        If ValidateStringInput(currentMessage) Then
            currentMessage &= Environment.NewLine
        End If
        currentMessage &= messageToAdd
    End Sub

    Function ValidateStringInput(val As String) As Boolean
        Dim validated As Boolean = False
        If val IsNot Nothing Then
            If val.Length > 0 Then
                validated = True
            End If
        End If
        Return validated
    End Function

    Function ValidateIntegerInput(val As Integer) As Boolean
        Return (val > 0)
    End Function

    Function ValidateDecimalInput(val As Decimal) As Boolean
        Return val > 0
    End Function
End Module
