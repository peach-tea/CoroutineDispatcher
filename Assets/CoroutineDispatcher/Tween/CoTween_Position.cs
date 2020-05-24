using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoTween_Position : CoTween_Transform{

	protected override void Reset() {
		base.Reset();
		_from =	_transform.localPosition;
		_to = _from;
	}
	public override void ValidateValue(float value) {
		Vector3 position = GetValidateValue(value);
		if( _is_local ){
			_transform.localPosition = position;
		}else{
			_transform.position = position;
		}
	}
}
