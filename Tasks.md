# Tasks to Mirror Original Asciiquarium Logic

1. Refactor the core runtime so entities and callbacks receive the animation engine context, allowing death handlers and per-frame logic to spawn secondary entities (e.g., random objects, bubbles) exactly like the Perl version.
2. Implement the full random-object roster (ship, whale, new/old monsters, both big-fish variants, shark) with their original sprites, colour masks, speeds, depths, and death callbacks, ensuring only one random object is active at a time and a new one is queued when the previous leaves the screen.
3. Restore shark behaviour to remove attached teeth entities on death and funnel respawns through the random-object manager instead of hard-coding another shark.
4. Expand `Fish` to include every old and new fish sprite/mask pair, replicate the original spawn probabilities, honour the classic (`-c`) mode toggle, and randomise initial positions and directions precisely like `add_new_fish`/`add_old_fish`.
5. Move bubble emission into the fish update flow with the same per-frame probability as `fish_callback`, using the engine context instead of the CLI loop so bubbles originate from the correct fin positions.
6. Port the blood "splat" animation that appears when small fish collide with shark teeth, including timing, colour, and automatic cleanup frames.
7. Add the ship, whale, and monster movement patterns (including animation frame cycling, colour masks, and spout/eye effects) as dedicated entity classes and hook them into the random-object rotation.
8. Flesh out `BigFish` so it supports both original variants and uses the same randomised colour masking logic as the Perl `add_big_fish_1`/`add_big_fish_2` routines.
9. Match seaweed and castle rendering details (sway timing distribution, lifespan, colour masks/default colours) to the values in `add_seaweed` and `add_castle`.
10. Update the CLI to expose the classic mode switch, reinitialise the scene exactly like the Perl redraw path, and ensure user controls mirror the original behaviour (`q`, `p`, `r`).
