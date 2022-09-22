using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class OcclusionManager2
  {
    private ChunkManager2 chunkManager;
    private PrefabInstantiator prefabGetter = new PrefabInstantiator();
    private Transform playerTransform;

    private const int starOcclusionRadius = 200;
    private const int asteroidOcclusionRadius = 50;

    private Dictionary<Vector2, Asteroid2> activeAsteroids = 
      new Dictionary<Vector2, Asteroid2>();

    private Dictionary<Vector2, Asteroid2> starAsteroids =
      new Dictionary<Vector2, Asteroid2>();

    private Dictionary<Vector2, Asteroid2> inactiveAsteroids =
      new Dictionary<Vector2, Asteroid2>();

    private Dictionary<Vector2, Star> activeStars = 
      new Dictionary<Vector2, Star>();

    private Dictionary<Vector2, Star> inactiveStars =
      new Dictionary<Vector2, Star>();

    //------------------------------------------------------------------------------

    public OcclusionManager2(Transform player, ChunkManager2 _chunkManager) {
      chunkManager = _chunkManager;
      playerTransform = player;
    }

    public void UpdateOcclusion()
    {
      PopulateDictionaries();

      Debug.Log("Active Asteroids: " + activeAsteroids.Count);
      Debug.Log("Active Stars: " + activeStars.Count);

      CheckOcclusion();

      DisposeInactiveObjects();
    }

    private void PopulateDictionaries()
    {
      foreach (var chunk in chunkManager.LazyChunks)
      {
        var lazyChunk = chunk.Value;

        GetStars(lazyChunk);
      }

      foreach (var chunk in chunkManager.ActiveChunks)
      {
        var activeChunk = chunk.Value;

        GetStars(activeChunk);
        GetAsteroids(activeChunk);
      }
    }

    private void CheckOcclusion()
    {
      foreach (var star in activeStars)
      {
        var starInfo = star.Value;
        var distance = FastDistance(starInfo.Position, playerTransform.position);

        if (distance < starOcclusionRadius) 
        {
          if (starInfo.AttachedObject == null){
            starInfo.SetStarObject(prefabGetter.GetStar(starInfo));
          }
          return;
        }

        inactiveStars.Add(starInfo.Position, starInfo);
      }

      foreach (var starAsteroidInfo in starAsteroids)
      {
        var starAsteroid = starAsteroidInfo.Value;
        var distance = FastDistance(starAsteroid.Position, playerTransform.position);

        if (distance < asteroidOcclusionRadius) 
        {
          if (starAsteroid.RendererStatus == false) {
            starAsteroid.IsRendered(true);
          }
          return;
        }

        inactiveAsteroids.Add(starAsteroid.Position, starAsteroid);
      }

      foreach (var activeAsteroidInfo in activeAsteroids)
      {
        var activeAsteroid = activeAsteroidInfo.Value;
        var distance = FastDistance(activeAsteroid.Position, playerTransform.position);

        if (distance < asteroidOcclusionRadius)
        {
          if (activeAsteroid.AttachedObject == null) {
            activeAsteroid.SetAsteroidObject(
              prefabGetter.GetAsteroid(activeAsteroid.Size),
              activeAsteroid.SpawnPoint
            );
          }
          return;
        }

        inactiveAsteroids.Add(activeAsteroid.Position, activeAsteroid);
      }
    }

    private void DisposeInactiveObjects()
    {
      foreach (var inactiveStar in inactiveStars)
      {
        var starInfo = inactiveStar.Value;
        starInfo.DisposeObject();
        activeStars.Remove(starInfo.Position);
      }

      foreach (var inactiveAsteroid in inactiveAsteroids)
      {
        var asteroidInfo = inactiveAsteroid.Value;
        asteroidInfo.DisposeObject();
        activeAsteroids.Remove(asteroidInfo.Position);
      }
    }

    //------------------------------------------------------------------------------

    private void GetStars(Chunk2 _chunkInfo)
    {
      if (!_chunkInfo.HasStar) return;
      if (activeStars.ContainsKey(_chunkInfo.ChunkStar.Position)) return;
      
      activeStars.Add(_chunkInfo.ChunkStar.Position, _chunkInfo.ChunkStar);

      GetStarAsteroids(_chunkInfo);
    }

    private void GetStarAsteroids(Chunk2 _chunkInfo)
    {
      foreach(var asteroid in _chunkInfo.Asteroids)
      {
        var asteroidInfo = asteroid.Value;

        if (starAsteroids.ContainsKey(asteroidInfo.SpawnPoint)) return;

        asteroidInfo.SetAsteroidObject(
          prefabGetter.GetAsteroid(asteroidInfo.Size),
          asteroidInfo.SpawnPoint
        );

        asteroidInfo.IsRendered(false);

        starAsteroids.Add(asteroidInfo.SpawnPoint, asteroidInfo);
      }
    }

    private void GetAsteroids(Chunk2 _chunkInfo)
    {
      foreach (var asteroid in _chunkInfo.Asteroids)
      {
        if (activeAsteroids.ContainsKey(asteroid.Value.SpawnPoint)) return;
        activeAsteroids.Add(asteroid.Value.SpawnPoint, asteroid.Value);
      }
    }

    private float FastDistance(Vector2 _point1, Vector2 _point2)
    {
      var x = _point1.x - _point2.x;
      var y = _point1.y - _point2.y;

      return x * x + y * y;
    }
  }
}