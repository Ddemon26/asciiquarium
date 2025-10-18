namespace Asciiquarium.Rendering;

/// <summary>
/// Utility class for managing console colors and color masks
/// </summary>
public static class ColorConsole
{
    private static readonly Random _random = new();

    // Available colors for random assignment (excluding black for visibility)
    private static readonly ConsoleColor[] _fishColors = new[]
    {
        ConsoleColor.Cyan,
        ConsoleColor.DarkCyan,
        ConsoleColor.Red,
        ConsoleColor.DarkRed,
        ConsoleColor.Yellow,
        ConsoleColor.DarkYellow,
        ConsoleColor.Blue,
        ConsoleColor.DarkBlue,
        ConsoleColor.Green,
        ConsoleColor.DarkGreen,
        ConsoleColor.Magenta,
        ConsoleColor.DarkMagenta
    };

    /// <summary>
    /// Get a random fish body color
    /// </summary>
    public static ConsoleColor GetRandomColor()
    {
        return _fishColors[_random.Next(_fishColors.Length)];
    }

    /// <summary>
    /// Apply random colors to numbered positions in a color mask
    /// Replaces digits 1-9 with random color codes
    /// </summary>
    public static string[] ApplyRandomColors(string[] colorMask)
    {
        var result = new string[colorMask.Length];
        var colorMap = new Dictionary<char, char>();

        // Generate random colors for each digit
        for (char digit = '1'; digit <= '9'; digit++)
        {
            colorMap[digit] = GetRandomColorChar();
        }

        // Apply the color mapping
        for (int i = 0; i < colorMask.Length; i++)
        {
            var line = colorMask[i];
            var newLine = new char[line.Length];

            for (int j = 0; j < line.Length; j++)
            {
                char c = line[j];
                if (char.IsDigit(c) && c >= '1' && c <= '9')
                {
                    newLine[j] = colorMap[c];
                }
                else
                {
                    newLine[j] = c;
                }
            }

            result[i] = new string(newLine);
        }

        return result;
    }

    /// <summary>
    /// Get a random color character for masks
    /// </summary>
    private static char GetRandomColorChar()
    {
        // Use both light and dark variants
        char[] colors = { 'c', 'C', 'r', 'R', 'y', 'Y', 'b', 'B', 'g', 'G', 'm', 'M' };
        return colors[_random.Next(colors.Length)];
    }

    /// <summary>
    /// Convert color mask character to ConsoleColor
    /// </summary>
    public static ConsoleColor CharToColor(char c, ConsoleColor defaultColor = ConsoleColor.White)
    {
        return c switch
        {
            'c' => ConsoleColor.DarkCyan,
            'C' => ConsoleColor.Cyan,
            'r' => ConsoleColor.DarkRed,
            'R' => ConsoleColor.Red,
            'y' => ConsoleColor.DarkYellow,
            'Y' => ConsoleColor.Yellow,
            'b' => ConsoleColor.DarkBlue,
            'B' => ConsoleColor.Blue,
            'g' => ConsoleColor.DarkGreen,
            'G' => ConsoleColor.Green,
            'm' => ConsoleColor.DarkMagenta,
            'M' => ConsoleColor.Magenta,
            'w' => ConsoleColor.Gray,
            'W' => ConsoleColor.White,
            'k' => ConsoleColor.Black,
            'K' => ConsoleColor.DarkGray,
            _ => defaultColor
        };
    }
}
