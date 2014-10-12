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

    Private Sub DarkThemeSelected()

        Dim BackgroundBrush As ImageBrush = New ImageBrush(New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/BGDark.png", UriKind.Absolute)))
        BackgroundBrush.Stretch = Stretch.None
        WindowBackground.Background = BackgroundBrush
        MainWindowSettings.MainWindowGrid.Background = BackgroundBrush
        ThemeButton.Content = ChrW(&HF204)
        For Each currentDependencyObject As DependencyObject In Utils.FindVisualChildren(WindowBackground)
            If TypeOf currentDependencyObject Is Control Then
                DirectCast(currentDependencyObject, Control).Foreground = Brushes.White
            End If
        Next

        MainWindowSettings.EnableDarkTheme()

        My.Settings.IsDarkTheme = True
        My.Settings.Save()

        MainWindowSettings.NoHighlightColor = Brushes.White

    End Sub
    Private Sub LightThemeSelected()
        Dim BackgroundBrush As ImageBrush = New ImageBrush(New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/BG.png", UriKind.Absolute)))
        BackgroundBrush.Stretch = Stretch.None
        WindowBackground.Background = BackgroundBrush
        MainWindowSettings.MainWindowGrid.Background = BackgroundBrush
        ThemeButton.Content = ChrW(&HF205)
        For Each currentDependencyObject As DependencyObject In Utils.FindVisualChildren(WindowBackground)
            If TypeOf currentDependencyObject Is Control Then
                DirectCast(currentDependencyObject, Control).Foreground = Brushes.Black
            End If
        Next

        MainWindowSettings.EnableLightTheme()

        My.Settings.IsDarkTheme = False
        My.Settings.Save()

        MainWindowSettings.NoHighlightColor = Brushes.Black
    End Sub
    Public Sub New(ByRef Window As MainWindow)
        MyBase.New()
        InitializeComponent()
        MainWindowSettings = Window
    End Sub

    Private Sub UpdateWindowWithTheme(sender As Object, e As SelectionChangedEventArgs)
        If My.Settings.IsDarkTheme Then
            DarkThemeSelected()
        End If
    End Sub

    Private Sub UpdateWindowWithThemeStartup(sender As Object, e As RoutedEventArgs)
        If My.Settings.IsDarkTheme Then
            DarkThemeSelected()
        End If
    End Sub

    Private Sub SwitchTheme(sender As Object, e As MouseButtonEventArgs) Handles ThemeButton.MouseDown
        If My.Settings.IsDarkTheme Then
            LightThemeSelected()
        Else
            DarkThemeSelected()
        End If
    End Sub

    Private Sub ChangeBackground(sender As Object, e As MouseButtonEventArgs) Handles ChangeBackgroundButton.MouseDown
        Dim dialog As New Microsoft.Win32.OpenFileDialog
        dialog.Filter = "Image files (.jpg, .jpeg, .png) | *.jpg; *.jpeg; *.png"
        dialog.Multiselect = False

        Dim result As Boolean = dialog.ShowDialog
        If result Then
            My.Settings.CustomBackgroundPath = dialog.FileName
            My.Settings.Save()
        End If

    End Sub

    Private Sub ResetBackground(sender As Object, e As MouseButtonEventArgs)

    End Sub

End Class




Public Class Utils
    Public Shared Function FindVisualChildren(theDependencyObject As DependencyObject) As List(Of DependencyObject)
        Dim results As New List(Of DependencyObject)
        If theDependencyObject IsNot Nothing Then
            For i As Integer = 0 To VisualTreeHelper.GetChildrenCount(theDependencyObject) - 1
                Dim child As DependencyObject = VisualTreeHelper.GetChild(theDependencyObject, i)
                If child IsNot Nothing AndAlso TypeOf child Is DependencyObject Then
                    results.Add(DirectCast(child, DependencyObject))
                End If
                For Each childOfChild As DependencyObject In FindVisualChildren(child)
                    results.Add(childOfChild)
                Next
            Next
        End If
        Return results
    End Function
End Class

