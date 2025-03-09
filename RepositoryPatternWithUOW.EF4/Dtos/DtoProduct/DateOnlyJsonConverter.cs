namespace RepositoryPatternWithEFCore.EF4
{
    public class DateOnlyJsonConverter : JsonConverter<DateTime>
    {
        private const string _format = "yyyy-MM-dd"; // ⬅️ تنسيق التاريخ فقط

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.ParseExact(reader.GetString()!, _format, null);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(_format));
        }
    }
}
