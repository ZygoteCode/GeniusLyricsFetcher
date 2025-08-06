using System.Text;
using System.Text.RegularExpressions;

namespace FUDChromeDriver
{
    public class Patcher
    {
        private string? _driverExecutablePath;
        public Patcher(string? driverExecutablePath = null)
        {
            _driverExecutablePath = driverExecutablePath;
        }

        public void Auto()
        {
            if (_driverExecutablePath == null)
            {
                throw new Exception("Parameter driverExecutablePath is required.");
            }

            if (!isBinaryPatched())
            {
                patchExe();
            }
        }

        private bool isBinaryPatched()
        {
            if (_driverExecutablePath == null)
            {
                throw new Exception("Parameter driverExecutablePath is required.");
            }

            using (FileStream fs = new FileStream(_driverExecutablePath, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(fs, Encoding.GetEncoding("ISO-8859-1")))
            {
                while (true)
                {
                    var line = reader.ReadLine();

                    if (line == null)
                    {
                        break;
                    }

                    if (line.Contains("undetected chromedriver"))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        private void patchExe()
        {
            if (_driverExecutablePath == null)
            {
                throw new Exception("Parameter driverExecutablePath is required.");
            }

            using (FileStream fs = new FileStream(_driverExecutablePath, FileMode.Open, FileAccess.ReadWrite))
            {
                byte[] buffer = new byte[1024];
                StringBuilder stringBuilder = new StringBuilder();
                int read = 0;

                while (true)
                {
                    read = fs.Read(buffer, 0, buffer.Length);

                    if (read == 0)
                    {
                        break;
                    }

                    stringBuilder.Append(Encoding.GetEncoding("ISO-8859-1").GetString(buffer, 0, read));
                }

                string? content = stringBuilder.ToString();
                Match? match = Regex.Match(content.ToString(), @"\{window\.cdc.*?;\}");

                if (match.Success)
                {
                    string target = match.Value;
                    string newTarget = "{console.log(\"undetected chromedriver 1337!\")}".PadRight(target.Length, ' ');
                    string? newContent = content.Replace(target, newTarget);

                    fs.Seek(0, SeekOrigin.Begin);
                    byte[]? bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(newContent);
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }
    }
}