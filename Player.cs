using UnityEngine;
using System.Collections;

public class Player : PhysicsObject {

	//Public Variables for the Inspector
	public float horizontalSpeed = 3f, jumpSpeed = 10f, gravitySpeed = 15f, jumpAscensionTimer = .5f;

	private int inputHorizontal, inputVertical;// Horizontal: 1 = Right, -1 = Left, 0 = nothing. Vertical: 2 = Jump, 1 = Ascend, 0 = Nothing/Normal Gravity, -1 = FallFast, -2 = Slam
	private bool jumping, fallingFast, vertPressedDown;//is the player currently jumping? Is the player currently fallingfast? Did we just receive vertical input?
	


	// Use this for initialization
	void Start () {
		PlayerInitialize();
	}
	
	// Update is called once per frame
	void Update () {
		PlayerInput();

	}

	void FixedUpdate()
	{
		PhysicsObject_Move(inputHorizontal, inputVertical, true);
	}

	/// <summary>
	/// Set the Values Required for the Physics Object
	/// </summary>
	private void PlayerInput()
	{
		#region Variable Declaration
		//Determine the current value of the horizontal input and the vertical input.
		float horX = Input.GetAxis("Horizontal");
		float vertY = Input.GetAxis("Vertical");
		
		//Determine whether or not the horizontal and vertical input buttons are being held down.
		bool vertYHeld = Input.GetButton("Vertical");
		bool horXHeld = Input.GetButton("Horizontal");

		//Debug.Log("Keyboard Input for Horizontal Axis: " + horX);
		//Debug.Log("Keyboard Input for Vertical Axis: " + vertY);

		#endregion

		#region Horizontal Input
		//get horizontal input

		//If my horizontal input is not 0, and the input button is being held down.
		//Check for held, because if not, the player will briefly coast since the horX value takes time to go back to 0.
		if (horX != 0 && horXHeld)
		{
			inputHorizontal = Mathf.RoundToInt(Mathf.Sign(horX));
		}
		else
		{
			inputHorizontal = 0;
		}

		#endregion

		#region Vertical Input
		//get vertical input

		//determine if the jump button has been released
		if (Input.GetButtonUp("Vertical"))
		{
			//the jump button has been released/is no longer held,

			//Determine if the player was jumping or fast falling
			if (vertY > 0)
			{
				jumping = false;
			}
			else if (vertY < 0)
			{
				fallingFast = false;
			}

			//Debug.Log("Vertical Button has been released: " + vertY);
		}

		//Determine if the jump button was pressed
		if (Input.GetButtonDown("Vertical"))
		{
			//The jump button has been pressed. Determine if the player wants to fall faster, or begin jumping
			if (vertY > 0)
			{
				jumping = true;
				vertPressedDown = true;
				inputVertical = 2;
			}
			else if (vertY < 0)
			{
				fallingFast = true;
				vertPressedDown = true;
				inputVertical = -2;
			}

			//Debug.Log("Vertical Button has been pressed: " + vertY);
		}

		//Determine if the player is jumping higher or falling faster

		//if the vertical input is not 0, and we have not just started jumping or falling fast and the player is still holding the vertical input.
		if (vertY != 0 && !vertPressedDown && vertYHeld)
		{
			if (vertY > 0)
			{
				inputVertical = 1;
			}
			else if (vertY < 0)
			{
				inputVertical = -1;
			}

			//Debug.Log("Vertical Button has been Held: " + vertY);
		}
		//The player is giving no input for vertical movement.
		else if (/*vertY == 0 &&*/ !vertPressedDown && !vertYHeld)
		{
			inputVertical = 0;
		}

		vertPressedDown = false;

		#endregion
	}

	private void PlayerInitialize()
	{
		PhysicsObjectInitialize();
		Speed_Horizontal = horizontalSpeed;
		Speed_Gravity = gravitySpeed;
		Speed_Jump = jumpSpeed;
		JumpAmount = 1;
		JumpAscensionTimer = jumpAscensionTimer;
		CanJump = true;
	}
		
}
