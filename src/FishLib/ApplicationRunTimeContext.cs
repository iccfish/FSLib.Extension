using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.FishLib
{
	using System.Diagnostics;
	using System.IO;

	/// <summary>
	/// 应用程序上下文
	/// </summary>
	public class ApplicationRunTimeContext
	{
		/// <summary>
		/// 获得当前进程的主模块
		/// </summary>
		/// <returns>返回当前进程的主模块（<see cref="ProcessModule"/>）</returns>
		public static ProcessModule GetProcessMainModule()
		{
			return System.Diagnostics.Process.GetCurrentProcess().MainModule;
		}

		/// <summary>
		/// 获得当前进程主模块的路径
		/// </summary>
		/// <returns>返回当前进程的主模块路径（<see cref="String"/>）</returns>
		public static string GetProcessMainModulePath()
		{
			return GetProcessMainModule()?.FileName;
		}

		/// <summary>
		/// 获得当前进程主模块所在的目录
		/// </summary>
		/// <returns>获得当前进程主模块所在的目录（<see cref="String"/>）</returns>
		public static string GetProcessMainModuleDirectory()
		{
			var path = GetProcessMainModule()?.FileName;
			return string.IsNullOrEmpty(path) ? null : Path.GetDirectoryName(path);
		}
	}
}
