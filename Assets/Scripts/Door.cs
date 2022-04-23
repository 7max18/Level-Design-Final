using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Sprite closed;
    public Sprite open;
    public bool active;
    private SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = closed;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            sr.sprite = open;
        }
    }
}
