using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Wait{

	public static IEnumerator Touch(){
		while( true ){
			if( Input.GetMouseButtonDown(0) ){
				yield break;
			}
			yield return null;
		}
	}

	public static IEnumerator Seconds( float time, bool is_ignore_timescale = false ){
		float elapsed = 0.0f;
		while( true ){
			if( is_ignore_timescale ){
				elapsed += Co.unscaledDeltaTime;
			}else{
				elapsed += Co.deltaTime;
			}
			if( elapsed >= time ){
				yield break;
			}
			yield return null;
		}
	}
}
