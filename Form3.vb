Imports System.IO
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Text

Public Class Form3
    Inherits Form

    Private WithEvents richTextBox As New RichTextBox()
    Private WithEvents treeView As New TreeView()
    Private WithEvents saveToolStripMenuItem As New ToolStripMenuItem()
    Private WithEvents loadToolStripMenuItem As New ToolStripMenuItem()
    Private WithEvents loadCustomButton As New Button()
    Private writer As StreamWriter ' Declare the writer variable
    Dim logFilePath As String = "log.txt"
    Dim currentTime As DateTime = DateTime.Now
    Dim logMessage As String = currentTime.ToString("yyyy-MM-dd HH:mm:ss")
    Console.WriteLine(logMessage)
    
    Private Sub Log(message As String)
        ' Open the log file in append mode
        Using writer As New StreamWriter("log.txt", True)
            ' Write the log message to the file

        End Using
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Console.WriteLine(logMessage)
    End Sub

    Public Sub New()
        ' Set up the form
        Me.Text = "Text Editor"
        Me.Size = New Size(800, 600)
        writer.WriteLine($"{DateTime.Now} Application started")

        ' Add the RichTextBox control to the form
        richTextBox.Multiline = True
        richTextBox.Dock = DockStyle.Fill
        Me.Controls.Add(richTextBox)
        writer.WriteLine($"{DateTime.Now} richTextBox has been initialized successfully")

        ' Add the TreeView control to the form
        Me.Controls.Add(treeView)
        writer.WriteLine(DateTime.Now, "treeView has been initialized successfully")

        ' Add the File menu options
        Dim fileToolStripMenuItem As New ToolStripMenuItem("File")
        fileToolStripMenuItem.DropDownItems.Add(saveToolStripMenuItem)
        fileToolStripMenuItem.DropDownItems.Add(loadToolStripMenuItem)
        writer.WriteLine($"{DateTime.Now} fileToolStripMenuItem has been initialized successfully")

        Dim menuStrip As New MenuStrip()
        menuStrip.Items.Add(fileToolStripMenuItem)
        Me.MainMenuStrip = menuStrip
        Me.Controls.Add(menuStrip)

        ' Set up event handlers
        AddHandler saveToolStripMenuItem.Click, AddressOf SaveToolStripMenuItem_Click
        AddHandler loadToolStripMenuItem.Click, AddressOf LoadToolStripMenuItem_Click
        writer.WriteLine($"{DateTime.Now} menuStrip has been initialized successfully")
        ' Set up file tree
        treeView.Dock = DockStyle.Left
        treeView.Width = 200
        Dim rootNode As New TreeNode("Root")
        treeView.Nodes.Add(rootNode)

        ' Set up file explorer
        Dim fileExplorer As New ListView()
        fileExplorer.Dock = DockStyle.Left
        fileExplorer.View = View.Details
        fileExplorer.Columns.Add("Name")
        fileExplorer.Columns.Add("Size")
        fileExplorer.Columns.Add("Date Modified")
        fileExplorer.Width = 200
        Me.Controls.Add(fileExplorer)

        ' Populate file explorer with files from the root directory
        Dim rootDirectory As New DirectoryInfo("C:\") ' Replace with your desired root directory
        For Each file As FileInfo In rootDirectory.GetFiles()
            Dim item As New ListViewItem(file.Name)
            item.SubItems.Add(file.Length.ToString())
            item.SubItems.Add(file.LastWriteTime.ToString())
            fileExplorer.Items.Add(item)
        Next

        ' Create or append to the log file
        File.AppendAllText(logFilePath, $"Application started at {DateTime.Now}{Environment.NewLine}")
    End Sub

    Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles saveToolStripMenuItem.Click
        Using saveFileDialog As New SaveFileDialog()
            saveFileDialog.Filter = "Degamisu N64 Script (*.dens)|*.dens"
            If saveFileDialog.ShowDialog() <> DialogResult.OK Then
                Return ' Cancelling the operation
            End If
            Dim encryptedText As String = EncryptText(richTextBox.Text)
            File.WriteAllText(saveFileDialog.FileName, encryptedText)
            LogEvent($"File saved: {saveFileDialog.FileName}")
        End Using
    End Sub

    Private Sub LoadToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles loadToolStripMenuItem.Click
        Using openFileDialog As New OpenFileDialog()
            openFileDialog.Filter = "Degamisu N64 Script (*.dens)|*.dens"
            If openFileDialog.ShowDialog() <> DialogResult.OK Then
                Return ' Cancelling the operation
            End If
            Dim encryptedText As String = File.ReadAllText(openFileDialog.FileName)
            Dim decryptedText As String = DecryptText(encryptedText)
            richTextBox.Text = decryptedText
            LogEvent($"File loaded: {openFileDialog.FileName}")
        End Using
    End Sub

    Private Function EncryptCharacter(c As Char) As Char
        If Char.IsLetterOrDigit(c) Then
            Dim offset As Integer = If(Char.IsDigit(c), 48, If(Char.IsUpper(c), 65, 97))
            Dim encryptedValue As Integer = (Asc(c) - offset + 26) Mod 26
            Return Convert.ToChar(encryptedValue + offset)
        End If
        Return c
    End Function

    Private Function DecryptCharacter(c As Char) As Char
        If Char.IsLetterOrDigit(c) Then
            Dim offset As Integer = If(Char.IsDigit(c), 48, If(Char.IsUpper(c), 65, 97))
            Dim decryptedValue As Integer = (Asc(c) - offset + 26) Mod 26
            Return Convert.ToChar(decryptedValue + offset)
        End If
        Return c
    End Function

    Public Function EncryptText(text As String) As String
        Dim encryptedText As New StringBuilder()
        For Each c As Char In text
            encryptedText.Append(EncryptCharacter(c))
        Next
        Return encryptedText.ToString()
    End Function

    Public Function DecryptText(encryptedText As String) As String
        Dim decryptedText As New StringBuilder()
        For Each c As Char In encryptedText
            decryptedText.Append(DecryptCharacter(c))
        Next
        Return decryptedText.ToString()
    End Function

    Private Sub LoadCustomButton_Click(sender As Object, e As EventArgs) Handles loadCustomButton.Click
        Using openFileDialog As New OpenFileDialog()
            openFileDialog.Filter = "Degamisu N64 Script (*.dens)|*.dens"
            If openFileDialog.ShowDialog() = DialogResult.OK Then
                Dim encryptedText As String = File.ReadAllText(openFileDialog.FileName)
                Dim decryptedText As String = DecryptText(encryptedText)

                ' Use the decryptedText as needed, such as assigning it to a TextBox
                richTextBox.Text = decryptedText

                LogEvent($"File loaded: {openFileDialog.FileName}")
            End If
        End Using
    End Sub

    ' Declare the saveCustomButton control with WithEvents
    Private WithEvents saveCustomButton As Button

    Private Sub SaveCustomButton_Click(sender As Object, e As EventArgs) Handles saveCustomButton.Click
        Using saveFileDialog As New SaveFileDialog()
            saveFileDialog.Filter = "Degamisu N64 Script (*.dens)|*.dens"
            If saveFileDialog.ShowDialog() = DialogResult.OK Then
                ' Custom logic for saving .dens file
            End If
        End Using
    End Sub

    ' Declare the selectFolderButton control with WithEvents
    Private WithEvents selectFolderButton As Button

    Private Sub SelectFolderButton_Click(sender As Object, e As EventArgs) Handles selectFolderButton.Click
        Using folderBrowserDialog As New FolderBrowserDialog()
            If folderBrowserDialog.ShowDialog() = DialogResult.OK Then
                ' Custom logic for selecting a folder
            End If
        End Using
    End Sub

    Private Sub LogEvent(message As String)
        File.AppendAllText(logFilePath, $"{message}{Environment.NewLine}")
    End Sub

End Class