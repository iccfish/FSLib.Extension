using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Diagnostics
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Diagnostics;
	using System.IO;
	using System.Threading;

		/// <summary>
		/// 进程扩展类
		/// </summary>
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public static class FishProcessExtension
		{
			/// <summary>
			/// 启动外部进程,并根据设置来读取信息
			/// </summary>
			/// <param name="psi">要启动的进程信息</param>
			/// <param name="standardOutput">读取标准输出,将会以一个独立的线程启动</param>
			/// <param name="standardError">读取标准错误流,将会以一个独立的线程启动</param>
			/// <param name="standardInput">写入标准输入流,将会以一个独立的线程启动</param>
			/// <returns></returns>
			/// <remarks>这里的线程会在进程退出后强行被结束,因此不要在此做复杂的耗时操作,可能会出现意外情况</remarks>
			public static Process StartExternal(this ProcessStartInfo psi, Action<StreamReader> standardOutput, Action<StreamReader> standardError, Action<StreamWriter> standardInput)
			{
				psi.RedirectStandardError = standardError != null;
				psi.RedirectStandardInput = standardInput != null;
				psi.RedirectStandardOutput = standardOutput != null;

				if (psi.RedirectStandardOutput || psi.RedirectStandardInput || psi.RedirectStandardError)
				{
					psi.UseShellExecute = false;
				}
				var p = Process.Start(psi);
				p.EnableRaisingEvents = true;

				Thread inputThread, outputThread, errorThread;

				if (psi.RedirectStandardError) (errorThread = new Thread(new ParameterizedThreadStart(s => { standardError(s as StreamReader); })) { IsBackground = true, Name = "thread to read standard error of external process" }).Start(p.StandardError);
				else errorThread = null;

				if (psi.RedirectStandardInput) (inputThread = new Thread(new ParameterizedThreadStart(s => { standardInput(s as StreamWriter); })) { IsBackground = true, Name = "thread to write standard input of external process" }).Start(p.StandardInput);
				else inputThread = null;

				if (psi.RedirectStandardOutput) (outputThread = new Thread(new ParameterizedThreadStart(s => { standardOutput(s as StreamReader); })) { IsBackground = true, Name = "thread to read standard output of external process" }).Start(p.StandardOutput);
				else outputThread = null;

				p.Exited += (s, e) =>
				{
					//等待线程退出
					KillThread(inputThread);
					KillThread(outputThread);
					KillThread(errorThread);
				};

				return p;
			}

			/// <summary>
			/// 强行终止线程
			/// </summary>
			/// <param name="thread"></param>
			static void KillThread(Thread thread)
			{
				if (thread == null || !thread.IsAlive) return;

				thread.Join(1000 * 10);
				if (thread.IsAlive)
				{
					try
					{
						thread.Abort();
					}
					finally { }
				}
			}
		}
}
