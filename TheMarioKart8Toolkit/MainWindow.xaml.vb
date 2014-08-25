Imports System.Windows.Threading
Imports System.Windows.Media.Animation
Imports System.Windows.Media.Effects
Imports Newtonsoft.Json.Linq
Imports Newtonsoft.Json
Imports System.IO

Class MainWindow

    Dim Timer As DispatcherTimer
    Dim PanelOpen, PanelClose As DoubleAnimation
    Dim IsPanelOpen As Boolean
    Dim VehicleParts As JObject
    Dim PossibleTracks As Integer()
    Dim TTViewerMiiverseLinks As String()

    Private Delegate Sub TTRankingDelegate(ByVal id As Integer)

#Region "Main Window"
    Private Sub AppStart(sender As Object, e As RoutedEventArgs)

        InitTTViewer()

        Timer = New DispatcherTimer
        AddHandler Timer.Tick, AddressOf TimerTick
        Timer.Interval = New TimeSpan(0, 0, 0, 0, 10)
        Timer.Start()
        Opacity = 0

        PanelOpen = New DoubleAnimation(940, 1140, New Duration(New TimeSpan(0, 0, 0, 0, 500)))
        PanelOpen.EasingFunction = New CircleEase

        PanelClose = New DoubleAnimation(1140, 940, New Duration(New TimeSpan(0, 0, 0, 0, 500)))
        PanelClose.EasingFunction = New CircleEase

        VehicleParts = JObject.Parse(File.ReadAllText("partlist.json"))
        TTViewerMiiverseLinks = {"", "", "", "", "", ""}

    End Sub

    Private Sub InitTTViewer()

        PossibleTracks = {0, 0, 0, 0, 0}

        MushroomCupPicture.Effect = New DropShadowEffect()

        Track1Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_Mario_Kart_Stadium.png"))
        PossibleTracks(1) = 27
        Track2Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_Water_Park.png"))
        PossibleTracks(2) = 28
        Track3Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_Sweet_Sweet_Canyon.png"))
        PossibleTracks(3) = 19
        Track4Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_Thwomp_Ruins.png"))
        PossibleTracks(4) = 17

        Dim effect As New DropShadowEffect()
        effect.ShadowDepth = 10
        Track1Image.Effect = effect
        DoTTRankings(PossibleTracks(1))

    End Sub

    Private Sub TimerTick()
        Opacity += 0.03
        If Opacity > 1 Then
            Opacity = 1
            Timer.Stop()
        End If
    End Sub

    Private Sub MinimiseHighlight(sender As Object, e As MouseEventArgs) Handles MinimiseWindow.MouseEnter
        MinimiseWindow.Foreground = Brushes.Orange
    End Sub

    Private Sub MinimiseLeave(sender As Object, e As MouseEventArgs) Handles MinimiseWindow.MouseLeave
        MinimiseWindow.Foreground = Brushes.Black
    End Sub

    Private Sub CloseLeave(sender As Object, e As MouseEventArgs) Handles CloseWindow.MouseLeave
        CloseWindow.Foreground = Brushes.Black
    End Sub

    Private Sub CloseHighlight(sender As Object, e As MouseEventArgs) Handles CloseWindow.MouseEnter
        CloseWindow.Foreground = Brushes.Red
    End Sub

    Private Sub CloseWindow_Click(sender As Object, e As RoutedEventArgs) Handles CloseWindow.Click
        Close()
    End Sub

    Private Sub MinimiseWindow_Click(sender As Object, e As RoutedEventArgs) Handles MinimiseWindow.Click
        WindowState = Windows.WindowState.Minimized
    End Sub

    Private Sub DragWindow(sender As Object, e As MouseButtonEventArgs)
        DragMove()
    End Sub

    Private Sub MenuHighlight(sender As Object, e As MouseEventArgs) Handles MenuButton.MouseEnter
        MenuButton.Foreground = Brushes.Turquoise
    End Sub

    Private Sub MenuLeave(sender As Object, e As MouseEventArgs) Handles MenuButton.MouseLeave
        MenuButton.Foreground = Brushes.Black
    End Sub

    Private Sub MenuButton_Click(sender As Object, e As RoutedEventArgs) Handles MenuButton.Click
        If IsPanelOpen Then
            BeginAnimation(WidthProperty, PanelClose)
        Else
            BeginAnimation(WidthProperty, PanelOpen)
        End If
        IsPanelOpen = Not IsPanelOpen
    End Sub

    Private Sub IconHighlight(sender As Object, e As MouseEventArgs) Handles HomepageIcon.MouseEnter, CalculatorIcon.MouseEnter, TableIcon.MouseEnter, TimeIcon.MouseEnter, MKTVIcon.MouseEnter
        DirectCast(sender, Label).Foreground = Brushes.MediumSeaGreen
        DirectCast(sender, Label).Effect = New BlurEffect
    End Sub

    Private Sub IconLeave(sender As Object, e As MouseEventArgs) Handles HomepageIcon.MouseLeave, CalculatorIcon.MouseLeave, TableIcon.MouseLeave, TimeIcon.MouseLeave, MKTVIcon.MouseLeave, VisitThread.MouseLeave
        DirectCast(sender, Label).Foreground = Brushes.Black
        DirectCast(sender, Label).Effect = Nothing
    End Sub

    Private Sub VisitThread_Click(sender As Object, e As RoutedEventArgs) Handles VisitThread.MouseUp
        System.Diagnostics.Process.Start("http://www.mariokartwii.com/threads/139519-The-Mario-Kart-8-App")
    End Sub

    Private Sub HomepageClick(sender As Object, e As MouseButtonEventArgs) Handles HomepageIcon.MouseLeftButtonDown
        MainWindowTabControl.SelectedIndex = 0
    End Sub

    Private Sub CalcClick(sender As Object, e As MouseButtonEventArgs) Handles CalculatorIcon.MouseUp
        MainWindowTabControl.SelectedIndex = 1
    End Sub

    Private Sub TableClick(sender As Object, e As MouseButtonEventArgs) Handles TableIcon.MouseUp
        MainWindowTabControl.SelectedIndex = 2
    End Sub

    Private Sub TTClick(sender As Object, e As MouseButtonEventArgs) Handles TimeIcon.MouseUp
        MainWindowTabControl.SelectedIndex = 3
    End Sub

    Private Sub MKTVClick(sender As Object, e As MouseButtonEventArgs) Handles MKTVIcon.MouseUp
        MainWindowTabControl.SelectedIndex = 4
    End Sub

    Private Sub LabelClick(sender As Object, e As MouseEventArgs) Handles VisitThread.MouseEnter
        DirectCast(sender, Label).Foreground = Brushes.DodgerBlue
    End Sub
