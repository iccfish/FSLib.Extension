namespace FSLib.Extension
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.InteropServices;

	/// <summary>
	/// 提供对字符串的自然排序(基于Win32的PINVOKE版本)
	/// </summary>
	public class StringNaturalComparerWin32 : IComparer<string>
	{
		[DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
		private static extern int StrCmpLogicalW(string psz1, string psz2);

		/// <summary>
		/// 获得当前唯一的静态比较器实例
		/// </summary>
		public readonly static StringNaturalComparerWin32 Current = new StringNaturalComparerWin32();

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
			return StrCmpLogicalW(x, y);
		}

		#endregion
	}
}
