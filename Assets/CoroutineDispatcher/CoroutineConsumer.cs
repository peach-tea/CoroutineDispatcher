using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CoroutineConsumer{
	IEnumerator[] _coroutines = null;
	int _update_coroutine_num = 0;

	Dictionary<IEnumerator,IEnumerator> _nest_table = null;
	Dictionary<IEnumerator,GameObject> _link_object_table = null;

	List<IEnumerator> _now_begin_coroutines = null;
	//! 動作中コルーチン数取得
	public int runNum {
		get {
			return _update_coroutine_num;
		}
	}
	//! 動作中コルーチン取得
	public IEnumerator[] coroutines {
		get {
			return _coroutines;
		}
	}
	//! 一時停止中のコルーチン
	List<IEnumerator> _pause_coroutines = new List<IEnumerator>(128);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="array_size">初期配列サイズ</param>
	public CoroutineConsumer( int array_size ) {
		_coroutines = new IEnumerator[array_size];
		_now_begin_coroutines = new List<IEnumerator>(array_size);
		_nest_table = new Dictionary<IEnumerator, IEnumerator>(array_size);
		_link_object_table = new Dictionary<IEnumerator, GameObject>(array_size);
	}

	/// <summary>
	/// 実行開始
	/// </summary>
	/// <param name="routine">追加するコルーチン</param>
	/// <param name="game_object">紐づけるゲームオブジェクト(生存チェックをする場合に設定)</param>
	/// <returns>実行したコルーチン</returns>
	public IEnumerator Begin( IEnumerator routine, GameObject game_object = null ){

		if( game_object != null ){
			_link_object_table.Add( routine, game_object );
		}

		IEnumerator coroutine = routine;

		while( coroutine != null && !_now_begin_coroutines.Contains( coroutine ) ) {
			// update coroutine
			if( !coroutine.MoveNext() ){
				UnlinkGameObject( coroutine );
				IEnumerator parent;
				if( _nest_table.TryGetValue( coroutine, out parent )) {
					_nest_table.Remove( coroutine );
					coroutine = parent;
					continue;
				}
				break;
			}

			IEnumerator nest_coroutine = coroutine.Current as IEnumerator;
			if( nest_coroutine != null && !_nest_table.ContainsKey( nest_coroutine )) {
				_nest_table.Add( nest_coroutine, coroutine );
				coroutine = nest_coroutine;
				continue;
			}
			_now_begin_coroutines.Add( coroutine );

			// resize array
			if( _coroutines.Length == _update_coroutine_num ){
				Array.Resize( ref _coroutines, _update_coroutine_num * 2 );
				Debug.Log( "Resize Coroutine Array : " + _coroutines.Length );
			}
			// add coroutine
			_coroutines[_update_coroutine_num++] = coroutine;
		}
		return coroutine;
	}
	/// <summary>
	/// コルーチン強制停止
	/// </summary>
	/// <param name="routine">停止するコルーチン</param>
	public void End( IEnumerator routine ){
		if( routine == null ) {
			return;
		}

		int index = -1;
		for( int i = 0; i < _update_coroutine_num; ++i ) {
			IEnumerator itr = _coroutines[i];
			if( itr == null ) {
				continue;
			}
			if( itr == routine || itr == routine.Current ) {
				index = i;
				break;
			}
		}
		if( index < 0 ) {
			return;
		}
		IEnumerator coroutine = _coroutines[index];
		// 親も含めて全て停止.
		IEnumerator parent;
		while( coroutine != null ) {
			if( _nest_table.TryGetValue( coroutine, out parent )) {
				_nest_table.Remove( coroutine );
			}
			UnlinkGameObject( coroutine );
			coroutine = parent;
		}
		_coroutines[index] = null;
	}
	/// <summary>
	/// コルーチン更新
	/// </summary>
	public void ConsumeEnumerator(){
		int end_num = 0;
		for( int i = 0; i < _update_coroutine_num; ++i ){
			IEnumerator coroutine = _coroutines[i];
			if( coroutine == null ){
				end_num++;
				continue;
			}

			if( _now_begin_coroutines.Contains( coroutine )){
				continue;
			}
			if( _pause_coroutines.Contains( coroutine )){
				continue;
			}
			// game_object 生存チェック
			if( !IsAliveGameObject( coroutine )){
				End( coroutine );
				continue;
			}
			// update coroutine
			if( coroutine.MoveNext() ){
				// check nest coroutine
				IEnumerator nest_coroutine = coroutine.Current as IEnumerator;
				if( nest_coroutine == null || _nest_table.ContainsKey( nest_coroutine ) ) {
					continue;
				}
				_nest_table.Add( nest_coroutine, coroutine );
				Begin( nest_coroutine );
			} else {
				IEnumerator parent_coroutine;
				if( _nest_table.TryGetValue( coroutine, out parent_coroutine )) {
					Begin( parent_coroutine );
					_nest_table.Remove( coroutine );
				}
			}
			UnlinkGameObject( coroutine );
			_coroutines[i] = null;
			end_num++;
		}

		// find null
		for( int i = 0; i < _update_coroutine_num; ++i ){
			if( _coroutines[i] != null ) {
				continue;
			}
			for( int j = i+1; j < _update_coroutine_num; ++j ){
				// swap
				if(_coroutines[j] != null) {
					_coroutines[i] = _coroutines[j];
					_coroutines[j] = null;
					break;
				}
			}
		}

		_now_begin_coroutines.Clear();
		_update_coroutine_num -= end_num;
	}
	/// <summary>
	/// 一時停止
	/// </summary>
	/// <param name="routine">対象コルーチン</param>
	/// <param name="is_pause">true:一時停止. false:再開</param>
	/// <returns>成否</returns>
	public bool Pause( IEnumerator routine, bool is_pause ){
		if( is_pause ){
			if( _pause_coroutines.Contains(routine)){
				return false;
			}
			_pause_coroutines.Add( routine );
			return true;
		}else{
			return _pause_coroutines.Remove( routine );
		}
	}

	/// <summary>
	/// コルーチンの実行状態確認
	/// </summary>
	/// <param name="routine">判定対象</param>
	/// <returns>true:実行中</returns>
	public bool IsUpdating( IEnumerator routine ){
		if( routine == null ) {
			return false;
		}
		for( int i = 0; i < _update_coroutine_num; ++i ){
			IEnumerator coroutine = _coroutines[i];
			if( coroutine == null ) {
				continue;
			}
			if( coroutine == routine ) {
				return true;
			}
			if( routine.Current == coroutine ) {
				return true;
			}

			IEnumerator current = coroutine;
			IEnumerator parent = null;
			while( _nest_table.TryGetValue( current, out parent )) {
				if( parent == routine ) {
					return true;
				}
				if( parent == routine.Current ) {
					return true;
				}
				current = parent;
			}
		}
		return false;
	}

	/// <summary>
	/// 紐づけているゲームオブジェクト取得
	/// </summary>
	/// <param name="routine">コルーチン</param>
	/// <returns>ゲームオブジェクト</returns>
	public GameObject GetLinkGameObject( IEnumerator routine ){
		if( routine == null ){
			return null;
		}
		GameObject game_object;
		IEnumerator parent;
		while( routine != null ) {
			if( _link_object_table.TryGetValue( routine, out game_object )){
				return game_object;
			}
			_nest_table.TryGetValue( routine, out parent );
			routine = parent;
		}
		return null;
	}
	/// <summary>
	/// 紐づきゲームオブジェクト存在有無チェック
	/// </summary>
	/// <param name="routine">コルーチン</param>
	/// <returns>true:有り false:無し</returns>
	bool IsAliveGameObject( IEnumerator routine ){
		GameObject game_object;
		IEnumerator parent;
		while( routine != null ) {
			if( _link_object_table.TryGetValue( routine, out game_object )){
				// null or deactive
				if( game_object == null || !game_object.activeSelf ){
					return false;
				}
			}
			_nest_table.TryGetValue( routine, out parent );
			routine = parent;
		}
		return true;
	}
	/// <summary>
	/// 紐づけ解除
	/// </summary>
	/// <param name="routine">コルーチン</param>
	void UnlinkGameObject( IEnumerator routine ){
		if( _link_object_table.ContainsKey( routine )){
			_link_object_table.Remove( routine );
		}
	}

}
