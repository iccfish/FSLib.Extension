namespace FSLib.Extension.Json
{
	using System.IO;

	using Newtonsoft.Json;

	public class JsonUtils
	{
		/// <summary>
		/// 将对象序列化到文件
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <param name="path"></param>
		public static void ToJsonFile<T>(T obj, string path)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(path));
			File.WriteAllText(path, JsonConvert.SerializeObject(obj));
		}

		/// <summary>
		/// 将JSON文件反序列化为对象
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T FromJsonFile<T>(string path) => JsonConvert.DeserializeObject<T>(File.ReadAllText(path));

		/// <summary>
		/// 将JSON文件反序列化为对象
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T FromJsonFile<T>(string path, T anonymousType) => JsonConvert.DeserializeAnonymousType<T>(File.ReadAllText(path), anonymousType);

		/// <summary>
		/// 将JSON文件反序列化为对象
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static void PopulateFromJsonFile<T>(string path, T defaultObj) => JsonConvert.PopulateObject(File.ReadAllText(path), defaultObj);
	}
}
