using System.ComponentModel;
using System.Diagnostics;
using Bcdl.Downloader.Helpers;
using Downloader;

namespace Bcdl.Downloader.Services;

public interface IDownloadFileService
{
    Task DownloadFileAsync(string url, FileInfo fileInfo);
}

public sealed class DownloadFileService : IDownloadFileService
{
    public async Task DownloadFileAsync(string url, FileInfo fileInfo)
    {
        var downloadService = new DownloadService();
        downloadService.DownloadStarted += OnDownloadStarted;
        downloadService.DownloadProgressChanged += OnDownloadProgressChanged;
        downloadService.DownloadFileCompleted += OnDownloadFileCompleted;

        await downloadService.DownloadFileTaskAsync(url, fileInfo.FullName).ConfigureAwait(false);
    }

    private static void OnDownloadFileCompleted(object? sender, AsyncCompletedEventArgs e)
    {
        if (e.UserState is not DownloadPackage downloadPackage)
        {
            Debug.Fail($"{nameof(e.UserState)} is not a {typeof(DownloadPackage)}");
            return;
        }

        if (e.Cancelled)
        {
            Console.WriteLine($"Download cancelled for {downloadPackage.Address} - File: {downloadPackage.FileName} - Error: {e.Error}");
        }

        Console.WriteLine($"Download finished for for {downloadPackage.Address} - File: {downloadPackage.FileName}");
    }

    private static void OnDownloadProgressChanged(object? sender, DownloadProgressChangedEventArgs e)
    {
        Console.WriteLine($"Downloading: {e.ReceivedBytesSize} / {e.TotalBytesToReceive} - Average speed: {UnitsHelper.BytesToKiloBytes(e.AverageBytesPerSecondSpeed)} kB/s");
    }

    private static void OnDownloadStarted(object? sender, DownloadStartedEventArgs e)
    {
        Console.WriteLine($"Starting download: expecting to receive {UnitsHelper.BytesToKiloBytes(e.TotalBytesToReceive)} kB to file: {e.FileName}");
    }
}
