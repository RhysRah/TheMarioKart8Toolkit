Imports System.Windows.Threading
Imports System.Windows.Media.Animation
Imports System.Windows.Media.Effects
Imports Newtonsoft.Json.Linq
Imports System.IO
Class MainWindow
    Dim Timer As DispatcherTimer
    Dim PanelOpen, PanelClose As DoubleAnimation
    Dim IsPanelOpen As Boolean
    Dim VehicleParts As JObject
#Region "Main Window"
    Private Sub AppStart(sender As Object, e As RoutedEventArgs)
        Timer = New DispatcherTimer
        AddHandler Timer.Tick, AddressOf TimerTick
        Timer.Interval = New TimeSpan(0, 0, 0, 0, 10)
        Timer.Start()
        Opacity = 0

        PanelOpen = New DoubleAnimation(940, 1140, New Duration(New TimeSpan(0, 0, 0, 0, 500)))
        PanelOpen.EasingFunction = New SineEase

        PanelClose = New DoubleAnimation(1140, 940, New Duration(New TimeSpan(0, 0, 0, 0, 500)))
        PanelClose.EasingFunction = New SineEase

        VehicleParts = JObject.Parse(File.ReadAllText("partlist.json"))
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
            Player1TotalB.Text = Val(Player1GP1B.Text) + Val(Player1GP2B.Text) + Val(Player1GP3b.Text)
            Player2TotalB.Text = Val(Player2GP1B.Text) + Val(Player2GP2b.Text) + Val(Player2GP3b.Text)
            Player3TotalB.Text = Val(Player3GP1B.Text) + Val(Player3GP2B.Text) + Val(Player3GP3b.Text)
            Player4TotalB.Text = Val(Player4GP1B.Text) + Val(Player4GP2B.Text) + Val(Player4GP3b.Text)
            Player5TotalB.Text = Val(Player5GP1B.Text) + Val(Player5GP2B.Text) + Val(Player5GP3b.Text)
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
End Class
