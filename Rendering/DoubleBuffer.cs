namespace Asciiquarium.Rendering;

/// <summary>
/// Double-buffered frame rendering system to eliminate flicker
/// </summary>
public class DoubleBuffer
{
    private char[,] _frontBuffer;
    private char[,] _backBuffer;
    private ConsoleColor[,] _frontColors;
    private ConsoleColor[,] _backColors;
    private readonly int _width;
    private readonly int _height;
    private bool _firstRender = true;

    public int Width => _width;
    public int Height => _height;

    public DoubleBuffer(int width, int height)
    {
        _width = width;
        _height = height;
        _frontBuffer = new char[height, width];
        _backBuffer = new char[height, width];
        _frontColors = new ConsoleColor[height, width];
        _backColors = new ConsoleColor[height, width];

        Clear();
    }

    /// <summary>
    /// Clear the back buffer with spaces
    /// </summary>
    public void Clear()
    {
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                _backBuffer[y, x] = ' ';
                _backColors[y, x] = ConsoleColor.Black;
            }
        }
    }

    /// <summary>
    /// Write a character to the back buffer at specified position
    /// </summary>
    public void Write(int x, int y, char c, ConsoleColor color = ConsoleColor.White)
    {
        if (x >= 0 && x < _width && y >= 0 && y < _height)
        {
            _backBuffer[y, x] = c;
            _backColors[y, x] = color;
        }
    }

    /// <summary>
    /// Write a string to the back buffer at specified position
    /// </summary>
    public void Write(int x, int y, string text, ConsoleColor color = ConsoleColor.White)
    {
        for (int i = 0; i < text.Length; i++)
        {
            int xPos = x + i;
            if (xPos >= 0 && xPos < _width && y >= 0 && y < _height)
            {
                _backBuffer[y, xPos] = text[i];
                _backColors[y, xPos] = color;
            }
        }
    }

    /// <summary>
    /// Write multi-line text to the back buffer
    /// </summary>
    public void WriteMultiline(int x, int y, string[] lines, ConsoleColor color = ConsoleColor.White)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            Write(x, y + i, lines[i], color);
        }
    }

    /// <summary>
    /// Write multi-line text with color mask
    /// Colors: c/C=Cyan, r/R=Red, y/Y=Yellow, b/B=Blue, g/G=Green, m/M=Magenta, w/W=White, k/K=Black
    /// Numbers 1-9 are placeholders for random colors (handled by caller)
    /// </summary>
    public void WriteMultilineWithMask(int x, int y, string[] lines, string[] colorMask, ConsoleColor defaultColor = ConsoleColor.White)
    {
        for (int lineIdx = 0; lineIdx < lines.Length; lineIdx++)
        {
            string line = lines[lineIdx];
            string mask = lineIdx < colorMask.Length ? colorMask[lineIdx] : "";

            for (int charIdx = 0; charIdx < line.Length; charIdx++)
            {
                char c = line[charIdx];
                ConsoleColor color = defaultColor;

                if (charIdx < mask.Length)
                {
                    color = CharToColor(mask[charIdx], defaultColor);
                }

                Write(x + charIdx, y + lineIdx, c, color);
            }
        }
    }

    /// <summary>
    /// Convert color character to ConsoleColor
    /// </summary>
    private ConsoleColor CharToColor(char c, ConsoleColor defaultColor)
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

    /// <summary>
    /// Swap buffers and render only changed regions to console
    /// </summary>
    public void Present()
    {
        Console.CursorVisible = false;

        if (_firstRender)
        {
            // First render: draw everything
            Console.Clear();
            Console.SetCursorPosition(0, 0);

            var currentColor = ConsoleColor.White;
            Console.ForegroundColor = currentColor;

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    var color = _backColors[y, x];
                    if (color != currentColor)
                    {
                        Console.ForegroundColor = color;
                        currentColor = color;
                    }
                    Console.Write(_backBuffer[y, x]);
                }
            }

            _firstRender = false;
        }
        else
        {
            // Subsequent renders: only update changed cells
            var currentColor = Console.ForegroundColor;

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    if (_backBuffer[y, x] != _frontBuffer[y, x] ||
                        _backColors[y, x] != _frontColors[y, x])
                    {
                        Console.SetCursorPosition(x, y);

                        var color = _backColors[y, x];
                        if (color != currentColor)
                        {
                            Console.ForegroundColor = color;
                            currentColor = color;
                        }

                        Console.Write(_backBuffer[y, x]);
                    }
                }
            }
        }

        // Swap buffers
        (_frontBuffer, _backBuffer) = (_backBuffer, _frontBuffer);
        (_frontColors, _backColors) = (_backColors, _frontColors);
    }

    /// <summary>
    /// Force a full redraw on next Present()
    /// </summary>
    public void ForceRedraw()
    {
        _firstRender = true;
    }
}
