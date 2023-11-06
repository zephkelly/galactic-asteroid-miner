using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly.AI.Scavenger
{
  public class ScavengerAttackState : IState
  {
    private ScavengerController scavenger;
    private Transform scavengerTransform;
    private Rigidbody2D scavengerRigid2D;

    private Transform playerTransform;
    private Rigidbody2D playerRigid2D;

    private float timeSinceLastShot = 0f;
    private float randomDistance = 30f;

    public ScavengerAttackState(ScavengerController scavenger, Transform playerTransform, Rigidbody2D scavengerRigid2D)
    {
      this.scavenger = scavenger;
      scavengerTransform = scavenger.transform;
      this.scavengerRigid2D = scavengerRigid2D;

      playerRigid2D = playerTransform.GetComponent<Rigidbody2D>();
      this.playerTransform = playerTransform;
    }

    public void Enter()
    {
      scavenger.ThrusterParticleSystem.Play();
      scavenger.ThrusterLight.enabled = true;

      Debug.Log("Scavenger is chasing the player!");
    }

    public void Execute()
    {
      ShootAtPlayerPosition();

      //distance between player and scavenger
      float distance = Vector2.Distance(scavengerTransform.position, playerTransform.position);

      if (distance > randomDistance)
      {
        scavenger.ChangeState(new ScavengerChaseState(scavenger, playerTransform, scavengerRigid2D));
        return;
      }
    }

    private void ShootAtPlayerPosition()
    {
      //grab the players position and velocity and try to predict the players position
      Vector2 velocity = playerRigid2D.velocity;
      Vector2 playerPosition = playerTransform.position;

      // Predict further ahead, e.g., 2 seconds. Adjust as necessary.
      Vector2 predictedPosition = playerPosition + (velocity * 0.4f);

      // Calculate the direction from the scavenger to the predicted position.
      Vector2 directionToPredictedPosition = predictedPosition - (Vector2)scavengerTransform.position;
      
      // Calculate the angle towards the predicted position.
      float angle = Mathf.Atan2(directionToPredictedPosition.y, directionToPredictedPosition.x) * Mathf.Rad2Deg - 90; // Subtracting 90 because of Unity's orientation for 2D.
      
      // Set the rotation of the scavenger.
      scavengerTransform.rotation = Quaternion.Euler(0, 0, angle);

      // Debug line.
      Debug.DrawLine(scavengerTransform.position, predictedPosition, Color.red);

      timeSinceLastShot += Time.deltaTime;

      if (timeSinceLastShot >= 0.40f)
      {
        timeSinceLastShot = 0f;
        scavenger.LaserParticleSystem.Play();

        randomDistance = Random.Range(27f, 32f);
      }
    }

    public void FixedUpdate()
    {

    }

    public void Exit()
    {

    }
  }
}