using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LightController : MonoBehaviour {

	public float dayAngle;
	public float nightAngle;
	public float time;
	
	private Tween _tween = null;
	private bool _daytonight = true;
	
	void Update () {
		if(Input.GetButtonDown("Debug")){
			if(_tween != null){
				_tween.Kill();
				_tween = null;
			}

			if(_daytonight){
				_tween = transform.DOLocalRotate(new Vector3(nightAngle, transform.rotation.y, transform.rotation.z), time).OnComplete(()=>DynamicGI.UpdateEnvironment());
			}else{
				_tween = transform.DOLocalRotate(new Vector3(dayAngle, transform.rotation.y, transform.rotation.z), time).OnComplete(()=>DynamicGI.UpdateEnvironment());
			}

			_daytonight = !_daytonight;
		}
	}
}
