# Proto_Progression
Prototype 2 for VIZA 689 Adv Game Design, in which at least three different progression mechanics must be demonstrated.

The word "different" may seem superfluous until you understand that it is applied to the means of progression, not of play. The requirement is to implement mechanics that cause the player to experience progression in three separate "categories", where progression, for the purposes of this project, is loosely defined as "that which controls the player's access to game content". As such, 'solving a puzzle to unlock a door' falls under the same classification of progression as 'requiring a player to complete a platforming challenge to cross a chasm' because both mechanics control the player's access to a spatial area of the game. So it is not that three mechanics contributing to progression must be present, but that three categories of progression must be mechanically supported.

This game, a roguelite platformer, has several categories of progression:
- The quantity and variety of enemies is determined by the current difficulty counter, which is driven by the amount of time that has passed in the current run. Therefore the progression of difficulty in each run is driven by the natural progression of time.
- During a run, players earn points by defeating enemies. If the player voluntarily ends the run by leaving the arena before they run out of health, they may spend those points on upgrades between runs (but they lose those points if the run ends by losing all their health). Upgrades include new mechanics that can change how the player interacts with the game, so a combination of player skill and time investment drives progression of mechanics. This is a combination because the player has the choice of how long they wish to remain in the level, and can either accumulate more points faster by persisting in greater but more rewarding difficulties, or accumulate points more slowly by ending quicker on lower difficulties.
- There are some aspects of the arena that cannot be accessed until the player has unlocked a corresponding ability that allows them to traverse the obstacle. Therefore the progression of mechanics drives progression in access to level areas.


[NOTE: This project references paid assets that cannot be publicly redistributed. The project may not work correctly without importing these assets.]
- DOTween (Assets/Plugins/Demigiant)
- OdinInspector (Assets/Plugins/Sirenix)
- UModeler (Assets/tripolygon)
- Easy Character Movement (Assets/Easy Character Movement)