#End Region

#Region "Stats Calculator"
    Private Sub CharacterChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim CharacterName As String
        CharacterName = DirectCast(DirectCast(sender, ComboBox).SelectedItem, ListBoxItem).Content
        CharSpeedGround.Content = VehicleParts(CharacterName)("SpeedGround").ToString()
        CharSpeedWater.Content = VehicleParts(CharacterName)("SpeedWater").ToString()
        CharSpeedAir.Content = VehicleParts(CharacterName)("SpeedAir").ToString()
        CharSpeedAntigrav.Content = VehicleParts(CharacterName)("SpeedAntiGrav").ToString()
        CharAccel.Content = VehicleParts(CharacterName)("Accel").ToString()
        CharWeight.Content = VehicleParts(CharacterName)("Weight").ToString()
        CharHandlingGround.Content = VehicleParts(CharacterName)("HandlingGround").ToString()
        CharHandlingWater.Content = VehicleParts(CharacterName)("HandlingWater").ToString()
        CharHandlingAir.Content = VehicleParts(CharacterName)("HandlingAir").ToString()
        CharHandlingAntigrav.Content = VehicleParts(CharacterName)("HandlingAntiGrav").ToString()
        CharTraction.Content = VehicleParts(CharacterName)("Traction").ToString()
        CharMiniturbo.Content = VehicleParts(CharacterName)("MiniTurbo").ToString()
        CalculateAllParts()
    End Sub

    Private Sub BodyChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim BodyName As String
        BodyName = DirectCast(DirectCast(sender, ComboBox).SelectedItem, ListBoxItem).Content
        BodySpeedGround.Content = VehicleParts(BodyName)("SpeedGround").ToString()
        BodySpeedWater.Content = VehicleParts(BodyName)("SpeedWater").ToString()
        BodySpeedAir.Content = VehicleParts(BodyName)("SpeedAir").ToString()
        BodySpeedAntigrav.Content = VehicleParts(BodyName)("SpeedAntiGrav").ToString()
        BodyAccel.Content = VehicleParts(BodyName)("Accel").ToString()
        BodyWeight.Content = VehicleParts(BodyName)("Weight").ToString()
        BodyHandlingGround.Content = VehicleParts(BodyName)("HandlingGround").ToString()
        BodyHandlingWater.Content = VehicleParts(BodyName)("HandlingWater").ToString()
        BodyHandlingAir.Content = VehicleParts(BodyName)("HandlingAir").ToString()
        BodyHandlingAntigrav.Content = VehicleParts(BodyName)("HandlingAntiGrav").ToString()
        BodyTraction.Content = VehicleParts(BodyName)("Traction").ToString()
        BodyMiniturbo.Content = VehicleParts(BodyName)("MiniTurbo").ToString()
        CalculateAllParts()
    End Sub

    Private Sub WheelsChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim WheelsName As String
        WheelsName = DirectCast(DirectCast(sender, ComboBox).SelectedItem, ListBoxItem).Content
        WheelSpeedGround.Content = VehicleParts(WheelsName)("SpeedGround").ToString()
        WheelSpeedWater.Content = VehicleParts(WheelsName)("SpeedWater").ToString()
        WheelSpeedAir.Content = VehicleParts(WheelsName)("SpeedAir").ToString()
        WheelSpeedAntigrav.Content = VehicleParts(WheelsName)("SpeedAntiGrav").ToString()
        WheelAccel.Content = VehicleParts(WheelsName)("Accel").ToString()
        WheelWeight.Content = VehicleParts(WheelsName)("Weight").ToString()
        WheelHandlingGround.Content = VehicleParts(WheelsName)("HandlingGround").ToString()
        WheelHandlingWater.Content = VehicleParts(WheelsName)("HandlingWater").ToString()
        WheelHandlingAir.Content = VehicleParts(WheelsName)("HandlingAir").ToString()
        WheelHandlingAntigrav.Content = VehicleParts(WheelsName)("HandlingAntiGrav").ToString()
        WheelTraction.Content = VehicleParts(WheelsName)("Traction").ToString()
        WheelMiniturbo.Content = VehicleParts(WheelsName)("MiniTurbo").ToString()
        CalculateAllParts()
    End Sub

    Private Sub GliderChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim GliderName As String
        GliderName = DirectCast(DirectCast(sender, ComboBox).SelectedItem, ListBoxItem).Content
        GliderSpeedGround.Content = VehicleParts(GliderName)("SpeedGround").ToString()
        GliderSpeedWater.Content = VehicleParts(GliderName)("SpeedWater").ToString()
        GliderSpeedAir.Content = VehicleParts(GliderName)("SpeedAir").ToString()
        GliderSpeedAntigrav.Content = VehicleParts(GliderName)("SpeedAntiGrav").ToString()
        GliderAccel.Content = VehicleParts(GliderName)("Accel").ToString()
        GliderWeight.Content = VehicleParts(GliderName)("Weight").ToString()
        GliderHandlingGround.Content = VehicleParts(GliderName)("HandlingGround").ToString()
        GliderHandlingWater.Content = VehicleParts(GliderName)("HandlingWater").ToString()
        GliderHandlingAir.Content = VehicleParts(GliderName)("HandlingAir").ToString()
        GliderHandlingAntigrav.Content = VehicleParts(GliderName)("HandlingAntiGrav").ToString()
        GliderTraction.Content = VehicleParts(GliderName)("Traction").ToString()
        GliderMiniturbo.Content = VehicleParts(GliderName)("MiniTurbo").ToString()
        CalculateAllParts()
    End Sub

    Private Sub CalculateAllParts()

        Try
            SpeedGroundTotal.Value = Val(CharSpeedGround.Content) + Val(BodySpeedGround.Content) + Val(WheelSpeedGround.Content) + Val(GliderSpeedGround.Content)
            SpeedAirTotal.Value = Val(CharSpeedAir.Content) + Val(BodySpeedAir.Content) + Val(WheelSpeedAir.Content) + Val(GliderSpeedAir.Content)
            SpeedWaterTotal.Value = Val(CharSpeedWater.Content) + Val(BodySpeedWater.Content) + Val(WheelSpeedWater.Content) + Val(GliderSpeedWater.Content)
            SpeedAntiGravTotal.Value = Val(CharSpeedAntigrav.Content) + Val(BodySpeedAntigrav.Content) + Val(WheelSpeedAntigrav.Content) + Val(GliderSpeedAntigrav.Content)
            WeightTotal.Value = Val(CharWeight.Content) + Val(BodyWeight.Content) + Val(WheelWeight.Content) + Val(GliderWeight.Content)
            MiniTurboTotal.Value = Val(CharMiniturbo.Content) + Val(BodyMiniturbo.Content) + Val(WheelMiniturbo.Content) + Val(GliderMiniturbo.Content)
            TractionTotal.Value = Val(CharTraction.Content) + Val(BodyTraction.Content) + Val(WheelTraction.Content) + Val(GliderTraction.Content)
            AccelTotal.Value = Val(CharAccel.Content) + Val(BodyAccel.Content) + Val(WheelAccel.Content) + Val(GliderAccel.Content)
            HandlingAirTotal.Value = Val(CharHandlingAir.Content) + Val(BodyHandlingAir.Content) + Val(WheelHandlingAir.Content) + Val(GliderHandlingAir.Content)
            HandlingGroundTotal.Value = Val(CharHandlingGround.Content) + Val(BodyHandlingGround.Content) + Val(WheelHandlingGround.Content) + Val(GliderHandlingGround.Content)
            HandlingWaterTotal.Value = Val(CharHandlingWater.Content) + Val(BodyHandlingWater.Content) + Val(WheelHandlingWater.Content) + Val(GliderHandlingWater.Content)
            HandlingAntiGravTotal.Value = Val(CharHandlingAntigrav.Content) + Val(BodyHandlingAntigrav.Content) + Val(WheelHandlingAntigrav.Content) + Val(GliderHandlingAntigrav.Content)

            SpeedGroundString.Content = SpeedGroundTotal.Value.ToString
            SpeedWaterString.Content = SpeedWaterTotal.Value.ToString
            SpeedAirString.Content = SpeedAirTotal.Value.ToString
            SpeedAntGravString.Content = SpeedAntiGravTotal.Value.ToString
            AccelString.Content = AccelTotal.Value.ToString
            WeightString.Content = WeightTotal.Value.ToString
            HandlingGroundString.Content = HandlingGroundTotal.Value.ToString
            HandlingWaterString.Content = HandlingWaterTotal.Value.ToString
            HandlingAirString.Content = HandlingAirTotal.Value.ToString
            HandlingAntiGravString.Content = HandlingAntiGravTotal.Value.ToString
            TractionString.Content = TractionTotal.Value.ToString
            MiniTurboString.Content = MiniTurboTotal.Value.ToString
        Catch ex As Exception

        End Try


    End Sub

    Private Sub ProgressBarChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles SpeedGroundTotal.ValueChanged, SpeedWaterTotal.ValueChanged, SpeedAirTotal.ValueChanged, SpeedAntiGravTotal.ValueChanged, AccelTotal.ValueChanged, WeightTotal.ValueChanged, HandlingGroundTotal.ValueChanged, HandlingWaterTotal.ValueChanged, HandlingAirTotal.ValueChanged, HandlingAntiGravTotal.ValueChanged, TractionTotal.ValueChanged, MiniTurboTotal.ValueChanged
        Select Case DirectCast(sender, ProgressBar).Value
            Case 0 To 2.5
                DirectCast(sender, ProgressBar).Foreground = Brushes.Red
            Case 2.5 To 4.5
                DirectCast(sender, ProgressBar).Foreground = Brushes.Yellow
            Case 4.5 To 6
                DirectCast(sender, ProgressBar).Foreground = Brushes.Green
        End Select
    End Sub
