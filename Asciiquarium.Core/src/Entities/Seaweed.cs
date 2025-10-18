using Asciiquarium.Core.Core;
namespace Asciiquarium.Core.Entities;

/// <summary>
///     Swaying seaweed at the bottom of the aquarium
/// </summary>
public class Seaweed : Entity {
    static readonly Random _random = new();

    public static void AddSeaweed(AnimationEngine engine, Entity? oldSeaweed = null) {
        int height = _random.Next( 3, 8 ); // Random height 3-7 segments

        // Create two animation frames for swaying effect
        List<string> leftFrame = new();
        List<string> rightFrame = new();

        for (var i = 0; i < height; i++) {
            if ( i % 2 == 0 ) {
                leftFrame.Add( "( " );
                rightFrame.Add( " )" );
            }
            else {
                leftFrame.Add( " )" );
                rightFrame.Add( "( " );
            }
        }

        var seaweed = new Seaweed {
            X = _random.Next( 1, engine.Width - 2 ),
            Y = engine.Height - height,
            Depth = 21,
            Type = "seaweed",
            DefaultColor = ConsoleColor.Green,
            Frames = new[] { leftFrame.ToArray(), rightFrame.ToArray() },
            ColorMasks = Array.Empty<string[]>(),
            FrameRate = (float)(_random.NextDouble() * 0.1 + 0.25), // Random sway speed
            MaxLifeTime = _random.Next( 8 * 60, 12 * 60 ), // Lives 8-12 minutes
            DeathCallback = entity => AddSeaweed( engine, entity ),
        };

        engine.AddEntity( seaweed );
    }

    public static void AddAllSeaweed(AnimationEngine engine) {
        // Figure out how many seaweed to add by the width of the screen
        int seaweedCount = engine.Width / 15;
        for (var i = 0; i < seaweedCount; i++) {
            AddSeaweed( engine );
        }
    }
}