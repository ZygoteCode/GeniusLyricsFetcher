using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace FUDChromeDriver
{
    public class UndetectedChromeDriver : ChromeDriver
    {
        private UndetectedChromeDriver(ChromeDriverService service, ChromeOptions options,
            TimeSpan commandTimeout) : base(service, options, commandTimeout) { }

        private static readonly TimeSpan DefaultPageLoadTimeout = TimeSpan.FromSeconds(30);
        private static readonly TimeSpan DefaultElementLoadTimeout = TimeSpan.FromSeconds(15);
        private static readonly string Code = @"!function(){try{Object.defineProperty(navigator,""languages"",{get:()=>[""it-IT"",""it"",""en-US"",""en""]})}catch(e){console.warn(""Failed to spoof navigator.languages:"",e)}try{const e=[{type:""application/pdf"",description:""Portable Document Format"",suffixes:""pdf"",enabledPlugin:{name:""Chrome PDF Viewer""}},{type:""application/x-google-chrome-pdf"",suffixes:""pdf"",description:""Portable Document Format"",enabledPlugin:{name:""Chrome PDF Plugin""}},{type:""application/x-nacl"",suffixes:"""",description:""Native Client Executable"",enabledPlugin:{name:""Native Client""}},{type:""application/x-pnacl"",suffixes:"""",description:""Portable Native Client Executable"",enabledPlugin:{name:""Native Client""}}];e.forEach((t=>{t.enabledPlugin=e.find((e=>e.name===t.enabledPlugin.name))})),Object.defineProperty(navigator,""mimeTypes"",{get:()=>e})}catch(e){console.warn(""Failed to spoof navigator.mimeTypes:"",e)}try{const e=WebGLRenderingContext.prototype.getParameter;WebGLRenderingContext.prototype.getParameter=function(t){return 37445===t?""Intel Inc."":37446===t?""Intel Iris OpenGL Engine"":this.getContextAttributes()&&this.getContextAttributes().failIfMajorPerformanceCaveat&&t===this.RENDERER?""Google SwiftShader"":e.call(this,t)}}catch(e){console.warn(""Failed to spoof WebGLRenderingContext.getParameter:"",e)}try{AnalyserNode.prototype.getFloatFrequencyData;AnalyserNode.prototype.getFloatFrequencyData=function(e){for(let t=0;t<e.length;t++)e[t]=.5*Math.random()-140};const e=AudioBuffer.prototype.getChannelData;AudioBuffer.prototype.getChannelData=function(t){const o=e.call(this,t);for(let e=0;e<o.length;e++)o[e]+=1e-4*(Math.random()-.5);return o}}catch(e){console.warn(""Failed to spoof AudioContext methods:"",e)}try{const e=[4,8,12,16],t=[4,8,16];Object.defineProperty(navigator,""hardwareConcurrency"",{get:()=>e[Math.floor(Math.random()*e.length)]}),Object.defineProperty(navigator,""deviceMemory"",{get:()=>t[Math.floor(Math.random()*t.length)]})}catch(e){console.warn(""Failed to spoof hardwareConcurrency/deviceMemory:"",e)}try{Object.defineProperty(navigator,""connection"",{get:()=>({downlink:(50*Math.random()+50).toFixed(2),effectiveType:""4g"",rtt:Math.floor(50*Math.random()+25),saveData:!1,onchange:null})})}catch(e){console.warn(""Failed to spoof navigator.connection:"",e)}try{const e=[1,1.25,1.5,2];Object.defineProperty(window,""devicePixelRatio"",{get:()=>e[Math.floor(Math.random()*e.length)]})}catch(e){console.warn(""Failed to spoof window.devicePixelRatio:"",e)}try{navigator.getBattery=()=>Promise.resolve({charging:Math.random()>.3,chargingTime:Math.random()>.5?Math.floor(3600*Math.random()):0,dischargingTime:Math.random()>.5?Math.floor(1e4*Math.random()+3600):1/0,level:parseFloat((.49*Math.random()+.51).toFixed(2)),onchargingchange:null,onchargingtimechange:null,ondischargingtimechange:null,onlevelchange:null})}catch(e){console.warn(""Failed to spoof navigator.getBattery:"",e)}try{Object.defineProperty(performance,""memory"",{get:()=>({jsHeapSizeLimit:1073741824*Math.floor(1*Math.random()+1),totalJSHeapSize:1048576*Math.floor(30*Math.random()+20),usedJSHeapSize:1048576*Math.floor(20*Math.random()+5)})})}catch(e){console.warn(""Failed to spoof performance.memory:"",e)}try{void 0!==window.chrome&&(window.chrome={...window.chrome,app:{isInstalled:!1,InstallState:{DISABLED:""disabled"",INSTALLED:""installed"",NOT_INSTALLED:""not_installed""},RunningState:{CANNOT_RUN:""cannot_run"",READY_TO_RUN:""ready_to_run"",RUNNING:""running""}},runtime:{id:Array(32).fill(null).map((()=>Math.floor(16*Math.random()).toString(16))).join(""""),connect:()=>({}),sendMessage:()=>({}),onMessage:{addListener:()=>{}},onInstalled:{addListener:()=>{}},getManifest:()=>({manifest_version:2,name:""Test Extension""})},loadTimes:()=>({commitLoadTime:performance.now()-1e3*Math.random(),connectionInfo:""http/1.1"",finishDocumentLoadTime:performance.now()-100*Math.random(),finishLoadTime:performance.now()-50*Math.random(),firstPaintTime:performance.now()-500*Math.random(),navigationType:""Other"",npnNegotiatedProtocol:""unknown"",requestTime:performance.now()-2e3*Math.random(),startLoadTime:performance.now()-2e3*Math.random(),wasFetchedViaSpdy:!1,wasNpnNegotiated:!1}),csi:()=>({startE:performance.now()-2e3*Math.random(),onloadT:performance.now()-100*Math.random(),pageT:1e4*Math.random(),tran:15})})}catch(e){console.warn(""Failed to spoof window.chrome object:"",e)}try{if([""$cdc_asdjflasutopfhvcZLmcfl_"",""$chrome_asyncScriptInfo"",""$cdc_adoQpoLmvpĲ$"",""__driver_evaluate"",""__webdriver_evaluate"",""__selenium_evaluate"",""__fxdriver_evaluate"",""__driver_unwrapped"",""__webdriver_unwrapped"",""__selenium_unwrapped"",""__fxdriver_unwrapped"",""_Selenium_IDE_Recorder"",""_selenium"",""calledSelenium"",""_WEBDRIVER_ELEM_CACHE"",""ChromeDriverwInjector"",""webdriver"",""driver"",""__webdriver_script_fn"",""__webdriver_script_func"",""__webdriver_script_executed"",""__webdriverFunc"",""__webdriver_evaluate"",""__selenium_evaluate"",""__driver_evaluate""].forEach((e=>{window[e]&&delete window[e],document[e]&&delete document[e]})),document.documentElement){const e=Element.prototype.getAttribute;Element.prototype.getAttribute=function(t){return""webdriver""===t&&this===document.documentElement?null:e.call(this,t)}}}catch(e){console.warn(""Failed to remove CDC properties:"",e)}try{navigator.permissions.query;Object.defineProperty(navigator,""permissions"",{get:()=>({query:e=>{if(""notifications""===e.name)return Promise.resolve({state:Notification.permission||""prompt""});const t=[""granted"",""prompt""];return Promise.resolve({state:t[Math.floor(Math.random()*t.length)],onchange:null})}})})}catch(e){console.warn(""Failed to spoof navigator.permissions:"",e)}try{window.console.debug=(...e)=>{},window.console.info=(...e)=>{},window.console.warn=(...e)=>{}}catch(e){console.warn(""Failed to spoof console methods:"",e)}try{const e=Function.prototype.toString;Function.prototype.toString=function(){navigator.webdriver,WebGLRenderingContext.prototype.getParameter,HTMLCanvasElement.prototype.toDataURL,CanvasRenderingContext2D.prototype.getImageData,AnalyserNode.prototype.getFloatFrequencyData,AudioBuffer.prototype.getChannelData,navigator.permissions.query;return this.name&&(""getParameter""===this.name||""toDataURL""===this.name||""getImageData""===this.name||""getFloatFrequencyData""===this.name||""getChannelData""===this.name||""query""===this.name&&this.toString().includes(""parameters.name === 'notifications'""))?`function ${this.name||""""}() { [native code] }`:this===Object.getOwnPropertyDescriptor(Navigator.prototype,""webdriver"")?.get?""function get webdriver() { [native code] }"":e.call(this)},Function.prototype.toString.toString=()=>""function toString() { [native code] }""}catch(e){console.warn(""Failed to spoof Function.prototype.toString:"",e)}try{Object.defineProperty(Intl,""DateTimeFormat"",{value:new Proxy(Intl.DateTimeFormat,{construct(e,t){let o=t[0];const n=t[1];(!o||Math.random()>.7)&&(o=Math.random()>.5?""it-IT"":""en-US"");try{return new e(o,n)}catch(t){return new e(""it-IT"",n)}}})})}catch(e){console.warn(""Failed to spoof Intl.DateTimeFormat:"",e)}try{Object.defineProperty(navigator,""mediaDevices"",{get:()=>({enumerateDevices:()=>Promise.resolve([{deviceId:""default"",kind:""audioinput"",label:""Default - Microfono (Built-in Audio)"",groupId:""default""},{deviceId:""audio_device_1"",kind:""audioinput"",label:""Microfono (Realtek High Definition Audio)"",groupId:""groupAudioRealtek""},{deviceId:""default"",kind:""videoinput"",label:""Default - Fotocamera (Built-in Camera)"",groupId:""default""},{deviceId:""video_device_8372"",kind:""videoinput"",label:""Integrated Webcam (USB Video Device)"",groupId:""groupVideoUSB""}]),getUserMedia:e=>e&&(e.audio||e.video)?Promise.resolve({getTracks:()=>[],stop:()=>{}}):Promise.reject(new DOMException(""Permission denied"")),getSupportedConstraints:()=>({width:!0,height:!0,aspectRatio:!0,frameRate:!0,facingMode:!0,resizeMode:!0,sampleRate:!0,sampleSize:!0,echoCancellation:!0,autoGainControl:!0,noiseSuppression:!0,latency:!0,channelCount:!0,deviceId:!0,groupId:!0})})})}catch(e){console.warn(""Failed to spoof navigator.mediaDevices:"",e)}try{Object.defineProperty(window,""speechSynthesis"",{get:()=>({pending:!1,speaking:!1,paused:!1,onvoiceschanged:null,speak:function(e){},cancel:function(){this.pending=!1,this.speaking=!1},pause:function(){this.paused=!0},resume:function(){this.paused=!1},getVoices:()=>[{default:!0,lang:""it-IT"",localService:!0,name:""Google italiano"",voiceURI:""Google italiano""},{default:!1,lang:""en-US"",localService:!0,name:""Google US English"",voiceURI:""Google US English""},{default:!1,lang:""en-GB"",localService:!0,name:""Google UK English Female"",voiceURI:""Google UK English Female""},{default:!1,lang:""es-ES"",localService:!0,name:""Google español"",voiceURI:""Google español""}]})})}catch(e){console.warn(""Failed to spoof window.speechSynthesis:"",e)}try{const e=""Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36"",t=e.substring(e.indexOf(""/"")+1),o=""Win32"";Object.defineProperty(navigator,""userAgent"",{get:()=>e}),Object.defineProperty(navigator,""appVersion"",{get:()=>t}),Object.defineProperty(navigator,""platform"",{get:()=>o}),Object.defineProperty(navigator,""vendor"",{get:()=>""Google Inc.""})}catch(e){console.warn(""Failed to spoof userAgent/platform:"",e)}try{Object.defineProperty(navigator,""doNotTrack"",{get:()=>null})}catch(e){console.warn(""Failed to spoof navigator.doNotTrack:"",e)}try{Object.defineProperty(navigator,""maxTouchPoints"",{get:()=>0})}catch(e){console.warn(""Failed to spoof navigator.maxTouchPoints:"",e)}try{const e=Element.prototype.getBoundingClientRect;Element.prototype.getBoundingClientRect=function(){const t=e.call(this),o=()=>.1*(Math.random()-.5);return DOMRect.fromRect({x:t.x+o(),y:t.y+o(),width:Math.max(0,t.width+o()),height:Math.max(0,t.height+o()),top:t.top+o(),left:t.left+o(),right:t.right+o(),bottom:t.bottom+o()})}}catch(e){console.warn(""Failed to spoof Element.getBoundingClientRect:"",e)}try{const e=Object.getOwnPropertyDescriptor(Error.prototype,""stack"");e&&Object.defineProperty(Error.prototype,""stack"",{configurable:!0,get:function(){return e.get.call(this)}})}catch(e){console.warn(""Failed to spoof Error.prototype.stack:"",e)}try{if(""undefined""!=typeof FontFaceSet&&navigator.platform.startsWith(""Win"")){const e=[""Arial"",""Courier New"",""Georgia"",""Times New Roman"",""Verdana"",""Tahoma""];e.map((e=>new FontFace(e,`local(""${e}"")`)));document.fonts.check=(t,o)=>e.some((e=>t.toLowerCase().includes(e.toLowerCase()))),document.fonts.ready.then((()=>{}))}}catch(e){console.warn(""Failed to spoof document.fonts:"",e)}}();";

        private bool _headless = false;
        private ChromeOptions? _options = null;
        private ChromeDriverService? _service = null;
        private Process? _browser = null;
        private bool _keepUserDataDir = true;
        private string? _userDataDir = null;

        /// <summary>
        /// Creates a new instance of the chrome driver.
        /// </summary>
        /// <param name="options">Used to define browser behavior.</param>
        /// <param name="userDataDir">Set chrome user profile directory.
        /// creates a temporary profile if userDataDir is null,
        /// and automatically deletes it after exiting.</param>
        /// <param name="driverExecutablePath">Set chrome driver executable file path. (patches new binary)</param>
        /// <param name="browserExecutablePath">Set browser executable file path.
        /// default using $PATH to execute.</param>
        /// <param name="port">Set the port used by the chromedriver executable. (not debugger port)</param>
        /// <param name="logLevel">Set chrome logLevel.</param>
        /// <param name="headless">Specifies to use the browser in headless mode.
        /// warning: This reduces undetectability and is not fully supported.</param>
        /// <param name="noSandbox">Set use --no-sandbox, and suppress the "unsecure option" status bar.
        /// this option has a default of true since many people seem to run this as root(....) ,
        /// and chrome does not start when running as root without using --no-sandbox flag.</param>
        /// <param name="suppressWelcome">First launch using the welcome page.</param>
        /// <param name="hideCommandPromptWindow">Hide selenium command prompt window.</param>
        /// <param name="commandTimeout">The maximum amount of time to wait for each command.
        /// default value is 60 seconds.</param>
        /// <param name="prefs">Prefs is meant to store lightweight state that reflects user preferences.
        /// dict value can be value or json.</param>
        /// <param name="configureService">Initialize configuration ChromeDriverService.</param>
        /// <returns>UndetectedChromeDriver</returns>
        public static UndetectedChromeDriver Create(
            ChromeOptions? options = null,
            string? userDataDir = null,
            string? driverExecutablePath = null,
            string? browserExecutablePath = null,
            int port = 0,
            int logLevel = 0,
            bool headless = false,
            bool noSandbox = true,
            bool suppressWelcome = true,
            bool hideCommandPromptWindow = false,
            TimeSpan? commandTimeout = null,
            Dictionary<string, object>? prefs = null,
            Action<ChromeDriverService>? configureService = null)
        {
            if (driverExecutablePath == null)
            {
                throw new Exception("Parameter driverExecutablePath is required.");
            }

            Patcher patcher = new Patcher(driverExecutablePath);
            patcher.Auto();

            if (options == null)
            {
                options = new ChromeOptions();
            }

            if (options.DebuggerAddress != null)
            {
                throw new Exception("Options is already used, please create new ChromeOptions.");
            }

            string? debugHost = "127.0.0.1";
            int debugPort = FindFreePort();

            options.AddArgument($"--remote-debugging-host={debugHost}");
            options.AddArgument($"--remote-debugging-port={debugPort}");
            options.DebuggerAddress = $"{debugHost}:{debugPort}";

            bool keepUserDataDir = true;
            string? userDataDirArg = options.Arguments
                .Select(it => Regex.Match(it,
                    @"(?:--)?user-data-dir(?:[ =])?(.*)"))
                .Select(it => it.Groups[1].Value)
                .FirstOrDefault(it => !string.IsNullOrEmpty(it));

            if (userDataDirArg != null)
            {
                userDataDir = userDataDirArg;
            }
            else
            {
                if (userDataDir == null)
                {
                    keepUserDataDir = false;
                    userDataDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                }

                options.AddArgument($"--user-data-dir={userDataDir}");
            }

            string? language = CultureInfo.CurrentCulture.Name;

            if (!options.Arguments.Any(it => it.Contains("--lang")))
            {
                options.AddArgument($"--lang={language}");
            }

            if (browserExecutablePath == null)
            {
                ChromeExecutable executable = new ChromeExecutable();
                browserExecutablePath = executable.GetExecutablePath();

                if (browserExecutablePath == null)
                {
                    throw new Exception("Not found chrome.exe.");
                }
            }

            options.BinaryLocation = browserExecutablePath;

            if (suppressWelcome)
            {
                options.AddArguments("--no-default-browser-check", "--no-first-run");
            }

            if (noSandbox)
            {
                options.AddArguments("--no-sandbox", "--test-type");
            }

            if (headless)
            {
                try
                {
                    ChromeDriverInstaller installer = new ChromeDriverInstaller();
                    string? version = installer.GetDriverVersion(driverExecutablePath).GetAwaiter().GetResult();
                    string? versionMain = version.Substring(0, version.IndexOf('.'));

                    if (int.Parse(versionMain) < 108)
                    {
                        options.AddArguments("--headless=chrome");
                    }
                    else
                    {
                        options.AddArguments("--headless=new");
                    }
                }
                catch
                {
                    options.AddArguments("--headless=new");
                }
            }

            options.AddArguments("--window-size=1920,1080");
            options.AddArguments("--start-maximized");
            options.AddArguments($"--log-level={logLevel}");

            if (prefs != null)
            {
                HandlePrefs(userDataDir, prefs);
            }

            try
            {
                string filePath = Path.Combine(userDataDir, @"Default/Preferences");
                string json = File.ReadAllText(filePath,
                    Encoding.GetEncoding("ISO-8859-1"));
                Regex regex = new Regex(@"(?<=exit_type"":)(.*?)(?=,)");
                string exitType = regex.Match(json).Value;
                if (exitType != "" && exitType != "null")
                {
                    json = regex.Replace(json, "null");
                    File.WriteAllText(filePath, json,
                        Encoding.GetEncoding("ISO-8859-1"));
                }
            }
            catch
            {

            }

            string args = options.Arguments
                .Select(it => it.Trim())
                .Aggregate("", (r, it) => r + " " +
                    (it.Contains(" ") ? $"\"{it}\"" : it));

            ProcessStartInfo info = new ProcessStartInfo(options.BinaryLocation, args);
            info.UseShellExecute = false;
            info.RedirectStandardInput = true;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            Process? browser = Process.Start(info);

            if (browser == null)
            {
                throw new Exception("Browser process start error.");
            }

            try
            {
                ChromeDriverService? service = ChromeDriverService.CreateDefaultService(
                    Path.GetDirectoryName(driverExecutablePath),
                    Path.GetFileName(driverExecutablePath));

                service.HideCommandPromptWindow = hideCommandPromptWindow;

                if (port != 0)
                {
                    service.Port = port;
                }

                if (configureService != null)
                {
                    configureService(service);
                }

                if (commandTimeout == null)
                {
                    commandTimeout = TimeSpan.FromSeconds(60);
                }

                UndetectedChromeDriver driver = new UndetectedChromeDriver(service, options, commandTimeout.Value);

                driver._headless = headless;
                driver._options = options;
                driver._service = service;
                driver._browser = browser;
                driver._keepUserDataDir = keepUserDataDir;
                driver._userDataDir = userDataDir;

                return driver;
            }
            catch
            {
                DisposeBrowser(browser, userDataDir, keepUserDataDir);
                throw;
            }
        }

        public void GoToUrl(string url)
        {
            if (_headless)
            {
                ConfigureAntiDetection();
                ConfigureHeadless();
            }
            else
            {
                ConfigureAntiDetection();
            }

            Navigate().GoToUrl(url);
        }

        private void ConfigureAntiDetection()
        {
            ExecuteCdpCommand(
                    "Page.addScriptToEvaluateOnNewDocument",
                    new Dictionary<string, object>
                    {
                        ["source"] = Code
                    });
        }

        private void ConfigureHeadless()
        {
            if (ExecuteScript("return navigator.webdriver") != null)
            {
                ExecuteCdpCommand(
                    "Page.addScriptToEvaluateOnNewDocument",
                    new Dictionary<string, object>
                    {
                        ["source"] =
                        @"
                            Object.defineProperty(window, ""navigator"", {
                                Object.defineProperty(window, ""navigator"", {
                                  value: new Proxy(navigator, {
                                    has: (target, key) => (key === ""webdriver"" ? false : key in target),
                                    get: (target, key) =>
                                      key === ""webdriver""
                                        ? false
                                        : typeof target[key] === ""function""
                                        ? target[key].bind(target)
                                        : target[key],
                                  }),
                                });
                         "
                    });
                ExecuteCdpCommand(
                    "Network.setUserAgentOverride",
                    new Dictionary<string, object>
                    {
                        ["userAgent"] =
                        ((string)ExecuteScript(
                            "return navigator.userAgent"
                        )).Replace("Headless", "")
                    });
                ExecuteCdpCommand(
                    "Page.addScriptToEvaluateOnNewDocument",
                    new Dictionary<string, object>
                    {
                        ["source"] =
                        @"
                            Object.defineProperty(navigator, 'maxTouchPoints', {get: () => 1});
                            Object.defineProperty(navigator.connection, 'rtt', {get: () => 100});

                            window.chrome = {
                                app: {
                                    isInstalled: false,
                                    InstallState: {
                                        DISABLED: 'disabled',
                                        INSTALLED: 'installed',
                                        NOT_INSTALLED: 'not_installed'
                                    },
                                    RunningState: {
                                        CANNOT_RUN: 'cannot_run',
                                        READY_TO_RUN: 'ready_to_run',
                                        RUNNING: 'running'
                                    }
                                },
                                runtime: {
                                    OnInstalledReason: {
                                        CHROME_UPDATE: 'chrome_update',
                                        INSTALL: 'install',
                                        SHARED_MODULE_UPDATE: 'shared_module_update',
                                        UPDATE: 'update'
                                    },
                                    OnRestartRequiredReason: {
                                        APP_UPDATE: 'app_update',
                                        OS_UPDATE: 'os_update',
                                        PERIODIC: 'periodic'
                                    },
                                    PlatformArch: {
                                        ARM: 'arm',
                                        ARM64: 'arm64',
                                        MIPS: 'mips',
                                        MIPS64: 'mips64',
                                        X86_32: 'x86-32',
                                        X86_64: 'x86-64'
                                    },
                                    PlatformNaclArch: {
                                        ARM: 'arm',
                                        MIPS: 'mips',
                                        MIPS64: 'mips64',
                                        X86_32: 'x86-32',
                                        X86_64: 'x86-64'
                                    },
                                    PlatformOs: {
                                        ANDROID: 'android',
                                        CROS: 'cros',
                                        LINUX: 'linux',
                                        MAC: 'mac',
                                        OPENBSD: 'openbsd',
                                        WIN: 'win'
                                    },
                                    RequestUpdateCheckStatus: {
                                        NO_UPDATE: 'no_update',
                                        THROTTLED: 'throttled',
                                        UPDATE_AVAILABLE: 'update_available'
                                    }
                                }
                            }

                            // https://github.com/microlinkhq/browserless/blob/master/packages/goto/src/evasions/navigator-permissions.js
                            if (!window.Notification) {
                                window.Notification = {
                                    permission: 'denied'
                                }
                            }

                            const originalQuery = window.navigator.permissions.query
                            window.navigator.permissions.__proto__.query = parameters =>
                                parameters.name === 'notifications'
                                    ? Promise.resolve({ state: window.Notification.permission })
                                    : originalQuery(parameters)

                            const oldCall = Function.prototype.call
                            function call() {
                                return oldCall.apply(this, arguments)
                            }
                            Function.prototype.call = call

                            const nativeToStringFunctionString = Error.toString().replace(/Error/g, 'toString')
                            const oldToString = Function.prototype.toString

                            function functionToString() {
                                if (this === window.navigator.permissions.query) {
                                    return 'function query() { [native code] }'
                                }
                                if (this === functionToString) {
                                    return nativeToStringFunctionString
                                }
                                return oldCall.call(oldToString, this)
                            }
                            // eslint-disable-next-line
                            Function.prototype.toString = functionToString
                         "
                    });
            }
        }

        /// <summary>
        /// This can be useful in case of heavy detection methods.
        /// -stops the chromedriver service which runs in the background
        /// -starts the chromedriver service which runs in the background
        /// -recreate session
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task Reconnect(int timeout = 100)
        {
            if (_service == null)
            {
                throw new Exception("ChromeDriverService cannot be null.");
            }

            MethodInfo? methodInfo = typeof(DriverService).GetMethod("Stop", BindingFlags.NonPublic | BindingFlags.Instance);

            if (methodInfo == null)
            {
                throw new Exception(@"Not found ChromeDriverService.Stop method.");
            }

            try
            {
                methodInfo.Invoke(_service, new object[] { });
            }
            catch
            {

            }

            await Task.Delay(timeout);

            try
            {
                _service.Start();
            }
            catch
            {

            }

            try
            {
                StartSession();
            }
            catch
            {

            }
        }

        /// <summary>
        /// Differentiates from the regular method in that it does not
        /// require a capabilities argument.The capabilities are automatically
        /// recreated from the options at creation time.
        /// </summary>
        /// <param name="capabilities"></param>
        /// <exception cref="Exception"></exception>
        public new void StartSession(ICapabilities? capabilities = null)
        {
            if (_options == null)
            {
                throw new Exception("ChromeOptions cannot be null.");
            }

            if (capabilities == null)
            {
                capabilities = _options.ToCapabilities();
            }

            base.StartSession(capabilities);
        }

        private static int FindFreePort()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                IPEndPoint? localEP = new IPEndPoint(IPAddress.Any, 0);
                socket.Bind(localEP);
                IPEndPoint? freeEP = (IPEndPoint?)socket.LocalEndPoint;

                if (freeEP == null)
                {
                    throw new Exception("Not found free port.");
                }

                return freeEP.Port;
            }
            finally
            {
                socket.Close();
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                DisposeBrowser(_browser, _userDataDir, _keepUserDataDir);
            }
        }

        private static void DisposeBrowser(Process? browser, string? userDataDir, bool keepUserDataDir)
        {
            try
            {
                if (browser != null)
                {
                    browser.Kill();
                    browser.Dispose();
                }
            }
            catch
            {

            }

            if (!keepUserDataDir)
            {
                for (int i = 0; i < 5; i++)
                {
                    try
                    {
                        if (userDataDir != null)
                        {
                            if (Directory.Exists(userDataDir))
                            {
                                Directory.Delete(userDataDir, true);
                            }
                        }

                        break;
                    }
                    catch
                    {
                        Thread.Sleep(100);
                    }
                }
            }
        }

        private static void HandlePrefs(string userDataDir, Dictionary<string, object> prefs)
        {
            string defaultPath = Path.Combine(userDataDir, "Default");

            if (!Directory.Exists(defaultPath))
            {
                Directory.CreateDirectory(defaultPath);
            }

            Dictionary<string, object> newPrefs = new Dictionary<string, object>();
            string prefsFile = Path.Combine(defaultPath, "Preferences");

            if (File.Exists(prefsFile))
            {
                using (FileStream fs = File.Open(prefsFile, FileMode.Open, FileAccess.Read))
                using (StreamReader reader = new StreamReader(fs, Encoding.GetEncoding("ISO-8859-1")))
                {
                    try
                    {
                        string json = reader.ReadToEnd();
                        newPrefs = Json.DeserializeData(json);
                    }
                    catch
                    {

                    }
                }
            }

            void UndotMerge(string key, object value, Dictionary<string, object> dict)
            {
                if (key.Contains("."))
                {
                    string[] split = key.Split(new char[] { '.' }, 2);
                    string k1 = split[0];
                    string k2 = split[1];

                    if (!dict.ContainsKey(k1))
                    {
                        dict[k1] = new Dictionary<string, object>();
                    }

                    UndotMerge(k2, value, (Dictionary<string, object>)dict[k1]);
                    return;
                }

                dict[key] = value;
            }

            try
            {
                foreach (KeyValuePair<string, object> pair in prefs)
                {
                    UndotMerge(pair.Key, pair.Value, newPrefs);
                }
            }
            catch
            {
                throw new Exception("Prefs merge faild.");
            }

            using (FileStream fs = File.Open(prefsFile, FileMode.OpenOrCreate, FileAccess.Write))
            using (StreamWriter writer = new StreamWriter(fs, Encoding.GetEncoding("ISO-8859-1")))
            {
                string json = JsonConvert.SerializeObject(newPrefs);
                writer.Write(json);
            }
        }

        private object? ExecuteScriptSafely(string script, params object[] args)
        {
            try
            {
                return ExecuteScript(script, args);
            }
            catch
            {
                return null;
            }
        }

        public bool IsPageContentLoaded()
        {
            object? result = ExecuteScriptSafely("return (document.readyState == 'complete' || document.readyState == 'interactive')");
            return result != null && (bool)result;
        }

        public bool IsAjaxRequestCompleted()
        {
            object? result = ExecuteScriptSafely("var result = true; try { if (typeof jQuery != 'undefined') { result = jQuery.active == 0; } } catch (e) { result = false; }; return result;");
            return result != null && (bool)result;
        }

        public bool AreAnimationsComplete()
        {
            object? result = ExecuteScriptSafely(@"return document.querySelectorAll('.ng-animating, .busy, [class*='-active'], [class*='-entering'], [class*='-leaving'], [class*='-moving'], [class*='-shifting'], [class*='-transitioning'], [class*='-animate'], [class*='-in'], [class*='-out']').length === 0;");
            return result != null && (bool)result;
        }

        public void WaitForPageReady(TimeSpan? timeout = null)
        {
            TimeSpan effectiveTimeout = timeout ?? DefaultPageLoadTimeout;
            WebDriverWait wait = new WebDriverWait(this, effectiveTimeout);

            try
            {
                wait.Until(drv =>
                {
                    bool pageLoaded = IsPageContentLoaded();
                    bool ajaxLoaded = IsAjaxRequestCompleted();
                    bool animationsComplete = AreAnimationsComplete();

                    return pageLoaded && ajaxLoaded && animationsComplete;
                });
            }
            catch
            {
                throw new WebDriverTimeoutException($"The page didn't became ready after {effectiveTimeout.TotalSeconds} seconds.");
            }
        }

        public void MaximizeWindow()
        {
            try
            {
                Manage().Window.Maximize();
            }
            catch
            {

            }
        }

        public bool IsElementReady(IWebElement element)
        {
            try
            {
                if (element == null)
                {
                    return false;
                }

                bool isPresentInDom = true;

                try
                {
                    var tag = element.TagName;
                }
                catch
                {
                    isPresentInDom = false;
                }

                return isPresentInDom && element.Displayed && element.Enabled;
            }
            catch
            {
                return false;
            }
        }

        public IWebElement FindClickableElement(By locator, TimeSpan? timeout = null)
        {
            TimeSpan effectiveTimeout = timeout ?? DefaultElementLoadTimeout;
            WebDriverWait wait = new WebDriverWait(this, effectiveTimeout);

            try
            {
                return wait.Until(drv =>
                {
                    try
                    {
                        IWebElement element = drv.FindElement(locator);
                        return element.Displayed && element.Enabled ? element : null;
                    }
                    catch
                    {
                        return null;
                    }
                });
            }
            catch
            {
                throw new NoSuchElementException($"Element with locator'{locator}' not found or not clickable within {effectiveTimeout.TotalSeconds} seconds.");
            }
        }

        public IWebElement FindVisibleElement(By locator, TimeSpan? timeout = null)
        {
            TimeSpan effectiveTimeout = timeout ?? DefaultElementLoadTimeout;
            WebDriverWait wait = new WebDriverWait(this, effectiveTimeout);

            try
            {
                return wait.Until(drv =>
                {
                    try
                    {
                        IWebElement element = drv.FindElement(locator);
                        return element.Displayed ? element : null;
                    }
                    catch
                    {
                        return null;
                    }
                });
            }
            catch
            {
                throw new NoSuchElementException($"Element with locator'{locator}' not found or not visible within {effectiveTimeout.TotalSeconds} seconds.");
            }
        }

        public ReadOnlyCollection<IWebElement> FindLoadedElements(By locator, TimeSpan? timeout = null)
        {
            TimeSpan effectiveTimeout = timeout ?? DefaultElementLoadTimeout;
            WebDriverWait wait = new WebDriverWait(this, effectiveTimeout);

            try
            {
                return wait.Until(drv =>
                {
                    ReadOnlyCollection<IWebElement> elements = drv.FindElements(locator);
                    return elements.Count > 0 ? elements : null;
                });
            }
            catch
            {
                throw new WebDriverTimeoutException($"No element with locator '{locator}' found within {effectiveTimeout.TotalSeconds} seconds.");
            }
        }

        public IWebElement FindClickableElementByTagValue(string tagName, string tagValue, TimeSpan? timeout = null)
        {
            return FindClickableElement(By.CssSelector($"[{tagName}=\"{tagValue}\"]"), timeout);
        }

        public string WaitForAndHandleAlert(bool acceptAlert = true, TimeSpan? timeout = null)
        {
            TimeSpan effectiveTimeout = timeout ?? TimeSpan.FromSeconds(10);
            WebDriverWait wait = new WebDriverWait(this, effectiveTimeout);

            try
            {
                IAlert alert = wait.Until(drv =>
                {
                    try
                    {
                        return drv.SwitchTo().Alert();
                    }
                    catch
                    {
                        return null;
                    }
                });

                string alertText = alert.Text;

                if (acceptAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }

                return alertText;
            }
            catch
            {
                return null;
            }
        }

        public bool WaitForElementToDisappear(By locator, TimeSpan? timeout = null)
        {
            TimeSpan effectiveTimeout = timeout ?? DefaultElementLoadTimeout;
            WebDriverWait wait = new WebDriverWait(this, effectiveTimeout);

            try
            {
                return wait.Until(drv =>
                {
                    try
                    {
                        IWebElement element = drv.FindElement(locator);
                        return !element.Displayed;
                    }
                    catch (NoSuchElementException)
                    {
                        return true;
                    }
                    catch (StaleElementReferenceException)
                    {
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                });
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine($"The element with locator '{locator}' remained visible or present after {effectiveTimeout.TotalSeconds} seconds.");
                return false;
            }
        }
    }
}