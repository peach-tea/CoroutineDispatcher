using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorSample 
{
	[MenuItem("CoroutineDispatcher/EditorRun")]
	static void Test(){
		CoroutineDispatcher.instance.Begin(CoroutineTest());
	}

	static IEnumerator CoroutineTest(){
		Debug.Log("Test Start");
		for( int i = 0; i < 100; ++i ){
			yield return null;
		}
		Debug.Log("Test End");
	}
}
