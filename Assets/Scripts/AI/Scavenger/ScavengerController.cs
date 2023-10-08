using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengerController : MonoBehaviour
{
  private StateMachine stateMachine;
  private Rigidbody2D scavengerRigid2D;
  private GameObject playerObject;
  private Transform playerTransform;

  [SerializeField] public Vector3[] positiveAngles = new Vector3[16];
  [SerializeField] public Vector3[] negativeAngles = new Vector3[16];

  private void Awake()
  {
    scavengerRigid2D = GetComponent<Rigidbody2D>();
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

  private void FixedUpdate()
  {
    stateMachine.FixedUpdate();
    //Linear drag
    scavengerRigid2D.AddForce(-scavengerRigid2D.velocity * scavengerRigid2D.mass, ForceMode2D.Force);
    if (scavengerRigid2D.velocity.magnitude < 0.1f) scavengerRigid2D.velocity = Vector2.zero;
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if(other.CompareTag("Player") && stateMachine.currentState.GetType() != typeof(ScavengerChaseState))
    {
      playerObject = other.gameObject;
      playerTransform = other.transform;
      stateMachine.ChangeState(new ScavengerChaseState(this, playerTransform, scavengerRigid2D));
    }
  }

  public void ChangeState(IState newState)
  {
    stateMachine.ChangeState(newState);
  }
}
