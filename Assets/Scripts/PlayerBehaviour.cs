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
	public float spawnPoint;
	public GameObject prefab;
}

public enum PlayerMode{
	CombatMode,
	BuildMode
}

public class PlayerBehaviour : MonoBehaviour {

	#region PublicVar
	public Transform playerHead;
	public float scanRange;
	public float destroyRange;
	public float maxFireRate;
	public float antimatterQuantity;
	public float antimatterRegen;
	public float shotForce;
	#endregion

	#region PublicVarUI
	[Header("--UI--")]
	public Text[] inventoryText = new Text[4];
	public Color selectedColor, baseColor;
	public Slider antimatterSlider;
	public float antimatterRegenLerpSpeed;
	public ScanIconRotation scanCursor;
	public Text alreadyScannedText;
	public Text destroyText;
	public RawImage buildIcon;
	public RawImage combatIcon;
	#endregion
	
	#region PrivateVar
	private PropsSlot[] inventory = new PropsSlot[4];
	private PropsBehaviour _propsScanned = null;
	private PropsBehaviour _propsTemp = null;
	private PropsBehaviour _propsPrinted = null;
	private PropsBehaviour _propsSelected = null;
	private PropsBehaviour _propsPreview = null;
	private RaycastHit _hit = new RaycastHit();
	private PlayerMode _mode = PlayerMode.CombatMode;
	private Camera _cam;
	private Tween _tweenShake = null;
	private int _selectedSlot = 0;
	private float _antimatterValue;
	#endregion
	

	void Start () {
		_cam = playerHead.GetComponent<Camera>();
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
						_propsPrinted = Instantiate(inventory[_selectedSlot].prefab, playerHead.position + (playerHead.forward*inventory[_selectedSlot].spawnPoint), playerHead.rotation).GetComponent<PropsBehaviour>();
						_propsPrinted.SetOwner(this);
						_propsPrinted.Shot(playerHead.forward * shotForce);
						//_propsPrinted.Print();
						_propsPrinted = null;
					}
				}else{
					if(_propsPreview != null){
						_propsPreview.Print();
						_propsPreview = null;
					}
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
			if(Input.GetAxis("Mouse ScrollWheel") < 0f){
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

		// HIGHLIGHT & DESTROY ---------------------------------------------------

		if(_mode == PlayerMode.BuildMode){
			Highlight();
			if(_propsSelected != null && _propsSelected.GetOwner() == this){
				float dist = Vector3.Distance(_propsSelected.transform.position, playerHead.position);
				if(dist <= destroyRange){
					destroyText.transform.position = _cam.WorldToScreenPoint(_propsSelected.transform.position);
					destroyText.transform.localScale = Vector3.one * (1f - dist/destroyRange);
					destroyText.gameObject.SetActive(true);

					if(Input.GetButtonDown("Destroy")){
						Destroy(_propsSelected.gameObject);
						_propsSelected = null;
						_propsTemp = null;
					}
				}else{
					destroyText.gameObject.SetActive(false);
				}
			}else{
				destroyText.gameObject.SetActive(false);
			}
		}else{
			if(_propsSelected != null){
				_propsSelected.Highlight(false);
				_propsSelected = null;
			}
		}

		

		// MODE SWAP ------------------------------------------------------------

		if(Input.GetButtonDown("Action")){
			if(_mode == PlayerMode.BuildMode){
				SetMode(PlayerMode.CombatMode);
			}else{
				SetMode(PlayerMode.BuildMode);
			}
		}

		// PREVIEW --------------------------------------------------------------


	}

	private void SetMode(PlayerMode mode){
		switch(mode){
			case PlayerMode.CombatMode : 
				combatIcon.gameObject.SetActive(true);
				buildIcon.gameObject.SetActive(false);
				destroyText.gameObject.SetActive(false);

				_mode = PlayerMode.CombatMode;
				if(_propsPreview != null){
					Destroy(_propsPreview.gameObject);
					_propsPreview = null;
				}
				break;
				
			case PlayerMode.BuildMode : 
				combatIcon.gameObject.SetActive(false);
				buildIcon.gameObject.SetActive(true);

				_mode = PlayerMode.BuildMode;
				if(inventory[_selectedSlot].prefab != null){
					_propsPreview = Instantiate(inventory[_selectedSlot].prefab).GetComponent<PropsBehaviour>();
					_propsPreview.Preview();
				}
				break;
		}
	}

	
	#region Highlight

	private void Highlight(){ //Call to highlight the props you are looking at
		
		if(Physics.Raycast(playerHead.position, playerHead.forward, out _hit, scanRange)){
			_propsTemp = _hit.collider.GetComponent<PropsBehaviour>();
			if(_propsTemp != null){
				if(_propsSelected != null){
					if(_propsTemp == _propsSelected){ //still looking at the same props
						_propsTemp = null;
					}else{ //from an already highlighted props to another one
						_propsSelected.Highlight(false);
						_propsSelected = _propsTemp;
						_propsSelected.Highlight(true);
						_propsTemp = null;
					}
				}else{ //from nothing to a new selected props
					_propsSelected = _propsTemp;
					_propsSelected.Highlight(true);
					_propsTemp = null;
				}
			}else{ //Deselect
				StopHighlight();
			}
		}else{ //Deselect
			StopHighlight();
		}
	}

	private void StopHighlight(){
		if(_propsSelected != null){
			_propsSelected.Highlight(false);
			_propsSelected = null;
		}
	}

	#endregion

	#region Scan
	
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

	#endregion
	
	#region Inventory
	public void AddProps(GameObject prefab, float scanningDuration, float printingDuration, float printingCost, float spawnPoint, int id){
		if(prefab!=null){

			foreach(PropsSlot ps in inventory){
				if(ps.id == id){
					//warning message when already scanned props
					alreadyScannedText.color = new Color(alreadyScannedText.color.r, alreadyScannedText.color.g, alreadyScannedText.color.b, 1f);
					alreadyScannedText.DOColor(new Color(alreadyScannedText.color.r, alreadyScannedText.color.g, alreadyScannedText.color.b, 0f), 1f);
					return;
				}
			}

			//adding props in the inventory
			inventory[_selectedSlot].prefab = prefab;
			inventory[_selectedSlot].scanningDuration = scanningDuration;
			inventory[_selectedSlot].printingDuration = printingDuration;
			inventory[_selectedSlot].printingCost = printingCost;
			inventory[_selectedSlot].spawnPoint = spawnPoint;
			inventory[_selectedSlot].id = id;
			inventoryText[_selectedSlot].DOText(prefab.name,0.5f);
		}
	}
	private void ChangeSlot(int slot){
		inventoryText[_selectedSlot].color = baseColor;
		_selectedSlot = slot;
		inventoryText[_selectedSlot].color = selectedColor;
	}

	#endregion

	#region Antimatter

	private void SetAntimatter(float value){
		_antimatterValue = value;
		antimatterSlider.DOValue(_antimatterValue/antimatterQuantity, (Mathf.Abs(_antimatterValue-antimatterSlider.value) / antimatterQuantity)*antimatterRegenLerpSpeed );
	}

	private bool CheckAntimatter(){
		float temp = _antimatterValue - inventory[_selectedSlot].printingCost;
		if(temp >= 0f){
			SetAntimatter(temp);
			return true;
		}
		//Shake antimatter bar
		if(_tweenShake != null){
			_tweenShake.Kill(true);
		}
		_tweenShake = antimatterSlider.transform.DOShakeScale(0.3f,0.5f).OnComplete(()=> _tweenShake = null);
		
		return false;
	}

	#endregion
}