using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace System.ComponentModel
{
	/// <summary>
	/// <see cref="INotifyPropertyChanged"/> 接口的扩展方法
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class FishNotifyPropertyChangeExtension
	{
		/// <summary>
		/// 判断事件是否是指定的属性变更引发的
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TProp"></typeparam>
		/// <param name="obj"></param>
		/// <param name="expression"></param>
		/// <returns></returns>
		public static bool IsPropertyOf<TSource, TProp>(this TSource obj, PropertyChangedEventArgs e, Expression<Func<TSource, TProp>> expression)
			where TSource : INotifyPropertyChanged
		{
			return obj != null && expression != null && e.PropertyName == expression.GetExpressionAccessedMemberName();
		}

		/// <summary>
		/// 判断事件是否是指定的属性变更引发的
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TProp"></typeparam>
		/// <param name="obj"></param>
		/// <param name="expression"></param>
		/// <returns></returns>
		public static bool IsPropertyOf<TSource, TProp>(this TSource obj, PropertyChangingEventArgs e, Expression<Func<TSource, TProp>> expression)
			where TSource : INotifyPropertyChanging
		{
			return obj != null && expression != null && e.PropertyName == expression.GetExpressionAccessedMemberName();
		}

	}
}
