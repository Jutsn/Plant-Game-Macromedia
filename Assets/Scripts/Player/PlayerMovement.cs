using System;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;

    [SerializeField] private float groundDrag;

    [SerializeField] private float normalJumpForce;
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

	[Header("Movement Skills")]
	[SerializeField] private int jumpCount;
	[SerializeField] private float extraJumpForce;
	private int jumpsLeft;
	[SerializeField] private int waterConsumptionExtraJump;

	//[SerializeField] private float jetpackFlyTimeMax;
	//private float FlyTimeLeft;
	[SerializeField] private bool hasJetpack;
	[SerializeField] private float jetpackFlyForce;
	[SerializeField] private float TimeUntilFlyingAfterJump;
	private bool readyToFly = true;


	[SerializeField] private int waterConsumptionFlying;
	[SerializeField] private float waterConsumptionFlyingIntervallInSeconds;
	private float flyWaterConsumptionTimer = 0f;

	[Header("Knockback")]
    [SerializeField] private int knockbackForce;
    [SerializeField] private float upwardModifier;
    [SerializeField] private float maxVerticalKnockback;
    [SerializeField] private float knockbackDuration;
    private float knockbackTime;
    private bool isKnockbacked;


    [Header("Keybinds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode weaponModeChangeKey = KeyCode.C;
    [SerializeField] private KeyCode InteractionKey = KeyCode.E;


    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    private bool grounded;

    [Header("Standing in Water Check/Refill")]
    [SerializeField] private LayerMask whatIsWater;
    [SerializeField] private int standingInWaterTankFillAmount;
    [SerializeField] private float TankFillRateInSeconds;
    private bool fillWater;

	WeaponBehaviour weaponBehaviourSkript;
    MainPlant mainPlantSkript;

	[Header("Antitoxin-Interaction")]
	public bool hasAntitoxin;
    public bool inInteractionRangeWithPlant;
    public float interactionRange;


    private void OnEnable()
    {
        StatsManager.OnStatsChanged += RefreshStats;
    }

    private void OnDisable()
    {
        StatsManager.OnStatsChanged -= RefreshStats;
    }

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.freezeRotation = true;
		weaponBehaviourSkript = GameObject.Find("Weapon").GetComponent<WeaponBehaviour>();
		mainPlantSkript = GameObject.Find("Great Plant").GetComponent<MainPlant>();
        moveSpeed = StatsManager.Instance.stats.moveSpeed;
        groundDrag = StatsManager.Instance.stats.groundDrag;
        jumpForce = StatsManager.Instance.stats.jumpForce;
        jumpCooldown = StatsManager.Instance.stats.jumpCooldown;
        airMultiplier = StatsManager.Instance.stats.airMultiplier;

	}

    private void RefreshStats(StatsManager stats)
    {
        moveSpeed = StatsManager.Instance.stats.moveSpeed;
        groundDrag = StatsManager.Instance.stats.groundDrag;
        jumpForce = StatsManager.Instance.stats.jumpForce;
        jumpCooldown = StatsManager.Instance.stats.jumpCooldown;
        airMultiplier = StatsManager.Instance.stats.airMultiplier;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
		//FlyTimeLeft = jetpackFlyTimeMax;
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

        JumpButtonInput();


		

		GetMouseInput();

        if (Input.GetKeyDown(weaponModeChangeKey) || isMouseWheelScrolled && !GameManager.Instance.gameOver)
        {
            weaponBehaviourSkript.SwitchWeaponMode();
		}

        if (Input.GetKeyDown(InteractionKey) && inInteractionRangeWithPlant && hasAntitoxin && !GameManager.Instance.gameOver) //Pflanze entgiften
		{
            hasAntitoxin = false;
            mainPlantSkript.DetoxPlant();
		}

    }
    private void JumpButtonInput()
    {
		if (Input.GetKey(jumpKey) && readyToJump && grounded && jumpsLeft > 0 && !GameManager.Instance.gameOver) //Sprung am Boden
		{
			readyToJump = false; // verhindert durchgängiges Anwenden von Kraft und sorgt für kontrollierten, kurzen Impuls
			readyToFly = false;
			float jumpPower = normalJumpForce;

			jumpsLeft -= 1;

			Jump(jumpPower);

			Invoke("ResetJump", jumpCooldown);
			Invoke("ResetFly", TimeUntilFlyingAfterJump);
		}
		else if (Input.GetKeyDown(jumpKey) && readyToJump && jumpsLeft > 0 && WaterTank.Instance.waterLevel > 0 && !GameManager.Instance.gameOver) //Sprünge in der Luft
		{
			readyToJump = false;
			readyToFly = false;
			float jumpPower = extraJumpForce;
			jumpsLeft -= 1;

			WaterTank.Instance.waterLevel -= waterConsumptionExtraJump;

			Jump(jumpPower);

			Invoke("ResetJump", jumpCooldown);
			Invoke("ResetFly", TimeUntilFlyingAfterJump);
		}
		else if (Input.GetKey(jumpKey) && readyToJump && readyToFly && hasJetpack && jumpsLeft == 0 && WaterTank.Instance.waterLevel > 0 && !GameManager.Instance.gameOver) //Fly with Jetpack (&& FlyTimeLeft > 0)
		{
			Fly();
			//FlyTimeLeft -= Mathf.Round(Time.deltaTime * 100) / 100f; //Diese Zeile und Bool in else if-Bedingung rausnehmen, um Flugdauer nur von Wasserstand abhängig zu machen. Außerdem maxFlyTime und FlyTime Variablen im Kopf entfernen, da nicht mehr gebraucht.
			FlyingWaterConsumption();
		}

		//if (FlyTimeLeft <= 0) // Verhindere negative FlyTime
		//{
		//	FlyTimeLeft = 0;
		//}

		if (readyToJump && grounded) // reset jumpCount and flyTime, wenn auf Boden
		{
			jumpsLeft = jumpCount;
			//FlyTimeLeft = jetpackFlyTimeMax;
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
    private void Jump(float jumpPower)
    {
        //reset y velocity
        playerRb.linearVelocity = new Vector3(playerRb.linearVelocity.x, 0f, playerRb.linearVelocity.z);

        playerRb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
    }
    private void Fly()
    {
		//reset y velocity
		playerRb.linearVelocity = new Vector3(playerRb.linearVelocity.x, 0f, playerRb.linearVelocity.z);

		playerRb.AddForce(transform.up * jetpackFlyForce, ForceMode.Impulse);
	}

    private void FlyingWaterConsumption()
    {
        flyWaterConsumptionTimer += Time.deltaTime;
        if(flyWaterConsumptionTimer >= waterConsumptionFlyingIntervallInSeconds) //Nach Ablauf einer Sekunde...
        {
			WaterTank.Instance.waterLevel -= waterConsumptionFlying;
			flyWaterConsumptionTimer = 0f;
		}
		
	}

	private void ResetJump()
    {
        readyToJump = true;
    }

	private void ResetFly()
	{
		readyToFly = true;
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
			StartCoroutine(FillWaterTankCoroutine(standingInWaterTankFillAmount)); //Timer einbauen
        }
        if (other.gameObject.CompareTag("PickUp"))
        {
			CheckPickUpType(other);
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
			yield return new WaitForSeconds(TankFillRateInSeconds);
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