using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengerChaseState : IState
{
  private ScavengerController scavenger;
  private Transform scavengerTransform;
  private Transform playerTransform;

  private float chaseRadius = 60f;
  private LayerMask whichLayers;

  public ScavengerChaseState(ScavengerController scavenger, Transform playerTransform)
  {
    this.scavenger = scavenger;

    scavengerTransform = scavenger.transform;
    this.playerTransform = playerTransform;
  }

  public void Enter()
  {
    whichLayers = LayerMask.GetMask("Player", "Asteroid");

    Debug.Log("Scavenger is chasing the player!");
  }

  Vector2 lastKnownPlayerPosition;
  public void Execute()
  {
    if (playerTransform == null)
    {
      scavenger.ChangeState(new ScavengerIdleState(scavenger));
      return;
    }

    RaycastToPlayer();
    RaycastRadially();

    ShouldReturnToIdleState();
  }

  private void RaycastToPlayer() 
  {
    RaycastHit2D hit = Physics2D.Raycast(scavengerTransform.position, playerTransform.position - scavengerTransform.position, chaseRadius, whichLayers);

    if (hit.collider == null) {
      return;
    }

    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
    {
      Debug.DrawRay(scavengerTransform.position, playerTransform.position - scavengerTransform.position, Color.green);
      lastKnownPlayerPosition = hit.point;
    }
    else
    {
      Debug.DrawRay(scavengerTransform.position, playerTransform.position - scavengerTransform.position, Color.red);
      //draw a circle at the last known position of the player
      Debug.DrawRay(lastKnownPlayerPosition, Vector2.one, Color.blue);
    }
  }

  private float collisionCheckRadius = 5f;
  private void RaycastRadially()
  {
    //fire 8 raycasts in each direction around scavengar
    for (int i = 0; i < 12; i++)
    {
      float angle = i * Mathf.PI / 6;
      Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

      RaycastHit2D hit = Physics2D.Raycast(scavengerTransform.position, direction, collisionCheckRadius, whichLayers);

      if (hit.collider == null)
      {
        Debug.DrawRay(scavengerTransform.position, direction * collisionCheckRadius, Color.green);
      }
      else 
      {
        Debug.DrawRay(scavengerTransform.position, direction * collisionCheckRadius, Color.red);
      }
    }
  }

  private float returnToIdleTimer = 0f;
  private void ShouldReturnToIdleState()
  {
    returnToIdleTimer += Time.deltaTime;

    if (Vector2.Distance(scavengerTransform.position, playerTransform.position) > chaseRadius && returnToIdleTimer > 5f)
    {
      scavenger.ChangeState(new ScavengerIdleState(scavenger));
    }
  }

  public void Exit()
  {

  }
}