Public Class Form1
    Private WithEvents Button1 As Button
    Private TreeView1 As TreeView

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Button1 = New Button()
        Button1.Text = "Open File Explorer"
        Button1.Location = New Point(100, 100)
        Me.Controls.Add(Button1)

        TreeView1 = New TreeView()
        TreeView1.Location = New Point(100, 150)
        TreeView1.Size = New Size(400, 300)
        Me.Controls.Add(TreeView1)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim folderBrowser As New FolderBrowserDialog()

        If folderBrowser.ShowDialog() = DialogResult.OK Then
            Dim selectedFolder As String = folderBrowser.SelectedPath
            DisplayFiles(selectedFolder)
        End If
    End Sub

    Private Sub DisplayFiles(folderPath As String)
        TreeView1.Nodes.Clear()
        Dim rootNode As New TreeNode(folderPath)
        TreeView1.Nodes.Add(rootNode)
        PopulateTreeView(folderPath, rootNode)
        TreeView1.ExpandAll()
    End Sub

    Private Sub PopulateTreeView(folderPath As String, parentNode As TreeNode)
        Dim directoryInfo As New IO.DirectoryInfo(folderPath)
        Try
            For Each directory As IO.DirectoryInfo In directoryInfo.GetDirectories()
                Dim directoryNode As New TreeNode(directory.Name)
                parentNode.Nodes.Add(directoryNode)
                PopulateTreeView(directory.FullName, directoryNode)
            Next

            For Each file As IO.FileInfo In directoryInfo.GetFiles()
                Dim fileNode As New TreeNode(file.Name)
                parentNode.Nodes.Add(fileNode)
            Next
        Catch ex As Exception
            ' Handle any exceptions that may occur
        End Try
    End Sub
End Class