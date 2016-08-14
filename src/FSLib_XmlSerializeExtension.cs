namespace System
{
	using IO;

	using Xml.Serialization;

	/// <summary>
	/// XML序列化支持类
	/// </summary>
	public static class FSLib_XmlSerializeExtension
	{
		/// <summary>
		/// 序列化对象到文件
		/// </summary>
		/// <param name="objectToSerialize">要序列化的对象</param>
		/// <param name="fileName">保存到的目标文件</param>
		public static void XmlSerilizeToFile(this object objectToSerialize, string fileName)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(fileName));

			using (var stream = new FileStream(fileName, FileMode.Create))
			{
				objectToSerialize.XmlSerializeToStream(stream);
				stream.Dispose();
			}
		}

		/// <summary>
		/// 序列化对象为文本
		/// </summary>
		/// <param name="objectToSerialize">要序列化的对象</param>
		/// <returns>保存信息的 <see cref="T:System.String"/></returns>
		public static string XmlSerializeToString(this object objectToSerialize)
		{
			if (objectToSerialize == null)
				return null;

			using (var ms = objectToSerialize.XmlSerializeToStream())
			{
				ms.Dispose();
				return Text.Encoding.UTF8.GetString(ms.ToArray());
			}
		}

		/// <summary>
		/// 序列化指定对象为一个内存流
		/// </summary>
		/// <param name="objectToSerialize">要序列化的对象</param>
		/// <returns>保存序列化信息的 <see cref="T:System.IO.MemoryStream"/></returns>
		public static MemoryStream XmlSerializeToStream(this object objectToSerialize)
		{
			MemoryStream result;
			if (objectToSerialize == null)
				return null;

			result = new MemoryStream();
			objectToSerialize.XmlSerializeToStream(result);

			return result;
		}

		/// <summary>
		/// 序列化指定对象到指定流中
		/// </summary>
		/// <param name="objectToSerialize">要序列化的对象</param>
		/// <param name="stream">目标流</param>
		public static void XmlSerializeToStream(this object objectToSerialize, Stream stream)
		{
			if (objectToSerialize == null || stream == null)
				return;

			var xso = new XmlSerializer(objectToSerialize.GetType());
			xso.Serialize(stream, objectToSerialize);
		}

		/// <summary>
		/// 从指定的字符串或文件中反序列化对象
		/// </summary>
		/// <param name="type">目标类型</param>
		/// <param name="content">文件路径或XML文本</param>
		/// <returns>反序列化的结果</returns>
		public static object XmlDeserialize(this Type type, string content)
		{
			content = content.Trim();

			if (string.IsNullOrEmpty(content))
				return null;
			if (content[0] == '<')
			{
				using (var ms = new MemoryStream())
				{
					byte[] buffer = Text.Encoding.Unicode.GetBytes(content);
					ms.Write(buffer, 0, buffer.Length);
					ms.Seek(0, SeekOrigin.Begin);

					return ms.XmlDeserialize(type);
				}
			}
			else
			{
				return XmlDeserializeFromFile(content, type);
			}
		}

		/// <summary>
		/// 从文件中反序列化指定类型的对象
		/// </summary>
		/// <param name="objType">反序列化的对象类型</param>
		/// <param name="fileName">文件名</param>
		/// <returns>对象</returns>
		public static object XmlDeserializeFromFile(string fileName, System.Type objType)
		{
			using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				object res = stream.XmlDeserialize(objType);
				stream.Dispose();
				return res;
			}
		}


		/// <summary>
		/// 从流中反序列化出指定对象类型的对象
		/// </summary>
		/// <param name="objType">对象类型</param>
		/// <param name="stream">流对象</param>
		/// <returns>反序列结果</returns>
		public static object XmlDeserialize(this Stream stream, System.Type objType)
		{
			var xso = new XmlSerializer(objType);
			object res = xso.Deserialize(stream);

			return res;
		}

		/// <summary>
		/// 从流中反序列化对象
		/// </summary>
		/// <typeparam name="T">对象类型</typeparam>
		/// <param name="stream">流对象</param>
		/// <returns>反序列化结果</returns>
		public static T XmlDeserialize<T>(this Stream stream) where T : class
		{
			T res = stream.XmlDeserialize(typeof(T)) as T;

			return res;
		}

		/// <summary>
		/// 序列化文本或文件为对象
		/// </summary>
		/// <returns>保存信息的 <see cref="T:System.String"/></returns>
		public static T XmlDeserialize<T>(this string content) where T : class
		{
			return (T)typeof(T).XmlDeserialize(content);
		}
	}
}
