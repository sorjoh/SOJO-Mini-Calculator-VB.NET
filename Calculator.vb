Public Class Calculator
    ' ========= OPERATORS =========
    Public Enum Operators
        PLUS
        MINUS
        MULTIPLY
        DIVIDE

        NONE
    End Enum


    ' ========= GET OPERATOR SIGN =========
    Public Shared Function getOperationSign(ByVal op As Operators)
        Dim sign As String = ""

        Select Case op
            Case Operators.PLUS
                sign = "+"
            Case Operators.MINUS
                sign = "-"
            Case Operators.MULTIPLY
                sign = "×"
            Case Operators.DIVIDE
                sign = "÷"
        End Select

        Return sign
    End Function


    ' ========= GET OPERATOR =========
    Public Shared Function getOperation(ByVal key As String)
        Dim operation As Calculator.Operators

        Select Case key
            Case "+"
                operation = Calculator.Operators.PLUS
            Case "−", "-"
                operation = Calculator.Operators.MINUS
            Case "×", "*", "x", "X"
                operation = Calculator.Operators.MULTIPLY
            Case "÷", "/"
                operation = Calculator.Operators.DIVIDE
            Case Else
                operation = Calculator.Operators.NONE
        End Select

        Return operation
    End Function


    ' ========= CALCULATE RESULT =========
    Public Shared Function Calculate(ByVal firstValue As Double, ByVal secondValue As Double, ByVal operation As Calculator.Operators) As Double
        Dim result As Double = 0

        Select Case operation
            Case Calculator.Operators.PLUS
                result = firstValue + secondValue
            Case Calculator.Operators.MINUS
                result = firstValue - secondValue
            Case Calculator.Operators.MULTIPLY
                result = firstValue * secondValue
            Case Calculator.Operators.DIVIDE
                result = firstValue / secondValue
            Case Else
                Debug.Print("Case Else")
        End Select

        Return result
    End Function

End Class
