using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengerChaseState : IState
{
  private ScavengerController scavenger;
  private Transform scavengerTransform;
  private Rigidbody2D scavengerRigid2D;
  private Transform playerTransform;
  private LayerMask whichLayers;

  private Vector2 lastKnownPlayerPosition = Vector2.zero;
  private Vector2 weightedDirection = Vector2.zero;

  private float chaseRadius = 60f;
  private float collisionCheckRadius = 6.5f;
  private float returnToIdleTimer = 0f;

  public ScavengerChaseState(ScavengerController scavenger, Transform playerTransform, Rigidbody2D scavengerRigid2D)
  {
    this.scavenger = scavenger;

    scavengerTransform = scavenger.transform;
    this.playerTransform = playerTransform;
    this.scavengerRigid2D = scavengerRigid2D;
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

  public void FixedUpdate()
  {
    Vector2 lerpVector = Vector2.Lerp(scavengerTransform.up, weightedDirection, 0.5f);
    Vector2 visualLerpVector = Vector2.Lerp(scavengerTransform.up, weightedDirection, 0.3f);

    scavengerRigid2D.AddForce(lerpVector * 100f, ForceMode2D.Force);
    scavengerTransform.up = visualLerpVector;
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
    }
    else
    {
      Debug.DrawRay(scavengerTransform.position, playerTransform.position - scavengerTransform.position, Color.red);
      Debug.DrawRay(lastKnownPlayerPosition, Vector2.one, Color.blue);
    }
  }

  private void RaycastRadially()
  {
    scavenger.positiveAngles = new Vector3[16];
    scavenger.negativeAngles = new Vector3[16];

    for (int i = 0; i < 16; i++)
    {
      float angle = i * Mathf.PI / 8;
      Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
      RaycastHit2D hit = Physics2D.Raycast(scavengerTransform.position, direction, collisionCheckRadius, whichLayers);

      if (hit.collider == null)
      {
        //find the angle between the direction, and the lastknowPlayerPosition
        float angleBetween = Vector2.Angle(direction, lastKnownPlayerPosition - (Vector2)scavengerTransform.position);
        float weight = 1f - (angleBetween / 180f);
        scavenger.positiveAngles[i] = direction;
        scavenger.positiveAngles[i].z = weight;

        //the lower the weight, the shorter the raycast
        Debug.DrawRay(scavengerTransform.position, direction * collisionCheckRadius * weight, Color.green);
      }
      else 
      {
        scavenger.negativeAngles[i] = direction;

        float angleBetween = Vector2.Angle(direction, hit.centroid - (Vector2)scavengerTransform.position);
        float weight = 1f - (angleBetween / 180f);

        scavenger.negativeAngles[i].z = weight;
        scavenger.negativeAngles[Mathf.Clamp(i+1, 0, 11)].z = (scavenger.negativeAngles[Mathf.Clamp(i+1, 0, 11)].z + (weight * 0.8f)) / 2;
        scavenger.negativeAngles[Mathf.Clamp(i-1, 0, 11)].z = (scavenger.negativeAngles[Mathf.Clamp(i+1, 0, 11)].z + (weight * 0.8f)) / 2;
        scavenger.negativeAngles[Mathf.Clamp(i+2, 0, 11)].z = (scavenger.negativeAngles[Mathf.Clamp(i+1, 0, 11)].z + (weight * 0.6f)) / 2;
        scavenger.negativeAngles[Mathf.Clamp(i-2, 0, 11)].z = (scavenger.negativeAngles[Mathf.Clamp(i+1, 0, 11)].z + (weight * 0.6f)) / 2;
        
        Debug.DrawRay(scavengerTransform.position, direction * collisionCheckRadius * weight, Color.red);
      }
    }

    float totalWeight = 0f;
    for (int i = 0; i < 16; i++)
    {
      totalWeight += scavenger.positiveAngles[i].z;
    }

    totalWeight = totalWeight / 16;
    for (int i = 0; i < 16; i++)
    {
      weightedDirection += (Vector2)scavenger.positiveAngles[i] * (scavenger.positiveAngles[i].z / totalWeight);
      weightedDirection -= (Vector2)scavenger.negativeAngles[i] * (scavenger.negativeAngles[i].z / totalWeight);
    }

    weightedDirection.Normalize();
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