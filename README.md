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
- Occasional Mushrooms appear:
  - Provide a temporary speed boost and regenerate health
- Hazards/Obstacles reduce health or slow the player. 
- When the timer or HP runs out: game over.

![mermaid-diagram-2025-06-10-185338](https://github.com/user-attachments/assets/3db948f2-0b2a-453f-bfdb-486968d1a61a)


## Key Features to Implement
- 3-Lane Endless Runner: Automatic forward movement across left, center, right lanes
- Shiny Bit Collection: Increases game timer & adds score
- Magic Mushroom Potion (Power-ups): Grants Speed Boost and Health Boost (❤ UI)
- Obstacles & Hazards: tires, chainsaw, stove, toilet, etc. Random junkyard stuff.
- Robber Goblin (Obstacle): After every 25 Shiny Bits, dodge or get walloped, and taxed - 25% tax!
- Masked Scavenger (Obstacle): Spawns after 5 Mushrooms. Breathes fire down the lane. Dodge or receive heavy damage.
- Animation States: Idle, Running, Jumping, Damaged/Collision, (Death?)
- Audio Feedback: Background music, footsteps, pickups, damage, jumps, power-up, etc. via WWise
- UI Elements: Timer bar, score counter, health (❤)

### Features Implemented as of 13.06.25
- Movement System, automatic forward movement across left, center, and right lanes
- Obstacles & Hazards
- Magic Mushroom Potion (Speed Boost, visualized with particle system)
- Shiny Bits: collecting coins increases the score
- (New) Checkpoints - every 8 chunks, a checkpoint appears that increases the timer by +5s
- More to come...

## System Design
**Scenes & Flow**  
- **Menu** → Play, Highscore, Credits 
- **Game** (runs until timer ≤ 0 or health ≤ 0) → GameOver  
- **GameOver** → Score display, Retry/Quit
  
## System Infrastructure
![Class Diagram](https://github.com/user-attachments/assets/0aa2ba14-ef13-4e66-b1b8-e136088b4971)


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



