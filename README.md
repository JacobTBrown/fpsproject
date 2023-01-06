## **Controls:**
	Move up: W
	Move down: S
	Move left: A
	Move right: D
	Walk slowly: Left ctrl
	Sprint: Left shift
	Slide: F (Must be pressed while Sprinting)
	Jump: Space (Hold space to easily double-jump)
	Reload: R
	Menu: Escape
	Scoreboard: Tab
	Chat: NUM-PAD Enter
	Shooting(left mouse click/left pad press)

## **How to Launch the Game**
	Our app is inteded for one client per user, but multiple windows may be opened for testing.
	An executable has been provided for your convenience.
	To re-build a new executable in the Editor, select File -> BuildSettings(ctrl+shift+B)-> Build -> create a folder named "Build", and select it.
	Your executable can now be launched from the build folder /FPSProject/Build/FPSProject.exe", 
	You may also play the game in the Unity editor. If the editor does not default to the InitialScene, you may need to open the Initial scene in the Editor's Project window: "Assets/Scenes/InitialScene".


## **Guns & Powerups**
	An ItemSpawner respanws guns and powerups on a shared respawn timer of 10 sec.
	Powerups can optionally be placed as items that do not respawn
#### Guns
	Handguns are semi-automatic and have 8 bullets per magazine and do reasonable damage with reasonable distance.
	AKs are fully automatic and have 31 bullets per magazine and do reasonable damage with exceptional range.
	Shotguns are semi-automatic and have 6 bullets per magazine and do exceptional damage with subpar distance
#### Powerups
	Walk over the colored powerups to gain an advantage
	Red = Health	  Green = Super Speed	    Purple = Invincibility  	Yellow - Instant Kill
## **Settings**
	The user may rebind their keys. Cannot re-bind a key that is already in use.

## **Game Over / Time Limit**
	The Free For All game mode has a kill limit of 20 kills, and the Team Deathmatch game mode has a limit of 10 kills. When a player, or team, respectively, reaches the kill limit, the winner is displayed. However, there is a time limit of 5 minutes. If no party reaches the kill limit before the timer expires, no winner will be declared.


## **Minor Unexpected Behaviors** 
### When running multiple instance of the game, the name and stats data are overridden by other clients.
### In the welcome screen, we enter a name, but this applies only to the chat room. The in-game-name must be changed separately.
### The user can change their name in-game in the Settings Panel to remedy this.
### In the Setting Panel, a user cannot change a key that is already bound. An error message will popup on Map 1, but this is missing in Map 2 & Map 3.
### If a user has a very slow internet connection speed, their team's colors may not be loaded properly in TDM. 
	The player is still on the correct team, this is only a visual error of the player's model. This is an infrequently occuring bug that was missed during testing. 
