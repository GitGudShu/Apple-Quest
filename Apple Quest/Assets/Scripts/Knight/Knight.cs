using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using System;

public class Knight : MonoBehaviour
{
    public static event Action OnPlayerDeath;
    public static event Action onWin;

    private Animator m_Anim;
    private Rigidbody2D m_RB;
    private SpriteRenderer m_SpriteRenderer;
    private BoxCollider2D m_BC;
    private Health m_Health;

    private Coroutine m_FlashCR;

    private bool m_FacingRight = true;
    private bool doubleJump;
    private bool isRolling;
    private bool canRoll = true;
    private bool isDead = false;
    private bool isOnIce;
    public bool isImmune;

    private Vector2 rollingDirection;

    private float movement;
    private float iceSpeed;

    [SerializeField] private float Speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float rollVelocity = 10f;
    [SerializeField] private float rollTime = 0.5f;
    [SerializeField] private Transform FeetPosition;
    [SerializeField] private LayerMask GroundLayer;
    [SerializeField] private LayerMask IceLayer;
    [SerializeField] private LayerMask TrapLayer;
    [SerializeField] private LayerMask WallLayer;

    private void OnEnable()
    {
        OnPlayerDeath += DisablePlayerMovement;
        onWin += DisablePlayerMovement;
    }

    private void OnDisable()
    {
        OnPlayerDeath -= DisablePlayerMovement;
        onWin -= DisablePlayerMovement;
    }

    // Awake is called before Start (You want to define your private variable or reference here)
    void Awake()
    {
        // References for rigidbody and animator from gameObject
        m_Anim = GetComponent<Animator>();
        m_RB = GetComponent<Rigidbody2D>();
        m_BC = GetComponent<BoxCollider2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Health = GetComponent<Health>();
        iceSpeed = Speed * 1.5f;
    }

    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        var rollInput = Input.GetButtonDown("Shift");
        isOnIce = Physics2D.OverlapCircle(new Vector2(FeetPosition.position.x, FeetPosition.position.y), 0.2f, IceLayer.value);

        // Movement
        movement = Input.GetAxis("Horizontal");
        if(movement != 0)
            m_RB.velocity = new Vector2(movement * (isOnIce ? iceSpeed:Speed), m_RB.velocity.y);

        // Orientation
        if (movement < 0 && m_FacingRight)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            m_FacingRight = false;
        }
        else if (movement > 0 && !m_FacingRight)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            m_FacingRight = true;
        }

        if (isGrounded() && !Input.GetButton("Jump"))
            doubleJump = false;

        if (Input.GetButtonDown("Jump"))
            if (isGrounded() || doubleJump) Jump();

        // Roll
        if (rollInput && canRoll)
        {
            isRolling = true;
            canRoll = false;
            isImmune = true;
            rollingDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (rollingDirection == Vector2.zero)
                rollingDirection = new Vector2(transform.localScale.x, 0);

            // Stop Roll
            StartCoroutine(stopRolling());

        }

        // Set animator parameters
        m_Anim.SetBool("Grounded", isGrounded());
        m_Anim.SetFloat("Speed", Mathf.Abs(movement));
        m_Anim.SetBool("isRolling", isRolling);

        if (isRolling)
        {
            m_RB.velocity = rollingDirection.normalized * rollVelocity;
            return;
        }

        if (isGrounded())
            canRoll = true;
        else
            canRoll = false;

        // Death
        if (isDead)
        {
            Debug.Log("Game Over");
        }

        // Manage friction 
        //m_BC.sharedMaterial.friction = (!isGrounded() || isOnIce)?  0f : 0.6f;
    }

    private void Jump()
    {
        m_RB.velocity = Vector2.up * jumpForce;
        doubleJump = !doubleJump;
        m_Anim.SetTrigger("Jump");
    }

    public void TakeDamage(int a_Damage)
    {
        if (!isImmune)
        {
            m_Health.health -= a_Damage;
            if (m_Health.health <= 0)
                Die();
            // Visual flash
            if (m_FlashCR != null)
            {
                StopCoroutine(m_FlashCR);
            }

            m_Anim.SetTrigger("Hurt");
            m_FlashCR = StartCoroutine(CR_Flash());
        }
    }

    private void Die()
    {
        Debug.Log("Player dead!");
        isImmune = true;
        isDead = true;
        m_Anim.SetTrigger("Dead");
        OnPlayerDeath?.Invoke();
    }

    IEnumerator stopRolling()
    {
        yield return new WaitForSeconds(rollTime);
        isRolling = false;
        isImmune = false;
    }

    IEnumerator CR_Flash()
    {
        isImmune = true;
        for (int i = 0; i < 4; i++)
        {
            m_SpriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.15f);
            m_SpriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.15f);
        }
        isImmune = false;
    }

    public bool isGrounded()
    {
        Vector2 t_FeetPos = new Vector2(FeetPosition.position.x, FeetPosition.position.y);
        return Physics2D.OverlapCircle(t_FeetPos, 0.2f, GroundLayer.value)
            || Physics2D.OverlapCircle(t_FeetPos, 0.2f, IceLayer.value)
            || Physics2D.OverlapCircle(t_FeetPos, 0.2f, TrapLayer.value);
    }

    public bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(m_BC.bounds.center
                                                    , m_BC.bounds.size
                                                    , 0
                                                    , new Vector2(transform.localScale.x, 0)
                                                    , 0.1f
                                                    , WallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return Input.GetAxis("Horizontal") == 0 && isGrounded() && !onWall();
    }

    private void DisablePlayerMovement()
    {
        m_Anim.enabled = false;
        Time.timeScale = 0;
    }


    public void Win()
    {
       onWin?.Invoke();
    }
}
