using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float normVar = 0.70922f;
    //I had trouble with normalization in the dash and found that multiplying Vector2 dash z or y component by this number 'normalizes' it. 
    //Not the best solution, but it works 
    //-- Robin 

    //movement
    [SerializeField] private float moveSpeed;
    private Vector2 moveDirection;

    //dash
    private bool canDash = true;
    private bool isDashing;
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;

    private AudioSource dashSound;
    [SerializeField] private string soundName;

    //Sprites and animation
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        dashSound = GameObject.Find(soundName).GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing) //makes it so you can't move while dashing
        {
            return;
        }

        Processinputs();
    }

    private void FixedUpdate()
    {
        if (isDashing) //makes it so you can't move while dashing
        {
            return;
        }

        Move();
    }

    private void Processinputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;

        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    private IEnumerator Dash()
    {
        dashSound.Play();
        canDash = false;
        isDashing = true;
        ManageDashDirection();
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void ManageDashDirection()
    {
        //check each axis direction if it is being used, and make the dash go in the corresponding direction
        //I don't like this implementation. If you can think of a better way to do it, please message me and let me know
        // -- Robin
        if (Input.GetAxisRaw("Horizontal") > 0f && Input.GetAxisRaw("Vertical") > 0f) //right-up
        {
            rb.velocity = new Vector2(transform.localScale.x * dashingPower * normVar, transform.localScale.y * dashingPower * normVar);
        }
        else if (Input.GetAxisRaw("Horizontal") > 0f && Input.GetAxisRaw("Vertical") < 0f) //right-down
        {
            rb.velocity = new Vector2(transform.localScale.x * dashingPower * normVar, transform.localScale.y * dashingPower * -1f * normVar);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0f && Input.GetAxisRaw("Vertical") > 0f) //left-up
        {
            rb.velocity = new Vector2(transform.localScale.x * dashingPower * -1f * normVar, transform.localScale.y * dashingPower * normVar);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0f && Input.GetAxisRaw("Vertical") < 0f) //left-down
        {
            rb.velocity = new Vector2(transform.localScale.x * dashingPower * -1f * normVar, transform.localScale.y * dashingPower * -1f * normVar);
        }
        else if (Input.GetAxisRaw("Horizontal") > 0f) //right
        {
            rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0f) //left
        {
            rb.velocity = new Vector2(transform.localScale.x * dashingPower * -1f, 0f);
        }
        else if (Input.GetAxisRaw("Vertical") > 0f) //up
        {
            rb.velocity = new Vector2(0f, transform.localScale.y * dashingPower);
        }
        else if (Input.GetAxisRaw("Vertical") < 0f) //down
        {
            rb.velocity = new Vector2(0f, transform.localScale.y * dashingPower * -1f);
        }
    }
}
