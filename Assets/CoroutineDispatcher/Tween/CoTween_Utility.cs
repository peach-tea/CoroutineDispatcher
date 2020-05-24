using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoTween_Utility {

	public static List<CoTween> GetTweens( Transform root, string group_name = "" ){
		var tweens = root.GetComponentsInChildren<CoTween>();
		List<CoTween> result = new List<CoTween>(tweens.Length);
		foreach( var tween in tweens ){
			if( string.IsNullOrEmpty( group_name )){
				result.Add(tween);
				continue;
			}
			if( group_name == tween.groupName ){
				result.Add( tween );
			}
		}
		return result;
	}

}
