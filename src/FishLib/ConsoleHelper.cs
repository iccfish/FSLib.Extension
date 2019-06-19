namespace FSLib.Extension
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text.RegularExpressions;

	/// <summary>
	/// 控制台辅助类
	/// </summary>
	public class ConsoleHelper
	{
		static ConsoleHelper()
		{
			UseSynchroizeOut = true;
		}

		static readonly object _lockobject = new object();

		/// <summary>
		/// 获得或设置是否使用同步输出。当为 <see langword="true" /> 时，每次输出都将会使用加锁输出，两次输出之间不会重叠。
		/// </summary>
		public static bool UseSynchroizeOut { get; set; }

		/// <summary>
		/// 设置前景色
		/// </summary>
		/// <param name="c">颜色</param>
		public static void SetFor(ConsoleColor c) { global::System.Console.ForegroundColor = c; }

		/// <summary>
		/// 设置背景色
		/// </summary>
		/// <param name="c">颜色</param>
		public static void SetBack(ConsoleColor c) { global::System.Console.BackgroundColor = c; }

		/// <summary>
		/// 获得或设置是否显示光标
		/// </summary>
		public static bool ShowCursor
		{
			get
			{
				return System.Console.CursorVisible;
			}
			set
			{
				System.Console.CursorVisible = value;
			}
		}

		/// <summary>
		/// 从标准输入流读取一个按键
		/// </summary>
		/// <param name="intercept">是否回显；默认不回显</param>
		/// <returns>按键信息</returns>
		public static ConsoleKeyInfo ReadKey(bool intercept = false)
		{
			return Console.ReadKey(intercept);
		}

		/// <summary>
		/// WriteLine的简化
		/// </summary>
		public static void WL()
		{
			WL(String.Empty);
		}

		/// <summary>
		/// WriteLine的简化
		/// </summary>
		public static void WL(string str) { global::System.Console.WriteLine(str); }

		/// <summary>
		/// WriteLine的简化
		/// </summary>
		public static void WL(ConsoleColor forColor, object message)
		{
			lock (_lockobject)
			{
				Backup();
				SetFor(forColor);
				WL(message.ToString());
				Restore();
			}
		}

		/// <summary>
		/// Write的简化
		/// </summary>
		public static void W(string message)
		{
			System.Console.Write(message);
		}


		/// <summary>
		/// 以指定的前景色和背景色输出信息
		/// </summary>
		public static void WL(ConsoleColor forColor, ConsoleColor backColor, object message)
		{
			lock (_lockobject)
			{
				Backup();
				SetFor(forColor);
				SetBack(backColor);
				WL(message.ToString());
				Restore();
			}
		}

		/// <summary>
		/// 在指定的位置输出信息
		/// </summary>
		public static void WL(int x, int y, object message)
		{
			lock (_lockobject)
			{
				SetLoc(x, y);
				WL(message.ToString());
			}
		}

		/// <summary>
		/// 在指定的位置以指定的前景色输出信息
		/// </summary>
		public static void WL(int x, int y, ConsoleColor forColor, object message)
		{
			lock (_lockobject)
			{
				Backup();
				SetLoc(x, y);
				SetFor(forColor);
				WL(message.ToString());
				Restore();
			}
		}

		/// <summary>
		/// 在指定的位置以指定的前景色和背景色输出信息
		/// </summary>
		public static void WL(int x, int y, ConsoleColor forColor, ConsoleColor backColor, object message)
		{
			lock (_lockobject)
			{
				Backup();
				SetLoc(x, y);
				SetFor(forColor);
				SetBack(backColor);
				WL(message.ToString());
				Restore();
			}
		}

		/// <summary>
		/// 以指定的前景色输出文本
		/// </summary>
		public static void W(ConsoleColor forColor, object message)
		{
			lock (_lockobject)
			{
				Backup();
				SetFor(forColor);
				W(message.ToString());
				Restore();
			}
		}


		/// <summary>
		/// 以指定的前景色和背景色输出文本
		/// </summary>
		public static void W(ConsoleColor forColor, ConsoleColor backColor, object message)
		{
			lock (_lockobject)
			{
				Backup();
				SetFor(forColor);
				SetBack(backColor);
				W(message.ToString());
				Restore();
			}
		}
		/// <summary>
		/// 在指定位置输出文本
		/// </summary>
		public static void W(int x, int y, object message)
		{
			lock (_lockobject)
			{
				SetLoc(x, y);
				W(message.ToString());
			}
		}

		/// <summary>
		/// 在指定的位置以指定的前景色输出信息
		/// </summary>
		public static void W(int x, int y, ConsoleColor forColor, object message)
		{
			lock (_lockobject)
			{
				Backup();
				SetLoc(x, y);
				SetFor(forColor);
				W(message.ToString());
				Restore();
			}
		}

		/// <summary>
		/// 在指定的位置以指定的前景色和背景色输出信息
		/// </summary>
		public static void W(int x, int y, ConsoleColor forColor, ConsoleColor backColor, object message)
		{
			lock (_lockobject)
			{
				Backup();
				SetLoc(x, y);
				SetFor(forColor);
				SetBack(backColor);
				W(message.ToString());
				Restore();
			}
		}


		/// <summary>
		/// 设置光标位置
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public static void SetLoc(int x, int y)
		{
			System.Console.SetCursorPosition(x, y);
		}
		/// <summary>
		/// 设置窗口标题
		/// </summary>
		/// <param name="title"></param>
		public static void Title(string title) { System.Console.Title = title; }

		/// <summary>
		/// 清空屏幕
		/// </summary>
		public static void Clear() { System.Console.Clear(); }


		/// <summary>
		/// 显示进度条
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="count"></param>
		/// <param name="color"></param>
		/// <param name="current"></param>
		/// <param name="totalCount"></param>
		public static void ShowProcess(int x, int y, int count, ConsoleColor color, int current, int totalCount)
		{
			lock (_lockobject)
			{
				Backup();

				SetLoc(x, y);
				SetBack(color);

				var length = count * current / totalCount;
				W("".PadLeft(length, ' '));

				Restore();
			}
		}

		/// <summary>
		/// 显示进度条
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="count"></param>
		/// <param name="ch"></param>
		/// <param name="current"></param>
		/// <param name="totalCount"></param>
		public static void ShowProcess(int x, int y, int count, char ch, int current, int totalCount)
		{
			lock (_lockobject)
			{
				SetLoc(x, y);

				int length = (int)(count * current / totalCount);
				W("".PadLeft(length, ch));
			}
		}

		/// <summary>
		/// 截取字符串
		/// </summary>
		/// <param name="message"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string SubStringPath(string message, int length)
		{
			if (message.Length <= length) return message;
			else
			{
				string _dic = System.IO.Path.GetDirectoryName(message);
				string _file = System.IO.Path.GetFileName(message);

				return _dic.Substring(0, length - 1 - _file.Length) + "\\" + _file;
			}
		}

		/// <summary>
		/// 读取一行
		/// </summary>
		/// <returns></returns>
		public static string RL()
		{
			return System.Console.ReadLine();
		}

		/// <summary>
		/// 读取一行
		/// </summary>
		/// <returns></returns>
		public static string RL(string Tip)
		{
			lock (_lockobject)
			{
				W(Tip);
				return System.Console.ReadLine();
			}
		}


		/// <summary>
		/// 读取一行数字
		/// </summary>
		/// <returns></returns>
		public static int ReadInt(string tip, int defValue)
		{
			lock (_lockobject)
			{
				string val = RL(tip);
				int temp;
				if (int.TryParse(val, out temp)) return temp;
				return defValue;
			}
		}

		static Regex _consoleRegex = new Regex(@"<(/?)([BF])(:([a-z]+))?>", RegexOptions.IgnoreCase);

		/// <summary>
		/// 输出格式化的字符串 
		/// </summary>
		/// <param name="str"></param>
		/// <param name="args"></param>
		[StringFormatMethod("str")]
		public static void WFL(string str, params object[] args)
		{
			lock (_lockobject)
			{
				WF(str, args);
				WL();
			}
		}

		/// <summary>
		/// 输出格式化的字符串 
		/// </summary>
		/// <param name="str"></param>
		/// <param name="args"></param>
		/// <remarks>
		/// 可用 &lt;[BF]:ColorName&gt;.....&lt;/[BF]&gt; 来设置颜色
		/// </remarks>
		[StringFormatMethod("str")]
		public static void WF(string str, params object[] args)
		{
			lock (_lockobject)
			{
				if (args != null && args.Length > 0)
				{
					str = str.FormatWith(args);
				}

				//分割
				var matches = _consoleRegex.Matches(str);
				if (matches.Count > 0)
				{
					var cf = Console.ForegroundColor;
					var cb = Console.BackgroundColor;
					var sf = new Stack<ConsoleColor>();
					var sb = new Stack<ConsoleColor>();

					var strPos = 0;

					foreach (Match match in matches)
					{
						if (match.Index > strPos)
						{
							Console.Write(str.Substring(strPos, match.Index - strPos));
						}

						var cstr = match.Groups[4].Value;
						var tag = match.Groups[2].Value;

						if (match.Groups[1].Value == "/")
						{
							if (tag[0] == 'B' || tag[0] == 'b')
							{
								if (sb.Count > 0) Console.BackgroundColor = sb.Pop();
							}
							else
							{
								if (sf.Count > 0) Console.ForegroundColor = sf.Pop();
							}
						}
						else
						{
							var cc = ConsoleColor.White;
							var success = false;
#if NET35
							try
							{
								cc = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), cstr, true);
								success = true;
							}
							catch (Exception)
							{
								success = false;
							}
#else
							success = Enum.TryParse(cstr, true, out cc);
#endif
							if (success)
							{
								if (tag[0] == 'B' || tag[0] == 'b')
								{
									sb.Push(Console.BackgroundColor);
									Console.BackgroundColor = cc;
								}
								else
								{
									sf.Push(Console.ForegroundColor);
									Console.ForegroundColor = cc;
								}
							}
							else
							{
								Console.Write(match.Value);
							}
						}
						strPos = match.Index + match.Length;
					}
					if (strPos < str.Length)
					{
						Console.Write(str.Substring(strPos));
					}
					Console.ForegroundColor = cf;
					Console.BackgroundColor = cb;
				}
				else
				{
					Console.Write(str);
				}
			}
		}

		static ConsoleColor _bkColorFore, _bkColorBack;

		/// <summary>
		/// 备份背景色
		/// </summary>
		static void Backup()
		{
			_bkColorBack = global::System.Console.BackgroundColor;
			_bkColorFore = System.Console.ForegroundColor;
		}

		/// <summary>
		/// 恢复颜色配置
		/// </summary>
		static void Restore()
		{
			System.Console.ForegroundColor = _bkColorFore;
			System.Console.BackgroundColor = _bkColorBack;
		}
	}
}
