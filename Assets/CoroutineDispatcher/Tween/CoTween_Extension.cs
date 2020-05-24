using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoTween_Extension {

	public static IEnumerator Play( this CoTween[] tweens, string group_name = "", bool is_reverse = false ){
		foreach( var tween in tweens ){
			if(string.IsNullOrEmpty( group_name )){
				Co.Begin( tween.Play( is_reverse ));
			}else if( tween.groupName == group_name ){
				Co.Begin( tween.Play( is_reverse ));
			}
		}
		foreach( var tween in tweens ){
			if( string.IsNullOrEmpty( group_name )){
				while( tween.isPlaying && tween.playType == CoTween.ePlayType.PlayOnce ){
					yield return null;
				}
			}else if( tween.groupName == group_name ){
				while( tween.isPlaying && tween.playType == CoTween.ePlayType.PlayOnce ){
					yield return null;
				}
			}
		}
	}
	public static void Stop( this CoTween[] tweens, string group_name = "" ){
		foreach( var tween in tweens ){
			if(string.IsNullOrEmpty( group_name )){
				tween.Stop();
			}else if( tween.groupName == group_name ){
				tween.Stop();
			}
		}
	}
	public static void Reset( this CoTween[] tweens, string group_name = "", bool is_reverse = false ){
		foreach( var tween in tweens ){
			if(string.IsNullOrEmpty( group_name )){
				tween.Reset( is_reverse );
			}else if( tween.groupName == group_name ){
				tween.Reset( is_reverse );
			}
		}
	}
}
