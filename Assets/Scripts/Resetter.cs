using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;

/* manage the reset, or end level UI */
public class Resetter : MonoBehaviour
{

	public float resetSpeed = 0.025f;
	public GameObject projectiles;
	public GameObject targets;
	public GameObject endMenu;

	private float resetSpeedSqr;
	private SpringJoint2D spring;
	private List<GameObject> projectile;
	private List<GameObject> target;
	private int idScene;

    void Start()
    {
        resetSpeedSqr = resetSpeed*resetSpeed;
        
        projectile = new List<GameObject>();
        for(int i = 0; i < projectiles.transform.childCount; ++i){
        	projectile.Add(projectiles.transform.GetChild(i).gameObject);
        }

        idScene = projectile[0].scene.buildIndex;

        SceneManager.SetActiveScene(projectile[0].scene);

        Component tmp;

        if(projectile[0].TryGetComponent(typeof(ProjectileDragging), out tmp))  projectile[0].GetComponent<ProjectileDragging>().Launch();
        else projectile[0].GetComponent<BombDragging>().Launch();

        target = new List<GameObject>();
        for(int i = 0; i < targets.transform.childCount; ++i){
        	target.Add(targets.transform.GetChild(i).gameObject);
        }
    }

    void Update()
    {
        spring = projectile[0].GetComponent<SpringJoint2D>();

        /* launch the next projectile when the one that have been fired is stoped */
        if(spring == null && projectile[0].GetComponent<Rigidbody2D>().velocity.sqrMagnitude < resetSpeedSqr){
        	LaunchNext();
        	if(projectile.Count == 0 && target.Count > 0) Reset();
        }

        /* check if ennemys are still alive **/
        for(int i = 0; i < target.Count; ++i){
        	if(target[i].GetComponent<Rigidbody2D>().isKinematic){
        		target.RemoveAt(i);
        	}
        }

        /* if not, the level is done */
        if(target.Count == 0){
        	endMenu.SetActive(true);
        	if(idScene == SceneManager.sceneCountInBuildSettings - 1){
        		endMenu.transform.GetChild(0).gameObject.SetActive(false);
        	}
        }
    }

    /* launch the next projectile if the last one get out of the level */
    void OnTriggerExit2D(Collider2D other){
    	if(projectile.Count > 0 && other.GetComponent<Rigidbody2D>() == projectile[0].GetComponent<Rigidbody2D>()){
    		LaunchNext();
    	}
    }

    /* reload the playing level */
    void Reset(){
    	Application.LoadLevel(Application.loadedLevel);
    }

    /* destroy the last projectile, and call the launch of the next */
    public void LaunchNext(){
        GameObject tmp = projectile[0];
        projectile.RemoveAt(0);
        Destroy(tmp);

        if(projectile.Count > 0){
            projectile[0].GetComponent<ProjectileDragging>().Launch();
        }
    }

    /* on click, load the next level */
    public void nextLevel(){
    	SceneManager.LoadScene(idScene + 1, LoadSceneMode.Additive);
    	SceneManager.UnloadScene(idScene);
    }
}
