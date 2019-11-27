using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDragging : MonoBehaviour
{
    public float maxStretch = 3.0f;
	public LineRenderer catapultLineFront;
	public LineRenderer catapultLineBack;
	public GameObject trajectoryDotPrefab;
	public GameObject resetter;	

	private SpringJoint2D spring;
	private Transform catapult;
	private Ray rayToMouse;
	private Ray leftCatapultToProjectile;
	private float maxStretchSqr;
	private float circleRadius;
	private Vector2 prevVelocity;
	private bool clickedOn;

	private Vector2 Gravity;
	private int numDotToShow = 20;
	private GameObject[] trajectoryDots = new GameObject[20];
	private float dotTimeStep = 0.10f;
	private bool launched;
	private bool soundOn;


	void Awake(){
		spring = GetComponent <SpringJoint2D>();
		catapult = spring.connectedBody.transform;
		Gravity = Physics2D.gravity;
		launched = false;
	}

    void Start()
    {
        rayToMouse = new Ray(catapult.position, Vector3.zero);
        leftCatapultToProjectile = new Ray(catapultLineFront.transform.position, Vector3.zero);
        maxStretchSqr = maxStretch*maxStretch;
        CircleCollider2D circle = GetComponent<Collider2D>() as CircleCollider2D;
        circleRadius = circle.radius;
        soundOn = true;
    }

    void Update()
    {
    	if(!launched) return;
    	if (spring != null && Input.GetMouseButtonDown(0) &&  isOnMe())
            OnMouseDown();
        if (spring != null && clickedOn && Input.GetMouseButtonUp(0))
        	OnMouseUp();

        if(clickedOn){
        	Dragging();
        }
        if(spring != null){
        	if(!GetComponent<Rigidbody2D>().isKinematic && prevVelocity.sqrMagnitude > GetComponent<Rigidbody2D>().velocity.sqrMagnitude){
        		Destroy(spring);
        		GetComponent<Rigidbody2D>().velocity = prevVelocity;
        	}

        	if(!clickedOn){
        		prevVelocity = GetComponent<Rigidbody2D>().velocity;
        	}

        	LineRendererUpdate();
        }
        else{
        	catapultLineFront.enabled = false;
        	catapultLineBack.enabled = false;
        }
    }

    void LineRendererSetUp(){
    	catapultLineFront.enabled = true;
        catapultLineBack.enabled = true;
    	catapultLineFront.SetPosition(0, catapultLineFront.transform.position);
    	catapultLineBack.SetPosition(0, catapultLineBack.transform.position);

    	catapultLineBack.sortingLayerName = "Foreground";
    	catapultLineBack.sortingLayerName = "Foreground";

    	catapultLineFront.sortingOrder = 2;
    	catapultLineBack.sortingOrder = 5;
    }

    void OnMouseDown(){
    	spring.enabled = false;
    	clickedOn = true;
    }

    void OnMouseUp(){	
    	spring.enabled = true;
    	GetComponent<Rigidbody2D>().isKinematic = false;
    	clickedOn = false;
    }

    void Dragging(){
    	Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    	Vector2 catapultToMouse = mouseWorldPoint - catapult.position;

    	for(int i = 0; i < numDotToShow; ++i){
    		Destroy(trajectoryDots[i]);
    		trajectoryDots[i] = Instantiate(trajectoryDotPrefab);
    		trajectoryDots[i].transform.position = calculatePosition(dotTimeStep * i);
    	}

    	if (catapultToMouse.sqrMagnitude > maxStretchSqr){
    		rayToMouse.direction = catapultToMouse;
    		mouseWorldPoint = rayToMouse.GetPoint(maxStretch);
    	}

    	mouseWorldPoint.z = 0f;
    	transform.position = mouseWorldPoint;
    }

    void LineRendererUpdate(){
    	Vector2 catapultToProjectile = transform.position - catapultLineFront.transform.position;
    	leftCatapultToProjectile.direction = catapultToProjectile;
    	Vector3 holdPoint = leftCatapultToProjectile.GetPoint(catapultToProjectile.magnitude + circleRadius);
    	catapultLineFront.SetPosition(1, holdPoint);
    	catapultLineBack.SetPosition(1, holdPoint);
    }

    bool isOnMe(){
    	if(!launched) return false;
    	Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    	Vector2 asteroidToMouse = transform.position - mouseWorldPoint;

    	if (asteroidToMouse.sqrMagnitude < circleRadius*circleRadius){
    		return true;
    	}
    	return false;
    }

    Vector2 calculatePosition(float elapsedTime){
    	Vector2 position = new Vector2(transform.position.x, transform.position.y);
    	return  Gravity * elapsedTime * elapsedTime * 0.5f + position + forecastVelocity() * elapsedTime;
    }

    Vector2 forecastVelocity(){
    	float powerMultiplier = 9f;
    	float radianAngle = calculateAngle();
	
    	float distance = Vector2.Distance(transform.position, catapult.position);

        float vel = distance * powerMultiplier;

        float xVel = vel * Mathf.Cos(radianAngle);
        float yVel = vel * Mathf.Sin(radianAngle);

        return new Vector2(xVel, yVel);
    }

    public float calculateAngle()
    {
        Vector3 dir = catapult.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x);
        return angle;
    }

    public void Launch(){
    	launched = true;

    	Vector3 newPosition = catapult.position;
    	newPosition.x -= 1f;
    	newPosition.y -= 0.5f;
    	transform.position = newPosition;

    	LineRendererSetUp();
    }

    void OnDestroy(){
     	for(int i = 0; i < numDotToShow; ++i){
    		Destroy(trajectoryDots[i]);
    	}
    }

    void OnCollisionEnter2D(){
    	if(soundOn) GetComponent<AudioSource>().Play();
    	AddExplosionForce(5f, transform.position, 50f);
    	resetter.GetComponent<Resetter>().LaunchNext();
    }

    void AddExplosionForce(float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
    	Rigidbody2D body = GetComponent<Rigidbody2D>();
        var dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        body.AddForce(dir.normalized * explosionForce * wearoff, ForceMode2D.Impulse);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
