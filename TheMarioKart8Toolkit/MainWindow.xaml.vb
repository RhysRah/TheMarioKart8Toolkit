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
            MiniTurboTotal.Value = Val(CharMiniTurbo.Content) + Val(BodyMiniTurbo.Content) + Val(WheelMiniTurbo.Content) + Val(GliderMiniTurbo.Content)
            TractionTotal.Value = Val(CharTraction.Content) + Val(BodyTraction.Content) + Val(WheelTraction.Content) + Val(GliderTraction.Content)
            AccelTotal.Value = Val(Characcel.Content) + Val(Bodyaccel.Content) + Val(Wheelaccel.Content) + Val(Glideraccel.Content)
            HandlingAirTotal.Value = Val(CharHandlingAir.Content) + Val(BodyHandlingAir.Content) + Val(WheelHandlingAir.Content) + Val(GliderHandlingAir.Content)
            HandlinggroundTotal.Value = Val(CharHandlingground.Content) + Val(BodyHandlingGround.Content) + Val(WheelHandlingGround.Content) + Val(GliderHandlingGround.Content)
            HandlingWaterTotal.Value = Val(CharHandlingWater.Content) + Val(BodyHandlingWater.Content) + Val(WheelHandlingWater.Content) + Val(GliderHandlingWater.Content)
            HandlingAntigravTotal.Value = Val(CharHandlingAntigrav.Content) + Val(BodyHandlingAntigrav.Content) + Val(WheelHandlingAntigrav.Content) + Val(GliderHandlingAntigrav.Content)

        Catch ex As Exception

        End Try
    End Sub
End Class
