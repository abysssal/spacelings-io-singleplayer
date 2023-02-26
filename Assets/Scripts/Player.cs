using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    [Header("Movement")]

    public float speed;
    public float jumpPower;
    public int jumps;

    public float fallDamageThreshold;
    private float previousYVelocity;

    public Vector3 mousePos;

    [Header("Attributes")]
    public float health;
    public float hunger;

    [Header("Components")]
    public Animator anim;
    public Rigidbody2D rb;
    public Inventory inventory;

    public Slider healthBar, hungerBar;

    [Header("Blocks")]
    public float timeBetweenAction;
    public Tilemap tilemap;

    public bool canDestroy, canBuild;

    [Header("Technical")]
    public Vector3 airBorneWhere;
    public float timeAirBorne;

    public void Awake()
    {
        healthBar = GameObject.Find("HPBar").GetComponent<Slider>();
        hungerBar = GameObject.Find("HungerBar").GetComponent<Slider>();
        anim = GetComponent<Animator>();
        canBuild = true;
        canDestroy = true;
        rb = GetComponent<Rigidbody2D>();
        jumps = 2;
    }

    public void Start()
    {
        InvokeRepeating("useHunger", 5, 5);
    }

    public void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        healthBar.value = health;
        hungerBar.value = hunger;

        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(new Vector2(speed * Time.deltaTime, 0), ForceMode2D.Impulse);
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(new Vector2(-speed * Time.deltaTime, 0), ForceMode2D.Impulse);
            anim.SetBool("isWalking", true);
        } 
        else
        {
            anim.SetBool("isWalking", false);
        }

        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.W) && jumps > 0)
        {
            rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
            anim.SetTrigger("jump");
            jumps--;
        }

        if (rb.velocity.y < -0.5f)
        {
            anim.SetBool("isFalling", true);
        }
        else
        {
            anim.SetBool("isFalling", false);
        }

        if (mousePos.x < transform.position.x)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }  
        else if (mousePos.x > transform.position.x)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(Mathf.Floor(mousePos.x), Mathf.Floor(mousePos.y)), 1);
            if (hit.collider && canDestroy)
            {
                StartCoroutine(DestroyBlock(tilemap, timeBetweenAction, new Vector3Int(Mathf.FloorToInt(mousePos.x), Mathf.FloorToInt(mousePos.y), 0)));
            } 
        }
    }

    public void FixedUpdate()
    {
        if (rb.velocity.y < previousYVelocity)
        {
            float fallDistance = Mathf.Abs(rb.velocity.y) * Time.fixedDeltaTime;
            if (fallDistance >= fallDamageThreshold)
            {
                ApplyFallDamage(fallDistance);
            }
        }
        previousYVelocity = rb.velocity.y;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumps = 2;
        }

        if (collision == null)
        {
            airBorneWhere = transform.position;
            InvokeRepeating("timeAirBorne", 0, 1);
        }

        if (collision != null)
        {
            if (airBorneWhere.y <= 3)
            {
                return;
            } else
            {
                health -= airBorneWhere.y - transform.position.y * (timeAirBorne * 3);
            }
        }
    }

    public IEnumerator DestroyBlock(Tilemap tilemap, float timeBetweenActions, Vector3Int pos)
    {
        canDestroy = false;
        canBuild = false;

        tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), null);

        Debug.DrawLine(transform.position, new Vector2(pos.x, pos.y), Color.red);

        yield return new WaitForSeconds(timeBetweenActions);

        canDestroy = true;
        canBuild = true;
    }

    public void useHunger()
    {
        if (hunger >= 1)
        {
            hunger--;
        } 
        else
        {
            health = health - 10;
        }
    }

    public void ApplyFallDamage(float fallDistance)
    {
        int damage = Mathf.RoundToInt(fallDistance - fallDamageThreshold);
        health -= damage;
    }
}
