using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoTween_Color : CoTween{

	[SerializeField]
	Color _from = Color.white;
	[SerializeField]
	Color _to = Color.white;

	[SerializeField]
	MaskableGraphic _graphic = null;
	[SerializeField]
	Renderer _renderer = null;

	void Reset() {
		_graphic = GetComponent<MaskableGraphic>();
		_renderer = GetComponent<Renderer>();

		if( _graphic ){
			_from = _graphic.color;
		}else if( _renderer ){
#if UNITY_EDITOR
			if( !Application.isPlaying ){
				_from = _renderer.sharedMaterial.color;
				return;
			}
#endif
			_from = _renderer.material.color;
		}
	}

	public override void ValidateValue(float value) {
		if( !_renderer && !_graphic ){
			_graphic = GetComponent<MaskableGraphic>();
			_renderer = GetComponent<Renderer>();
		}
#if UNITY_EDITOR
		if( !_renderer && !_graphic ){
			Debug.LogWarning("not found renderer component.", this );
			return;
		}
#endif
		Color color = Color.white;
		color.r = Ease( _from.r, _to.r, value );
		color.g = Ease( _from.g, _to.g, value );
		color.b = Ease( _from.b, _to.b, value );
		if( _graphic ){
			_graphic.color = color;
		}else if( _renderer ){
#if UNITY_EDITOR
			if( !Application.isPlaying ){
				_renderer.sharedMaterial.color = color;
				return;
			}
#endif
			_renderer.material.color = color;
		}
	}
}
