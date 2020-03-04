using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各コルーチンの管理・実行
/// </summary>
public class CoroutineDispatcher : MonoBehaviour{

	/// <summary>
	/// 更新タイプ
	/// </summary>
	public enum eUpdateType{
		Update,
		LateUpdate,
		FixedUpdate,
		Max,
	}

	[SerializeField]
	int _coroutine_array_size = 64;
	CoroutineConsumer[] _consumers = null;

	public CoroutineConsumer[] consumers{
		get{
			return _consumers;
		}
	}

	void Awake() {
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
	/// コルーチン実行
	/// </summary>
	/// <param name="coroutine">実行するコルーチン</param>
	/// <param name="game_object">紐づけるゲームオブジェクト</param>
	/// <param name="type">更新タイプ</param>
	/// <returns></returns>
	public IEnumerator Begin( IEnumerator coroutine, GameObject game_object, eUpdateType type = eUpdateType.Update ){
		return _consumers[(int)type].Begin(coroutine, game_object);
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
}
