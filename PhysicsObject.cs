using UnityEngine;
using System.Collections;

public class PhysicsObject : MonoBehaviour {

    public LayerMask collision_layerMask;

    private float speed_Horizontal = 1, speedVertical = 1, speed_Jump, speed_gravity, timeToAscend, timeToAscendUsed;
	private float timeMove;//time since last fixedUpdate
	private int jumpAmountTotal, jumpAmountCur;//The total amount of times and current amount of times the player has jumped
    private Vector2 velocity, size, center;
    private Collider2D collider;
    private bool grounded = true;//am I on the ground or falling to it? Determine which direction Horizontal rays need to be drawn.
    private bool sideCollision = false;
	private bool canJump = false;

    public float Speed_Horizontal
    {
        get { return speed_Horizontal; }
        set { speed_Horizontal = value; }
    }

	public float Speed_Vertical
	{
		get { return speedVertical; }
		set{speedVertical = value; }
	}

    public float Speed_Jump
    {
        get { return speed_Jump; }
        set { speed_Jump = value; }
    }

    public float Speed_Gravity
    {
        get { return speed_gravity; }
        set { speed_gravity = value; }
    }

	public bool CanJump
	{
		get { return canJump; }
		set { canJump = value; }
	}

	public float JumpAscensionTimer
	{
		get { return timeToAscend; }
		set { timeToAscend = value; }
	}

	public int JumpAmount
	{
		get { return jumpAmountTotal; }
		set { jumpAmountTotal = value; }
	}

    void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    public void PhysicsObjectInitialize()
    {
        size = collider.bounds.size;
        center = collider.bounds.center;
		timeToAscendUsed = 0;
		//Debug.Log("Size: " + size);
		//Debug.Log("Center: " + center);
		//Debug.Log("Extents: " + collider.bounds.extents);
		//Debug.Log("Min: " + collider.bounds.min);
		//Debug.Log("Max: " + collider.bounds.max);
        
    }

    void Update()
    {
		//if (Input.GetKeyDown(KeyCode.Space))
		//{
		//	PhysicsObjectInitialize();
		//}
        //float hor_x = Input.GetAxis("Horizontal");
        //Debug.Log("Horizontal Input: " + hor_x);
        //if (hor_x > 0.01)
        //{
        //    PhysicsObject_Move(1, Input.GetAxis("Vertical"));
        //}
        //else if (hor_x < -0.01)
        //{
        //    PhysicsObject_Move(-1, Input.GetAxis("Vertical"));
        //}
        //else
        //{
        //    PhysicsObject_Move(0, Input.GetAxis("Vertical"));
        //}

		//if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
		//{
		//	Debug.Log("Left Input");
		//	PhysicsObject_Move(-1, 0);
		//}
		//if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
		//{
		//	Debug.Log("Right Input");
		//	PhysicsObject_Move(1, 0);
		//}
        
    }

    public void PhysicsObject_Move(int moveX,int moveY, bool collide = true)
    {
        //if we want to check for collisions then
        //Debug.Log("PhysicsObject_Move");

		//Set the amount of time since the last fixedupdate
		timeMove = Time.fixedDeltaTime;

		//If we should check for collisions
        if (collide)
        {
            //Debug.Log("Move with Collisions");
            float delta_X = 0, delta_Y = 0;
            //find center
            center = collider.bounds.center;

			//Determine whether or not we are touching the ground.
			IsTouchingGround();

            //determine if horizontal movement.
            if (moveX != 0)
            {
                delta_X = CollisionHorizontal(moveX);
            }

            //determine if vertical movement
			if (moveY  >= -2 && moveY <= 2)
			{
				delta_Y = CollisionVertical(moveY);
			}


			//move into position.
            transform.Translate(delta_X, delta_Y, 0, Space.Self);

        }
            //since we are not checking for collisions, just move.
        else
        {
            transform.Translate(new Vector2(moveX * Speed_Horizontal, moveY * Speed_Jump) * /*Time.deltaTime*/ timeMove, Space.Self);
        }
    }

