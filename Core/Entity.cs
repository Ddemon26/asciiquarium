using Asciiquarium.Rendering;

namespace Asciiquarium.Core;

/// <summary>
/// Base class for all animated entities in the aquarium
/// </summary>
public abstract class Entity
{
    // Position
    public float X { get; set; }
    public float Y { get; set; }
    public int Depth { get; set; }  // Z-order for layering

    // Movement
    public float VelocityX { get; set; }
    public float VelocityY { get; set; }

    // Animation
    public string[][] Frames { get; set; } = Array.Empty<string[]>();
    public string[][] ColorMasks { get; set; } = Array.Empty<string[]>();
    protected int CurrentFrameIndex { get; set; } = 0;
    protected float FrameTimer { get; set; } = 0;
    public float FrameRate { get; set; } = 0.25f; // Time between frames in seconds

    // Rendering
    public ConsoleColor DefaultColor { get; set; } = ConsoleColor.White;
    public bool IsTransparent { get; set; } = true; // Treat spaces and '?' as transparent

    // Lifecycle
    public bool IsAlive { get; set; } = true;
    public bool DieOffscreen { get; set; } = false;
    public float LifeTime { get; set; } = 0; // How long entity has been alive
    public float MaxLifeTime { get; set; } = float.MaxValue; // When to die (-1 = infinite)

    // Collision
    public bool IsPhysical { get; set; } = false; // Can collide with other entities
    public string Type { get; set; } = "entity";
    public List<Entity> Collisions { get; private set; } = new();

    // Callback
    public Action<Entity>? DeathCallback { get; set; }

    /// <summary>
    /// Update the entity's state
    /// </summary>
    public virtual void Update(float deltaTime, int screenWidth, int screenHeight)
    {
        // Update lifetime
        LifeTime += deltaTime;
        if (LifeTime >= MaxLifeTime)
        {
            Kill();
            return;
        }

        // Update position
        X += VelocityX * deltaTime;
        Y += VelocityY * deltaTime;

        // Update animation frame
        if (Frames.Length > 1)
        {
            FrameTimer += deltaTime;
            if (FrameTimer >= FrameRate)
            {
                FrameTimer = 0;
                CurrentFrameIndex = (CurrentFrameIndex + 1) % Frames.Length;
            }
        }

        // Check if offscreen
        if (DieOffscreen)
        {
            var (width, height) = GetSize();
            if (X + width < 0 || X > screenWidth ||
                Y + height < 0 || Y > screenHeight)
            {
                Kill();
                return;
            }
        }

        // Custom update logic
        OnUpdate(deltaTime, screenWidth, screenHeight);
    }

    /// <summary>
    /// Custom update logic for derived classes
    /// </summary>
    protected virtual void OnUpdate(float deltaTime, int screenWidth, int screenHeight)
    {
    }

    /// <summary>
    /// Render the entity to the buffer
    /// </summary>
    public virtual void Render(DoubleBuffer buffer)
    {
        if (!IsAlive || Frames.Length == 0)
            return;

        var frame = Frames[CurrentFrameIndex];
        var colorMask = ColorMasks.Length > CurrentFrameIndex ? ColorMasks[CurrentFrameIndex] : null;

        int startX = (int)X;
        int startY = (int)Y;

        for (int lineIdx = 0; lineIdx < frame.Length; lineIdx++)
        {
            string line = frame[lineIdx];
            string? maskLine = colorMask != null && lineIdx < colorMask.Length ? colorMask[lineIdx] : null;

            for (int charIdx = 0; charIdx < line.Length; charIdx++)
            {
                char c = line[charIdx];

                // Skip transparent characters
                if (IsTransparent && (c == ' ' || c == '?'))
                    continue;

                // Determine color
                ConsoleColor color = DefaultColor;
                if (maskLine != null && charIdx < maskLine.Length)
                {
                    color = ColorConsole.CharToColor(maskLine[charIdx], DefaultColor);
                }

                // Replace '?' with space for rendering
                if (c == '?')
                    c = ' ';

                buffer.Write(startX + charIdx, startY + lineIdx, c, color);
            }
        }
    }

    /// <summary>
    /// Get the size of the current frame
    /// </summary>
    public (int width, int height) GetSize()
    {
        if (Frames.Length == 0 || CurrentFrameIndex >= Frames.Length)
            return (0, 0);

        var frame = Frames[CurrentFrameIndex];
        int height = frame.Length;
        int width = frame.Length > 0 ? frame.Max(line => line.Length) : 0;

        return (width, height);
    }

    /// <summary>
    /// Get bounding box for collision detection
    /// </summary>
    public (int x, int y, int width, int height) GetBounds()
    {
        var (width, height) = GetSize();
        return ((int)X, (int)Y, width, height);
    }

    /// <summary>
    /// Check if this entity collides with another
    /// </summary>
    public bool CollidesWith(Entity other)
    {
        var (x1, y1, w1, h1) = GetBounds();
        var (x2, y2, w2, h2) = other.GetBounds();

        return x1 < x2 + w2 &&
               x1 + w1 > x2 &&
               y1 < y2 + h2 &&
               y1 + h1 > y2;
    }

    /// <summary>
    /// Handle collision with another entity
    /// </summary>
    public virtual void OnCollision(Entity other)
    {
        // To be overridden by derived classes
    }

    /// <summary>
    /// Kill this entity
    /// </summary>
    public void Kill()
    {
        IsAlive = false;
    }
}
