using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemy : MonoBehaviour
{
  
    private float DaggerSpeed = 20f;
    private Rigidbody2D rb;
    private Animator anim;
    private float elapsedTime=0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {

        rb.velocity = new Vector2(DaggerSpeed, 0);
        if(transform!=null)
        {
            elapsedTime += Time.deltaTime;
        }

        if(elapsedTime > 3f)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            anim.SetBool("HitEnemy", true);
            Destroy(gameObject, 2f);
        }
    }
}