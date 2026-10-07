' ***************************************************
'
'               SOJO Mini Calculator
'
' ***************************************************

' ========= MAIN FORM =========
Public Class Main

    ' Use debugging if needed
    Const _DEBUG As Boolean = True

    ' Use rounded controls or not
    Const ROUNDED_BUTTON As Boolean = False
    Const ROUNDED_DISPLAY As Boolean = False

    Const CALCULATOR_BACKGROUND_COLOR As Integer = &HFF585A67

    Const DISPLAY_BACKGROUND_COLOR As Integer = &HFFC1D797
    Const DISPLAY_TEXT_COLOR As Integer = &HFFFDFFFE

    Const NUMBER_BACKGROUND_COLOR As Integer = &HFF3A3D46
    Const NUMBER_TEXT_COLOR As Integer = &HFFFDFFFE

    Const FUNCTION_BACKGROUND_COLOR As Integer = &HFFBABEC1
    Const FUNCTION_TEXT_COLOR As Integer = &HFFFDFFFE

    ' Max length of the display numbers
    Const MAX_LENGTH As Integer = 13

    ' Status indicator if equal button or another calculation has presented a total result
    Dim hasResult As Boolean

    ' Variables holding current and previous operations
    Dim currentOperator As Calculator.Operators = Calculator.Operators.NONE

    ' Variables holding the result and input values
    Dim result As Double = 0
    Dim firstValue As Double = 0
    Dim secondValue As Double = 0

    ' Variables holding the input and output text
    Dim inputText As String = ""
    Dim outputText As String = ""

    ' ========= MAIN FORM - Control Added =========
    Private Sub Main_ControlAdded(sender As Object, e As System.Windows.Forms.ControlEventArgs) Handles Me.ControlAdded
        ' Center application on the current screen
        Me.StartPosition = FormStartPosition.CenterScreen
    End Sub


    ' ========= MAIN FORM - Load =========
    Private Sub Main_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Me.ActiveControl = Nothing

        ' Change period symbol for current language
        Dot.Text = Calculator.DOT_SIGN

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

        ' Select the result display as default
        Display.Select()
    End Sub


    ' ========= NUMBER BUTTONS =========
    Private Sub NumberButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Zero.Click, One.Click, Two.Click, Three.Click, Four.Click, Five.Click, Six.Click, Seven.Click, Eight.Click, Nine.Click, Dot.Click
        ' Get the current selected button
        Dim btn = CType(sender, Button)

        If _DEBUG Then Debug.Print("Number(before)=" & btn.Text & " : inputText=" & inputText & ", outputText=" & outputText & ", Display=" & Display.Text & ", currentOperation=" & Calculator.getOperatorSign(currentOperator))

        If hasResult Then
            outputText = ""
            currentOperator = Calculator.Operators.NONE
            hasResult = False
        End If

        'TODO: -3 + 5 = (2)

        ' Check if a period exist and if not check if its a empty string and if so add a zero before the period
        If btn.Text = Calculator.DOT_SIGN Then
            If inputText.Contains(Calculator.DOT_SIGN) Then Return

            If outputText = "" Then outputText = "0"
        Else
            ' Check if the output is zero and clear it if so
            If outputText = "0" Then outputText = ""
        End If

        ' Add key to the values
        inputText &= btn.Text
        outputText &= btn.Text

        ' Show and store the outputValue
        Display.Text = outputText

        If _DEBUG Then Debug.Print("Number(after)=" & btn.Text & " : inputText=" & inputText & ", outputText=" & outputText & ", Display=" & Display.Text & ", currentOperation=" & Calculator.getOperatorSign(currentOperator))

        ' Select the result display as default
        Display.Select()
    End Sub


    ' ========= OPERATOR BUTTONS =========
    Private Sub OperatorButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Plus.Click, Minus.Click, Multiply.Click, Divide.Click
        ' Get the current selected button
        Dim btn = CType(sender, Button)

        If _DEBUG Then Debug.Print("Operator(before)=" & btn.Text & " : firstValue=" & firstValue & ", secondValue=" & secondValue & ", result=" & result & ", inputText=" & inputText & ", outputText=" & outputText & ", currentOperation=" & Calculator.getOperatorSign(currentOperator))

        ' Set current operation
        Dim previousOperation = currentOperator
        currentOperator = Calculator.getOperator(btn.Text)

        ' Set the firstValue if it's zero or else set the second value
        If hasResult Then
            firstValue = result
        ElseIf firstValue = 0 Then
            Double.TryParse(inputText, firstValue)
        Else
            Double.TryParse(inputText, secondValue)
        End If

        ' Check if MINUS is pressed and if input is zero or blank
        If outputText = "" Or outputText = "0" Then
            If currentOperator = Calculator.Operators.MINUS Then
                outputText = Calculator.getOperatorSign(Calculator.Operators.MINUS)

                hasResult = False

                result = 0
                firstValue = 0
                secondValue = 0

                currentOperator = Calculator.Operators.NONE
                previousOperation = Calculator.Operators.NONE
            Else
                outputText = "0" & Calculator.getOperatorSign(currentOperator)
            End If

            Display.Text = outputText

            Return
        End If

        If previousOperation <> Calculator.Operators.NONE Then
            result = Calculator.Calculate(firstValue, secondValue, previousOperation)

            outputText = result & Calculator.getOperatorSign(currentOperator)

            firstValue = result
        ElseIf hasResult And secondValue <> 0 Then
            result = Calculator.Calculate(firstValue, secondValue, currentOperator)

            outputText = result & Calculator.getOperatorSign(currentOperator)

            firstValue = result
        Else
            outputText &= Calculator.getOperatorSign(currentOperator)
        End If

        hasResult = False

        inputText = ""

        Display.Text = outputText

        If _DEBUG Then Debug.Print("Operator(after)=" & btn.Text & " : firstValue=" & firstValue & ", secondValue=" & secondValue & ", result=" & result & ", inputText=" & inputText & ", outputText=" & outputText & ", currentOperation=" & Calculator.getOperatorSign(currentOperator) & ", previousOperation=" & Calculator.getOperatorSign(previousOperation))

        ' Select the result display as default
        Display.Select()
    End Sub


    ' ========= EQUAL BUTTON =========
    Private Sub Equal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Equal.Click
        If _DEBUG Then Debug.Print("Equal(before) : firstValue=" & firstValue & ", secondValue=" & secondValue & ", result=" & result & ", currentOperation=" & Calculator.getOperatorSign(currentOperator))

        ' Set the firstValue if it's zero or else set the second value
        If firstValue = 0 Then
            Double.TryParse(inputText, firstValue)
        Else
            Double.TryParse(inputText, secondValue)
        End If

        result = Calculator.Calculate(firstValue, secondValue, currentOperator)

        outputText = result
        inputText = ""

        firstValue = 0
        secondValue = 0
        hasResult = True

        currentOperator = Calculator.Operators.NONE

        Display.Text = outputText

        If _DEBUG Then Debug.Print("Equal(after) : firstValue=" & firstValue & ", secondValue=" & secondValue & ", result=" & result & ", currentOperation=" & Calculator.getOperatorSign(currentOperator))

        ' Select the result display as default
        Display.Select()
    End Sub


    ' ========= KEYBOARD INPUT =========
    Private Sub Main_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles Me.KeyPress
        Dim key As Char = e.KeyChar

        ' ===== DIGITS & DECIMAL =====
        If Char.IsDigit(key) OrElse key = Calculator.DOT_SIGN Then
            Dim btn As New Button()

            btn.Text = key.ToString

            NumberButton_Click(btn, EventArgs.Empty)
            e.Handled = True
            Return
        End If

        ' ===== OPERATORS =====
        Select Case key
            Case "+"c, "-"c, "*"c, "/"c
                Dim opBtn As New Button()

                Select Case key
                    Case "+"c : opBtn.Text = Calculator.getOperatorSign(Calculator.Operators.PLUS)
                    Case "-"c : opBtn.Text = Calculator.getOperatorSign(Calculator.Operators.MINUS)
                    Case "*"c : opBtn.Text = Calculator.getOperatorSign(Calculator.Operators.MULTIPLY)
                    Case "/"c : opBtn.Text = Calculator.getOperatorSign(Calculator.Operators.DIVIDE)
                End Select

                OperatorButton_Click(opBtn, EventArgs.Empty)
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
            ' Reset variable and previous result if calculation result is set to true
            If hasResult Then
                outputText = "0"
                inputText = ""

                firstValue = 0
                secondValue = 0
                result = 0

                currentOperator = Calculator.Operators.NONE

                hasResult = False
            End If

            ' Delete number from the end if no calculation has been made
            If Not hasResult AndAlso outputText.Trim().Length > 0 Then
                outputText = outputText.Substring(0, outputText.Length - 1).Trim()
                If inputText.Length > 0 Then inputText = inputText.Substring(0, inputText.Length - 1).Trim()
            End If

            ' If the value is empty replaces it with a zero
            If outputText.Trim() = "" Then
                outputText = "0"
                inputText = ""

                firstValue = 0
                secondValue = 0
                result = 0

                currentOperator = Calculator.Operators.NONE
            End If

            Display.Text = outputText

            ' Select the result display as default
            Display.Select()

            e.Handled = True
            Return
        End If

    End Sub


    ' Handles special keys (like numpad multiply/divide)
    Private Sub Main_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Multiply
                OperatorButton_Click(New Button() With {.Text = Calculator.getOperatorSign(Calculator.Operators.MULTIPLY)}, Nothing)
                e.Handled = True

            Case Keys.Divide
                OperatorButton_Click(New Button() With {.Text = Calculator.getOperatorSign(Calculator.Operators.DIVIDE)}, Nothing)
                e.Handled = True

            Case Keys.Add
                OperatorButton_Click(New Button() With {.Text = Calculator.getOperatorSign(Calculator.Operators.PLUS)}, Nothing)
                e.Handled = True

            Case Keys.Subtract
                OperatorButton_Click(New Button() With {.Text = Calculator.getOperatorSign(Calculator.Operators.MINUS)}, Nothing)
                e.Handled = True
        End Select
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
