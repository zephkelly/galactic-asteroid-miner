using System.Collections;
using System.Collections.Generic;
using TMPro;
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

  private int numberOfRays = 28;
  private float chaseRadius = 100f;
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

    scavengerRigid2D.AddForce(lerpVector * scavenger.scavengerSpeed, ForceMode2D.Force);
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
      Debug.DrawRay(lastKnownPlayerPosition, Vector2.one * 0.5f, Color.green);
    }
  }

  private void RaycastRadially()
  {
    scavenger.positiveAngles = new Vector3[numberOfRays];

    for (int i = 0; i < numberOfRays; i++)
    {
      float angle = i * 2 * Mathf.PI / numberOfRays;
      Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

      float angleBetween = Vector2.Angle(direction, lastKnownPlayerPosition - (Vector2)scavengerTransform.position);
      float weight = 1f - (angleBetween / 180f);

      RaycastHit2D hit = Physics2D.Raycast(scavengerTransform.position, direction, collisionCheckRadius * (1 + Mathf.InverseLerp(0, 10, scavengerRigid2D.velocity.magnitude)) * weight, whichLayers);
      scavenger.positiveAngles[i] = direction;

      if (hit.collider == null)
      {
        scavenger.positiveAngles[i].z = weight;
      }
      else 
      {
        float normalizedDistance = 1f - (hit.distance / collisionCheckRadius);
        scavenger.positiveAngles[i].z = -normalizedDistance; // This will be between 0 (far) and -1 (close)
      }
    }

    for (int i = 0; i < numberOfRays; i++)
    {
      if (scavenger.positiveAngles[i].z <= 0)
      {
        //on each index next to current index, disinhibit
        scavenger.positiveAngles[(i + 1 + numberOfRays) % numberOfRays].z = (scavenger.positiveAngles[(i + 1 + numberOfRays) % numberOfRays].z - 0.8f) / 2;
        scavenger.positiveAngles[(i - 1 + numberOfRays) % numberOfRays].z = (scavenger.positiveAngles[(i - 1 + numberOfRays) % numberOfRays].z - 0.8f) / 2;
        scavenger.positiveAngles[(i + 1 + numberOfRays) % numberOfRays].z = (scavenger.positiveAngles[(i + 1 + numberOfRays) % numberOfRays].z - 0.2f) / 2;
        scavenger.positiveAngles[(i - 1 + numberOfRays) % numberOfRays].z = (scavenger.positiveAngles[(i - 1 + numberOfRays) % numberOfRays].z - 0.2f) / 2;
      }
    }

    for (int i = 0; i < numberOfRays; i++)
    {
      if (scavenger.positiveAngles[i].z > 0.7)
      {
        Debug.DrawRay(scavengerTransform.position, (Vector2)scavenger.positiveAngles[i] * collisionCheckRadius * (1 + Mathf.InverseLerp(0, 10, scavengerRigid2D.velocity.magnitude)) * scavenger.positiveAngles[i].z, Color.green);
      }
      else 
      {
        Debug.DrawRay(scavengerTransform.position, (Vector2)scavenger.positiveAngles[i] * collisionCheckRadius * (1 + Mathf.InverseLerp(0, 10, scavengerRigid2D.velocity.magnitude)) * scavenger.positiveAngles[i].z, Color.red);
      }
    }
   
    for (int i = 0; i < numberOfRays; i++)
    {
      weightedDirection += (Vector2)scavenger.positiveAngles[i] * scavenger.positiveAngles[i].z;
    }

    weightedDirection.Normalize();
    Debug.DrawLine(scavengerTransform.position, scavengerTransform.position + (Vector3)weightedDirection * 10f, Color.yellow);
  }

  private void ShouldReturnToIdleState()
  {
    if (Vector2.Distance(scavengerTransform.position, playerTransform.position) > chaseRadius)
    {
      returnToIdleTimer += Time.deltaTime;

      if (returnToIdleTimer > 20f)
      {
        returnToIdleTimer = 0f;
        scavenger.ChangeState(new ScavengerIdleState(scavenger));
      }
    }
  }

  public void Exit()
  {
    scavenger.scavengerThrusterParticle.Stop();
    scavenger.scavengerThrusterLight.enabled = false;
  }
}