using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.FishLib.Drawing
{
	using System.Drawing;
	using System.Reflection;

	public class BrushUtility
	{
		static Dictionary<KnownColor, Brush> _brushes = new Dictionary<KnownColor, Brush>();
		static readonly object _lockKey = new object();

		/// <summary>
		/// 根据颜色名获得对应的笔刷
		/// </summary>
		/// <param name="colorName"></param>
		/// <returns></returns>
		public static Brush FromKnownName(KnownColor colorName)
		{
			Brush brush;

			if (!_brushes.TryGetValue(colorName, out brush))
			{
				lock (_lockKey)
				{
					if (!_brushes.TryGetValue(colorName, out brush))
					{
						var type = typeof (Brushes);
						var p = type.GetProperty(colorName.ToString(), BindingFlags.Static | BindingFlags.Public);
						if (p != null)
						{
							brush = (Brush)p.GetValue(null, null);
						}

						_brushes.Add(colorName, brush);
					}
				}
			}

			return brush;
		}
	}
}
