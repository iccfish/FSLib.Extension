using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
	using FSLib.Extension;
	using FSLib.Extension.FishLib;

	/// <summary>
	/// 包含了与字符串相关的一些常用扩展方法
	/// </summary>
	[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	public static class FishStringExtension
	{
		#region char

		/// <summary>
		/// 转换为BYTE
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		public static byte ToHexByte(this char code)
		{
			if (code >= 'A' && code <= 'F') return (byte)(10 + (code - 'A'));
			if (code >= 'a' && code <= 'f') return (byte)(10 + (code - 'a'));
			if (code >= '0' && code <= '9') return (byte)(code - '0');
			return 0;
		}

		#endregion

		#region Common

		/// <summary>
		/// 判断当前字符串是否为空或长度为零
		/// </summary>
		/// <param name="str">字符串</param>
		/// <returns>true为空或长度为零</returns>
		public static bool IsNullOrEmpty(this string str)
		{
			return string.IsNullOrEmpty(str);
		}

		/// <summary>
		/// 将字符串分割成行数组
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string[] ToLines(this string value)
		{
			if (String.IsNullOrEmpty(value))
				return null;

			return value.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
		}

		/// <summary>
		/// 替换模板字符
		/// </summary>
		/// <param name="template">模板内容</param>
		/// <param name="tags">标签数组</param>
		/// <param name="dest">内容数组</param>
		/// <returns>替换后的结果</returns>
		public static string TemplateTagReplace(this string template, string[] tags, string[] dest)
		{
			if (tags.Length != dest.Length) throw new InvalidOperationException("数组长度必须一样.");

			for (int i = 0; i < tags.Length; i++)
			{
				template = template.Replace(tags[i], dest[i]);
			}

			return template;
		}

		static readonly Regex ExpressConvertor = new Regex(@"(\r|\n|'|""|\\|/)");

		/// <summary>
		/// 将字符串转义为表达式
		/// </summary>
		/// <param name="value">要转换的字符串</param>
		/// <returns>转义后的结果</returns>
		public static string EncodeToJsExpression(this string value)
		{
			if (value.IsNullOrEmpty()) return string.Empty;

			return ExpressConvertor.Replace(value, (s) =>
			{
				if (s.Value == "\r") return "\\r";
				if (s.Value == "\n") return "\\n";
				return string.Concat("\\", s.Value);
			});
		}

		static readonly Regex ExpressReConvertor = new Regex(@"\\(r|n|'|""|\|/|u([a-fA-F0-9]{4}))");

		/// <summary>
		/// 将字符串从JS的转义中转换
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string DecodeFromJsExpression(this string value)
		{
			if (value.IsNullOrEmpty()) return string.Empty;

			return ExpressReConvertor.Replace(value, (s) =>
			{
				var key = s.Groups[1].Value;

				if (key == "r") return "\r";
				if (key == "n") return "\n";
				if (key[0] == 'u')
				{
					return ((char)s.Groups[2].Value.ToInt32(style: NumberStyles.AllowHexSpecifier)).ToString();
				}
				return key;
			});
		}

		/// <summary>
		/// 为字符串设定默认值
		/// </summary>
		/// <param name="value">要设置的值</param>
		/// <param name="defaultValue">如果要设定的值为空，则返回此默认值</param>
		/// <returns>设定后的结果</returns>
		public static string DefaultForEmpty(this string value, string defaultValue)
		{
			return string.IsNullOrEmpty(value) ? defaultValue : value;
		}

		/// <summary>
		/// 使用指定参数来对当前字符串进行格式化
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="args">The args.</param>
		/// <returns></returns>
		[StringFormatMethod("value")]
		public static string FormatWith(this string value, params object[] args)
		{
			if (value == null) throw new ArgumentNullException("value");
			else if (value.Length == 0) return string.Empty;
			else if (args.Length == 0) return value;
			else return string.Format(value, args);
		}

		static readonly Regex _emailReg = new Regex(@"^\w+[\.\-_0-9a-z]*@[0-9a-z]+([\-_\.][0-9a-z]+)*\.[a-z]{2,3}$", RegexOptions.IgnoreCase);

		/// <summary>
		/// 判断一个字符串是否是邮件地址
		/// </summary>
		/// <param name="value">地址</param>
		/// <returns>如果是，则返回 true</returns>
		public static bool IsEmail(this string value)
		{
			return !value.IsNullOrEmpty() && _emailReg.IsMatch(value);
		}

		/// <summary>
		/// 比较两个字符串在忽略大小写的情况下是否相等
		/// </summary>
		/// <param name="value">字符串1</param>
		/// <param name="compareTo">要比较的字符串</param>
		/// <returns>是否相等</returns>
		public static bool IsIgnoreCaseEqualTo(this string value, string compareTo)
		{
			return string.Compare(value, compareTo, true) == 0;
		}

		/// <summary>
		/// 按照字节截取字符串
		/// </summary>
		/// <param name="value">字符串</param>
		/// <param name="byteLength">字节长度，一个汉字两个字节</param>
		/// <returns>截取后的字符串</returns>
		/// <exception cref="System.ArgumentException">指定了截取后的省略字符串，但要截取的字符串过短，不足以容纳省略字符串</exception>
		public static string GetSubString(this string value, int byteLength)
		{
			return GetSubString(value, byteLength, string.Empty);
		}
		/// <summary>
		/// 按照字节截取字符串
		/// </summary>
		/// <param name="value">字符串</param>
		/// <param name="byteLength">字节长度，一个汉字两个字节</param>
		/// <param name="tailString">如果截取了，那么省略字符串</param>
		/// <returns>截取后的字符串</returns>
		/// <exception cref="System.ArgumentException">指定了截取后的省略字符串，但要截取的字符串过短，不足以容纳省略字符串</exception>
		public static string GetSubString(this string value, int byteLength, string tailString)
		{
			if (value.IsNullOrEmpty()) return value;
			tailString = tailString ?? "";
			var tailLength = tailString.Select(s => (int)s > 255 ? 2 : 1).Sum();

			if (tailLength > 0) byteLength -= tailLength;
			if (byteLength < 1) throw new ArgumentException(SR.StringExtract_GetSubString_LengthError);

			var currentLength = 0;
			var wordPosition = 0;
			while (currentLength < byteLength && wordPosition < value.Length) currentLength += value[wordPosition++] > 255 ? 2 : 1;
			if (wordPosition == value.Length) return value;
			else return value.Substring(0, wordPosition) + tailString;
		}

		/// <summary>
		/// 确认字符串是以指定字符串结尾的
		/// </summary>
		/// <param name="value">字符串</param>
		/// <param name="ending">结尾</param>
		/// <returns></returns>
		public static string EnsureEndWith(this string value, string ending)
		{
			if (value == null || value.EndsWith(ending)) return value;
			return value + ending;
		}

		/// <summary>
		/// 获得指定字符串的字节长度
		/// </summary>
		/// <param name="value">值</param>
		/// <returns><see cref="T:System.Int32"/></returns>
		public static int GetByteCount(this string value)
		{
			return GetByteCount(value, Encoding.Unicode);
		}

		/// <summary>
		/// 获得指定字符串的字节长度
		/// </summary>
		/// <param name="value">值</param>
		/// <param name="encoding">编码</param>
		/// <returns><see cref="T:System.Int32"/></returns>
		public static int GetByteCount(this string value, Encoding encoding)
		{
			if (value.IsNullOrEmpty()) return 0;
			return encoding.GetByteCount(value);
		}

		/// <summary>
		/// 转换为字节数组
		/// </summary>
		/// <param name="value">字符串值</param>
		/// <returns>结果字节数组</returns>
		public static byte[] ToBytes(this string value)
		{
			return ToBytes(value, null);
		}
		/// <summary>
		/// 转换为字节数组
		/// </summary>
		/// <param name="value">字符串值</param>
		/// <param name="encoding">使用的编码</param>
		/// <returns>结果字节数组</returns>
		public static byte[] ToBytes(this string value, Encoding encoding)
		{
			return value.IsNullOrEmpty() ? new byte[] { } :
				(encoding ?? Encoding.Unicode).GetBytes(value);
		}

		/// <summary>
		/// 返回指定的字符串中是否包含另外一个子字符串
		/// </summary>
		/// <param name="str">字符串</param>
		/// <param name="key">关键字</param>
		/// <param name="comparison">比较方式</param>
		/// <returns>包含为true， 否则为false</returns>
		public static bool Contains(this string str, string key, StringComparison comparison)
		{
			return str.IndexOf(key, comparison) != -1;
		}


		/// <summary>
		/// 按照标签分割并枚举
		/// </summary>
		/// <param name="text">文本</param>
		/// <param name="startTag">开始标签</param>
		/// <param name="endTag">结束标签</param>
		/// <param name="startPos">开始位置。默认为0</param>
		/// <returns>符合要求的代码片段</returns>
		public static IEnumerable<string> SplitByTag(this string text, string startTag, string endTag, int startPos = 0)
		{
			var index = 0;
			while ((index = text.IndexOf(startTag, startPos, StringComparison.OrdinalIgnoreCase)) != -1)
			{
				var endIndex = text.IndexOf(endTag, index + startTag.Length, StringComparison.OrdinalIgnoreCase);
				if (endIndex == -1) yield break;

				var str = text.Substring(index, endIndex - index + endTag.Length);
				startPos = endIndex + endTag.Length + 1;
				yield return str;
			}
		}

		/// <summary>
		/// 对字符串进行正则表达式匹配，并返回所有匹配的字符串数组
		/// </summary>
		/// <param name="text">字符串</param>
		/// <param name="pattern">正则表达式模式</param>
		/// <param name="options">选项</param>
		/// <returns>如果匹配失败，则返回false</returns>
		public static string[] RegMatch(this string text, string pattern, RegexOptions options = RegexOptions.IgnoreCase)
		{
			var m = Regex.Match(text, pattern, options);
			if (!m.Success) return null;

			return m.Groups.Cast<Group>().Select(s => s.Value).ToArray();
		}

		/// <summary>
		/// 对字符串进行正则表达式匹配，并返回所有匹配的字符串数组
		/// </summary>
		/// <param name="text">字符串</param>
		/// <param name="pattern">正则表达式模式</param>
		/// <param name="options">选项</param>
		/// <returns>如果匹配失败，则返回false</returns>
		public static List<string[]> RegMatches(this string text, string pattern, RegexOptions options = RegexOptions.IgnoreCase)
		{
			var m = Regex.Matches(text, pattern, options);

			return m.Cast<Match>().Select(s => s.Groups.Cast<Group>().Select(x => x.Value).ToArray()).ToList();
		}

		/// <summary>
		/// 在字符串中搜索指定的特征字符串并截取其中的一段。
		/// </summary>
		/// <param name="text">源字符串</param>
		/// <param name="beginTag">开始特征字符串</param>
		/// <param name="includeTag">是否包含标签本身，默认为 <see langword="true" /></param>
		/// <param name="endTag">结束特征字符串</param>
		/// <param name="beginIndex">开始索引</param>
		/// <param name="comparison">比较类型</param>
		/// <returns></returns>
		public static string SearchStringTag(this string text, string beginTag, string endTag, int beginIndex = 0, bool includeTag = true, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
		{
			return SearchStringTag(text, beginTag, endTag, ref beginIndex, includeTag, comparison);
		}

		/// <summary>
		/// 在字符串中搜索指定的特征字符串并截取其中的一段。
		/// </summary>
		/// <param name="text">源字符串</param>
		/// <param name="beginTag">开始特征字符串</param>
		/// <param name="endTag">结束特征字符串</param>
		/// <param name="beginIndex">开始索引。当搜索完成后，将会指向匹配结束后的下一个位置</param>
		/// <param name="includeTag">是否包含标签本身，默认为 <see langword="true" /></param>
		/// <param name="comparison">比较类型</param>
		/// <returns></returns>
		public static string SearchStringTag(this string text, string beginTag, string endTag, ref int beginIndex, bool includeTag = true, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
		{
			if (string.IsNullOrEmpty(text) || beginIndex >= text.Length)
				return string.Empty;

			var startIndex = beginIndex;
			var endIndex = text.Length;
			var tagStartEmpty = beginTag.IsNullOrEmpty();
			var tagEndEmpty = endTag.IsNullOrEmpty();

			if (!tagStartEmpty)
				startIndex = text.IndexOf(beginTag, startIndex, comparison);
			if (startIndex == -1)
				return string.Empty;

			if (!tagEndEmpty)
				endIndex = text.IndexOf(endTag, startIndex + (!tagStartEmpty ? beginTag.Length + 1 : 1), comparison);
			if (endIndex == -1)
				return string.Empty;

			beginIndex = endIndex + (!tagEndEmpty ? endTag.Length + 1 : 1);

			return text.Substring(
				startIndex + (includeTag || tagStartEmpty ? 0 : beginTag.Length),
				endIndex - startIndex - (includeTag || tagStartEmpty ? 0 : beginTag.Length) + (tagEndEmpty || !includeTag ? 0 : endTag.Length)
			);
		}


		#endregion

		#region 流操作



		#endregion

		#region MD5

		/// <summary>
		/// 计算指定字符串的MD5值
		/// </summary>
		/// <param name="key">要计算Hash的字符串</param>
		/// <returns>字符串的Hash</returns>
		public static string MD5(this string key)
		{
			return key.MD5(System.Text.Encoding.UTF8);
		}

		/// <summary>
		/// 计算指定字符串的MD5值
		/// </summary>
		/// <param name="key">要计算Hash的字符串</param>
		/// <param name="encoding">计算Hash的编码方法</param>
		/// <returns>字符串的Hash</returns>
		public static string MD5(this string key, string encoding)
		{
			return key.MD5(System.Text.Encoding.GetEncoding(encoding));
		}

		/// <summary>
		/// 计算指定字符串的MD5值
		/// </summary>
		/// <param name="key">要计算Hash的字符串</param>
		/// <param name="encoding">计算Hash的编码方法</param>
		/// <returns>字符串的Hash</returns>
		public static string MD5(this string key, System.Text.Encoding encoding)
		{
			if (key == null) throw new ArgumentNullException();

			var md5 = Security.Cryptography.MD5.Create();
			var has = md5.ComputeHash(encoding.GetBytes(key));
			return BitConverter.ToString(has).Replace("-", "").ToUpper();
		}

		#endregion


		#region SHA1

		/// <summary>
		/// 计算指定字符串的SHA1值
		/// </summary>
		/// <param name="key">要计算Hash的字符串</param>
		/// <returns>字符串的Hash</returns>
		public static string SHA1(this string key)
		{
			return key.SHA1(System.Text.Encoding.UTF8);
		}

		/// <summary>
		/// 计算指定字符串的SHA1值
		/// </summary>
		/// <param name="key">要计算Hash的字符串</param>
		/// <param name="encoding">计算Hash的编码方法</param>
		/// <returns>字符串的Hash</returns>
		public static string SHA1(this string key, string encoding)
		{
			return key.SHA1(System.Text.Encoding.GetEncoding(encoding));
		}

		/// <summary>
		/// 计算指定字符串的SHA1值
		/// </summary>
		/// <param name="key">要计算Hash的字符串</param>
		/// <param name="encoding">计算Hash的编码方法</param>
		/// <returns>字符串的Hash</returns>
		public static string SHA1(this string key, System.Text.Encoding encoding)
		{
			if (key == null) throw new ArgumentNullException();

			var md5 = Security.Cryptography.SHA1.Create();
			var has = md5.ComputeHash(encoding.GetBytes(key));
			return BitConverter.ToString(has).Replace("-", "").ToUpper();
		}

		#endregion


		#region ToInt32

		/// <summary>
		/// 将字符串转换为Int值，如果转换失败，则返回0
		/// </summary>
		/// <param name="value">字符串</param>
		/// <param name="defaultValue">如果转换失败,则返回的默认值</param>
		/// <returns>转换后的 <see cref="System.Int32"/></returns>
		public static int ToInt32(this string value, int defaultValue = 0, NumberStyles style = NumberStyles.Any, IFormatProvider provider = null)
		{
			int temp;
			return int.TryParse(value, style, provider, out temp) ? temp : defaultValue;
		}

		/// <summary>
		/// 将字符串转换为Int值，如果转换失败，则返回null
		/// </summary>
		/// <param name="value">字符串</param>
		/// <returns>转换后的 <see cref="System.Int32"/></returns>
		public static int? ToInt32Nullable(this string value, NumberStyles style = NumberStyles.Any, IFormatProvider provider = null)
		{
			int temp;
			return int.TryParse(value, style, provider, out temp) ? (int?)temp : null;
		}

		static char[] StringSpliter = new char[] { ',', '|', '\\', '/', ':', ';', '_', '#', '$', '%', '@', '!', '^', '&', '*' };

		/// <summary>
		/// 将字符串分割为整数数组
		/// </summary>
		/// <param name="value">要分割的字符串</param>
		/// <returns>返回最终的 <see cref="System.Int32"/>数组</returns>
		public static int[] SplitAsIntArray(this string value, NumberStyles style = NumberStyles.Any, IFormatProvider provider = null)
		{
			if (string.IsNullOrEmpty(value)) return new int[0];
			return value.Split(StringSpliter, StringSplitOptions.RemoveEmptyEntries)
				.Select(s => s.ToInt32(style: style, provider: provider)).ToArray();
		}

		#endregion

		#region ToInt64

		/// <summary>
		/// 将字符串转换为Int值
		/// </summary>
		/// <param name="value">字符串</param>
		/// <param name="defaultValue">如果转换失败,则返回的默认值</param>
		/// <returns>转换后的 <see cref="System.Int64"/></returns>
		public static long ToInt64(this string value, long defaultValue = 0, NumberStyles style = NumberStyles.Any, IFormatProvider provider = null)
		{
			long temp;
			return long.TryParse(value, style, provider, out temp) ? temp : defaultValue;
		}

		/// <summary>
		/// 将字符串转换为Int值
		/// </summary>
		/// <param name="value">字符串</param>
		/// <returns>转换后的 <see cref="System.Int64"/></returns>
		public static long? ToInt64Nullable(this string value, NumberStyles style = NumberStyles.Any, IFormatProvider provider = null)
		{
			long temp;
			return long.TryParse(value, style, provider, out temp) ? (long?)temp : null;
		}

		#endregion

		#region ToSingle

		/// <summary>
		/// 转换字符串为浮点数.如果转换失败,则返回指定的默认值
		/// </summary>
		/// <param name="value">要转换的字符串</param>
		/// <param name="defaultValue">如果转换失败,则返回的默认值</param>
		/// <returns>转换后的 <see cref="System.Single"/></returns>
		public static float ToSingle(this string value, float defaultValue = 0.0F, NumberStyles style = NumberStyles.Any, IFormatProvider provider = null)
		{
			float temp;
			return float.TryParse(value, style, provider, out temp) ? temp : defaultValue;
		}

		/// <summary>
		/// 转换字符串为浮点数.如果转换失败,则返回null
		/// </summary>
		/// <param name="value">要转换的字符串</param>
		/// <returns>转换后的 <see cref="System.Single"/></returns>
		public static float? ToSingleNullable(this string value, NumberStyles style = NumberStyles.Any, IFormatProvider provider = null)
		{
			float temp;
			return float.TryParse(value, style, provider, out temp) ? (float?)temp : null;
		}

		#endregion

		#region ToDateTime

		/// <summary>
		/// 转换字符串为日期时间.如果转换失败,则返回指定的默认值
		/// </summary>
		/// <param name="value">要转换的字符串</param>
		/// <param name="defaultValue">如果转换失败,则返回的默认值</param>
		/// <param name="formatProvider">格式</param>
		/// <param name="styles"></param>
		/// <returns>
		/// 转换后的 <see cref="System.DateTime"/>
		/// </returns>
		public static DateTime ToDateTime(this string value, DateTime defaultValue, IFormatProvider formatProvider = null, DateTimeStyles styles = DateTimeStyles.None)
		{
			DateTime temp;
			return DateTime.TryParse(value, formatProvider, styles, out temp) ? temp : defaultValue;
		}


		/// <summary>
		/// 转换字符串为日期时间.如果转换失败,则返回指定的默认值
		/// </summary>
		/// <param name="value">要转换的字符串</param>
		/// <param name="defaultValue">如果转换失败,则返回的默认值</param>
		/// <returns>转换后的 <see cref="System.DateTime"/></returns>
		public static DateTime ToDateTimeExact(this string value, DateTime defaultValue, string format, IFormatProvider formatProvider = null, DateTimeStyles styles = DateTimeStyles.None)
		{
			DateTime temp;
			return DateTime.TryParseExact(value, format, formatProvider, styles, out temp) ? temp : defaultValue;
		}


		/// <summary>
		/// 转换字符串为日期时间.如果转换失败,则返回指定的默认值
		/// </summary>
		/// <param name="value">要转换的字符串</param>
		/// <param name="defaultValue">如果转换失败,则返回的默认值</param>
		/// <returns>转换后的 <see cref="System.DateTime"/></returns>
		public static DateTime ToDateTimeExact(this string value, DateTime defaultValue, string[] formats, IFormatProvider formatProvider = null, DateTimeStyles styles = DateTimeStyles.None)
		{
			DateTime temp;
			return DateTime.TryParseExact(value, formats, formatProvider, styles, out temp) ? temp : defaultValue;
		}


		/// <summary>
		/// 转换字符串为日期时间.如果转换失败,则返回指定的默认值
		/// </summary>
		/// <param name="value">要转换的字符串</param>
		/// <returns>转换后的 <see cref="System.DateTime"/></returns>
		public static DateTime ToDateTime(this string value, IFormatProvider formatProvider = null, DateTimeStyles styles = DateTimeStyles.None)
		{
			DateTime temp;
			return DateTime.TryParse(value, formatProvider, styles, out temp) ? temp : DateTime.MinValue;
		}

		/// <summary>
		/// 转换字符串为日期时间.如果转换失败,则返回指定的默认值
		/// </summary>
		/// <param name="value">要转换的字符串</param>
		/// <returns>转换后的 <see cref="System.DateTime"/></returns>
		public static DateTime ToDateTimeExact(this string value, string[] format = null, IFormatProvider formatProvider = null, DateTimeStyles styles = DateTimeStyles.None)
		{
			DateTime temp;
			return DateTime.TryParseExact(value, format, formatProvider, styles, out temp) ? temp : DateTime.MinValue;
		}

		/// <summary>
		/// 转换字符串为日期时间.如果转换失败,则返回 <see cref="F:System.DataTime.MinValue"/>
		/// </summary>
		/// <param name="value">要转换的字符串</param>
		/// <returns>转换后的 <see cref="System.DateTime"/></returns>
		public static DateTime ToDateTimeExact(this string value, string format, IFormatProvider formatProvider = null, DateTimeStyles styles = DateTimeStyles.None)
		{
			DateTime temp;
			return DateTime.TryParseExact(value, format, formatProvider, styles, out temp) ? temp : DateTime.MinValue;
		}

		/// <summary>
		/// 转换字符串为日期时间.如果转换失败,则返回null
		/// </summary>
		/// <param name="value">要转换的字符串</param>
		/// <returns>转换后的 <see cref="System.DateTime"/></returns>
		public static DateTime? ToDateTimeNullable(this string value, IFormatProvider formatProvider = null, DateTimeStyles styles = DateTimeStyles.None)
		{
			DateTime temp;
			return DateTime.TryParse(value, out temp) ? (DateTime?)temp : null;
		}


		/// <summary>
		/// 转换字符串为日期时间.如果转换失败,则返回null
		/// </summary>
		/// <param name="value">要转换的字符串</param>
		/// <returns>转换后的 <see cref="System.DateTime"/></returns>
		public static DateTime? ToDateTimeNullableExact(this string value, string format, IFormatProvider formatProvider = null, DateTimeStyles styles = DateTimeStyles.None)
		{
			DateTime temp;
			return DateTime.TryParseExact(value, format, formatProvider, styles, out temp) ? (DateTime?)temp : null;
		}

		/// <summary>
		/// 转换字符串为日期时间.如果转换失败,则返回null
		/// </summary>
		/// <param name="value">要转换的字符串</param>
		/// <returns>转换后的 <see cref="System.DateTime"/></returns>
		public static DateTime? ToDateTimeNullableExact(this string value, string[] formats, IFormatProvider formatProvider = null, DateTimeStyles styles = DateTimeStyles.None)
		{
			DateTime temp;
			return DateTime.TryParseExact(value, formats, formatProvider, styles, out temp) ? (DateTime?)temp : null;
		}

#if NET40

		/// <summary>
		/// 将字符串分析为可空Timespan
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static TimeSpan? ToTimeSpanNullable(this string value, IFormatProvider formatProvider = null)
		{
			TimeSpan ts;
			if (TimeSpan.TryParse(value, formatProvider, out ts))
				return ts;

			return null;
		}


		/// <summary>
		/// 将字符串分析为可空Timespan
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static TimeSpan? ToTimeSpanNullableExact(this string value, string format, IFormatProvider formatProvider = null, TimeSpanStyles styles = TimeSpanStyles.None)
		{
			TimeSpan ts;
			if (TimeSpan.TryParseExact(value, format, formatProvider, styles, out ts))
				return ts;

			return null;
		}

		/// <summary>
		/// 将字符串分析为可空Timespan
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static TimeSpan? ToTimeSpanNullableExact(this string value, string[] format, IFormatProvider formatProvider = null, TimeSpanStyles styles = TimeSpanStyles.None)
		{
			TimeSpan ts;
			if (TimeSpan.TryParseExact(value, format, formatProvider, styles, out ts))
				return ts;

			return null;
		}


		/// <summary>
		/// 将字符串分析为Timespan
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static TimeSpan ToTimeSpan(this string value, TimeSpan timeSpan, IFormatProvider formatProvider = null)
		{
			TimeSpan ts;
			if (TimeSpan.TryParse(value, formatProvider, out ts))
				return ts;

			return timeSpan;
		}


		/// <summary>
		/// 将字符串分析为Timespan
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static TimeSpan ToTimeSpanExact(this string value, TimeSpan timeSpan, string format, IFormatProvider formatProvider = null, TimeSpanStyles styles = TimeSpanStyles.None)
		{
			TimeSpan ts;
			if (TimeSpan.TryParseExact(value, format, formatProvider, styles, out ts))
				return ts;

			return timeSpan;
		}

		/// <summary>
		/// 将字符串分析为Timespan
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static TimeSpan ToTimeSpanExact(this string value, TimeSpan timeSpan, string[] format, IFormatProvider formatProvider = null, TimeSpanStyles styles = TimeSpanStyles.None)
		{
			TimeSpan ts;
			if (TimeSpan.TryParseExact(value, format, formatProvider, styles, out ts))
				return ts;

			return timeSpan;
		}


		/// <summary>
		/// 将字符串分析为Timespan
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static TimeSpan ToTimeSpan(this string value, IFormatProvider formatProvider = null)
		{
			TimeSpan ts;
			if (TimeSpan.TryParse(value, formatProvider, out ts))
				return ts;

			return TimeSpan.Zero;
		}


		/// <summary>
		/// 将字符串分析为Timespan
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static TimeSpan ToTimeSpanExact(this string value, string format, IFormatProvider formatProvider = null, TimeSpanStyles styles = TimeSpanStyles.None)
		{
			TimeSpan ts;
			if (TimeSpan.TryParseExact(value, format, formatProvider, styles, out ts))
				return ts;

			return TimeSpan.Zero;
		}

		/// <summary>
		/// 将字符串分析为Timespan
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static TimeSpan ToTimeSpanExact(this string value, string[] format, IFormatProvider formatProvider = null, TimeSpanStyles styles = TimeSpanStyles.None)
		{
			TimeSpan ts;
			if (TimeSpan.TryParseExact(value, format, formatProvider, styles, out ts))
				return ts;

			return TimeSpan.Zero;
		}
#endif


		#endregion

		#region ToDouble

		/// <summary>
		/// 转换字符串为双精度数.如果转换失败,则返回 0.0
		/// </summary>
		/// <param name="value">要转换的字符串</param>
		/// <param name="defaultValue">默认值，默认为 0.0</param>
		/// <param name="styles">分析的格式</param>
		/// <param name="formatProvider">区域信息</param>
		/// <returns>转换后的 <see cref="System.Double"/></returns>
		public static double ToDouble(this string value, double defaultValue = 0.0, NumberStyles styles = NumberStyles.Any, IFormatProvider formatProvider = null)
		{
			double temp;
			return double.TryParse(value, styles, formatProvider, out temp) ? temp : defaultValue;
		}


		/// <summary>
		/// 转换字符串为双精度数.如果转换失败,则返回指定的默认值
		/// </summary>
		/// <param name="value">要转换的字符串</param>
		/// <returns>转换后的 <see cref="System.Double"/></returns>
		public static double? ToDoubleNullable(this string value, NumberStyles styles = NumberStyles.Any, IFormatProvider formatProvider = null)
		{
			double temp;
			return double.TryParse(value, styles, formatProvider, out temp) ? (double?)temp : null;
		}

		#endregion

		#region ToDecimal

		/// <summary>
		/// 转换字符串为双精度数.如果转换失败,则返回指定的默认值
		/// </summary>
		/// <param name="value">要转换的字符串</param>
		/// <param name="defaultValue">如果转换失败,则返回的默认值</param>
		/// <returns>转换后的 <see cref="System.Double"/></returns>
		public static decimal ToDecimal(this string value, decimal defaultValue = 0m, NumberStyles styles = NumberStyles.Any, IFormatProvider formatProvider = null)
		{
			decimal temp;
			return decimal.TryParse(value, styles, formatProvider, out temp) ? temp : defaultValue;
		}

		/// <summary>
		/// 转换字符串为双精度数.如果转换失败,则返回 0.0
		/// </summary>
		/// <param name="value">要转换的字符串</param>
		/// <returns>转换后的 <see cref="System.Double"/></returns>
		public static decimal? ToDecimalNullable(this string value, NumberStyles styles = NumberStyles.Any, IFormatProvider formatProvider = null)
		{
			decimal temp;
			return decimal.TryParse(value, styles, formatProvider, out temp) ? (decimal?)temp : null;
		}

		#endregion

#if !NETSTANDARD1_6_1 && !NETSTANDARD2_0 && !NETSTANDARD3_0

		#region ToPoint

		/// <summary>
		/// 将字符串转换为坐标点格式
		/// </summary>
		/// <param name="location">字符串</param>
		/// <returns><see cref="T:System.Drawing.Point"/></returns>
		public static System.Drawing.Point ParseToPoint(this string location)
		{
			if (string.IsNullOrEmpty(location))
				return System.Drawing.Point.Empty;

			var pts = location.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			if (pts.Length < 2) return System.Drawing.Point.Empty;

			return new System.Drawing.Point(pts[0].ToInt32(), pts[1].ToInt32());
		}

		#endregion

#endif

		#region 对象扩展

		/// <summary>
		/// 转换为文件夹对象
		/// </summary>
		/// <param name="folder">文件夹路径</param>
		/// <returns>对应的文件夹信息对象</returns>
		public static System.IO.DirectoryInfo AsDirectoryInfo(this string folder)
		{
			return new System.IO.DirectoryInfo(folder);
		}

		/// <summary>
		/// 转换为文件信息对象
		/// </summary>
		/// <param name="filePath">文件路径</param>
		/// <returns><see cref="T:System.IO.FileInfo"/></returns>
		public static System.IO.FileInfo AsFileInfo(this string filePath)
		{
			return new System.IO.FileInfo(filePath);
		}

		#endregion

		#region ToByte[]

		/// <summary>
		/// 将Base64格式的字符串转换为字节数组
		/// </summary>
		/// <param name="base64String">要转换的Base64字符串</param>
		/// <returns>字节数组</returns>
		public static byte[] ConvertBase64ToBytes(this string base64String)
		{
			if (base64String.IsNullOrEmpty()) return null;

			return Convert.FromBase64String(base64String);
		}


		/// <summary>
		/// 压缩数据组
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static byte[] Compress(this string source)
		{
			using (var ms = new System.IO.MemoryStream())
			using (var gzip = new System.IO.Compression.GZipStream(ms, IO.Compression.CompressionMode.Compress))
			using (var sw = new System.IO.StreamWriter(gzip, System.Text.Encoding.UTF8))
			{
				sw.Write(source);
				sw.Dispose();
				return ms.ToArray();
			}
		}

		/// <summary>
		/// 解压缩数据组
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static string DecompressAsString(this byte[] source)
		{
			using (var ms = new System.IO.MemoryStream(source))
			using (var gzip = new System.IO.Compression.GZipStream(ms, IO.Compression.CompressionMode.Decompress))
			using (var sr = new System.IO.StreamReader(gzip, System.Text.Encoding.UTF8, true))
			{
				return sr.ReadToEnd();
			}
		}

		/// <summary>
		/// 将HEX字符串转换为对应的字节数组
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static byte[] ConvertHexStringToBytes(this string source)
		{
			if (source.IsNullOrEmpty())
				return new byte[0];

			var matches = Regex.Matches(source, @"[a-f\d]{2}", RegexOptions.IgnoreCase);
			return matches.Cast<Match>().Select(s => (byte)((s.Value[0].ToHexByte() << 4) + s.Value[0].ToHexByte())).ToArray();
		}

		#endregion

#if !NETSTANDARD1_6_1 && !NETSTANDARD2_0 && !NETSTANDARD3_0

		#region 其它对象到字符串转换

		/// <summary>
		/// 将坐标点转换为字符串格式
		/// </summary>
		/// <param name="point">坐标点位置</param>
		/// <returns>
		/// 字符串格式
		/// </returns>
		public static string ToStringFormat(this System.Drawing.Point point)
		{
			return point.X + "," + point.Y;
		}

		#endregion

#endif
	}
}
