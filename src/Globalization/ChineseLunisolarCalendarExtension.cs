namespace System.Globalization
{
	using System;
	using System.Linq;

	public static class ChineseLunisolarCalendarExtension
	{
		/// <summary>
		/// 十天干
		/// </summary>
		private static string[] _tiangan = { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };

		/// <summary>
		/// 十二地支
		/// </summary>
		private static string[] _dizhi = { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };

		/// <summary>
		/// 十二生肖
		/// </summary>
		private static string[] _shengxiao = { "鼠", "牛", "虎", "免", "龙", "蛇", "马", "羊", "猴", "鸡", "狗", "猪" };

		/// <summary>
		/// 返回农历天干地支年 
		/// </summary>
		/// <param name="year">农历年</param>
		/// <returns></returns>
		public static string GetLunisolarYear(int year)
		{
			if (year > 3)
			{
				int tgIndex = (year - 4) % 10;
				int dzIndex = (year - 4) % 12;

				return string.Concat(_tiangan[tgIndex], _dizhi[dzIndex]/*, "[", shengxiao[dzIndex], "]"*/);
			}

			throw new ArgumentOutOfRangeException("无效的年份!");
		}

		/// <summary>
		/// 获得指定公历日期中对应的农历天干地支年
		/// </summary>
		/// <param name="calendar"></param>
		/// <param name="date"></param>
		/// <returns></returns>
		public static string GetLunisolarYear(this ChineseLunisolarCalendar calendar, DateTime date)
		{
			return GetLunisolarYear(calendar.GetYear(date));
		}

		/// <summary>
		/// 农历月
		/// </summary>
		private static string[] months = { "正", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二(腊)" };


		/// <summary>
		/// 返回农历月
		/// </summary>
		/// <param name="month">月份</param>
		/// <returns></returns>
		public static string GetLunisolarMonth(int month)
		{
			if (month < 13 && month > 0)
			{
				return months[month - 1];
			}

			throw new ArgumentOutOfRangeException("无效的月份!");
		}

		/// <summary>
		/// 返回指定公历日期对应的农历月
		/// </summary>
		/// <param name="calendar"></param>
		/// <param name="date"></param>
		/// <returns></returns>
		public static string GetLunisolarMonth(this ChineseLunisolarCalendar calendar, DateTime date)
		{
			var month = calendar.GetMonth(date);
			var leapMonth = calendar.GetLeapMonth(calendar.GetYear(date), calendar.GetEra(date));
			var isleep = leapMonth > 0 && leapMonth == month;

			if (leapMonth > 0)
			{
				if (leapMonth <= month)
					month--;
			}

			return string.Concat(isleep ? "闰" : string.Empty, GetLunisolarMonth(month), "月");
		}

		/// <summary>
		/// 日期前缀
		/// </summary>
		private static string[] days1 = { "初", "十", "廿", "三" };

		/// <summary>
		/// 日
		/// </summary>
		private static string[] days = { "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };


		/// <summary>
		/// 返回农历日
		/// </summary>
		/// <param name="day"></param>
		/// <returns></returns>
		public static string GetLunisolarDay(int day)
		{
			if (day > 0 && day < 32)
			{
				if (day != 20 && day != 30)
				{
					return string.Concat(days1[(day - 1) / 10], days[(day - 1) % 10]);
				}
				else
				{
					return string.Concat(days[(day - 1) / 10], days1[1]);
				}
			}

			throw new ArgumentOutOfRangeException("无效的日!");
		}

		/// <summary>
		/// 返回农历日
		/// </summary>
		/// <param name="day"></param>
		/// <returns></returns>
		public static string GetLunisolarDay(this ChineseLunisolarCalendar calendar, DateTime datetime)
		{
			return GetLunisolarDay(calendar.GetDayOfMonth(datetime));
		}


		/// <summary>
		/// 返回生肖
		/// </summary>
		/// <param name="datetime">公历日期</param>
		/// <returns></returns>
		public static string GetShengXiao(this ChineseLunisolarCalendar calendar, DateTime datetime)
		{
			return _shengxiao[calendar.GetTerrestrialBranch(calendar.GetSexagenaryYear(datetime)) - 1];
		}

		/// <summary>
		/// 获得指定公历的农历表述方式
		/// </summary>
		/// <param name="calendar"></param>
		/// <param name="date"></param>
		/// <returns></returns>
		public static string GetLunisolarDateString(this ChineseLunisolarCalendar calendar, DateTime date)
		{
			return calendar.GetLunisolarYear(date) + "年" + calendar.GetLunisolarMonth(date) + calendar.GetLunisolarDay(date);
		}
	}
}
