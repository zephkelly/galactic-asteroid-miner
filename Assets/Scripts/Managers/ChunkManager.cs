using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly
{
  public class ChunkManager : MonoBehaviour
  {
    //The chunk size
    [SerializeField] int chunkDiameter = 500;

    //Give our chunks original names!
    private int _chunkNamer;

    //Dictonary for deactivated chunks
    Dictionary<Vector2, GameObject> deactivatedChunks = new Dictionary<Vector2, GameObject>();

    //Dictonary for activated chunks
    Dictionary<Vector2, GameObject> activeChunks = new Dictionary<Vector2, GameObject>();

    //----------------------------------------------------------------------------------------------

    private Transform playerTransform;

    private Vector2 playerCurrentChunk;

    private Vector2 playerLastChunk;

    private void Start()
    {
      //Get the player transform
      playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
      //Get the player position rounded to the nearest chunk
      GenerateChunk(playerLastChunk = QuantisePosition(playerTransform.position));
    }

    private void Update()
    {
      //Gets the players chunk through a quantised position
      playerCurrentChunk = QuantisePosition(playerTransform.position);

      //If we are in a new chunk, make a new chunk
      if (playerCurrentChunk != playerLastChunk)
      {
        playerLastChunk = playerCurrentChunk;
        GenerateChunk(playerCurrentChunk);
      }
    }

    //Function to quantise the position to the nearest chunk
    private Vector2 QuantisePosition(Vector2 position)
    {
      //Get the player position
      Vector2 playerPosition = playerTransform.position;

      return new Vector2(
        Mathf.Round(position.x / chunkDiameter),
        Mathf.Round(position.y / chunkDiameter)
      );
    }

    private void GenerateChunk(Vector2 key)
    {  
      if (activeChunks.ContainsKey(key)) return;

      if (deactivatedChunks.ContainsKey(key))
      {
        //Get the chunk from the deactivated chunk dictionary
        GameObject reactivatedChunk = deactivatedChunks[key];
        //Activate the chunk
        reactivatedChunk.SetActive(true);
        //Add it to the active chunks dictionary
        activeChunks.Add(key, reactivatedChunk);
        //Remove it from the deactivated chunks dictionary
        deactivatedChunks.Remove(key);
      }
      else
      {
        Debug.Log("Generating new chunk: " + key);
        //Create a new chunk
        GameObject newChunk = new GameObject("Chunk " + _chunkNamer);
        //Add it to the active chunks dictionary
        activeChunks.Add(key, newChunk);
        //Increase the chunk namer
        _chunkNamer++;
      }
    }
  }
}