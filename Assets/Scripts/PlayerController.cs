using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;
    private float xInput;

    [Header("Movemment Settings")]
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float moveResponsiveness = 5f;
    [SerializeField] float moveResponsivenessAir = 5f;

    [SerializeField] float fallSpeed = 5f;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode resetKey = KeyCode.R;

    public bool isGrounded = true;
    public bool didntDoubleJumpedYet = true;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        GetInput();
        ProcessInput();
        CheckGround();

        //Debug.Log(isGrounded);
    }

    private void GetInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(jumpKey))
        {
            Jump();
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
        }
        else if (didntDoubleJumpedYet)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            didntDoubleJumpedYet = false;
        }
    }

    private void CheckGround()
    {
        // BoxCast a the character down a little bit (0.01) until it hits the anything part of the "Floor Layer"
        // 2.991782f is the player x size idk how to get
        // 4.42372f is the player y size 
        isGrounded = (Physics2D.BoxCast(transform.position, new Vector2(2.991782f, 4.42372f), 0, Vector2.down, .01, LayerMask.GetMask("Floor Layer"))) ? true : false;
    }
}
