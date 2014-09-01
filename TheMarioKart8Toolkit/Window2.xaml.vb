Public Class Window2
    Dim MainWindowSettings As MainWindow

    Private Property Label As SolidColorBrush

    Private Sub CloseHighlight(sender As Object, e As MouseEventArgs) Handles CloseSettings.MouseEnter
        CloseSettings.Foreground = Brushes.Red
    End Sub

    Private Sub CloseLeave(sender As Object, e As MouseEventArgs) Handles CloseSettings.MouseLeave
        CloseSettings.Foreground = Brushes.Black
    End Sub

    Private Sub CloseSettings_Click(sender As Object, e As RoutedEventArgs) Handles CloseSettings.Click
        Close()
    End Sub

    Private Sub DragWindow(sender As Object, e As MouseButtonEventArgs)
        DragMove()
    End Sub

    Private Sub DarkThemeSelected(sender As Object, e As MouseButtonEventArgs)

        Dim BackgroundBrush As ImageBrush = New ImageBrush(New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/BGDark.png", UriKind.Absolute)))
        BackgroundBrush.Stretch = Stretch.None
        WindowBackground.Background = BackgroundBrush
        UpdateLayout()
        DarkThemeButton.Content = ChrW(&HF205)
        LightThemeButton.Content = ChrW(&HF204)
        DarkThemeButton.Foreground = Brushes.White
        LightThemeButton.Foreground = Brushes.White
        Label = Brushes.White
    End Sub

    Public Sub New(ByRef Window As MainWindow)
        MyBase.New()
        InitializeComponent()
        MainWindowSettings = Window
    End Sub
End Class
