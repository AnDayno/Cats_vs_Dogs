using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed = 10;
    public float gravity = 10;
    public float maxVelocityChange = 10;
    public float jumpHeight = 2;
    public float rotationSpeed = 200;
    public int health = 1;
    public int points;

    private Transform playerTransform;
    private Rigidbody _rigidbody;
    [SerializeField] float groundCheckDistance = 5f;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody>();

        _rigidbody.useGravity = false;
        _rigidbody.freezeRotation = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        playerTransform.Rotate(0, horizontalInput * rotationSpeed * Time.deltaTime, 0);

        Vector3 targetVelocity = new(0, 0, Input.GetAxis("Vertical"));
        targetVelocity = playerTransform.TransformDirection(targetVelocity);
        targetVelocity *= speed;

        Vector3 velocity = _rigidbody.velocity;
        Vector3 velocityChange = targetVelocity - velocity;
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;
        _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);

        if (Input.GetButton("Jump") && Physics.Raycast(transform.position, Vector3.down, out RaycastHit _, groundCheckDistance))
        {
            _rigidbody.velocity = new Vector3(velocity.x, CalculateJump(), velocity.z);
        }

        _rigidbody.AddForce(new Vector3(0, -gravity * _rigidbody.mass, 0));
 
    }

    private void Update()
    {
        if (health < 1)
        {
            SceneManager.LoadScene(0);
        }
    }
    float CalculateJump()
    {
        float jump = Mathf.Sqrt(2 * jumpHeight * gravity);

        return jump;
    }

    private void OnTriggerEnter(Collider buddy)
    {
        if (buddy.CompareTag("Coin"))
        {
            points += 5;
            Destroy(buddy.gameObject);
        }
    }
}
