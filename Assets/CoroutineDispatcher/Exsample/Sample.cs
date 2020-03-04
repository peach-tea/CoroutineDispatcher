using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour{

	void Update(){
		if(Input.GetKeyDown( KeyCode.Z )){
			Debug.Log("[StartCoroutine]");
			StartCoroutine( Test() );
		}
		if(Input.GetKeyDown( KeyCode.X )){
			Debug.Log("[CoroutineDispatcher] this game_object begin ");
			this.BeginCoroutine(Test());
		}
		if(Input.GetKeyDown( KeyCode.C )){
			Debug.Log("[CoroutineDispatcher] Dispatcher begin ");
			Co.Begin(Test());
		}
	}

	public IEnumerator Test(){
		float start_time = Time.time;
		yield return Test2();
		Debug.Log( "elapsed time : " + (Time.time - start_time) );
	}
	IEnumerator Test2(){
		yield return WaitSeconds(1.0f);
		yield break;
	}

	IEnumerator WaitSeconds( float second ){
		var startTime = System.DateTimeOffset.UtcNow;
		while (true){
			yield return null;
 
			var elapsed = (System.DateTimeOffset.UtcNow - startTime).TotalSeconds;
			if (elapsed >= second){
				break;
			}
		};
	}
}
