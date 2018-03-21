using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
#if NET40
	static class CompatibleMethods
	{
		public static Type GetTypeInfo(this Type type) => type;
	}
#endif
}
