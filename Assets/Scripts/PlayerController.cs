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

        //Debug.Log(isGrounded);
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
        isGrounded = (Physics2D.BoxCast(transform.position, new Vector2(2.991782f, transform.localScale.x * 5f), 0, Vector2.down, .01f, LayerMask.GetMask("Floor Layer"))) ? true : false;
    }

    private void IncreaseSize()
    {
        Vector3 scaleChange = new Vector3(growRate, growRate, growRate);
        cam.orthographicSize += Mathf.Abs(xInput) *  cameraGrowthRate * Time.deltaTime;
        transform.localScale += Mathf.Abs(xInput) * scaleChange;
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
}
