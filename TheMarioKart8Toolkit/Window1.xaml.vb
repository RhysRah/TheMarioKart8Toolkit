Imports MktvdbQuery

Public Class Window1

    Public SelectedVideo As VideoObject

    Private Sub BBCode(sender As Object, e As RoutedEventArgs)
        Dim BBCode As String = "[URL="""
        BBCode = BBCode & "mktvdb::" & SelectedVideo.youtubeId
        BBCode = BBCode & """]" & SelectedVideo.miiName & " on " & SelectedVideo.track & " - Open in The Mario Kart 8 Toolkit[/URL]"
        Clipboard.SetDataObject(BBCode)
        MsgBoxThenEnd()
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        Clipboard.SetDataObject("mktvdb::" & SelectedVideo.youtubeId)
        MsgBoxThenEnd()
    End Sub

    Private Sub Button_Click_2(sender As Object, e As RoutedEventArgs)
        Dim HTMLCode As String = "<a href="""
        HTMLCode = HTMLCode & "mktvdb::" & SelectedVideo.youtubeId
        HTMLCode = HTMLCode & """>" & SelectedVideo.miiName & " on " & SelectedVideo.track & " - Open in The Mario Kart 8 Toolkit</a>"
        Clipboard.SetDataObject(HTMLCode)
        MsgBoxThenEnd()
    End Sub

    Private Sub Button_Click_3(sender As Object, e As RoutedEventArgs)
        Dim YoutubeLink As String
        YoutubeLink = "http://www.youtube.com/watch?v=" & SelectedVideo.youtubeId
        Clipboard.SetDataObject(YoutubeLink)
        MsgBoxThenEnd()
    End Sub

    Private Sub MsgBoxThenEnd()
        MsgBox("Copied to Clipboard.")
        Close()
    End Sub
End Class
