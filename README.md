# FSLib.Extension

## 介绍

FSLib.Extension库是一个用于.NET的扩展函数库，所提供的函数和方法均使用扩展方法引入，包含数以百计的用于日常编写程序时使用的扩展方法。
用法非常简单，依据你的需要引用net35(.net 3.5/3.0)或net4(.net 4.0/4.5)下的dll文件即可。

一般情况下不需要特殊的using即可在代码中使用。

附带的扩展方法的命名空间一般和扩展的对象在同一个命名空间。


## 安装
你有三种方式可以使用本库：

* 直接使用包管理器（Nuget Package Manager）安装（推荐）
* 直接下载已编译的二进制包，并根据需要引用
* 使用源码自行编译


### 1.直接使用包管理器（Nuget Package Manager）安装

在Visual Studio中，打开Nuget包管理器，进入联机选项，搜索 iFish，找到“iFish’s Extension Methods Library”后，安装即可。

### 2.直接下载已编译二进制包

在[FishExtension主页](http://www.fishlee.net/soft/fishextension.net/)上下载压缩包，解压后可见内有针对不同版本framework的dll文件，引用即可。

### 3.使用源码自行编译

在[GitHub的源码主页](https://github.com/iccfish/FSLib.Extension)上，直接下载或克隆后，在Visual Studio中编译再引用，或直接作为项目引用即可。

> 现在的源码已部分应用C#6语法特性，因此需要VisualStudio2015才能安全顺利地编译。
> 由于本库是一个历史沿用时间很久的库，因此比较杂乱，欢迎Fork并修正后Pull Request。

> 自 v1.4 开始，项目已经启用 .NET CORE 的项目格式（project.json），如果您想成功编译所有的源码，可能需要至少 Visual Studio 2015。

### 4.源码&文档

本扩展库源码已托管在GITHUB上。GITHUB仓库主页： [https://github.com/iccfish/FSLib.Extension](https://github.com/iccfish/FSLib.Extension) ，欢迎提交更好的扩展方法。

本扩展方法库API文档参见 [http://docs.fishlee.net/ifish/fslib.extension](http://docs.fishlee.net/ifish/fslib.extension)

### 5.相关网址

* **作者**：木鱼(iFish) 
* 主页：[https://www.fishlee.net/soft/fishextension.net/](https://www.fishlee.net/soft/fishextension.net/)
* 博客：[https://blog.fishlee.net](http://blog.fishlee.net/)
* 微博：[https://weibo.com/imcfish](https://weibo.com/imcfish)
