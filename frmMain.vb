Public Class frmMain

    Private Sub frmMain_ControlAdded(sender As Object, e As System.Windows.Forms.ControlEventArgs) Handles Me.ControlAdded
        ' Center application on the current screen
        Me.StartPosition = FormStartPosition.CenterScreen

        ' Change color and make all controls rounded
        Me.BackColor = Color.FromArgb(&HFF585A67)
        tblResult.BackColor = Color.FromArgb(&HFF585A67)

        RoundedLabel(lblCurrentResult)
        lblCurrentResult.BackColor = Color.FromArgb(&HFFC1D797)
        lblCurrentResult.ForeColor = Color.FromArgb(&HFFFDFFFE)

        For Each cntrl As Control In Me.Controls
            If TypeOf cntrl Is Button Then
                If cntrl.Tag.ToString.ToLower = "number" Then
                    cntrl.BackColor = Color.FromArgb(&HFF3A3D46)
                    cntrl.ForeColor = Color.FromArgb(&HFFFDFFFE)
                ElseIf cntrl.Tag.ToString.ToLower = "function" Then
                    cntrl.BackColor = Color.FromArgb(&HFFBABEC1)
                    cntrl.ForeColor = Color.FromArgb(&HFFFDFFFE)
                End If

                RoundedButton(cntrl)
            End If
        Next
    End Sub

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

    Private Sub RoundedLabel(label As Label)
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
