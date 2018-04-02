using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour {

	private void Update()
	{
		if(Input.GetButtonDown("Debug")){
			SceneManager.LoadScene(0);
		}
	}
}
