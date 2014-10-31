Imports System.Security.Cryptography
Imports System.Net

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
        Try
            DragMove()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub DarkThemeSelected()

        Dim BackgroundBrush As ImageBrush
        If My.Settings.CustomBackgroundPath = "" Then
            BackgroundBrush = New ImageBrush(New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/BGDark.png", UriKind.Absolute)))
        Else
            BackgroundBrush = New ImageBrush(New BitmapImage(New Uri(My.Settings.CustomBackgroundPath, UriKind.Absolute)))
        End If
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

        Dim BackgroundBrush As ImageBrush
        If My.Settings.CustomBackgroundPath = "" Then
            BackgroundBrush = New ImageBrush(New BitmapImage(New Uri("pack://application:,,,/TheMarioKart8Toolkit;component/Images/BG.png", UriKind.Absolute)))
        Else
            BackgroundBrush = New ImageBrush(New BitmapImage(New Uri(My.Settings.CustomBackgroundPath, UriKind.Absolute)))
        End If
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

        LightThemeSelected()
    End Sub

    Private Sub ResetBackground(sender As Object, e As MouseButtonEventArgs) Handles ResetButton.MouseDown
        My.Settings.CustomBackgroundPath = ""
        My.Settings.Save()

        LightThemeSelected()
    End Sub

    Private Sub ToggleMenuBarDefault(sender As Object, e As MouseButtonEventArgs) Handles MenuBarOpenButton.MouseDown
        My.Settings.MenuBarOpenByDefault = Not My.Settings.MenuBarOpenByDefault
        My.Settings.Save()
        If My.Settings.MenuBarOpenByDefault Then
            MenuBarOpenButton.Content = ChrW(&HF205)
        Else
            MenuBarOpenButton.Content = ChrW(&HF204)
        End If

    End Sub

    Private Sub WindowOpen(sender As Object, e As RoutedEventArgs)
        If My.Settings.MenuBarOpenByDefault Then
            MenuBarOpenButton.Content = ChrW(&HF205)
        Else
            MenuBarOpenButton.Content = ChrW(&HF204)
        End If

        StartupToolSelector.SelectedIndex = My.Settings.DefaultTab

        If Not My.Settings.NNID = "" And Not My.Settings.SyncKey = "" Then
            RegisterNNID.IsEnabled = False
            RegisterButton.IsEnabled = False

            RegisterNNID.Text = My.Settings.NNID
        End If

    End Sub

    Private Sub ComboBox_SelectionChanged_1(sender As Object, e As SelectionChangedEventArgs)
        My.Settings.DefaultTab = StartupToolSelector.SelectedIndex
        My.Settings.Save()
    End Sub

    Private Sub RegisterButton_Click(sender As Object, e As RoutedEventArgs) Handles RegisterButton.Click
        Dim rng As RNGCryptoServiceProvider
        rng = New RNGCryptoServiceProvider()
        Dim password As Byte() = New Byte(31) {}
        rng.GetBytes(password)
        Dim syncKey As String = Convert.ToBase64String(password)

        Dim request As New WebClient()
        request.Encoding = System.Text.Encoding.UTF8

        Dim reqparm As New Specialized.NameValueCollection
        reqparm.Add("nnid", WebUtility.UrlEncode(RegisterNNID.Text))
        reqparm.Add("password", WebUtility.UrlEncode(syncKey))
        Dim responsebytes = request.UploadValues("https://winepicgaming.de/mkapp/accounts/registeraccount.php", "POST", reqparm)
        Dim result = (New Text.UTF8Encoding).GetString(responsebytes)

        If result.StartsWith("ERROR") Then
            MsgBox(result)
        Else
            MsgBox("Registration successful. Your performance will now be tracked by MKRS.")
            My.Settings.NNID = RegisterNNID.Text
            My.Settings.SyncKey = syncKey
            My.Settings.Save()

            'RegisterNNID.IsEnabled = False
            'RegisterButton.IsEnabled = False
        End If
    End Sub

    Private Sub GetPassButton_Click(sender As Object, e As RoutedEventArgs) Handles GetPassButton.Click
        MsgBox("Your Sync Key is: " & vbNewLine & My.Settings.SyncKey & vbNewLine & "Use it on another computer to sync your NNID over multiple computers. It was copied to your clipboard.")
        Clipboard.SetDataObject(My.Settings.SyncKey)
    End Sub

    Private Sub LoginButton_Click(sender As Object, e As RoutedEventArgs) Handles LoginButton.Click
        Dim request As New WebClient()
        request.Encoding = System.Text.Encoding.UTF8

        Dim reqparm As New Specialized.NameValueCollection
        reqparm.Add("nnid", WebUtility.UrlEncode(LoginNNID.Text))
        reqparm.Add("password", WebUtility.UrlEncode(LoginPass.Text))
        Dim responsebytes = request.UploadValues("https://winepicgaming.de/mkapp/accounts/accountlogin.php", "POST", reqparm)
        Dim result = (New Text.UTF8Encoding).GetString(responsebytes)

        If result.StartsWith("ERROR") Then
            MsgBox(result)
        Else
            MsgBox("Sync successful. Your performance will now be tracked by MKRS.")
            My.Settings.NNID = LoginNNID.Text
            My.Settings.SyncKey = LoginPass.Text
            My.Settings.Save()

            'RegisterNNID.IsEnabled = False
            'RegisterButton.IsEnabled = False
        End If
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

