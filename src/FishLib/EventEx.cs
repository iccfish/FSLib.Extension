using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FSLib.Extension {
	using System.Reflection;

	/// <summary>
	/// 事件辅助类
	/// </summary>
	public class EventEx
	{
		/// <summary>
		/// 添加一个事件句柄，并在目标对象销毁时自动取消关联
		/// </summary>
		/// <param name="sourceType">引发事件的源对象类型</param>
		/// <param name="targetType">捕捉事件的目标对象类型</param>
		/// <param name="source">引发事件的源对象</param>
		/// <param name="target">捕捉事件的目标对象</param>
		/// <param name="eventName">事件名</param>
		/// <param name="handler">句柄</param>
		/// <returns>取消挂载事件的对象句柄</returns>
		public static EventHandler AddAutoDisposeEventHandler(
			[NotNull] Type sourceType,
			[NotNull] object source,
			[NotNull] Type targetType,
			[NotNull] object target,
			[NotNull] string eventName,
			Delegate handler)
		{
			if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (targetType == null) throw new ArgumentNullException(nameof(targetType));
			if (target == null) throw new ArgumentNullException(nameof(target));
			if (eventName == null) throw new ArgumentNullException(nameof(eventName));

			var targetEvent = sourceType.GetTypeInfo().GetEvent(eventName);
			var dispose = targetType.GetTypeInfo().GetEvent("Disposed");
			if (targetEvent == null)
			{
				throw new InvalidOperationException($"Event '{eventName}' in type '{sourceType.Name}' not found.");
			}
			if (dispose == null)
			{
				throw new NotSupportedException($"Event 'Disposed' not found in type '{targetType.Name}'.");
			}

			targetEvent.AddEventHandler(source, handler);

			var disposeEventHandler = new EventHandler((_1, _2) =>
			{
				targetEvent.RemoveEventHandler(source, handler);
			});
			dispose.AddEventHandler(target, disposeEventHandler);

			return disposeEventHandler;
		}

		/// <summary>
		/// 添加一个事件句柄，并在目标对象销毁时自动取消关联
		/// </summary>
		/// <typeparam name="TSource">引发事件的源对象类型</typeparam>
		/// <typeparam name="TTarget">捕捉事件的目标对象类型</typeparam>
		/// <param name="source">引发事件的源对象</param>
		/// <param name="target">捕捉事件的目标对象</param>
		/// <param name="eventName">事件名</param>
		/// <param name="handler">句柄</param>
		/// <returns>取消挂载事件的对象句柄</returns>
		public static EventHandler AddAutoDisposeEventHandler<TSource, TTarget>(TSource source, TTarget target, string eventName, Delegate handler) where TTarget : class where TSource : class
		{
			return AddAutoDisposeEventHandler(typeof(TSource), source, typeof(TTarget), target, eventName, handler);
		}

	}
}
