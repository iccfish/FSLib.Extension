namespace FSLib.Extension.Json
{
	using System;

	using Newtonsoft.Json;

	/// <summary>
	/// 一个 JsonTypeConverter,用于将字符串形式的1/true/YES/Y等表现形式转换为boolean
	/// </summary>
	public class JsonString2BoolConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(bool);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			switch (reader.TokenType)
			{
				case JsonToken.Boolean: return reader.Value;
				case JsonToken.String:
					var str = reader.Value as string;
					return string.Compare(str, "Y", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(str, "YES", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(str, "true", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(str, "1", StringComparison.OrdinalIgnoreCase) == 0;
				default: break;
			}

			return null;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null)
				writer.WriteValue((bool?)null);
			else
				writer.WriteToken(JsonToken.Boolean, value);
		}
	}
}
