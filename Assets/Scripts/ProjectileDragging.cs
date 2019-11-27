using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* manage the basic projectiles */
public class ProjectileDragging : MonoBehaviour
{
	/* max distance betwin catapult and asteroid */
	public float maxStretch = 3.0f;

	/* the line displayed on the catapult */
	public LineRenderer catapultLineFront;
	public LineRenderer catapultLineBack;

	/* prefab for the dot used on trajectory predict */
	public GameObject trajectoryDotPrefab;	

	private SpringJoint2D spring;
	private Transform catapult;

	/* used when the mouse is at a radius > maxStretch */
	private Ray rayToMouse;

	private Ray leftCatapultToProjectile;
	private float maxStretchSqr;

	/* radios of the projectile */
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
    	/* used only if the projectile is the one we want to shot */
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

    /* setup the line on the catapult */
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

    /* update the line */
    void LineRendererUpdate(){
    	Vector2 catapultToProjectile = transform.position - catapultLineFront.transform.position;
    	leftCatapultToProjectile.direction = catapultToProjectile;
    	Vector3 holdPoint = leftCatapultToProjectile.GetPoint(catapultToProjectile.magnitude + circleRadius);
    	catapultLineFront.SetPosition(1, holdPoint);
    	catapultLineBack.SetPosition(1, holdPoint);
    }

    void OnMouseDown(){
    	spring.enabled = false;
    	clickedOn = true;
    }

    void OnMouseUp(){	
    	spring.enabled = true;

    	/* submit the object to unity physics */
    	GetComponent<Rigidbody2D>().isKinematic = false;
    	clickedOn = false;
    }

    /* used when the mouse is down on the object */
    void Dragging(){
    	Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    	Vector2 catapultToMouse = mouseWorldPoint - catapult.position;

    	/*  display a prevision of the trajectory (to be fixed) */
    	for(int i = 0; i < numDotToShow; ++i){
    		Destroy(trajectoryDots[i]);
    		trajectoryDots[i] = Instantiate(trajectoryDotPrefab);
    		trajectoryDots[i].transform.position = calculatePosition(dotTimeStep * i);
    	}

    	/* if the mouse is to far, it keep the aligment */
    	if (catapultToMouse.sqrMagnitude > maxStretchSqr){
    		rayToMouse.direction = catapultToMouse;
    		mouseWorldPoint = rayToMouse.GetPoint(maxStretch);
    	}

    	mouseWorldPoint.z = 0f;
    	/* positioned the object under the mouse */
    	transform.position = mouseWorldPoint;
    }


    /* look if the mouse click on the object (OnMouseEvents weren't working) */
    bool isOnMe(){
    	if(!launched) return false;
    	Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    	Vector2 asteroidToMouse = transform.position - mouseWorldPoint;

    	if (asteroidToMouse.sqrMagnitude < circleRadius*circleRadius){
    		return true;
    	}
    	return false;
    }

    /* calculate the position after a given time */
    Vector2 calculatePosition(float elapsedTime){
    	Vector2 position = new Vector2(transform.position.x, transform.position.y);
    	return  Gravity * elapsedTime * elapsedTime * 0.5f + position + forecastVelocity() * elapsedTime;
    }

    /* the velocity at the start */ 
    Vector2 forecastVelocity(){
    	float powerMultiplier = 9f;
    	float radianAngle = calculateAngle();
	
    	float distance = Vector2.Distance(transform.position, catapult.position);

        float vel = distance * powerMultiplier;

        float xVel = vel * Mathf.Cos(radianAngle);
        float yVel = vel * Mathf.Sin(radianAngle);

        return new Vector2(xVel, yVel);
    }

    /* the angle with the catapult before the shot */
    public float calculateAngle()
    {
        Vector3 dir = catapult.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x);
        return angle;
    }

    /* prepare the object to be shot */
    public void Launch(){
    	launched = true;

    	Vector3 newPosition = catapult.position;
    	newPosition.x -= 1f;
    	newPosition.y -= 0.5f;
    	transform.position = newPosition;

    	LineRendererSetUp();
    }

    /* delete the trajectory preview */
    void OnDestroy(){
    	for(int i = 0; i < numDotToShow; ++i){
    		Destroy(trajectoryDots[i]);
    	}
    }

    /* play a sound on collision */
    void OnCollisionEnter2D(){
    	if(soundOn) GetComponent<AudioSource>().Play();
    }
}
