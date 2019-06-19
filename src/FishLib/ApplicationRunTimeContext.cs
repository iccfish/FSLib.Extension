using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSLib.Extension
{
	using System.Diagnostics;
	using System.IO;

	/// <summary>
	/// 应用程序上下文
	/// </summary>
	public class ApplicationRunTimeContext
	{
		static ApplicationRunTimeContext()
		{
			Type t = Type.GetType("Mono.Runtime");
			IsMono = t != null;

			IsLinux = Path.DirectorySeparatorChar == '/';
		}

		/// <summary>
		/// 获得当前的运行环境是否是Mono
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is mono; otherwise, <c>false</c>.
		/// </value>
		public static bool IsMono { get; private set; }

		/// <summary>
		/// 获得当前运行的平台是否是Linux
		/// </summary>
		public static bool IsLinux { get; private set; }

#if !NETSTANDARD1_6_1 && !NETSTANDARD2_0 && !NETSTANDARD3_0
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
#endif
	}
}
