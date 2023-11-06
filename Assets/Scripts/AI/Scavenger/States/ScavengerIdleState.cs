using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zephkelly.AI.Scavenger
{
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

    public void FixedUpdate()
    {

    }

    public void Exit()
    {

    }
  }
}