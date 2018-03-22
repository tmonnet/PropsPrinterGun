using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public struct PropsSlot{
	public float id;
	public float scanningDuration;
	public float printingDuration;
	public float printingCost;
	public GameObject prefab;
}

public enum PlayerMode{
	CombatMode,
	BuildMode
}

public class PlayerBehaviour : MonoBehaviour {

	public Transform playerHead;
	public float scanRange;
	public float maxFireRate;
	public float antimatterQuantity;
	public float antimatterRegen;

	[Header("--UI--")]
	public Text[] inventoryText = new Text[4];
	public Color selectedColor, baseColor;
	public Slider antimatterSlider;
	public float antimatterRegenLerpSpeed;
	public ScanIconRotation scanCursor;
	public Text alreadyScannedText;

	private PropsSlot[] inventory = new PropsSlot[4];

	private PropsBehaviour _propsScanned = null;
	private PropsBehaviour _propsTemp = null;
	private RaycastHit _hit = new RaycastHit();
	private PlayerMode _mode = PlayerMode.CombatMode;
	private int _selectedSlot = 0;
	private float _antimatterValue;

	public void AddProps(GameObject prefab, float scanningDuration, float printingDuration, float printingCost, int id){
		if(prefab!=null){

			foreach(PropsSlot ps in inventory){
				if(ps.id == id){
					alreadyScannedText.color = new Color(alreadyScannedText.color.r, alreadyScannedText.color.g, alreadyScannedText.color.b, 1f);
					alreadyScannedText.DOColor(new Color(alreadyScannedText.color.r, alreadyScannedText.color.g, alreadyScannedText.color.b, 0f), 1f);
					return;
				}
			}

			inventory[_selectedSlot].prefab = prefab;
			inventory[_selectedSlot].scanningDuration = scanningDuration;
			inventory[_selectedSlot].printingDuration = printingDuration;
			inventory[_selectedSlot].printingCost = printingCost;
			inventory[_selectedSlot].id = id;
			inventoryText[_selectedSlot].DOText(prefab.name,0.5f);
		}
	}

	void Start () {
		_antimatterValue = antimatterQuantity;
		ChangeSlot(_selectedSlot);
	}
	
	void Update () {

		// PRINTING  ------------------------------------------------------------

		if(Input.GetButtonDown("Fire1")){
			if(inventory[_selectedSlot].prefab != null){
				if(_mode == PlayerMode.CombatMode){
					//Instantiate and launch object
					if(CheckAntimatter()){
						_propsTemp = Instantiate(inventory[_selectedSlot].prefab, playerHead.position + playerHead.forward, playerHead.rotation).GetComponent<PropsBehaviour>();
						_propsTemp.Print();
						_propsTemp = null;
					}
				}else{
					//Instantiate object
				}
			}
		}

		// SCANNING  ------------------------------------------------------------

		//Start scanning
		if(Input.GetButtonDown("Fire2")){
			scanCursor.gameObject.SetActive(true);
			StartScan();
		}

		//Stop scanning
		if(Input.GetButton("Fire2")){
			if(_propsScanned == null){
				StartScan();
			}else{
				CheckScan();
			}
		}

		//Stop scanning
		if(Input.GetButtonUp("Fire2")){
			scanCursor.gameObject.SetActive(false);
			StopScan();
		}

		// CHANGE SLOT -----------------------------------------------------------

		if(Input.GetAxis("Mouse ScrollWheel") != 0f){
			if(Input.GetAxis("Mouse ScrollWheel") > 0f){
				if(_selectedSlot == 3){
					ChangeSlot(0);
				}else{
					ChangeSlot(_selectedSlot + 1);
				}
			}else{
				if(_selectedSlot == 0){
					ChangeSlot(3);
				}else{
					ChangeSlot(_selectedSlot - 1);
				}
			}
		}

		if(Input.GetButtonDown("Hotkey1")){
			ChangeSlot(0);
		}

		if(Input.GetButtonDown("Hotkey2")){
			ChangeSlot(1);
		}

		if(Input.GetButtonDown("Hotkey3")){
			ChangeSlot(2);
		}

		if(Input.GetButtonDown("Hotkey4")){
			ChangeSlot(3);
		}

		// ANTI MATTER REGEN -----------------------------------------------------

		if(_antimatterValue < antimatterQuantity){
			SetAntimatter(_antimatterValue + antimatterRegen*Time.deltaTime);

		}
	}

	private void SetAntimatter(float value){
		_antimatterValue = value;
		antimatterSlider.DOValue(_antimatterValue/antimatterQuantity, (Mathf.Abs(_antimatterValue-antimatterSlider.value) / antimatterQuantity)*antimatterRegenLerpSpeed );
	}

	private void StartScan(){
		if(Physics.Raycast(playerHead.position, playerHead.forward, out _hit, scanRange)){
			_propsScanned = _hit.collider.GetComponent<PropsBehaviour>();
			if(_propsScanned != null){
				scanCursor.SetScanning(true);
				_propsScanned.Scan(this);
			}
		}
	}

	private void CheckScan(){
		if(Physics.Raycast(playerHead.position, playerHead.forward, out _hit, scanRange)){
			Debug.Log("Check");
			_propsTemp = _hit.collider.GetComponent<PropsBehaviour>();
			if(_propsTemp != _propsScanned){
				Debug.Log("Abort");
				StopScan();
			}
		}else{
			StopScan();
		}
	}

	private void StopScan(){
		scanCursor.SetScanning(false);
		Debug.Log("Stop");
		if(_propsScanned != null){
			_propsScanned.ScanStop();
			_propsScanned = null;
			_propsTemp = null;
		}
	}

	private void ChangeSlot(int slot){
		inventoryText[_selectedSlot].color = baseColor;
		_selectedSlot = slot;
		inventoryText[_selectedSlot].color = selectedColor;
	}

	private bool CheckAntimatter(){
		float temp = _antimatterValue - inventory[_selectedSlot].printingCost;
		if(temp >= 0f){
			SetAntimatter(temp);
			return true;
		}
		return false;
	}
}
