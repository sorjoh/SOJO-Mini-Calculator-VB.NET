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

    ' Status indicator if value is stored in memory
    Dim hasMemoryRecall As Boolean = False

    ' Variables holding current and previous operations
    Dim currentOperation As Calculator.Operators = Calculator.Operators.NONE
    Dim previousOperation As Calculator.Operators = Calculator.Operators.NONE

    ' Variables holding the first and second values
    Dim firstValue As Double = 0
    Dim secondValue As Double = 0

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
    Private Sub NumberButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Zero.Click, One.Click, Two.Click, Three.Click, Four.Click, Five.Click, Six.Click, Seven.Click, Eight.Click, Nine.Click
        ' Get the current selected button
        Dim btn = CType(sender, Button)

        If _DEBUG Then Debug.Print("Number: firstValue=" & firstValue & ", secondValue=" & secondValue & ", currentOperation=" & Calculator.getOperatorSign(currentOperation) & ", previousOperation=" & Calculator.getOperatorSign(previousOperation))

    End Sub


    ' ========= OPERATOR BUTTONS =========
    Private Sub OperatorButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Plus.Click, Minus.Click, Multiply.Click, Divide.Click
        If _DEBUG Then Debug.Print("Operator: firstValue=" & firstValue & ", secondValue=" & secondValue & ", currentOperation=" & Calculator.getOperatorSign(currentOperation) & ", previousOperation=" & Calculator.getOperatorSign(previousOperation))

        ' Get the current selected button
        Dim btn As Button = CType(sender, Button)

    End Sub


    ' ========= DOT BUTTON =========
    Private Sub Dot_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Dot.Click
        If _DEBUG Then Debug.Print("Dot: firstValue=" & firstValue & ", secondValue=" & secondValue & ", currentOperation=" & Calculator.getOperatorSign(currentOperation) & ", previousOperation=" & Calculator.getOperatorSign(previousOperation))


    End Sub


    ' ========= EQUAL BUTTON =========
    Private Sub Equal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Equal.Click
        If _DEBUG Then Debug.Print("Equal: firstValue=" & firstValue & ", secondValue=" & secondValue & ", currentOperation=" & Calculator.getOperatorSign(currentOperation) & ", previousOperation=" & Calculator.getOperatorSign(previousOperation))

    End Sub


    ' ========= KEYBOARD INPUT =========
    Private Sub Main_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles Me.KeyPress
        Dim key As Char = e.KeyChar

    End Sub


    ' Handles special keys (like numpad multiply/divide)
    Private Sub Main_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles Me.KeyDown

    End Sub


    ' ========= RESET VALUES =========
    Sub resetValues()
        hasResult = False

        firstValue = 0
        secondValue = 0

        currentOperation = Calculator.Operators.NONE
        previousOperation = Calculator.Operators.NONE
    End Sub


    ' ========= DISPLAY RESULT =========
    Sub displayResult(ByVal firstDisplayValue As String, ByVal secondDisplayValue As String, ByVal operation As Calculator.Operators, ByVal result As Double, Optional ByVal extraSignRight As String = "", Optional ByVal extraSignLeft As String = "")
        If _DEBUG Then Debug.Print("Display result: firstValue=" & firstDisplayValue & ", secondDisplayValue=" & secondValue & ", operation=" & Calculator.getOperatorSign(operation))


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
