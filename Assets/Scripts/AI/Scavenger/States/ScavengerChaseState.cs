using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengerChaseState : IState
{
  private ScavengerController scavenger;
  private Transform scavengerTransform;
  private Transform playerTransform;
  private LayerMask whichLayers;
  private Vector2 lastKnownPlayerPosition;
  private float chaseRadius = 60f;
  private float collisionCheckRadius = 5f;
  private float returnToIdleTimer = 0f;

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

  private bool positionSet = false;
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
      positionSet = false;
    }
    else
    {
      Debug.DrawRay(scavengerTransform.position, playerTransform.position - scavengerTransform.position, Color.red);
      Debug.DrawRay(lastKnownPlayerPosition, Vector2.one, Color.blue);

      Vector2 reflectedDirection = Vector2.Reflect(playerTransform.position - scavengerTransform.position, hit.normal);

      if (positionSet == true) return;
      lastKnownPlayerPosition = hit.point + (reflectedDirection.normalized * 2f);
      positionSet = true;
    }
  }

  private void RaycastRadially()
  {
    scavenger.positiveAngles = new Vector3[12];
    scavenger.negativeAngles = new Vector3[12];

    for (int i = 0; i < 12; i++)
    {
      float angle = i * Mathf.PI / 6;
      Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
      RaycastHit2D hit = Physics2D.Raycast(scavengerTransform.position, direction, collisionCheckRadius, whichLayers);

      if (hit.collider == null)
      {
        //find the angle between the direction, and the lastknowPlayerPosition
        float angleBetween = Vector2.Angle(direction, lastKnownPlayerPosition - (Vector2)scavengerTransform.position);
        float weight = 1f - (angleBetween / 180f);
        scavenger.positiveAngles[i] = direction;
        scavenger.positiveAngles[i].z = weight;

        Debug.DrawRay(scavengerTransform.position, direction * collisionCheckRadius, Color.green);
      }
      else 
      {
        scavenger.negativeAngles[i] = direction;
        scavenger.negativeAngles[i].z = 1f;

        Debug.DrawRay(scavengerTransform.position, direction * collisionCheckRadius, Color.red);
      }
    }

    float totalWeight = 0f;
    for (int i = 0; i < 12; i++)
    {
      totalWeight += scavenger.positiveAngles[i].z;
    }

    totalWeight = totalWeight / 12;
    Vector2 weightedDirection = Vector2.zero;
    for (int i = 0; i < 12; i++)
    {
      weightedDirection += (Vector2)scavenger.positiveAngles[i] * (scavenger.positiveAngles[i].z / totalWeight);
    }

    Debug.Log(weightedDirection);
    scavengerTransform.up = weightedDirection;
    scavengerTransform.position = Vector2.MoveTowards(scavengerTransform.position, lastKnownPlayerPosition, 5f * Time.deltaTime);
  }

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