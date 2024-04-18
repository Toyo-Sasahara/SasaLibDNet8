using SasaLib;
using System;
using System.Reflection;

public static class PagePictureControlInfo
{
    public static string GetAssemblyFileName()
    {
        System.Diagnostics.FileVersionInfo ver = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
        return ver.FileName;
    }

    public static string GetAssemblyProductVersion()
    {
        System.Diagnostics.FileVersionInfo ver = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
        return ver.ProductVersion;
    }

    public static DateTime GetAssemblyLastWriteTime()
    {
        System.Diagnostics.FileVersionInfo ver = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
        DateTime dt = System.IO.File.GetLastWriteTime(ver.FileName);
        return dt;
    }
    public static DateTime GetAssemblyCreationTime()
    {
        System.Diagnostics.FileVersionInfo ver = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
        DateTime dt = System.IO.File.GetCreationTime(ver.FileName);
        return dt;
    }

    public static string GetAssemblyFileVersion()
    {
        System.Diagnostics.FileVersionInfo ver = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
        return ver.FileVersion;
    }

    /// <summary>
    /// アセンブリバージョンを取得
    /// </summary>
    /// <returns></returns>
    public static string GetAssmblyVersion()
    {
        System.Reflection.Assembly assembly = Assembly.GetExecutingAssembly();
        System.Reflection.AssemblyName asmName = assembly.GetName();
        System.Version version = asmName.Version;

        return version.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static string GetAssemblyFileMD5()
    {
        System.Diagnostics.FileVersionInfo ver = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
        string md5 = FileFolder.GetMD5FileHash(ver.FileName);
        return md5;
    }


}
