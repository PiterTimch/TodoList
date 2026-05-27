using System.Globalization;

namespace BLL.Helpers;

public static class DateStringParser
{
    private const string DateFormat = "yyyy-MM-dd";

    public static DateTime? ParseToUtcDate(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (!DateOnly.TryParseExact(
                value.Trim(),
                DateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateOnly dateOnly))
        {
            throw new ArgumentException($"Invalid due date format. Expected {DateFormat}.");
        }

        return dateOnly.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
    }
}
