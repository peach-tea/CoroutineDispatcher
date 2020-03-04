using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Coroutine実行アクセッサ
/// </summary>
public static class Co {
	static CoroutineDispatcher _dispatcher = null;		//!< コルーチン実行者
	static CoroutineDispatcher dispatcher{
		get{
			if( _dispatcher == null ){
				_dispatcher = GameObject.FindObjectOfType(typeof(CoroutineDispatcher)) as CoroutineDispatcher;
				if( _dispatcher == null ){
					GameObject obj = new GameObject("CoroutineDispatcher");
					_dispatcher = obj.AddComponent<CoroutineDispatcher>();
				}
			}
			return _dispatcher;
		}
	}
#if UNITY_EDITOR
	static CoroutineConsumer _editor_consumer = null;	//!< Editor用のコルーチン実行者
	static CoroutineConsumer editorConsumer{
		get{
			if( _editor_consumer == null ){
				_editor_consumer = new CoroutineConsumer(256);
				UnityEditor.EditorApplication.update += EditorUpdate;
			}
			return _editor_consumer;
		}
	}
	static void EditorUpdate(){
		_editor_consumer?.ConsumeEnumerator();
	}
#endif
	/// <summary>
	/// コルーチン実行
	/// </summary>
	/// <param name="coroutine">実行するコルーチン</param>
	/// <param name="type">更新タイプ</param>
	/// <returns></returns>
	public static IEnumerator Begin( IEnumerator coroutine, CoroutineDispatcher.eUpdateType type = CoroutineDispatcher.eUpdateType.Update ){
		return Begin( coroutine, null, type );
	}
	/// <summary>
	/// コルーチン実行
	/// </summary>
	/// <param name="coroutine">実行するコルーチン</param>
	/// <param name="type">更新タイプ</param>
	/// <returns></returns>
	public static IEnumerator Begin( IEnumerator coroutine, GameObject game_object, CoroutineDispatcher.eUpdateType type = CoroutineDispatcher.eUpdateType.Update ){
#if UNITY_EDITOR
		if( !Application.isPlaying ){
			return editorConsumer.Begin(coroutine, game_object);
		}
#endif
		return dispatcher.Begin( coroutine, game_object );
	}
	/// <summary>
	/// コルーチン複数実行
	/// </summary>
	/// <param name="routine0">コルーチン</param>
	/// <param name="routine1">コルーチン</param>
	/// <param name="routine2">コルーチン</param>
	/// <param name="routine3">コルーチン</param>
	/// <param name="routine4">コルーチン</param>
	/// <param name="routine5">コルーチン</param>
	/// <remarks>
	/// paramsを使いたくないので、ベタ書き
	/// </remarks>
	public static IEnumerator Begin( CoroutineDispatcher.eUpdateType type, IEnumerator routine0, IEnumerator routine1,
		IEnumerator routine2 = null, IEnumerator routine3= null, IEnumerator routine4 = null, IEnumerator routine5 = null ){

		IEnumerator r0 = null;
		if( routine0 != null ){
			r0 = Begin( routine0, type );
		}
		IEnumerator r1 = null;
		if( routine1 != null ){
			r1 = Begin( routine1, type );
		}
		IEnumerator r2 = null;
		if( routine2 != null ){
			r2 = Begin( routine2, type );
		}
		IEnumerator r3 = null;
		if( routine3 != null ){
			r3 = Begin( routine3, type );
		}
		IEnumerator r4 = null;
		if( routine4 != null ){
			r4 = Begin( routine4, type );
		}
		IEnumerator r5 = null;
		if( routine5 != null ){
			r5 = Begin( routine5, type );
		}
		while( r0 != null || r1 != null || r2 != null || r3 != null || r4 != null || r5 != null ){
			yield return null;
		}
	}
	/// <summary>
	/// コルーチン停止
	/// </summary>
	/// <param name="routine">停止するコルーチン</param>
	/// <param name="type">更新タイプ</param>
	public static void End( IEnumerator routine, CoroutineDispatcher.eUpdateType type = CoroutineDispatcher.eUpdateType.Update ){
#if UNITY_EDITOR
		if( !Application.isPlaying ){
			_editor_consumer?.End( routine );
			return;
		}
#endif
		dispatcher.End( routine, type );
	}
	/// <summary>
	/// コルーチンの実行状態確認
	/// </summary>
	/// <param name="routine">判定対象</param>
	/// <returns>true:実行中</returns>
	public static bool IsUpdating( IEnumerator routine ) {
#if UNITY_EDITOR
		if( !Application.isPlaying ){
			if( _editor_consumer != null ){
				return _editor_consumer.IsUpdating( routine );
			}else{
				return false;
			}
		}
#endif
		return dispatcher.IsUpdating( routine );
	}

	/// <summary>
	/// 実行中判定.
	/// </summary>
	/// <param name="routines">コルーチン</param>
	/// <returns>実行中かどうか</returns>
	public static bool IsUpdating( List<IEnumerator> routines ){
		for( int i = 0; i < routines.Count; ++i ) {
			if( IsUpdating( routines[i] ) ){
				return true;
			}
		}
		return false;
	}
	/// <summary>
	/// 実行中判定.
	/// </summary>
	/// <param name="routines">コルーチン</param>
	/// <returns>実行中かどうか</returns>
	public static bool IsUpdating( IEnumerator[] routines ){
		for( int i = 0; i < routines.Length; ++i ) {
			if( IsUpdating( routines[i] ) ){
				return true;
			}
		}
		return false;
	}
}