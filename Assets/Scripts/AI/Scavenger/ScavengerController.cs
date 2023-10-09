using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ScavengerController : MonoBehaviour
{
  private StateMachine stateMachine;
  private Rigidbody2D scavengerRigid2D;
  [SerializeField] private SpriteRenderer scavengerSpriteRenderer;

  private GameObject playerObject;
  private Transform playerTransform;

  public ParticleSystem scavengerThrusterParticle;
  public Light2D scavengerThrusterLight;

  [SerializeField] public Vector3[] positiveAngles = new Vector3[16];
  [SerializeField] public Vector3[] negativeAngles = new Vector3[16];

  private int maxHealth = 100;
  private int health = 100;

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

  private int TakeDamage(int damage)
  {
    health -= damage;
    if (health <= 0)
    {
      health = 0;
      Die();
    }

    StartCoroutine(InvulnerabilityFlash());
    return health;
  }

  IEnumerator InvulnerabilityFlash()
  {
    scavengerSpriteRenderer.color = Color.red;
    yield return new WaitForSeconds(0.4f);
    scavengerSpriteRenderer.color = Color.white;
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
  private void OnCollisionEnter2D(Collision2D other)
  {
    GameObject otherObject = other.gameObject;

    if (otherObject.CompareTag("Asteroid"))
    {
      var damage = 10;
      TakeDamage(damage);
      return;
    }
  }

  public void ChangeState(IState newState)
  {
    stateMachine.ChangeState(newState);
  }

  private void Die()
  {
    Destroy(gameObject);
  }
}
