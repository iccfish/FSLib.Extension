using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
	using System.ComponentModel;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class FishDelegateExtension
	{
		/// <summary>
		/// Return true if all the atomic delegates in the multicast delegate handler are bound into the
		/// publisher, grouped together and in the same order.
		/// </summary>
		/// <param name="publisher"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		public static bool HasBound(this Delegate publisher, Delegate handler)
		{
			if (publisher == null || handler == null)
				return false;
			if (publisher == handler)
				return true;
			var publisherList = publisher.GetInvocationList();
			var handlerList = handler.GetInvocationList();

			if (handlerList.Length > publisherList.Length)
				return false;

			var comparer = EqualityComparer<Delegate>.Default;
			for (int j = 0; j <= publisherList.Length - handlerList.Length; j++)
			{
				for (var i = 0; i < handlerList.Length; i++)
				{
					if (!comparer.Equals(publisherList[i + j], handlerList[i]))
						return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Return true if all the atomic delegates in the multicast delegate handler are bound into the
		/// publisher, grouped together and in the same order.
		/// </summary>
		/// <param name="publisher"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		public static bool HasBound<TEventArgs>(this EventHandler<TEventArgs> publisher, EventHandler<TEventArgs> handler) where TEventArgs : EventArgs
		{
			return HasBound((Delegate)publisher, (Delegate)handler);
		}

		/// <summary>
		/// Return true if all the atomic delegates in the multicast delegate handler are bound into the
		/// publisher, grouped together and in the same order.
		/// </summary>
		/// <param name="publisher"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		public static bool HasBound(this EventHandler publisher, EventHandler handler)
		{
			return HasBound((Delegate)publisher, (Delegate)handler);
		}
	}
}
