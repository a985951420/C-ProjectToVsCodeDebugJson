using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using VsCodeDebugGen.Desktop.ViewModels;
using VsCodeDebugGen.Desktop.Views;
using VsCodeDebugGen.Desktop.Services;
using VsCodeDebugGen.Desktop.Services.Interfaces;
using VsCodeDebugGen.Core.Interfaces;
using VsCodeDebugGen.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;

namespace VsCodeDebugGen.Desktop;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        ConfigureServices();
    }

    private void ConfigureServices()
    {
        var services = new ServiceCollection();

        // 配置日志
        ConfigureLogging(services);

        // 注册 Core 层服务
        services.AddSingleton<IProjectFinder, ProjectFinderService>();
        services.AddSingleton<IProjectParser, ProjectParserService>();
        services.AddSingleton<IConfigGenerator, ConfigGeneratorService>();

        // 注册 UI 层服务
        services.AddSingleton<ILoggingService, LoggingService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<ITemplateService, TemplateService>();
        services.AddSingleton<IHistoryService, HistoryService>();
        services.AddSingleton<IPortConfigurationService, PortConfigurationService>();
        services.AddSingleton<IProjectSelectionService, ProjectSelectionService>();

        // 注册 ViewModels
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<ProjectScanViewModel>();
        services.AddTransient<ConfigurationViewModel>();
        services.AddTransient<LogViewModel>();
        services.AddTransient<TemplateManagerViewModel>();
        services.AddTransient<HistoryViewModel>();
        services.AddTransient<PreviewDialogViewModel>();
        services.AddTransient<TemplateEditDialogViewModel>();

        Services = services.BuildServiceProvider();
    }

    private void ConfigureLogging(IServiceCollection services)
    {
        var logDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "VsCodeDebugGen",
            "logs");

        Directory.CreateDirectory(logDirectory);

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(
                path: Path.Combine(logDirectory, "vscodegen-.log"),
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog(dispose: true);
        });
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit.
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();

            var mainWindowViewModel = Services.GetRequiredService<MainWindowViewModel>();
            var mainWindow = new MainWindow
            {
                DataContext = mainWindowViewModel
            };

            // 设置对话框服务的主窗口引用
            var dialogService = Services.GetRequiredService<IDialogService>();
            dialogService.SetMainWindow(mainWindow);

            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}