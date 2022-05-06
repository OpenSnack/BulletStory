using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Entity
{
    public float moveSpeed;
    public float fireRate;
    private float fireLimiter;

    public GameObject bullets;
    public Transform bulletSpawn;
    public LevelManager levelManager;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        tag = "player";
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire") && Time.time > fireLimiter)
        {
            fireLimiter += 1 / fireRate;
            Instantiate(bullets, bulletSpawn.position, bulletSpawn.rotation);
        }
        else if (Input.GetButtonDown("Pause"))
        {
            if (levelManager.GetPaused())
            {
                levelManager.Unpause();
            }
            else
            {
                levelManager.Pause();
            }
        }
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        float moveVertical = Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime;

        transform.Translate(new Vector3(moveHorizontal, moveVertical));
        animator.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Entity entity = collider.gameObject.GetComponent<Entity>();
        if (entity)
        {
            Destroy(collider.gameObject);
        }
    }
}
