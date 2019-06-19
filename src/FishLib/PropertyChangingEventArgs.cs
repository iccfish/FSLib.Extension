using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSLib.Extension
{
	/// <summary>
	/// 属性正在变化事件类
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class PropertyChangingEventArgs<T> : EventArgs
	{
		public string PropertyName { get; set; }

		/// <summary>
		/// 初始化 <see cref="PropertyChangingEventArgs{T}"/> 类的新实例。
		/// </summary>
		public PropertyChangingEventArgs(T original, T current)
		{
			Original = original;
			Current = current;
		}

		/// <summary>
		/// 获得原始值
		/// </summary>
		public T Original { get; private set; }

		/// <summary>
		/// 获得或设置当前值
		/// </summary>
		public T Current { get; set; }

		/// <summary>
		/// 获得或设置是否取消变更
		/// </summary>
		public bool Cancelled { get; set; }

		/// <summary>
		/// 获得或设置是否取消变更时抛出异常
		/// </summary>
		public bool ThrowOnCancelled { get; set; }

		/// <summary>
		/// 获得或设置验证异常
		/// </summary>
		public Exception ValidationException { get; set; }


	}
}
