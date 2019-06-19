using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if NET20 || NET35 || NET40
namespace System.RuntimeCompatible.CompilerServices
{
	public sealed class CallerMemberNameAttribute : Attribute { }
}
#endif