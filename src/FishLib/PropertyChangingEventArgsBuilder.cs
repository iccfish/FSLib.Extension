namespace FSLib.Extension
{
	using System;

	public class PropertyChangingEventArgsBuilder
	{
		/// <summary>
		/// 创建新的 <see cref="PropertyChangingEventArgs{T}"/> 实例
		/// </summary>
		/// <typeparam name="T">事件属性的类型</typeparam>
		/// <param name="original">原始值</param>
		/// <param name="current">当前值</param>
		/// <returns></returns>
		public static PropertyChangingEventArgs<T> Create<T>(T original, T current)
		{
			return new PropertyChangingEventArgs<T>(original, current);
		}

		/// <summary>
		/// 创建新的 <see cref="PropertyChangingEventArgs{T}"/> 实例
		/// </summary>
		/// <typeparam name="T">事件属性的类型</typeparam>
		/// <param name="handler">事件处理列表</param>
		/// <param name="original">原始值</param>
		/// <param name="current">当前值</param>
		/// <param name="sender">事件的发起方</param>
		/// <returns></returns>
		public static T CreateAndInvoke<T>(object sender, EventHandler<PropertyChangingEventArgs<T>> handler, T original, T current)
		{
			if (handler == null)
				return current;

			var e= new PropertyChangingEventArgs<T>(original, current);
			handler(sender, e);

			if (e.Cancelled)
			{
				if (e.ThrowOnCancelled)
				{
					throw e.ValidationException ?? new OperationCanceledException("Property change has been blocked.");
				}

				return original;
			}

			return e.Current;
		}
	}
}