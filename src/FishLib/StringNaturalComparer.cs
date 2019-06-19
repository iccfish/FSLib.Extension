namespace FSLib.Extension
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// 提供对字符串的自然排序
	/// </summary>
	public class StringNaturalComparer : IComparer<string>
	{
		/// <summary>
		/// 获得当前唯一的静态比较器实例
		/// </summary>
		public readonly static StringNaturalComparer Current = new StringNaturalComparer();

		#region Implementation of IComparer<string>

		/// <summary>
		/// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
		/// </summary>
		/// <returns>
		/// Value Condition Less than zerox is less than y.Zerox equals y.Greater than zerox is greater than y.
		/// </returns>
		/// <param name="y">The second object to compare.</param><param name="x">The first object to compare.</param>
		public int Compare(string x, string y)
		{
			var isXEmpty = x.IsNullOrEmpty();
			var isYEmpty = y.IsNullOrEmpty();

			if (isXEmpty && isYEmpty) return 0;
			if (isXEmpty ^ isYEmpty) return isXEmpty ? -1 : 1;

			//开始比较
			var indexX = 0;
			var indexY = 0;
			while (indexX < x.Length && indexY < y.Length)
			{
				var cx = x[indexX];
				var cy = y[indexY];

				if (char.IsDigit(cx) && char.IsDigit(cy))
				{
					var nx = GetNumber(x, ref indexX);
					var ny = GetNumber(y, ref indexY);

					if (nx != ny) return nx - ny;
				}
				else
				{
					var sx = GetEmbedString(x, ref indexX);
					var sy = GetEmbedString(y, ref indexY);
					var diff = StringComparer.OrdinalIgnoreCase.Compare(sx, sy);
					if (diff != 0) return diff;
				}
			}

			return x.Length == y.Length ? 0 : y.Length - x.Length;
		}

		#endregion

		/// <summary>
		/// 获得下一个数字
		/// </summary>
		/// <param name="str"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		static int GetNumber(string str, ref int index)
		{
			var current = index + 1;
			while (current < str.Length && char.IsDigit(str[current]))
			{
				current += 1;
			}

			var value = int.Parse(str.Substring(index, current - index));
			index = current;
			return value;
		}

		/// <summary>
		/// 获得下一个字符串组
		/// </summary>
		/// <param name="str"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		static string GetEmbedString(string str, ref int index)
		{
			var current = index + 1;
			while (current < str.Length && !char.IsDigit(str[current]))
			{
				current += 1;
			}

			var value = str.Substring(index, current - index);
			index = current;
			return value;
		}
	}
}
