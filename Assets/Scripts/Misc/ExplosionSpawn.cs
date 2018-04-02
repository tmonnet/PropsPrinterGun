using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSpawn : MonoBehaviour {

	public GameObject fx;
	public float delay;
	
	public void Explode(){
		StartCoroutine(ExplodeCoroutine());
	}

	IEnumerator ExplodeCoroutine(){
		yield return new WaitForSeconds(delay);
		Instantiate(fx, transform.position, Quaternion.identity);
		yield return new WaitForSeconds(0.2f);
		Destroy(gameObject);
	}
}
