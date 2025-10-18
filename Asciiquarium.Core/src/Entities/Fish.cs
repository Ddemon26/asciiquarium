using Asciiquarium.Core.Core;
using Asciiquarium.Core.Rendering;
namespace Asciiquarium.Core.Entities;

/// <summary>
///     Fish entities with multiple types and colors
/// </summary>
public class Fish : Entity {
    static readonly Random _random = new();

    // Fish designs with their right-facing and left-facing frames
    static readonly (string[] right, string[] rightMask, string[] left, string[] leftMask)[] FishTypes = new[] {
        // Small fish 1
        (
            new[] { "   \\", "  / \\", ">=_('", ">", "  \\_/", "   /" },
            new[] { "   2", "  1 1", "661745", "  111", "   3" },
            new[] { "  /", " / \\", "<')_=<", " \\_/", "  \\" },
            new[] { "  2", " 1 1", "547166", " 111", "  3" }
        ),
        // Small fish 2
        (
            new[] { "  __", "><_'>", "   '" },
            new[] { "  11", "61145", "   3" },
            new[] { " __", "<'_><", " `" },
            new[] { " 11", "54116", " 3" }
        ),
        // Small fish 3
        (
            new[] { "  ,\\", ">=('", ">", "  '/" },
            new[] { "  12", "66745", "  13" },
            new[] { " /,", "<')=<", " \\`" },
            new[] { " 21", "54766", " 31" }
        ),
        // Medium fish 1
        (
            new[] { "       \\", "     ...\\...,", "\\  /'       \\", " >=     (  ' >", "/  \\      / /", "    `\"'\"'/'" },
            new[] { "       2", "     1112111", "6  11       1", " 66     7  4 5", "6  1      3 1", "    11111311" },
            new[] { "      /", "  ,../...", " /       '\\  /", "< '  )     =<", " \\ \\      /  \\", "  `'\\'\"'\"'" },
            new[] { "      2", "  1112111", " 1       11  6", "5 4  7     66", " 1 3      1  6", "  11311111" }
        ),
        // Medium fish 2
        (
            new[] { "    \\", "\\ /--\\", ">=  (o>", "/ \\__/", "    /" },
            new[] { "    2", "6 1111", "66  745", "6 1111", "    3" },
            new[] { "  /", " /--\\ /", "<o)  =<", " \\__/ \\", "  \\" },
            new[] { "  2", " 1111 6", "547  66", " 1111 6", "  3" }
        ),
        // Medium fish 3
        (
            new[] { "   ..,\\", ">='   ('>", "  '''/'" },
            new[] { "   1121", "661   745", "  111311" },
            new[] { "  ,..", "<')   `=<", " ``\\```" },
            new[] { "  1211", "547   166", " 113111" }
        ),
    };

    public static void AddFish(AnimationEngine engine, Entity? oldFish = null) {
        // Choose random fish type
        int fishTypeIndex = _random.Next( FishTypes.Length );
        (string[] rightFrame, string[] rightMask, string[] leftFrame, string[] leftMask) = FishTypes[fishTypeIndex];

        // Determine direction (left or right)
        bool goingRight = _random.Next( 2 ) == 0;
        var speed = (float)(_random.NextDouble() * 2 + 0.25);

        string[] frame;
        string[] mask;

        if ( goingRight ) {
            frame = rightFrame;
            mask = rightMask;
        }
        else {
            frame = leftFrame;
            mask = leftMask;
            speed = -speed;
        }

        // Apply random colors to the mask (replace numbers with color codes)
        string[] colorMask = ColorConsole.ApplyRandomColors( mask );

        // Random depth for fish layering
        int depth = _random.Next( 3, 21 ); // Between fish_start and fish_end

        // Random Y position (below water, above bottom)
        var minY = 9;
        int maxY = engine.Height - frame.Length - 1;
        int y = _random.Next( minY, Math.Max( minY + 1, maxY ) );

        // Start position based on direction
        int x = goingRight ? -GetMaxLineLength( frame ) : engine.Width;

        var fish = new Fish {
            X = x,
            Y = y,
            Depth = depth,
            VelocityX = speed,
            Type = "fish",
            IsPhysical = true,
            DieOffscreen = true,
            Frames = new[] { frame },
            ColorMasks = new[] { colorMask },
            DefaultColor = ConsoleColor.Yellow,
            DeathCallback = entity => AddFish( engine, entity ),
        };

        engine.AddEntity( fish );
    }

    public static void AddAllFish(AnimationEngine engine) {
        // Figure out how many fish to add by screen size
        int screenSize = (engine.Height - 9) * engine.Width;
        int fishCount = screenSize / 350;

        for (var i = 0; i < fishCount; i++) {
            AddFish( engine );
        }
    }

    protected override void OnUpdate(float deltaTime, int screenWidth, int screenHeight) {
        // Randomly generate bubbles
        if ( _random.Next( 100 ) > 97 ) {
            // Need access to engine to add bubble - we'll handle this via a callback pattern
            // For now, skip bubble generation in this method
        }
    }

    public override void OnCollision(Entity other) {
        // Fish can be eaten by shark teeth
        if ( other.Type == "teeth" ) {
            (_, int height) = GetSize();
            if ( height <= 5 ) // Only small fish get eaten
            {
                Kill();
            }
        }
    }

    static int GetMaxLineLength(string[] lines) {
        return lines.Length > 0 ? lines.Max( line => line.Length ) : 0;
    }
}