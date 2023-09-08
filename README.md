# TrainsAndTracksTool
A little collection of perhaps useful little tools and gadgets for unity.

- Spline Tool: A small tool for creating splines in unity and letting things follow the generated track. (dont use it, there is a better free package in the store -> Dreamteck Splines)

- A Healthbar Component: an implementation for a health system, includes events for onDamage, onHeal, onHealthChange and onDeath. Supports different types of health representations (HealthBar ->normal unity slider, Hearts -> integer health represented as health(needs to be extended for half hearts etc), Shader -> a procedual healthbar shader (by https://github.com/josebasierra/procedural-healthbar-shader))

- A playerController with a Movement Hierarchical State Machine: a simple finite state machine is included as well, furthermore some tinkering with scriptable object as stat-containers has been done, the playerController uses unities new input system, demo controller contains a verlet integration fps consistent triple jump
//todo: add a scene with four players!

- A Menu template with options and scene transitions (and animations! maybe a shader?) etc: to be done in the future
https://www.youtube.com/watch?v=zc8ac_qUXQY
https://www.youtube.com/watch?v=YOaYQrN1oYQ
https://www.youtube.com/watch?v=JivuXdrIHK0
levelLoader with transitions
https://www.youtube.com/watch?v=CE9VOZivb3I
-> for loading animation -> use doTween!
save the options as playerPrefs -> for game settings
https://www.youtube.com/watch?v=UNpuhewlAfQ
or as jsonutility -> for save game states
https://www.youtube.com/watch?v=aSNj2nvSyD4

https://blog.unity.com/games/persistent-data-how-to-save-your-game-states-and-settings

//todo: look into scene management and add it here -> eG having persistent scene with level transition etc.

- Phones as controllers: implemented in a way to fit into unities new Input system.

- A game system: base class with access to all players and their data

- A Behaviour Tree: for some ai, perhaps add some steering behaviours; tbd in more detail; check: https://www.youtube.com/watch?v=nKpM98I7PeM&t=124s

- A nice 2D jump: to be done in the future (partly in the playerController thingy, want to do a specific one however)

- A Dialog system: tbd in detail see https://gamedevbeginner.com/dialogue-systems-in-unity/

- something with a shader: want to write one myself: TBD about what

