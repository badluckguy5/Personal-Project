## Overview
A game in development where the player progresses through levels by defeated enemies and completing objectives

<img width="600" height="428" alt="Main gameplay" src="https://github.com/user-attachments/assets/d32fddf4-4221-4c9c-b5b4-c00b4c38be0f" />

I built the game from the ground up to explore both game development and software engineering concepts, with an emphasis on designing systems that are modular and easy to extend. 
The player automatically tracks enemies and fires projectiles, but once fired they don't continually track enemies. This keeps the player from feeling too strong from the beginning.
Through defeating a certain number of different enemies, completing objectives, and equipping items, the player can temporarily or permanently increase their stats.
As the player progresses levels, they encounter more unique objectives and enemies; Such as a water environment where movement is slowed and firing is disabled,
or having to storm a building while enemies on the outside chase them indoors, encouraging working quickly.

## Features
- Save and load system between levels using JSON
- Global stat & enemy kill count tracking for player stats upgrades and unique abilities
- Equipment system that utilizes scriptable objects to easily create unique equipment and abilities
- Simple UI system for the player that includes health, ability, and equipment tracking
- Visible environmental factors that affect different entities differently
- Utilizes events to drive players forward and dynamically change locations

## Game Architecture

<img width="8006" height="1255" alt="Game System Management Flow-2026-09-04-201326" src="https://github.com/user-attachments/assets/8f707fa2-dfd4-4530-baa0-f39d6d2456d2" />

Game Manager: Loads/Saves levels, player equipment, stats, and kill tracker <br>
Level Manager: Tracks level objectives and progression, completes level <br>
Input Manager: Captures user's physical actions and translates them in game <br>
Inventory: Stores acquired item information <br>

Player Stats: Tracks and calculates player object information, such as movement speed or health points <br>
Player Equipment: Tracks equipped items along with stat changes and abilities acquired from equipped items <br>

Player UI: Puts the mini-map, player health, enemy health, and acquired abilities on the screen. <br>
Message Manager: Displays messages from the game to the player on the screen <br>
Inventory Window: Displays inventory window on the screen, enabling player to equip or unequip items. <br>

## Key Systems
Event System <br>
The event system is utilized for unique objectives and gameplay. Examples include when a player moves "indoors", an event is sent out and all enemies inside and outside both adjust their movement tracking to adjust. 
Another example would be whenever an item is added, removed, equipped or unequipped an event is sent out to refresh the inventory window and ability screen to adjust.

Upgrade System <br>
Scriptable Objects are utilized for easy creation of stat upgrades/upgrade milestones. Game Manager dynamically loads all these scriptable objects on start of the game, along
with the kill tracker. It then applies all completed stat upgrades to the player's stats. It continues to track all kills and applies upgrades as needed.

Save / Load <br>
The first screen opening the game enables reading a JSON file and loading player's last level, stat upgrades, kill tracker information, and acquired/equipped items. <br>
The game saves all this information on every level completion, and loads the information on every restart.

## Technical Challenges
PROBLEM: When the player "invades" a fort, move the player to another location but keep the enemies "outside" active and doing something because time wouldn't stop for them. <br>

SOLUTION: For enemies already created and "outside", have the fort send out an event that tells all enemies that the player is now indoors. The level manager will save the player's last
known location, and the outdoors enemies will move at a reduced speed towards the last known location. When they reach the invaded fort, the fort will allow them to "invade" like how the player does in order to 
keep chasing them. They will resume normal movement speed once they do. Any newly created enemies will check player's indoor/outdoor condition and follow suite. <br>

RESULT: Enemies have 2 different states, tracking a player that is indoors vs. outdoors. Events will be sent out any time they need to switch, and they will adjust automatically. 

<img width="600" height="428" alt="Indoors" src="https://github.com/user-attachments/assets/522e07c6-bb9e-4185-8e26-efb603f5b262" />



PROBLEM: Create a player UI that accurately tracks which items the player has equipped, which have abilities and any cooldown remaining on those abilities.

SOLUTION: Create two separate "inventories", one tracking all acquired items in the Game Manager and one tracking equipped items on the player. The Game Manager inventory is being used as a data bank 
and when the player equips an item, an instance will be created of that item in the player inventory. This instance is going to be the one that displays any relevant active information, such as cooldown. 

RESULT: Inventory Window can differentiate between multiple copies of the same item, and equipped items all have their own visible, separate cooldowns.

<img width="600" height="428" alt="Inventory" src="https://github.com/user-attachments/assets/6a1bc762-11f9-4609-a433-d0d98efb2c85" />


