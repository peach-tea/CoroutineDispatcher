using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CoTween_Transform : CoTween{

	[SerializeField]
	protected Vector3 _from = new Vector3();
	[SerializeField]
	protected Vector3 _to = new Vector3();
	[SerializeField]
	protected Transform _transform = null;
	[SerializeField]
	protected bool _is_local = true;

	[SerializeField]
	bool _is_freeze_x = false;
	[SerializeField]
	bool _is_freeze_y = false;
	[SerializeField]
	bool _is_freeze_z = false;
	
	protected virtual void Reset() {
		_transform = GetComponent<Transform>();
	}

	protected Vector3 GetValidateValue( float value ){
		Vector3 vec3 = _from;
		if( !_is_freeze_x ){
			vec3.x = Ease( _from.x, _to.x, value );
		}
		if( !_is_freeze_y ){
			vec3.y = Ease( _from.y, _to.y, value );
		}
		if( !_is_freeze_z ){
			vec3.z = Ease( _from.z, _to.z, value );
		}
		return vec3;
	}
}
