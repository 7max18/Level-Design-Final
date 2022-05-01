using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWall : MonoBehaviour
{
    public Vector3 startingLocation;
    public bool activated;
    public Vector2 wallSpeed;

    void Start()
    {
        startingLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            GetComponent<Rigidbody2D>().velocity = wallSpeed;
            GetComponent<AudioSource>().Play();
            activated = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Throwable") && !collision.isTrigger)
        {
            if (collision.gameObject.GetComponent<ThrowableObject>().thrown)
            {
                StartCoroutine("TempSpeedDecrease");
            }
        }
    }

    private IEnumerator TempSpeedDecrease()
    {
        GetComponent<Rigidbody2D>().velocity = wallSpeed / 2;

        yield return new WaitForSeconds(1.5f);

        GetComponent<Rigidbody2D>().velocity = wallSpeed;
    }
}
