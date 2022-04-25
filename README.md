[![license](https://img.shields.io/badge/license-MIT-brightgreen.svg?style=flat-square)](https://github.com/YuloongBY/MultiEnumStateMachine/blob/main/LICENSE)

# MultiEnumStateMachine

複数ステートリストが持てるステートマシン
 
Statemachine that can support multiple statelists
 
```diff
- ジェネリッククラスの制約条件にSystem.Enumが利用されたところがあるので、C#のバージョンが7.3以上が必要になります。
``` 
## 紹介
　ステートマシンは基本的に一つステートリスト（Enum利用する場合が多い）しか持つことができませんが、実際利用した時、親クラスが汎用ステートリストを持って、子クラスが個別専用ステートリストを持ちたい場合がたくさんありました。もちろん、親クラスの汎用ステートリストに「FreeState」みたいな子クラスから自由的に使えるステートを追加することが方法の一つですが、やや不便なので、根本的に解決するため、複数ステートリストが持てるステートマシンを作成しました。

## 特徴
### ・複数ステートリストが持てる
　※現在最大3つステートリストまで持てますが、拡張可能です。
### ・Garbage Collection発生しない

### ・Monobehaviourの機能を使わないので、処理が速いし、移植も簡単

## 使い方
　パッケージ中のサンプルを参考にしてください。

  ![Image](https://github.com/YuloongBY/BYImage/blob/main/MultiEnumStateMachine/MultiEnumStateMachineSample.gif)
  ※「Idle」ステートが親クラスの汎用ステートリスト中にあり、「Color」、「Move」、「Zoom」ステートが子クラスの専用ステートリスト中にあるような構造です。

## API
### StateMachineBasic
  ステートマシン基盤クラス
  
#### Public Methods
<details>
  <summary>details</summary>
 
```csharp
/// <summary>
/// 時間を加算
/// ※「Timer」がステート遷移を発生した時、自動的にクリア処理を行う
/// </summary>
/// <param name="_dt">DeltaTime</param>
public void AddTimer( float _dt )
```

```csharp
/// <summary>
/// 時間を取得
/// ※「Timer」がステート遷移を発生した時、自動的にクリア処理を行う
/// </summary>  
public float GetTimer()
```

```csharp
/// <summary>
/// 時間をクリア
/// ※「Timer」がステート遷移を発生した時、自動的にクリア処理を行う
/// </summary>
public void ClearTimer()
```

```csharp
/// <summary>
/// サブ時間を加算
/// ※「SubTimer」がサブステップを変化した時、自動的にクリア処理を行う
/// </summary>
/// <param name="_dt">DeltaTime</param>
public void AddSubTimer( float _dt )
```

```csharp
/// <summary>
/// サブ時間を取得
/// ※「SubTimer」がサブステップを変化した時、自動的にクリア処理を行う
/// </summary>    
public float GetSubTimer()
```

```csharp
/// <summary>
/// サブ時間をクリア
/// ※「SubTimer」がサブステップを変化した時、自動的にクリア処理を行う
/// </summary>        
public void ClearSubTimer()
```

```csharp
/// <summary>
/// サブステップを設定
/// </summary>
public void SetSubStep( int _subStep )
```

```csharp
/// <summary>
/// サブステップを取得
/// </summary>   
public int GetSubStep()
```

```csharp
/// <summary>
/// 更新（外部実行）
/// </summary>
/// <param name="_dt">DeltaTime</param>
public void Update( float _dt )
```

```csharp
/// <summary>
/// すべてのステートをクリア
/// </summary>
public void ClearAllState()
```

```csharp
/// <summary>
/// 現在ステートのインデックスを取得
/// </summary>
public int GetCurrentStateIdx()
```  

```csharp
/// <summary>
/// 現在ステートのクラスを取得
/// </summary>
public ChildStateBasic GetCurrentStateClass()
```  

```csharp
/// <summary>
/// ステートのクラスを取得
/// </summary>
public W GetStateIdxClass<W>( int _stateIdx ) where W : ChildStateBasic
```  

```csharp
/// <summary>
/// ステートのクラスを取得
/// </summary>
public ChildStateBasic GetStateIdxClass( int _stateIdx )
```  

```csharp
/// <summary>
/// デフォルトステートに遷移
/// </summary>
public void ToDefaultState()
```  

```csharp
/// <summary>
/// ポーズ判断
/// </summary>    
public bool IsPause_{ get;set;}
```  

```csharp
/// <summary>
/// アクティブ判断
/// </summary>
public bool IsActive_{ get; private set;}
```

</details>

#### Virtual Methods
<details>
  <summary>details</summary>
 
```csharp
/// <summary>
/// 開始
/// </summary>
virtual protected void OnBegin()
```

```csharp
/// <summary>
/// 更新
/// </summary>
/// <param name="_dt">DeltaTime</param>
virtual protected void OnUpdate( float _dt )
```

```csharp
/// <summary>
/// 終了
/// </summary>
virtual protected void OnEnd()
```

```csharp
/// <summary>
/// ステート遷移した時呼ばれる
/// </summary>
/// <param name="_prevStateIdx">前ステートのインデックス</param>
virtual protected void OnChangeState( int _prevStateIdx )
```
</details> 

---

### ChildStateBasic
  ステート基盤クラス

#### Public Methods
<details>
  <summary>details</summary>
 
```csharp
/// <summary>
/// 親ステートマシン
/// </summary>
public StateMachineBasic ParentMachine_{ protected get; set; } 
```

```csharp
/// <summary>
/// デフォルトステートに遷移
/// </summary>
public void ToDefaultState()
```

```csharp
/// <summary>
/// 現在ステートのインデックスを取得
/// </summary>
public int GetCurrentStateIdx()
```

```csharp
/// <summary>
/// 現在ステートのクラスを取得
/// </summary>
public ChildStateBasic GetCurrentStateClass()
```

```csharp
/// <summary>
/// 親ステートマシンを取得
/// </summary>
public T GetParentMachine<T>() where T : StateMachineBasic
```
</details>
 
#### Protected Methods

<details>
  <summary>details</summary> 
 
```csharp
/// <summary>
/// 時間を加算
/// ※「Timer」がステート遷移を発生した時、自動的にクリア処理を行う
/// </summary>
/// <param name="_dt">DeltaTime</param>    
protected void AddTimer( float _dt )
```

```csharp
/// <summary>
/// 時間を取得
/// ※「Timer」がステート遷移を発生した時、自動的にクリア処理を行う
/// </summary>    
protected float GetTimer()
```

```csharp
/// <summary>
/// 時間をクリア
/// ※「Timer」がステート遷移を発生した時、自動的にクリア処理を行う
/// </summary>
protected void ClearTimer()
```

```csharp
/// <summary>
/// サブ時間を加算
/// ※「SubTimer」がサブステップを変化した時、自動的にクリア処理を行う
/// </summary>
/// <param name="_dt">DeltaTime</param>
protected void AddSubTimer( float _dt )
```

```csharp
/// <summary>
/// サブ時間を取得
/// ※「SubTimer」がサブステップを変化した時、自動的にクリア処理を行う
/// </summary>    
protected float GetSubTimer()
```

```csharp
/// <summary>
/// サブ時間をクリア
/// ※「SubTimer」がサブステップを変化した時、自動的にクリア処理を行う
/// </summary>    
protected void ClearSubTimer()
```

```csharp
/// <summary>
/// サブステップを設定
/// </summary>
protected void SetSubStep( int _subStep )
```

```csharp
/// <summary>
/// サブステップを取得
/// </summary>   
protected int GetSubStep()
```

</details>

#### Virtual Methods

<details>
  <summary>details</summary>

```csharp
/// <summary>
/// 開始
/// </summary>
/// <param name="_prevStateIdx">前ステートのインデックス</param>    
virtual public void OnBegin( int _prevStateIdx )
```

```csharp
/// <summary>
/// 更新
/// </summary>
/// <param name="_dt">DeltaTime</param>
virtual public void OnUpdate( float _dt )
```

```csharp
/// <summary>
/// 終了
/// </summary>
/// <param name="_nextStateIdx">次ステートのインデックス</param>    
virtual public void OnEnd( int _nextStateIdx )
```

```csharp
/// <summary>
/// このステートに遷移できるかどうか判断
/// </summary>    
virtual public bool CanChangeState()
```

</details>

---

### StateMachineBase
  ステートマシンベースクラス
  
  ステートマシンを作成する時、 Enum利用数によって、継承されるステートマシンベースクラスが違う
```
  StateMachineBase<ENUM,ACTOR>
  StateMachineBase<ENUM_1,ENUM_2,ACTOR>  
  StateMachineBase<ENUM_1,ENUM_2,ENUM_3,ACTOR>
```

#### Public Methods
<details>
  <summary>details</summary>

```csharp
/// <summary>
/// ステートを登録
/// </summary>
/// <param name="_state">ステート</param>
/// <param name="_childState">ステートクラス</param>
/// <param name="_isDefaultState">デフォルトステートにとして利用</param>
public void RegisterState( ENUM _state , ChildStateBasic _childState , bool _isDefaultState = false )
``` 
 
```csharp
/// <summary>
/// ステートを削除
/// </summary>
public void RemoveState( ENUM _state )
```

```csharp
/// <summary>
/// ステートクラスを取得
/// </summary>
public ChildStateBasic GetStateClass( ENUM _state )
```

```csharp
/// <summary>
/// ステートクラスを取得
/// </summary>
public CHILD_CLASS GetStateClass<CHILD_CLASS>( ENUM _state ) where CHILD_CLASS : ChildStateBasic
```

```csharp
/// <summary>
/// 初期ステートを設定
/// </summary>
public void BeginState( ENUM _state )
```

```csharp
/// <summary>
/// ステートを設定
/// </summary>
public bool SetState( ENUM _state )
```

```csharp
/// <summary>
/// 指定したステートは現在のステートと同じかどうか判断
/// </summary>
public bool IsEqualsCurrentState( ENUM _state )
```

```csharp
/// <summary>
/// ステート遷移可能かどうか判断
/// </summary>
public bool CanChangeState( ENUM _state )
```

```csharp
/// <summary>
/// ステートとインデックスの一致性を判断
/// </summary>
public bool IsStateEqualsIdx( ENUM _state , int _idx )
```

```csharp
/// <summary>
/// ステートインデックスを取得
/// </summary>
public int GetStateIdx( ENUM _state )
```

```csharp
/// <summary>
/// ステートを取得
/// </summary>
public bool GetState( out ENUM _state , int _idx )
```

 </details>

---

### ChildStateBase
  ステートベースクラス
  
  ステート処理を作成する時、 Enum利用数によって、継承されるステートベースクラスが違う
```
  ChildStateBase<ENUM,ACTOR>
  ChildStateBase<ENUM_1,ENUM_2,ACTOR>  
  ChildStateBase<ENUM_1,ENUM_2,ENUM_3,ACTOR>
```
#### Public Methods
<details>
  <summary>details</summary>

```csharp
/// <summary>
/// 指定ステートは現在のステートと同じかどうか判断
/// </summary>
public bool IsEqualsCurrentState( ENUM _state )
``` 
 
```csharp
/// <summary>
/// ステートとインデックスの一致性を判断
/// </summary>
public bool IsStateEqualsIdx( ENUM _state , int _idx )
``` 
 
```csharp
/// <summary>
/// ステート遷移可能かどうか判断
/// </summary>
public bool CanChangeState( ENUM _state )
``` 
 
```csharp
/// <summary>
/// ステートを設定
/// </summary>
public bool SetState( ENUM _state )
```
 
```csharp
/// <summary>
/// ステートを設定
/// </summary>
public bool SetState( ENUM _state )
``` 
 
```csharp
/// <summary>
/// ステートクラスを取得
/// </summary>
public ChildStateBasic GetStateClass( ENUM _state )
``` 
 
```csharp
/// <summary>
/// ステートクラスをを取得
/// </summary>
public CHILD_CLASS GetStateClass<CHILD_CLASS>( ENUM _state ) where CHILD_CLASS : ChildStateBasic
```

```csharp
/// <summary>
/// ステートを取得
/// </summary>
public bool GetState( out ENUM _state , int _idx )
``` 
 
</details>



