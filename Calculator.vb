Public Class Calculator
    ' Get decimal seperator
    Public Shared DOT_SIGN As String = Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator

    ' ========= OPERATORS =========
    Public Enum Operators
        PLUS
        MINUS
        MULTIPLY
        DIVIDE

        NONE
    End Enum


    ' ========= GET OPERATOR SIGN =========
    Public Shared Function getOperatorSign(ByVal op As Operators)
        Dim sign As String = ""

        Select Case op
            Case Operators.PLUS
                sign = " + "
            Case Operators.MINUS
                sign = " - "
            Case Operators.MULTIPLY
                sign = "×"
            Case Operators.DIVIDE
                sign = "÷"
        End Select

        Return sign
    End Function


    ' ========= GET OPERATOR =========
    Public Shared Function getOperator(ByVal key As String)
        Dim op As Calculator.Operators

        Select Case key
            Case "+"
                op = Calculator.Operators.PLUS
            Case "−", "-"
                op = Calculator.Operators.MINUS
            Case "×", "x", "*", "x", "X"
                op = Calculator.Operators.MULTIPLY
            Case "÷", "/"
                op = Calculator.Operators.DIVIDE
            Case Else
                op = Calculator.Operators.NONE
        End Select

        Return op
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


    ' ========= FORMAT VALUE =========
    Public Shared Function FormatValue(ByVal input As String, Optional ByVal forceFormat As Boolean = False) As String

        If input.Length > 1 And input.Substring(0, 1) = "0" And input.Contains(DOT_SIGN) = False Then
            Return input.Substring(1)
        ElseIf input.Contains(DOT_SIGN) Then
            If forceFormat Then
                Return Format(CDbl(input), "##,##0.#####################")
            Else
                Return input
            End If
        Else
            Return Format(CDbl(input), "##,##0.#####################")
        End If
    End Function

End Class
