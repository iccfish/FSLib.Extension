using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO
{
	using System.Text.RegularExpressions;

	public class PathUtility
	{
		/// <summary>
		/// 合并路径
		/// </summary>
		/// <param name="paths"></param>
		/// <returns></returns>
		public static string Combine(params string[] paths)
		{
			if (paths == null || paths.Length == 0)
				throw new ArgumentException("paths is null or empty.", "paths");

#if NET35
			var path = paths.Join(Path.DirectorySeparatorChar.ToString());

			return Path.GetFullPath(Regex.Replace(path, Path.DirectorySeparatorChar.ToString() + @"{2,}", Path.DirectorySeparatorChar.ToString()));
#else
			return Path.GetFullPath(Path.Combine(paths));
#endif
		}
	}
}
