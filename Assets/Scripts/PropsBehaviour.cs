﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum PropsState{
	Preview_Idle,
	Preview_Error,
	Printed,
	Printed_In_Progress
}

public class PropsBehaviour : MonoBehaviour {

	public int id;
	public float scanningDuration;
	public float printingDuration;
	public float printingCost;
	public float wallPenetration;
	public GameObject prefab;
	[Header("Material")]
	public float scanStartValue;
	public float scanEndValue;

	private Material _printMaterial;
	private Material _scanMaterial;
	private Material _selectionMaterial;
	private Color _scanColor;
	private Collider _collider;
	private Rigidbody _rb;
	private bool _isScanned = false;
	private Tween _scanTween = null;
	private PropsState _state;

	void Awake()
	{
		_scanMaterial = GetComponent<Renderer>().materials[0];
		_printMaterial = GetComponent<Renderer>().materials[1];
		_selectionMaterial = GetComponent<Renderer>().materials[2];
		_collider = GetComponent<Collider>();
		_rb = GetComponent<Rigidbody>();
		_state = PropsState.Printed;
		_scanColor = _scanMaterial.color;
	}

	public void Preview(){
		_rb.isKinematic = true;
		_collider.enabled = false;
		_state = PropsState.Preview_Idle;
		_printMaterial.SetFloat("_DissolveRatio", 1f);
		_scanMaterial.SetFloat("_ScanValue", scanEndValue);
		_scanMaterial.color = _scanColor;
	}

	public void PreviewError(){
		_rb.isKinematic = true;
		_collider.enabled = false;
		_state = PropsState.Preview_Error;
		_scanMaterial.SetFloat("_ScanValue", scanEndValue);
		_scanMaterial.color = new Color(1f, 0f, 0f, _scanColor.a);
	}

	//called when this props is printed
	public void Print(){
		_rb.isKinematic = true;
		_collider.enabled = true;
		_state = PropsState.Printed_In_Progress;
		_scanMaterial.SetFloat("_ScanValue", scanEndValue);
		_printMaterial.SetFloat("_DissolveRatio", 1f);
		_printMaterial.DOFloat(0f, "_DissolveRatio", printingDuration).SetEase(Ease.Linear).OnComplete(
				()=>{
					_scanMaterial.SetFloat("_ScanValue", scanStartValue);
					_state = PropsState.Printed;
					_rb.isKinematic = false;
				}
		);

	}

	public void Highlight(bool value){
		if(!_isScanned && _state == PropsState.Printed){
			if(value){
				_selectionMaterial.SetFloat("_ScanValue", 1f);
			}else{
				_selectionMaterial.SetFloat("_ScanValue", 0f);
			}
		}
	}

	//called when the player start scanning this props
	public void Scan(PlayerBehaviour player){ //add player as an argument
		if(_state == PropsState.Printed && !_isScanned){
			_isScanned = true;
			_scanTween = _scanMaterial.DOFloat(scanEndValue, "_ScanValue", scanningDuration).SetEase(Ease.Linear).OnComplete(
				()=>{
					_scanMaterial.SetFloat("_ScanValue", scanStartValue);
					_isScanned = false;
					_scanTween = null;
					player.AddProps(prefab, scanningDuration, printingDuration, printingCost, id);
					//call method on the player to add this props in a slot
				}
			);

		}
	}

	//called when the player stop scanning this props
	public void ScanStop(){
		if(_state == PropsState.Printed && _scanTween != null && _isScanned){
			_scanTween.Kill();
			_scanTween = null;
			_isScanned = false;
			_scanMaterial.SetFloat("_ScanValue", scanStartValue);
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if(wallPenetration > 0f && other.transform.tag == "Penetrable"){
			
		}
	}
}
