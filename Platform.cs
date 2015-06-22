using UnityEngine;
using System.Collections;

public class Platform : PhysicsObject {

    public float time_Switch, spd_h;
    private float time_SwitchUsed;
    private int dir;

	// Use this for initialization
	void Start () {
        dir = 1;
        //time_Switch = 3f;
        Speed_Horizontal = spd_h;
        time_SwitchUsed = 0;
        PhysicsObject_Initialize();
	}
	
	// Update is called once per frame
	void Update () {

        if (time_SwitchUsed < time_Switch)
        {
            PhysicsObject_Move(dir, 0, false);

            time_SwitchUsed += Time.deltaTime;
            if (time_SwitchUsed >= time_Switch)
            {
                time_SwitchUsed = 0;
                dir *= -1;
            }
        }
        
	}
}
