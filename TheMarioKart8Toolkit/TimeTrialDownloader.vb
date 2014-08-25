Imports Newtonsoft.Json.Linq
Imports System.Text
Imports System.Net

Public Class TimeTrialDownloader
    Public Structure Score
        Public playerName As String
        Public playerNNID As String
        Public time As String
        Public countryCode As String
        Public iconUrl As String
    End Structure

    Dim response As String
    Dim request As WebClient
    Dim ttData As JObject

    Public Sub New(ByVal track_id As Integer)
        request = New WebClient()
        request.Encoding = Encoding.UTF8
        response = request.DownloadString("http://mariokart.tv/en_us/jsonapi/time_trials?category_id=" & track_id.ToString())
        ttData = JObject.Parse(response)
    End Sub

    Public Function Scores(ByVal rank As Integer) As Score
        Dim ranks As New Score
        ranks.iconUrl = ttData("top_rankers")(rank)("icon_url")
        ranks.playerNNID = ttData("top_rankers")(rank)("nnid")
        ranks.playerName = ttData("top_rankers")(rank)("name")
        ranks.time = ttData("top_rankers")(rank)("time")
        ranks.countryCode = ttData("top_rankers")(rank)("country_iso")
        Return ranks
    End Function

    Public ReadOnly Property Chart As Integer()
        Get
            Dim ints As Integer() = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
            For i As Integer = 0 To 19 Step 1
                ints(i) = ttData("histogram")("chartList")(0)("partList")(i)
            Next
            Return ints

        End Get
    End Property


End Class
