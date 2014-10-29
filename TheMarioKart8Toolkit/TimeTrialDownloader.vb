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
        Try
            request = New WebClient()
            request.Encoding = Encoding.UTF8
            response = request.DownloadString("http://mariokart.tv/en_us/jsonapi/time_trials?category_id=" & track_id.ToString())
            ttData = JObject.Parse(response)
        Catch ex As Exception
            ttData = JObject.Parse("{ ""cup_id"": 1, ""cup_name"": ""Pilz-Cup"", ""course_id"": 27, ""course_name"": ""Mario Kart-Stadion"", ""histogram"": { ""category"": 27, ""orderBy"": 0, ""chartList"": [ { ""scale"": 100, ""since"": 0, ""totalEntry"": 395340, ""groupNum"": 0, ""centerScore"": 119035, ""right"": 95824, ""partList"": [ 380, 1637, 11581, 45572, 85377, 87258, 40134, 30557, 22965, 17612, 13887, 10318, 7762, 5733, 4198, 3152, 2447, 1855, 1574, 1341 ], ""topScore"": 95824, ""chartCode"": 0, ""groupIndex"": 255, ""totalScore"": 48755899330, ""endScore"": 179999, ""partWidth"": 4209, ""addList"": null, ""left"": 180004 } ], ""axis_label"": { ""left"": ""3:00.004"", ""right"": ""1:35.824"", ""center"": ""2:17.914"" } }, ""top_rankers"": [ { ""score"": 0, ""time"": ""0:00.000"", ""name"": ""None"", ""nnid"": ""None"", ""icon_url"": ""about:blank"", ""country_iso"": ""JP"" }, { ""score"": 0, ""time"": ""0:00.000"", ""name"": ""None"", ""nnid"": ""None"", ""icon_url"": ""about:blank"", ""country_iso"": ""GB"" }, { ""score"": 0, ""time"": ""0:00.000"", ""name"": ""None"", ""nnid"": ""None"", ""icon_url"": ""about:blank"", ""country_iso"": ""JP"" }, { ""score"": 0, ""time"": ""0:00.000"", ""name"": ""None"", ""nnid"": ""None"", ""icon_url"": ""about:blank"", ""country_iso"": ""JP"" }, { ""score"": 0, ""time"": ""0:00.000"", ""name"": ""None"", ""nnid"": ""None"", ""icon_url"": ""about:blank"", ""country_iso"": ""FR"" }, { ""score"": 0, ""time"": ""0:00.000"", ""name"": ""None"", ""nnid"": ""None"", ""icon_url"": ""about:blank"", ""country_iso"": ""ES"" } ] } ")
        End Try
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
