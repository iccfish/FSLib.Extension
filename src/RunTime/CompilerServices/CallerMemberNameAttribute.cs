using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if !NET45 && !NET_GT_45
namespace System.RunTime.CompilerServices
{
	public sealed class CallerMemberNameAttribute : Attribute { }
}
#endif