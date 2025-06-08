using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PostCodeSerialMonitor.Models;
using PostCodeSerialMonitor.Services;
using PostCodeSerialMonitor.Utils;
using System.Threading.Tasks;
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace PostCodeSerialMonitor.ViewModels;

public partial class ConfigurationDialogViewModel : ViewModelBase
{
    private readonly ConfigurationService _configurationService;
    private readonly AppConfiguration _originalConfiguration;

    [ObservableProperty]
    private bool checkForAppUpdates;

    [ObservableProperty]
    private bool checkForCodeUpdates;

    [ObservableProperty]
    private bool checkForFwUpdates;

    [ObservableProperty]
    private string appUpdateUrl;

    [ObservableProperty]
    private string codesMetaBaseUrl;

    [ObservableProperty]
    private string fwUpdateUrl;

    [ObservableProperty]
    private ObservableCollection<string> languages;

    [ObservableProperty]
    private string selectedLanguage;

    public static ObservableCollection<string> GetAvailableLanguages()
    {
        var languages = new ObservableCollection<string>();
        var cultures = LocalizationUtils.GetAvailableCultures();
        foreach (CultureInfo culture in cultures)
            languages.Add(culture.Name);
        return languages;
    }

    public ConfigurationDialogViewModel(ConfigurationService configurationService)
    {
        _configurationService = configurationService;
        _originalConfiguration = configurationService.Config;

        // Initialize properties from current configuration
        CheckForAppUpdates = _originalConfiguration.CheckForAppUpdates;
        CheckForCodeUpdates = _originalConfiguration.CheckForCodeUpdates;
        CheckForFwUpdates = _originalConfiguration.CheckForFwUpdates;
        AppUpdateUrl = _originalConfiguration.AppUpdateUrl.ToString();
        CodesMetaBaseUrl = _originalConfiguration.CodesMetaBaseUrl.ToString();
        FwUpdateUrl = _originalConfiguration.FwUpdateUrl.ToString();
        SelectedLanguage = _originalConfiguration.Language;

        //Add available languages
        Languages = GetAvailableLanguages();
    }

    [RelayCommand]
    private async Task SaveAsync(Window window)
    {
        await _configurationService.UpdateConfigurationAsync(config =>
        {
            config.CheckForAppUpdates = CheckForAppUpdates;
            config.CheckForCodeUpdates = CheckForCodeUpdates;
            config.CheckForFwUpdates = CheckForFwUpdates;
            config.AppUpdateUrl = new Uri(AppUpdateUrl);
            config.CodesMetaBaseUrl = new Uri(CodesMetaBaseUrl);
            config.FwUpdateUrl = new Uri(FwUpdateUrl);
            config.Language = SelectedLanguage;
        });

        window.Close();
    }

    [RelayCommand]
    private void Cancel(Window window)
    {
        window.Close();
    }
} 