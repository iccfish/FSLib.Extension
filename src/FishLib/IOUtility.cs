namespace FSLib.Extension
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text.RegularExpressions;

	/// <summary>
	/// 文件操作辅助类
	/// </summary>
	/// <remarks>
	/// 本辅助操作类的作用是为了简化一些代码，实际上考虑到程序的性能问题时，可能还是由自已来针对
	/// 特定的File Directory FileInfo DirectoryInfo Path Drive DriveInfo来操作可能更好
	/// </remarks>
	public static class IOUtility
	{
		private static HashSet<char> _invalidPathChars=Path.GetInvalidPathChars().MapToHashSet();
		private static HashSet<char> _invalidFileNames=Path.GetInvalidFileNameChars().MapToHashSet();

		/// <summary>
		/// 移除路径中无效的路径字符
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string RemoveInvalidPathChars(string path) => path.Where(s => !_invalidPathChars.Contains(s)).JoinAsString("");

		/// <summary>
		/// 移除文件名中无效字符
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string RemoveInvalidFileNameChars(string path) => path.Where(s => !_invalidFileNames.Contains(s)).JoinAsString("");

		/// <summary>
		/// 获得指定目录下的所有文件。此方法将会跳过无法访问的文件夹
		/// </summary>
		/// <param name="path">要获得的目录</param>
		/// <returns>包含文件名的数组</returns>
		public static string[] GetAllFiles(string path)
		{
			if (path == null || path.IsNullOrEmpty())
				throw new ArgumentNullException(nameof(path));
			if (!Directory.Exists(path))
				throw new DirectoryNotFoundException();

			string[] files, folders;
			try
			{
				files = Directory.GetFiles(path);
				folders = Directory.GetDirectories(path);
			}
			catch (Exception)
			{
				return new string[]
				{
				};
			}

			return files.Union(folders.SelectMany(GetAllFiles)).ToArray();
		}

		/// <summary>
		/// 返回没有前缀符号的扩展名
		/// </summary>
		/// <param name="path">路径</param>
		/// <returns>扩展名</returns>
		public static string GetExtensionWithoutDot(string path)
		{
			return Path.GetExtension(path).Trim('.');
		}

		/// <summary>
		/// 将数据完全写入文件。当指定路径不存在时，会自动创建
		/// </summary>
		/// <param name="path">路径</param>
		/// <param name="data">数据</param>
		public static void WriteAllBytes(string path, byte[] data)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(path));
			File.WriteAllBytes(path, data);
		}

		/// <summary>
		/// 将数据完全写入文件。当指定路径不存在时，会自动创建
		/// </summary>
		/// <param name="path">路径</param>
		public static void WriteAllText(string path, string content)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(path));
			File.WriteAllText(path, content);
		}

		/// <summary>
		/// 将数据完全写入文件。当指定路径不存在时，会自动创建
		/// </summary>
		/// <param name="path">路径</param>
		public static void WriteAllLines(string path, string[] lines)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(path));
			File.WriteAllLines(path, lines);
		}

		/// <summary>
		/// 获得指定目录下的文件，并根据指定的过滤规则进行过滤
		/// </summary>
		/// <param name="path">要检索的路径</param>
		/// <param name="searchPattern">过滤规则（正则表达式）</param>
		/// <param name="option">搜索选项</param>
		/// <returns><see cref="T:System.Array"/></returns>
		public static string[] GetFiles(string path, string searchPattern, SearchOption option)
		{
			var reg = new Regex(searchPattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
			return Directory.GetFiles(path, "*.*", option).Where(s => reg.IsMatch(s)).ToArray();
		}


		/// <summary>
		/// 文件是否只读
		/// </summary>
		/// <param name="fullpath"></param>
		/// <returns></returns>
		public static bool FileIsReadOnly(string fullpath)
		{
			FileInfo file = new FileInfo(fullpath);
			if ((file.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
			{
				return true;
			}
			else
			{
				return false;
			}
		}


		/// <summary>
		/// 设置文件是否只读
		/// </summary>
		/// <param name="fullpath"></param>
		/// <param name="flag">true表示只读，反之</param>
		public static void SetFileReadonly(string fullpath, bool flag)
		{
			FileInfo file = new FileInfo(fullpath);

			if (flag)
			{
				// 添加只读属性
				file.Attributes |= FileAttributes.ReadOnly;
			}
			else
			{
				// 移除只读属性
				file.Attributes &= ~FileAttributes.ReadOnly;
			}
		}



		/// <summary>
		/// 取文件最后存储时间
		/// </summary>
		/// <param name="fullpath"></param>
		/// <returns></returns>
		public static DateTime GetLastWriteTime(string fullpath)
		{
			FileInfo fi = new FileInfo(fullpath);
			return fi.LastWriteTime;
		}



		/// <summary>
		/// 计算一个目录的大小
		/// </summary>
		/// <param name="di">指定目录</param>
		/// <param name="includeSubDir">是否包含子目录</param>
		/// <returns></returns>
		public static long CalculateDirectorySize(DirectoryInfo di, bool includeSubDir)
		{
			long totalSize = 0;

			// 检查所有（直接）包含的文件
			FileInfo[] files = di.GetFiles();
			foreach (FileInfo file in files)
			{
				totalSize += file.Length;
			}

			// 检查所有子目录，如果includeSubDir参数为true
			if (includeSubDir)
			{
				DirectoryInfo[] dirs = di.GetDirectories();
				foreach (DirectoryInfo dir in dirs)
				{
					totalSize += CalculateDirectorySize(dir, includeSubDir);
				}
			}

			return totalSize;
		}



		/// <summary>
		/// 复制目录到目标目录
		/// </summary>
		/// <param name="source">源目录</param>
		/// <param name="destination">目标目录</param>
		public static void CopyDirectory(DirectoryInfo source, DirectoryInfo destination)
		{
			// 如果两个目录相同，则无须复制
			if (destination.FullName.Equals(source.FullName))
			{
				return;
			}

			// 如果目标目录不存在，创建它
			if (!destination.Exists)
			{
				destination.Create();
			}

			// 复制所有文件
			FileInfo[] files = source.GetFiles();
			foreach (FileInfo file in files)
			{
				// 将文件复制到目标目录
				file.CopyTo(Path.Combine(destination.FullName, file.Name), true);
			}

			// 处理子目录
			DirectoryInfo[] dirs = source.GetDirectories();
			foreach (DirectoryInfo dir in dirs)
			{
				string destinationDir = Path.Combine(destination.FullName, dir.Name);

				// 递归处理子目录
				CopyDirectory(dir, new DirectoryInfo(destinationDir));
			}
		}


		/// <summary>
		/// 合并两个路径
		/// </summary>
		/// <param name="path1">路径1</param>
		/// <param name="path2">路径2</param>
		/// <param name="sourcePathIncludeFileName">指定路径1是否带有文件名。如果为true，那么会先将文件名去除（取文件夹）</param>
		/// <returns>合并的路径</returns>
		public static string CombinePath(string path1, string path2, bool sourcePathIncludeFileName = false)
		{
			var sep = Path.DirectorySeparatorChar;
			var isWinOsPath = path1[1] == ':';
			var newPath = "";

			if (sourcePathIncludeFileName)
				path1 = Path.GetDirectoryName(path1);

			if (path2.IsNullOrEmpty()) newPath = path1;
			else if (path1.IsNullOrEmpty()) newPath = path2;
			else if (path2[0] == sep)
			{
				if (isWinOsPath)
				{
					newPath = path1[0] + ":" + path2;
				}
				else
					newPath = path2;
			}
			else
			{
				if (path1.Last() == sep) newPath = path1 + path2;
				else newPath = path1 + sep + path2;
			}
			//格式化路径
			var stack = new Stack<string>();
			var arrays = newPath.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var item in arrays)
			{
				if (item == ".") continue;
				if (item == "..")
				{
					if (stack.Count > 0) stack.Pop();
					continue;
				}
				stack.Push(item);
			}
			var newPathArray = stack.ToArray();
			Array.Reverse(newPathArray);

			return newPathArray.Join(sep.ToString());
		}

		/// <summary>
		/// 获得相对地址
		/// </summary>
		/// <param name="basePath">当前地址</param>
		/// <param name="secondPath">要转换为相对地址的路径</param>
		/// <returns>相对地址</returns>
		public static string GetRelativePath(string basePath, string secondPath)
		{
			if (string.IsNullOrEmpty(secondPath) || string.IsNullOrEmpty(basePath) || char.ToLower(basePath[0]) != char.ToLower(secondPath[0]))
				return secondPath;

			var ps1 = basePath.Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
			var ps2 = secondPath.Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);

			var upperBound = Math.Min(ps1.Length, ps2.Length);
			var startIndex = Enumerable.Range(0, upperBound).FirstOrDefault(s => string.Compare(ps1[s], ps2[s], true) != 0);

			if (startIndex == 0)
				return secondPath;
			if (ps1.Length == startIndex && ps2.Length <= startIndex)
				return ".\\";

			return string.Join(Path.DirectorySeparatorChar.ToString(), Enumerable.Repeat("..", ps1.Length - startIndex).Concat(ps2.Skip(startIndex)).ToArray());
		}

		/// <summary>
		/// 获得用于显示的短路径
		/// </summary>
		/// <param name="src"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string GetShortDisplayPath(string src, int length)
		{
			if (string.IsNullOrEmpty(src)) return "";

			var fileName = Path.DirectorySeparatorChar + Path.GetFileName(src);
			var directory = Path.GetDirectoryName(src);

			var fileNameLength = fileName.Sum(s => s > 255 ? 2 : 1);
			var restBytesCount = Math.Max(0, length - fileNameLength);
			if (restBytesCount > 0) return directory.GetSubString(restBytesCount, "...") + fileName;
			return fileName;
		}

		/// <summary>
		/// 根据正则表达式查找文件
		/// </summary>
		/// <param name="path">查找源路径</param>
		/// <param name="pattern">过滤表达式</param>
		/// <param name="includeSubDirectory">是否包括子文件夹</param>
		/// <param name="applyFilerToPath">是否将过滤表达式应用到完整路径</param>
		/// <returns></returns>
		public static string[] RegFindFile(string path, string pattern, bool includeSubDirectory = true, bool applyFilerToPath = false)
		{
			var files = System.IO.Directory.GetFiles(path, "*.*", includeSubDirectory ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
			pattern = "^" + pattern + "$";

			var query = files.AsEnumerable();
			if (applyFilerToPath)
			{
				query = query.Where(s => Regex.IsMatch(s, pattern));
			}
			else
			{
				query = query.Where(s => Regex.IsMatch(System.IO.Path.GetFileName(s), pattern));
			}

			return query.ToArray();
		}

		[Flags]
		public enum GetPathOptions
		{
			/// <summary>
			/// 无标记位
			/// </summary>
			None = 0,
			/// <summary>
			/// 包含子文件夹
			/// </summary>
			ScanSubDirectory = 1,
			/// <summary>
			/// 包含文件
			/// </summary>
			IncludeFile = 2,
			/// <summary>
			/// 包含文件夹
			/// </summary>
			IncludeDirectory = 4,
			/// <summary>
			/// 默认值，等效于 IncludeFile | ScanSubDirectory
			/// </summary>
			Default = IncludeFile | ScanSubDirectory
		}

		/// <summary>
		/// 获得指定目录下所有文件或文件夹的相对路径
		/// </summary>
		/// <param name="path"></param>
		/// <param name="flags">标记位</param>
		/// <returns></returns>
		public static IEnumerable<string> GetDirectoryContainsByRelativePath(string path, GetPathOptions flags = GetPathOptions.Default) => GetDirectoryContainsByRelativePathInternal(string.Empty, path, flags);

		/// <summary>
		/// 获得指定目录下所有文件或文件夹的相对路径
		/// </summary>
		/// <param name="path"></param>
		/// <param name="flags">标记位</param>
		/// <returns></returns>
		internal static IEnumerable<string> GetDirectoryContainsByRelativePathInternal(string prefix, string path, GetPathOptions flags = GetPathOptions.Default)
		{
			if (path.IsNullOrEmpty())
				throw new ArgumentException($"{nameof(path)} can not be null of empty.");
			if (!Directory.Exists(path))
				throw new DirectoryNotFoundException($"The directory {path} was not found.");

			if ((flags & GetPathOptions.IncludeDirectory) != 0 && !prefix.IsNullOrEmpty())
			{
				yield return prefix + Path.DirectorySeparatorChar;
			}

			if ((flags & GetPathOptions.IncludeFile) != 0)
			{
				string[] files = null;
				try
				{
					files = Directory.GetFiles(path);
				}
				catch (Exception)
				{
				}

				if (files != null)
				{
					foreach (var file in files)
					{
						yield return Path.Combine(prefix, Path.GetFileName(file));
					}
				}
			}

			if ((flags & GetPathOptions.ScanSubDirectory) != 0)
			{
				string[] subdirs = null;
				try
				{
					subdirs = Directory.GetDirectories(path);
				}
				catch (Exception)
				{
				}

				if (subdirs != null)
				{
					foreach (var subdir in subdirs)
					{
						foreach (var file in GetDirectoryContainsByRelativePathInternal(Path.Combine(prefix, Path.GetFileName(subdir)), subdir, flags))
						{
							yield return file;
						}
					}
				}
			}
		}
	}
}
