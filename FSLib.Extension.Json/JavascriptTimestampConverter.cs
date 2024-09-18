namespace FSLib.Extension.Json
{
	using System;

	using Newtonsoft.Json;

	public class JavascriptTimestampConverter : JsonConverter
	{
		/// <inheritdoc />
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value is DateTime dt)
			{
				writer.WriteValue(dt.ToJsTicks());
			}
			else
				writer.WriteNull();
		}

		/// <inheritdoc />
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			switch (reader.TokenType)
			{
				case JsonToken.Integer:
				case JsonToken.Float:
					if (reader.Value != null)
					{
						var val = (long)reader.Value;
						return DateTimeEx.FromJsTicks(val);
					}

					break;
			}

			return objectType == typeof(DateTime) ? (object)FishDateTimeExtension.JsTicksStartBase : null;
		}

		/// <inheritdoc />
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
		}
	}
}
