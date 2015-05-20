using UnityEngine;
using System.Collections;

public class Player : LevelObject {
    
    public float gravity, maxHSpeed, hAccel, maxVSpeed, vAccel;
    
    /**
     * These variables track movement
     */
    private Vector2 _velocity;
    
    /**
     * These variables govern movement
     */
    private float _gravity, _maxHSpeed, _hAccel, _maxVSpeed, _vAccel;
    
    /**
     * These variables track the object's state and deterine which actions it can currently perform
     */
    
    private void Awake() {
        LevelObjectInit();
        PlayerInit();
    }
    
	/**
     * Initialize object
     */
	protected void PlayerInit() {
		_gravity = gravity;
		_maxHSpeed = maxHSpeed;
		_hAccel = hAccel;
		_maxVSpeed = maxVSpeed;
		_vAccel = vAccel;
    }
    
    private void Start() {
        
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
    	
    }
    
    private void Update() {
        
    }
}