    private float CollisionHorizontal(int x)
    {
		float distanceHorizontal = 0;
		if (x == 0)
		{
			return 0;
		}
		else
		{
			distanceHorizontal = speed_Horizontal * timeMove;
		}

        //Debug.Log("Collision_Horizontal");
        sideCollision = false;
        //float moveTime = Time.fixedDeltaTime;
        float rangeY;//range we will be using for the raycast

        //determine left or right
        float directionHorizontal = Mathf.Sign(x);

        //cast more rays if in the air, than if on the ground.
        //Debug.Log("Grounded Check");
        if (true)
        {
            //i = 0;
            rangeY = size.y;
        }
		//else
		//{
		//  //Come back and change this later
		//	//i = -0.05f;
		//	rangeY = size.y +.01f;//.01 determines how far we check above/below the collider.
		//}
        //Debug.Log("For Loop");
        //Debug.Log("Ray Range " + ray_Range);
		float i = 0;
        for (; i <= rangeY; i += rangeY / 3f)
        {
            //set x position of ray origin
            float originX;
            if (directionHorizontal > 0) { originX = collider.bounds.max.x; } else { originX = collider.bounds.min.x; }

            float originY;

            //if ground, nothing special
            if (true)
            {
                originY = collider.bounds.min.y + i + .01f;
            }
            //else check above and below the collider.
			//else
			//{
			//	//come back and change this later
			//	originY = collider.bounds.min.y + i;
			//}

            //perform the raycast

            Ray2D rayHorizontal = new Ray2D(new Vector2(originX, originY), new Vector2(directionHorizontal, 0));

            RaycastHit2D hit = Physics2D.Raycast(rayHorizontal.origin, rayHorizontal.direction, Mathf.Abs(distanceHorizontal), collision_layerMask);
            Debug.DrawRay(rayHorizontal.origin, rayHorizontal.direction);
            //Debug.Log("I: " + ray_Y);

            if (hit.collider != null)
            {
                Debug.DrawRay(rayHorizontal.origin, rayHorizontal.direction, Color.red);
                sideCollision = true;
                Debug.Log("Collided with " + hit.collider.transform.name + " on layer " + hit.collider.transform.gameObject.layer + " with ray with origin " + rayHorizontal.origin);

				distanceHorizontal = Mathf.Abs(hit.distance);
                //return 0;
            }
        }
        //Debug.Log("End For Loop");

		return distanceHorizontal * directionHorizontal;

        //return 0;
		//if (true)
		//{
		//	bool t = false;
							
		//}
    }

