# Asciiquarium .NET

A .NET recreation of the classic Asciiquarium terminal animation, featuring smooth double-buffered rendering and colorful ASCII art creatures.

## Features

- **Double-buffered rendering** - Smooth, flicker-free animation
- **Colorful entities** - Fish, sharks, seaweed, bubbles, and more with vibrant colors
- **Collision detection** - Sharks eat small fish, bubbles pop at waterline
- **Entity variety** - Multiple fish types with random colors
- **Swaying seaweed** - Animated plant life at the ocean floor
- **Interactive controls** - Pause, redraw, and quit

## Requirements

- .NET 8.0 or later
- Terminal with color support

## Building and Running

```bash
# Build the project
dotnet build

# Run the aquarium
dotnet run
```

## Controls

- **Q** or **ESC** - Quit
- **P** or **SPACE** - Pause/Resume
- **R** - Redraw (recreate all entities)

## Architecture

The project is organized into several key components:

### Rendering System
- `DoubleBuffer.cs` - Double-buffered frame rendering to eliminate flicker
- `ColorConsole.cs` - Color management and random color assignment

### Core Engine
- `Entity.cs` - Base class for all animated entities
- `AnimationEngine.cs` - Manages entity lifecycle, collision detection, and rendering

### Entities
- `Fish.cs` - Multiple fish types with random colors
- `Shark.cs` - Predator that eats small fish
- `BigFish.cs` - Large fish varieties
- `Bubble.cs` - Rising air bubbles
- `Seaweed.cs` - Swaying plant life
- `Castle.cs` - Static castle structure
- `Waterline.cs` - Water surface

## How It Works

### Double Buffering
The `DoubleBuffer` class maintains two frame buffers:
- Back buffer: Where the next frame is drawn
- Front buffer: What's currently displayed

Each frame:
1. Clear the back buffer
2. Draw all entities sorted by depth (z-order)
3. Compare back buffer to front buffer
4. Only update changed characters on screen
5. Swap buffers

This approach eliminates flicker and minimizes terminal updates.

### Entity System
All entities inherit from the `Entity` base class which provides:
- Position (x, y, z-depth)
- Velocity for movement
- Animation frames
- Color masks
- Collision detection
- Lifecycle management

### Collision Detection
The `AnimationEngine` checks for collisions between physical entities each frame:
- Sharks have invisible "teeth" entities for collision
- Small fish are eaten when they collide with teeth
- Bubbles pop when they hit the waterline

## Extending the Project

### Adding New Entity Types

1. Create a new class in `Entities/` that inherits from `Entity`
2. Define the ASCII art frames and color masks
3. Implement any custom behavior in `OnUpdate()` or `OnCollision()`
4. Add a factory method to spawn the entity
5. Call the factory method from `Program.cs` or as a death callback

Example:
```csharp
public class MyEntity : Entity
{
    public static void AddMyEntity(AnimationEngine engine)
    {
        var entity = new MyEntity
        {
            X = 0, Y = 0, Depth = 10,
            Frames = new[] { new[] { "ASCII", "ART" } },
            // ... configure entity
        };
        engine.AddEntity(entity);
    }
}
```

### Color Masks

Color masks use single characters to specify colors:
- `c/C` - Cyan (dark/light)
- `r/R` - Red (dark/light)
- `y/Y` - Yellow (dark/light)
- `b/B` - Blue (dark/light)
- `g/G` - Green (dark/light)
- `m/M` - Magenta (dark/light)
- `w/W` - White (gray/white)
- `1-9` - Random colors (replaced at spawn time)

## Credits

Based on the original Asciiquarium by Kirk Baucom
- Original Perl version: http://robobunny.com/projects/asciiquarium
- ASCII art by Joan Stark and contributors

This .NET recreation focuses on double-buffered rendering for smooth animation.

## License

This project is open source. The original Asciiquarium is licensed under GPL v2.
