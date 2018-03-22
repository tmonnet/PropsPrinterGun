using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScanIconRotation : MonoBehaviour {

	public float rotationSpeed;
	private float _rotSpeed;
	private Vector3 _rot;
	void Update () {
		_rot = transform.rotation.eulerAngles;
		_rot += new Vector3(0f, 0f, _rotSpeed * Time.deltaTime);
		transform.rotation = Quaternion.Euler(_rot);
	}

	public void SetScanning(bool value){
		if(value){
			_rotSpeed = rotationSpeed*2f;
		}else{
			_rotSpeed = rotationSpeed;
		}
	}
}