	private float CollisionVertical(int y)
	{
		//Declare variables
		float rangeX = size.x; //range of ray origins
		float directionVertical = 0;//are we moving up or down
		float distanceVertical = 0;

		bool defaultFall = false;// should we default to normal falling?

		//Check all possible jumping options and default to gravity falling if not grounded.
		#region Upwards
		//Jump / Ascend
		if (y > 0 && canJump)
		{
			//Ascend higher
			//If in the air, and the amount of ascension time used is less than it's max
			if (y == 1 && timeToAscendUsed < timeToAscend)
			{
				//Ascend
				//set the distance and decrease it based on time left.
				distanceVertical = (speed_Jump / 2) * (1 - (timeToAscendUsed / timeToAscend));
				
				//set the direction
				directionVertical = 1;

				//update the amount of time
				timeToAscendUsed += timeMove;
			}
			//We are not not able to ascend anymore.
			else if (y == 1 && timeToAscendUsed >= timeToAscend)
			{
				//gravity fall
				defaultFall = true;
			}

			//Initial Jump
			//If Jump Action and the current number of jumps is less than the total
			else if (y == 2 && jumpAmountCur < jumpAmountTotal)
			{
				//Jump
				//set the distance we will jump by.
				distanceVertical = speed_Jump;

				//set the direction we are moving in
				directionVertical = 1;

				//reset the amount of time used to ascend so that this jump can be of variable height as well.
				timeToAscendUsed = 0;

				//increase the amount of jumps used
				jumpAmountCur++;
			}

			//We are not able to jump anymore.
			else if (y == 2 && jumpAmountCur >= jumpAmountTotal)
			{
				//gravity fall
				defaultFall = true;
			}
		}
		//if upwards movement, and cant jump
		else if (y > 0 && !canJump)
		{
			distanceVertical = speedVertical;
		}
		#endregion
		#region Downwards
		//Make sure we are not currently grounded
		else if (!grounded && y < 0)
		{

			//downslam
			if (y == -2 && canJump)
			{
				//set the distance
				distanceVertical = speed_gravity * 3;

				//set the direction
				directionVertical = -1;

				//prevent the object from being able to ascend if they stopped
				timeToAscendUsed = timeToAscend;
			}
			//Can't downslam so default to normal falling
			else if (y == -2 && !canJump)
			{
				defaultFall = true;
			}
			//Fall Faster
			else if(y == -1 && canJump){
				//set the distance
				distanceVertical = speed_gravity * 2;

				//set the direction
				directionVertical = -1;

				//prevent the object from being able to ascend if they stopped
				timeToAscendUsed = timeToAscend;
			}
			//Can't fall faster, so default to normal falling
			else if(y == -1 && !canJump){
				defaultFall = true;
			}
			
		}

		//Fall if regular input or unable to perform requested action.
		if ((y == 0 || defaultFall) && !grounded)
		{
			//set the distance
			distanceVertical = speed_gravity;

			//set the direction
			directionVertical = -1;

			//prevent the object from being able to ascend if they stopped
			timeToAscendUsed = timeToAscend;
		}

		#endregion

		#region Raycasting for Collisions
		
		//prepare the distance
		distanceVertical *= timeMove;

		

		//create a variable distance in case we hit a collision early, but it's not the closest.
		//float maxDist = distanceVertical;

		if (distanceVertical != 0)
		{

			float originY;
			float i = 0;
			for (; i <= size.x; i += (size.x / 3f))
			{
				float originX = collider.bounds.min.x + i;


				//we are moving upwards
				if (directionVertical == 1)
				{
					originY = collider.bounds.max.y;
				}
				//we are moving down
				else
				{
					originY = collider.bounds.min.y;
				}

				//create the ray
				Ray2D rayVertical = new Ray2D(new Vector2(originX, originY), new Vector2(0, directionVertical));

				//prepare for a hit
				RaycastHit2D hit = Physics2D.Raycast(rayVertical.origin, rayVertical.direction, Mathf.Abs( distanceVertical), collision_layerMask);

				//draw the ray on the screen.
				Debug.DrawRay(rayVertical.origin, rayVertical.direction, Color.green);

				//if we hit something...
				if (hit.collider != null)
				{
					Debug.DrawRay(rayVertical.origin, rayVertical.direction, Color.red);
					Debug.Log(name + " Vertically collided with " + hit.collider.transform.name + " on layer " + hit.collider.transform.gameObject.layer);

					//update the distance with the new shortened distance
					//This way, if we tried to jump and our jump would have brought us to height of 8, but we hit something at 6.5, we will now only go to 6.5. This will repeat within the for loop until the maximum distance we can move is found.

					distanceVertical = Mathf.Abs(hit.distance);
					//distanceVertical = hit.distance -.01f;
					
					//return 0;
				}
			}
		}
		


		#endregion
		return distanceVertical * directionVertical;
	}



	/// <summary>
	/// Return whether or not the object is touching the ground.
	/// </summary>
	/// <returns></returns>
	private bool IsTouchingGround()
	{
		//perform raycasts toward the ground

		//start at left and go right.

		float y = collider.bounds.min.y + .01f;
		float i = 0;
		for (; i < size.x; i += (size.x / 3f))
		{
			float rayX = collider.bounds.min.x + i;

			Ray2D rayDown = new Ray2D(new Vector2(rayX, y), new Vector2(0, -1));
			RaycastHit2D hit = Physics2D.Raycast(rayDown.origin, rayDown.direction, .01f, collision_layerMask);
			Debug.DrawRay(rayDown.origin, rayDown.direction, Color.white);

			if (hit.collider != null)
			{
				Debug.DrawRay(rayDown.origin, rayDown.direction, Color.red);
				Debug.Log(name + " Is currently grounded and on top of " + hit.collider.transform.name + " on layer " + hit.collider.transform.gameObject.layer);
				grounded = true;

				//reset the amount of jumps
				jumpAmountCur = 0;
				return true;
			}
		}
		//I did not hit anything, so I must not be on the ground.
		grounded = false;
		return false;
	}

}
