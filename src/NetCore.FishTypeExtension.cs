#if NETSTANDARD1_6_1 || NETSTANDARD2_0 || NETSTANDARD3_0
using System.IO;

namespace System.Reflection
{
	public static class FishTypeExtension
	{
		/// <summary>
		///兼容用途，仅供迁移老代码使用，不要在新代码中使用！
		/// </summary>
		[Obsolete("Just for backward compatiblity. DO NOT USE THIS FUNCTION IN NEW CODES!")]
		public static PropertyInfo GetProperty(this Type type, string name) => type.GetTypeInfo().GetProperty(name);

		/// <summary>
		///兼容用途，仅供迁移老代码使用，不要在新代码中使用！
		/// </summary>
		[Obsolete("Just for backward compatiblity. DO NOT USE THIS FUNCTION IN NEW CODES!")]
		public static PropertyInfo GetProperty(this Type type, string name, BindingFlags flags) => type.GetTypeInfo().GetProperty(name, flags);

		/// <summary>
		///兼容用途，仅供迁移老代码使用，不要在新代码中使用！
		/// </summary>
		[Obsolete("Just for backward compatiblity. DO NOT USE THIS FUNCTION IN NEW CODES!")]
		public static PropertyInfo GetProperty(this Type type, string name, Type returnType) => type.GetTypeInfo().GetProperty(name, returnType);

		/// <summary>
		///兼容用途，仅供迁移老代码使用，不要在新代码中使用！
		/// </summary>
		[Obsolete("Just for backward compatiblity. DO NOT USE THIS FUNCTION IN NEW CODES!")]
		public static PropertyInfo GetProperty(this Type type, string name, Type[] returnTypes) => type.GetTypeInfo().GetProperty(name, returnTypes);

		/// <summary>
		///兼容用途，仅供迁移老代码使用，不要在新代码中使用！
		/// </summary>
		[Obsolete("Just for backward compatiblity. DO NOT USE THIS FUNCTION IN NEW CODES!")]
		public static PropertyInfo GetProperty(this Type type, string name, Type returnType, Type[] types) => type.GetTypeInfo().GetProperty(name, type, types);

		/// <summary>
		///兼容用途，仅供迁移老代码使用，不要在新代码中使用！
		/// </summary>
		[Obsolete("Just for backward compatiblity. DO NOT USE THIS FUNCTION IN NEW CODES!")]
		public static PropertyInfo GetProperty(this Type type, string name, Type returnType, Type[] types, ParameterModifier[] modifiers) => type.GetTypeInfo().GetProperty(name, type, types, modifiers);

		/// <summary>
		///兼容用途，仅供迁移老代码使用，不要在新代码中使用！
		/// </summary>
		[Obsolete("Just for backward compatiblity. DO NOT USE THIS FUNCTION IN NEW CODES!")]
		public static FieldInfo[] GetFields(this Type type, BindingFlags flags = BindingFlags.Default) => type.GetTypeInfo().GetFields(flags);
	}

	public static class FishStreamExtension
	{
		/// <summary>
		///兼容用途，仅供迁移老代码使用，不要在新代码中使用！
		/// </summary>
		/// <param name="stream"></param>
		[Obsolete("Just for backward compatiblity. DO NOT USE THIS FUNCTION IN NEW CODES!")]
		public static void Close(this Stream stream) => stream.Dispose();
	}

}
#endif