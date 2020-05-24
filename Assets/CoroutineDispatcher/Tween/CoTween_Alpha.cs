using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoTween_Alpha : CoTween{

	[SerializeField]
	float _from = 0.0f;
	[SerializeField]
	float _to = 1.0f;

	[SerializeField]
	MaskableGraphic _graphic = null;
	[SerializeField]
	Renderer _renderer = null;
	[SerializeField]
	CanvasGroup _canvas_group = null;

	void Reset() {
		_graphic = GetComponent<MaskableGraphic>();
		_canvas_group = GetComponent<CanvasGroup>();
		_renderer = GetComponent<Renderer>();

		if( _graphic ){
			_from = _graphic.color.a;
		}else if( _canvas_group ){
			_from = _canvas_group.alpha;
		}else if( _renderer ){
#if UNITY_EDITOR
			if( !Application.isPlaying ){
				_from = _renderer.sharedMaterial.color.a;
				return;
			}
#endif
			_from = _renderer.material.color.a;
		}
	}

	public override void ValidateValue(float value) {
		if( !_renderer && !_graphic && !_canvas_group ){
			_graphic = GetComponent<MaskableGraphic>();
			_canvas_group = GetComponent<CanvasGroup>();
			_renderer = GetComponent<Renderer>();
		}
#if UNITY_EDITOR
		if( !_renderer && !_graphic && !_canvas_group ){
			Debug.LogWarning("not found alpha component.", this );
			return;
		}
#endif

		float alpha = Ease( _from, _to, value );
		if( _graphic ){
			Color color = Color.white;
			color =  _graphic.color;
			color.a = alpha;
			_graphic.color = color;
		}else if( _canvas_group ){
			_canvas_group.alpha = alpha;
		}else if( _renderer ){
			Color color = Color.white;
#if UNITY_EDITOR
			if( !Application.isPlaying ){
				color =  _renderer.sharedMaterial.color;
				color.a = alpha;
				_renderer.sharedMaterial.color = color;
				return;
			}
#endif
			color =  _renderer.material.color;
			color.a = alpha;
			_renderer.material.color = color;
		}
	}
}
