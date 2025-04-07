using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;

    [SerializeField] private float groundDrag;

    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    private bool readyToJump = true;

	public Transform playerOrientation;

	float horizontalInput;
	float verticalInput;
	public float mouseWheelInput;
	bool isMouseWheelScrolled;

	Vector3 moveDirection;

	Rigidbody playerRb;

	[Header("Knockback")]
    [SerializeField] private int knockbackForce;
    [SerializeField] private float upwardModifier;
    [SerializeField] private float maxVerticalKnockback;
    [SerializeField] private float knockbackDuration;
    private float knockbackTime;
    private Vector3 finalKnockback;
    private bool isKnockbacked;


    [Header("Keybinds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode weaponModeChangeKey = KeyCode.C;


    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    private bool grounded;

    [Header("Water Check")]
    [SerializeField] private LayerMask whatIsWater;
    [SerializeField] private int standingInWaterTankFillAmount;
    [SerializeField] private float generalTankFillRate;
    private bool fillWater;

    WeaponBehaviour weaponBehaviourSkript;
    MainPlant mainPlantSkript;

    public bool hasAntitoxin;




	private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.freezeRotation = true;
		weaponBehaviourSkript = GameObject.Find("Weapon").GetComponent<WeaponBehaviour>();
		mainPlantSkript = GameObject.Find("Great Plant").GetComponent<MainPlant>();

	}

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();

        //handle drag
        if (grounded)
        {
            playerRb.linearDamping = groundDrag;
        }
        else
        {
            playerRb.linearDamping = 0;
        }
    }

    private void FixedUpdate()
    {
        if (isKnockbacked)
        {
            knockbackTime -= Time.deltaTime;

            if (knockbackTime < 0)
            {
                isKnockbacked = false;
            }
        }
        else if (!isKnockbacked)
        {
            MovePlayer();
        }
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        mouseWheelInput = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetKey(jumpKey) && readyToJump && grounded && !GameManager.Instance.gameOver)
        {
            readyToJump = false;

            Jump();

            Invoke("ResetJump", jumpCooldown);
        }

        GetMouseInput();

        if (Input.GetKeyDown(weaponModeChangeKey) || isMouseWheelScrolled)
        {
            weaponBehaviourSkript.SwitchWeaponMode();
		}

        if (Input.GetKeyDown(KeyCode.E) && hasAntitoxin) //Pflanze entgiften
        {
            hasAntitoxin = false;
            mainPlantSkript.DetoxPlant();
		}

    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = playerOrientation.forward * verticalInput + playerOrientation.right * horizontalInput;

        //on ground
        if (grounded && !GameManager.Instance.gameOver)
            playerRb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else if (!grounded && !GameManager.Instance.gameOver)
            playerRb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(playerRb.linearVelocity.x, 0f, playerRb.linearVelocity.z);

        //limit move velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            playerRb.linearVelocity = new Vector3(limitedVel.x, playerRb.linearVelocity.y, limitedVel.z);
        }
    }
    private void Jump()
    {
        //reset y velocity
        playerRb.linearVelocity = new Vector3(playerRb.linearVelocity.x, 0f, playerRb.linearVelocity.z);

        playerRb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

	private void GetMouseInput()
	{
		if (mouseWheelInput != 0)
		{
			isMouseWheelScrolled = true;
		}
		else
		{
			isMouseWheelScrolled = false;
		}
	}

	private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Knockback(collision);
        }
    }
    private void Knockback(Collision collision)
    {
        Vector3 knockbackDirection = (transform.position - collision.transform.position).normalized;

        // Begrenze die Y-Komponente des Knockbacks
        knockbackDirection.y = Mathf.Clamp(knockbackDirection.y + upwardModifier, 0, maxVerticalKnockback);

        // Stelle sicher, dass der Knockback nicht zu stark vertikal ausfällt
        Vector3 finalKnockback = new Vector3(knockbackDirection.x, knockbackDirection.y, knockbackDirection.z);
        knockbackTime = knockbackDuration;
        isKnockbacked = true;
        playerRb.AddForce(finalKnockback * knockbackForce, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water")) //Wassertank auffüllen, wenn im Wasser stehend
        {
			fillWater = true;
			StartCoroutine(FillWaterTankCoroutine(standingInWaterTankFillAmount)); //Timer einbauen
        }

        CheckPickUpType(other);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water")) //Wassertank auffüllen, wenn im Wasser stehend
        {
            fillWater = false;
        }
    }

    IEnumerator FillWaterTankCoroutine(int tankFillAmount) //Falls Powerups Tank auch auffüllen können, einfach Coroutine callen und Wert durchgeben
    {
        while (fillWater)
        {
			WaterTank.Instance.FillTank(tankFillAmount);
			yield return new WaitForSeconds(generalTankFillRate);
		}
    }

    void CheckPickUpType(Collider other)
    {
		if (other.gameObject.GetComponent<PickUp>().pickUpType == PickUpType.antitoxin) //Antitoxin-Check
		{
			hasAntitoxin = true;
            Destroy(other.gameObject);
		}
	}
}