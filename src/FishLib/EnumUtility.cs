namespace FSLib.Extension
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

#if NETSTANDARD1_6_1 || NETSTANDARD2_0 || NETSTANDARD3_0
	using TypeInfo = System.Reflection.TypeInfo;
#else
	using TypeInfo = System.Type;
#endif
	/// <summary>
	/// 工具类
	/// </summary>
	public static class EnumUtility
	{
		static Dictionary<Type, HashSet<string>> _enumNameKeysCache = new Dictionary<Type, HashSet<string>>();
		static object _lockObject = new object();

		/// <summary>
		/// 获得指定枚举的所有值名称
		/// </summary>
		/// <returns>所有值的集合</returns>
		/// <exception cref="T:System.ArgumentNullException">type为null</exception>
		public static HashSet<string> GetEnumNameKeys<T>()
		{
			return GetEnumNameKeys(typeof(T));
		}

		/// <summary>
		/// 获得指定枚举的所有值名称
		/// </summary>
		/// <param name="type">指定的类型，可以为枚举或可空枚举</param>
		/// <returns>所有值名称的集合</returns>
		/// <exception cref="T:System.ArgumentNullException">type为null</exception>
		public static HashSet<string> GetEnumNameKeys(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");

			var enumType = FishObjectExtension.GetTypeInfo(type);
			if (enumType.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				type = enumType.GetGenericArguments()[0];
				enumType = FishObjectExtension.GetTypeInfo(type);
			}
			if (!enumType.IsEnum) throw new InvalidProgramException("Not an enum type.");

			HashSet<string> result;
			if (!_enumNameKeysCache.TryGetValue(type, out result))
			{
				lock (_lockObject)
				{
					if (!_enumNameKeysCache.ContainsKey(type))
					{
						_enumNameKeysCache.Add(type, Enum.GetNames(type).MapToHashSet(StringComparer.OrdinalIgnoreCase));
					}
				}
				result = _enumNameKeysCache[type];
			}

			return result;

		}




		/// <summary>
		/// 获得指定枚举值的显示名
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetEnumDisplayName<T>(T value)
		{
			var enumDesc = value.GetType().GetEnumDescription();

			return enumDesc.FirstOrDefault(s => (!s.IsFlag && (int)s.Value == (int)(object)value) || (s.IsFlag && ((int)s.Value & (int)(object)value) > 0)).SelectValue(s => s.DisplayName) ?? "";
		}

		/// <summary>
		/// 获得指定枚举值的显示名
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string[] GetEnumDisplayNames<T>(T value)
		{
			var enumDesc = value.GetType().GetEnumDescription();

			return enumDesc.Where(s => (!s.IsFlag && (int)s.Value == (int)(object)value) || (s.IsFlag && ((int)s.Value & (int)(object)value) > 0)).Select(s => s.DisplayName).ToArray();
		}


		/// <summary>
		/// 将十六进制数组字符串转换为十六进制数组
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static byte[] ConvertByteString(string text)
		{
			var buffer = new byte[text.Length / 2];

			for (var i = 0; i < buffer.Length; i++)
			{
				buffer[i] = (byte)((FU.ConvertHexCharToByte(text[i * 2]) << 4) + FU.ConvertHexCharToByte(text[i * 2 + 1]));
			}

			return buffer;
		}

		/// <summary>
		/// 将一个已格式化的十六进制数组字符串转换为十六进制数组
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static byte[] ConvertFormattedByteString(string text)
		{
			text = System.Text.RegularExpressions.Regex.Replace(text, @"[^\da-fA-F]", "");

			return ConvertByteString(text);
		}
	}
}
