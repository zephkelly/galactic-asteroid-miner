using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengerIdleState : IState
{
  private ScavengerController scavenger;

  public ScavengerIdleState(ScavengerController scavenger)
  {
    this.scavenger = scavenger;
  }

  public void Enter()
  {
    Debug.Log("Scavenger is idle!");
  }

  public void Execute()
  {

  }

  public void Exit()
  {

  }
}