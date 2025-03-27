using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float speed = 5f;
	[SerializeField] private float sensitivity = 2f;
	[SerializeField] private float gravity = -9.81f;
	[SerializeField] private float jumpHeight = 1.5f;

	private CharacterController controller;
	private Vector3 velocity;
	private float xRotation;

	public Transform cameraTransform;

	private Vector3 knockbackDirection;
	private float knockbackTime;
	[SerializeField] private float knockbackForce = 10f;
	[SerializeField] private float knockbackDuration = 0.2f;

	void Start()
	{
		controller = GetComponent<CharacterController>();
		Cursor.lockState = CursorLockMode.Locked; // Maus zentrieren
		Cursor.visible = false;
	}

	void Update()
	{
		if (!GameManager.Instance.gameOver)
		{
			CameraMovement();

			if (knockbackTime > 0) 
			{
				ApplyKnockback(); //Knockback
			}
			else 
			{
				Movement(); //oder bewegen können
			}

			SimulateGravity();

			//Sprung
			if (Input.GetButtonDown("Jump") && controller.isGrounded)
			{
				Jump();
			}
		}
	}
	
	private void CameraMovement()
	{
		// Maussteuerung
		float mouseMovementX = Input.GetAxis("Mouse X") * sensitivity;
		float mouseMovementY = Input.GetAxis("Mouse Y") * sensitivity;

		xRotation -= mouseMovementY;
		xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Begrenzung nach oben/unten
		cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Kamera hoch und runter bewegen
		transform.Rotate(Vector3.up * mouseMovementX); // Charakter nur rechts und links bewegen
	}

	private void Movement()
	{
		// Bewegung
		float movementX = Input.GetAxis("Horizontal"); // A/D oder Pfeiltasten
		float movemenZ = Input.GetAxis("Vertical");   // W/S oder Pfeiltasten

		Vector3 movement = transform.right * movementX + transform.forward * movemenZ;
		controller.Move(movement * speed * Time.deltaTime);
	}

	private void SimulateGravity()
	{
		// Gravitationsimulation durch Haften am Boden
		if (controller.isGrounded && velocity.y < 0)
		{
			velocity.y = -2f;
		}

		velocity.y += gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime); //Charakter wird anhand des übergebenen Vector3 Velocity "bewegt"
	}
	private void Jump()
	{
		velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
	}

	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hit.collider.CompareTag("Enemy"))
		{
			Vector3 pushDirection = transform.position - hit.collider.transform.position;
			pushDirection.y = 0; // Keine vertikale Bewegung
			CalculateKnockback(pushDirection);
		}
	}
	private void CalculateKnockback(Vector3 direction)
	{
		knockbackDirection = direction.normalized * knockbackForce;
		knockbackTime = knockbackDuration;
	}
	private void ApplyKnockback()
	{
		knockbackTime -= Time.deltaTime;
		controller.Move(knockbackDirection * Time.deltaTime);
	}
}