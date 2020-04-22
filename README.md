# CoroutineDispatcher

## 概要
　コルーチンを管理・実行する機能です。

## UnityのStartCoroutineとの違いは？
* 全コルーチンの管理、制御が可能。
* 速い＆即時実行。
* Editorでもそのまま動く。

## 使い方
　StartCoroutineを次の関数に置き換えるだけです。
 
* 自身(MonoBehavior)に紐づけて実行する場合

  　this.BeginCoroutine(SampleA());   
  　UntiyのStartCoroutineと同様、  
　　ゲームオブジェクトが非アクティブになる or ゲームオブジェクトが削除される  
　　と停止します。
    
* CoroutineDispatherに実行させる場合

　　Co.Begin(Test());  
　　BeginCoroutineと違い、明示的に止めない限りは停止しません。  
　　※Co.End(Test());

## 全コルーチンの管理、制御
　インスペクタに各コルーチンの実行状態が表示されます。  
　関数提供していないですが、すべてのコルーチンを一時停止したり、強制終了したりも可能です。  
　※実装予定

![CoroutineDispatcherInspector](https://user-images.githubusercontent.com/20371710/79996451-713d8d00-84f3-11ea-9a84-af1e082da6a5.png)

## 速度比較
　空の関数(Test)を10000回実行。

* UnityStartCoroutine
```
UnityEngine.Profiling.Profiler.BeginSample("UnityStartCoroutine ");
for( int i = 0; i < 10000; ++i ){
	StartCoroutine( Test() );
}
UnityEngine.Profiling.Profiler.EndSample();
```
![UnityStartCoroutine](https://user-images.githubusercontent.com/20371710/79993393-ac3dc180-84ef-11ea-8699-de60065b2958.png)

* CoroutineDispatcher
```
UnityEngine.Profiling.Profiler.BeginSample("CoroutineDispatcher");
for( int i = 0; i < 10000; ++i ){
	Co.Begin(Test());
}
UnityEngine.Profiling.Profiler.EndSample();
```
![CoroutineDispatcher](https://user-images.githubusercontent.com/20371710/79994075-79e09400-84f0-11ea-8447-30608c4737fb.png)

## 即時実行
　コルーチンを実行したタイミングで処理を行うので実行遅延がないです。  
　yield break;しかしてないからすぐ次の処理に行くと思っていると  
　UnityのStartCoroutineでは遅延が起こるケースがあるので対策しています。
```
IEnumerator SampleA(){
	float start_time = Time.time;
	yield return SampleB();
	Debug.Log( "elapsed time : " + (Time.time - start_time) );
}
IEnumerator SampleB(){
	yield break;
}
```
* UnityのStartCorotineで実行した場合

　　elapsed time : 0.008575201

* CoroutineDispatcherの場合

　　elapsed time : 0

## Editor実行時
　次のようなコルーチンも、そのままUnityEditorスクリプトで実行することが可能になっています。
```
[MenuItem("CoroutineDispatcher/EditorRun")]
static void Test(){
	Co.Begin(CoroutineTest());
}

static IEnumerator CoroutineTest(){
	Debug.Log("Test Start");
	yield return WaitSeconds(1.0f);
	Debug.Log("Test End");
}
static IEnumerator WaitSeconds( float second ){
	var startTime = System.DateTimeOffset.UtcNow;
	while (true){
		yield return null;

		var elapsed = (System.DateTimeOffset.UtcNow - startTime).TotalSeconds;
		if (elapsed >= second){
			break;
		}
	};
}
```
