using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* manage the damage taken by planks */
public class PlankDamage : MonoBehaviour
{
    public int hitPoints = 20;
	public float damageImpactSpeed;

	private int currentHitPoints;
	private float damageImpactSpeedSqr;
	private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHitPoints = hitPoints;
        damageImpactSpeedSqr = damageImpactSpeed*damageImpactSpeed;
    }

    /* on collision, the item's life decrease */
    void OnCollisionEnter2D(Collision2D collision){
    	if(collision.collider.tag != "Damager") return;
    	if(collision.relativeVelocity.sqrMagnitude < damageImpactSpeedSqr) return;

    	currentHitPoints --;

    	if(currentHitPoints <= 0) Kill();
    }

    /* kill the object when hp = 0 */
    void Kill(){
    	spriteRenderer.enabled = false;
    	GetComponent<Collider2D>().enabled = false;
    	GetComponent<Rigidbody2D>().isKinematic = true;
    }
}
