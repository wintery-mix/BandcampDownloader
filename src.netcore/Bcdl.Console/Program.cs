// https://weneverlearnedtolive.bandcamp.com/album/ode
// https://weneverlearnedtolive.bandcamp.com/track/shadows-in-hibernation-2
// var url = @"https://f4.bcbits.com/img/a0405819805_0.jpg";
// var fileInfo = new FileInfo(@"C:\Users\Leito\Desktop\test.jpg");

// var downloadFileService = new DownloadFileService();
// await downloadFileService.DownloadFileAsync(url, fileInfo).ConfigureAwait(false);

using Bcdl.Console;
using Bcdl.Downloader.Services;
using Serilog;

AppDomain.CurrentDomain.UnhandledException += Helpers.CurrentDomainOnUnhandledException;

var container = Bootstrap.InitializeContainer();

var albumInfoService = container.GetInstance<IAlbumInfoService>();

var albumUrl = "https://weneverlearnedtolive.bandcamp.com/album/silently-i-threw-them-skyward";
var albumInfo = await albumInfoService.DownloadAlbumInfoAsync(albumUrl, CancellationToken.None).ConfigureAwait(false);

var logger = container.GetInstance<ILogger>();

logger.Information("Logging...");
logger.Debug("AlbumInfo retrieved for album {title}", albumInfo.Title);

Console.WriteLine("Finished");
