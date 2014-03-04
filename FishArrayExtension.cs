using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace System
{
	/// <summary>
	/// 对数组的扩展方法
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
	public static class FishArrayExtension
	{
		/// <summary>
		/// 对象数组进行过滤，并返回新的数组
		/// </summary>
		/// <typeparam name="T">数组类型</typeparam>
		/// <param name="array">原数组</param>
		/// <param name="filterExpression">过滤比较</param>
		/// <returns>过滤后的数组</returns>
		/// <exception cref="System.ArgumentNullException">array</exception>
		public static T[] Filter<T>(this T[] array, Func<int, T, bool> filterExpression)
		{
			if (array == null) throw new ArgumentNullException("array");
			if (filterExpression == null) throw new ArgumentNullException("filterExpression");

			var list = new List<T>(array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				if (filterExpression(i, array[i])) list.Add(array[i]);
			}

			return list.ToArray();
		}

		/// <summary>
		/// 遍历数组，并对数组执行指定操作
		/// </summary>
		/// <typeparam name="T">数组类型</typeparam>
		/// <param name="array">原数组</param>
		/// <param name="action">执行的操作</param>
		/// <exception cref="System.ArgumentNullException">array</exception>
		public static void ForEachWithIndex<T>(this T[] array, Action<int, T> action)
		{
			if (array == null) throw new ArgumentNullException("array");
			if (action == null) throw new ArgumentNullException("action");

			for (int i = 0; i < array.Length; i++)
			{
				action(i, array[i]);
			}
		}

		/// <summary>
		/// 测试一个元素是否在数组中
		/// </summary>
		/// <typeparam name="T">数组类型</typeparam>
		/// <param name="array">原数组</param>
		/// <param name="value">值</param>
		/// <param name="comparer">比较器</param>
		/// <returns>是否在数组中</returns>
		/// <exception cref="System.ArgumentNullException">array</exception>
		public static bool Contains<T>(this T[] array, T value, Func<T, T, bool> comparer)
		{
			if (array == null) throw new ArgumentNullException("array");
			if (comparer == null) throw new ArgumentNullException("comparer");


			foreach (T item in array)
			{
				if (comparer(value, item)) return true;
			}

			return false;
		}

		/// <summary>
		/// 对可遍历对象进行遍历并进行指定操作
		/// </summary>
		/// <typeparam name="T">遍历的类型</typeparam>
		/// <param name="enumberable">对象</param>
		/// <param name="predicate">函数委托</param>
		/// <exception cref="System.ArgumentNullException">predicate</exception>
		public static void ForEach<T>(this IEnumerable<T> enumberable, Action<T> predicate)
		{
			if (enumberable == null)
				throw new ArgumentNullException("enumberable", "enumberable is null.");
			if (predicate == null)
				throw new ArgumentNullException("predicate", "predicate is null.");

			foreach (T item in enumberable)
			{
				predicate(item);
			}
		}

		/// <summary>
		/// 获得指定数组是否为空
		/// </summary>
		/// <typeparam name="T">数组类型</typeparam>
		/// <param name="array">要检测的数组</param>
		/// <returns>如果为空或长度为零的数组，则返回true</returns>
		public static bool IsEmpty<T>(this T[] array)
		{
			return array == null || array.Length == 0;
		}

		/// <summary>
		/// 使用指定的分隔符将字符串数组连接起来
		/// </summary>
		/// <param name="array">字符串数组</param>
		/// <param name="seperator">分隔符</param>
		/// <returns>参见 <see cref="T:System.String"/></returns>
		public static string Join(this string[] array, string seperator)
		{
			return string.Join(seperator, array);
		}

		/// <summary>
		/// 使用指定的分隔符将字符串可遍历对象连接起来
		/// </summary>
		/// <param name="array">字符串可遍历对象</param>
		/// <param name="seperator">分隔符</param>
		/// <returns>参见 <see cref="T:System.String"/></returns>
		public static string Join(this IEnumerable<string> array, string seperator)
		{
			return array.ToArray().Join(seperator);
		}

		/// <summary>
		/// 使用指定的分隔符将字符串可遍历对象连接起来
		/// </summary>
		/// <param name="array">字符串可遍历对象</param>
		/// <param name="seperator">分隔符</param>
		/// <returns>参见 <see cref="T:System.String"/></returns>
		public static string Join(this IEnumerable<char> array, string seperator)
		{
			return array.Select(s => s.ToString()).ToArray().Join(seperator);
		}

		///<summary>
		///	检测索引是否在数组的范围之内
		///</summary>
		///<param name = "source">源数组</param>
		///<param name = "index">检查的索引</param>
		///<param name="dimension">检查的维度</param>
		///<returns><c>true</c> 表示有效，<c>false</c> 表示索引超过范围</returns>
		public static bool WithinIndex(this Array source, int index)
		{
			return WithinIndex(source, index, 0);
		}

		///<summary>
		///	检测索引是否在数组的范围之内
		///</summary>
		///<param name = "source">源数组</param>
		///<param name = "index">检查的索引</param>
		///<param name="dimension">检查的维度</param>
		///<returns><c>true</c> 表示有效，<c>false</c> 表示索引超过范围</returns>
		public static bool WithinIndex(this Array source, int index, int dimension)
		{
			return source != null && index >= source.GetLowerBound(dimension) && index <= source.GetUpperBound(dimension);
		}

		/// <summary>
		/// 清空指定的数组
		/// </summary>
		/// <typeparam name="T">数组类型</typeparam>
		/// <param name="array">要清空的数组</param>
		public static void Clear<T>(this T[] array)
		{
			if (array == null) return;

			Array.Clear(array, 0, array.Length);
		}

		/// <summary>
		/// 查找索引
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="src"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static int FindIndex<T>(this IEnumerable<T> src, Func<T, bool> predicate)
		{
			if (src == null)
				throw new ArgumentNullException("src", "src is null.");
			if (predicate == null)
				throw new ArgumentNullException("predicate", "predicate is null.");

			var index = 0;
			foreach (var item in src)
			{
				if (predicate(item)) return index;
				index++;
			}

			return -1;
		}


		#region byte[]

		/// <summary>
		/// 将指定的字节数组转换为Base64格式
		/// </summary>
		/// <param name="array">要转换的字节数组</param>
		/// <returns>Base64格式的字符串</returns>
		public static string ToBase64(this byte[] array)
		{
			return Convert.ToBase64String(array);
		}

		/// <summary>
		/// 将指定的字节数组转换为Image图像对象
		/// </summary>
		/// <param name="array">要转换的字节数组</param>
		/// <returns><see cref="T:System.Drawing.Image"/></returns>
		public static Image ToImage(this byte[] array)
		{
			try
			{
				using (var ms = new System.IO.MemoryStream())
				{
					ms.Write(array, 0, array.Length);
					ms.Seek(0, System.IO.SeekOrigin.Begin);

					var img = Image.FromStream(ms);
					ms.Close();

					return img;
				}
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		/// <summary>
		/// 将指定的数值转换为字节组
		/// </summary>
		/// <param name="value">要转换的数值</param>
		/// <returns>字节数组</returns>
		public static byte[] ToBytes(this bool value)
		{
			return BitConverter.GetBytes(value);
		}

		/// <summary>
		/// 将指定的数值转换为字节组
		/// </summary>
		/// <param name="value">要转换的数值</param>
		/// <returns>字节数组</returns>
		public static byte[] ToBytes(this short value)
		{
			return BitConverter.GetBytes(value);
		}

		/// <summary>
		/// 将指定的数值转换为字节组
		/// </summary>
		/// <param name="value">要转换的数值</param>
		/// <returns>字节数组</returns>
		public static byte[] ToBytes(this ushort value)
		{
			return BitConverter.GetBytes(value);
		}

		/// <summary>
		/// 将指定的数值转换为字节组
		/// </summary>
		/// <param name="value">要转换的数值</param>
		/// <returns>字节数组</returns>
		public static byte[] ToBytes(this char value)
		{
			return BitConverter.GetBytes(value);
		}

		/// <summary>
		/// 将指定的数值转换为字节组
		/// </summary>
		/// <param name="value">要转换的数值</param>
		/// <returns>字节数组</returns>
		public static byte[] ToBytes(this long value)
		{
			return BitConverter.GetBytes(value);
		}

		/// <summary>
		/// 将指定的数值转换为字节组
		/// </summary>
		/// <param name="value">要转换的数值</param>
		/// <returns>字节数组</returns>
		public static byte[] ToBytes(this ulong value)
		{
			return BitConverter.GetBytes(value);
		}

		/// <summary>
		/// 将指定的数值转换为字节组
		/// </summary>
		/// <param name="value">要转换的数值</param>
		/// <returns>字节数组</returns>
		public static byte[] ToBytes(this int value)
		{
			return BitConverter.GetBytes(value);
		}

		/// <summary>
		/// 将指定的数值转换为字节组
		/// </summary>
		/// <param name="value">要转换的数值</param>
		/// <returns>字节数组</returns>
		public static byte[] ToBytes(this uint value)
		{
			return BitConverter.GetBytes(value);
		}

		/// <summary>
		/// 将指定的数值转换为字节组
		/// </summary>
		/// <param name="value">要转换的数值</param>
		/// <returns>字节数组</returns>
		public static byte[] ToBytes(this float value)
		{
			return BitConverter.GetBytes(value);
		}

		/// <summary>
		/// 将指定的数值转换为字节组
		/// </summary>
		/// <param name="value">要转换的数值</param>
		/// <returns>字节数组</returns>
		public static byte[] ToBytes(this double value)
		{
			return BitConverter.GetBytes(value);
		}

		/// <summary>
		/// 返回指定的字节数组中以指定位置的字节转换来的值
		/// </summary>
		/// <param name="buffer">来源字节数组</param>
		/// <returns>值</returns>
		public static bool ToBoolean(this byte[] buffer)
		{
			return ToBoolean(buffer, 0);
		}

		/// <summary>
		/// 返回指定的字节数组中以指定位置的字节转换来的值
		/// </summary>
		/// <param name="buffer">来源字节数组</param>
		/// <param name="offset">数据偏移</param>
		/// <returns>值</returns>
		public static bool ToBoolean(this byte[] buffer, int offset)
		{
			return BitConverter.ToBoolean(buffer, offset);
		}

		/// <summary>
		/// 返回指定的字节数组中以指定位置的字节转换来的值
		/// </summary>
		/// <param name="buffer">来源字节数组</param>
		/// <returns>值</returns>
		public static char ToChar(this byte[] buffer)
		{
			return ToChar(buffer, 0);
		}

		/// <summary>
		/// 返回指定的字节数组中以指定位置的字节转换来的值
		/// </summary>
		/// <param name="buffer">来源字节数组</param>
		/// <param name="offset">数据偏移</param>
		/// <returns>值</returns>
		public static char ToChar(this byte[] buffer, int offset)
		{
			return BitConverter.ToChar(buffer, offset);
		}

		/// <summary>
		/// 返回指定的字节数组中以指定位置的字节转换来的值
		/// </summary>
		/// <param name="buffer">来源字节数组</param>
		/// <returns>值</returns>
		public static double ToDouble(this byte[] buffer)
		{
			return ToDouble(buffer, 0);
		}

		/// <summary>
		/// 返回指定的字节数组中以指定位置的字节转换来的值
		/// </summary>
		/// <param name="buffer">来源字节数组</param>
		/// <param name="offset">数据偏移</param>
		/// <returns>值</returns>
		public static double ToDouble(this byte[] buffer, int offset)
		{
			return BitConverter.ToDouble(buffer, offset);
		}

		/// <summary>
		/// 返回指定的字节数组中以指定位置的字节转换来的值
		/// </summary>
		/// <param name="buffer">来源字节数组</param>
		/// <returns>值</returns>
		public static short ToShort(this byte[] buffer)
		{
			return ToShort(buffer, 0);
		}

		/// <summary>
		/// 返回指定的字节数组中以指定位置的字节转换来的值
		/// </summary>
		/// <param name="buffer">来源字节数组</param>
		/// <param name="offset">数据偏移</param>
		/// <returns>值</returns>
		public static short ToShort(this byte[] buffer, int offset)
		{
			return BitConverter.ToInt16(buffer, offset);
		}

		/// <summary>
		/// 返回指定的字节数组中以指定位置的字节转换来的值
		/// </summary>
		/// <param name="buffer">来源字节数组</param>
		/// <returns>值</returns>
		public static int ToInt(this byte[] buffer)
		{
			return ToInt32(buffer, 0);
		}

		/// <summary>
		/// 返回指定的字节数组中以指定位置的字节转换来的值
		/// </summary>
		/// <param name="buffer">来源字节数组</param>
		/// <param name="offset">数据偏移</param>
		/// <returns>值</returns>
		public static int ToInt32(this byte[] buffer, int offset)
		{
			return BitConverter.ToInt32(buffer, offset);
		}

		/// <summary>
		/// 返回指定的字节数组中以指定位置的字节转换来的值
		/// </summary>
		/// <param name="buffer">来源字节数组</param>
		/// <returns>值</returns>
		public static long ToInt64(this byte[] buffer)
		{
			return ToInt64(buffer, 0);
		}

		/// <summary>
		/// 返回指定的字节数组中以指定位置的字节转换来的值
		/// </summary>
		/// <param name="buffer">来源字节数组</param>
		/// <param name="offset">数据偏移</param>
		/// <returns>值</returns>
		public static long ToInt64(this byte[] buffer, int offset)
		{
			return BitConverter.ToInt64(buffer, offset);
		}

		/// <summary>
		/// 返回指定的字节数组中以指定位置的字节转换来的值
		/// </summary>
		/// <param name="buffer">来源字节数组</param>
		/// <returns>值</returns>
		public static float ToSingle(this byte[] buffer)
		{
			return ToSingle(buffer, 0);
		}

		/// <summary>
		/// 返回指定的字节数组中以指定位置的字节转换来的值
		/// </summary>
		/// <param name="buffer">来源字节数组</param>
		/// <param name="offset">数据偏移</param>
		/// <returns>值</returns>
		public static float ToSingle(this byte[] buffer, int offset)
		{
			return BitConverter.ToSingle(buffer, offset);
		}

		/// <summary>
		/// 将字节数组转换为字符串形式
		/// </summary>
		/// <param name="buffer">字节数组</param>
		/// <param name="seperator">分隔符</param>
		/// <returns><see cref="T:System.String"/></returns>
		public static string ToString(this byte[] buffer, char seperator = '\0')
		{
			return ToString(buffer, 0, seperator);
		}

		/// <summary>
		/// 将字节数组转换为字符串形式
		/// </summary>
		/// <param name="buffer">字节数组</param>
		/// <returns><see cref="T:System.String"/></returns>
		public static string ToString(this byte[] buffer)
		{
			return ToString(buffer, 0, buffer.Length);
		}

		/// <summary>
		/// 将字节数组转换为字符串形式
		/// </summary>
		/// <param name="buffer">字节数组</param>
		/// <param name="offset">数据偏移</param>
		/// <returns><see cref="T:System.String"/></returns>
		public static string ToString(this byte[] buffer, int offset)
		{
			return ToString(buffer, offset);
		}

		/// <summary>
		/// 将字节数组转换为字符串形式
		/// </summary>
		/// <param name="buffer">字节数组</param>
		/// <param name="offset">数据偏移</param>
		/// <param name="addSeperator">是否添加分隔符</param>
		/// <returns><see cref="T:System.String"/></returns>
		public static string ToString(this byte[] buffer, int offset, char seperator = '\0')
		{
			return ToString(buffer, offset, buffer.Length - offset, seperator);
		}

		/// <summary>
		/// 将字节数组转换为字符串形式
		/// </summary>
		/// <param name="buffer">字节数组</param>
		/// <param name="offset">数据偏移</param>
		/// <param name="length">长度</param>
		/// <returns><see cref="T:System.String"/></returns>
		public static string ToString(this byte[] buffer, int offset, int length)
		{
			return ToString(buffer, offset, length);
		}

		/// <summary>
		/// 将字节数组转换为字符串形式
		/// </summary>
		/// <param name="buffer">字节数组</param>
		/// <param name="offset">数据偏移</param>
		/// <param name="length">长度</param>
		/// <param name="seperator">分隔符</param>
		/// <returns><see cref="T:System.String"/></returns>
		public static string ToString(this byte[] buffer, int offset, int length, char seperator = '\0')
		{
			if (seperator == '-')
				return BitConverter.ToString(buffer, offset, length);

			var sb = new StringBuilder(buffer.Length * 2 + (seperator == '\0' ? 0 : buffer.Length - 1));
			foreach (var b in buffer)
			{
				sb.Append(b.ToString("H2"));
				if (seperator != '\0')
					sb.Append(seperator);
			}

			return sb.ToString();
		}

		/// <summary>
		/// 返回指定的字节数组中以指定位置的字节转换来的值
		/// </summary>
		/// <param name="buffer">来源字节数组</param>
		/// <param name="offset">数据偏移</param>
		/// <returns>值</returns>
		public static ushort ToUInt16(this byte[] buffer, int offset)
		{
			return BitConverter.ToUInt16(buffer, offset);
		}

		/// <summary>
		/// 返回指定的字节数组中以指定位置的字节转换来的值
		/// </summary>
		/// <param name="buffer">来源字节数组</param>
		/// <param name="offset">数据偏移</param>
		/// <returns>值</returns>
		public static uint ToInt32U(this byte[] buffer, int offset)
		{
			return BitConverter.ToUInt32(buffer, offset);
		}

		/// <summary>
		/// 返回指定的字节数组中以指定位置的字节转换来的值
		/// </summary>
		/// <param name="buffer">来源字节数组</param>
		/// <param name="offset">数据偏移</param>
		/// <returns>值</returns>
		public static ulong ToUInt64(this byte[] buffer, int offset)
		{
			return BitConverter.ToUInt64(buffer, offset);
		}



		/// <summary>
		/// 将指定的值复制到指定的缓冲字节组中
		/// </summary>
		/// <param name="value">要写入的值</param>
		/// <param name="buffer">目标缓冲数组</param>
		/// <exception cref="System.ArgumentException">目标数组为空或者目标长度不足以写入值</exception>
		public static void CopyToBuffer(this int value, byte[] buffer)
		{
			CopyToBuffer(value, buffer, 0);
		}

		/// <summary>
		/// 将指定的值复制到指定的缓冲字节组中
		/// </summary>
		/// <param name="value">要写入的值</param>
		/// <param name="buffer">目标缓冲数组</param>
		/// <param name="offset">数据偏移</param>
		/// <exception cref="System.ArgumentException">目标数组为空或者目标长度不足以写入值</exception>
		public static void CopyToBuffer(this int value, byte[] buffer, int offset)
		{
			if (buffer == null || buffer.Length < offset + 4) throw new ArgumentException();
			Array.Copy(BitConverter.GetBytes(value), 0, buffer, offset, 4);
		}

		/// <summary>
		/// 将指定的值复制到指定的缓冲字节组中
		/// </summary>
		/// <param name="value">要写入的值</param>
		/// <param name="buffer">目标缓冲数组</param>
		/// <exception cref="System.ArgumentException">目标数组为空或者目标长度不足以写入值</exception>
		public static void CopyToBuffer(this uint value, byte[] buffer)
		{
			CopyToBuffer(value, buffer, 0);
		}

		/// <summary>
		/// 将指定的值复制到指定的缓冲字节组中
		/// </summary>
		/// <param name="value">要写入的值</param>
		/// <param name="buffer">目标缓冲数组</param>
		/// <param name="offset">数据偏移</param>
		/// <exception cref="System.ArgumentException">目标数组为空或者目标长度不足以写入值</exception>
		public static void CopyToBuffer(this uint value, byte[] buffer, int offset)
		{
			if (buffer == null || buffer.Length < offset + 4) throw new ArgumentException();
			Array.Copy(BitConverter.GetBytes(value), 0, buffer, offset, 4);
		}

		/// <summary>
		/// 将指定的值复制到指定的缓冲字节组中
		/// </summary>
		/// <param name="value">要写入的值</param>
		/// <param name="buffer">目标缓冲数组</param>
		/// <exception cref="System.ArgumentException">目标数组为空或者目标长度不足以写入值</exception>
		public static void CopyToBuffer(this short value, byte[] buffer)
		{
			CopyToBuffer(value, buffer, 0);
		}

		/// <summary>
		/// 将指定的值复制到指定的缓冲字节组中
		/// </summary>
		/// <param name="value">要写入的值</param>
		/// <param name="buffer">目标缓冲数组</param>
		/// <param name="offset">数据偏移</param>
		/// <exception cref="System.ArgumentException">目标数组为空或者目标长度不足以写入值</exception>
		public static void CopyToBuffer(this short value, byte[] buffer, int offset)
		{
			if (buffer == null || buffer.Length < offset + 2) throw new ArgumentException();
			Array.Copy(BitConverter.GetBytes(value), 0, buffer, offset, 2);
		}

		/// <summary>
		/// 将指定的值复制到指定的缓冲字节组中
		/// </summary>
		/// <param name="value">要写入的值</param>
		/// <param name="buffer">目标缓冲数组</param>
		/// <exception cref="System.ArgumentException">目标数组为空或者目标长度不足以写入值</exception>
		public static void CopyToBuffer(this ushort value, byte[] buffer)
		{
			CopyToBuffer(value, buffer, 0);
		}

		/// <summary>
		/// 将指定的值复制到指定的缓冲字节组中
		/// </summary>
		/// <param name="value">要写入的值</param>
		/// <param name="buffer">目标缓冲数组</param>
		/// <param name="offset">数据偏移</param>
		/// <exception cref="System.ArgumentException">目标数组为空或者目标长度不足以写入值</exception>
		public static void CopyToBuffer(this ushort value, byte[] buffer, int offset)
		{
			if (buffer == null || buffer.Length < offset + 2) throw new ArgumentException();
			Array.Copy(BitConverter.GetBytes(value), 0, buffer, offset, 2);
		}

		/// <summary>
		/// 将指定的值复制到指定的缓冲字节组中
		/// </summary>
		/// <param name="value">要写入的值</param>
		/// <param name="buffer">目标缓冲数组</param>
		/// <exception cref="System.ArgumentException">目标数组为空或者目标长度不足以写入值</exception>
		public static void CopyToBuffer(this long value, byte[] buffer)
		{
			CopyToBuffer(value, buffer, 0);
		}

		/// <summary>
		/// 将指定的值复制到指定的缓冲字节组中
		/// </summary>
		/// <param name="value">要写入的值</param>
		/// <param name="buffer">目标缓冲数组</param>
		/// <param name="offset">数据偏移</param>
		/// <exception cref="System.ArgumentException">目标数组为空或者目标长度不足以写入值</exception>
		public static void CopyToBuffer(this ulong value, byte[] buffer, int offset)
		{
			if (buffer == null || buffer.Length < offset + 8) throw new ArgumentException();
			Array.Copy(BitConverter.GetBytes(value), 0, buffer, offset, 8);
		}

		/// <summary>
		/// 将指定的值复制到指定的缓冲字节组中
		/// </summary>
		/// <param name="value">要写入的值</param>
		/// <param name="buffer">目标缓冲数组</param>
		/// <exception cref="System.ArgumentException">目标数组为空或者目标长度不足以写入值</exception>
		public static void CopyToBuffer(this float value, byte[] buffer)
		{
			CopyToBuffer(value, buffer, 0);
		}

		/// <summary>
		/// 将指定的值复制到指定的缓冲字节组中
		/// </summary>
		/// <param name="value">要写入的值</param>
		/// <param name="buffer">目标缓冲数组</param>
		/// <param name="offset">数据偏移</param>
		/// <exception cref="System.ArgumentException">目标数组为空或者目标长度不足以写入值</exception>
		public static void CopyToBuffer(this float value, byte[] buffer, int offset)
		{
			if (buffer == null || buffer.Length < offset + 4) throw new ArgumentException();
			Array.Copy(BitConverter.GetBytes(value), 0, buffer, offset, 4);
		}

		/// <summary>
		/// 将指定的值复制到指定的缓冲字节组中
		/// </summary>
		/// <param name="value">要写入的值</param>
		/// <param name="buffer">目标缓冲数组</param>
		/// <exception cref="System.ArgumentException">目标数组为空或者目标长度不足以写入值</exception>
		public static void CopyToBuffer(this double value, byte[] buffer)
		{
			CopyToBuffer(value, buffer, 0);
		}

		/// <summary>
		/// 将指定的值复制到指定的缓冲字节组中
		/// </summary>
		/// <param name="value">要写入的值</param>
		/// <param name="buffer">目标缓冲数组</param>
		/// <param name="offset">数据偏移</param>
		/// <exception cref="System.ArgumentException">目标数组为空或者目标长度不足以写入值</exception>
		public static void CopyToBuffer(this double value, byte[] buffer, int offset)
		{
			if (buffer == null || buffer.Length < offset + 8) throw new ArgumentException();
			Array.Copy(BitConverter.GetBytes(value), 0, buffer, offset, 8);
		}

		/// <summary>
		/// 将指定的字符串复制到缓冲数组中
		/// </summary>
		/// <param name="value">值</param>
		/// <param name="buffer">缓冲数组</param>
		/// <returns>写入的长度</returns>
		/// <exception cref="System.ArgumentNullException">buffer</exception>
		/// <exception cref="System.InvalidOperationException">缓冲数组长度不足</exception>
		public static int CopyToBuffer(this string value, byte[] buffer)
		{
			return CopyToBuffer(value, buffer, 0);
		}

		/// <summary>
		/// 将指定的字符串复制到缓冲数组中
		/// </summary>
		/// <param name="value">值</param>
		/// <param name="buffer">缓冲数组</param>
		/// <param name="offset">数据偏移</param>
		/// <returns>写入的长度</returns>
		/// <exception cref="System.ArgumentNullException">buffer</exception>
		/// <exception cref="System.InvalidOperationException">缓冲数组长度不足</exception>
		public static int CopyToBuffer(this string value, byte[] buffer, int offset)
		{
			return CopyToBuffer(value, null, buffer, offset);
		}

		/// <summary>
		/// 将指定的字符串复制到缓冲数组中
		/// </summary>
		/// <param name="value">值</param>
		/// <param name="encoding">编码，默认为Unicode</param>
		/// <param name="buffer">缓冲数组</param>
		/// <param name="offset">数据偏移</param>
		/// <returns>写入的长度</returns>
		/// <exception cref="System.ArgumentNullException">buffer</exception>
		/// <exception cref="System.InvalidOperationException">缓冲数组长度不足</exception>
		public static int CopyToBuffer(this string value, System.Text.Encoding encoding, byte[] buffer, int offset)
		{
			if (buffer == null) throw new ArgumentNullException("buffer");
			if (value.IsNullOrEmpty()) return 0;

			var bytes = (encoding ?? Encoding.Unicode).GetBytes(value);
			if (buffer.Length < offset + bytes.Length) throw new InvalidOperationException("缓冲数组长度不足");
			Array.Copy(bytes, 0, buffer, offset, buffer.Length);
			return buffer.Length;
		}

		/// <summary>
		/// 计算指定字节数组的MD5
		/// </summary>
		/// <param name="source">来源数组</param>
		/// <returns></returns>
		public static byte[] MD5(this byte[] source)
		{
			var md5 = System.Security.Cryptography.MD5CryptoServiceProvider.Create();
			return md5.ComputeHash(source);
		}

		/// <summary>
		/// 计算指定字节数组的MD5字符串
		/// </summary>
		/// <param name="source">来源数组</param>
		/// <returns></returns>
		public static string Md5String(this byte[] source)
		{
			return BitConverter.ToString(source.MD5()).Replace("-", "").ToUpper();
		}

		/// <summary>
		/// 计算指定字节数组的SHA1
		/// </summary>
		/// <param name="source">来源数组</param>
		/// <returns></returns>
		public static byte[] Sha1(this byte[] source)
		{
			var sha1 = System.Security.Cryptography.SHA1CryptoServiceProvider.Create();
			return sha1.ComputeHash(source);
		}

		/// <summary>
		/// 计算指定字节数组的SHA1
		/// </summary>
		/// <param name="source">来源数组</param>
		/// <returns></returns>
		public static string Sha1String(this byte[] source)
		{
			return BitConverter.ToString(source.Sha1()).Replace("-", "").ToUpper();
		}

		/// <summary>
		/// 压缩数据组
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static byte[] Compress(this byte[] source)
		{
			using (var ms = new System.IO.MemoryStream())
			using (var gzip = new System.IO.Compression.GZipStream(ms, CompressionMode.Compress))
			{
				gzip.Write(source);
				gzip.Close();
				return ms.ToArray();
			}
		}

		/// <summary>
		/// 解压缩数据组
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static byte[] Decompress(this byte[] source)
		{
			using (var ms = new System.IO.MemoryStream(source))
			using (var gzip = new System.IO.Compression.GZipStream(ms, CompressionMode.Decompress))
			using (var msout = new System.IO.MemoryStream())
			{
				var buffer = new byte[1024];
				var count = 0;
				while ((count = gzip.Read(buffer, 0, buffer.Length)) > 0)
				{
					msout.Write(buffer, 0, count);
				}
				msout.Close();

				return msout.ToArray();
			}
		}

		#endregion
	}
}