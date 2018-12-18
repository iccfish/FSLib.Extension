using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSLib.Extension.FishLib
{
	/// <summary>
	/// 对时间的一些辅助工具方法
	/// </summary>
	public class DateTimeEx
	{
		/// <summary>
		/// 从UNIX时间戳中获得市级的时间
		/// </summary>
		/// <param name="ticks"></param>
		/// <returns></returns>
		public static DateTime FromUnixTicks(long ticks) => FishDateTimeExtension.JsTicksStartBase.AddSeconds(ticks);

		/// <summary>
		/// 从JS的时间戳中获得时间
		/// </summary>
		/// <param name="ticks"></param>
		/// <returns></returns>
		public static DateTime FromJsTicks(long ticks) => FishDateTimeExtension.JsTicksStartBase.AddMilliseconds(ticks);
	}
}
