Imports System.Windows.Threading
Imports System.Windows.Media.Animation
Imports System.Windows.Media.Effects
Class MainWindow
    Dim Timer As DispatcherTimer
    Dim PanelOpen, PanelClose As DoubleAnimation
    Dim IsPanelOpen As Boolean
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
End Class
