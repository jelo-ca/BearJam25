using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;
    private float xInput;
    private Camera cam;

    [Header("Movemment Settings")]
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float moveResponsiveness = 5f;
    [SerializeField] float moveResponsivenessAir = 5f;

    [SerializeField] float growRate = 0.2f;
    [SerializeField] float cameraGrowthRate = 2f;
    [SerializeField] float fallSpeed = 5f;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode resetKey = KeyCode.R;

    [Header("Animations")]
    private Animator anim;

    public bool isGrounded = true;
    public bool didntDoubleJumpedYet = true;
    public int justJammed = 0;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        GetInput();
        ProcessInput();
        FlipPlayer();
        CheckGround();
        IncreaseSize();
        Animate();
        Audio();

        //Debug.Log(isGrounded);
    }
    void FixedUpdate()
    {
        Squished();
    }


    private void GetInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(jumpKey))
        {
            Jump();
        }
        if (Input.GetKeyDown(resetKey))
        {
            GameManager.instance.ResetLevel();
        }
    }

    private void ProcessInput()
    {
        // hehe, funky math :3
        rb.linearVelocity = rb.linearVelocity + Vector2.right * (xInput * moveSpeed * Vector2.right - rb.linearVelocity) * (1-Mathf.Exp(-(isGrounded ? moveResponsiveness : moveResponsivenessAir)*Time.deltaTime));
        //transform.position = new Vector3(transform.position.x + xInput * Time.deltaTime * moveSpeed, transform.position.y);
    }

    private void Jump()
    {
        
        if (isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            didntDoubleJumpedYet = true;
            anim.SetTrigger("Jump");
        }
        else if (didntDoubleJumpedYet)
        {
            rb.linearVelocityY = 0;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            didntDoubleJumpedYet = false;
            anim.SetTrigger("Jump");
        }
    }

    private void CheckGround()
    {
        // BoxCast a the character down a little bit (0.01) until it hits the anything part of the "Floor Layer"
        // 2.991782f is the player x size idk how to get
        // 4.42372f is the player y size 
        isGrounded = (Physics2D.BoxCast(transform.position + new Vector3(-0.04532099f, -0.2114919f, 0) * transform.localScale.x, new Vector2(3.100352f, 4.595364f) * transform.localScale.x, 0, Vector2.down, .01f, LayerMask.GetMask("Floor Layer"))) ? true : false;
    }
    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return; // Only show in play mode

        // Define the box position, size, and direction
        Vector2 boxSize = new Vector2(3.100352f, 4.595364f) * transform.localScale.x;
        Vector2 boxOrigin = (Vector2)(transform.position + new Vector3(-0.04532099f, -0.2114919f, 0) * transform.localScale.x);
        float castDistance = 0.01f;
        Vector2 castDirection = Vector2.down;

        // Perform the BoxCast and get the hit info
        RaycastHit2D hit = Physics2D.BoxCast(boxOrigin, boxSize, 0, castDirection, castDistance, LayerMask.GetMask("Floor Layer"));

        // Change the Gizmos color
        Gizmos.color = hit ? Color.green : Color.red;

        // Draw the starting position of the box
        Gizmos.DrawWireCube(boxOrigin, boxSize);

        // Draw the cast area
        Gizmos.DrawWireCube(boxOrigin + castDirection * castDistance, boxSize);
    }


    private void IncreaseSize()
    {
        rb.mass = (transform.localScale.x / 0.2f);
        cam.orthographicSize *= Mathf.Exp(Mathf.Abs(rb.linearVelocity.x/moveSpeed) * cameraGrowthRate * Time.deltaTime);
        transform.localScale *= Mathf.Exp(Mathf.Abs(rb.linearVelocity.x / moveSpeed) * growRate * Time.deltaTime);
    }

    public void DecreaseSize()
    {
        cam.orthographicSize /= Mathf.Pow(2, cameraGrowthRate/growRate);
        transform.localScale /= 2;
    }

    public void Grow()
    {
        cam.orthographicSize *= Mathf.Pow(2, cameraGrowthRate / growRate);
        transform.localScale *= 2;
        justJammed = 2;
    }

    public void Squished()
    {
        if (justJammed==0)
        {
            ContactPoint2D[] contacts = new ContactPoint2D[32];
            int n = rb.GetContacts(contacts);
            for (int i = 0; i <= n; i++)
            {
                if (contacts[i].enabled && contacts[i].separation <= -.1)
                {
                    GameManager.instance.ResetLevel();
                    n = 0;
                }
            }
        }
        else
        {
            justJammed--;
        }
    }

    private void FlipPlayer()
    {
        transform.eulerAngles = (xInput < 0) ? new Vector3(0, 180, 0) : new Vector3(0, 0, 0);
    }

    private void Animate()
    {
        anim.SetFloat("Speed", Mathf.Abs(xInput));
        anim.SetBool("isGrounded", isGrounded);
    }

    private void Audio()
    {
        if (isGrounded && !(xInput == 0))
        {
            SFXManager.instance.PlayFootsteps();
        }
        else
        {
            SFXManager.instance.StopFootsteps();
        }
    }
}