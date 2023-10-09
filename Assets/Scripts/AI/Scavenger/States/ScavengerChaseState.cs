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

  private int numberOfRays = 24;
  private float chaseRadius = 60f;
  private float collisionCheckRadius = 8f;
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
    scavenger.scavengerThrusterParticle.Play();
    scavenger.scavengerThrusterLight.enabled = true;

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
    Vector2 lerpVector = Vector2.Lerp(scavengerTransform.up, weightedDirection, 0.7f);
    Vector2 visualLerpVector = Vector2.Lerp(scavengerTransform.up, weightedDirection, 0.15f);

    scavengerRigid2D.AddForce(lerpVector * 100f, ForceMode2D.Force);
    scavengerTransform.up = visualLerpVector;
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
      Debug.DrawRay(lastKnownPlayerPosition, Vector2.one, Color.blue);
    }
  }

  private void RaycastRadially()
  {
    scavenger.positiveAngles = new Vector3[numberOfRays];
    scavenger.negativeAngles = new Vector3[numberOfRays];

    for (int i = 0; i < numberOfRays; i++)
    {
      float angle = i * 2 * Mathf.PI / numberOfRays;
      Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
      RaycastHit2D hit = Physics2D.Raycast(scavengerTransform.position, direction, collisionCheckRadius * (1 + Mathf.InverseLerp(0, 10, scavengerRigid2D.velocity.magnitude)), whichLayers);

      Debug.Log(1 + Mathf.InverseLerp(0, 10, scavengerRigid2D.velocity.magnitude));

      if (hit.collider == null)
      {
        //find the angle between the direction, and the lastknowPlayerPosition
        float angleBetween = Vector2.Angle(direction, lastKnownPlayerPosition - (Vector2)scavengerTransform.position);
        float weight = 1f - (angleBetween / 180f);
        scavenger.positiveAngles[i] = direction;
        scavenger.positiveAngles[i].z = weight;

        //the lower the weight, the shorter the raycast
        Debug.DrawRay(scavengerTransform.position, direction * collisionCheckRadius * (1 + Mathf.InverseLerp(0, 10, scavengerRigid2D.velocity.magnitude)) * weight , Color.green);
      }
      else 
      {
        scavenger.negativeAngles[i] = direction;

        float angleBetween = Vector2.Angle(direction, hit.centroid - (Vector2)scavengerTransform.position);
        float weight = 1f - (angleBetween / 180f);

        scavenger.negativeAngles[i].z = weight;
        scavenger.negativeAngles[(i + 1 + numberOfRays) % numberOfRays].z = (scavenger.negativeAngles[(i + 1 + numberOfRays) % numberOfRays].z + (weight * 0.3f)) / 2;
        scavenger.negativeAngles[(i + 1 + numberOfRays) % numberOfRays].z = (scavenger.negativeAngles[(i + 1 + numberOfRays) % numberOfRays].z + (weight * 0.3f)) / 2;
        scavenger.negativeAngles[(i + 2 + numberOfRays) % numberOfRays].z = (scavenger.negativeAngles[(i + 2 + numberOfRays) % numberOfRays].z + (weight * 0.15f)) / 2;
        scavenger.negativeAngles[(i + 2 + numberOfRays) % numberOfRays].z = (scavenger.negativeAngles[(i + 2 + numberOfRays) % numberOfRays].z + (weight * 0.15f)) / 2;
        
        Debug.DrawRay(scavengerTransform.position, direction * collisionCheckRadius * 0.1f, Color.red);
      }
    }

    float totalWeight = 0f;
    for (int i = 0; i < numberOfRays; i++)
    {
      totalWeight += scavenger.positiveAngles[i].z;
    }

    totalWeight = totalWeight / numberOfRays;
    for (int i = 0; i < numberOfRays; i++)
    {
      weightedDirection += (Vector2)scavenger.positiveAngles[i] * (scavenger.positiveAngles[i].z);
      weightedDirection -= (Vector2)scavenger.negativeAngles[i] * (scavenger.negativeAngles[i].z);
    }

    weightedDirection.Normalize();
    Debug.DrawLine(scavengerTransform.position, scavengerTransform.position + (Vector3)weightedDirection * 10f, Color.yellow);
  }

  private void ShouldReturnToIdleState()
  {
    returnToIdleTimer += Time.deltaTime;

    if (Vector2.Distance(scavengerTransform.position, playerTransform.position) > chaseRadius && returnToIdleTimer > 20f)
    {
      scavenger.ChangeState(new ScavengerIdleState(scavenger));
    }
  }

  public void Exit()
  {
    scavenger.scavengerThrusterParticle.Stop();
    scavenger.scavengerThrusterLight.enabled = false;
  }
}