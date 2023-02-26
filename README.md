# Galactic Asteroid Miner v1.0
![GAM Hero](https://img.itch.zone/aW1hZ2UvMTc0MDIyMS8xMDI1MjYwNi5qcGc=/original/VVM%2FqJ.jpg)

2D Infinite procedurally generated asteroid mining rouge-like. Created as a small proof-of concept based on the game [Motherload](http://www.xgenstudios.com/play/motherload).

<br>

Play the game for yourself on [Itch.io](https://zephyrmg.itch.io/galactic-asteroid-miner)

<br>

I've been tinkering inside the unity engine for 6+ years, and to show that I am capable of taking a concept from idea to a finished, realised product, I stuck to one idea and published it.

<br>

## Technologies

- Unity3D
- C#
- Gimp
- bfxr

<br>

![White Dwarf](https://img.itch.zone/aW1hZ2UvMTc0MDIyMS8xMDI1MjYwNS5qcGc=/original/z%2FaKXM.jpg)

<br>

Every line of code is my own work. my own implementations and solutions to problems I encounterd. There were a number of 
hurdles I had to overcome, mostly involving the physics system and gravity simulations with large numbers of objects. 

<br>

![Code snippet](https://imgur.com/22I8PbG.jpg)

<br>

The following are some of the systems implemented to realise this project:

- Infinite procedural generation: custom generation algorithm not based on pseudo-random generators such as `perlin noise`
- Chunking System
- Occlusion Culling
- Spatial hashing: implemented using a dictionary, could be converted to hashmap
- Realistic 2D Newtonian physics