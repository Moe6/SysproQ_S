Public Class MessagingHelper
    Public Function GetFullMessage(ex As Exception) As String
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
End Class
