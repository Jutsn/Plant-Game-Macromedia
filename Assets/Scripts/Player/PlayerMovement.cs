using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerMovement : MonoBehaviour
{
	#region Variablen
	[Header("Movement")]
    [SerializeField] private float moveSpeed;
	private float walkSpeed;
	private float sprintSpeed;
    [SerializeField] private float normalSprintMultiplier;
    [SerializeField] private float groundDrag;
    private float floorDrag;
	private bool speedControlActive = true;

	[SerializeField] private float normalJumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    private bool readyToJump = true;
	

	public Transform playerOrientation;
	Rigidbody playerRb;
	Vector3 moveDirection;

	float horizontalInput;
	float verticalInput;
	public float mouseWheelInput;
	bool isMouseWheelScrolled;

    [Header("DoubleTapRecognitionOfSprintKey")]
	private float doubleTapTime = 0.7f;  // Max Zeit zwischen zwei Tastenanschlägen
	private float lastTapTime = -1f;
	private int tapCount = 1;

	private bool isHoldingSprint = false;
	private bool wasDoubleTapped = false;

	[Header("IceSprint")]
	[SerializeField] private bool iceSprintIsUnlocked;
	[SerializeField] private float iceMoveSpeed;
	[SerializeField] private int waterConsumptionIceSprint;
	[SerializeField] private float WaterConsumptionIceSprintIntervallInSeconds;
	private float iceSprintWaterConsumptionTimer = 0f;

	[Header("Dash")]
	[SerializeField] private bool dashIsUnlocked;
	[SerializeField] private int maxDashCount = 3;
	[SerializeField] private int dashCount = 1;
	[SerializeField] private float dashCountResetIntervallInSeconds;
	[SerializeField] private float dashForce = 18;
	[SerializeField] private float timeUntilSpeedControlGetsActivatedAgain = 0.5f;
	[SerializeField] private int waterConsumptionDash = 2;
	private bool doDash;

	[Header("MultiJumpSkill")]
	[SerializeField] private int jumpCount;
	[SerializeField] private float extraJumpForce;
	private int jumpsLeft;
	[SerializeField] private int waterConsumptionExtraJump;

	[Header("FlyingSkill")]
	[SerializeField] private bool flyingIsUnlocked;
	[SerializeField] private float timeUntilFlyingAfterJump;
	[SerializeField] private float jetpackFlyForce;
	[SerializeField] private int waterConsumptionFlying;
	[SerializeField] private float waterConsumptionFlyingIntervallInSeconds;
	private bool readyToFly = true;
	private float flyWaterConsumptionTimer = 0f;
	//[SerializeField] private float jetpackFlyTimeMax;
	//private float FlyTimeLeft;

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
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode dashKey = KeyCode.LeftAlt;


    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    private bool grounded;

    [Header("Standing in Water Check/Refill")]
    [SerializeField] private LayerMask whatIsWater;
    [SerializeField] private int standingInWaterTankFillAmount;
    [SerializeField] private float tankFillRateInSeconds;
    private bool fillWater;

	WeaponBehaviour weaponBehaviourSkript;
    MainPlant mainPlantSkript;

	[Header("Antitoxin-Interaction")]
	[SerializeField] private float interactionRange;
	private bool inInteractionRangeWithPlant;

	#endregion Variablen

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

		StatsManager.OnStatsChanged.Invoke(StatsManager.Instance); //Abgleichevent callen statt die RefreshStats Funktion nochmal zu kopieren

		floorDrag = groundDrag;
		walkSpeed = moveSpeed;
		sprintSpeed = walkSpeed * normalSprintMultiplier;
		//FlyTimeLeft = jetpackFlyTimeMax;
	}

	private void RefreshStats(StatsManager stats)
    {
		#region set stats
		//Movement
		moveSpeed = StatsManager.Instance.stats.moveSpeed;
		normalSprintMultiplier = StatsManager.Instance.stats.sprintMultiplier;
		groundDrag = StatsManager.Instance.stats.groundDrag;
		normalJumpForce = StatsManager.Instance.stats.normalJumpForce;
		jumpCooldown = StatsManager.Instance.stats.jumpCooldown;
		airMultiplier = StatsManager.Instance.stats.airMultiplier;
		//Ice-Sprint
		iceSprintIsUnlocked = StatsManager.Instance.stats.iceSprintIsUnlocked;
		iceMoveSpeed = StatsManager.Instance.stats.iceMoveSpeed;
		waterConsumptionIceSprint = StatsManager.Instance.stats.waterConsumptionIceSprint;
		WaterConsumptionIceSprintIntervallInSeconds = StatsManager.Instance.stats.waterConsumptionIceSprintIntervallInSeconds;
		//Dash
		dashIsUnlocked = StatsManager.Instance.stats.dashIsUnlocked;
		maxDashCount = StatsManager.Instance.stats.maxDashCount;
		dashCount = StatsManager.Instance.stats.dashCount;
		dashCountResetIntervallInSeconds = StatsManager.Instance.stats.dashCountResetIntervallInSeconds;
		dashForce = StatsManager.Instance.stats.dashForce;
		timeUntilSpeedControlGetsActivatedAgain = StatsManager.Instance.stats.timeUntilSpeedControlGetsActivatedAgain;
		waterConsumptionDash = StatsManager.Instance.stats.waterConsumptionDash;
		//Multi-Jump
		jumpCount = StatsManager.Instance.stats.jumpCount;
		waterConsumptionExtraJump = StatsManager.Instance.stats.waterConsumptionExtraJump;
		//Flying
		flyingIsUnlocked = StatsManager.Instance.stats.flyingIsUnlocked;
		timeUntilFlyingAfterJump = StatsManager.Instance.stats.timeUntilFlyingAfterJump;
		jetpackFlyForce = StatsManager.Instance.stats.jetpackFlyForce;
		waterConsumptionFlying = StatsManager.Instance.stats.waterConsumptionFlying;
		waterConsumptionFlyingIntervallInSeconds = StatsManager.Instance.stats.waterConsumptionFlyingIntervallInSeconds;
		//Water-Stats
		standingInWaterTankFillAmount = StatsManager.Instance.stats.standingInWaterTankFillAmount;
		tankFillRateInSeconds = StatsManager.Instance.stats.tankFillRateInSeconds;
		//Antitoxin-Interaction
		interactionRange = StatsManager.Instance.stats.interactionRange;
		#endregion stats
	}

	private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
		// is MainPlant in Range for Interaction
		inInteractionRangeWithPlant = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, interactionRange) && hit.collider.CompareTag("Plant");

		MyInput();

		if (speedControlActive)
		{
			SpeedControl();
		}
			


		//handle drag
		if (grounded && doDash)
		{
			playerRb.linearDamping = 0f;
		}
		else if (grounded)
        {
			playerRb.linearDamping = groundDrag;
		}
		else if (!grounded && doDash)
		{
			playerRb.linearDamping = 0.5f;
		}
        else
        {
            playerRb.linearDamping = 0f;
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
			Dash();
        }
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        mouseWheelInput = Input.GetAxis("Mouse ScrollWheel");

		GetMouseInput();

		SprintButtonInput();

		JumpButtonInput();

		if (Input.GetKeyDown(dashKey) && dashCount > 0 && !doDash && dashIsUnlocked && !GameManager.Instance.gameOver) // wird in Fixed Update ausgeführt
		{
			dashCount--;
			StatsManager.Instance.SetDashCount(dashCount);
			WaterTank.Instance.UseTankWater(waterConsumptionDash);
			doDash = true;
			StartCoroutine(ResetDashCoroutine());
		}


		if (Input.GetKeyDown(weaponModeChangeKey) || isMouseWheelScrolled && !GameManager.Instance.gameOver)
        {
            weaponBehaviourSkript.SwitchWeaponMode();
		}

        if (Input.GetKeyDown(interactionKey) && inInteractionRangeWithPlant && GameManager.Instance.resources.antitoxin > 0 && !GameManager.Instance.gameOver) //Pflanze entgiften
		{
			GameManager.Instance.LooseAntitoxin();
			mainPlantSkript.DetoxPlant();
		}
    }
	#region Movement
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
	private void SprintButtonInput()
    {
		// Tastenanschlag registrieren
		if (Input.GetKeyDown(sprintKey))
		{
			float currentTime = Time.time;

			if (currentTime - lastTapTime <= doubleTapTime)
			{
				tapCount++;
			}
			else
			{
				tapCount = 1;
			}

			lastTapTime = currentTime;

			if (tapCount == 2 && iceSprintIsUnlocked)
			{
				wasDoubleTapped = true;
			}
			else
			{
				wasDoubleTapped = false;
				StatsManager.Instance.SetGroundDrag(floorDrag);
				StatsManager.Instance.SetSpeed(walkSpeed);
				isHoldingSprint = true;
			}
		}

		
		if (Input.GetKeyUp(sprintKey) && isHoldingSprint) // Taste losgelassen
		{
			isHoldingSprint = false;
			tapCount = 1;
			StatsManager.Instance.SetSpeed(walkSpeed);
		}

		if (WaterTank.Instance.playerTankWaterLevel <= 0)
		{
			wasDoubleTapped = false;
			StatsManager.Instance.SetGroundDrag(floorDrag);
			StatsManager.Instance.SetSpeed(walkSpeed);
		}

		if (wasDoubleTapped && WaterTank.Instance.playerTankWaterLevel > 0) //Aktiviert Eis-Schlittern beim Doppeldrücken von Sprintknopf
		{
			Debug.Log("Doppelt gedrückt und aktiv");
			StatsManager.Instance.SetSpeed(iceMoveSpeed);
			int iceGroundDrag = 0;
			StatsManager.Instance.SetGroundDrag(iceGroundDrag);
			IceSprintWaterConsumption();
		}

		// Aktionen ausführen
		if (isHoldingSprint) //Aktiviert normalen Sprint beim Einfachen drücken und gedrückt halten vom Sprintknopf
		{
			Debug.Log("Einmal gedrückt + gehalten");
			StatsManager.Instance.SetSpeed(sprintSpeed);// Hier deine Logik für Einfach-Drücken + Halten
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
			Invoke("ResetFly", timeUntilFlyingAfterJump);
		}
		else if (Input.GetKeyDown(jumpKey) && readyToJump && jumpsLeft > 0 && WaterTank.Instance.playerTankWaterLevel > 0 && !GameManager.Instance.gameOver) //Sprünge in der Luft
		{
			readyToJump = false;
			readyToFly = false;
			float jumpPower = extraJumpForce;
			jumpsLeft -= 1;

            WaterTank.Instance.UseTankWater(waterConsumptionExtraJump);

			Jump(jumpPower);

			Invoke("ResetJump", jumpCooldown);
			Invoke("ResetFly", timeUntilFlyingAfterJump);
		}
		else if (Input.GetKey(jumpKey) && readyToJump && readyToFly && flyingIsUnlocked && jumpsLeft == 0 && WaterTank.Instance.playerTankWaterLevel > 0 && !GameManager.Instance.gameOver) //Fly with Jetpack (&& FlyTimeLeft > 0)
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
	private void Dash()
	{
		if (doDash)
		{
			StartCoroutine(DashCoroutine());
		}
	}
	IEnumerator DashCoroutine()
	{
		speedControlActive = false;
		playerRb.AddForce(moveDirection.normalized * dashForce, ForceMode.Impulse);
		yield return new WaitForSeconds(timeUntilSpeedControlGetsActivatedAgain);
		doDash = false;
		speedControlActive = true;
	}
	IEnumerator ResetDashCoroutine()
	{
		if (dashCount < maxDashCount)
		{
			yield return new WaitForSeconds(dashCountResetIntervallInSeconds);
			dashCount++;
			StatsManager.Instance.SetDashCount(dashCount);
		}
		yield return null;
	}
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(playerRb.linearVelocity.x, 0f, playerRb.linearVelocity.z);

        //limit move velocity if needed
        if (flatVel.magnitude > 1.5f * moveSpeed) // Aufs 1,5 facche gesetzt , um Dash zu ermöglichen
        {
			Vector3 limitedVel = flatVel.normalized * (1.5f * moveSpeed);
            playerRb.linearVelocity = new Vector3(limitedVel.x, playerRb.linearVelocity.y, limitedVel.z);
        }
    }
    private void Jump(float jumpPower)
    {
        //reset y velocity
        playerRb.linearVelocity = new Vector3(playerRb.linearVelocity.x, 0f, playerRb.linearVelocity.z);

        playerRb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
    }
	private void ResetJump()
	{
		readyToJump = true;
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
        if (flyWaterConsumptionTimer >= waterConsumptionFlyingIntervallInSeconds) //Nach Ablauf einer Sekunde...
        {
			WaterTank.Instance.UseTankWater(waterConsumptionFlying);
			flyWaterConsumptionTimer = 0f;
		}
		
	}
	private void ResetFly()
	{
		readyToFly = true;
	}
	private void IceSprintWaterConsumption()
	{
		iceSprintWaterConsumptionTimer += Time.deltaTime;
		if (iceSprintWaterConsumptionTimer >= WaterConsumptionIceSprintIntervallInSeconds) //Nach Ablauf einer Sekunde...
		{
			WaterTank.Instance.UseTankWater(waterConsumptionIceSprint);
			iceSprintWaterConsumptionTimer = 0f;
		}

	}
	#endregion Movement

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
			if (WaterTank.Instance.playerTankWaterLevel < WaterTank.Instance.playerTankMaxWaterLevel)
			{
				WaterTank.Instance.FillTank(tankFillAmount);
			}
			
			yield return new WaitForSeconds(tankFillRateInSeconds);
		}
    }
    void CheckPickUpType(Collider other)
    {
		if (other.gameObject.GetComponent<PickUp>().pickUpType == PickUpType.antitoxin) //Antitoxin-Check
		{
			GameManager.Instance.GetAntitoxin();
			Destroy(other.gameObject);
		}
		else if (other.gameObject.GetComponent<PickUp>().pickUpType == PickUpType.resource1)
		{
			GameManager.Instance.GetResource1();
			other.gameObject.SetActive(false);
		}
		else if (other.gameObject.GetComponent<PickUp>().pickUpType == PickUpType.resource2)
		{
			GameManager.Instance.GetResource2();
			other.gameObject.SetActive(false);
		}
	}
}