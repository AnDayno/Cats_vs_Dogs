using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed = 10;
    public float gravity = 10;
    public float maxVelocityChange = 10;
    public float jumpHeight = 2;
    public int health = 1;
    public int points;

    private bool grounded;
    private Transform playerTransform;
    private Rigidbody _rigidbody;

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
        playerTransform.Rotate(0, Input.GetAxis("Horizontal"), 0);

        Vector3 targetVelocity = new(0, 0, Input.GetAxis("Vertical"));
        targetVelocity = playerTransform.TransformDirection(targetVelocity);
        targetVelocity *= speed;

        Vector3 velocity = _rigidbody.velocity;
        Vector3 velocityChange = targetVelocity - velocity;
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;
        _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);

        if (Input.GetButton("Jump") && grounded == true)
        {
            _rigidbody.velocity = new Vector3(velocity.x, CalculateJump(), velocity.z);
        }

        _rigidbody.AddForce(new Vector3(0, -gravity * _rigidbody.mass, 0));
        grounded = false;
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

    private void OnCollisionStay(Collision collision)
    {
        grounded = true;
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
