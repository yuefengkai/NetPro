
## Checker使用
 [![NuGet](https://img.shields.io/nuget/v/NetPro.Checker.svg)](https://nuget.org/packages/NetPro.Checker)

对Microsoft.AspNetCore.Diagnostics.HealthChecks的强化和redis，mongodb检查的完善

### 使用

#### appsetting.json 

```json
"HealthChecksUI": {
 "HealthChecks": [
 	{
 	 "Name": "HealthList",
 	 "Uri": "/health"
 	}
 ],
 "Webhooks": [],
 "EvaluationTimeOnSeconds": 3600, //检查周期，单位秒
 "MinimumSecondsBetweenFailureNotifications": 60
},
```
#### 启用服务
```csharp
public void ConfigureServices(IServiceCollection services)
{
     var healthbuild = services.AddHealthChecks();
     if (!string.IsNullOrWhiteSpace(mongoDbOptions?.ConnectionString))
         healthbuild.AddMongoDb(mongoDbOptions.ConnectionString, tags: new string[] { "mongodb" });//健康检查mongodb

     healthbuild.AddRedis($redisconnection, name:$"redisTest")//健康检查redis
     //检查其他组件，引用相关nuget即可
     services.AddHealthChecksUI();//添加健康检查UI能力
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    application.UseHealthChecks("/health", new HealthCheckOptions()//健康检查服务地址
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

    application.UseHealthChecksUI(s => s.UIPath = "/ui");//健康检查UI地址

    application.UseCheck(envPath:"/env",infoPath:"/info");//envPath:应用环境地址；infoPath:应用自身信息地址
}
```

#### 访问 /health
得到以下检查结果，可根据此来判断组件健康程度来做下一步处理
```json
{
  "status": "Unhealthy",
  "totalDuration": "00:00:10.0147119",
  "entries": {
    "redis-192.168.66.33:6665": {
      "data": {
        
      },
      "description": "It was not possible to connect to the redis server(s). UnableToConnect on 192.168.66.33:6665/Interactive, Initializing/NotStarted, last: NONE, origin: BeginConnectAsync, outstanding: 0, last-read: 0s ago, last-write: 0s ago, keep-alive: 60s, state: Connecting, mgr: 10 of 10 available, last-heartbeat: never, global: 0s ago, v: 2.1.30.38891",
      "duration": "00:00:10.0141772",
      "exception": "It was not possible to connect to the redis server(s). UnableToConnect on 192.168.66.33:6665/Interactive, Initializing/NotStarted, last: NONE, origin: BeginConnectAsync, outstanding: 0, last-read: 0s ago, last-write: 0s ago, keep-alive: 60s, state: Connecting, mgr: 10 of 10 available, last-heartbeat: never, global: 0s ago, v: 2.1.30.38891",
      "status": "Unhealthy"
    },
    "redis-192.168.66.66:6666": {
      "data": {
        
      },
      "description": "It was not possible to connect to the redis server(s). UnableToConnect on 192.168.66.66:6666/Interactive, Initializing/NotStarted, last: NONE, origin: BeginConnectAsync, outstanding: 0, last-read: 0s ago, last-write: 0s ago, keep-alive: 60s, state: Connecting, mgr: 10 of 10 available, last-heartbeat: never, global: 0s ago, v: 2.1.30.38891",
      "duration": "00:00:10.0141671",
      "exception": "It was not possible to connect to the redis server(s). UnableToConnect on 192.168.66.66:6666/Interactive, Initializing/NotStarted, last: NONE, origin: BeginConnectAsync, outstanding: 0, last-read: 0s ago, last-write: 0s ago, keep-alive: 60s, state: Connecting, mgr: 10 of 10 available, last-heartbeat: never, global: 0s ago, v: 2.1.30.38891",
      "status": "Unhealthy"
    }
  }
}
```
#### 访问 /ui 
可视化查看健康检查
<p align="center">
  <img  src="https://github.com/LeonKou/NetPro/blob/master/docs/images/checkhealth.jpg">
</p>

#### 访问 /env 

将得到应用所在系统的环境参数值，可快速定位问题
```json

  "ProcessId": 11232,
  "ProcessStartTime": "2020-05-25T03:55:55.9760077Z",
  "Hostname": "Leon",
  "EnvironmentVariables": {
    "MSBuildLoadMicrosoftTargetsReadOnly": "true",
    "JAVA_HOME": "C:\\Program Files (x86)\\jdk-14",
    "ThreadedWaitDialogDpiContext": "-4",
    "COMPUTERNAME": "Leon",
    "FPS_BROWSER_APP_PROFILE_STRING": "Internet Explorer",
    "CommonProgramW6432": "C:\\Program Files\\Common Files",
    "HOMEPATH": "\\Users\\Administrator",
    "LOGONSERVER": "\\\\Leon",
    "SESSIONNAME": "Console",
    "ProgramFiles(x86)": "C:\\Program Files (x86)",
    "VSLANG": "2052",
    "USERNAME": "Administrator",
    "ASPNETCORE_URLS": "http://localhost:5001",
    "SystemDrive": "C:",
    "ProgramFiles": "C:\\Program Files",
    "PROCESSOR_LEVEL": "6",
    "OS": "Windows_NT",
    "VisualStudioVersion": "16.0",
    "USERDOMAIN_ROAMINGPROFILE": "Leon",
    "PATHEXT": ".COM;.EXE;.BAT;.CMD;.VBS;.VBE;.JS;.JSE;.WSF;.WSH;.MSC;.PY;.PYW",
    "Path": "C:\\Program Files\\Python38\\Scripts\\;C:\\Program Files\\Python38\\;C:\\Windows\\system32;C:\\Windows;C:\\Windows\\System32\\Wbem;C:\\Windows\\System32\\WindowsPowerShell\\v1.0\\;C:\\Windows\\System32\\OpenSSH\\;C:\\Program Files (x86)\\NVIDIA Corporation\\PhysX\\Common;C:\\Program Files\\NVIDIA Corporation\\NVIDIA NvDLISR;C:\\Program Files\\TortoiseSVN\\bin;C:\\Program Files\\Microsoft SQL Server\\130\\Tools\\Binn\\;C:\\Program Files\\Microsoft SQL Server\\Client SDK\\ODBC\\170\\Tools\\Binn\\;C:\\Program Files (x86)\\Microsoft SQL Server\\150\\DTS\\Binn\\;C:\\Program Files\\Microsoft VS Code\\bin;C:\\Program Files\\TortoiseGit\\bin;C:\\Program Files\\dotnet\\;C:\\ProgramData\\chocolatey\\bin;C:\\Program Files (x86)\\jdk-14\\bin;C:\\Program Files (x86)\\jdk-14\\jre\\bin;C:\\Program Files\\Git\\cmd;F:\\工作代码库\\Alarm\\src\\AlarmProcess\\bin\\Debug\\netcoreapp3.1;D:\\Program Files\\nodejs\\;C:\\Users\\Administrator\\AppData\\Local\\Microsoft\\WindowsApps;C:\\Users\\Administrator\\.dotnet\\tools;C:\\Users\\Administrator\\AppData\\Local\\Programs\\Fiddler;C:\\Users\\Administrator\\Documents\\WindowsPowerShell\\Scripts;C:\\kube;C:\\Users\\Administrator\\AppData\\Roaming\\npm",//所有环境变量值
    "SystemRoot": "C:\\Windows",
    "CommonProgramFiles(x86)": "C:\\Program Files (x86)\\Common Files",
    "ComSpec": "C:\\Windows\\system32\\cmd.exe",
    "OneDrive": "C:\\Users\\Administrator\\OneDrive",
    "ALLUSERSPROFILE": "C:\\ProgramData",
    "HOMEDRIVE": "C:",
    "PROCESSOR_REVISION": "9e0d",
    "PSModulePath": "C:\\Program Files\\WindowsPowerShell\\Modules;C:\\Windows\\system32\\WindowsPowerShell\\v1.0\\Modules",
    "VisualStudioDir": "C:\\Users\\Administrator\\Documents\\Visual Studio 2019",
    "VisualStudioEdition": "Microsoft Visual Studio Enterprise 2019",
    "JMETER_HOME": "D:\\apache-jmeter-5.2.1",
    "PROCESSOR_ARCHITECTURE": "AMD64",
    "ProgramData": "C:\\ProgramData",
    "USERDOMAIN": "Leon",
    "CommonProgramFiles": "C:\\Program Files\\Common Files",
    "LOCALAPPDATA": "C:\\Users\\Administrator\\AppData\\Local",
    "VSSKUEDITION": "Enterprise",
    "OneDriveConsumer": "C:\\Users\\Administrator\\OneDrive",
    "DriverData": "C:\\Windows\\System32\\Drivers\\DriverData",
    "ServiceHubLogSessionKey": "E66AF406",
    "windir": "C:\\Windows",
    "FPS_BROWSER_USER_PROFILE_STRING": "Default",
    "ChocolateyInstall": "C:\\ProgramData\\chocolatey",
    "TMP": "C:\\Users\\AppData\\Local\\Temp",
    "CLASS_PATH": "C:\\Program Files (x86)\\jdk-14\\bin;C:\\Program Files (x86)\\jdk-14\\lib\\dt.jar;C:\\Program Files (x86)\\jdk-14\\lib\\tools.jar;D:\\apache-jmeter-5.2.1\\lib\\ext\\ApacheJMeter_core.jar;D:\\apache-jmeter-5.2.1\\lib\\jorphan.jar;D:\\apache-jmeter-5.2.1\\lib\\logkit-2.0.jar;",
    "TEMP": "C:\\Users\\ADMINI~1\\AppData\\Local\\Temp",
    "USERPROFILE": "C:\\Users\\Administrator",
    "ASPNETCORE_ENVIRONMENT": "Development",
    "VSAPPIDDIR": "C:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\Enterprise\\Common7\\IDE\\",
    "NUMBER_OF_PROCESSORS": "6",
    "ProgramW6432": "C:\\Program Files",
    "PUBLIC": "C:\\Users\\Public",
    "ChocolateyLastPathUpdate": "1615616511616458546",
    "APPDATA": "C:\\Users\\Administrator\\AppData\\Roaming",
    "PkgDefApplicationConfigFile": "C:\\Users\\Administrator\\AppData\\Local\\Microsoft\\VisualStudio\\16.0_258a1b31\\devenv.exe.config",
    "VSAPPIDNAME": "devenv.exe"
  }
}
```

#### 访问 /info 
可得到.netcore应用本身的环境信息，例如appsetting.json；系统环境变量；配置文件所有驱动，主机地址等等

```json
{
  "RequestHeaders": {
    "Connection": "keep-alive",
    "Accept": "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9",
    "Accept-Encoding": "gzip, deflate, br",
    "Accept-Language": "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6",
    "Host": "localhost:5001",
    "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4178.0 Safari/537.36 Edg/85.0.558.0",
    "Upgrade-Insecure-Requests": "1",
    "sec-ch-ua": "\"Chromium\";v=\"85\", \"\\\\Not;A\\\"Brand\";v=\"99\", \"Microsoft Edge\";v=\"85\"",
    "sec-ch-ua-mobile": "?0",
    "Sec-Fetch-Site": "none",
    "Sec-Fetch-Mode": "navigate",
    "Sec-Fetch-User": "?1",
    "Sec-Fetch-Dest": "document"
  },
  "ConfigProviders": [
    "Microsoft.Extensions.Configuration.ChainedConfigurationProvider",
    "JsonConfigurationProvider for 'appsettings.json' (Optional)",
    "JsonConfigurationProvider for 'appsettings.Development.json' (Optional)",
    "EnvironmentVariablesConfigurationProvider",
    "CommandLineConfigurationProvider",
    "JsonConfigurationProvider for 'appsettings.json' (Optional)",
    "JsonConfigurationProvider for 'appsettings.Development.json' (Optional)",
    "EnvironmentVariablesConfigurationProvider",
    "CommandLineConfigurationProvider"
  ],
  "Configs": [
    {
      "Key": "windir",
      "Value": "C:\\Windows"
    },
    {
      "Key": "VSSKUEDITION",
      "Value": "Enterprise"
    },
    {
      "Key": "VSLANG",
      "Value": "2052"
    },
    {
      "Key": "VSAPPIDNAME",
      "Value": "devenv.exe"
    },
    {
      "Key": "VSAPPIDDIR",
      "Value": "C:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\Enterprise\\Common7\\IDE\\"
    },
    {
      "Key": "VisualStudioVersion",
      "Value": "16.0"
    },
    {
      "Key": "VisualStudioEdition",
      "Value": "Microsoft Visual Studio Enterprise 2019"
    },
    {
      "Key": "VisualStudioDir",
      "Value": "C:\\Users\\Administrator\\Documents\\Visual Studio 2019"
    },
    {
      "Key": "VerifySignOption",
      "Value": null
    }]
}
```


