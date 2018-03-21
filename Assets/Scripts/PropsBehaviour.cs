using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum PropsState{

}

public class PropsBehaviour : MonoBehaviour {

	public float scanningDuration;
	public float printingDuration;
	public float printingCost;
	public float wallPenetration;
	public GameObject prefab;

	private Material _printMaterial;
	private Material _scanMaterial;
	private Collider _collider;
	private Rigidbody _rb;
	private bool _isProjectile = false;
	private bool _isScanned = false;
	private Tween _scanTween = null;

	void Awake()
	{
		_scanMaterial = GetComponent<Renderer>().materials[0];
		_collider = GetComponent<Collider>();
		_rb = GetComponent<Rigidbody>();
	}



	//called when this props is printed
	public void Print(){

	}

	//called when the player start scanning this props
	public void Scan(){ //add player as an argument
		if(!_isScanned){
			_isScanned = true;
			_scanMaterial.DOFloat(1f, "_ScanValue", scanningDuration).OnComplete(
				()=>{
					_scanMaterial.SetFloat("_ScanValue", 0f);
					_isScanned = false;
					_scanTween = null;
				}
			);

			//call method on the player to add this props in a slot
		}
	}

	//called when the player stop scanning this props
	public void ScanStop(){
		if(_scanTween != null && _isScanned){
			_scanTween.Kill();
			_scanTween = null;
			_scanMaterial.SetFloat("_ScanValue", 0f);
		}
	}
}
