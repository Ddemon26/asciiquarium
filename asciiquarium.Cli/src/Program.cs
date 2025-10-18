using System.Diagnostics;
using Asciiquarium.Core.Core;
using Asciiquarium.Core.Entities;
namespace asciiquarium.Cli;

internal class Program {
    static AnimationEngine? _engine;
    static bool _paused;
    static bool _running = true;
    static readonly Random _random = new();

    static void Main(string[] args) {
        try {
            // Setup console
            Console.CursorVisible = false;
            Console.Clear();

            // Get console dimensions
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            // Create animation engine
            _engine = new AnimationEngine( width, height );

            // Initialize the aquarium
            InitializeAquarium();

            // Start input thread
            var inputThread = new Thread( HandleInput );
            inputThread.IsBackground = true;
            inputThread.Start();

            // Main animation loop
            RunAnimationLoop();
        }
        catch (Exception ex) {
            Console.Clear();
            Console.WriteLine( $"Error: {ex.Message}" );
            Console.WriteLine( ex.StackTrace );
        }
        finally {
            Console.CursorVisible = true;
            Console.Clear();
        }
    }

    static void InitializeAquarium() {
        if ( _engine == null ) return;

        // Add environment
        Waterline.AddWaterline( _engine );

        // Add static elements
        Castle.AddCastle( _engine );

        // Add seaweed
        Seaweed.AddAllSeaweed( _engine );

        // Add fish
        Fish.AddAllFish( _engine );

        // Add random objects (shark, big fish)
        AddRandomObject();
    }

    static void AddRandomObject() {
        if ( _engine == null ) return;

        // Randomly add a shark or big fish
        int choice = _random.Next( 2 );
        if ( choice == 0 ) {
            Shark.AddShark( _engine );
        }
        else {
            BigFish.AddBigFish( _engine );
        }
    }

    static void RunAnimationLoop() {
        if ( _engine == null ) return;

        var stopwatch = Stopwatch.StartNew();
        float lastTime = 0;
        const float targetFrameTime = 1.0f / 30.0f; // 30 FPS
        float bubbleTimer = 0;

        while (_running) {
            var currentTime = (float)stopwatch.Elapsed.TotalSeconds;
            float deltaTime = currentTime - lastTime;

            // Cap delta time to prevent huge jumps
            if ( deltaTime > 0.1f ) deltaTime = 0.1f;

            if ( !_paused && deltaTime >= targetFrameTime ) {
                lastTime = currentTime;

                // Update all entities
                _engine.Update( deltaTime );

                // Randomly add bubbles to fish
                bubbleTimer += deltaTime;
                if ( bubbleTimer >= 0.5f ) // Check every half second
                {
                    bubbleTimer = 0;
                    List<Entity> fish = _engine.GetEntitiesOfType( "fish" );
                    foreach (var f in fish) {
                        if ( _random.Next( 100 ) > 97 ) {
                            Bubble.AddBubble( _engine, f );
                        }
                    }
                }

                // Render
                _engine.Render();
            }

            // Small sleep to prevent CPU spinning
            Thread.Sleep( 1 );
        }
    }

    static void HandleInput() {
        while (_running) {
            if ( Console.KeyAvailable ) {
                var key = Console.ReadKey( true ).Key;

                switch (key) {
                    case ConsoleKey.Q:
                    case ConsoleKey.Escape:
                        _running = false;
                        break;

                    case ConsoleKey.P:
                    case ConsoleKey.Spacebar:
                        _paused = !_paused;
                        break;

                    case ConsoleKey.R:
                        if ( _engine != null ) {
                            _engine.Clear();
                            InitializeAquarium();
                            _engine.ForceRedraw();
                        }

                        break;
                }
            }

            Thread.Sleep( 10 );
        }
    }
}