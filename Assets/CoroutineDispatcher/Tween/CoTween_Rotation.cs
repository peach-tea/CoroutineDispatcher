using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoTween_Rotation : CoTween_Transform{

	protected override void Reset() {
		base.Reset();
		_from =	_transform.localEulerAngles;
		_to = _from;
	}

	public override void ValidateValue(float value) {
		Vector3 rotation = GetValidateValue(value);
		if( _is_local ){
			_transform.localEulerAngles = rotation;
		}else{
			_transform.eulerAngles = rotation;
		}
	}
}
