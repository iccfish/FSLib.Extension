#if NETCLR

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSLib.Extension.FishLib.Drawing
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