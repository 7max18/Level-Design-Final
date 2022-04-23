using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ThrowableObject : MonoBehaviour
{
    public bool isGrounded;
    public bool pickedUp;
    LayerMask layerMask = ~(1 << 2 | 1 << 8);
    private Collider2D mainCollider;
    private Vector3 spawnPoint;
    private UnityAction restartListener;

    private void Awake()
    {
        restartListener = new UnityAction(ReturnToSpawnPoint);
    }

    private void OnEnable()
    {
        EventManager.StartListening("Restart", restartListener);
    }

    private void OnDisable()
    {
        EventManager.StopListening("Restart", restartListener);
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCollider = GetComponent<Collider2D>();
        spawnPoint = transform.position;
    }

    private void FixedUpdate()
    {
        Bounds colliderBounds = mainCollider.bounds;
        Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, 0.1f, 0);
        isGrounded = Physics2D.OverlapCircle(groundCheckPos, 0.2f, layerMask);
    }

    private void ReturnToSpawnPoint()
    {
        transform.position = spawnPoint;
    }
}
