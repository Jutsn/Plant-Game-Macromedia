using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]

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
<<<<<<< Updated upstream
=======
    MainPlant mainPlantSkript;
    
>>>>>>> Stashed changes



    private void Awake()
    {
        //stats = GameObject.Find("StatsManager").GetComponent<Stats>();
    }


    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.freezeRotation = true;
		weaponBehaviourSkript = GameObject.Find("Weapon").GetComponent<WeaponBehaviour>();
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
            playerRb.linearDamping = StatsManager.Instance.stats.groundDrag;
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

            Invoke("ResetJump", StatsManager.Instance.stats.jumpCooldown);
        }
       
        if (mouseWheelInput != 0)
        {
            isMouseWheelScrolled = true;
        }
        else
        {
            isMouseWheelScrolled = false;
        }

        if (Input.GetKeyDown(weaponModeChangeKey) || isMouseWheelScrolled)
        {
            weaponBehaviourSkript.SwitchWeaponMode();

		}
    }
    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = playerOrientation.forward * verticalInput + playerOrientation.right * horizontalInput;

        //on ground
        if (grounded && !GameManager.Instance.gameOver)
            playerRb.AddForce(moveDirection.normalized * StatsManager.Instance.stats.moveSpeed * 10f, ForceMode.Force);
        else if (!grounded && !GameManager.Instance.gameOver)
            playerRb.AddForce(moveDirection.normalized * StatsManager.Instance.stats.moveSpeed * 10f * StatsManager.Instance.stats.airMultiplier, ForceMode.Force);
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(playerRb.linearVelocity.x, 0f, playerRb.linearVelocity.z);

        //limit move velocity if needed
        if (flatVel.magnitude > StatsManager.Instance.stats.moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * StatsManager.Instance.stats.moveSpeed;
            playerRb.linearVelocity = new Vector3(limitedVel.x, playerRb.linearVelocity.y, limitedVel.z);
        }
    }
    private void Jump()
    {
        //reset y velocity
        playerRb.linearVelocity = new Vector3(playerRb.linearVelocity.x, 0f, playerRb.linearVelocity.z);

        playerRb.AddForce(transform.up * StatsManager.Instance.stats.jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
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

        // Stelle sicher, dass der Knockback nicht zu stark vertikal ausf�llt
        Vector3 finalKnockback = new Vector3(knockbackDirection.x, knockbackDirection.y, knockbackDirection.z);
        knockbackTime = knockbackDuration;
        isKnockbacked = true;
        playerRb.AddForce(finalKnockback * knockbackForce, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water")) //Wassertank auff�llen, wenn im Wasser stehend
        {
			fillWater = true;
<<<<<<< Updated upstream
			StartCoroutine(FillWaterTankCoroutine(standingInWaterTankFillAmount)); //Timer einbauen

=======
			StartCoroutine(FillWaterTankCoroutine(StatsManager.Instance.stats.standingInWaterTankFillAmount)); //Timer einbauen
>>>>>>> Stashed changes
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water")) //Wassertank auff�llen, wenn im Wasser stehend
        {
            fillWater = false;
        }
    }

    IEnumerator FillWaterTankCoroutine(int tankFillAmount) //Falls Powerups Tank auch auff�llen k�nnen, einfach Coroutine callen und Wert durchgeben
    {
        while (fillWater)
        {
			WaterTank.Instance.FillTank(tankFillAmount);
			yield return new WaitForSeconds(StatsManager.Instance.stats.generalTankFillRate);
		}
    }
}