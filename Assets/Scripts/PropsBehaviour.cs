using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum PropsState{
	Preview_Idle,
	Preview_Error,
	Printed
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
	private bool _isScanned = false;
	private Tween _scanTween = null;
	private PropsState _state;

	void Awake()
	{
		_scanMaterial = GetComponent<Renderer>().materials[0];
		_collider = GetComponent<Collider>();
		_rb = GetComponent<Rigidbody>();
		_state = PropsState.Printed;
	}



	//called when this props is printed
	public void Print(){
		_state = PropsState.Printed;
	}

	//called when the player start scanning this props
	public void Scan(PlayerBehaviour player){ //add player as an argument
		if(_state == PropsState.Printed && !_isScanned){
			Debug.Log("Start Internal");
			_isScanned = true;
			_scanMaterial.DOFloat(0.75f, "_ScanValue", scanningDuration).SetEase(Ease.Linear).OnComplete(
				()=>{
					_scanMaterial.SetFloat("_ScanValue", 0.25f);
					_isScanned = false;
					_scanTween = null;
					player.AddProps(prefab, scanningDuration, printingDuration, printingCost);
					//call method on the player to add this props in a slot
				}
			);

		}
	}

	//called when the player stop scanning this props
	public void ScanStop(){
		if(_state == PropsState.Printed && _scanTween != null && _isScanned){
			Debug.Log("Stop Internal");
			_scanTween.Kill();
			_scanTween = null;
			_isScanned = false;
			_scanMaterial.SetFloat("_ScanValue", 0f);
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if(wallPenetration > 0f && other.transform.tag == "Penetrable"){

		}
	}
}
