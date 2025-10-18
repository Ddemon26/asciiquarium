using Asciiquarium.Core.Core;
namespace Asciiquarium.Core.Entities;

/// <summary>
///     Air bubbles that rise from fish to the surface
/// </summary>
public class Bubble : Entity {
    public static void AddBubble(AnimationEngine engine, Entity fish) {
        (int fishWidth, int fishHeight) = fish.GetSize();
        float bubbleX = fish.X;
        float bubbleY = fish.Y + fishHeight / 2;

        // If fish is moving right, place bubble at the right edge
        if ( fish.VelocityX > 0 ) {
            bubbleX += fishWidth;
        }

        var bubble = new Bubble {
            X = bubbleX,
            Y = bubbleY,
            Depth = fish.Depth - 1, // Bubble appears on top of fish
            VelocityY = -1, // Rise upward
            Type = "bubble",
            DefaultColor = ConsoleColor.Cyan,
            IsPhysical = true,
            DieOffscreen = true,
            Frames = new[] {
                new[] { "." },
                new[] { "o" },
                new[] { "O" },
                new[] { "O" },
                new[] { "O" },
            },
            ColorMasks = Array.Empty<string[]>(),
            FrameRate = 0.1f,
        };

        engine.AddEntity( bubble );
    }

    public override void OnCollision(Entity other) {
        // Pop when hitting the waterline
        if ( other.Type == "waterline" ) {
            Kill();
        }
    }
}