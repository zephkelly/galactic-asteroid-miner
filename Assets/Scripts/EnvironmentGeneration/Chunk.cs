using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class Chunk
  {
    public static Vector2 chunkSize = new Vector2(50, 50);

    public Vector2 chunkPosition;

    public int numberOfStars = 3;
    public int numberOfAsteroids = 40;

    public Chunk(Vector2 _chunkPosition, int _numberOfStars, int _numberOfAsteroids)
    {
      this.chunkPosition = _chunkPosition;
      this.numberOfStars = _numberOfStars;
      this.numberOfAsteroids = _numberOfAsteroids;
    } 
  }
}