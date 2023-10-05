# Galactic Asteroid Miner v1.0
![GAM Hero](https://img.itch.zone/aW1hZ2UvMTc0MDIyMS8xMDI1MjYwNi5qcGc=/original/VVM%2FqJ.jpg)

2D Infinite procedurally generated asteroid mining rouge-like. Created as a small proof of concept; based on the game [Motherload](http://www.xgenstudios.com/play/motherload).

<br>

Play the game for yourself on [Itch.io](https://zephyrmg.itch.io/galactic-asteroid-miner)

<br>

I've been tinkering inside the unity engine for 6+ years and wanted to show that I am capable of taking a concept from idea to a finished product.

<br>

## Technologies

- Unity3D
- C#
- Gimp
- bfxr

<br>

![White Dwarf](https://img.itch.zone/aW1hZ2UvMTc0MDIyMS8xMDI1MjYwNS5qcGc=/original/z%2FaKXM.jpg)

<br>

## Game systems

The following are some of the systems implemented to realise this project:

- Infinite procedural generation: custom generation algorithm without pseudo-random generators such as `perlin noise`
- Spatial hashing: implemented using a dictionary, could be converted to hashmap
- Chunking System
- Occlusion Culling
- Newtonian-like 2D physics

<br>

![Code snippet](https://imgur.com/22I8PbG.jpg)

<br>

## In-Development
### Multiplayer

Multiplayer is front and center on the next update to the game. This involves a total rework of the game systems from the ground up. By using the `Mirror` library, the game makes RPC calls between client / server / and host versions of the client. All information is serialised through a KCP transport.

![Multiplayer mirror](https://imgur.com/JQPTlFD.jpg)
