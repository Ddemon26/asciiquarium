using Asciiquarium.Rendering;

namespace Asciiquarium.Core;

/// <summary>
/// Main animation engine that manages all entities and handles updates/rendering
/// </summary>
public class AnimationEngine
{
    private readonly List<Entity> _entities = new();
    private readonly List<Entity> _entitiesToAdd = new();
    private readonly List<Entity> _entitiesToRemove = new();
    private readonly DoubleBuffer _buffer;
    private readonly int _width;
    private readonly int _height;

    public int Width => _width;
    public int Height => _height;

    public AnimationEngine(int width, int height)
    {
        _width = width;
        _height = height;
        _buffer = new DoubleBuffer(width, height);
    }

    /// <summary>
    /// Add an entity to the animation
    /// </summary>
    public void AddEntity(Entity entity)
    {
        _entitiesToAdd.Add(entity);
    }

    /// <summary>
    /// Remove an entity from the animation
    /// </summary>
    public void RemoveEntity(Entity entity)
    {
        _entitiesToRemove.Add(entity);
    }

    /// <summary>
    /// Get all entities of a specific type
    /// </summary>
    public List<Entity> GetEntitiesOfType(string type)
    {
        return _entities.Where(e => e.Type == type && e.IsAlive).ToList();
    }

    /// <summary>
    /// Remove all entities
    /// </summary>
    public void Clear()
    {
        _entities.Clear();
        _entitiesToAdd.Clear();
        _entitiesToRemove.Clear();
    }

    /// <summary>
    /// Force a full screen redraw
    /// </summary>
    public void ForceRedraw()
    {
        _buffer.ForceRedraw();
    }

    /// <summary>
    /// Update all entities
    /// </summary>
    public void Update(float deltaTime)
    {
        // Add pending entities
        if (_entitiesToAdd.Count > 0)
        {
            _entities.AddRange(_entitiesToAdd);
            _entitiesToAdd.Clear();
        }

        // Update all entities
        foreach (var entity in _entities)
        {
            if (entity.IsAlive)
            {
                entity.Update(deltaTime, _width, _height);
            }
        }

        // Handle collisions
        DetectCollisions();

        // Remove dead entities and call death callbacks
        foreach (var entity in _entities.Where(e => !e.IsAlive).ToList())
        {
            _entitiesToRemove.Add(entity);
            entity.DeathCallback?.Invoke(entity);
        }

        // Remove pending entities
        if (_entitiesToRemove.Count > 0)
        {
            foreach (var entity in _entitiesToRemove)
            {
                _entities.Remove(entity);
            }
            _entitiesToRemove.Clear();
        }
    }

    /// <summary>
    /// Detect collisions between physical entities
    /// </summary>
    private void DetectCollisions()
    {
        var physicalEntities = _entities.Where(e => e.IsAlive && e.IsPhysical).ToList();

        for (int i = 0; i < physicalEntities.Count; i++)
        {
            var entity1 = physicalEntities[i];
            entity1.Collisions.Clear();

            for (int j = i + 1; j < physicalEntities.Count; j++)
            {
                var entity2 = physicalEntities[j];

                if (entity1.CollidesWith(entity2))
                {
                    entity1.Collisions.Add(entity2);
                    entity2.Collisions.Add(entity1);

                    // Notify both entities of collision
                    entity1.OnCollision(entity2);
                    entity2.OnCollision(entity1);
                }
            }
        }
    }

    /// <summary>
    /// Render all entities to the screen
    /// </summary>
    public void Render()
    {
        // Clear buffer
        _buffer.Clear();

        // Sort entities by depth (higher depth = drawn later = appears in front)
        var sortedEntities = _entities
            .Where(e => e.IsAlive)
            .OrderBy(e => e.Depth)
            .ToList();

        // Render each entity
        foreach (var entity in sortedEntities)
        {
            entity.Render(_buffer);
        }

        // Present the buffer to the screen
        _buffer.Present();
    }

    /// <summary>
    /// Get the current entity count
    /// </summary>
    public int GetEntityCount()
    {
        return _entities.Count(e => e.IsAlive);
    }

    /// <summary>
    /// Get statistics about entity types
    /// </summary>
    public Dictionary<string, int> GetEntityStats()
    {
        return _entities
            .Where(e => e.IsAlive)
            .GroupBy(e => e.Type)
            .ToDictionary(g => g.Key, g => g.Count());
    }
}
