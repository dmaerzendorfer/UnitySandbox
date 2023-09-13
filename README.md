# TrainsAndTracksTool
A little collection of perhaps useful little tools and gadgets for unity.

- Spline Tool: A small tool for creating splines in unity and letting things follow the generated track. (dont use it, there is a better free package in the store -> Dreamteck Splines)

- A Healthbar Component: an implementation for a health system, includes events for onDamage, onHeal, onHealthChange and onDeath. Supports different types of health representations (HealthBar ->normal unity slider, Hearts -> integer health represented as health(needs to be extended for half hearts etc), Shader -> a procedual healthbar shader (by https://github.com/josebasierra/procedural-healthbar-shader))

- A playerController with a Movement Hierarchical State Machine: a simple finite state machine is included as well, furthermore some tinkering with scriptable object as stat-containers has been done, the playerController uses unities new input system, demo controller contains a verlet integration fps consistent triple jump
//todo: add a scene with four players!
//todo: add lobby screen where controllers/players can join connect
//todo: add a playerManager that other scripts cann access to get the players stats
//todo: make stats more generic -> an interface or abstract base class -> has getter/setter with string for member names or even better
https://www.youtube.com/watch?v=WLDgtRNK2VE
this seems fun :D

//for lobby stuff
https://www.youtube.com/watch?v=_5pOiYHJgl0
fuck it, stop for now. tomorrow do the whole tutorial one by one and then change it afterwards
start: 10:20 break 11.20
todo: fix ui movement then try my own shit :)
continue 12:24

todo: cleanup, delete old tutorial stuff, check if scene change works, then update player controller to use the new playerconfigs
then -> make something for testing so we dont always have to do lobby stuff first


- A Menu template with options and scene transitions; transitions are handled by the LevelLoader, There is a prefab for the main menu and a pause menu. The settings of the menu are saved in the playerPrefs and are accessible from anywhere
//todo: look into scene management and add it here -> eG having persistent scene with level transition etc.


- Phones as controllers: implemented in a way to fit into unities new Input system.

- A Behaviour Tree: for some ai, perhaps add some steering behaviours; tbd in more detail; check: https://www.youtube.com/watch?v=nKpM98I7PeM&t=124s

- A nice 2D jump: to be done in the future (partly in the playerController thingy, want to do a specific one however)

- A Dialog system: tbd in detail see https://gamedevbeginner.com/dialogue-systems-in-unity/

- something with a shader: want to write one myself: TBD about what
