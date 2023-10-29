Imports System.Windows.Forms

Public Class Form2
    Inherits Form

    Private pictureBox As New PictureBox()

    Public Sub New()
        ' Set up the form
        Me.Text = "3D Model Viewer"
        Me.Size = New Size(800, 600)

        ' Add the PictureBox control to the form
        Me.Controls.Add(pictureBox)

        ' Load and display the 3D model
        LoadModel("path_to_your_3d_model.obj")
    End Sub

    Private Sub LoadModel(modelFilePath As String)
        ' Load the 3D model from the file
        ' Replace this code with your actual code to load and render the 3D model

        ' For demonstration purposes, let's display a placeholder image
        Dim placeholderImage As Image = Image.FromFile("path_to_placeholder_image.png")
        pictureBox.Image = placeholderImage
        pictureBox.SizeMode = PictureBoxSizeMode.Zoom
    End Sub

End Class