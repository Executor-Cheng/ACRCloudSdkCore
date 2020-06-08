## ACRCloudSdkCore  
[![NuGet version (ACRCloudSdkCore)](https://img.shields.io/nuget/v/ACRCloudSdkCore.svg?style=flat)](https://www.nuget.org/packages/ACRCloudSdkCore/)  

### Overview
  [ACRCloud](https://www.acrcloud.com/) provides [Automatic Content Recognition](https://www.acrcloud.com/docs/introduction/automatic-content-recognition/) services for [Audio Fingerprinting](https://www.acrcloud.com/docs/introduction/audio-fingerprinting/) based applications such as **[Audio Recognition](https://www.acrcloud.com/music-recognition)** (supports music, video, ads for both online and offline), **[Broadcast Monitoring](https://www.acrcloud.com/broadcast-monitoring)**, **[Second Screen](https://www.acrcloud.com/second-screen-synchronization)**, **[Copyright Protection](https://www.acrcloud.com/copyright-protection-de-duplication)** and etc.  
  This **audio recognition C# SDK** support most of audio / video files.  
>Audio: mp3, wav, m4a, flac, aac, amr, ape, ogg ...<br>
>Video: mp4, mkv, wmv, flv, ts, avi ...

### Requirements
Follow one of the tutorials to create a project and get your host, access_key and access_secret.  

* [How to identify songs by sound](https://www.acrcloud.com/docs/tutorials/identify-music-by-sound/)  
* [How to detect custom audio content by sound](https://www.acrcloud.com/docs/tutorials/identify-audio-custom-content/)  

## Windows Runtime Library
**If you run the SDK on Windows, you must install this library.**  
x86: [download and install Library(vcredist_x86.exe)](https://www.microsoft.com/en-us/download/details.aspx?id=5555)  
x64: [download and install Library(vcredist_x64.exe)](https://www.microsoft.com/en-us/download/details.aspx?id=14632)  

### Example
You need to replace 3 properties in the following file with your project's host, access_key and access_secret, and run it.   
[ExampleFile](https://github.com/Executor-Cheng/ACRCloudSdkCore/blob/master/ACRCloudSdkCore.Example/Program.cs)  
