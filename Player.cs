using UnityEngine;
using System.Collections;

public class Player : LevelObject
{

    #region Variables

    /**
     * Inspector's Variables
     */
    public float _gravity, _maxHSpeed, _hAccel, _maxVSpeed, _vAccel;
    

    /**
     * These variables track movement
     */
    private Vector2 velocity;
    

    /**
     * These variables govern movement
     */
    private float gravity, maxHSpeed, hAccel, maxVSpeed, vAccel;
    

    /**
     * These variables track the object's state and deterine which actions it can currently perform
     */
    private int jump_max, jump_current;
    private float jump_duration, jump_start;//the amount of time the jump button can be held, when the jump began.


    #endregion


    

    private void Awake() {
        LevelObjectInit();
        PlayerInit();
    }
    
	/**
     * Initialize object
     */
	protected void PlayerInit() {
		gravity = _gravity;
		maxHSpeed = _maxHSpeed;
		hAccel = _hAccel;
		maxVSpeed = _maxVSpeed;
		vAccel = _vAccel;
    }
    

    private void Start() {
        
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
    	
    }
    
    private void Update() {
        PlayerMovement();
    }


    /**
     * Perform all movement interactions. Called from the Update function
     */
    private void PlayerMovement()
    {
        //get input
        float horX = Input.GetAxis("Horizontal"), vertY = Input.GetAxis("Vertical");


        //modify the velocity vector
        //apply the horizontal input
        velocity.x = Mathf.Clamp(velocity.x + horX * hAccel, -maxHSpeed, maxHSpeed);

        //velocity.y = Mathf.Clamp(velocity.y + vertY * vAccel, gravity, maxVSpeed);


        //raycast check to see if we would collide with something
        if (velocity.x != 0){
            Debug.Log("Player:PlayerMovement: The X Velocity is not 0");
            float raycastOriginX = gameObject.GetComponent<Collider2D>().bounds.extents.x * velocity.x / Mathf.Abs(velocity.x);
            float raycastOriginY = gameObject.GetComponent<Collider2D>().bounds.extents.y;

            RaycastHit2D xTop = Physics2D.Raycast(new Vector2(raycastOriginX, raycastOriginY), new Vector2(velocity.x,0f), Mathf.Abs(velocity.x));
            RaycastHit2D xCenter = Physics2D.Raycast(new Vector2(raycastOriginX, 0), new Vector2(velocity.x, 0f), Mathf.Abs(velocity.x));
            RaycastHit2D xBottom = Physics2D.Raycast(new Vector2(raycastOriginX, -raycastOriginY), new Vector2(velocity.x, 0f), Mathf.Abs(velocity.x));

            //draw the rays for testing purposes.
            Debug.DrawRay(new Vector2(raycastOriginX, raycastOriginY), new Vector2(velocity.x, 0f), Color.red, 1f, false);
            Debug.DrawRay(new Vector2(raycastOriginX, 0), new Vector2(velocity.x, 0f), Color.black, 1f,false);
            Debug.DrawRay(new Vector2(raycastOriginX, -raycastOriginY), new Vector2(velocity.x, 0f), Color.white, 1f,false);

            Debug.Log("Player:PlayerMovement: Raycast Section Complete");
        }

        //Physics2D.Raycast()
        //if no collision, move in that direction
    }
}
