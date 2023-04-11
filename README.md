# Pick Sword, Kill Slime
#### Video Demo: https://youtu.be/nfee1Nzn3MU

## Summary
You control a character in a rectangular arena where slimes, swords, and healing items regularly spawn. Your goal is to gain as much points as possible by defeating as many slimes that you can. The game ends once you run out of time or you are defeated by the slimes. Additionally, you gain 3 seconds more to your remaining time for each slime that you defeat.

Note, however, that you are unable to attack unless you have picked up a sword. Attacking consumes the sword, so you must pick up a new one in order to attack again. There is no limit to how many swords you can carry.

You are also able to pick up health items that spawn to increase your health once you get damaged.

*Note that this game has also been submitted as GD50's final project requirement*

## Scenes and Scene Management
The game has two scenes, the Main Menu Scene, where the player is able to learn how to play the game, and the Game Scene, where the bulk of the game happens. Transitions between scenes is handled by a Loader static class, which uses the public Scene enum to identify the scenes in the project. Should errors relating to scene transitions occur, debugging becomes easier as the location at which they may happen is isolated.

## Main Menu Scene
This scene contains two parent objects that serves as a container for a set of UI elements. The MainMenuUI parent contains the **Play**, **How to Play**, and **Quit** buttons. The **Play** button transitiontions to the Game Scene and the **Quit** button closes the application. The **How to Play** button on the other hand disables the MainMenuUI and opens the HowToPlayUI, which contains the controls the a player needs to play the game. A **Back** button is also present to disable the HowToPlayUI and re-enables the MainMenuUI.

## Game Scene
The Game Scene contains the bulk of the whole project. As explained above, the player goes around the rectangular arena, picking up swords and health items, to defeat as many slimes as possible.

### Game Manager
The main state machine is implemented by the GameManager script where the possible states are stored in a private enum called **State**, in which one of them will be assigned to the current state variable named **state**. Since these variables have been setup as private variables, the state machine can only be manipulated by the GameManager script. Public methods have been built in order for other game objects within the scene to know what state the game is currently in. The way the GameManager knows when to change the state is through an event system which acts as triggers found in other game objects.

In order for other game objects to access some of the GameManager's public methods, a public static GameManager named **Instance** was made. This way, game objects that require the GameManager would not need to reference the GameManager object as a Serialized Field.

### High Score Manager
The highscore of a player is managed by this class containing a static method to get and set a player's high score via Unity's built in PlayerPrefs class.

### Game Input
This script manages the player's input using Unity's built-in Input System. As a separate object, this will be referenced by the Player game object in order for the Player to move and act based on player's input.

### Scriptable Objects and Interface Implementation
Scriptable Objects have been used a number of times to generalize data between similar objects, particulary used for pickable items and entites, as well as a list of used sound effects for the game. The Pickable Scriptable Objects only contain data for their icon, name and a reference to the prefab that uses this Scriptable Object. The Entity Scriptable Object is used for the Player and the Enemies and contains various data like movement speed,damage, and the like. Though it may be argued that the use of Scriptable Objects in this way may be unnecessary for a project this small, this makes the game easy to expand when adding new enemy types or pickable items.

Only one interface has been implemented in this project, the IDamageable interface. Players and Enemies implement this interface and is required to include the methods defined in the IDamageable script. This guarantees other scripts that the method that they wish to call exists.

### Player
The Player game object contains the script that handles player input as well as public methods that would interact with enemies and pickable items. Player movement and interaction only happens during the GamePlay state, which can be known by the Game Manager's IsPlayable public method.

The Player game object also acts as a parent game object for its visuals and UI elements and interact with each other via an event system. The reason for this design choice is to separate each logic, making it easier to debug should problems arise.

The Player Animator game object contains the animator for the Player and handles visuals the player may have depending on what action was triggered via the Player game objects events. This also contains several key frame events that triggers events which the Player game object relies on. Due to the hitbox's size and position relying on player visuals during the attack animation, the hitbox was placed as a child of the Player Animator game object.

The UI game object displays the player's sword amount as well as remaining health when damaged. These visuals rely, once again, on the triggers from Player's events when hit by an enemy or when picking up an item.

### Enemy
The Enemy game object works similarly with the Player in terms of structure. However, different from the Player, the Enemy contains a Detector that regularly checks if the Player is within the detection range and triggers an event should they do. As long as the player is within the detection range, the Enemy would constantly move towards the Player.

Additionally there is a SpawnEnemy public static method can be called to spawn an enemy at a given location. This method is used by an Enemy Spawner.

### Pickable Items
These objects interact with the Player via the Player script rather than have a public method for the pickable item. Similarly to the Enemy, a SpawnPickableObject public static method can be called to spawn the pickable item at a given location.

A player picking up a sword increases their sword count thus increasing the number of times they can attack. The health item can only be picked up by the player if their health is below the maximum.

### Spawners
Spawner game objects are present in the scene and are responsible for spawning enemies and pickable items using their respective public static spawn methods. They spawn their assigned objects at given time intervals within a specified bounds and can spawn no more than the specified amount.

### SFX Manager
The SFX Manager handles all audio within the game. Each sound effect is triggered via subscribed events from other game objects in the scene.

## Credits
Sound Effects from freesounds.org

Pixel Art assets

Mystic Woods by Game Endeavor (https://game-endeavor.itch.io/)

UI Assets

Kyrise's 16x16 RPG Icon Pack by Kyrise (https://kyrise.itch.io/)

Pixel Fantasy Icons Keyboard by Caz Wolf (https://cazwolf.itch.io/)
