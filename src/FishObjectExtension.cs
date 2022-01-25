using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#if !NETSTANDARD1_6_1 && !NETSTANDARD2_0 && !NETSTANDARD3_0
using System.Drawing;
using System.Drawing.Imaging;
#endif
using System.Reflection;
using System.Text.RegularExpressions;

namespace System
{
	using ComponentModel;

	using Linq.Expressions;

	/// <summary>
	/// 对象扩展
	/// </summary>
	[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	public static class FishObjectExtension
	{
#if NETFRAMEWORK

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
		/// 使用指定的图片质量将图片保存到指定位置为JPEG图片
		/// </summary>
		/// <param name="image">要保存的图片</param>
		/// <param name="quality">质量</param>
		public static MemoryStream SaveAsJpeg(this Image image, int quality = 90)
		{
			EncoderParameters parameters = new EncoderParameters(1);
			parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ((long)quality));

			ImageCodecInfo myImageCodecInfo = (from p in ImageCodecInfo.GetImageEncoders() where p.MimeType == "image/jpeg" select p).Single<ImageCodecInfo>();
			var ms = new MemoryStream();
			image.Save(ms, myImageCodecInfo, parameters);

			ms.Seek(0, SeekOrigin.Begin);

			return ms;
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

		#region System.Drawing.Color

		/// <summary>
		/// 获得颜色的WEB RGB表示形式（如#000000）
		/// </summary>
		/// <param name="color">颜色</param>
		/// <param name="upperCase">是否使用大写。默认为 <see langword="true" /></param>
		/// <returns>对应的 <see langword="string" /></returns>
		public static string ToWebRgbColor(this Color color, bool upperCase = true)
		{
			var format = upperCase ? "X2" : "x2";
			return $"#{color.R.ToString(format)}{color.G.ToString(format)}{color.B.ToString(format)}";
		}

		/// <summary>
		/// 获得颜色的WEB RGB表示形式（如 rgba(255,255,255,1)）
		/// </summary>
		/// <param name="color">颜色</param>
		/// <returns>对应的 <see langword="string" /></returns>
		public static string ToWebRgbaColor(this Color color)
		{
			return $"rgba({color.R},{color.G},{color.B},{(color.A / 255.0).ToString("#0.00")})";
		}

		#endregion

#endif

		#region Reflection

#if NETSTANDARD1_6_1 || NETSTANDARD2_0 || NETSTANDARD3_0
		internal static TypeInfo GetTypeInfo(Type type) => type.GetTypeInfo();
#else
		internal static Type GetTypeInfo(Type type) => type;
#endif

		/// <summary>
		/// 根据指定的自定义属性来过滤类型列表
		/// </summary>
		/// <typeparam name="T">要查找的自定义属性类型</typeparam>
		/// <param name="assembly">程序集</param>
		/// <returns>查找到的结果</returns>
		public static Dictionary<Type, T[]> GetFilteredTypeWithCustomerAttribute<T>(this Assembly assembly)
		{
			var dic = assembly.GetTypes().ToDictionary(s => s, s => GetTypeInfo(s).GetCustomAttributes(typeof(T), true).Cast<T>().ToArray());
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
			var dic = GetTypeInfo(type).GetMethods(flags).ToDictionary(s => s, s => s.GetCustomAttributes(typeof(T), true).Cast<T>().ToArray());
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
			var dic = GetTypeInfo(type).GetProperties(flags).ToDictionary(s => s, s => s.GetCustomAttributes(typeof(T), true).Cast<T>().ToArray());
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

#if !NETSTANDARD1_6_1 && !NETSTANDARD2_0 && !NETSTANDARD3_0

		/// <summary>
		/// 对类型列表进行过滤。
		/// </summary>
		/// <typeparam name="T">过滤的类型，可以根据基类、接口或自定义属性进行过滤</typeparam>
		/// <param name="typeList">要过滤的类型列表</param>
		/// <param name="imp">对应的类型</param>
		/// <returns>过滤后的类型列表</returns>
		[Obsolete("This method was meaning less and will be removed soon.")]
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
		[Obsolete("This method was meaning less and will be removed soon.")]
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

#endif
#if !NETSTANDARD1_6_1 && !NETSTANDARD2_0 && !NETSTANDARD3_0

		/// <summary>
		/// 获得类的完整名称（含程序集名称）
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		[Obsolete("This method will be removed soon and not intend to use. Using `type.AssemblyQualifiedName` instead.")]
		public static string GetTypeFullNameWithAssembly(this Type type)
		{
			return type.FullName + ", " + System.IO.Path.GetFileNameWithoutExtension(type.Assembly.Location);
		}

#endif

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

#if !NETSTANDARD1_6_1 && !NETSTANDARD2_0 && !NETSTANDARD3_0

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

#endif

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

		/// <summary>
		/// True if object is value type.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static bool IsNumericType(this object obj)
		{
			switch (Type.GetTypeCode(obj.GetType()))
			{
				case TypeCode.Byte:
				case TypeCode.SByte:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
				case TypeCode.UInt64:
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.Int64:
				case TypeCode.Decimal:
				case TypeCode.Double:
				case TypeCode.Single:
					return true;
				default:
					return false;
			}
		}

		/// <summary>
		/// 添加一个支持属性过滤的事件捕捉句柄
		/// </summary>
		/// <typeparam name="TObj">目标类型</typeparam>
		/// <typeparam name="TProp">属性类型</typeparam>
		/// <param name="obj">对象</param>
		/// <param name="exp">表达式</param>
		/// <param name="handler">事件</param>
		/// <param name="host">绑定的目标对象，如果支持Disposed事件的话，将会在触发时自动清理挂载</param>
		/// <returns>返回一个可以用来取消挂载事件的 <see cref="Action"/></returns>
		public static Action AddPropertyChangingEventHandler<TObj, TProp>(this TObj obj, Expression<Func<TObj, TProp>> exp, EventHandler<PropertyChangingEventArgs> handler, object host = null)
			where TObj : INotifyPropertyChanging
		{
			var propName = exp.GetExpressionAccessedMemberName();
			if (propName.IsNullOrEmpty())
				throw new InvalidOperationException("Unable to figure out which property.");

			if (handler == null)
				throw new ArgumentNullException(nameof(handler));

			var callback = new PropertyChangingEventHandler((sender, e) =>
			{
				if (e.PropertyName == propName)
					handler(sender, e);
			});
			Action unsub = () => obj.PropertyChanging -= callback;
			var dv = host?.GetType().GetTypeInfo().GetEvent("Disposed");
			if (dv != null)
			{
				dv.AddEventHandler(host, new EventHandler(((sender, args) =>
				{
					unsub();
				})));
			}

			return unsub;
		}

		/// <summary>
		/// 添加一个支持属性过滤的事件捕捉句柄
		/// </summary>
		/// <typeparam name="TObj">目标类型</typeparam>
		/// <typeparam name="TProp">属性类型</typeparam>
		/// <param name="obj">对象</param>
		/// <param name="exp">表达式</param>
		/// <param name="handler">事件</param>
		/// <param name="host">绑定的目标对象，如果支持Disposed事件的话，将会在触发时自动清理挂载</param>
		/// <returns>返回一个可以用来取消挂载事件的 <see cref="Action"/></returns>
		public static Action AddPropertyChangedEventHandler<TObj, TProp>(this TObj obj, Expression<Func<TObj, TProp>> exp, EventHandler<PropertyChangedEventArgs> handler, object host = null)
			where TObj : INotifyPropertyChanged
		{
			var propName = exp.GetExpressionAccessedMemberName();
			if (propName.IsNullOrEmpty())
				throw new InvalidOperationException("Unable to figure out which property.");

			if (handler == null)
				throw new ArgumentNullException(nameof(handler));

			var callback = new PropertyChangedEventHandler((sender, e) =>
			{
				if (e.PropertyName == propName)
					handler(sender, e);
			});
			obj.PropertyChanged += callback;

			Action unsub = () => obj.PropertyChanged -= callback;
			var dv = host?.GetType().GetTypeInfo().GetEvent("Disposed");
			if (dv != null)
			{
				dv.AddEventHandler(host, new EventHandler(((sender, args) =>
				{
					unsub();
				})));
			}

			return unsub;
		}
	}

}