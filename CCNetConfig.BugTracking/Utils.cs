using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

namespace CCNetConfig.BugTracking {
  /// <summary>
  /// 
  /// </summary>
  public static class Utils {

    /// <summary>
    /// Gets the OS information.
    /// </summary>
    /// <returns></returns>
    public static string GetOSInformation ( ) {
      string format = "OS: {0} {1} SP: {2}{7}Processor Count: {3}{7}CLR Version: {4}{7}WorkingSet: {5}{7}System Directory: {6}";
      return string.Format ( format, Environment.OSVersion.Platform.ToString ( ), Environment.OSVersion.VersionString,
        Environment.OSVersion.ServicePack, Environment.ProcessorCount, Environment.Version.ToString ( ),
        Environment.WorkingSet, Environment.SystemDirectory, Environment.NewLine );
    }

    /// <summary>
    /// Gets the assembly information.
    /// </summary>
    /// <returns></returns>
    public static string GetAssemblyInformation ( ) {
      string format = "Version: {2}{4}Time: {3}{4}App Path:{0}, Assembly: {1}";
      Assembly asm = typeof(Utils).Assembly;
      return string.Format ( format, Path.GetDirectoryName ( asm.Location ), asm.GetName ( ).FullName, 
        asm.GetName ( ).Version, DateTime.Now.ToString(), Environment.NewLine );
    }
  }
}
