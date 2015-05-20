using UnityEngine;
using System.Collections;

public class LevelObject : MonoBehaviour {
    
	public float X {
		get { return transform.position.x; }
		set { transform.position = new Vector3(value, transform.position.y, transform.position.z); }
	}
	public float Y {
		get { return transform.position.y; }
		set { transform.position = new Vector3(transform.position.x, value, transform.position.z); }
	}
	public float Z {
		get { return transform.position.z; }
		set { transform.position = new Vector3(transform.position.x, transform.position.y, value); }
	}
	public Vector3 Position {
		get { return transform.position; }
		set { transform.position = value; }
	}
    
    protected void LevelObjectInit() {
    	
    }
}
