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
    Const MAX_OUTPUT_LENGTH As Integer = 12

    ' Status indicator if equal button or another calculation has presented a total result
    Dim hasResult As Boolean

    ' Variables holding current and previous operations
    Dim currentOperator As Calculator.Operators = Calculator.Operators.NONE

    ' Variables holding the result and input values
    Dim resultValue As Double
    Dim firstValue As Double
    Dim secondValue As Double

    ' Variables holding the input and output text
    Dim inputValue As String
    Dim outputValue As String

    ' Get decimal seperator
    Dim DECIMAL_SEPARATOR As String = "." 'Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator

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
        Plus.Text = Calculator.getOperatorSign(Calculator.Operators.PLUS)
        Minus.Text = Calculator.getOperatorSign(Calculator.Operators.MINUS)
        Multiply.Text = Calculator.getOperatorSign(Calculator.Operators.MULTIPLY)
        Divide.Text = Calculator.getOperatorSign(Calculator.Operators.DIVIDE)

        ResetValues()
        DisplayOutput("0")
    End Sub


    ' ========= NUMBER INPUT =========
    Private Sub NumberInput_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Zero.Click, One.Click, Two.Click, Three.Click, Four.Click, Five.Click, Six.Click, Seven.Click, Eight.Click, Nine.Click
        ' Get the current selected button
        Dim btn = CType(sender, Button)

        If _DEBUG Then Debug.Print("Number(before) : button=" & btn.Text & ", inputValue=" & inputValue & ", outputValue=" & outputValue & ", Display=" & Display.Text & ", currentOperation=" & Calculator.getOperatorSign(currentOperator) & ", hasResult=" & hasResult)

        If hasResult Then inputValue = "" 'Modified 'ResetValues()

        If outputValue = "0" Or outputValue = "" Then
            ResetValues()
            inputValue = btn.Text
        ElseIf outputValue.Length > MAX_OUTPUT_LENGTH Then
            Return
        Else
            inputValue &= btn.Text
        End If

        outputValue &= btn.Text

        DisplayOutput(outputValue)

        If _DEBUG Then Debug.Print("Number(after) : button=" & btn.Text & ", inputValue=" & inputValue & ", outputValue=" & outputValue & ", Display=" & Display.Text & ", currentOperation=" & Calculator.getOperatorSign(currentOperator) & ", hasResult=" & hasResult)
    End Sub


    ' ========= DECIMAL INPUT =========
    Private Sub DecimalInput_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Dot.Click
        If _DEBUG Then Debug.Print("Decimal(before) : inputValue=" & inputValue & ", outputValue=" & outputValue & ", Display=" & Display.Text & ", currentOperation=" & Calculator.getOperatorSign(currentOperator) & ", hasResult=" & hasResult)

        If inputValue.Contains(DECIMAL_SEPARATOR) Then Return

        If inputValue = "" Then
            inputValue = "0" & DECIMAL_SEPARATOR
        Else
            inputValue &= DECIMAL_SEPARATOR
        End If

        If outputValue = "" Then
            outputValue = "0" & DECIMAL_SEPARATOR
        Else
            outputValue &= DECIMAL_SEPARATOR
        End If

        DisplayOutput(outputValue)

        If _DEBUG Then Debug.Print("Decimal(after) : inputValue=" & inputValue & ", outputValue=" & outputValue & ", Display=" & Display.Text & ", currentOperation=" & Calculator.getOperatorSign(currentOperator) & ", hasResult=" & hasResult)
    End Sub

    ' ========= OPERATOR INPUT =========
    Private Sub OperatorInput_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Plus.Click, Minus.Click, Multiply.Click, Divide.Click
        ' Get the current selected button
        Dim btn = CType(sender, Button)

        If _DEBUG Then Debug.Print("Operator(before) : button=" & btn.Text & ", inputValue=" & inputValue & ", outputValue=" & outputValue & ", Display=" & Display.Text & ", currentOperation=" & Calculator.getOperatorSign(currentOperator) & ", hasResult=" & hasResult)

        If inputValue = "" Then
            ResetValues()

            If btn.Text = Calculator.getOperatorSign(Calculator.Operators.MINUS) Then
                inputValue = Calculator.getOperatorSign(Calculator.Operators.MINUS)
                outputValue = Calculator.getOperatorSign(Calculator.Operators.MINUS)
            Else
                currentOperator = Calculator.getOperator(btn.Text)
                inputValue = "0"
                outputValue = "0" & Calculator.getOperatorSign(currentOperator)
            End If
        Else
            Dim previousOperator As Calculator.Operators = currentOperator
            currentOperator = Calculator.getOperator(btn.Text)

            If firstValue = 0 Then
                If outputValue.Trim().StartsWith(Calculator.getOperatorSign(Calculator.Operators.MINUS)) Then
                    Double.TryParse(inputValue, firstValue)
                    firstValue = 0 - firstValue
                Else
                    Double.TryParse(inputValue, firstValue)
                End If
            Else
                Double.TryParse(inputValue, secondValue)
            End If

            ' TODO 4 + - +
            If previousOperator <> Calculator.Operators.NONE Then
                resultValue = Calculator.Calculate(firstValue, secondValue, previousOperator)

                inputValue = resultValue
                firstValue = resultValue 'Added

                outputValue = resultValue & Calculator.getOperatorSign(currentOperator)
                hasResult = True
            Else
                resultValue = 0.0

                outputValue = inputValue & Calculator.getOperatorSign(currentOperator)
                hasResult = False
                inputValue = "" 'Added
            End If

        End If

        DisplayOutput(outputValue)

        If _DEBUG Then Debug.Print("Operator(after) : button=" & btn.Text & ", resultValue=" & resultValue & ", firstValue=" & firstValue & ", secondValue=" & secondValue & ", inputValue=" & inputValue & ", outputValue=" & outputValue & ", Display=" & Display.Text & ", currentOperation=" & Calculator.getOperatorSign(currentOperator) & ", hasResult=" & hasResult)
    End Sub


    ' ========= EQUAL BUTTON =========
    Private Sub Equal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Equal.Click
        If _DEBUG Then Debug.Print("Equal(before) : inputValue=" & inputValue & ", outputValue=" & outputValue & ", Display=" & Display.Text & ", currentOperation=" & Calculator.getOperatorSign(currentOperator) & ", hasResult=" & hasResult)

        If _DEBUG Then Debug.Print("Equal(after) : inputValue=" & inputValue & ", outputValue=" & outputValue & ", Display=" & Display.Text & ", currentOperation=" & Calculator.getOperatorSign(currentOperator) & ", hasResult=" & hasResult)
    End Sub


    ' ========= KEYBOARD INPUT =========
    Private Sub Main_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles Me.KeyPress


    End Sub


    ' Handles special keys (like numpad multiply/divide)
    Private Sub Main_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles Me.KeyDown

    End Sub


    ' ========= Reset Values =========
    Private Sub ResetValues()
        hasResult = False

        currentOperator = Calculator.Operators.NONE

        resultValue = 0.0
        firstValue = 0.0
        secondValue = 0.0

        inputValue = ""
        outputValue = ""
    End Sub


    ' ========= Display Output =========
    Private Sub DisplayOutput(output As String)
        ' Display the output
        Display.Text = output

        ' Select the result display as default
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
