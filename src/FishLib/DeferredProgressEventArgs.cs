namespace FSLib.Extension.FishLib
{
	using System;

	/// <summary>
	/// 进度事件数据
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class DeferredProgressEventArgs : EventArgs
	{
		/// <summary>
		/// 获得进度标示
		/// </summary>
		public object ProgressData { get; private set; }

		/// <summary>
		/// 创建 <see cref="DeferredProgressEventArgs{T}" />  的新实例(DeferredProgressEventArgs)
		/// </summary>
		/// <param name="progressData"></param>
		public DeferredProgressEventArgs(object progressData)
		{
			ProgressData = progressData;
		}
	}
}