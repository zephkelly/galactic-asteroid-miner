using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly.AI.Scavenger
{
  public class ScavengerChaseState : IState
  {
    private ScavengerController scavenger;
    private Transform scavengerTransform;
    private Rigidbody2D scavengerRigid2D;
    private Transform playerTransform;
    private LayerMask whichLayers;

    private Vector3[] positiveAngles = new Vector3[16];
    private Vector2 lastKnownPlayerPosition = Vector2.zero;
    private Vector2 weightedDirection = Vector2.zero;
    private bool setBiasDirection = false;

    private Vector2 lerpVector = Vector2.zero;
    private Vector2 visualLerpVector = Vector2.zero;

    private int numberOfRays = 40;
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
      scavenger.ThrusterParticleSystem.Play();
      scavenger.ThrusterLight.enabled = true;

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
      // CheckWeightedDirection();

      lerpVector = Vector2.Lerp(scavengerTransform.up, weightedDirection, 0.7f);
      visualLerpVector = Vector2.Lerp(scavengerTransform.up, weightedDirection, 0.15f);

      CheckForNewStates();
    }

    public void FixedUpdate()
    {
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

        setBiasDirection = false;
      }
      else
      {
        Debug.DrawRay(scavengerTransform.position, playerTransform.position - scavengerTransform.position, Color.red);

        if (!setBiasDirection)
        {
          Vector2 obstacleDirection = hit.centroid - (Vector2)scavengerTransform.position;
          float lateralVelocity = Vector2.Dot(scavengerRigid2D.velocity, Vector2.Perpendicular(obstacleDirection).normalized);

          Vector2 biasDirection;
          if (lateralVelocity > 0) {
              biasDirection = Vector2.Perpendicular(obstacleDirection).normalized;
          } else {
              biasDirection = -Vector2.Perpendicular(obstacleDirection).normalized;
          }
          lastKnownPlayerPosition = hit.point + (biasDirection * 8f);

          setBiasDirection = true;
        }

        Debug.DrawRay(lastKnownPlayerPosition, Vector2.one * 0.5f, Color.blue);
      }
    }

    private void RaycastRadially()
    {
      positiveAngles = new Vector3[numberOfRays];
      Vector2 direction = Vector2.zero;
      Vector2 rayStartPosition = Vector2.zero;

      for (int i = 0; i < numberOfRays; i++)
      {
        float angle = i * 2 * Mathf.PI / numberOfRays;
        direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        direction.Normalize();

        // rayStartPosition = (Vector2)scavengerTransform.position + (direction * 2);

        float angleBetween = Vector2.Angle(direction, lastKnownPlayerPosition - (Vector2)scavengerTransform.position);
        float weight = Mathf.Pow(1f - (angleBetween / 180f), 2f);

        RaycastHit2D hit = Physics2D.Raycast((Vector2)scavengerTransform.position, direction, collisionCheckRadius * (1 + Mathf.InverseLerp(0, 40, scavengerRigid2D.velocity.magnitude)) * weight, whichLayers);
        positiveAngles[i] = direction;

        if (hit.collider == null)
        {
          positiveAngles[i].z = weight;
        }
        else 
        {
          float normalizedDistance = 1f - (hit.distance / collisionCheckRadius);
          positiveAngles[i].z = -normalizedDistance; // This will be between 0 (far) and -1 (close)
        }
      }

      //calulate the weighted direction
      for (int i = 0; i < numberOfRays; i++)
      {
        if (positiveAngles[i].z <= 0)
        {
          //on each index next to current index, disinhibit
          positiveAngles[(i + 1 + numberOfRays) % numberOfRays].z = (positiveAngles[(i + 1 + numberOfRays) % numberOfRays].z - 0.8f) / 2;
          positiveAngles[(i - 1 + numberOfRays) % numberOfRays].z = (positiveAngles[(i - 1 + numberOfRays) % numberOfRays].z - 0.8f) / 2;
        }
      }

      for (int i = 0; i < numberOfRays; i++)
      {
        if (positiveAngles[i].z > 0.6)
        {
          Debug.DrawRay((Vector2)scavengerTransform.position, (Vector2)positiveAngles[i] * collisionCheckRadius * (1 + Mathf.InverseLerp(0, 10, scavengerRigid2D.velocity.magnitude)) * positiveAngles[i].z, Color.green);
        }
        else 
        {
          Debug.DrawRay((Vector2)scavengerTransform.position, (Vector2)positiveAngles[i] * collisionCheckRadius * (1 + Mathf.InverseLerp(0, 10, scavengerRigid2D.velocity.magnitude)) * positiveAngles[i].z, Color.red);
        }

        weightedDirection += (Vector2)positiveAngles[i] * positiveAngles[i].z;
      }

      weightedDirection.Normalize();
      Debug.DrawLine(scavengerTransform.position, scavengerTransform.position + (Vector3)weightedDirection * 10f, Color.yellow);
    }

    private void CheckWeightedDirection()
    {
      RaycastHit2D hit = Physics2D.Raycast(scavengerTransform.position, weightedDirection, 5f, whichLayers);

      Vector2 obstacleDirection = hit.centroid - (Vector2)scavengerTransform.position;
      float lateralVelocity = Vector2.Dot(scavengerRigid2D.velocity, Vector2.Perpendicular(obstacleDirection).normalized);

      Vector2 biasDirection;
      float biasMagnitude = 0.9f; // You might want to adjust this based on testing

      if (lateralVelocity > 0) {
          // Ship is moving more towards the right of its current trajectory.
          // Steer it further to the right.
          biasDirection = Vector2.Perpendicular(obstacleDirection).normalized;
      } else {
          // Ship is moving more towards the left or straight.
          // Steer it to the left.
          biasDirection = -Vector2.Perpendicular(obstacleDirection).normalized;
      }

      weightedDirection += biasDirection * biasMagnitude;
      weightedDirection.Normalize();
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

    private void CheckForNewStates()
    {
      if (Vector2.Distance(scavengerTransform.position, playerTransform.position) < 20f)
      {
        scavenger.ChangeState(new ScavengerAttackState(scavenger, playerTransform, scavengerRigid2D));
      }
    }

    public void Exit()
    {
      scavenger.ThrusterParticleSystem.Stop();
      scavenger.ThrusterLight.enabled = false;
    }
  }
}