using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Text.RegularExpressions;

namespace System
{
	using FishExtension;

	/// <summary>
	/// 对象扩展
	/// </summary>
	[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	public static class FishObjectExtension
	{
		#region System.Drawing.Image

		/// <summary>
		/// 将图像转换为字节格式
		/// </summary>
		/// <param name="image">要转换的图像</param>
		/// <returns>转换后的字节数组</returns>
		public static byte[] ToBytes(this Image image)
		{
			return ToBytes(image, ImageFormat.Png);
		}

		/// <summary>
		/// 将图像转换为字节格式
		/// </summary>
		/// <param name="image">要转换的图像</param>
		/// <returns>转换后的字节数组</returns>
		public static byte[] ToBytes(this Image image, System.Drawing.Imaging.ImageFormat format)
		{
			if (image == null) return null;

			using (var ms = new System.IO.MemoryStream())
			{
				image.Save(ms, format);
				ms.Close();

				return ms.ToArray();
			}
		}

		/// <summary>
		/// 使用质量90将图片保存到指定位置为JPEG图片
		/// </summary>
		/// <param name="image">要保存的图片</param>
		/// <param name="path">保存的路径</param>
		public static void SaveAsJpeg(this Image image, string path)
		{
			SaveAsJpeg(image, path, 90);
		}

		/// <summary>
		/// 使用指定的图片质量将图片保存到指定位置为JPEG图片
		/// </summary>
		/// <param name="image">要保存的图片</param>
		/// <param name="path">保存的路径</param>
		/// <param name="quality">质量</param>
		public static void SaveAsJpeg(this Image image, string path, int quality)
		{
			EncoderParameters parameters = new EncoderParameters(1);
			parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ((long)quality));

			ImageCodecInfo myImageCodecInfo = (from p in ImageCodecInfo.GetImageEncoders() where p.MimeType == "image/jpeg" select p).Single<ImageCodecInfo>();
			image.Save(path, myImageCodecInfo, parameters);
		}

		/// <summary>
		/// 由原始的小图创建一个居中的大图
		/// </summary>
		/// <param name="image">原始图像</param>
		/// <param name="width">新图像的宽度</param>
		/// <param name="height">新图像的高度</param>
		/// <returns><see cref="T:System.Drawing.Image"/></returns>
		public static Image ResizeWithMargin(this Image image, int width, int height)
		{
			if (image == null || image.Width >= width || image.Height >= height) return image;

			var nimg = new Bitmap(width, height);
			using (var g = Graphics.FromImage(nimg))
			{
				g.DrawImage(image, (width - image.Width) / 2, (height - image.Height) / 2, image.Height, image.Width);
			}

			return nimg;
		}

		#endregion

		#region Reflection

		/// <summary>
		/// 根据指定的自定义属性来过滤类型列表
		/// </summary>
		/// <typeparam name="T">要查找的自定义属性类型</typeparam>
		/// <param name="assembly">程序集</param>
		/// <returns>查找到的结果</returns>
		public static Dictionary<Type, T[]> GetFilteredTypeWithCustomerAttribute<T>(this Assembly assembly)
		{
			var dic = assembly.GetTypes().ToDictionary(s => s, s => s.GetCustomAttributes(typeof(T), true).Cast<T>().ToArray());
			dic.Keys.Where(s => dic[s].Length == 0).ToArray().ForEach(s => dic.Remove(s));
			return dic;
		}

		/// <summary>
		/// 根据指定的自定义属性来过滤方法列表
		/// </summary>
		/// <typeparam name="T">要查找的自定义属性类型</typeparam>
		/// <param name="type">程序集</param>
		/// <returns>查找到的结果</returns>
		public static Dictionary<MethodInfo, T[]> GetFilteredMethodWithCustomerAttribute<T>(this Type type)
		{
			return GetFilteredMethodWithCustomerAttribute<T>(type, BindingFlags.Public | BindingFlags.Instance);
		}

		/// <summary>
		/// 根据指定的自定义属性来过滤方法列表
		/// </summary>
		/// <typeparam name="T">要查找的自定义属性类型</typeparam>
		/// <param name="flags">查找的标志位</param>
		/// <param name="type">程序集</param>
		/// <returns>查找到的结果</returns>
		public static Dictionary<MethodInfo, T[]> GetFilteredMethodWithCustomerAttribute<T>(this Type type, BindingFlags flags)
		{
			var dic = type.GetMethods(flags).ToDictionary(s => s, s => s.GetCustomAttributes(typeof(T), true).Cast<T>().ToArray());
			dic.Keys.Where(s => dic[s].Length == 0).ToArray().ForEach(s => dic.Remove(s));
			return dic;
		}

		/// <summary>
		/// 根据指定的自定义属性来过滤属性列表
		/// </summary>
		/// <typeparam name="T">要查找的自定义属性类型</typeparam>
		/// <param name="type">程序集</param>
		/// <returns>查找到的结果</returns>
		public static Dictionary<PropertyInfo, T[]> GetFilteredPropertyWithCustomerAttribute<T>(this Type type)
		{
			return GetFilteredPropertyWithCustomerAttribute<T>(type, BindingFlags.Public | BindingFlags.Instance);
		}

		/// <summary>
		/// 根据指定的自定义属性来过滤属性列表
		/// </summary>
		/// <typeparam name="T">要查找的自定义属性类型</typeparam>
		/// <param name="type">程序集</param>
		/// <param name="flags">查找的标志位</param>
		/// <returns>查找到的结果</returns>
		public static Dictionary<PropertyInfo, T[]> GetFilteredPropertyWithCustomerAttribute<T>(this Type type, BindingFlags flags)
		{
			var dic = type.GetProperties(flags).ToDictionary(s => s, s => s.GetCustomAttributes(typeof(T), true).Cast<T>().ToArray());
			dic.Keys.Where(s => dic[s].Length == 0).ToArray().ForEach(s => dic.Remove(s));
			return dic;
		}

		/// <summary>
		/// 获得程序集所在的目录
		/// </summary>
		/// <param name="assembly">程序集</param>
		/// <returns>指定程序集所在的位置目录</returns>
		public static string GetLocation(this System.Reflection.Assembly assembly)
		{
			return System.IO.Path.GetDirectoryName(assembly.Location);
		}

		/// <summary>
		/// 获得程序集版本信息
		/// </summary>
		/// <param name="assembly">程序集</param>
		/// <returns>指定程序集的版本</returns>
		public static Version GetVersion(this System.Reflection.Assembly assembly)
		{
			return assembly.GetName().Version;
		}

		/// <summary>
		/// 获得程序集的文件信息
		/// </summary>
		/// <param name="assembly">程序集</param>
		/// <returns><see cref="T:System.Diagnostics.FileVersionInfo"/></returns>
		public static System.Diagnostics.FileVersionInfo GetFileVersionInfo(this System.Reflection.Assembly assembly)
		{
			return System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
		}

		/// <summary>
		/// 对类型列表进行过滤。
		/// </summary>
		/// <typeparam name="T">过滤的类型，可以根据基类、接口或自定义属性进行过滤</typeparam>
		/// <param name="typeList">要过滤的类型列表</param>
		/// <param name="imp">对应的类型</param>
		/// <returns>过滤后的类型列表</returns>
		public static IEnumerable<Type> FilterType<T>(this IEnumerable<Type> typeList)
		{
			return FilterType<T>(typeList, false);
		}

		/// <summary>
		/// 对类型列表进行过滤。
		/// </summary>
		/// <typeparam name="T">过滤的类型，可以根据基类、接口或自定义属性进行过滤</typeparam>
		/// <param name="typeList">要过滤的类型列表</param>
		/// <param name="imp">对应的类型</param>
		/// <param name="ignoreAttribute">是否忽略自定义属性；如果为 true，则传入自定义属性时，不会按照自定义属性过滤</param>
		/// <returns>过滤后的类型列表</returns>
		public static IEnumerable<Type> FilterType<T>(this IEnumerable<Type> typeList, bool ignoreAttribute)
		{
			var t = typeof(T);

			if (t.IsInterface)
			{
				return typeList.Where(s => s.GetInterface(t.FullName) != null);
			}
			else if (!ignoreAttribute && t.IsClass && t.IsSubclassOf(typeof(Attribute)))
			{
				return typeList.Where(s => !s.GetCustomAttributes(t, true).IsEmpty());
			}
			else if (t.IsClass)
			{
				return typeList.Where(s => s.IsSubclassOf(t));
			}

			return typeList;
		}

		/// <summary>
		/// 获得类的完整名称（含程序集名称）
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static string GetTypeFullNameWithAssembly(this Type type)
		{
			return type.FullName + ", " + System.IO.Path.GetFileNameWithoutExtension(type.Assembly.Location);
		}

		/// <summary>
		/// 获得自定义属性
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="type">自定义属性类型</param>
		/// <param name="inherit">是否继承</param>
		/// <returns></returns>
		public static T[] GetCustomerAttributes<T>(this ICustomAttributeProvider type, bool inherit) where T : Attribute
		{
			if (type == null)
				return null;

			return type.GetCustomAttributes(typeof(T), inherit).Cast<T>().ToArray();
		}
		/// <summary>
		/// 获得自定义属性
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="type">自定义属性类型</param>
		/// <returns></returns>
		public static T[] GetCustomerAttributes<T>(this ICustomAttributeProvider type) where T : Attribute
		{
			if (type == null)
				return null;

			return type.GetCustomAttributes(typeof(T), false).Cast<T>().ToArray();
		}

		/// <summary>
		/// 获得程序集是否是调试版本编译的
		/// </summary>
		/// <param name="assembly">程序集</param>
		/// <returns>如果是调试版本，返回 true；否则返回false。</returns>
		public static bool IsDebugAssembly(this Assembly assembly)
		{
			if (assembly == null)
				return false;

			var debugAttributes = assembly.GetCustomerAttributes<System.Diagnostics.DebuggableAttribute>();
			return !debugAttributes.IsEmpty() && debugAttributes[0].IsJITTrackingEnabled;
		}

		#endregion

		#region Common

		/// <summary>
		/// 将对象选择为字符串并进行格式化
		/// </summary>
		/// <typeparam name="T">来源类型</typeparam>
		/// <param name="source">来源</param>
		/// <param name="format">格式化字符串</param>
		/// <param name="selector">要填充的参数列</param>
		/// <returns><see cref="IEnumerable{T}"/></returns>
		public static IEnumerable<string> FormatValue<T>(this IEnumerable<T> source, string format, params Func<T, object>[] selector)
		{
			return source.Select(s => string.Format(format, selector.Select(m => m(s)))).ToArray();
		}

		/// <summary>
		/// 根据对象选择值，如果为空，则返回默认值
		/// </summary>
		/// <typeparam name="T">对象类型</typeparam>
		/// <typeparam name="R">结果类型</typeparam>
		/// <param name="obj">对象</param>
		/// <param name="selector">选择器</param>
		/// <returns><typeparamref name="R"/></returns>
		public static R SelectValue<T, R>(this T obj, Func<T, R> selector) where T : class
		{
			if (obj == null) return default(R);

			return selector(obj);
		}

		/// <summary>
		/// 根据对象选择值，如果为空，则返回默认值
		/// </summary>
		/// <typeparam name="T">对象类型</typeparam>
		/// <typeparam name="R">结果类型</typeparam>
		/// <param name="obj">对象</param>
		/// <param name="selector">选择器</param>
		/// <param name="defaultValue">返回的默认值</param>
		/// <returns><typeparamref name="R"/></returns>
		public static R SelectValue<T, R>(this T obj, Func<T, R> selector, R defaultValue) where T : class
		{
			if (obj == null) return defaultValue;

			return selector(obj);
		}

		/// <summary>
		/// 根据对象选择值，如果为空，则返回默认值
		/// </summary>
		/// <typeparam name="T">对象类型</typeparam>
		/// <param name="obj">对象</param>
		/// <param name="selector">选择器</param>
		/// <returns><see cref="T:System.String"/></returns>
		public static string SelectValue<T>(this T obj, Func<T, string> selector) where T : class
		{
			if (obj == null) return null;

			return selector(obj);
		}

		/// <summary>
		/// 根据对象选择值，如果为空，则返回默认值
		/// </summary>
		/// <typeparam name="T">对象类型</typeparam>
		/// <param name="obj">对象</param>
		/// <param name="selector">选择器</param>
		/// <param name="defaultValue">返回的默认值</param>
		/// <returns><see cref="T:System.String"/></returns>
		public static string SelectValue<T>(this T obj, Func<T, string> selector, string defaultValue) where T : class
		{
			if (obj == null) return defaultValue;

			return selector(obj);
		}

		/// <summary>
		/// 根据对象选择值，如果为空，则返回默认值
		/// </summary>
		/// <typeparam name="T">对象类型</typeparam>
		/// <param name="obj">对象</param>
		/// <param name="selector">选择器</param>
		/// <returns><see cref="T:System.DateTime"/></returns>
		public static DateTime SelectValue<T>(this T obj, Func<T, DateTime> selector) where T : class
		{
			if (obj == null) return DateTime.MinValue;

			return selector(obj);
		}

		/// <summary>
		/// 根据对象选择值，如果为空，则返回默认值
		/// </summary>
		/// <typeparam name="T">对象类型</typeparam>
		/// <param name="obj">对象</param>
		/// <param name="selector">选择器</param>
		/// <param name="defaultValue">返回的默认值</param>
		/// <returns><see cref="T:System.DateTime"/></returns>
		public static DateTime SelectValue<T>(this T obj, Func<T, DateTime> selector, DateTime defaultValue) where T : class
		{
			if (obj == null) return defaultValue;

			return selector(obj);
		}

		/// <summary>
		/// 根据对象选择值，如果为空，则返回默认值
		/// </summary>
		/// <typeparam name="T">对象类型</typeparam>
		/// <param name="obj">对象</param>
		/// <param name="selector">选择器</param>
		/// <returns><see cref="T:System.Int32"/></returns>
		public static int SelectValue<T>(this T obj, Func<T, int> selector) where T : class
		{
			if (obj == null) return 0;

			return selector(obj);
		}

		/// <summary>
		/// 根据对象选择值，如果为空，则返回默认值
		/// </summary>
		/// <typeparam name="T">对象类型</typeparam>
		/// <param name="obj">对象</param>
		/// <param name="selector">选择器</param>
		/// <param name="defaultValue">返回的默认值</param>
		/// <returns><see cref="T:System.Int32"/></returns>
		public static int SelectValue<T>(this T obj, Func<T, int> selector, int defaultValue) where T : class
		{
			if (obj == null) return defaultValue;

			return selector(obj);
		}

		/// <summary>
		/// 将对象序列连接为字符串
		/// </summary>
		/// <typeparam name="T">序列类型</typeparam>
		/// <param name="src">源序列</param>
		/// <returns><see cref="T:System.String"/></returns>
		public static string JoinAsString<T>(this IEnumerable<T> src)
		{
			return JoinAsString(src, ",");
		}

		/// <summary>
		/// 将对象序列连接为字符串
		/// </summary>
		/// <typeparam name="T">序列类型</typeparam>
		/// <param name="src">源序列</param>
		/// <param name="seperator">分隔符</param>
		/// <returns><see cref="T:System.String"/></returns>
		public static string JoinAsString<T>(this IEnumerable<T> src, string seperator)
		{
			return src.Where(s => s != null).Select(s => s.ToString()).Join(seperator);
		}

		#endregion

		#region Regex

		/// <summary>
		/// 获得一个匹配结果中指定分组的值
		/// </summary>
		/// <param name="match">匹配结果</param>
		/// <param name="index">指定的索引</param>
		/// <returns>对应分组的值；如果不成功或索引不对，则返回null</returns>
		public static string GetGroupValue(this Match match, int index)
		{
			return match == null || !match.Success || match.Groups.Count <= index ? null : match.Groups[index].Value;
		}

		#endregion

		#region IEnumerable

		/// <summary>
		/// 对两个序列进行合并。如果其中一个是null，则返回另一个
		/// </summary>
		/// <typeparam name="T">序列类型</typeparam>
		/// <param name="source">源序列</param>
		/// <param name="second">要合并的序列</param>
		/// <returns>合并后的序列</returns>
		public static IEnumerable<T> UnionWith<T>(this IEnumerable<T> source, IEnumerable<T> second)
		{
			if (source == null ^ second == null) return second == null ? source : second;
			return source == null ? null : source.Union(second);

		}

		/// <summary>
		/// 对可遍历数据源进行分页操作
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="source">源</param>
		/// <param name="pagesize">每页尺寸</param>
		/// <param name="executor">执行器</param>
		public static void PagedExecute<T>(this IEnumerable<T> source, int pagesize, Action<int, IEnumerable<T>> executor)
		{
			var count = source.Count();
			var pagecount = count.CeilingDivide(pagesize);

			for (int i = 0; i < pagecount; i++)
			{
				executor(i, source.Skip(pagesize * i).Take(pagesize).ToArray());
			}
		}

		/// <summary>
		/// 将指定的序列生成为树状结构
		/// </summary>
		/// <typeparam name="T">树类型</typeparam>
		/// <typeparam name="TKey">上级键值类型</typeparam>
		/// <param name="src">原始序列</param>
		/// <param name="parentSelector">上级键选择器</param>
		/// <param name="childrenSelector">下级集合选择器</param>
		/// <returns>顶级节点</returns>
		public static IEnumerable<T> PopulateTree<T, TKey>(this IEnumerable<T> src, Func<T, TKey> keySelector, Func<T, TKey> parentSelector, Func<T, IList<T>> childrenSelector, IEqualityComparer<TKey> comparer = null)
			where T : class
		{
			if (src == null)
				throw new ArgumentNullException("src", "src is null.");
			if (keySelector == null)
				throw new ArgumentNullException("parentSelector", "keySelector is null.");
			if (parentSelector == null)
				throw new ArgumentNullException("parentSelector", "parentSelector is null.");
			if (childrenSelector == null)
				throw new ArgumentNullException("childrenSelector", "childrenSelector is null.");
			if (comparer == null)
				comparer = EqualityComparer<TKey>.Default;

			var emptyValue = default(TKey);
			var array = src.ToArray();
			var dicCache = array.ToDictionary(keySelector, comparer);

			var result = array.Where(s => comparer.Equals(parentSelector(s), emptyValue)).ToArray();
			array.ForEach(s =>
			{
				var pkey = parentSelector(s);
				if (comparer.Equals(pkey, emptyValue))
					return;

				var parent = dicCache.GetValue(pkey);
				if (parent == null)
					throw new Exception("指定的ParentID无效");

				var children = childrenSelector(parent);
				if (children == null)
					return;
				children.Add(s);
			});

			return result;
		}

		#endregion

		#region Uri

		/// <summary>
		/// 获得指定地址中的文件名
		/// </summary>
		/// <param name="uri"></param>
		/// <returns></returns>
		public static string GetFileName(this Uri uri)
		{
			if (uri == null) return string.Empty;

			var path = uri.LocalPath;
			var index = path.LastIndexOf("/");
			return index == -1 ? path : path.Substring(index + 1);
		}

		readonly static Regex _topLevelHostReg = new Regex(@"[-a-zA-Z\d]+\.[-a-zA-Z\d]+$", RegexOptions.IgnoreCase);

		/// <summary>
		/// 获得一个域名空间的顶级主机地址
		/// </summary>
		/// <param name="uri">完整域名</param>
		/// <returns><see cref="T:System.String"/></returns>
		public static string GetTopLevelHost(this Uri uri)
		{
			var m = _topLevelHostReg.Match(uri.Host);
			if (m.Success) return m.Value;
			else return uri.Host;
		}


		#endregion

		#region Stream

		/// <summary>
		/// 从流中读入一个 <see cref="T:System.Int16"/>
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <returns>读取的 <see cref="T:System.Int16"/></returns>
		/// <exception cref="Exception">流读取失败</exception>
		public static ushort ReadUInt16(this System.IO.Stream stream)
		{
			return BitConverter.ToUInt16(stream.ReadBuffer(2), 0);
		}

		/// <summary>
		/// 从流中读入一个 <see cref="T:System.Int32"/>
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <returns>读取的 <see cref="T:System.Int32"/></returns>
		/// <exception cref="Exception">流读取失败</exception>
		public static uint ReadUInt32(this Stream stream)
		{
			return BitConverter.ToUInt32(stream.ReadBuffer(4), 0);
		}

		/// <summary>
		/// 从流中读入一个 <see cref="T:System.Int64"/>
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <returns>读取的 <see cref="T:System.Int64"/></returns>
		/// <exception cref="Exception">流读取失败</exception>
		public static ulong ReadUInt64(this Stream stream)
		{
			return BitConverter.ToUInt64(stream.ReadBuffer(8), 0);
		}

		/// <summary>
		/// 从流中读入一个 <see cref="T:System.Int16"/>
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <returns>读取的 <see cref="T:System.Int16"/></returns>
		/// <exception cref="Exception">流读取失败</exception>
		public static short ReadInt16(this System.IO.Stream stream)
		{
			return BitConverter.ToInt16(stream.ReadBuffer(2), 0);
		}

		/// <summary>
		/// 从流中读入一个 <see cref="T:System.Int32"/>
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <returns>读取的 <see cref="T:System.Int32"/></returns>
		/// <exception cref="Exception">流读取失败</exception>
		public static int ReadInt32(this Stream stream)
		{
			return BitConverter.ToInt32(stream.ReadBuffer(4), 0);
		}

		/// <summary>
		/// 从流中读入一个 <see cref="T:System.Int64"/>
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <returns>读取的 <see cref="T:System.Int64"/></returns>
		/// <exception cref="Exception">流读取失败</exception>
		public static long ReadInt64(this Stream stream)
		{
			return BitConverter.ToInt64(stream.ReadBuffer(8), 0);
		}

		/// <summary>
		/// 从流中读入一个 <see cref="T:System.Int64"/>
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <returns>读取的 <see cref="T:System.Int64"/></returns>
		/// <exception cref="Exception">流读取失败</exception>
		public static double ReadDouble(this Stream stream)
		{
			return BitConverter.ToDouble(stream.ReadBuffer(sizeof(double)), 0);
		}

		/// <summary>
		/// 从流中读入一个缓冲数组
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <param name="length">读取的字节长度</param>
		/// <returns>缓冲数组</returns>
		public static byte[] ReadBuffer(this Stream stream, int length)
		{
			var count = 0;


			return stream.ReadBuffer(length, out count);
		}

		/// <summary>
		/// 从流中读入一个缓冲数组
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <param name="length">读取的字节长度</param>
		/// <returns>缓冲数组</returns>
		public static byte[] ReadBuffer(this Stream stream, int length, out int readedBytesCount)
		{
			var buffer = new byte[length];
			readedBytesCount = stream.Read(buffer, 0, buffer.Length);

			return buffer;
		}

		/// <summary>
		/// 从流中读入一个缓冲数组
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <param name="buffer">缓冲数组</param>
		/// <returns>缓冲数组</returns>
		public static byte[] FillBuffer(this Stream stream, byte[] buffer)
		{
			if (stream.Read(buffer, 0, buffer.Length) != buffer.Length)
			{
				throw new Exception();
			}

			return buffer;
		}

		/// <summary>
		/// 读取所有的数据到内存流中
		/// </summary>
		/// <param name="stream">要读取的流</param>
		/// <returns>包含所有数据的 <see cref="MemoryStream"/> </returns>
		[CanBeNull]
		public static MemoryStream ReadToEnd([NotNull] this Stream stream)
		{
			if (stream == null || !stream.CanRead)
				return null;

			if (stream is MemoryStream)
				return stream as MemoryStream;

			var ms = new MemoryStream();
			var buffer = new byte[0x400];
			var count = 0;
			while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
				ms.Write(buffer, 0, count);
			ms.Flush();
			ms.Seek(0, SeekOrigin.Begin);

			return ms;
		}


		/// <summary>
		/// 压缩原始流
		/// </summary>
		/// <param name="stream">要写入的目标流</param>
		/// <returns>供写入的压缩流</returns>
		public static Stream Zip(this Stream stream)
		{
			return new System.IO.Compression.GZipStream(stream, CompressionMode.Compress);
		}

		/// <summary>
		/// 解压缩原始流
		/// </summary>
		/// <param name="stream">供读取的压缩流</param>
		/// <returns>供读取的解压缩流</returns>
		public static Stream UnZip(this Stream stream)
		{
			return new GZipStream(stream, CompressionMode.Decompress);
		}

		/// <summary>
		/// 获得当前流位置的显示字符串格式
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static string GetPositionString(this Stream stream)
		{
			return string.Format("0x{0:X8}", stream.Position);
		}

		/// <summary>
		/// 将指定的缓冲数组全部写入流中
		/// </summary>
		/// <param name="stream">目标流</param>
		/// <param name="buffer">缓冲数组</param>
		public static T Write<T>(this T stream, byte[] buffer) where T : Stream
		{
			stream.Write(buffer, 0, buffer.Length);
			return stream;
		}

		/// <summary>
		/// 将指定的缓冲数组全部写入流中
		/// </summary>
		/// <param name="stream">目标流</param>
		/// <param name="buffer">缓冲数组</param>
		public static T Write<T>(this T stream, IEnumerable<byte> buffer) where T : Stream
		{
			stream.Write(buffer.ToArray());
			return stream;
		}

		/// <summary>
		/// 将目标值写入流中
		/// </summary>
		/// <param name="stream">当前流</param>
		/// <param name="value">值</param>
		public static T Write<T>(this T stream, int value) where T : Stream
		{
			stream.Write(BitConverter.GetBytes(value));
			return stream;
		}

		/// <summary>
		/// 将目标值写入流中
		/// </summary>
		/// <param name="stream">当前流</param>
		/// <param name="value">值</param>
		public static T Write<T>(this T stream, uint value) where T : Stream
		{
			stream.Write(BitConverter.GetBytes(value));
			return stream;
		}

		/// <summary>
		/// 将目标值写入流中
		/// </summary>
		/// <param name="stream">当前流</param>
		/// <param name="value">值</param>
		public static T Write<T>(this T stream, short value) where T : Stream
		{
			stream.Write(BitConverter.GetBytes(value));
			return stream;
		}

		/// <summary>
		/// 将目标值写入流中
		/// </summary>
		/// <param name="stream">当前流</param>
		/// <param name="value">值</param>
		public static T Write<T>(this T stream, ushort value) where T : Stream
		{
			stream.Write(BitConverter.GetBytes(value));
			return stream;
		}

		/// <summary>
		/// 将目标值写入流中
		/// </summary>
		/// <param name="stream">当前流</param>
		/// <param name="value">值</param>
		public static T Write<T>(this T stream, long value) where T : Stream
		{
			stream.Write(BitConverter.GetBytes(value));
			return stream;
		}

		/// <summary>
		/// 将目标值写入流中
		/// </summary>
		/// <param name="stream">当前流</param>
		/// <param name="value">值</param>
		public static T Write<T>(this T stream, ulong value) where T : Stream
		{
			stream.Write(BitConverter.GetBytes(value));
			return stream;
		}

		#endregion

		#region 其它

		/// <summary>
		/// 对指定的数据进行条件判断，如果符合要求则执行
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="value">值</param>
		/// <param name="eval">表达式</param>
		/// <param name="action">执行方法</param>
		public static void ExecuteIf<T>(this T value, Func<T, bool> eval, Action<T> action)
		{
			if (eval(value)) action(value);
		}

		/// <summary>
		/// 对指定的数据进行条件判断，如果大于0则执行
		/// </summary>
		/// <param name="value">值</param>
		/// <param name="action">执行方法</param>
		public static void ExecuteIfPositive(this int value, Action<int> action)
		{
			if (value > 0) action(value);
		}

		/// <summary>
		/// 对指定的数据进行条件判断，如果不为空字符串则执行
		/// </summary>
		/// <param name="value">值</param>
		/// <param name="action">执行方法</param>
		public static void ExecuteIfNotEmpty(this string value, Action<string> action)
		{
			if (!string.IsNullOrEmpty(value)) action(value);
		}

		/// <summary>
		/// 对指定的数据进行条件判断，如果不为null则执行
		/// </summary>
		/// <param name="value">值</param>
		/// <param name="action">执行方法</param>
		public static void ExecuteIfNotEmpty<T>(this T value, Action<T> action) where T : class
		{
			if (value != null) action(value);
		}

		/// <summary>
		/// 对指定的数据进行条件判断，如果不为空序列则执行
		/// </summary>
		/// <param name="value">值</param>
		/// <param name="action">执行方法</param>
		public static void ExecuteIfNotEmpty<T>(this IEnumerable<T> value, Action<IEnumerable<T>> action)
		{
			if (value != null && value.Any()) action(value);
		}

		/// <summary>
		/// 双重加锁执行
		/// </summary>
		/// <param name="obj">当前锁对象</param>
		/// <param name="condition">条件，返回false则不执行</param>
		/// <param name="action">执行的操作</param>
		public static void LockExecute(this object obj, Func<bool> condition, Action action)
		{
			if (action == null || obj == null) return;

			if (condition == null || condition())
			{
				lock (obj)
				{
					if (condition == null || condition())
					{
						action();
					}
				}
			}
		}

		#endregion

		#region 转换


		#endregion
	}

}