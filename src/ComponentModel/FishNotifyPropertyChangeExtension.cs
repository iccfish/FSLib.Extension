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
		static Dictionary<WeakReference, List<PropertyChangedEventHandler>> _propertyChangedEvents = new Dictionary<WeakReference, List<PropertyChangedEventHandler>>();
		static Dictionary<WeakReference, List<PropertyChangingEventHandler>> _propertyChangingEvents = new Dictionary<WeakReference, List<PropertyChangingEventHandler>>();


		public static void BindPropertyChanged<T, TP>(this T obi, Expression<Func<T, TP>> expression, PropertyChangedEventHandler handler, object userToken) where T : INotifyPropertyChanged
		{
			var weakReference = new WeakReference(handler);

		}
	}
}
