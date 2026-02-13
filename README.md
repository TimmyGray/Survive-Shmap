# Survive Shmap

# Shoot 'em All - MVP Description

## Practical Scope Recommendation (for a first playable build)

To keep development realistic and shippable, split work into three milestones:

1. **M1 - Playable Core (1-2 weeks)**
   - One player ship, one weapon (Blaster), and 2 enemy types
   - Survive loop with spawning, collisions, HP, death, and restart
   - Basic XP gain and 3 level-up choices

2. **M2 - Retention Layer (1-2 weeks)**
   - Add 2 more weapons from the current list
   - Add one mini-boss and one simple meta currency upgrade
   - Add settings (audio, controls) and save/load for upgrades

3. **M3 - Polish + Validation (1 week)**
   - Visual/audio feedback pass (hit flash, simple particles, SFX)
   - Balance pass (enemy HP/speed curves, weapon tuning)
   - Small playtest and bug-fix sprint

### What to postpone until after MVP

Move these ideas to a post-launch backlog so they don't block release:
- Story mode, weekly events, community events
- Companion/drone system
- Dynamic environment hazards (solar flares, gravity wells)
- Deep achievement + leaderboard ecosystem
- Multiple ship classes with unique skill trees

### Success criteria for MVP

- New player can start a run in **under 20 seconds**
- Stable 10-minute run with no game-breaking bugs
- At least 1 meaningful build decision every 2-3 levels
- Clear death summary screen with XP/Gold earned
- Consistent performance on target hardware (define FPS target)

## Core Gameplay

### Basic Mechanics
- Movement: Arrow keys (following cursor)
- Scrolling: Left to right
- Basic auto-firing system with cooldown (forward direction)
- Session duration: 15 minutes
- Resource types:
  * Session points (XP)
  * Global upgrade points (Gold)

### Combat System
- Enemies approach from different directions
  * Straight paths
  * Diagonal paths
  * Wave patterns
  * Rear attacks (with warning system)
- Boss encounters:
  * Mini-boss every 5 minutes
  * Final boss (conditional appearance)

### Progression System
- In-session upgrades through level-ups
- Multiple effect choices per level
- Effect combinations for enhanced DPS
- Ultimate combinations unlock super weapons

## Weapons & Effects

### Basic Weapons
1. Blaster
   - Straight shots
   - Upgrades: Speed, Damage, Penetration

2. Bracket Shot
   - Short-range expanding shot

3. Rocket Launcher
   - Upgrades: Speed, Blast radius

4. Triple Shot
   - Three-way firing pattern

### Defensive Systems
- Pulsating shield
  * Upgradeable duration and area
- Orbital station
  * Moving damage zone around the ship

### Stats Upgrades
- Maximum HP increase
- Health regeneration

## MVP Requirements
- Functional player ship
- Enemy systems
- Hit detection
- Timer system
- XP counter (drops from kills)
- Gold counter (time-based acquisition)
- Death registration

## Menu Structure
- Start Game
- Settings
- Upgrade Shop
- Exit


# Game Enhancement Suggestions

## Setting & Theme
### Cyberpunk Space Setting
- Neon-colored environments
- Tech-enhanced enemies
- Futuristic city backdrops
- Holographic UI elements

### Ship Variety
- Light-class ships (high speed, low HP)
- Medium-class ships (balanced stats)
- Heavy-class ships (high HP, low speed)
- Each class with unique special abilities

### Dynamic Environment
- Space debris fields
- Solar flares affecting gameplay
- Asteroid fields requiring navigation
- Weather anomalies (space storms, etc.)

## Advanced Gameplay Features

### Combo System
- Multiplier for consecutive kills
- Time-based combo maintenance
- Special rewards for high combos
- Unique visual effects for combo stages

### Environmental Mechanics
- Destructible obstacles
- Shield recharge stations
- Gravity wells affecting movement
- Teleport gates for tactical positioning

### Mission Objectives
- Escort missions
- Resource collection
- Territory control
- Survivor rescue

### Companion System
- Deployable combat drones
- Different drone specializations:
  * Attack drones
  * Shield drones
  * Healing drones
  * Resource collector drones

## Enhanced Progression

### Ship Specializations
- Attack specialist
- Defense specialist
- Support specialist
- Speed specialist

### Permanent Upgrades
- Ship part modifications
- Weapon modifications
- Core stat improvements
- Special ability enhancements

### Challenge System
#### Daily Challenges
- Unique mission parameters
- Special rewards
- Leaderboard competition
- Rotating challenge types

#### Achievement System
- Combat achievements
- Collection achievements
- Skill-based achievements
- Special ship skins as rewards

## Technical Enhancements

### Visual Feedback
- Screen shake on impacts
- Hit freeze frames
- Dynamic camera zoom
- Flash effects

### Particle Systems
- Weapon trails
- Engine effects
- Explosion particles
- Shield impacts

### Audio Design
- Dynamic music system
  * Intensity based on combat
  * Boss fight themes
  * Ambient exploration
- Sound effect variety
- Positional audio

### Visual Effects
- Power-up animations
- Level-up effects
- Achievement pop-ups
- Kill streak indicators

## Additional Content

### Game Modes
- Story mode
- Endless survival
- Time attack
- Wave defense

### Special Events
- Weekly challenges
- Limited-time events
- Special boss encounters
- Community events

### Boss Rush Mode
- Sequential boss fights
- Minimal breaks between fights
- Special rewards
- Leaderboard system

## Quality of Life Features

### Tutorial System
- Interactive tutorials
- Training room
- Weapon testing area
- Strategy guides

### Statistics & Analysis
- Detailed combat stats
- Achievement progress
- Personal best records
- Comparison charts

### Performance Rating
- Mission grade system
- Combat efficiency rating
- Accuracy tracking
- Speed run times

## Common Enemies

| Enemy Type | Description | Attack Patterns | Movement |
| --- | --- | --- | --- |
| **Scout Drone** | Small, agile drone. Weak but numerous. | Single-shot laser. | Erratic, quick movements. |
| **Interceptor** | Fast, maneuverable ship. Moderate threat. | Twin blasters, short bursts. | Weaving patterns, quick turns. |
| **Bomber** | Slow, heavily armored ship. Drops explosive charges. | Large area-of-effect bombs. | Straightforward, predictable movement. |
| **Turret** | Stationary defense platform. Heavy firepower. | Rapid laser fire, limited firing arc. | None, but can rotate to track the player. |
| **Mines** | Stationary explosives. Detonate on proximity. | High damage explosion in a small radius. | None, but can be deployed in patterns. |

## Elite Enemies (Mini-Bosses)

| Enemy Type | Description | Special Abilities |
| --- | --- | --- |
| **Bruiser** | Heavily armored ship with powerful shields. | Ramming attack, shockwave blast. |
| **Specter** | Fast, cloaking ship. Difficult to hit. | Short-range teleport, homing missiles. |
| **Overseer** | Support ship that buffs other enemies. | Deploys shield drones, increases fire rate. |

## Boss Enemies (End-of-Session)

| Enemy Type | Description | Attack Patterns & Phases |
| --- | --- | --- |
| **Dreadnought** | Massive capital ship with overwhelming firepower. | Multiple weapon systems, shield regeneration, summons smaller ships. |
| **Leviathan** | Gigantic serpentine creature native to the asteroid field. | Charges through the battlefield, spits projectiles, creates shockwaves by slamming into the arena walls. |
| **Void Ripper** | Interdimensional entity capable of manipulating space-time. | Teleportation, energy projectiles, creates black holes that pull the player in. |
