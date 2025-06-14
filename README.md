# Legacy of Ninu

Help Ninu get through the ages!

For task management we're using [Jira](https://teodorlepadatu2004.atlassian.net/jira/software/projects/SCRUM/boards/1/backlog?atlOrigin=eyJpIjoiYzg0MjgyNjdlZWEzNDdmZmJkNjcxYWZiMDMyZDRiNTEiLCJwIjoiaiJ9)


## Controls

🕹 Movement
W / A / S / D or Arrow Keys – Move the player up, left, down, and right.

Movement is analog: diagonals and smooth transitions are supported.

🧨 Attacking

Q – Launch a projectile in the direction you're moving.



🗣 Interacting with NPCs

H – Talk to nearby NPCs when close.

Dialogue will be displayed on the screen if the NPC allows interaction.



👜 Picking Up / Dropping Items

P – Interact with pickable objects:

If you're not holding anything, pressing P will pick up a nearby object.

If you're already holding an object, pressing P will drop it in front of you.

## User stories

1.   As a player, when I start the game, I want to see a start menu so that I can navigate to different game options.

2.   As a player, I want to be able to pause the game at any moment so that I can take a break and resume later.

3.   As a player, when playing a logic-based minigame, I want to be able to request a hint so that I can get help solving the puzzle.

4.   As a player, I want to choose the order of the two minigames per era so that I can play based on my preference.

5.   As a player, I want to receive coins after each minigame so that I can track my progress.

6.   As a player, I want each land to represent a different historical era so that I can explore different time periods.

7.   As a player, I want to progress through the lands in chronological order so that I follow a logical timeline.

8.   As a player, I want to access a shop where I can buy items using the coins I earn in mini-games.

9.   As a player, I want to start with a standard character so that I have a base avatar to play with.

10.  As a player, I want background music to play during the game to enhance the atmosphere.

11.  As a player, I want to be able to adjust the music volume (increase, decrease, or mute) so that I can customize my experience.

12.  As a player, I want to be able to restart both the minigame that I am playing and the entire game whenever I want.

13.  As a player, I want to freely move on the map of the minigame I am currently playing.

14.  As a user, I want to be able to customize my controls using a settings menu.

## Prompt engineering

We have used ChatGPT and Github Copilot mostly for fixing bugs such as:

- bosses teleporting from a minigame to another

- minigames not ending

- settings not saving properly

- ideas for some aspects of the minigames

## Other aspects

We also have unit tests, comments in the code and design patterns such as singletons for utility classes.