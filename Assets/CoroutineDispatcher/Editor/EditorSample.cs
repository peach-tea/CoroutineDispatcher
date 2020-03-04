using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorSample 
{
	[MenuItem("CoroutineDispatcher/EditorRun")]
	static void Test(){
		Co.Begin(CoroutineTest());
	}

	static IEnumerator CoroutineTest(){
		Debug.Log("Test Start");
		yield return WaitSeconds(1.0f);
		Debug.Log("Test End");
	}
	static IEnumerator WaitSeconds( float second ){
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
