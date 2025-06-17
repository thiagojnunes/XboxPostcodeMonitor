using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using PostCodeSerialMonitor.Models;
using PostCodeSerialMonitor.Utils;
using Microsoft.Extensions.Logging;

namespace PostCodeSerialMonitor.Services;

public class GithubUpdateService
{
    private readonly ConfigurationService _configurationService;
    private readonly JsonSerializerOptions _jsonSerializeOptions;
    private readonly string _localPath;
    private readonly HttpClient _httpClient;
    private readonly ILogger<GithubUpdateService> _logger;
    public AppConfiguration Config => _configurationService.Config;

    public GithubUpdateService(
        ConfigurationService configurationService,
        JsonSerializerOptions jsonOptions,
        ILogger<GithubUpdateService> logger)
    {
        _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
        _jsonSerializeOptions = jsonOptions ?? throw new ArgumentNullException(nameof(jsonOptions));
        _localPath = _configurationService.Config.MetaStoragePath
            ?? throw new ArgumentNullException(nameof(_configurationService.Config.MetaStoragePath));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpClient = new HttpClient();

        //All GitHub's API requests must include a valid User-Agent header.
        //@see https://docs.github.com/en/rest/using-the-rest-api/getting-started-with-the-rest-api?apiVersion=2022-11-28#user-agent
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "XboxPostcodeMonitor");
    }

    public async Task<bool> CheckForAppUpdatesAsync(string localVersion)
    {
        // Get the latest release from GitHub repo.
        var remoteRelease = await GetRepositoryLatestReleaseAsync("xboxoneresearch", "XboxPostcodeMonitor");
        var remoteVersion = remoteRelease?.tag_name ?? string.Empty;

        SemanticVersionUtils local = new SemanticVersionUtils(localVersion);
        SemanticVersionUtils remote = new SemanticVersionUtils(remoteVersion);

        return remote > local;
    }

    public async Task<bool> CheckForFirmwareUpdatesAsync(string localVersion)
    {
        // Get the latest release from GitHub repo.
        var remoteRelease = await GetRepositoryLatestReleaseAsync("xboxoneresearch", "PicoDurangoPOST");
        var remoteVersion = remoteRelease?.tag_name ?? string.Empty;

        SemanticVersionUtils local = new SemanticVersionUtils(localVersion);
        SemanticVersionUtils remote = new SemanticVersionUtils(remoteVersion);

        return remote > local;
    }

    private async Task<ReleaseDefinition?> GetRepositoryLatestReleaseAsync(string owner, string repo)
    {
        var gitHubApiReleasesLatest = new Uri($"https://api.github.com/repos/{owner}/{repo}/releases/latest");

        try
        {
            var response = await _httpClient.GetAsync(gitHubApiReleasesLatest);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ReleaseDefinition>(json, _jsonSerializeOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Assets.Resources.FailedDownloadReleaseDefinition, gitHubApiReleasesLatest);
            return null;
        }
    }
}