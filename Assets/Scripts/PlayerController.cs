using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;
    private float xInput;

    [Header("Movemment Settings")]
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float moveResponsiveness = 5f;

    [SerializeField] float fallSpeed = 5f;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode resetKey = KeyCode.R;

    public bool isGrounded = true;

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        GetInput();
        ProcessInput();
        CheckGround();

        Debug.Log(isGrounded);
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
        rb.linearVelocity = rb.linearVelocity + Vector2.right * (xInput * moveSpeed * Vector2.right - rb.linearVelocity) * (1-Mathf.Exp(-moveResponsiveness*Time.deltaTime));
        //transform.position = new Vector3(transform.position.x + xInput * Time.deltaTime * moveSpeed, transform.position.y);
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void CheckGround()
    {
        // Cast a ray straight down.
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 2.3f);

        //Vector2 characterSize = new Vector2(2.991782f, 4.42372f); // there must be a way to get this right?

        //RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.right * characterSize, 0, Vector2.down, .5f);

        // If it hits something...
        //Debug.Log(hit.collider.tag);
        isGrounded = (Physics2D.Raycast(transform.position, -Vector2.up, 1.3f, LayerMask.GetMask("Floor Layer"))) ? true : false;
        Debug.DrawRay(transform.position, -Vector2.up * 1.3f, Color.green);
    }
}
