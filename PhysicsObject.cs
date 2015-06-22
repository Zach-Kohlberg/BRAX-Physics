using UnityEngine;
using System.Collections;

public class PhysicsObject : MonoBehaviour {

    public LayerMask collision_layerMask;

    private float speed_Horizontal = 1, speed_Jump, speed_gravity;
    private Vector2 velocity, size, center;
    private Collider2D collider;
    public bool ground = true;//am I on the ground or falling to it? Determine which direction Horizontal rays need to be drawn.
    private bool sideCollision = false;

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

    void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    public void PhysicsObject_Initialize()
    {
        size = collider.bounds.size;
        center = collider.bounds.center;
        Debug.Log("Size: " + size);
        Debug.Log("Center: " + center);
        Debug.Log("Extents: " + collider.bounds.extents);
        Debug.Log("Min: " + collider.bounds.min);
        Debug.Log("Max: " + collider.bounds.max);
        
    }

    void Update()
    {
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
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            PhysicsObject_Move(-1, 0);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            PhysicsObject_Move(1, 0);
        }
        
    }

    public void PhysicsObject_Move(float moveX,float moveY, bool collide = true)
    {
        //if we want to check for collisions then
        if (collide)
        {
            float delta_X = 0, delta_Y = 0;
            //find center
            center = collider.bounds.center;


            //determine if horizontal movement.
            if (moveX != 0)
            {
                delta_X = Collision_Horizontal(moveX);
            }
            //determine if vertical movement



            transform.Translate(delta_X, delta_Y, 0, Space.Self);

        }
            //since we are not checking for collisions, just move.
        else
        {
            transform.Translate(new Vector2(moveX * Speed_Horizontal, moveY) * Time.deltaTime, Space.Self);
        }
    }

    private float Collision_Horizontal(float x)
    {
        sideCollision = false;
        float moveTime = Time.deltaTime;
        float ray_Range;//range we will be using for the raycast
        //determine left or right
        float direction_Hor = Mathf.Sign(x);
        //cast more rays if in the air, than if on the ground.
        float i = 0;
        if (ground)
        {
            //i = 0;
            ray_Range = size.y;
        }
        else
        {
            //i = -0.05f;
            ray_Range = size.y * 1.01f;//1.01 determines how far we check above/below the collider.
        }
        for (; i <= ray_Range; i+= ray_Range / 4f )
        {
            //set x position of ray origin
            float ray_X;
            if (direction_Hor > 0) { ray_X = collider.bounds.max.x; } else { ray_X = collider.bounds.min.x; }

            float ray_Y;
            //if ground, nothing special
            if (ground)
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
            RaycastHit2D hit = Physics2D.Raycast(rayHorizontal.origin,rayHorizontal.direction,Mathf.Abs(speed_Horizontal * moveTime), collision_layerMask);
            Debug.DrawRay(rayHorizontal.origin, rayHorizontal.direction);
            //Debug.Log("I: " + ray_Y);

            if (hit.collider != null)
            {
                Debug.DrawRay(rayHorizontal.origin,rayHorizontal.direction,Color.red);
                sideCollision = true;
                Debug.Log("Collided with " + hit.collider.transform.name + " on layer " + hit.collider.transform.gameObject.layer);
                return 0;
            }
        }

        return speed_Horizontal * moveTime * direction_Hor;
    }
}
