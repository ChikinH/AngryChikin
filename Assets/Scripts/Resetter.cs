using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;

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
        if(Input.GetKeyDown(KeyCode.R)) Reset();

        spring = projectile[0].GetComponent<SpringJoint2D>();

        if(spring == null && projectile[0].GetComponent<Rigidbody2D>().velocity.sqrMagnitude < resetSpeedSqr){
        	LaunchNext();
        	if(projectile.Count == 0 && target.Count > 0) Reset();
        }

        for(int i = 0; i < target.Count; ++i){
        	if(target[i].GetComponent<Rigidbody2D>().isKinematic){
        		target.RemoveAt(i);
        	}
        }

        if(target.Count == 0){
        	endMenu.SetActive(true);
        	if(idScene == SceneManager.sceneCountInBuildSettings - 1){
        		endMenu.transform.GetChild(0).gameObject.SetActive(false);
        	}
        }
    }

    void OnTriggerExit2D(Collider2D other){
    	if(projectile.Count > 0 && other.GetComponent<Rigidbody2D>() == projectile[0].GetComponent<Rigidbody2D>()){
    		LaunchNext();
    	}
    }

    void Reset(){
    	Application.LoadLevel(Application.loadedLevel);
    }

    public void LaunchNext(){
        GameObject tmp = projectile[0];
        projectile.RemoveAt(0);
        Destroy(tmp);

        if(projectile.Count > 0){
            projectile[0].GetComponent<ProjectileDragging>().Launch();
        }
    }

    public void nextLevel(){
    	SceneManager.LoadScene(idScene + 1, LoadSceneMode.Additive);
    	SceneManager.UnloadScene(idScene);
    }
}
