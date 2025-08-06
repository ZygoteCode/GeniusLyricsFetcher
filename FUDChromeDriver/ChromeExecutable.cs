using System.Diagnostics;
using System.Runtime.InteropServices;

namespace FUDChromeDriver
{
    internal class ChromeExecutable
    {
        public async Task<string> GetVersion(string? browserExecutablePath = null)
        {
            if (browserExecutablePath == null)
            {
                browserExecutablePath = GetExecutablePath();
            }

            if (browserExecutablePath == null)
            {
                throw new Exception("Not found chrome.exe.");
            }

#if (NET48 || NET47 || NET46 || NET45)
            return await Task.Run(()=>
            {
                return FileVersionInfo.GetVersionInfo(browserExecutablePath).FileVersion
                    ?? throw new Exception("Chrome version not found in chrome.exe.");
            });
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return FileVersionInfo.GetVersionInfo(browserExecutablePath).FileVersion
                    ?? throw new Exception("Chrome version not found in chrome.exe.");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                string args = "--product-version";
                ProcessStartInfo info = new ProcessStartInfo(browserExecutablePath, args);
                info.CreateNoWindow = true;
                info.UseShellExecute = false;
                info.RedirectStandardOutput = true;
                info.RedirectStandardError = true;
                Process? process = Process.Start(info);

                if (process == null)
                {
                    throw new Exception("Process start error.");
                }
                try
                {
                    string output = await process.StandardOutput.ReadToEndAsync();
                    string error = await process.StandardError.ReadToEndAsync();
                    await process.WaitForExitPatchAsync();

                    process.Kill();
                    process.Dispose();

                    if (!string.IsNullOrEmpty(error))
                    {
                        throw new Exception(error);
                    }

                    return output;
                }
                catch
                {
                    process.Dispose();
                    throw;
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                string args = "--version";
                ProcessStartInfo info = new ProcessStartInfo(browserExecutablePath, args);
                info.CreateNoWindow = true;
                info.UseShellExecute = false;
                info.RedirectStandardOutput = true;
                info.RedirectStandardError = true;
                Process? process = Process.Start(info);

                if (process == null)
                {
                    throw new Exception("Process start error.");
                }
                try
                {
                    string output = await process.StandardOutput.ReadToEndAsync();
                    string error = await process.StandardError.ReadToEndAsync();
                    await process.WaitForExitPatchAsync();
                    process.Kill();
                    process.Dispose();

                    if (!string.IsNullOrEmpty(error))
                    {
                        throw new Exception(error);
                    }

                    return output.Replace("Google Chrome ", "");
                }
                catch
                {
                    process.Dispose();
                    throw;
                }
            }
            else
                throw new PlatformNotSupportedException("Your operating system is not supported.");
#endif
        }

        public string? GetExecutablePath()
        {
            string? result = null as string;
#if (NET48 || NET47 || NET46 || NET45)
            result = findChromeExecutable();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                result = FindChromeExecutableWindows();
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                result = FindChromeExecutableLinux();
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                result = FindChromeExecutableMacOS();
            }
#endif
            return result;
        }

        private static string? FindChromeExecutableWindows()
        {
            List<string> candidates = new List<string>();

            foreach (string item in new[] { "PROGRAMFILES", "PROGRAMFILES(X86)", "LOCALAPPDATA", "PROGRAMW6432" })
            {
                foreach (string subitem in new[] { @"Google\Chrome\Application", @"Google\Chrome Beta\Application", @"Google\Chrome Canary\Application" })
                {
                    string? variable = Environment.GetEnvironmentVariable(item);

                    if (variable != null)
                    {
                        candidates.Add(Path.Combine(variable, subitem, "chrome.exe"));
                    }
                }
            }

            foreach (string candidate in candidates)
            {
                if (File.Exists(candidate))
                {
                    return candidate;
                }
            }

            return null;
        }

        private static string? FindChromeExecutableLinux()
        {
            List<string> candidates = new List<string>();
            string? environmentPATH = Environment.GetEnvironmentVariable("PATH");

            if (environmentPATH == null)
            {
                throw new Exception("Not found environment PATH.");
            }

            string[] variables = environmentPATH.Split(Path.PathSeparator);

            foreach (string item in variables)
            {
                foreach (string subitem in new[]
                {
                    "google-chrome",
                    "chromium",
                    "chromium-browser",
                    "chrome",
                    "google-chrome-stable",
                })
                {
                    candidates.Add(Path.Combine(item, subitem));
                }
            }

            foreach (string candidate in candidates)
            {
                if (File.Exists(candidate))
                {
                    return candidate;
                }
            }

            return null;
        }

        private static string? FindChromeExecutableMacOS()
        {
            List<string> candidates = new List<string>();
            string? environmentPATH = Environment.GetEnvironmentVariable("PATH");

            if (environmentPATH == null)
            {
                throw new Exception("Not found environment PATH.");
            }

            string[] variables = environmentPATH.Split(Path.PathSeparator);

            foreach (string item in variables)
            {
                foreach (string subitem in new[]
                {
                    "google-chrome",
                    "chromium",
                    "chromium-browser",
                    "chrome",
                    "google-chrome-stable",
                })
                {
                    candidates.Add(Path.Combine(item, subitem));
                }
            }

            candidates.AddRange(new string[]
            {
                "/Applications/Google Chrome.app/Contents/MacOS/Google Chrome",
                "/Applications/Chromium.app/Contents/MacOS/Chromium"
            });

            foreach (string candidate in candidates)
            {
                if (File.Exists(candidate))
                {
                    return candidate;
                }
            }

            return null;
        }
    }
}