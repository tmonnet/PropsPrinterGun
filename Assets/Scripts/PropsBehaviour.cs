using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PropsBehaviour : MonoBehaviour {

	public float scanningDuration;
	public float printingDuration;
	public float printingCost;

	private Material _scanMaterial;
	private Collider _collider;

	void Awake()
	{
		_scanMaterial = GetComponent<Renderer>().materials[0];
		_collider = GetComponent<Collider>();
	}

	public void Print(){

	}

	public GameObject Scan(){
		_scanMaterial.DOFloat(1f, "_ScanValue", scanningDuration).OnComplete(
			()=>{
				_scanMaterial.SetFloat("_ScanValue", 0f);
			}
		);

		return gameObject;
	}
}
