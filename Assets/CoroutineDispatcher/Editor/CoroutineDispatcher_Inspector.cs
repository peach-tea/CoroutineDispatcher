using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CoroutineDispatcher))]
public class CoroutineDispatcher_Inspector : Editor{

	CoroutineDispatcher.eUpdateType _update_type = CoroutineDispatcher.eUpdateType.Update;

	public override void OnInspectorGUI(){
		DrawDefaultInspector();

		var dispatcher = target as CoroutineDispatcher;
		_update_type = (CoroutineDispatcher.eUpdateType)EditorGUILayout.EnumPopup( _update_type );
		CoroutineConsumer consumer = null;
		if( !Application.isPlaying ) {
			consumer = Co.editorConsumer;
		}else{
			consumer = dispatcher.consumers[(int)_update_type];
		}
		for( int i = 0; i < consumer.runNum; ++i ){
			var coroutine = consumer.coroutines[i];
			if( coroutine == null ){
				continue;
			}
			GameObject link_game_object = consumer.GetLinkGameObject( coroutine );
			if( link_game_object != null ){
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField( coroutine.ToString() );
				EditorGUILayout.ObjectField( link_game_object, typeof(GameObject), true );
				EditorGUILayout.EndHorizontal();
			}else{
				EditorGUILayout.LabelField( coroutine.ToString() );
			}
		}
	}
}
