using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSLib.Extension.Attributes
{
	/// <summary>
	///
	/// </summary>
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
	public class DisplayNameWAttribute : System.ComponentModel.DisplayNameAttribute
	{
		public DisplayNameWAttribute()
		{

		}

		public DisplayNameWAttribute(string displayName) : base(displayName)
		{

		}
	}
}
