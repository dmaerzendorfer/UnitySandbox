# TrainsAndTracksTool
A little collection of perhaps useful little tools and gadgets for unity.

- Spline Tool: A small tool for creating splines in unity and letting things follow the generated track. (dont use it, there is a better free package in the store -> Dreamteck Splines)

- A Healthbar Component: an implementation for a health system, includes events for onDamage, onHeal, onHealthChange and onDeath. Supports different types of health representations (HealthBar ->normal unity slider, Hearts -> integer health represented as health(needs to be extended for half hearts etc), Shader -> a procedual healthbar shader (by https://github.com/josebasierra/procedural-healthbar-shader))

- A playerController with a Movement Hierarchical State Machine: a simple finite state machine is included as well, furthermore some tinkering with scriptable object as stat-containers has been done, the playerController uses unities new input system, demo controller contains a verlet integration fps consistent triple jump
In addition: this also includes a PlayerConfigurationManager using unities Player Input Manager. On player join it creates a PlayerConfig prefab and an object that has a players input. A Lobby system uses this to wait for four players to join, ready up and then changes scene. The PlayerInput Manager also houses and distributes stats for players.

- A Menu template with options and scene transitions; transitions are handled by the LevelLoader, There is a prefab for the main menu and a pause menu. The settings of the menu are saved in the playerPrefs and are accessible from anywhere
//todo: look into scene management and add it here -> eG having persistent scene with level transition etc.

- A Score System: A script for counting points, extendable to be used for multiple players and multiple nested levels of points eG points for the meta-game and points for minigames inside of the meta game.
Includes an (untested) implementation of a ScorePersister that saves and loads the mainScore as a json from/to the streamingAssets.

- A fake liquid shader based on: https://www.youtube.com/watch?v=DKSpgFuKeb4

- A really fancy polished knockback on other players: eg camera shake, nice duration, force, still controll, ample amount of juice, sfx, vfx, particles etc...

- My own implementation of a raycast: just for funsies

- A custom UI grid component: check out "Game Dev Guide" on youtube. unity ui grid sucks

- A Dialog system: tbd in detail see https://gamedevbeginner.com/dialogue-systems-in-unity/

- Phones as controllers: implemented in a way to fit into unities new Input system.

- A Behaviour Tree: for some ai, perhaps add some steering behaviours; tbd in more detail; check: https://www.youtube.com/watch?v=nKpM98I7PeM&t=124s

- A nice 2D jump: to be done in the future (partly in the playerController thingy, want to do a specific one however)


- something with a shader: want to write one myself: TBD about what
