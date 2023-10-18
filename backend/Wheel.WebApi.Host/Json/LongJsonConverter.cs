using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wheel.Json
{
    internal class LongJsonConverter : JsonConverter<long>
    {
        public LongJsonConverter()
        {
        }

        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
                return reader.GetInt64();

            if(reader.TokenType == JsonTokenType.String &&  long.TryParse(reader.GetString(), out var value))
                return value;

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
    internal class LongArrayConverter : JsonConverter<long[]>
    {
        public LongArrayConverter() { }

        public override long[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string json = reader.GetString();

            var list = new List<long>();

            foreach (string str in json.Split(','))
            {
                if (!long.TryParse(str, out long l))
                {
                    throw new JsonException("Too big for a long");
                }

                list.Add(l);
            }

            return list.ToArray();
        }

        public override void Write(Utf8JsonWriter writer, long[] value, JsonSerializerOptions options)
        {
            var builder = new StringBuilder();

            for (int i = 0; i < value.Length; i++)
            {
                builder.Append(value[i].ToString());

                if (i != value.Length - 1)
                {
                    builder.Append(",");
                }
            }

            writer.WriteStringValue(builder.ToString());
        }
    }
}
