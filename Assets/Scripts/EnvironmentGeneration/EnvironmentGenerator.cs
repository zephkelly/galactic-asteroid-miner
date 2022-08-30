using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class EnvironmentGenerator : MonoBehaviour
  {
    [SerializeField] Chunk chunkBehaviour;

    [SerializeField] GameObject starPrefab;
    [SerializeField] GameObject asteroidPrefab;

    private Transform playerTransform;
    private Vector2 playerPosition;

    private void Awake()
    {
      playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
      GenerateChunk(chunkBehaviour);
    }

    private void Update()
    {
      playerPosition = playerTransform.position;
    }

    private void GenerateChunk(Chunk _chunk)
    {
      


      chunkBehaviour.GenerateChunk(chunkPosition);
    }
  }
}