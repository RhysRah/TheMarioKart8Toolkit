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

    Private Sub HomepageClick(sender As Object, e As MouseButtonEventArgs) Handles HomepageIcon.MouseUp
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
        CharSpeedGround.Content = VehicleParts(CharacterName)("SpeedGround")
        CharSpeedWater.Content = VehicleParts(CharacterName)("SpeedWater")
        CharSpeedAir.Content = VehicleParts(CharacterName)("SpeedAir")
        CharSpeedAntigrav.Content = VehicleParts(CharacterName)("SpeedAntiGrav")
        CharAccel.Content = VehicleParts(CharacterName)("Accel")
        CharWeight.Content = VehicleParts(CharacterName)("Weight")
        CharHandlingGround.Content = VehicleParts(CharacterName)("HandlingGround")
        CharHandlingWater.Content = VehicleParts(CharacterName)("HandlingWater")
        CharHandlingAir.Content = VehicleParts(CharacterName)("HandlingAir")
        CharHandlingAntigrav.Content = VehicleParts(CharacterName)("HandlingAntiGrav")
        CharTraction.Content = VehicleParts(CharacterName)("Traction")
        CharMiniturbo.Content = VehicleParts(CharacterName)("MiniTurbo")
        CalculateAllParts()
    End Sub

    Private Sub BodyChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim BodyName As String
        BodyName = DirectCast(DirectCast(sender, ComboBox).SelectedItem, ListBoxItem).Content
        BodySpeedGround.Content = VehicleParts(BodyName)("SpeedGround")
        BodySpeedWater.Content = VehicleParts(BodyName)("SpeedWater")
        BodySpeedAir.Content = VehicleParts(BodyName)("SpeedAir")
        BodySpeedAntigrav.Content = VehicleParts(BodyName)("SpeedAntiGrav")
        BodyAccel.Content = VehicleParts(BodyName)("Accel")
        BodyWeight.Content = VehicleParts(BodyName)("Weight")
        BodyHandlingGround.Content = VehicleParts(BodyName)("HandlingGround")
        BodyHandlingWater.Content = VehicleParts(BodyName)("HandlingWater")
        BodyHandlingAir.Content = VehicleParts(BodyName)("HandlingAir")
        BodyHandlingAntigrav.Content = VehicleParts(BodyName)("HandlingAntiGrav")
        BodyTraction.Content = VehicleParts(BodyName)("Traction")
        BodyMiniturbo.Content = VehicleParts(BodyName)("MiniTurbo")
        CalculateAllParts()
    End Sub

    Private Sub WheelsChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim WheelsName As String
        WheelsName = DirectCast(DirectCast(sender, ComboBox).SelectedItem, ListBoxItem).Content
        WheelSpeedGround.Content = VehicleParts(WheelsName)("SpeedGround")
        WheelSpeedWater.Content = VehicleParts(WheelsName)("SpeedWater")
        WheelSpeedAir.Content = VehicleParts(WheelsName)("SpeedAir")
        WheelSpeedAntigrav.Content = VehicleParts(WheelsName)("SpeedAntiGrav")
        WheelAccel.Content = VehicleParts(WheelsName)("Accel")
        WheelWeight.Content = VehicleParts(WheelsName)("Weight")
        WheelHandlingGround.Content = VehicleParts(WheelsName)("HandlingGround")
        WheelHandlingWater.Content = VehicleParts(WheelsName)("HandlingWater")
        WheelHandlingAir.Content = VehicleParts(WheelsName)("HandlingAir")
        WheelHandlingAntigrav.Content = VehicleParts(WheelsName)("HandlingAntiGrav")
        WheelTraction.Content = VehicleParts(WheelsName)("Traction")
        WheelMiniturbo.Content = VehicleParts(WheelsName)("MiniTurbo")
        CalculateAllParts()
    End Sub

    Private Sub GliderChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim GliderName As String
        GliderName = DirectCast(DirectCast(sender, ComboBox).SelectedItem, ListBoxItem).Content
        GliderSpeedGround.Content = VehicleParts(GliderName)("SpeedGround")
        GliderSpeedWater.Content = VehicleParts(GliderName)("SpeedWater")
        GliderSpeedAir.Content = VehicleParts(GliderName)("SpeedAir")
        GliderSpeedAntigrav.Content = VehicleParts(GliderName)("SpeedAntiGrav")
        GliderAccel.Content = VehicleParts(GliderName)("Accel")
        GliderWeight.Content = VehicleParts(GliderName)("Weight")
        GliderHandlingGround.Content = VehicleParts(GliderName)("HandlingGround")
        GliderHandlingWater.Content = VehicleParts(GliderName)("HandlingWater")
        GliderHandlingAir.Content = VehicleParts(GliderName)("HandlingAir")
        GliderHandlingAntigrav.Content = VehicleParts(GliderName)("HandlingAntiGrav")
        GliderTraction.Content = VehicleParts(GliderName)("Traction")
        GliderMiniturbo.Content = VehicleParts(GliderName)("MiniTurbo")
        CalculateAllParts()
    End Sub

    Private Sub CalculateAllParts()
        Try
            SpeedGroundTotal.Value = Val(CharSpeedGround.Content) + Val(BodySpeedGround.Content) + Val(WheelSpeedGround.Content) + Val(GliderSpeedGround.Content)

        Catch ex As Exception

        End Try
    End Sub
End Class
