Imports System.Net
Imports Ionic.Zip
Imports System.IO
Imports System.Threading

Public Class Updater

    Private Sub Updater_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim downloader As New WebClient
        ListBox1.Items.Add("Please wait, update is downloading.")
        AddHandler downloader.DownloadProgressChanged, AddressOf ChangeProgressBar
        AddHandler downloader.DownloadFileCompleted, AddressOf DownloadComplete
        downloader.DownloadFileAsync(New Uri("http://winepicgaming.de/MKapp/update2.zip"), "tmp")
    End Sub

    Private Sub ChangeProgressBar(ByVal sender As Object, ByVal e As DownloadProgressChangedEventArgs)
        ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub DownloadComplete()
        ListBox1.Items.Add("Finished downloading, now installing.")

        Dim tmpFile As ZipFile = ZipFile.Read("tmp")
        tmpFile.ExtractAll(My.Application.Info.DirectoryPath, ExtractExistingFileAction.OverwriteSilently)
        tmpFile.Dispose()
        My.Computer.FileSystem.DeleteFile("tmp")
        ListBox1.Items.Add("Deleted temporary update files.")

        If File.Exists("custom.bat") Then
            ListBox1.Items.Add("Running customized install script...")
            Dim cstmInstall As New Process
            cstmInstall.EnableRaisingEvents = True
            cstmInstall.StartInfo.UseShellExecute = False
            cstmInstall.StartInfo.FileName = "custom.bat"
            cstmInstall.StartInfo.RedirectStandardOutput = True
            AddHandler cstmInstall.Exited, AddressOf FinalizeInstall
            cstmInstall.Start()
        Else
            FinalizeInstall(Nothing, Nothing)
        End If
    End Sub


    Private Sub FinalizeInstall(sender As Object, e As EventArgs)
        ListBox1.Items.Add("Cleaning up...")
        File.Delete("custom.bat")

        ListBox1.Items.Add("Installation has finished, the update was installed successfully.")

        ListBox1.Items.Add("Restarting application...")
        Dim p As New Process()
        p.StartInfo.FileName = "TheMarioKart8Toolkit.exe"
        p.Start()
        End
    End Sub

End Class
