using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform gunPointTransform;

    [SerializeField] private float bulletSpeed = 100f;
    [SerializeField] private float movementSpeed = 10.0f;
    [SerializeField] private float rotationSpeed = 75.0f;
    [SerializeField] private float jumpVelocity = 5.0f;

    [SerializeField] private LayerMask groundLayer;

    private GameBehaviour _gameManager;

    private const float DistanceToGround = 0.1f;

    private float _vInput;
    private float _hInput;

    private Rigidbody _rigidbody;
    private Transform _transform;
    private CapsuleCollider _capsuleCollider;


    private void Start()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameBehaviour>();
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) & Time.timeScale > 0.0f)
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        Move();
        if (IsGrounded() & Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private bool IsGrounded()
    {
        var playerBounds = _capsuleCollider.bounds;
        var playerBottom = new Vector3(playerBounds.center.x, playerBounds.min.y, playerBounds.center.z);
        var isGrounded = Physics.CheckCapsule(playerBounds.center, playerBottom, DistanceToGround, groundLayer,
            QueryTriggerInteraction.Ignore);
        return isGrounded;
    }

    private void Jump()
    {
        _rigidbody.AddForce(jumpVelocity * Vector3.up, ForceMode.Impulse);
    }

    private void Move()
    {
        _vInput = Input.GetAxis("Vertical") * movementSpeed;
        _hInput = Input.GetAxis("Horizontal") * rotationSpeed;

        var rotationVector = Vector3.up * _hInput;
        var rotationAngle = Quaternion.Euler(rotationVector * Time.fixedDeltaTime);

        _rigidbody.MovePosition(_transform.position + _vInput * Time.fixedDeltaTime * _transform.forward);
        _rigidbody.MoveRotation(_rigidbody.rotation * rotationAngle);
    }

    private void Shoot()
    {
        var playerTransform = transform;
        var playerRotation = playerTransform.rotation;
        var newBullet = Instantiate(bullet, gunPointTransform.position, playerRotation);
        var bulletRigidbody = newBullet.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = playerTransform.forward * bulletSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag($"Enemy"))
        {
            _gameManager.Hp -= 1;
        }
    }
}