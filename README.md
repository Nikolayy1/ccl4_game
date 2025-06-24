# Fury Road

## Setup (for development)
Make sure to have editor version 2022.3.35f1!
- create empty folder on your device
- navigate to that folder in terminal
- git clone https://github.com/Nikolayy1/ccl4_game.git
- open Unity Hub
- Click on Add Project
- Select the folder (the one you created and contains the git repo content)
- open project
- should generate all the necessary files to compile (if not gg)
- start working ON DEV BRANCH (so use git checkout dev)
- important commands: git fetch, git pull, git add ., git commit -m "commit message", git push


# Concept
Fury Road is a fast-paced endless runner set in a grimy, post-collapse wasteland. The player takes control of a scrappy, hand-animated goblin sprinting down an unstable, debris-littered road. <br>
The goal: stay alive, dodge hazards, and collect Shiny Bits to grow your fame and extend your run.


## Gameplay
- The goblin runs forward automatically in a three-lane layout.
- The player collects Shiny Bits, a goblin currency that:
  - Increases the score
  - Extends the countdown timer — goblins get a "greed rush" when they collect
- Occasional Magic Mushroom Potions appear:
  - Provide a speed boost
- Hazards and obstacles slow the player. 
- When the timer runs out: game over.

![mermaid-diagram-2025-06-23-223418](https://github.com/user-attachments/assets/f1404bac-0620-47a0-b045-24eba3b192a7)

## Final Features

- **3-Lane Endless Runner**  
  The goblin runs forward automatically, and the player can switch between left, center, and right lanes to dodge hazards or collect items.

- **Shiny Bit (Coin) System**  
  Collectible coins that increase score & extend a bit of your game time. Every 250 coins triggers the RobberNPC event.

- **Magic Mushroom Potions**  
  Pickups that grant a speed boost. Yet, every 5 potions spawn a ScavengerNPC.

- **Custom NPCs**  
  - **Robber Goblin**: Immobilizes player and steals 25% of current score, also applies a basic speed penalty  
  - **Masked Scavenger**: Breathes vomit fire on a lane, applying a stackable speed penalty

- **Procedural Terrain System**  
  Endless generation of terrain chunks, each with randomized obstacles, coins, and traps

  - **Checkpoint System**  
  Special chunks appear every eight segments. When crossed, they add bonus time to the player's remaining game time and reduce the flying obstacle spawn interval, subtly increasing difficulty as the run progresses.

- **Flying Obstacle System**
  Random junk objects like barrels, tires, and toilets are spawned midair and slide horizontally across the screen. These obstacles are activated during chunk generation and move under diagonal gravity or custom physics, forcing players to react quickly or switch lanes   to avoid impact.

- **Wwise Spatial Audio**  
  Positional sound for coins, traps, NPCs, and background music, fully integrated using Wwise

- **Road Traps & Hazards**  
  Bear traps, rusty tire trip hazards, and vomit fire zones slow the player, adding additional difficulty.

- **Custom 3D Models & Animations**  
  Original character, enemies, and props like barrels, traps, and coins — all modeled and rigged in Blender

- **Environment Polish**  
  Fog particle systems, lighting tweaks, and camera zoom/tilt using Cinemachine

- **Game State Management**  
  StateManager that tracks the score, storing it in the Credits Scene 


## System Design
**Scenes & Flow**  
- **Intro** Intro about the game story & concept 
- **Menu** → Play, Highscore, Credits 
- **Game** Runs until timer ≤ 0 → GameOver  
- **GameOver** back to Menu button


## System Infrastructure

The project is organized into five logic domains: **Player**, **Pickups**, **Procedural Generation**, **Managers**, and **NPCs & Special Hazards**.

![TLJDpjCm4BpdAQmUk4NY0H0g5Ab5ugSYYFl6NGNBZXsjDqKjyEwak5-oRjnJafdTyQvdb6eR6OZ7tbNH6Zy0GQy6q1OwRFZYK15g1jyv50NQGOplcCcq8-D77NnZhgXULzMzLMsE3Yv86_MxKgbS9MGNF5Dm3cacTj0ZGKRa7SwPQ88_W3_TwB_o6AFjE4HvJe8MzM3ymWMUX7852dgFNYf](https://github.com/user-attachments/assets/7971905f-2242-4c99-800e-c762a5c4fd30)

### Player

- **`PlayerController.cs`**  
  Manages movement, input, lane switching, jumping (logic only), and animation triggers. Includes trap state and speed debuff handling.

- **`CameraController.cs`**  
  Adjusts Cinemachine camera FOV and vertical offset based on player speed, keeping the scene dynamic and readable.

- **`PlayerCollisionHandler.cs`**  
  Detects and handles collisions with traps, vomit, and interactive objects. Applies debuffs and interacts with managers.


### Pickups

- **`Pickup.cs`**  
  Abstract class for all pickups — defines detection logic and sound playback.

- **`Coin.cs`**  
  Increases score and time, triggers audio when collected.

- **`Potion.cs`**  
  Applies a temporary speed boost and tracks potion count for Scavenger spawning.


### Procedural Generation

- **`LevelGenerator.cs`**  
  Spawns new chunks in front of the player and recycles old ones behind. Coordinates obstacle and pickup placement.

- **`Chunk.cs`**  
  Marks each terrain segment and triggers regeneration once passed.

- **`Checkpoint.cs`**  
  Gives bonus time (+5s) when touched. Plays sound.

- **`ObstacleSpawner.cs`**  
  Randomly spawns obstacles within a chunk at init time.

- **`ObstacleDestroyer.cs`**  
  Destroys obstacles that pass out of view behind the player.


### Managers

- **`GameManager.cs`**  
  Handles game flow (start, game over, transitions) and scene logic.

- **`ScoreManager.cs`**  
  Tracks and updates score and time, triggers game over on timeout.

- **`StateManager.cs`**  
  Persists player score between scenes (e.g., from game to credits).


### NPCs & Special Hazards

- **`RobberNPC.cs`**  
  Spawns after every 250 collected coins. Immobilizes player and steals 25% of the current score, then leaves you free with a movement penalty on top.

- **`ScavengerNPC.cs`**  
  Spawns after 5 potions. Flies (animation) over one of the three lanes, projectile vomits it on fire, and laughs when you come into contact with the vomit-colored fire.

- **`VomitSegment.cs`**  
  Represents a patch of vomit fire. Applies a stackable speed penalty that ticks every second the player is in contact with the fire.

- **`BearTrap.cs`**  
  Comes with an animation trigger when stepped on, traps the player in place for an amount of time, and applies a movement penalty.

- **`Rock.cs`**  
  A heavy obstacle that drops onto a random lane and collides with the player. On impact, it applies a strong speed penalty (`-5f`), shakes the camera using a Cinemachine impulse, plays a particle effect at the contact point, and triggers a sound effect via Wwise. A cooldown timer prevents it from firing multiple times in quick succession.

- **`PotionTracker.cs`**  
  Singleton manager to track how many potions have been collected globally, important for the ScavengerNPC spawn.


## Models and Mood
### Main Character - Goblin
![grafik](https://github.com/user-attachments/assets/54361ea6-6e1e-4d2a-ba9e-20607618670f)
Animations:
- Idle
- Running 
- Jumping
- Collision/Damage
- Death 

### NPCs
#### Robber NPC 
![grafik](https://github.com/user-attachments/assets/cc18f77e-a0a7-4cee-8267-0aca2617bbbf)

#### Masked Scavenger
![grafik](https://github.com/user-attachments/assets/43112f8b-f4bf-4469-aeda-ba03c46d7736)

#

## How to Run the Program (Playable in Unity)

1. **Download or clone** the repository:
   git clone https://github.com/Nikolayy1/ccl4_game.git  
2. Open the project using Unity Hub (recommended version: 2022.3.x)  
3. Open the intro scene in Unity: Assets/Scenes/IntroAboutTheGame.unity  
4. Press the Play button in Unity to start the game. From the intro, you'll be directed to the main menu and then into the game level.

### Controls
- A / D – Switch lanes  
- Spacebar – Jump (unfortunately, not functional)  

### Known Issues
- Jump and hit animations were planned, but couldn't be implemented due to various issues
- If the run and idle animations do not work on the first download/try, open the animator of the main character and reapply the animations, "refreshing" them. 
- Audio may not play immediately due to Wwise integration quirks — restarting Unity or checking sound settings can help


