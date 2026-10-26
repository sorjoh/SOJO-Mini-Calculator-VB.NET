' **********************************************************************************************************************************************
'
'   SOJO Mini Calculator
' 
'   inspired from https://ecdn.teacherspayteachers.com/thumbitem/Calculator-Functions-Anchor-Chart-11020447-1707050000/original-11020447-1.jpg
'
' **********************************************************************************************************************************************

' ========= MAIN FORM =========
Public Class Main

    ' Use debugging if needed
    Const _DEBUG As Boolean = True

    ' Use rounded controls or not
    Const ROUNDED_BUTTON As Boolean = True
    Const ROUNDED_DISPLAY As Boolean = True

    ' Background and text colors
    Const CALCULATOR_BACKGROUND_COLOR As Integer = &HFF585A67

    Const DISPLAY_BACKGROUND_COLOR As Integer = &HFFC1D797
    Const DISPLAY_TEXT_COLOR As Integer = &HFFFDFFFE

    Const NUMBER_BACKGROUND_COLOR As Integer = &HFF3A3D46
    Const NUMBER_TEXT_COLOR As Integer = &HFFFDFFFE

    Const FUNCTION_BACKGROUND_COLOR As Integer = &HFFBABEC1
    Const FUNCTION_TEXT_COLOR As Integer = &HFFFDFFFE

    ' Max length of the display numbers
    Const MAX_OUTPUT_LENGTH As Integer = 13

    ' Status indicator if equal button or another calculation has presented a total result
    Dim hasResult As Boolean

    ' Variables holding current operations
    Dim currentOperator As Calculator.Operators = Calculator.Operators.NONE
    Dim hasOperator As Boolean

    ' Variables holding the result and input values
    Dim resultValue As Double
    Dim firstValue As Double
    Dim secondValue As Double

    ' Variables holding the input and output text
    Dim inputValue As String
    Dim outputValue As String

    ' Variable for checking if equal has been pressed or not
    Dim isEqual As Boolean

    ' Get decimal separator
    Dim DECIMAL_SEPARATOR As String = Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator

    ' ========= MAIN FORM - Control Added =========
    Private Sub Main_ControlAdded(sender As Object, e As System.Windows.Forms.ControlEventArgs) Handles Me.ControlAdded
        ' Center application on the current screen
        Me.StartPosition = FormStartPosition.CenterScreen
    End Sub


    ' ========= MAIN FORM - Load =========
    Private Sub Main_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Me.ActiveControl = Nothing

        ' Change period symbol for current language
        Dot.Text = DECIMAL_SEPARATOR

        ' Change background color for the main form
        Me.BackColor = Color.FromArgb(CALCULATOR_BACKGROUND_COLOR)
        DisplayTable.BackColor = Color.FromArgb(CALCULATOR_BACKGROUND_COLOR)

        ' Change color and make the display rounded if configured
        Display.BackColor = Color.FromArgb(DISPLAY_BACKGROUND_COLOR)
        Display.ForeColor = Color.FromArgb(DISPLAY_TEXT_COLOR)

        If ROUNDED_DISPLAY Then RoundedDisplay(Display)

        ' Change color and make all controls rounded if configured
        For Each cntrl As Control In Me.Controls
            If TypeOf cntrl Is Button Then
                If cntrl.Tag.ToString.ToLower = "number" Then
                    cntrl.BackColor = Color.FromArgb(NUMBER_BACKGROUND_COLOR)
                    cntrl.ForeColor = Color.FromArgb(NUMBER_TEXT_COLOR)
                ElseIf cntrl.Tag.ToString.ToLower = "function" Then
                    cntrl.BackColor = Color.FromArgb(FUNCTION_BACKGROUND_COLOR)
                    cntrl.ForeColor = Color.FromArgb(FUNCTION_TEXT_COLOR)
                End If

                If ROUNDED_BUTTON Then RoundedButton(cntrl)
            End If
        Next

        ' Set the operator sign for operator buttons
        Plus.Text = Calculator.getOperationSign(Calculator.Operators.PLUS)
        Minus.Text = Calculator.getOperationSign(Calculator.Operators.MINUS)
        Multiply.Text = Calculator.getOperationSign(Calculator.Operators.MULTIPLY)
        Divide.Text = Calculator.getOperationSign(Calculator.Operators.DIVIDE)

        ResetValues()

        DisplayOutput()
    End Sub


    ' ========= NUMBER INPUT =========
    Private Sub NumberInput_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Zero.Click, One.Click, Two.Click, Three.Click, Four.Click, Five.Click, Six.Click, Seven.Click, Eight.Click, Nine.Click
        ' Get the current selected button
        Dim btn = CType(sender, Button)

        If _DEBUG Then Debug.Print("Number(before) : button=" & btn.Text & ", inputValue=" & inputValue & ", outputValue=" & outputValue & ", Display=" & Display.Text & ", currentOperation=" & Calculator.getOperationSign(currentOperator) & ", hasResult=" & hasResult & ", hasOperator=" & hasOperator)

        ' Get the number
        Dim inputKey As String = btn.Text

        If hasResult Then ' Calculation has result
            ' Reset all values
            ResetValues()
            ' Add the first number to inputValue
            inputValue = inputKey
            ' Replace outputValue with the newly and initialized inputValue
            outputValue = inputValue
        ElseIf hasOperator Then ' Calculation has operator
            ' Get first and second value from the output
            Dim splittedOutputValue = outputValue.Split(Calculator.getOperationSign(currentOperator))

            ' Add the number to inputValue
            If inputValue = "0" Then
                inputValue = inputKey ' If inputValue is zero
            Else
                inputValue &= inputKey ' Otherwise add the number to inputValue
            End If

            ' Set current result initially as the first split value from the outputValue
            Dim currentResultValue As String = splittedOutputValue(0)

            ' Add minus sign in the beginning if current result value is empty and operator is minus 
            If currentOperator = Calculator.Operators.MINUS And currentResultValue = "" Then
                currentResultValue = Calculator.getOperationSign(Calculator.Operators.MINUS) & splittedOutputValue(1)
            End If

            ' Replace output value with the temporary current result value together with current operator sign and input value
            outputValue = currentResultValue & Calculator.getOperationSign(currentOperator) & inputValue
        Else
            ' Otherwise add the number to input value
            inputValue &= inputKey
            ' Replace output value with the new input value
            outputValue = inputValue
        End If

        ' Equal button has not been pressed
        isEqual = False

        ' Display the output
        DisplayOutput(outputValue)

        If _DEBUG Then Debug.Print("Number(after) : button=" & btn.Text & ", inputValue=" & inputValue & ", outputValue=" & outputValue & ", Display=" & Display.Text & ", currentOperation=" & Calculator.getOperationSign(currentOperator) & ", hasResult=" & hasResult & ", hasOperator=" & hasOperator)
    End Sub


    ' ========= DECIMAL INPUT =========
    Private Sub DecimalInput_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Dot.Click
        If _DEBUG Then Debug.Print("Decimal(before) : inputValue=" & inputValue & ", outputValue=" & outputValue & ", Display=" & Display.Text & ", currentOperation=" & Calculator.getOperationSign(currentOperator) & ", hasResult=" & hasResult & ", hasOperator=" & hasOperator)

        If hasResult Then ' Calculation has result
            ' Reset all values
            ResetValues()
        Else ' Calculation has no result
            ' Get first and second value from the output
            Dim splittedOutputValue = outputValue.Split(Calculator.getOperationSign(currentOperator))

            If hasOperator Then ' Calculation has operator
                ' Set output value to the first split output value plus the current operator sign
                outputValue = splittedOutputValue(0) & Calculator.getOperationSign(currentOperator)
                ' Set input value to the second split output value
                inputValue = splittedOutputValue(1)
            Else ' Otherwise if calculation has no operator
                ' Set output value to a empty string
                outputValue = ""

                ' If equal has been pressed before this button then replace inputValue with the a empty string, otherwise add the first split value
                If isEqual Then
                    inputValue = ""
                Else
                    inputValue = splittedOutputValue(0)
                End If
            End If

            ' Check if input value already contains a decimal otherwise add the decimal separator
            If Not inputValue.Contains(DECIMAL_SEPARATOR) Then
                inputValue &= DECIMAL_SEPARATOR
            End If
        End If

        ' Add a zero before the decimal separator if the input value begins with a decimal
        If inputValue.StartsWith(DECIMAL_SEPARATOR) Then inputValue = "0" & inputValue

        ' Add the decimal and the result of input value to the output value
        outputValue &= inputValue

        ' Equal button has not been pressed
        isEqual = False

        ' Display the output
        DisplayOutput(outputValue)

        If _DEBUG Then Debug.Print("Decimal(after) : inputValue=" & inputValue & ", outputValue=" & outputValue & ", Display=" & Display.Text & ", currentOperation=" & Calculator.getOperationSign(currentOperator) & ", hasResult=" & hasResult & ", hasOperator=" & hasOperator)
    End Sub


    ' ========= OPERATOR INPUT =========
    Private Sub OperatorInput_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Plus.Click, Minus.Click, Multiply.Click, Divide.Click
        ' Get the current selected button
        Dim btn = CType(sender, Button)

        If _DEBUG Then Debug.Print("Operator(before) : button=" & btn.Text & ", inputValue=" & inputValue & ", outputValue=" & outputValue & ", Display=" & Display.Text & ", currentOperation=" & Calculator.getOperationSign(currentOperator) & ", hasResult=" & hasResult & ", hasOperator=" & hasOperator)

        Dim previousOperator = currentOperator

        If hasOperator Then
            Dim splittedOutputValue = outputValue.Split(Calculator.getOperationSign(previousOperator))

            ResetValues()

            outputValue = splittedOutputValue(0)
            inputValue = splittedOutputValue(1)

            If previousOperator = Calculator.Operators.MINUS And outputValue = "" Then
                outputValue = Calculator.getOperationSign(Calculator.Operators.MINUS) & splittedOutputValue(1)
                inputValue = splittedOutputValue(2)
            End If

            Double.TryParse(outputValue, firstValue)
            Double.TryParse(inputValue, secondValue)

            ' Fix for example 12 * %
            If secondValue <> 0.0 Then
                resultValue = Calculator.Calculate(firstValue, secondValue, previousOperator)
            Else
                resultValue = firstValue
            End If

            currentOperator = Calculator.getOperation(btn.Text)

            inputValue = ""
            outputValue = resultValue & Calculator.getOperationSign(currentOperator)
        ElseIf outputValue = "" Then
            ResetValues()

            currentOperator = Calculator.getOperation(btn.Text)

            inputValue = "0"
            outputValue = inputValue & Calculator.getOperationSign(currentOperator)
        Else
            currentOperator = Calculator.getOperation(btn.Text)

            inputValue = ""
            outputValue &= Calculator.getOperationSign(currentOperator)
        End If

        hasOperator = True

        isEqual = False ' Fix for example 2.3 + 5 = .

        DisplayOutput(outputValue)

        If _DEBUG Then Debug.Print("Operator(after) : button=" & btn.Text & ", resultValue=" & resultValue & ", firstValue=" & firstValue & ", secondValue=" & secondValue & ", inputValue=" & inputValue & ", outputValue=" & outputValue & ", Display=" & Display.Text & ", currentOperation=" & Calculator.getOperationSign(currentOperator) & ", hasResult=" & hasResult & ", hasOperator=" & hasOperator)
    End Sub


    ' ========= EQUAL BUTTON =========
    Private Sub Equal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Equal.Click
        If _DEBUG Then Debug.Print("Equal(before) : inputValue=" & inputValue & ", outputValue=" & outputValue & ", Display=" & Display.Text & ", currentOperation=" & Calculator.getOperationSign(currentOperator) & ", hasResult=" & hasResult)

        Dim splittedOutputValue = outputValue.Split(Calculator.getOperationSign(currentOperator))

        outputValue = splittedOutputValue(0)

        Try
            If currentOperator = Calculator.Operators.MINUS And outputValue = "" Then
                outputValue = Calculator.getOperationSign(Calculator.Operators.MINUS) & splittedOutputValue(1)
                inputValue = splittedOutputValue(2)
            Else
                inputValue = splittedOutputValue(1)
            End If
        Catch ex As Exception
            Dim tmpOutputValue As String = ""

            ' Fix for example 22 =
            If Not isEqual Then
                tmpOutputValue = outputValue
            Else
                tmpOutputValue = ""
            End If

            ResetValues()

            outputValue = tmpOutputValue
        End Try

        Double.TryParse(outputValue, firstValue)
        Double.TryParse(inputValue, secondValue)

        If hasOperator Then
            resultValue = Calculator.Calculate(firstValue, secondValue, currentOperator)
        Else
            Double.TryParse(outputValue, resultValue)
        End If

        inputValue = ""
        outputValue = resultValue.ToString

        currentOperator = Calculator.Operators.NONE
        hasOperator = False
        hasResult = False

        isEqual = True ' Fix for example 2.3 + 5 = .

        DisplayOutput(outputValue)

        If _DEBUG Then Debug.Print("Equal(after) : inputValue=" & inputValue & ", outputValue=" & outputValue & ", Display=" & Display.Text & ", currentOperation=" & Calculator.getOperationSign(currentOperator) & ", hasResult=" & hasResult)
    End Sub


    ' ========= KEYBOARD INPUT =========
    Private Sub Main_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles Me.KeyPress
        Dim key As Char = e.KeyChar

        Dim btn As New Button()

        ' ===== DIGITS & DECIMAL =====
        If Char.IsDigit(key) Then
            btn.Text = key
            NumberInput_Click(btn, EventArgs.Empty)

            e.Handled = True
            Return
        ElseIf key = DECIMAL_SEPARATOR Then
            btn.Text = DECIMAL_SEPARATOR

            DecimalInput_Click(btn, EventArgs.Empty)

            e.Handled = True
            Return
        End If

        ' ===== OPERATORS =====
        Select Case key
            Case "+"c, "-"c, "*"c, "/"c
                Select Case key
                    Case "+"c : btn.Text = Calculator.getOperationSign(Calculator.Operators.PLUS)
                    Case "-"c : btn.Text = Calculator.getOperationSign(Calculator.Operators.MINUS)
                    Case "*"c : btn.Text = Calculator.getOperationSign(Calculator.Operators.MULTIPLY)
                    Case "/"c : btn.Text = Calculator.getOperationSign(Calculator.Operators.DIVIDE)
                End Select

                OperatorInput_Click(btn, EventArgs.Empty)
                e.Handled = True
                Return
        End Select

        ' ===== ENTER (=) =====
        If key = ChrW(Keys.Enter) OrElse key = "="c Then
            Equal_Click(Equal, EventArgs.Empty)
            e.Handled = True
            Return
        End If

        ' ===== BACKSPACE =====
        If key = ChrW(Keys.Back) Then
            Delete()

            e.Handled = True
            Return
        End If

    End Sub


    ' Handles special keys (like numpad multiply/divide)
    Private Sub frmMain_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Multiply
                OperatorInput_Click(New Button() With {.Text = Calculator.getOperationSign(Calculator.Operators.MULTIPLY)}, Nothing)
                e.Handled = True

            Case Keys.Divide
                OperatorInput_Click(New Button() With {.Text = Calculator.getOperationSign(Calculator.Operators.DIVIDE)}, Nothing)
                e.Handled = True

            Case Keys.Add
                OperatorInput_Click(New Button() With {.Text = Calculator.getOperationSign(Calculator.Operators.PLUS)}, Nothing)
                e.Handled = True

            Case Keys.Subtract
                OperatorInput_Click(New Button() With {.Text = Calculator.getOperationSign(Calculator.Operators.MINUS)}, Nothing)
                e.Handled = True

            Case Keys.Delete
                Delete()
                e.Handled = True
        End Select
    End Sub


    ' ========= Delete (by keyboard) =========
    Private Sub Delete()
        If _DEBUG Then Debug.Print("Delete(before) : inputValue=" & inputValue & ", outputValue=" & outputValue & ", Display=" & Display.Text & ", currentOperation=" & Calculator.getOperationSign(currentOperator) & ", hasResult=" & hasResult)

        If hasResult And hasOperator = False Then
            ResetValues()
            DisplayOutput("0")

            Return
        End If

        ' Delete number from the end if no calculation has been made
        Try
            outputValue = outputValue.Substring(0, outputValue.Length - 1).Trim()

            Dim splittedValues = outputValue.Split(Calculator.getOperationSign(currentOperator))

            If splittedValues.Length > 1 Then
                inputValue = splittedValues(1)
            Else
                inputValue = outputValue

                hasResult = False
                hasOperator = False
                currentOperator = Calculator.Operators.NONE
            End If
        Catch e As Exception
            Debug.Print(e.ToString)
        End Try

        ' If the value is empty replaces it with a zero
        If outputValue.Trim() = "" Then
            ResetValues()
            outputValue = "0"
        End If

        If _DEBUG Then Debug.Print("Delete(after) : inputValue=" & inputValue & ", outputValue=" & outputValue & ", Display=" & Display.Text & ", currentOperation=" & Calculator.getOperationSign(currentOperator) & ", hasResult=" & hasResult)

        isEqual = False ' Fix for example 2.3 + 5 = .

        DisplayOutput(outputValue)
    End Sub


    ' ========= Reset Values =========
    Private Sub ResetValues()
        ' Status indicator if equal button or another calculation has presented a total result
        hasResult = False

        'Variables holding current operations
        currentOperator = Calculator.Operators.NONE
        hasOperator = False

        ' Variables holding the result and input values
        resultValue = 0.0
        firstValue = 0.0
        secondValue = 0.0

        ' Variables holding the input and output text
        inputValue = ""
        outputValue = ""

        ' Variable for checking if equal has been pressed or not
        isEqual = False
    End Sub


    ' ========= Display Output =========
    Private Sub DisplayOutput(Optional ByVal output As String = "0")
        Display.Text = output

        Display.Select()
    End Sub

    ' ========= RoundedButton =========
    Private Sub RoundedButton(btn As Button)
        btn.FlatStyle = FlatStyle.Flat
        btn.FlatAppearance.BorderSize = 0

        Dim Raduis As New Drawing2D.GraphicsPath

        Raduis.StartFigure()
        Raduis.AddArc(New Rectangle(0, 0, 20, 20), 180, 90)
        Raduis.AddLine(10, 0, btn.Width - 20, 0)
        Raduis.AddArc(New Rectangle(btn.Width - 20, 0, 20, 20), -90, 90)
        Raduis.AddLine(btn.Width, 20, btn.Width, btn.Height - 10)
        Raduis.AddArc(New Rectangle(btn.Width - 25, btn.Height - 25, 25, 25), 0, 90)
        Raduis.AddLine(btn.Width - 10, btn.Width, 20, btn.Height)
        Raduis.AddArc(New Rectangle(0, btn.Height - 20, 20, 20), 90, 90)
        Raduis.CloseFigure()
        btn.Region = New Region(Raduis)
    End Sub

    ' ========= RoundedDisplay =========
    Private Sub RoundedDisplay(label As Label)
        label.FlatStyle = FlatStyle.Flat

        Dim Raduis As New Drawing2D.GraphicsPath

        Raduis.StartFigure()
        Raduis.AddArc(New Rectangle(0, 0, 20, 20), 180, 90)
        Raduis.AddLine(10, 0, label.Width - 20, 0)
        Raduis.AddArc(New Rectangle(label.Width - 20, 0, 20, 20), -90, 90)
        Raduis.AddLine(label.Width, 20, label.Width, label.Height - 10)
        Raduis.AddArc(New Rectangle(label.Width - 25, label.Height - 25, 25, 25), 0, 90)
        Raduis.AddLine(label.Width - 10, label.Width, 20, label.Height)
        Raduis.AddArc(New Rectangle(0, label.Height - 20, 20, 20), 90, 90)
        Raduis.CloseFigure()
        label.Region = New Region(Raduis)
    End Sub

End Class