#End Region

    Private Sub UpdateScores(sender As Object, e As TextChangedEventArgs) Handles Player1GP1A.TextChanged, Player1GP2A.TextChanged, Player1GP3A.TextChanged, Player2GP1A.TextChanged, Player2GP2A.TextChanged, Player2GP3A.TextChanged, Player3GP1A.TextChanged, Player3GP2A.TextChanged, Player3GP3A.TextChanged, Player4GP1A.TextChanged, Player4GP2A.TextChanged, Player4GP3A.TextChanged, Player5GP1A.TextChanged, Player5GP2A.TextChanged, Player5GP3A.TextChanged, Player6GP1A.TextChanged, Player6GP2A.TextChanged, Player6GP3A.TextChanged, Player1GP3B.TextChanged, Player1GP2B.TextChanged, Player1GP1B.TextChanged, Player2GP3B.TextChanged, Player2GP2B.TextChanged, Player2GP1B.TextChanged, Player3GP3B.TextChanged, Player3GP2B.TextChanged, Player3GP1B.TextChanged, Player4GP3B.TextChanged, Player4GP2B.TextChanged, Player4GP1B.TextChanged, Player5GP3B.TextChanged, Player5GP2B.TextChanged, Player5GP1B.TextChanged, Player6GP3B.TextChanged, Player6GP2B.TextChanged, Player6GP1B.TextChanged
        Try
            Player1TotalA.Text = Val(Player1GP1A.Text) + Val(Player1GP2A.Text) + Val(Player1GP3A.Text)
            Player2TotalA.Text = Val(Player2GP1A.Text) + Val(Player2GP2A.Text) + Val(Player2GP3A.Text)
            Player3TotalA.Text = Val(Player3GP1A.Text) + Val(Player3GP2A.Text) + Val(Player3GP3A.Text)
            Player4TotalA.Text = Val(Player4GP1A.Text) + Val(Player4GP2A.Text) + Val(Player4GP3A.Text)
            Player5TotalA.Text = Val(Player5GP1A.Text) + Val(Player5GP2A.Text) + Val(Player5GP3A.Text)
            Player6TotalA.Text = Val(Player6GP1A.Text) + Val(Player6GP2A.Text) + Val(Player6GP3A.Text)
            Player1TotalB.Text = Val(Player1GP1B.Text) + Val(Player1GP2B.Text) + Val(Player1GP3B.Text)
            Player2TotalB.Text = Val(Player2GP1B.Text) + Val(Player2GP2B.Text) + Val(Player2GP3B.Text)
            Player3TotalB.Text = Val(Player3GP1B.Text) + Val(Player3GP2B.Text) + Val(Player3GP3B.Text)
            Player4TotalB.Text = Val(Player4GP1B.Text) + Val(Player4GP2B.Text) + Val(Player4GP3B.Text)
            Player5TotalB.Text = Val(Player5GP1B.Text) + Val(Player5GP2B.Text) + Val(Player5GP3B.Text)
            Player6TotalB.Text = Val(Player6GP1B.Text) + Val(Player6GP2B.Text) + Val(Player6GP3B.Text)
            GP1TotalA.Text = Val(Player1GP1A.Text) + Val(Player2GP1A.Text) + Val(Player3GP1A.Text) + Val(Player4GP1A.Text) + Val(Player5GP1A.Text) + Val(Player6GP1A.Text)
            GP2TotalA.Text = Val(Player1GP2A.Text) + Val(Player2GP2A.Text) + Val(Player3GP2A.Text) + Val(Player4GP2A.Text) + Val(Player5GP2A.Text) + Val(Player6GP2A.Text)
            GP3TotalA.Text = Val(Player1GP3A.Text) + Val(Player2GP3A.Text) + Val(Player3GP3A.Text) + Val(Player4GP3A.Text) + Val(Player5GP3A.Text) + Val(Player6GP3A.Text)
            GP1TotalB.Text = Val(Player1GP1B.Text) + Val(Player2GP1B.Text) + Val(Player3GP1B.Text) + Val(Player4GP1B.Text) + Val(Player5GP1B.Text) + Val(Player6GP1B.Text)
            GP2TotalB.Text = Val(Player1GP2B.Text) + Val(Player2GP2B.Text) + Val(Player3GP2B.Text) + Val(Player4GP2B.Text) + Val(Player5GP2B.Text) + Val(Player6GP2B.Text)
            GP3TotalB.Text = Val(Player1GP3B.Text) + Val(Player2GP3B.Text) + Val(Player3GP3B.Text) + Val(Player4GP3B.Text) + Val(Player5GP3B.Text) + Val(Player6GP3B.Text)
            ScoreA.Content = Val(GP1TotalA.Text) + Val(GP2TotalA.Text) + Val(GP3TotalA.Text)
            ScoreB.Content = Val(GP1TotalB.Text) + Val(GP2TotalB.Text) + Val(GP3TotalB.Text)

            If Val(ScoreA.Content) > Val(ScoreB.Content) Then
                Diff.Content = ChrW(&HF062) & (Val(ScoreA.Content) - Val(ScoreB.Content)).ToString & ChrW(&HF063)
                Diff.Foreground = Brushes.Blue
            Else
                Diff.Content = ChrW(&HF063) & (Val(ScoreB.Content) - Val(ScoreA.Content)).ToString & ChrW(&HF062)
                Diff.Foreground = Brushes.Red
            End If
        Catch ex As Exception
        End Try

    End Sub

    Private Sub TagChanged(sender As Object, e As TextChangedEventArgs) Handles TagA.TextChanged, TagB.TextChanged
        Try
            TagNames.Content = TagA.Text & " Vs " & TagB.Text
        Catch ex As Exception

        End Try

    End Sub

    Private Sub DoTTRankings(id As Integer)
        Dim d As New TTRankingDelegate(AddressOf DisplayTTRankings)
        Me.Dispatcher.BeginInvoke(d, {id})
    End Sub

    'Do not use this directly, use DoTTRankings instead
    Private Sub DisplayTTRankings(id As Integer)
        Dim TrackResults As New TimeTrialDownloader(id)

        Dim bDecoder As BitmapDecoder = BitmapDecoder.Create(New Uri(TrackResults.Scores(0).iconUrl), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.None)

        If bDecoder IsNot Nothing AndAlso bDecoder.Frames.Count > 0 Then
            Rank1Mii.Source = bDecoder.Frames(0)
        End If

        bDecoder = BitmapDecoder.Create(New Uri(TrackResults.Scores(1).iconUrl), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.None)

        If bDecoder IsNot Nothing AndAlso bDecoder.Frames.Count > 0 Then
            Rank2Mii.Source = bDecoder.Frames(0)
        End If

        bDecoder = BitmapDecoder.Create(New Uri(TrackResults.Scores(2).iconUrl), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.None)

        If bDecoder IsNot Nothing AndAlso bDecoder.Frames.Count > 0 Then
            Rank3Mii.Source = bDecoder.Frames(0)
        End If

        bDecoder = BitmapDecoder.Create(New Uri(TrackResults.Scores(3).iconUrl), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.None)

        If bDecoder IsNot Nothing AndAlso bDecoder.Frames.Count > 0 Then
            Rank4Mii.Source = bDecoder.Frames(0)
        End If

        bDecoder = BitmapDecoder.Create(New Uri(TrackResults.Scores(4).iconUrl), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.None)

        If bDecoder IsNot Nothing AndAlso bDecoder.Frames.Count > 0 Then
            Rank5Mii.Source = bDecoder.Frames(0)
        End If

        bDecoder = BitmapDecoder.Create(New Uri(TrackResults.Scores(5).iconUrl), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.None)

        If bDecoder IsNot Nothing AndAlso bDecoder.Frames.Count > 0 Then
            Rank6Mii.Source = bDecoder.Frames(0)
        End If


        Rank1Name.Content = TrackResults.Scores(0).playerName
        Rank2Name.Content = TrackResults.Scores(1).playerName
        Rank3Name.Content = TrackResults.Scores(2).playerName
        Rank4Name.Content = TrackResults.Scores(3).playerName
        Rank5Name.Content = TrackResults.Scores(4).playerName
        Rank6Name.Content = TrackResults.Scores(5).playerName

        Rank1Time.Content = TrackResults.Scores(0).time
        Rank2Time.Content = TrackResults.Scores(1).time
        Rank3Time.Content = TrackResults.Scores(2).time
        Rank4Time.Content = TrackResults.Scores(3).time
        Rank5Time.Content = TrackResults.Scores(4).time
        Rank6Time.Content = TrackResults.Scores(5).time

        Rank1Flag.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Country Flags/" & TrackResults.Scores(0).countryCode.ToLower & ".png"))
        Rank2Flag.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Country Flags/" & TrackResults.Scores(1).countryCode.ToLower & ".png"))
        Rank3Flag.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Country Flags/" & TrackResults.Scores(2).countryCode.ToLower & ".png"))
        Rank4Flag.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Country Flags/" & TrackResults.Scores(3).countryCode.ToLower & ".png"))
        Rank5Flag.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Country Flags/" & TrackResults.Scores(4).countryCode.ToLower & ".png"))
        Rank6Flag.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Country Flags/" & TrackResults.Scores(5).countryCode.ToLower & ".png"))

        For i As Integer = 0 To 5
            TTViewerMiiverseLinks(i) = "https://miiverse.nintendo.net/users/" & TrackResults.Scores(i).playerNNID
        Next
    End Sub


    Private Sub CupSelect(sender As Object, e As MouseButtonEventArgs)
        Dim TheImage As Image = DirectCast(sender, Image)

        MushroomCupPicture.Effect = Nothing
        FlowerCupPicture.Effect = Nothing
        StarCupPicture.Effect = Nothing
        SpecialCupPicture.Effect = Nothing
        ShellCupPicture.Effect = Nothing
        BananaCupPicture.Effect = Nothing
        LightningCupPicture.Effect = Nothing
        LeafCupPicture.Effect = Nothing


        Track1Image.Effect = Nothing
        Track2Image.Effect = Nothing
        Track3Image.Effect = Nothing
        Track4Image.Effect = Nothing

        Dim effect As New DropShadowEffect()
        effect.ShadowDepth = 10
        DirectCast(sender, Image).Effect = effect

        Select Case TheImage.Name
            Case "MushroomCupPicture"
                Track1Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_Mario_Kart_Stadium.png"))
                PossibleTracks(1) = 27
                Track2Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_Water_Park.png"))
                PossibleTracks(2) = 28
                Track3Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_Sweet_Sweet_Canyon.png"))
                PossibleTracks(3) = 19
                Track4Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_Thwomp_Ruins.png"))
                PossibleTracks(4) = 17

            Case "FlowerCupPicture"
                Track1Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_Mario_Circuit.png"))
                PossibleTracks(1) = 16
                Track2Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_Toad_Harbor.png"))
                PossibleTracks(2) = 18
                Track3Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_Twisted_Mansion.png"))
                PossibleTracks(3) = 20
                Track4Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_Shy_Guy_Falls.png"))
                PossibleTracks(4) = 21

            Case "StarCupPicture"
                Track1Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_Sunshine_Airport.png"))
                PossibleTracks(1) = 26
                Track2Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_Dolphin_Shoals.png"))
                PossibleTracks(2) = 29
                Track3Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_Electrodrome.png"))
                PossibleTracks(3) = 25
                Track4Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_Mount_Wario.png"))
                PossibleTracks(4) = 24

            Case "SpecialCupPicture"
                Track1Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_Cloudtop_Cruise.png"))
                PossibleTracks(1) = 23
                Track2Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_Bone-Dry_Dunes.png"))
                PossibleTracks(2) = 22
                Track3Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_Bowser's_Castle.png"))
                PossibleTracks(3) = 30
                Track4Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_Rainbow_Road.png"))
                PossibleTracks(4) = 31

            Case "ShellCupPicture"
                Track1Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_Wii_Moo_Moo_Meadows.png"))
                PossibleTracks(1) = 33
                Track2Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_GBA_Mario_Circuit.png"))
                PossibleTracks(2) = 38
                Track3Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_DS_Cheep_Cheep_Beach.png"))
                PossibleTracks(3) = 36
                Track4Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_N64_Toad's_Turnpike.png"))
                PossibleTracks(4) = 35

            Case "BananaCupPicture"
                Track1Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_GCN_Dry_Dry_Desert.png"))
                PossibleTracks(1) = 42
                Track2Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_SNES_Donut_Plains_3.png"))
                PossibleTracks(2) = 41
                Track3Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_N64_Royal_Raceway.png"))
                PossibleTracks(3) = 34
                Track4Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_3DS_DK_Jungle.png"))
                PossibleTracks(4) = 32

            Case "LeafCupPicture"
                Track1Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_DS_Wario_Stadium.png"))
                PossibleTracks(1) = 46
                Track2Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_GCN_Sherbet_Land.png"))
                PossibleTracks(2) = 37
                Track3Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_3DS_Music_Park.png"))
                PossibleTracks(3) = 39
                Track4Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_N64_Yoshi_Valley.png"))
                PossibleTracks(4) = 45

            Case "LightningCupPicture"
                Track1Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_DS_Tick-Tock_Clock.png"))
                PossibleTracks(1) = 44
                Track2Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_3DS_Piranha_Plant_Slide.png"))
                PossibleTracks(2) = 43
                Track3Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_Wii_Grumble_Volcano.png"))
                PossibleTracks(3) = 40
                Track4Image.Source = New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/MK8ATTV/Tracks/MK8-_N64_Rainbow_Road.png"))
                PossibleTracks(4) = 47
        End Select
    End Sub

    Private Sub LoadRanks(ByVal ID As Integer)

    End Sub

    Private Sub GoToMiiverse(sender As Object, e As MouseButtonEventArgs)
        Select Case DirectCast(sender, Image).Name
            Case "Rank1Mii"
                System.Diagnostics.Process.Start(TTViewerMiiverseLinks(0))
            Case "Rank2Mii"
                System.Diagnostics.Process.Start(TTViewerMiiverseLinks(1))
            Case "Rank3Mii"
                System.Diagnostics.Process.Start(TTViewerMiiverseLinks(2))
            Case "Rank4Mii"
                System.Diagnostics.Process.Start(TTViewerMiiverseLinks(3))
            Case "Rank5Mii"
                System.Diagnostics.Process.Start(TTViewerMiiverseLinks(4))
            Case "Rank6Mii"
                System.Diagnostics.Process.Start(TTViewerMiiverseLinks(5))
        End Select
    End Sub

    Private Sub SelectTrack(sender As Object, e As MouseButtonEventArgs) Handles Track4Image.MouseUp

        Track1Image.Effect = Nothing
        Track2Image.Effect = Nothing
        Track3Image.Effect = Nothing
        Track4Image.Effect = Nothing

        Dim effect As New DropShadowEffect()
        effect.ShadowDepth = 10
        DirectCast(sender, Image).Effect = effect
        Select Case DirectCast(sender, Image).Name
            Case "Track1Image"
                DoTTRankings(PossibleTracks(1))
            Case "Track2Image"
                DoTTRankings(PossibleTracks(2))
            Case "Track3Image"
                DoTTRankings(PossibleTracks(3))
            Case "Track4Image"
                DoTTRankings(PossibleTracks(4))
        End Select

    End Sub

    Private Sub ComboBox_SelectionChanged_1(sender As Object, e As SelectionChangedEventArgs)

    End Sub
End Class
