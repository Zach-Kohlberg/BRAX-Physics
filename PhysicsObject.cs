using UnityEngine;
using System.Collections;

public class PhysicsObject : MonoBehaviour {

    public LayerMask collision_layerMask;

    private float speed_Horizontal = 1, speed_Jump, speed_gravity, timeToAscend, timeToAscendUsed;
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
		set { timeToAscendUsed = value; }
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

		
        if (collide)
        {
            //Debug.Log("Move with Collisions");
            float delta_X = 0, delta_Y = 0;
            //find center
            center = collider.bounds.center;


            //determine if horizontal movement.
            if (moveX != 0)
            {
                delta_X = CollisionHorizontal(moveX);
            }
            //determine if vertical movement



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
        //Debug.Log("Collision_Horizontal");
        sideCollision = false;
        //float moveTime = Time.fixedDeltaTime;
        float ray_Range;//range we will be using for the raycast
        //determine left or right
        float direction_Hor = Mathf.Sign(x);
        //cast more rays if in the air, than if on the ground.
        float i = 0;
        //Debug.Log("Grounded Check");
        if (grounded)
        {
            //i = 0;
            ray_Range = size.y;
        }
        else
        {
            //i = -0.05f;
            ray_Range = size.y * 1.01f;//1.01 determines how far we check above/below the collider.
        }
        //Debug.Log("For Loop");
        //Debug.Log("Ray Range " + ray_Range);
        for (; i <= ray_Range; i += ray_Range / 4f)
        {
            //set x position of ray origin
            float ray_X;
            if (direction_Hor > 0) { ray_X = collider.bounds.max.x; } else { ray_X = collider.bounds.min.x; }

            float ray_Y;
            //if ground, nothing special
            if (grounded)
            {
                ray_Y = collider.bounds.min.y + i;
            }
            //else check above and below the collider.
            else
            {
                ray_Y = collider.bounds.max.y + ((ray_Range - size.y) / 4f) - i;
            }

            //perform the raycast

            Ray2D rayHorizontal = new Ray2D(new Vector2(ray_X, ray_Y), new Vector2(direction_Hor, 0));
            RaycastHit2D hit = Physics2D.Raycast(rayHorizontal.origin, rayHorizontal.direction, Mathf.Abs(speed_Horizontal * timeMove), collision_layerMask);
            Debug.DrawRay(rayHorizontal.origin, rayHorizontal.direction);
            //Debug.Log("I: " + ray_Y);

            if (hit.collider != null)
            {
                Debug.DrawRay(rayHorizontal.origin, rayHorizontal.direction, Color.red);
                sideCollision = true;
                Debug.Log("Collided with " + hit.collider.transform.name + " on layer " + hit.collider.transform.gameObject.layer);
                return 0;
            }
        }
        //Debug.Log("End For Loop");

        return speed_Horizontal * timeMove * direction_Hor;

        //return 0;
		//if (true)
		//{
		//	bool t = false;
							
		//}
    }

	private float CollisionVertical(int y)
	{
		//Declare variables
		float rayRange;//range of ray origins
		float directionVertical;//are we moving up or down

		//Determine if we can jump.
		

		//Jump / Ascend
		if (y > 0)
		{
			//Ascend higher
			//If in the air, and the amount of ascension time used is less than it's max
			if (y == 1 && timeToAscendUsed < timeToAscend)
			{
				
			}
			//Initial Jump
			//If Jump Action and the current number of jumps is less than the total
			else if (y == 2 && jumpAmountCur < jumpAmountTotal)
			{

			}
		}
		//fall fast
		else if (y < 0)
		{

		}
		//Fall
		else if (y == 0)
		{
			//reset the amount of time used to ascend
			timeToAscendUsed = 0;
		}

		return 0;
	}



	/// <summary>
	/// Return whether or not the object is touching the ground.
	/// </summary>
	/// <returns></returns>
	private bool IsTouchingGround()
	{
		//perform raycasts toward the ground

		//start at left and go right.

		float xRange = size.x; float y = collider.bounds.min.y;
		float i = 0;
		for (; i < xRange; i += xRange / 4)
		{


			Ray2D rayDown = new Ray2D(new Vector2(i, y), new Vector2(0, -1));
			RaycastHit2D hit = Physics2D.Raycast(rayDown.origin, rayDown.direction, .001f, collision_layerMask);
			Debug.DrawRay(rayDown.origin, rayDown.direction, Color.white);

			if (hit.collider != null)
			{
				Debug.DrawRay(rayDown.origin, rayDown.direction, Color.red);
				Debug.Log(name + " Is currently on top of " + hit.collider.transform.name + " on layer " + hit.collider.transform.gameObject.layer);
				grounded = true;
				return true;
			}
		}
		//I did not hit anything, so I must not be on the ground.
		grounded = false;
		return false;
	}

}
