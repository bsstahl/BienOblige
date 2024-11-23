using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;

public class Iso8601TimespanConverter : JsonConverter<TimeSpan>
{
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return Iso8601TimespanConverter.Parse(value ?? string.Empty);
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        string iso8601Duration = Iso8601TimespanConverter.Format(value);
        writer.WriteStringValue(iso8601Duration);
    }

    private static TimeSpan Parse(string duration)
    {
        if (string.IsNullOrWhiteSpace(duration))
            throw new ArgumentException("Duration value cannot be null or empty.", nameof(duration));

        try
        {
            // Use XmlConvert to parse the ISO 8601 duration
            return XmlConvert.ToTimeSpan(duration);
        }
        catch (FormatException ex)
        {
            throw new ArgumentException("Invalid ISO 8601 duration format.", ex);
        }
    }

    private static string Format(TimeSpan timeSpan)
    {
        // Start with the period designator
        string duration = "P";

        // Append days if any
        if (timeSpan.Days > 0)
            duration += $"{timeSpan.Days}D";

        // Append time designator if there are hours, minutes, or seconds
        if (timeSpan.Hours > 0 || timeSpan.Minutes > 0 || timeSpan.Seconds > 0)
        {
            duration += "T";

            // Append hours if any
            if (timeSpan.Hours > 0)
                duration += $"{timeSpan.Hours}H";

            // Append minutes if any
            if (timeSpan.Minutes > 0)
                duration += $"{timeSpan.Minutes}M";

            // Append seconds if any
            if (timeSpan.Seconds > 0)
                duration += $"{timeSpan.Seconds}S";
        }

        // If the TimeSpan represents zero time, explicitly set it to "PT0S"
        if (duration == "P")
            duration = "PT0S";

        return duration;
    }
}
