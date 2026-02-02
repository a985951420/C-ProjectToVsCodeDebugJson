using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using VsCodeDebugGen.Desktop.ViewModels;

namespace VsCodeDebugGen.Desktop.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        InitializeViews();
    }

    private void InitializeViews()
    {
        // 从 DI 容器获取 ViewModels 并创建对应的 Views
        var projectScanViewModel = App.Services.GetRequiredService<ProjectScanViewModel>();
        var projectScanView = new ProjectScanView
        {
            DataContext = projectScanViewModel
        };
        ProjectScanViewContent.Content = projectScanView;

        var configurationViewModel = App.Services.GetRequiredService<ConfigurationViewModel>();
        var configurationView = new ConfigurationView
        {
            DataContext = configurationViewModel
        };
        ConfigurationViewContent.Content = configurationView;

        var logViewModel = App.Services.GetRequiredService<LogViewModel>();
        var logView = new LogView
        {
            DataContext = logViewModel
        };
        LogViewContent.Content = logView;
    }
}
