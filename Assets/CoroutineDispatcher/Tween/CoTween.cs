using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CoTween : MonoBehaviour{

	public enum ePlayType{
		PlayOnce,
		Loop,
		PingPong,
	}
	public ePlayType playType = ePlayType.PlayOnce;

	[SerializeField]
	AnimationCurve _ease_curve = new AnimationCurve(
		new Keyframe[]{
			new Keyframe(0.0f,0.0f), new Keyframe(1.0f,1.0f) 
		}
	);
	[SerializeField]
	float _duration = 1.0f;
	[SerializeField]
	float _delay = 0.0f;
	[SerializeField]
	bool _is_ignore_timescale = false;
	[SerializeField]
	bool _is_reverse = false;

	public bool isPlaying { 
		get{
			if( _execute == null ){
				return false;
			}
			return _execute.IsUpdating();
		}
	}

	[SerializeField]
	string _group_name = "";

	public string groupName{
		get{
			return _group_name;
		}
	}

	IEnumerator _execute = null;

	protected virtual void Start(){
		if( Application.isPlaying ){
			_Begin();
		}
	}

	public abstract void ValidateValue( float value );

	public IEnumerator Play( bool is_reverse = false ){
		Reset( is_reverse );
		return _Begin();
	}
	public void Stop(){
		Co.End( _execute );
		_execute = null;
	}
	public void Reset( bool is_reverse ){
		_is_reverse = is_reverse;
		Co.End( _execute );
		_execute = null;
		_EvaluateValue( is_reverse ? _duration : 0.0f );
	}

	protected static float Ease( float from, float to, float value ){
		return from * ( 1.0f - value ) + to * value;
	}

	IEnumerator _Begin(){
		Co.End( _execute );
		_execute = this.BeginCoroutine( _Execute());
		return _execute;
	}
	

	IEnumerator _Execute(){
		// delay
		yield return Wait.Seconds(_delay, _is_ignore_timescale );
		
		// current time update
		var current_time = 0.0f;
		while( true ){
			if( _is_reverse ) {
				if( _is_ignore_timescale ) {
					current_time -= Co.unscaledDeltaTime;
				} else {
					current_time -= Co.deltaTime;
				}
			} else {
				if( _is_ignore_timescale ) {
					current_time += Co.unscaledDeltaTime;
				} else {
					current_time += Co.deltaTime;
				}
			}

			bool is_end =  !_is_reverse && current_time > _duration;
			bool is_reverse_end = is_end ? false : _is_reverse && current_time < 0.0f;
			
			// validate value
			switch( playType ){
				case ePlayType.PlayOnce:
					if( is_end ){
						current_time = _duration;
						_EvaluateValue( 1.0f );
						yield break;
					}else if (  is_reverse_end ){
						current_time = 0.0f;
						_EvaluateValue( 0.0f );
						yield break;
					}else{
						_EvaluateValue( current_time / _duration );
					}
					break;
				case ePlayType.Loop:
					if( is_end ) {
						current_time -= _duration;
					}else if( is_reverse_end ) {
						current_time += _duration;
					}
					_EvaluateValue( current_time / _duration );
					break;
				case ePlayType.PingPong:
					if( is_end || is_reverse_end ) {
						_is_reverse = !_is_reverse;
					}
					_EvaluateValue( current_time / _duration );
					break;
			}
			yield return null;
		}
	}

	void _EvaluateValue( float time ){
		ValidateValue( _ease_curve.Evaluate( time ));
	}
}
