namespace System.FishLib.Diagnostics
{
	using System;
	using System.Linq;
	using System.Runtime.InteropServices;

	/// <summary>
	/// 简单的代码计时器对象
	/// </summary>
	public class CodeTimer
	{
		/// <summary>
		/// 运行测试
		/// </summary>
		/// <param name="iteration">重复次数</param>
		/// <param name="name">名称</param>
		/// <param name="predicate">测试函数</param>
		/// <returns>包含测试结果的<see cref="CodeTimerResult"/></returns>
		public static CodeTimerResult Run(int iteration, string name, Action predicate)
		{
			var _ctx_priority = System.Threading.Thread.CurrentThread.Priority;
			System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Highest;

			var result = new CodeTimerResult() { Title = name };
			predicate();

			result.GetSnapshotBefore();
			for (var i = 0; i < iteration; i++) predicate();
			result.GetSnapshotAfter();

			//restore environment
			System.Threading.Thread.CurrentThread.Priority = _ctx_priority;

			return result;
		}

		/// <summary>
		/// 运行测试，并在当前控制台显示结果
		/// </summary>
		/// <param name="iteration">重复次数</param>
		/// <param name="name">名称</param>
		/// <param name="predicate">测试函数</param>
		/// <returns>包含测试结果的<see cref="CodeTimerResult"/></returns>
		public static CodeTimerResult RunWithConsoleLog(int iteration, string name, Action predicate)
		{
			var result = Run(iteration, name, predicate);

			//保存当前的状态
			var _ctx_forcolor = System.Console.ForegroundColor;
			var _ctx_backcolor = System.Console.BackgroundColor;

			System.Console.BackgroundColor = ConsoleColor.Black;
			System.Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(result.Title);

			Action<string> _outtitle = (s) =>
			{
				Console.ForegroundColor = ConsoleColor.Magenta;
				Console.Write(("\t" + s + " :").PadRight(30, ' '));
			};
			Action<string> _outmessage = (s) =>
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine(s);
			};

			for (int i = 0; i < result.GCCount.Length; i++)
			{
				_outtitle(("GC Gen " + i.ToString()));
				_outmessage(result.GCCount[i].ToString("N0"));
			}
			_outtitle("Elapsed Time");
			_outmessage(result.ElapsedTime.TotalMilliseconds.ToString("N0") + "ms");
			_outtitle("CPU Time");
			_outmessage(result.CPUTimer.ToString("N0") + "ns");
			if (CodeTimerResult.IsSupportCycle)
			{
				_outtitle("Thread Cycles");
				_outmessage(result.ThreadCycleCount.ToString("N0"));
			}

			//restore environment
			Console.ForegroundColor = _ctx_forcolor;
			Console.BackgroundColor = _ctx_backcolor;

			return result;
		}
	}

	/// <summary>
	/// <see cref="CodeTimer"/> 运行测试的结果
	/// </summary>
	public class CodeTimerResult
	{
		#region 静态函数

		static CodeTimerResult()
		{
			IsSupportCycle = System.Environment.OSVersion.Version.Major > 5;
		}
		/// <summary>
		/// 当前环境是否支持线程计数
		/// </summary>
		public static bool IsSupportCycle { get; private set; }


		#endregion


		public CodeTimerResult()
		{
		}

		/// <summary>
		/// 测试名称
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// GC 计数
		/// </summary>
		public int[] GCCount { get; set; }

		/// <summary>
		/// 线程CPU计数
		/// </summary>
		public ulong ThreadCycleCount { get; set; }

		/// <summary>
		/// 线程计数(以100ns为单位)
		/// </summary>
		public long CPUTimer { get; set; }

		/// <summary>
		/// 经过的时间
		/// </summary>
		public TimeSpan ElapsedTime { get; private set; }


		ulong _cycle1, _cycle2;
		long _timer1, _timer2;
		int[] _gcBefore, _gcAfter;
		System.Diagnostics.Stopwatch _stopWatch;

		/// <summary>
		/// 获得第一个快照
		/// </summary>
		internal void GetSnapshotBefore()
		{
			GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
			_gcBefore = new int[GC.MaxGeneration];
			for (var i = 0; i < _gcBefore.Length; i++) _gcBefore[i] = GC.CollectionCount(i);

			var thread = CodeTimerNativeApi.GetCurrentThread();
			_timer1 = CodeTimerNativeApi.GetCurrentThreadTimes(thread);
			if (IsSupportCycle) CodeTimerNativeApi.QueryThreadCycleTime(thread, ref _cycle1);
			_stopWatch = new System.Diagnostics.Stopwatch();
			_stopWatch.Start();
		}

		/// <summary>
		/// 获得第一个快照
		/// </summary>
		internal void GetSnapshotAfter()
		{
			var thread = CodeTimerNativeApi.GetCurrentThread();
			_timer2 = CodeTimerNativeApi.GetCurrentThreadTimes(thread);
			if (IsSupportCycle) CodeTimerNativeApi.QueryThreadCycleTime(thread, ref _cycle2);

			_stopWatch.Stop();
			this.ElapsedTime = _stopWatch.Elapsed;

			_gcAfter = new int[GC.MaxGeneration];
			for (var i = 0; i < _gcBefore.Length; i++) _gcBefore[i] = GC.CollectionCount(i);

			GCCount = _gcAfter.Select((a, i) => a - _gcBefore[i]).ToArray();
			if (IsSupportCycle) ThreadCycleCount = _cycle2 - _cycle1;
			CPUTimer = _timer2 - _timer1;
			GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
		}
	}

	/// <summary>
	/// <see cref="CodeTimer"/> 使用的本地API
	/// </summary>
	internal static class CodeTimerNativeApi
	{
		#region Native API

		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool GetThreadTimes(IntPtr hThread, out long lpCreationTime, out long lpExitTime, out long lpKernelTime, out long lpUserTime);

		[DllImport("kernel32.dll")]
		internal static extern IntPtr GetCurrentThread();

		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool QueryThreadCycleTime(IntPtr threadHandle, ref ulong cycleTime);

		#endregion

		/// <summary>
		/// 获得线程本地计数
		/// </summary>
		/// <returns></returns>
		internal static long GetCurrentThreadTimes(IntPtr intptr)
		{
			long l;
			long kernelTime, userTimer;
			GetThreadTimes(intptr, out l, out l, out kernelTime, out userTimer);
			return kernelTime + userTimer;
		}
	}
}
