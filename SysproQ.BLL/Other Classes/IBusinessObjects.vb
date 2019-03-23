Imports SysproQ.Entity
Public Interface IBusinessObjects
    ReadOnly Property TrnMessage As String
    Function GetName() As String
    Function CreateXmlIn() As String
    Function CreateXmlIn2(line As SortraObj) As String
    Function CreateXmlParams(validateOnly As Boolean) As String
    Function ValidateInput() As Boolean
End Interface

