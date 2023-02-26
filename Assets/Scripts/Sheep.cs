using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    public int health;
    public int checkHealth;
    public int speed;

    public bool attacked;
    public bool wandering;

    public Animator anim;
    public Rigidbody2D rb;

    public Vector3 desiredLocation;

    public void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        checkHealth = health;

        InvokeRepeating("wander", Random.Range(5, 10), Random.Range(5, 10));
    }

    public void Update()
    {
        if (attacked)
        {
            run();
        }

        if (health <= 0) 
        {
            Destroy(gameObject);
        }
    }

    public void LateUpdate()
    {
        if (health != checkHealth)
        {
            run();
            checkHealth = health;
        }
        else
        {
            checkHealth = health;
        }
    }

    public void wander()
    {
        anim.SetBool("walking", true);
        wandering = true;
        if (Random.Range(0, 1) == 0)
        {
            desiredLocation.x -= transform.position.x - Random.Range(3, 10);
            while (transform.position != desiredLocation)
            {
                rb.AddForce(new Vector2(-speed * Time.deltaTime, 0), ForceMode2D.Impulse);
                transform.rotation = new Quaternion(0, 180, 0, 0);
            }
            anim.SetBool("walking", false);
            wandering = false;
        } 
        else
        {
            desiredLocation.x += transform.position.x + Random.Range(3, 10);
            while (transform.position != desiredLocation)
            {
                rb.AddForce(new Vector2(speed * Time.deltaTime, 0), ForceMode2D.Impulse);
                transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            anim.SetBool("walking", false);
            wandering = false;
        }
    }

    public void run()
    {
        anim.SetBool("attacked", true);
        attacked = true;
        if (Random.Range(0, 1) == 0)
        {
            desiredLocation.x -= transform.position.x - Random.Range(6, 20);
            while (transform.position != desiredLocation)
            {
                rb.AddForce(new Vector2((-speed * 2)* Time.deltaTime, 0), ForceMode2D.Impulse);
                transform.rotation = new Quaternion(0, 180, 0, 0);
            }
            attacked = false;
            anim.SetBool("attacked", true);
        }
        else
        {
            desiredLocation.x += transform.position.x + Random.Range(6, 20);
            while (transform.position != desiredLocation)
            {
                rb.AddForce(new Vector2((speed * 2) * Time.deltaTime, 0), ForceMode2D.Impulse);
                transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            attacked = false;
            anim.SetBool("attacked", true);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && (wandering || attacked))
        {
            rb.AddForce(new Vector2(0, 7), ForceMode2D.Impulse);
        }
    }
}
