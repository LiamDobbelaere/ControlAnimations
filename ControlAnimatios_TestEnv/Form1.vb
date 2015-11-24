Imports Digaly.ControlAnimations

Public Class Form1
    Dim anims As Animator = New Animator()

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        anims.Add("test", Panel1.MakeAnimation("Width", 100))
        anims.Add("testc", Panel1.MakeAnimation("BackColor", Color.FromArgb(0, 171, 11)))
        anims.Add("wow", Me.MakeAnimation("Opacity", CDbl(0), CDbl(100), 0.4))
        anims.Toggle("wow")
    End Sub

    Private Sub Panel1_MouseClick(sender As Object, e As MouseEventArgs) Handles Panel1.MouseClick
        anims.Toggle("test")
        anims.Toggle("testc")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        anims.Remove("test")
        anims.Add("test", Panel1.MakeAnimation("Width", 64))
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click

    End Sub
End Class
