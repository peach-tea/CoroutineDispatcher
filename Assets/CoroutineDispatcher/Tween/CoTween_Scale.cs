using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoTween_Scale : CoTween_Transform{

	protected override void Reset() {
		base.Reset();
		_from =	_transform.localScale;
		_to = _from;
	}

	public override void ValidateValue(float value) {
		Vector3 scale = GetValidateValue(value);
		_transform.localScale = scale;
	}
}
