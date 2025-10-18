using Asciiquarium.Core.Core;
namespace Asciiquarium.Core.Entities;

/// <summary>
///     Invisible teeth entity for shark collision detection
/// </summary>
public class Teeth : Entity { }

/// <summary>
///     Shark that eats small fish
/// </summary>
public class Shark : Entity {
    static readonly Random _random = new();

    static readonly string[][] SharkRight = new[] {
        new[] {
            "                              __",
            "                             ( `\\",
            "  ,??????????????????????????)   `\\",
            ";' `.????????????????????????(     `\\__",
            " ;   `.?????????????__..---''          `~~~~-._",
            "  `.   `.____...--''                       (b  `--._",
            "    >                     _.-'      .((      ._     )",
            "  .`.-`--...__         .-'     -.___.....-(|/|/|/|/'",
            " ;.'?????????`. ...----`.___.',,,_______......---'",
            " '???????????'-'",
        },
    };

    static readonly string[][] SharkRightMask = new[] {
        new[] {
            "",
            "",
            "",
            "",
            "",
            "                                           cR",
            " ",
            "                                          cWWWWWWWW",
            "",
            "",
        },
    };

    static readonly string[][] SharkLeft = new[] {
        new[] {
            "                     __",
            "                    /' )",
            "                  /'   (??????????????????????????,",
            "              __/'     )????????????????????????.' `;",
            "      _.-~~~~'          ``---..__?????????????.'   ;",
            " _.--'  b)                       ``--...____.'   .'",
            "(     _.      )).      `-._                     <",
            " `\\|\\|\\|\\|)-.....___.-     `-.         __...--'-.'.",
            "   `---......_______,,,`.___.'----... .'?????????`.;",
            "                                     `-`???????????`",
        },
    };

    static readonly string[][] SharkLeftMask = new[] {
        new[] {
            "",
            "",
            "",
            "",
            "",
            "        Rc",
            "",
            "  WWWWWWWWc",
            "",
            "",
        },
    };
    Teeth? _teethEntity;

    public static void AddShark(AnimationEngine engine, Entity? oldShark = null) {
        // Clean up old teeth entity if shark died
        if ( oldShark is Shark oldSharkInstance && oldSharkInstance._teethEntity != null ) {
            oldSharkInstance._teethEntity.Kill();
        }

        bool goingRight = _random.Next( 2 ) == 0;
        var speed = 2f;
        int x, teethX;
        int y = _random.Next( 9, engine.Height - 10 );
        int teethY = y + 7;

        string[][] frame;
        string[][] mask;

        if ( goingRight ) {
            frame = SharkRight;
            mask = SharkRightMask;
            x = -53;
            teethX = -9;
        }
        else {
            frame = SharkLeft;
            mask = SharkLeftMask;
            speed = -speed;
            x = engine.Width - 2;
            teethX = x + 9;
        }

        // Create the shark
        var shark = new Shark {
            X = x,
            Y = y,
            Depth = 2,
            VelocityX = speed,
            Type = "shark",
            DieOffscreen = true,
            Frames = frame,
            ColorMasks = mask,
            DefaultColor = ConsoleColor.Cyan,
            DeathCallback = entity => AddShark( engine, entity ),
        };

        // Create invisible "teeth" entity for collision detection
        var teeth = new Teeth {
            X = teethX,
            Y = teethY,
            Depth = 3,
            VelocityX = speed,
            Type = "teeth",
            IsPhysical = true,
            Frames = new[] { new[] { "*" } },
            ColorMasks = Array.Empty<string[]>(),
            DefaultColor = ConsoleColor.White,
            IsTransparent = true,
            DieOffscreen = true,
        };

        shark._teethEntity = teeth;

        engine.AddEntity( shark );
        engine.AddEntity( teeth );
    }
}