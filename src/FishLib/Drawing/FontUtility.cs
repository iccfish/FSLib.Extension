#if NET20 || NET35 || NET40 || NET45 || NET46 || NET47

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSLib.Extension.Drawing
{
	using System.Drawing;
	using System.Drawing.Text;

	class FontUtility
	{
		static FontFamily LoadRawFonts(byte[] buffer)
		{
			var f = new PrivateFontCollection();
			unsafe
			{
				fixed (byte* pfont04b = buffer)
				{
					f.AddMemoryFont((IntPtr)pfont04b, buffer.Length);
				}
			}

			return f.Families[0];
		}

	}
}
#endif