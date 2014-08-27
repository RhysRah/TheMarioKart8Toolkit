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
        ListBox1.Items.Add("Finished downloading, now installing.")
    End Sub

    Private Sub ChangeProgressBar(ByVal sender As Object, ByVal e As DownloadProgressChangedEventArgs)
        ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub DownloadComplete()
        Dim file As ZipFile = ZipFile.Read("tmp")
        file.ExtractAll(My.Application.Info.DirectoryPath, ExtractExistingFileAction.OverwriteSilently)
        ListBox1.Items.Add("installation has finished, the update was installed successfully.")
        file.Dispose()
        My.Computer.FileSystem.DeleteFile("tmp")
        ListBox1.Items.Add("Deleted temporary update files.")
        ListBox1.Items.Add("Restarting app")
        Dim p As New Process()
        p.StartInfo.FileName = "TheMarioKart8Toolkit.exe"
        p.Start()
        End
    End Sub
End Class
