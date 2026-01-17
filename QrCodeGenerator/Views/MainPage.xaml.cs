namespace QrCodeGenerator.Views;

public sealed partial class MainPage : Page, IRecipient<ErrorMessage>
{
    private MainPageViewModel ViewModel => (MainPageViewModel)DataContext;

    public MainPage()
    {
        InitializeComponent();
        WeakReferenceMessenger.Default.Register(this);
    }

    private void NavigationView_OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.IsSettingsSelected)
        {
            CommandBar.Visibility = Visibility.Collapsed;
            QrCodeImage.Visibility = Visibility.Collapsed;
            QrCodeOptions.Visibility = Visibility.Collapsed;
            AppearanceOptions.Visibility = Visibility.Collapsed;
            ContentFrame.Navigate(typeof(SettingsPage));
        }
        else if (args.SelectedItemContainer?.Tag != null)
        {
            CommandBar.Visibility = Visibility.Visible;
            QrCodeImage.Visibility = Visibility.Visible;
            QrCodeOptions.Visibility = Visibility.Visible;
            AppearanceOptions.Visibility = Visibility.Visible;
            ContentFrame.Navigate(args.SelectedItemContainer.Tag as Type);
            Core.Instance.ShowTextOnImage = false;
        }

        NavigationView.IsPaneOpen = false;
    }

    private void UIElement_OnKeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == VirtualKey.Enter)
        {
            XamlRoot!.Content!.Focus(FocusState.Programmatic);
        }
    }

    public void Receive(ErrorMessage message)
    {
        _ = ShowErrorMessageAsync(message.InputField == null
            ? message.Message
            : $"{message.InputField} {message.Message}");
    }

    private async Task ShowErrorMessageAsync(string message)
    {
        ContentDialog dialog = new()
        {
            XamlRoot = XamlRoot,
            Content = message,
            PrimaryButtonText = "OK"
        };

        await dialog.ShowAsync();
    }

    private void MainPage_OnLoaded(object sender, RoutedEventArgs e)
    {

        if (ViewModel.MenuItems.Any())
        {
            NavigationView.SelectedItem = ViewModel.MenuItems[0];
            ContentFrame.Navigate(ViewModel.MenuItems[0].GeneratorPage);
        }
        else
        {
            _ = ShowErrorMessageAsync(Core.StringLocalizer["StringErrorNoGeneratorsSelected"]);
            CommandBar.Visibility = Visibility.Collapsed;
            QrCodeImage.Visibility = Visibility.Collapsed;
            QrCodeOptions.Visibility = Visibility.Collapsed;
            ContentFrame.Navigate(typeof(SettingsPage));
        }
    }

    // ReSharper disable once AsyncVoidEventHandlerMethod
    private async void BtnSelectForegroundColor_OnClick(object sender, RoutedEventArgs e)
    {
        ColorPickerDialog dialog = new()
        {
            XamlRoot = XamlRoot,
            QrCodeColor = QrCodeColor.ForegroundColor,
            SelectedColor = ColorConverter.ConvertFromVectSharpColour(Core.Instance.ForegroundColor)
        };

        await dialog.ShowAsync();
    }

    // ReSharper disable once AsyncVoidEventHandlerMethod
    private async void BtnSelectBackgroundColor_OnClick(object sender, RoutedEventArgs e)
    {
        ColorPickerDialog dialog = new()
        {
            XamlRoot = XamlRoot,
            QrCodeColor = QrCodeColor.BackgroundColor,
            SelectedColor = ColorConverter.ConvertFromVectSharpColour(Core.Instance.BackgroundColor)
        };

        await dialog.ShowAsync();
    }

    private async void BtnSelectLogo_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            FileOpenPicker fileOpenPicker = new()
            {
                FileTypeFilter = { ".png", ".svg" },
                ViewMode = PickerViewMode.Thumbnail,
                CommitButtonText = Core.StringLocalizer["StringCommit"]
            };

            StorageFile file = await fileOpenPicker.PickSingleFileAsync();
            
            if (file == null)
            {
                return;
            }

            _ = ViewModel.LoadQrCodeLogoAsync(file);
        }
        catch (Exception ex)
        {
            Log.Error("Error Message: {message} - Stacktrace: {stacktrace}", ex.Message, ex.StackTrace);
            _ = ShowErrorMessageAsync(ex.Message);
        }
    }

    private async void BtnSave_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            if (Core.Instance.QrCodeData == null)
            {
                _ = ShowErrorMessageAsync(Core.StringLocalizer["StringErrorEmpty"]);
                return;
            }

            FileSavePicker fileSavePicker = new()
            {
                FileTypeChoices =
                    {
                        {"Portable Networks Graphic (*.png)", new List<string> {".png"}},
                        {"Scalable Vector Graphic (*.svg)", new List<string> {".svg"}}
                    },
                CommitButtonText = Core.StringLocalizer["MenuSave.Label"]
            };

            StorageFile file = await fileSavePicker.PickSaveFileAsync();

            if (file == null)
            {
                return;
            }

            Core.Instance.SaveQrCode(file);

        }
        catch (Exception ex)
        {
            Log.Error("Error Message: {message} - Stacktrace: {stacktrace}", ex.Message, ex.StackTrace);
            _ = ShowErrorMessageAsync(ex.Message);
        }
    }

    private async void BtnPrint_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            if (Core.Instance.QrCodeData == null)
            {
                _ = ShowErrorMessageAsync(Core.StringLocalizer["StringErrorEmpty"]);
                return;
            }

            PrintQrCodeDialog dialog = new()
            {
                XamlRoot = XamlRoot
            };

            await dialog.ShowAsync();
        }

        catch (Exception ex)
        {
            Log.Error("Error Message: {message} - Stacktrace: {stacktrace}", ex.Message, ex.StackTrace);
            _ = ShowErrorMessageAsync(ex.Message);
        }
    }
}
