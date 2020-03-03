using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineDispatcher : MonoBehaviour{

	public enum eUpdateType{
		Update,
		LateUpdate,
		FixedUpdate,
		Max,
	}

	#region -- Singleton --
	// singleton
	static CoroutineDispatcher _instance = null;
	public static CoroutineDispatcher instance {
		get{
			if( _instance == null ){
				_instance = FindObjectOfType(typeof(CoroutineDispatcher)) as CoroutineDispatcher;
				if( _instance == null ){
					GameObject obj = new GameObject("CoroutineDispatcher");
					_instance = obj.AddComponent<CoroutineDispatcher>();
				}
			}
			return _instance;
		}
	}
	#endregion

	[SerializeField]
	int _coroutine_array_size = 64;
	CoroutineConsumer[] _consumers = null;

#if UNITY_EDITOR
	CoroutineConsumer _editor_consumer = null;
#endif
	void Awake() {
		if( _instance == null){
			_instance = this;
		}else if( _instance != this ){
			Destroy(this);
		}

		_consumers = new CoroutineConsumer[(int)eUpdateType.Max];
		for( int i = 0; i < _consumers.Length; ++i ){
			_consumers[i] = new CoroutineConsumer(_coroutine_array_size);
		}
	}
	/// <summary>
	/// コルーチン実行
	/// </summary>
	/// <param name="coroutine">実行するコルーチン</param>
	/// <param name="type">更新タイプ</param>
	/// <returns></returns>
	public IEnumerator Begin( IEnumerator coroutine, eUpdateType type = eUpdateType.Update ){
		return Begin( coroutine, null );
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
	public IEnumerator Begin( eUpdateType type, IEnumerator routine0, IEnumerator routine1,
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
	/// コルーチン実行
	/// </summary>
	/// <param name="coroutine">実行するコルーチン</param>
	/// <param name="game_object">紐づけるゲームオブジェクト</param>
	/// <param name="type">更新タイプ</param>
	/// <returns></returns>
	public IEnumerator Begin( IEnumerator coroutine, GameObject game_object, eUpdateType type = eUpdateType.Update ){
#if UNITY_EDITOR
		if( !Application.isPlaying ){

			if( _editor_consumer == null ){
				_editor_consumer = new CoroutineConsumer(_coroutine_array_size);
				UnityEditor.EditorApplication.update += EditorUpdate;
			}
			return _editor_consumer.Begin(coroutine);
		}
#endif
		return _consumers[(int)type].Begin(coroutine);
	}
	/// <summary>
	/// コルーチン停止
	/// </summary>
	/// <param name="routine">停止するコルーチン</param>
	/// <param name="type">更新タイプ</param>
	public void End( IEnumerator routine, eUpdateType type = eUpdateType.Update ){
		_consumers[(int)type].End( routine );
	}
#region -- Update Coroutine --
#if UNITY_EDITOR
	void EditorUpdate(){
		_editor_consumer?.ConsumeEnumerator();
	}
#endif
	void Update() {
		_consumers[(int)eUpdateType.Update].ConsumeEnumerator();
	}
	void LateUpdate() {
		_consumers[(int)eUpdateType.LateUpdate].ConsumeEnumerator();
	}
	void FixedUpdate() {
		_consumers[(int)eUpdateType.FixedUpdate].ConsumeEnumerator();
	}
#endregion

#region -- IsUpdating Method --
	/// <summary>
	/// コルーチンの実行状態確認
	/// </summary>
	/// <param name="routine">判定対象</param>
	/// <returns>true:実行中</returns>
	public bool IsUpdating( IEnumerator routine ) {
		for ( int i = 0; i < (int)eUpdateType.Max; ++i ) {
			if ( _consumers[i].IsUpdating( routine ) ) {
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
	public bool IsUpdating( List<IEnumerator> routines ){
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
	public bool IsUpdating( IEnumerator[] routines ){
		for( int i = 0; i < routines.Length; ++i ) {
			if( IsUpdating( routines[i] ) ){
				return true;
			}
		}
		return false;
	}

#endregion
}
