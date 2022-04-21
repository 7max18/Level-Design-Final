//script taken from here: https://sharpcoderblog.com/blog/2d-platformer-character-controller

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]

public class CharacterController2D : MonoBehaviour
{
    // Move player in 2D space
    public float maxSpeed = 3.4f;
    public float jumpHeight = 5.0f;
    public float gravityScale = 1.5f;
    public float cameraBottomBound;
    public float cameraTopBound;
    public float cameraLeftBound;
    public float cameraRightBound;
    public Animator animator;
    public GameObject deathWall;
    public Vector2 wallSpeed;
    public Transform checkpoint;
    public Door door;
    private bool facingRight = true;
    private float moveDirection = 0;
    private bool isGrounded = false;
    private bool dead;
    private Rigidbody2D r2d;
    private Collider2D mainCollider;
    private AudioSource deathScream;

    // Check every collider except Player and Ignore Raycast
    LayerMask layerMask = ~(1 << 2 | 1 << 8);
    Transform t;

    // Use this for initialization
    void Start()
    {
        t = transform;
        r2d = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<Collider2D>();
        r2d.freezeRotation = true;
        r2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        r2d.gravityScale = gravityScale;
        deathScream = Camera.main.GetComponent<AudioSource>();
        facingRight = t.localScale.x > 0;
        gameObject.layer = 8;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement controls
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection = -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection = 1;
        }
        else if (isGrounded || r2d.velocity.magnitude < 0.01f)
        {
            moveDirection = 0;
        }

        // Change facing direction
        if (moveDirection != 0 && !dead)
        {
            if (moveDirection > 0 && !facingRight)
            {
                facingRight = true;
                t.localScale = new Vector3(Mathf.Abs(t.localScale.x), t.localScale.y, transform.localScale.z);
            }
            if (moveDirection < 0 && facingRight)
            {
                facingRight = false;
                t.localScale = new Vector3(-Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);
            }
        }

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
        }

        // Camera follow
        Camera.main.transform.position = new Vector3(Mathf.Clamp(t.position.x, cameraLeftBound, cameraRightBound), Mathf.Clamp(t.position.y, cameraBottomBound, cameraTopBound), Camera.main.transform.position.z);
    }

    void FixedUpdate()
    {
        Bounds colliderBounds = mainCollider.bounds;
        Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, 0.1f, 0);
        // Check if player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheckPos, 0.2f, layerMask);
        animator.SetBool("IsJumping", !isGrounded);

        // Apply movement velocity
        if (!dead)
        {
            r2d.velocity = new Vector2(moveDirection * maxSpeed, r2d.velocity.y);
            animator.SetFloat("Speed", Mathf.Abs(r2d.velocity.x));
        }
        else
        {
            r2d.velocity = Vector2.zero;
        }

        // Simple debug
        Debug.DrawLine(groundCheckPos, groundCheckPos - new Vector3(0, 0.23f, 0), isGrounded ? Color.green : Color.red);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Treasure"))
        {
            door.active = true;
            deathWall.GetComponent<Rigidbody2D>().velocity = wallSpeed;
            checkpoint = other.transform.parent;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Death"))
        {
            deathScream.Play();
            dead = true;
            StartCoroutine("Respawn");
        }
        else if (other.CompareTag("Door"))
        {
            if (other.gameObject.GetComponent<Door>().active)
            {
                Debug.Log("You win!");
            }
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1.0f);
        deathWall.transform.position = Vector3.zero;
        transform.position = checkpoint.position;
        dead = false;
    }

}
