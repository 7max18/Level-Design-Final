//script taken from here: https://sharpcoderblog.com/blog/2d-platformer-character-controller

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]

public class CharacterController2D : MonoBehaviour
{
    // Move player in 2D space
    public float maxSpeed = 3.4f;
    public float jumpHeight = 5.0f;
    public Vector2 throwForce;
    public float cameraBottomBound;
    public float cameraTopBound;
    public float cameraLeftBound;
    public float cameraRightBound;
    public Animator animator;
    public GameObject deathWall;
    public Door door;
    private bool facingRight = true;
    private float moveDirection = 0;
    private bool isGrounded;
    private bool jumping;
    private bool dead;
    private Vector3 checkpoint;
    private GameObject throwable;
    private bool pickedUpObject;
    private Rigidbody2D r2d;
    private Collider2D mainCollider;
    private AudioSource deathScream;
    

    // Check every collider except Player and Ignore Raycast
    LayerMask layerMask = ~(1 << 8);
    Transform t;

    // Use this for initialization
    void Start()
    {
        t = transform;
        r2d = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<Collider2D>();
        r2d.freezeRotation = true;
        r2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        deathScream = Camera.main.GetComponent<AudioSource>();
        facingRight = t.localScale.x > 0;
        gameObject.layer = 8;
        checkpoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement controls
        moveDirection = Input.GetAxisRaw("Horizontal");

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
            jumping = true;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (pickedUpObject)
            {
                if (t.localScale.x > 0)
                {
                    throwable.GetComponent<Rigidbody2D>().AddForce(throwForce, ForceMode2D.Impulse);
                }
                else
                {
                    throwable.GetComponent<Rigidbody2D>().AddForce(new Vector2(-throwForce.x, throwForce.y), ForceMode2D.Impulse);
                }
                throwable.GetComponent<ThrowableObject>().thrown = true;
                pickedUpObject = false;
                throwable = null;
            }
            else if (throwable)
            {
                pickedUpObject = true;
            }
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

            if (jumping)
            {
                r2d.velocity += new Vector2(0, jumpHeight);
                jumping = false;
            }

            animator.SetFloat("Speed", Mathf.Abs(r2d.velocity.x));
        }
        else
        {
            r2d.velocity = Vector2.zero;
        }

        if (pickedUpObject)
        {
            throwable.GetComponent<Rigidbody2D>().position = Vector2.Scale(new Vector2(0, 2.5f), new Vector2(transform.localScale.x, transform.localScale.y)) + r2d.position;
            throwable.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        // Simple debug
        Debug.DrawLine(groundCheckPos, groundCheckPos - new Vector3(0, 0.23f, 0), isGrounded ? Color.green : Color.red);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Treasure"))
        {
            door.active = true;
            deathWall.GetComponent<DeathWall>().activated = true;
            checkpoint = other.transform.position;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Death"))
        {
            deathScream.Play();
            dead = true;
            pickedUpObject = false;
            throwable = null;
            StartCoroutine("Respawn");
        }
        else if (other.CompareTag("Door"))
        {
            if (other.gameObject.GetComponent<Door>().active)
            {
                SceneManager.LoadScene(other.gameObject.GetComponent<Door>().nextLevel);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Throwable"))
        {
            if (collision.GetComponent<ThrowableObject>().isGrounded)
            {
                throwable = collision.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Throwable"))
        {
            throwable = null;
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1.0f);
        deathWall.transform.position = Vector3.zero;
        EventManager.TriggerEvent("Restart");
        transform.position = checkpoint;
        dead = false;
    }

}
