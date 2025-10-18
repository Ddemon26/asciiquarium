using Asciiquarium.Core.Core;
namespace Asciiquarium.Core.Entities;

/// <summary>
///     Castle structure at the bottom-right of the aquarium
/// </summary>
public class Castle : Entity {
    public static void AddCastle(AnimationEngine engine) {
        var castleImage = new[] {
            "               T~~",
            "               |",
            "              /^\\",
            "             /   \\",
            " _   _   _  /     \\  _   _   _",
            "[ ]_[ ]_[ ]/ _   _ \\[ ]_[ ]_[ ]",
            "|_=__-_ =_|_[ ]_[ ]_|_=-___-__|",
            " | _- =  | =_ = _    |= _=   |",
            " |= -[]  |- = _ =    |_-=_[] |",
            " | =_    |= - ___    | =_ =  |",
            " |=  []- |-  /| |\\   |=_ =[] |",
            " |- =_   | =| | | |  |- = -  |",
            " |_______|__|_|_|_|__|_______|",
        };

        var castleMask = new[] {
            "                RR",
            "                ",
            "              yyy",
            "             y   y",
            "            y     y",
            "           y       y",
            "",
            "",
            "",
            "              yyy",
            "             yy yy",
            "            y y y y",
            "            yyyyyyy",
        };

        var castle = new Castle {
            X = engine.Width - 32,
            Y = engine.Height - 13,
            Depth = 22,
            Type = "castle",
            DefaultColor = ConsoleColor.DarkGray,
            Frames = new[] { castleImage },
            ColorMasks = new[] { castleMask },
        };

        engine.AddEntity( castle );
    }
}