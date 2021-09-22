using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using ReleaseRetention.Abstractions.Model;

namespace ReleaseRetention.Tests.Converters
{
    public class VersionJsonConverter : JsonConverter<IVersion>
    {
        public override IVersion? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            var versionString = reader.GetString();

            if (versionString is null)
            {
                return null;
            }
            
            // Split into number and tag
            var splitString = versionString.Split('-', 2);
            var numberString = splitString[0];
            var tagString = splitString.Length > 1
                ? splitString[1]
                : string.Empty;

            return new Model.Version()
            {
                Number = Version.Parse(numberString),
                Tag = tagString,
            };
        }

        public override void Write(Utf8JsonWriter writer, IVersion? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStringValue($"{value.Number}-{value.Tag}");
        }
    }
}