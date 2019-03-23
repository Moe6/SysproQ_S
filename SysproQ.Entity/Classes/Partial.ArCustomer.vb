Partial Public Class ArCustomer
    Public Sub New()

    End Sub
    Public Sub New(cust As String)
        With Me
            .Customer = Trim(cust)
            .Name = cust
            .ShortName = cust
            .ExemptFinChg = "Y"
            .MaintHistory = "Y"
            .CreditStatus = "0"
            .CreditLimit = 0
            .InvoiceCount = 0
            .Salesperson = "C1"
            .Branch = "C1"
            .TermsCode = "0"
            .BalanceType = "I"
            .TaxStatus = "E"
            .OutstOrdVal = 0
            .NumOutstOrd = 0
            .Currency = "BWP"
            .GstExemptFlag = "E"
            .GstLevel = "I"
            .DetailMoveReqd = "Y"
            .InterfaceFlag = "Y"

        End With
    End Sub

End Class
