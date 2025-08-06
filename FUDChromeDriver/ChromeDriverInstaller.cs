using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using static System.Net.WebRequestMethods;

namespace FUDChromeDriver
{
    public class ChromeDriverInstaller
    {
        public async Task<string> Auto(string? browserExecutablePath = null, bool force = false)
        {
            string? version = await new ChromeExecutable().GetVersion(browserExecutablePath);
            return await Install(version, force);
        }

        public async Task<string> Install(string version, bool force = false)
        {
            if (string.IsNullOrWhiteSpace(version))
            {
                throw new Exception("Parameter version is required.");
            }

            version = version.Substring(0, version.LastIndexOf('.'));
            string platform = "", zipName = "", driverName = "", tempPath = "", ext = "";

#if (NET48 || NET47 || NET46 || NET45)
            platform = "win32";
            zipName = $"chromedriver-{platform}.zip";
            driverName = $"chromedriver_{version}.exe";
            tempPath = "AppData/Roaming/UndetectedChromeDriver";
            ext = ".exe";
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                platform = "win32";
                zipName = $"chromedriver-{platform}.zip";
                driverName = $"chromedriver_{version}.exe";
                tempPath = "AppData/Roaming/UndetectedChromeDriver";
                ext = ".exe";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                platform = "linux64";
                zipName = $"chromedriver-{platform}.zip";
                driverName = $"chromedriver_{version}";
                tempPath = ".local/share/UndetectedChromeDriver";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                platform = "mac-x64";
                zipName = $"chromedriver-{platform}.zip";
                driverName = $"chromedriver_{version}";
                tempPath = "Library/Application Support/UndetectedChromeDriver";
            }
            else
            {
                throw new PlatformNotSupportedException("Your operating system is not supported.");
            }
#endif

            string? driverPath = Path.GetFullPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), tempPath, driverName));

            if (!force && System.IO.File.Exists(driverPath))
            {
                return driverPath;
            }

            HttpClient? httpClient = Http.Client;
            HttpResponseMessage? versionResponse = await httpClient.GetAsync(
                "https://googlechromelabs.github.io/chrome-for-testing/latest-patch-versions-per-build.json");

            if (!versionResponse.IsSuccessStatusCode)
            {
                throw new Exception($"ChromeDriver version request failed with status code: {versionResponse.StatusCode}, reason phrase: {versionResponse.ReasonPhrase}");
            }

            string? json = await versionResponse.Content.ReadAsStringAsync();
            Match? match = Regex.Match(json, $@"""{version}"":{{""version"":""(.*?)""");
            string? driverVersion = match.Groups[1].Value;

            if (driverVersion == "")
            {
                throw new Exception($"ChromeDriver version not found for Chrome version {version}");
            }

            string? dirPath = Path.GetDirectoryName(driverPath);

            if (dirPath == null)
            {
                throw new Exception("Get ChromeDriver directory faild.");
            }

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            if (force)
            {
                void deleteDriver()
                {
                    if (System.IO.File.Exists(driverPath))
                    {
                        System.IO.File.Delete(driverPath);
                    }
                }

                try
                {
                    deleteDriver();
                }
                catch
                {
                    try
                    {
                        await forceKillInstances(driverPath);
                        deleteDriver();
                    }
                    catch
                    {

                    }
                }
            }

            string baseUrl = "https://storage.googleapis.com/chrome-for-testing-public";
            HttpResponseMessage? zipResponse = await httpClient.GetAsync($"{baseUrl}/{driverVersion}/{platform}/{zipName}");

            if (!zipResponse.IsSuccessStatusCode)
            {
                throw new Exception($"ChromeDriver download request failed with status code: {zipResponse.StatusCode}, reason phrase: {zipResponse.ReasonPhrase}");
            }

            using (Stream? zipStream = await zipResponse.Content.ReadAsStreamAsync())
            using (ZipArchive zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read))
            using (FileStream fs = new FileStream(driverPath, FileMode.Create))
            {
                string entryName = $"chromedriver-{platform}/chromedriver{ext}";
                ZipArchiveEntry? entry = zipArchive.GetEntry(entryName);

                if (entry == null)
                {
                    throw new Exception($"Not found zip entry {entryName}.");
                }
                using (Stream stream = entry.Open())
                {
                    await stream.CopyToAsync(fs);
                }
            }

#if (NET48 || NET47 || NET46 || NET45)
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                string args = @$"+x ""{driverPath}""";
                ProcessStartInfo info = new ProcessStartInfo("chmod", args);
                info.CreateNoWindow = true;
                info.UseShellExecute = false;
                info.RedirectStandardOutput = true;
                info.RedirectStandardError = true;
                Process process = Process.Start(info);

                if (process == null)
                {
                    throw new Exception("Process start error.");
                }

                try
                {
                    string? error = await process.StandardError.ReadToEndAsync();
                    await process.WaitForExitPatchAsync();
                    process.Kill();
                    process.Dispose();

                    if (!string.IsNullOrEmpty(error))
                    {
                        throw new Exception("Failed to make chromedriver executable.");
                    }
                }
                catch
                {
                    process.Dispose();
                    throw;
                }
            }
#endif
            return driverPath;
        }

        public async Task<string> GetDriverVersion(string driverExecutablePath)
        {
            if (driverExecutablePath == null)
            {
                throw new Exception("Parameter driverExecutablePath is required.");
            }

            string args = "--version";
            ProcessStartInfo info = new ProcessStartInfo(driverExecutablePath, args);

            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;

            Process process = Process.Start(info);

            if (process == null)
            {
                throw new Exception("Process start error.");
            }

            try
            {
                string? version = await process.StandardOutput.ReadToEndAsync();
                string? error = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitPatchAsync();
                process.Kill();
                process.Dispose();

                if (!string.IsNullOrEmpty(error))
                {
                    throw new Exception("Failed to execute driver --version.");
                }

                return version.Split(' ').Skip(1).First();
            }
            catch
            {
                process.Dispose();
                throw;
            }
        }

        private async Task forceKillInstances(string driverExecutablePath)
        {
            string exeName = Path.GetFileName(driverExecutablePath), cmd = "", args = "";

#if (NET48 || NET47 || NET46 || NET45)
            cmd = "taskkill";
            args = $"/f /im {exeName}";
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                cmd = "taskkill";
                args = $"/f /im {exeName}";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                cmd = "kill";
                args = $"-f -9 $(pidof {exeName})";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                cmd = "kill";
                args = $"-f -9 $(pidof {exeName})";
            }
#endif
            ProcessStartInfo info = new ProcessStartInfo(cmd, args);

            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;

            Process process = Process.Start(info);

            if (process == null)
            {
                throw new Exception("Process start error.");
            }

            try
            {
                await process.WaitForExitPatchAsync();
                process.Kill();
                process.Dispose();
            }
            catch
            {
                process.Dispose();
                throw;
            }
        }
    }
}