## 发布方式

##### 1. Exe打包

使用Visual Studio 2026 打开WutheringWavesTool.slnx方案文件，按照以下路径打开项目列表

Haiyu --> Properties --> PublishProfiles --> win-x64.pubxml，找到WindowsPackageType节点，确保节点数据生效而并非被注释

如以下示例：

```xml
<!--<WindowsPackageType>None</WindowsPackageType>-->
<!--当出现以上得注释文字时候需要把注释去掉-->

<!--变成下面的文字-->
<WindowsPackageType>None</WindowsPackageType>
```

回到../src/WutheringWavesTool文件夹中，找到 `Build_NoPackage.ps1` 文件，右键选择在PowerShell中运行，等待运行结果完毕，就生成了一个绿色可直接启动的程序。

打包结果路径为：`src/WutheringWavesTool/bin/win-x64/publish`

##### 2. Msix打包

Msix打包需要微软签名证书，在此处不提供，也可进行自签名运行，详细请看[SignTool.exe（签名工具） - .NET Framework | Microsoft Learn](https://learn.microsoft.com/zh-cn/dotnet/framework/tools/signtool-exe) 。


