using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FSLib
{
	/// <summary>
	/// 针对System.Uri的扩展方法
	/// </summary>
	[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	public static class UriExtension
	{
		#region System.Uri

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

	}
}
