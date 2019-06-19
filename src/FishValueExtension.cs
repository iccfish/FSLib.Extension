using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// ReSharper disable once CheckNamespace
namespace System
{
	using FSLib.Extension.FishLib;

	[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	public static class FishValueExtension
	{

		/// <summary>
		/// 判断一个数值是否在指定范围内
		/// </summary>
		/// <param name="value"></param>
		/// <param name="minValue"></param>
		/// <param name="maxValue"></param>
		/// <returns></returns>
		public static bool IsValueInRange(this int value, int minValue, int maxValue) { return value >= minValue && value <= maxValue; }

		/// <summary>
		/// 对数值进行上下限判定，返回限制范围内的数据
		/// </summary>
		/// <param name="value">当前值</param>
		/// <param name="min">最小值。如果 <paramref name="value"/> 小于此值， 则会返回最小值</param>
		/// <param name="max">最大值。如果 <paramref name="value"/> 大于此值， 则会返回最大值</param>
		/// <returns></returns>
		public static int LimitRange(this int value, int min, int max)
		{
			return value < min ? min : value > max ? max : value;
		}

		/// <summary>
		/// 返回大于等于数字被指定值除的商的数字
		/// </summary>
		/// <param name="value">被除数</param>
		/// <param name="divideBy">除数</param>
		/// <returns><see cref="T:System.Int32"/></returns>
		public static int CeilingDivide(this int value, int divideBy)
		{
			if (divideBy == 0) throw new ArgumentOutOfRangeException("divideBy");

			return (int)Math.Ceiling(value * 1.0 / divideBy);
		}

		private static readonly string[] SizeDefinitions = new[]
		{
			SR.Size_Bytes,
			SR.Size_KB,
			SR.Size_MB,
			SR.Size_GB,
			SR.Size_TB
		};

		/// <summary>
		/// 控制尺寸显示转换上限
		/// </summary>
		readonly static double SizeLevel = 0x400 * 0.9;

		/// <summary>
		/// 转换为尺寸显示方式
		/// </summary>
		/// <param name="size">大小</param>
		/// <returns>尺寸显示方式</returns>
		public static string ToSizeDescription(this double size)
		{
			return ToSizeDescription(size, 2);
		}

		/// <summary>
		/// 转换为尺寸显示方式
		/// </summary>
		/// <param name="size">大小</param>
		/// <param name="digits">小数位数</param>
		/// <returns>尺寸显示方式</returns>
		public static string ToSizeDescription(this double size, int digits)
		{
			var sizeDefine = 0;


			while (sizeDefine < SizeDefinitions.Length && size > SizeLevel)
			{
				size /= 0x400;
				sizeDefine++;
			}


			if (sizeDefine == 0) return size.ToString("#0") + " " + SizeDefinitions[sizeDefine];
			else
			{
				return size.ToString("#0." + string.Empty.PadLeft(digits, '0')) + " " + SizeDefinitions[sizeDefine];
			}
		}

		/// <summary>
		/// 转换为尺寸显示方式
		/// </summary>
		/// <param name="size">大小</param>
		/// <returns>尺寸显示方式</returns>
		public static string ToSizeDescription(this ulong size)
		{
			return ((double)size).ToSizeDescription();
		}

		/// <summary>
		/// 转换为尺寸显示方式
		/// </summary>
		/// <param name="size">大小</param>
		/// <param name="digits">小数位数</param>
		/// <returns>尺寸显示方式</returns>
		public static string ToSizeDescription(this ulong size, int digits)
		{
			return ((double)size).ToSizeDescription(digits);
		}

		/// <summary>
		/// 转换为尺寸显示方式
		/// </summary>
		/// <param name="size">大小</param>
		/// <returns>尺寸显示方式</returns>
		public static string ToSizeDescription(this long size)
		{
			return ((double)size).ToSizeDescription();
		}

		/// <summary>
		/// 转换为尺寸显示方式
		/// </summary>
		/// <param name="size">大小</param>
		/// <param name="digits">小数位数</param>
		/// <returns>尺寸显示方式</returns>
		public static string ToSizeDescription(this long size, int digits)
		{
			return ((double)size).ToSizeDescription(digits);
		}

		/// <summary>
		/// 转换为尺寸显示方式
		/// </summary>
		/// <param name="size">大小</param>
		/// <returns>尺寸显示方式</returns>
		public static string ToSizeDescription(this int size)
		{
			return ((double)size).ToSizeDescription();
		}

		/// <summary>
		/// 转换为尺寸显示方式
		/// </summary>
		/// <param name="size">大小</param>
		/// <param name="digits">小数位数</param>
		/// <returns>尺寸显示方式</returns>
		public static string ToSizeDescription(this int size, int digits)
		{
			return ((double)size).ToSizeDescription(digits);
		}

		/// <summary>
		/// 限制数值的最小值, 当数值小于一定范围时, 则返回最小值
		/// </summary>
		/// <param name="value">当前数值</param>
		/// <param name="minValue">最小值</param>
		/// <returns>不小于 <paramref name="minValue"/> 的值</returns>
		public static int Min(this int value, int minValue)
		{
			return value < minValue ? minValue : value;
		}

		/// <summary>
		/// 限制数值的最大值, 当数值大于一定范围时, 则返回最大值
		/// </summary>
		/// <param name="value">当前数值</param>
		/// <param name="maxValue">最大值</param>
		/// <returns>不大于 <paramref name="maxValue"/> 的值</returns>
		public static int Max(this int value, int maxValue)
		{
			return value > maxValue ? maxValue : value;
		}

		/// <summary>
		/// 转换字符串到可空逻辑值
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool? ToBooleanNullable(this string value)
		{
			if (string.IsNullOrEmpty(value)) return null;

			if (value == "1" || string.Compare("true", value, true) == 0) return true;
			if (value == "0" || string.Compare("false", value, true) == 0) return false;

			return null;
		}

		/// <summary>
		/// 转换字符串到逻辑值
		/// </summary>
		/// <param name="value">要转换的字符串，如果转换失败，则返回 false</param>
		/// <returns>转换后的 <see cref="T:System.Boolean"/></returns>
		public static bool ToBoolean(this string value)
		{
			return ToBoolean(value, false);
		}

		/// <summary>
		/// 转换字符串到逻辑值
		/// </summary>
		/// <param name="value">要转换的字符串</param>
		/// <param name="defaultValue">如果转换失败返回的默认值</param>
		/// <returns>转换后的 <see cref="T:System.Boolean"/></returns>
		public static bool ToBoolean(this string value, bool defaultValue)
		{
			var b = value.ToBooleanNullable();
			return b.HasValue ? b.Value : defaultValue;
		}

		/// <summary>
		/// 从可空类型中获得原始值
		/// </summary>
		/// <typeparam name="T">可空值类型的数值类型</typeparam>
		/// <param name="value">包装后的对象</param>
		/// <returns>原始 <typeparamref name="T"/> 类型的值</returns>
		public static T GetValue<T>(this System.Nullable<T> value) where T : struct
		{
			return value.HasValue ? value.Value : default(T);
		}

		/// <summary>
		/// 从可空类型中获得原始值
		/// </summary>
		/// <typeparam name="T">可空值类型的数值类型</typeparam>
		/// <param name="value">包装后的对象</param>
		/// <param name="defaultValue">默认值</param>
		/// <returns>原始 <typeparamref name="T"/> 类型的值</returns>
		public static T GetValue<T>(this System.Nullable<T> value, T defaultValue) where T : struct
		{
			return value.HasValue ? value.Value : defaultValue;
		}

		/// <summary>
		/// 获得第一个数字占第二个数字的百分比
		/// </summary>
		/// <param name="first">第一个数字</param>
		/// <param name="second">第二个数字</param>
		/// <returns>百分比-浮点数</returns>
		public static double GetPercentageBy(this double first, double second)
		{
			if (second == 0.0) return 0;

			return first / second;
		}


		/// <summary>
		/// 获得第一个数字占第二个数字的百分比
		/// </summary>
		/// <param name="first">第一个数字</param>
		/// <param name="second">第二个数字</param>
		/// <returns>百分比-浮点数</returns>
		public static decimal GetPercentageBy(this decimal first, decimal second)
		{
			if (second == 0.0m) return 0;

			return first / second;
		}

		/// <summary>
		/// 显示为字符串
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToDisplayString(this int? value)
		{
			return value == null ? "" : value.ToString();
		}

		/// <summary>
		/// 显示为字符串
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToDisplayString(this int? value, string defaultDisplayString)
		{
			return value == null ? defaultDisplayString : value.ToString();
		}

		/// <summary>
		/// 显示为字符串
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToDisplayString(this float? value)
		{
			return value == null ? "" : value.ToString();
		}

		/// <summary>
		/// 显示为字符串
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToDisplayString(this float? value, string defaultDisplayString)
		{
			return value == null ? defaultDisplayString : value.ToString();
		}

		/// <summary>
		/// 显示为字符串
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToDisplayString(this double? value)
		{
			return value == null ? "" : value.ToString();
		}

		/// <summary>
		/// 显示为字符串
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToDisplayString(this double? value, string defaultDisplayString)
		{
			return value == null ? defaultDisplayString : value.ToString();
		}

		#region INT的压缩
		static readonly string _numberCode = GetInitialCodeString();

		/// <summary>
		/// 获得初始化字符串
		/// </summary>
		/// <returns></returns>
		internal static string GetInitialCodeString()
		{
			var array = new int[10 + 26 + 26];
			array.ForEachWithIndex((i, b) => array[i] = i < 10 ? 0x30 + i : (i < 10 + 26 ? (int)'A' + i - 10 : (int)'a' + i - 10 - 26));
			return new string(array.Select(s => (char)s).ToArray());
		}

		/// <summary>
		/// 将int压缩为字符串
		/// </summary>
		/// <param name="value">要压缩的 <see cref="T:System.Int32"/></param>
		/// <returns>压缩后的字符串</returns>
		public static string CompressToString(this uint value)
		{
			var map = new List<char>();
			var step = (uint)_numberCode.Length;

			while (value > 0)
			{
				var digit = value % step;
				value = (value - digit) / step;
				map.Add(_numberCode[(int)digit]);
			}

			return new string(map.Reverse<char>().ToArray());
		}

		/// <summary>
		/// 将字符串解压为Int值
		/// </summary>
		/// <param name="value">要解压的 <see cref="T:System.String"/></param>
		/// <returns>解压后的 <see cref="T:System.Int32"/></returns>
		public static int DecompressToInt(this string value)
		{
			var array = value.Select(s => _numberCode.IndexOf(s)).Reverse().ToArray();
			int result = 0;
			var step = 1;
			for (int i = 0; i < array.Length; i++)
			{
				result += array[i] * step;
				step *= _numberCode.Length;
			}

			return result;
		}

		#endregion

	}
}
