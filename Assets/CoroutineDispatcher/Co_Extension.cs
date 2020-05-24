using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Co_Extension{

	/// <summary>
	/// コルーチン実行
	/// </summary>
	/// <param name="mono">紐づけるオブジェクト</param>
	/// <param name="coroutine">コルーチン</param>
	/// <param name="type">更新タイプ</param>
	/// <returns></returns>
	public static IEnumerator BeginCoroutine( this MonoBehaviour mono, IEnumerator coroutine, CoroutineDispatcher.eUpdateType type = CoroutineDispatcher.eUpdateType.Update ){
		return Co.Begin( coroutine, mono.gameObject, type );
	}

	/// <summary>
	/// コルーチン開始
	/// </summary>
	/// <param name="routine">コルーチン</param>
	/// <param name="type">更新タイプ</param>
	/// <returns>実行したコルーチン</returns>
	public static IEnumerator Begin( this IEnumerator routine, CoroutineDispatcher.eUpdateType type = CoroutineDispatcher.eUpdateType.Update ){
		return Co.Begin( routine, type );
	}

	/// <summary>
	/// コルーチン停止
	/// </summary>
	/// <param name="routine">停止するコルーチン</param>
	/// <param name="type">更新タイプ</param>
	public static IEnumerator End( this IEnumerator routine, CoroutineDispatcher.eUpdateType type = CoroutineDispatcher.eUpdateType.Update ){
		Co.End( routine, type );
		return routine;
	}
	/// <summary>
	/// コルーチン一時停止
	/// </summary>
	/// <param name="coroutine">実行するコルーチン</param>
	/// <param name="is_pause">true:一時停止. false:再開</param>
	/// <param name="type">更新タイプ</param>
	/// <returns>成否</returns>
	public static bool Pause( this IEnumerator routine, bool is_pause, CoroutineDispatcher.eUpdateType type = CoroutineDispatcher.eUpdateType.Update ){
		return Co.Pause( routine, is_pause, type );
	}
	/// <summary>
	/// コルーチン終了後にコルーチンを再生
	/// </summary>
	/// <param name="routine">コルーチン</param>
	/// <param name="next">次に再生したいコルーチン</param>
	/// <returns></returns>
	public static IEnumerator Then( this IEnumerator routine, IEnumerator next ) {
		yield return routine;
		yield return next;
	}

	/// <summary>
	/// アクション実行.
	/// </summary>
	/// <param name="routine">コルーチン</param>
	/// <param name="action">コルーチン終了後に実行する処理</param>
	/// <returns></returns>
	public static IEnumerator Then( this IEnumerator routine, System.Action action ) {
		yield return routine;
		action?.Invoke();
		yield return routine;
	}

	/// <summary>
	/// コルーチン実行後にディレイ.
	/// </summary>
	/// <param name="routine">コル―チン</param>
	/// <param name="duration">待機時間</param>
	/// <returns></returns>
	public static IEnumerator Delay( this IEnumerator routine, float duration ) {
		yield return routine;
		float start_time = Time.time;
		while( Time.time - start_time < duration ) {
			yield return null;
		}
	}
	/// <summary>
	/// 実行終了待ち.
	/// </summary>
	/// <param name="routine">コルーチン</param>
	/// <param name="type">更新タイプ</param>
	public static IEnumerator Wait( this IEnumerator routines ){
		while( routines.IsUpdating()) {
			yield return null;
		}
	}
	/// <summary>
	/// 実行終了待ち.
	/// </summary>
	/// <param name="routine">コルーチン</param>
	/// <param name="type">更新タイプ</param>
	public static IEnumerator Wait( this IEnumerator[] routines ){
		for( int i = 0; i < routines.Length; ++i ){
			while( routines[i].IsUpdating()) {
				yield return null;
			}
		}
	}

	/// <summary>
	/// 実行終了待ち.
	/// </summary>
	/// <param name="routine">コルーチン</param>
	/// <param name="type">更新タイプ</param>
	public static IEnumerator Wait( this List<IEnumerator> routines ){
		for( int i = 0; i < routines.Count; ++i ){
			while( routines[i].IsUpdating()) {
				yield return null;
			}
		}
	}
	/// <summary>
	/// 実行中判定.
	/// </summary>
	/// <param name="routine">コルーチン</param>
	/// <param name="update_type">更新タイプ</param>
	/// <returns>実行中かどうか</returns>
	public static bool IsUpdating( this IEnumerator routine ){
		return Co.IsUpdating( routine );
	}
	/// <summary>
	/// 実行中判定.
	/// </summary>
	/// <param name="routines">コルーチン</param>
	/// <param name="update_type">更新タイプ</param>
	/// <returns>実行中かどうか</returns>
	public static bool IsUpdating( this List<IEnumerator> routines ){
		return Co.IsUpdating( routines );
	}
	/// <summary>
	/// 実行中判定.
	/// </summary>
	/// <param name="routines">コルーチン</param>
	/// <param name="update_type">更新タイプ</param>
	/// <returns>実行中かどうか</returns>
	public static bool IsUpdating( this IEnumerator[] routines ){
		return Co.IsUpdating( routines );
	}
}
