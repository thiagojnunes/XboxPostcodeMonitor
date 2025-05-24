using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using PostCodeSerialMonitor.ViewModels;
using System.Diagnostics;
using System;
using Avalonia.Controls.Documents;
using Avalonia.Controls.Primitives;

namespace PostCodeSerialMonitor.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        if (DataContext is MainWindowViewModel viewModel)
        {
            viewModel.StorageProvider = StorageProvider;
            viewModel.OnLoaded();
        }
    }

    private void OnHyperlinkClick(object sender, RoutedEventArgs e)
    {
        if (sender is TextBlock textBlock && textBlock.Tag is string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                // Log the error or show a message to the user
                Debug.WriteLine($"Failed to open URL: {ex.Message}");
            }
        }
    }
}