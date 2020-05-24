using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour{
	[SerializeField]
	CoTween[] _tweens = null;

	IEnumerator _loop_coroutine = null;
	int _count = 0;

	private void OnGUI() {

		GUILayout.Label("コルーチン実行状況 " + _count);

		if( GUILayout.Button( "コルーチン実行" )){
			EndCoroutine();
			_loop_coroutine = Co.Begin( Loop() );
			// or
			// _loop_coroutine = Loop().Begin();
		}
		if( _loop_coroutine != null ){
			if( GUILayout.Button( "コルーチン一時停止" )){
				Co.Pause(_loop_coroutine, true);
				// or
				// _loop_coroutine.Pause(true);
			}
			if( GUILayout.Button( "コルーチン再開" )){
				Co.Pause(_loop_coroutine, false);
				// or
				// _loop_coroutine.Pause(false);
			}
			if( GUILayout.Button( "コルーチン停止" )){
				EndCoroutine();
			}
		}
		if( GUILayout.Button( "Tween実行" )){
			_tweens.Play().Begin();
		}
	}

	void EndCoroutine(){
		Co.End( _loop_coroutine );
		// or
		// _loop_coroutine.End();
		_count = 0;
		_loop_coroutine = null;
	}


	public IEnumerator Loop(){
		while( true ){
			_count++;
			yield return null;
		}
	}
}
