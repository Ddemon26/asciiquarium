using Asciiquarium.Core.Core;
namespace Asciiquarium.Core.Entities;

/// <summary>
///     Water surface segments that tile across the top of the screen
/// </summary>
public class Waterline : Entity {
    public static void AddWaterline(AnimationEngine engine) {
        var waterSegments = new[] {
            "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~",
            "^^^^ ^^^  ^^^   ^^^    ^^^^      ",
            "^^^^      ^^^^     ^^^    ^^     ",
            "^^      ^^^^      ^^^    ^^^^^^  ",
        };

        // Tile segments across the screen width
        int segmentWidth = waterSegments[0].Length;
        int repeatCount = engine.Width / segmentWidth + 1;

        for (var i = 0; i < waterSegments.Length; i++) {
            string tiledSegment = string.Concat( Enumerable.Repeat( waterSegments[i], repeatCount ) );

            var waterline = new Waterline {
                X = 0,
                Y = 5 + i,
                Depth = 6 + i * 2, // Alternating depths for layering
                Type = "waterline",
                IsPhysical = true,
                DefaultColor = ConsoleColor.Cyan,
                Frames = new[] { new[] { tiledSegment } },
                ColorMasks = Array.Empty<string[]>(),
            };

            engine.AddEntity( waterline );
        }
    }
}