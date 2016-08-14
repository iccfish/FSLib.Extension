namespace System
{
	using IO;

	using Runtime.Serialization.Formatters.Binary;

	/// <summary>
	/// XML序列化支持类
	/// </summary>
	public static class FSLib_BinarySerializeExtension
	{

		/// <summary>
		/// 序列化对象到文件
		/// </summary>
		/// <param name="ObjectToSerilize">要序列化的对象</param>
		/// <param name="FileName">保存到的文件路径</param>
		public static void SerializeToFile(this object ObjectToSerialize, string FileName)
		{
			if (ObjectToSerialize == null || String.IsNullOrEmpty(FileName))
				return;

			using (FileStream stream = new FileStream(FileName, FileMode.Create))
			{
				SerializeToStream(ObjectToSerialize, stream);
				stream.Dispose();
			}
		}

		/// <summary>
		/// 序列化对象到字节数组
		/// </summary>
		/// <param name="objectToSerialize">要序列化的对象</param>
		/// <returns>返回创建后的字节数组</returns>
		public static byte[] SerializeToBytes(this object objectToSerialize)
		{
			byte[] result = null;
			if (objectToSerialize == null)
				return result;

			using (var ms = new MemoryStream())
			{
				objectToSerialize.SerializeToStream(ms);
				ms.Dispose();
				result = ms.ToArray();
			}

			return result;
		}

		/// <summary>
		/// 序列化对象到流
		/// </summary>
		/// <param name="objectToSerialize">要序列化的对象</param>
		/// <param name="stream">保存对象信息的流</param>
		public static void SerializeToStream(this object objectToSerialize, Stream stream)
		{
			if (objectToSerialize == null || stream == null)
				return;

			BinaryFormatter xso = new BinaryFormatter();
			xso.Serialize(stream, objectToSerialize);
		}

		/// <summary>
		/// 从流中反序列化对象
		/// </summary>
		/// <param name="stream">流</param>
		/// <returns>反序列化的对象</returns>
		public static object DeserializeFromStream(this Stream stream)
		{
			object result = null;
			if (stream == null)
				return result;

			BinaryFormatter xso = new BinaryFormatter();
			result = xso.Deserialize(stream);

			return result;
		}
	}
}