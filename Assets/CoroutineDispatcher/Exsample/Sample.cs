using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour{

	void Update(){
		if(Input.GetKeyDown( KeyCode.Z )){
			Debug.Log("StartCoroutine");
			StartCoroutine( Test() );
		}
		if(Input.GetKeyDown( KeyCode.X )){
			Debug.Log("CoroutineManager");
			this.BeginCoroutine(Test());
			CoroutineDispatcher.instance.Begin( Test() );
		}
		if(Input.GetKeyDown( KeyCode.C )){

		}
		if(Input.GetKeyDown( KeyCode.C )){

		}
	}

	IEnumerator Test(){
		float start_time = Time.time;
		yield return Test2();
		Debug.Log( "elapsed time : " + (Time.time - start_time) );
	}
	IEnumerator Test2(){
		yield break;
	}
}
