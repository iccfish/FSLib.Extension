namespace FSLib.Extension
{
	using System;
	using System.Linq;

	/// <summary>
	/// 表示通用的包含事件数据的对象
	/// </summary>
	/// <typeparam name="T">数据对象类型</typeparam>
	public class GeneralEventArgs<T> : EventArgs
	{
		/// <summary>
		/// 事件数据
		/// </summary>
		public T Data { get; set; }

		/// <summary>
		/// 获得或设置是否已经取消
		/// </summary>
		public bool Canceled { get; set; }

		/// <summary>
		/// 创建 <see>
		///     <cref>GeneralEventArgs</cref>
		/// </see>
		///     的新实例(GeneralEventArgs)
		/// </summary>
		public GeneralEventArgs(T data)
		{
			Data = data;
		}
	}
	/// <summary>
	/// 表示通用的包含事件数据的对象
	/// </summary>
	/// <typeparam name="T1">数据对象1类型</typeparam>
	/// <typeparam name="T2">数据对象2类型</typeparam>
	public class GeneralEventArgs<T1, T2> : EventArgs
	{
		/// <summary>
		/// 事件数据1
		/// </summary>
		public T1 Data1 { get; set; }

		/// <summary>
		/// 事件数据2
		/// </summary>
		public T2 Data2 { get; set; }

		/// <summary>
		/// 获得或设置是否已经取消
		/// </summary>
		public bool Canceled { get; set; }

		/// <summary>
		/// 创建 <see>
		///     <cref>GeneralEventArgs</cref>
		/// </see>
		///     的新实例(GeneralEventArgs)
		/// </summary>
		public GeneralEventArgs(T1 data1, T2 data2)
		{
			Data1 = data1;
			Data2 = data2;
		}
	}

	/// <summary>
	/// 表示通用的包含事件数据的对象
	/// </summary>
	/// <typeparam name="T1">数据对象1类型</typeparam>
	/// <typeparam name="T2">数据对象2类型</typeparam>
	/// <typeparam name="T3">数据对象3类型</typeparam>
	public class GeneralEventArgs<T1, T2, T3> : GeneralEventArgs<T1, T2>
	{
		/// <summary>
		/// 事件数据3
		/// </summary>
		public T3 Data3 { get; set; }

		/// <summary>
		/// 创建 <see>
		///     <cref>GeneralEventArgs</cref>
		/// </see>
		///     的新实例(GeneralEventArgs)
		/// </summary>
		public GeneralEventArgs(T1 data1, T2 data2, T3 data3)
			: base(data1, data2)
		{
			Data3 = data3;
		}
	}
}
