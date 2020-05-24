using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor( typeof( CoTween ), true )]
public class CoTween_Inspector : Editor{

	bool _is_playing = false;
	string _filter = "";
	/// <summary>
	/// インスペクタ表示をラップ
	/// </summary>
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		var tween = target as CoTween;
		if( !_is_playing ){
			if( GUILayout.Button( "Play", GUILayout.ExpandWidth(false) ) ) {
				Co.Begin( _EditorPlay( tween ));
			}
		}else{
			if( GUILayout.Button( "Stop", GUILayout.ExpandWidth(false) ) ) {
				tween.Reset(false);
			}
		}

		EditorGUILayout.BeginHorizontal();
		if( !_is_playing ){
			if( GUILayout.Button( "Play Children", GUILayout.ExpandWidth(false) ) ) {
				Co.Begin( _EditorPlay( tween.gameObject.GetComponentsInChildren<CoTween>(true), _filter ));
			}
		}else{
			if( GUILayout.Button( "Stop Children", GUILayout.ExpandWidth(false) ) ) {
				tween.gameObject.GetComponentsInChildren<CoTween>(true).Reset(_filter);
			}
		}
		_filter = GUILayout.TextField( _filter );
		EditorGUILayout.EndHorizontal();
	}
	IEnumerator _EditorPlay( CoTween[] tweens, string filter ){
		IEnumerator coroutine = Co.Begin( tweens.Play(filter));
		_is_playing = true;
		while( coroutine.IsUpdating() ){
			yield return null;
		}
		_is_playing = false;
		tweens.Reset( filter );
	}
	IEnumerator _EditorPlay( CoTween tween ){
		IEnumerator coroutine = tween.Play( false );
		_is_playing = true;
		while( coroutine.IsUpdating() ){
			yield return null;
		}
		_is_playing = false;
		tween.Reset( false );
	}
}
