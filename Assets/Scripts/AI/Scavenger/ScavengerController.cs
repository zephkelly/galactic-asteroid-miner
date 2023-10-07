using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengerController : MonoBehaviour
{
  private StateMachine stateMachine;
  private GameObject playerObject;
  private Transform playerTransform;

  public void ChangeState(IState newState)
  {
    stateMachine.ChangeState(newState);
  }

  private void Start()
  {
    stateMachine = new StateMachine();

    ChangeState(new ScavengerIdleState(this));
  }

  private void Update()
  {
    stateMachine.Update();
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if(other.CompareTag("Player") && stateMachine.currentState.GetType() != typeof(ScavengerChaseState))
    {
      playerObject = other.gameObject;
      playerTransform = other.transform;
      stateMachine.ChangeState(new ScavengerChaseState(this, playerTransform));
    }
  }
}
