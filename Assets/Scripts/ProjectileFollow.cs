using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFollow : MonoBehaviour
{


	public GameObject projectiles;
	public Transform farLeft;
	public Transform farRight;

	private GameObject projectile;

	void Start(){
		setCamera();
	}

    void Update()
    {
    	if(projectiles.transform.childCount > 0 && projectile != projectiles.transform.GetChild(0).gameObject) setCamera();
    	else if(projectile.GetComponent<SpringJoint2D>() == null){
        	Vector3 newPosition = transform.position;
        	newPosition.x = projectile.transform.position.x;
        	newPosition.x = Mathf.Clamp(newPosition.x, farLeft.position.x, farRight.position.x);
        	transform.position = newPosition;
        }
        else{
        	Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        	Vector2 mouseToCenter = transform.position - mouseWorldPoint;

        	if(mouseToCenter.x < 10){
        		Vector3 newPosition = transform.position;
        		newPosition.x += 1;
        		newPosition.x = Mathf.Clamp(newPosition.x, farLeft.position.x, farRight.position.x);
        		transform.position = newPosition;
        	}

        	if(mouseToCenter.x > -10){
        		Vector3 newPosition = transform.position;
        		newPosition.x -= 1;
        		newPosition.x = Mathf.Clamp(newPosition.x, farLeft.position.x, farRight.position.x);
        		transform.position = newPosition;
        	}
        }
    }

    void setCamera(){
    	projectile = projectiles.transform.GetChild(0).gameObject;

        Vector3 newPosition = transform.position;
		newPosition.x = projectile.transform.position.x;
        newPosition.x = Mathf.Clamp(newPosition.x, farLeft.position.x, farRight.position.x);
        transform.position = newPosition;
    }
}
