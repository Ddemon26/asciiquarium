using Asciiquarium.Core.Core;
using Asciiquarium.Core.Rendering;
namespace Asciiquarium.Core.Entities;

/// <summary>
///     Large fish that swim across the screen
/// </summary>
public class BigFish : Entity {
    static readonly Random _random = new();

    static readonly string[] BigFishRightFrame = new[] {
        " ______",
        "`\"\"-. `````-----.....__",
        "     `.  .      .       `-.",
        "       :     .     .       `.",
        " ,?????:   .    .          _ :",
        ": `.???:                  (@) `._",
        " `. `..'     .     =`-.       .__)",
        "   ;     .        =  ~  :     .-\"",
        " .' .'`.   .    .  =.-'  `._ .'",
        ": .'???:               .   .'",
        " '???.'  .    .     .   .-'",
        "   .'____....----''.'=.'",
        "   \"\"?????????????.'.'",
        "               ''\"'`",
    };

    static readonly string[] BigFishRightMask = new[] {
        " 111111",
        "11111  11111111111111111",
        "     11  2      2       111",
        "       1     2     2       11",
        " 1     1   2    2          1 1",
        "1 11   1                  1W1 111",
        " 11 1111     2     1111       1111",
        "   1     2        1  1  1     111",
        " 11 1111   2    2  1111  111 11",
        "1 11   1               2   11",
        " 1   11  2    2     2   111",
        "   111111111111111111111",
        "   11             1111",
        "               11111",
    };

    static readonly string[] BigFishLeftFrame = new[] {
        "                           ______",
        "          __.....-----'''''  .-\"\"'",
        "       .-'       .      .  .'",
        "     .'       .     .     :",
        "    : _          .    .   :?????,",
        " _.' (@)                  :???.' :",
        "(__.       .-'=     .     `..' .'",
        " \"-.     :  ~  =        .     ;",
        "   `. _.'  `-.=  .    .   .'`. `.",
        "     `.   .               :???`. :",
        "       `-.   .     .    .  `.???`",
        "          `.=`.``----....____`.",
        "            `.`.?????????????\"\"",
        "              '`\"``",
    };

    static readonly string[] BigFishLeftMask = new[] {
        "                           111111",
        "          11111111111111111  11111",
        "       111       2      2  11",
        "     11       2     2     1",
        "    1 1          2    2   1     1",
        " 111 1W1                  1   11 1",
        "1111       1111     2     1111 11",
        " 111     1  1  1        2     1",
        "   11 111  1111  2    2   1111 11",
        "     11   2               1   11 1",
        "       111   2     2    2  11   1",
        "          111111111111111111111",
        "            1111             11",
        "              11111",
    };

    public static void AddBigFish(AnimationEngine engine, Entity? oldFish = null) {
        bool goingRight = _random.Next( 2 ) == 0;
        var speed = 3f;
        int x;

        string[] frame;
        string[] mask;

        if ( goingRight ) {
            frame = BigFishRightFrame;
            mask = BigFishRightMask;
            x = -34;
        }
        else {
            frame = BigFishLeftFrame;
            mask = BigFishLeftMask;
            speed = -speed;
            x = engine.Width - 1;
        }

        // Apply random colors
        string[] colorMask = ColorConsole.ApplyRandomColors( mask );

        var minY = 9;
        int maxY = engine.Height - 15;
        int y = _random.Next( minY, Math.Max( minY + 1, maxY ) );

        var bigFish = new BigFish {
            X = x,
            Y = y,
            Depth = 2,
            VelocityX = speed,
            Type = "bigfish",
            DieOffscreen = true,
            Frames = new[] { frame },
            ColorMasks = new[] { colorMask },
            DefaultColor = ConsoleColor.Yellow,
            DeathCallback = entity => AddRandomObject( engine ),
        };

        engine.AddEntity( bigFish );
    }

    // Placeholder for random object spawning
    static void AddRandomObject(AnimationEngine engine) {
        // Will be called when big fish dies
        int choice = _random.Next( 3 );
        switch (choice) {
            case 0:
                AddBigFish( engine );
                break;
            case 1:
                Shark.AddShark( engine );
                break;
            default:
                AddBigFish( engine );
                break;
        }
    }
}