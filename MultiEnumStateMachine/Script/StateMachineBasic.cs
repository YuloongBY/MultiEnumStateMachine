/**
 * The MIT License (MIT)
 *
 * Copyright (c) 2022 YuloongBY - Github: github.com/YuloongBY
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of
 * this software and associated documentation files (the "Software"), to deal in
 * the Software without restriction, including without limitation the rights to
 * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
 * the Software, and to permit persons to whom the Software is furnished to do so,
 * subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 * COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステートベーシック基盤
/// </summary>
public abstract class ChildStateBasic
{
    public ChildStateBasic(){}
    ~ChildStateBasic(){}

    /// <summary>
    /// 親ステートマシン
    /// </summary>
    public StateMachineBasic ParentMachine_{ protected get; set; } = null;
        
    /// <summary>
    /// 時間を加算
    /// ※「Timer」がステート遷移を発生した時、自動的にクリア処理を行う
    /// </summary>
    /// <param name="_dt">DeltaTime</param>    
    protected void AddTimer( float _dt ){ if( ParentMachine_ != null ) ParentMachine_.AddTimer( _dt );}
    
    /// <summary>
    /// 時間を取得
    /// ※「Timer」がステート遷移を発生した時、自動的にクリア処理を行う
    /// </summary>    
    protected float GetTimer(){ return ParentMachine_ != null ? ParentMachine_.GetTimer() : -1.0f;}
    
    /// <summary>
    /// 時間をクリア
    /// ※「Timer」がステート遷移を発生した時、自動的にクリア処理を行う
    /// </summary>
    protected void ClearTimer(){ if( ParentMachine_ != null ) ParentMachine_.ClearTimer();}

    /// <summary>
    /// サブ時間を加算
    /// ※「SubTimer」がサブステップを変化した時、自動的にクリア処理を行う
    /// </summary>
    /// <param name="_dt">DeltaTime</param>
    protected void AddSubTimer( float _dt ){ if( ParentMachine_ != null ) ParentMachine_.AddSubTimer( _dt );}
    
    /// <summary>
    /// サブ時間を取得
    /// ※「SubTimer」がサブステップを変化した時、自動的にクリア処理を行う
    /// </summary>    
    protected float GetSubTimer(){ return ParentMachine_ != null ? ParentMachine_.GetSubTimer() : -1.0f;}
    
    /// <summary>
    /// サブ時間をクリア
    /// ※「SubTimer」がサブステップを変化した時、自動的にクリア処理を行う
    /// </summary>    
    protected void ClearSubTimer(){ if( ParentMachine_ != null ) ParentMachine_.ClearSubTimer();}

    /// <summary>
    /// サブステップを設定
    /// </summary>
    protected void SetSubStep( int _subStep ){ if( ParentMachine_ != null ) ParentMachine_.SetSubStep( _subStep );}
    
    /// <summary>
    /// サブステップを取得
    /// </summary>   
    protected int GetSubStep(){ return ParentMachine_ != null ? ParentMachine_.GetSubStep() : -1;}
        
    /// <summary>
    /// 開始
    /// </summary>
    /// <param name="_prevStateIdx">前ステートのインデックス</param>    
    virtual public void OnBegin( int _prevStateIdx ){}
    
    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="_dt">DeltaTime</param>
    virtual public void OnUpdate( float _dt ){}
    
    /// <summary>
    /// 終了
    /// </summary>
    /// <param name="_nextStateIdx">次ステートのインデックス</param>    
    virtual public void OnEnd( int _nextStateIdx ){}

    /// <summary>
    /// このステートに遷移できるかどうか判断
    /// </summary>    
    virtual public bool CanChangeState(){ return true;}

    /// <summary>
    /// デフォルトステートに遷移
    /// </summary>
    public void ToDefaultState()
    {
        if( ParentMachine_ != null ){ ParentMachine_.ToDefaultState();}
    }

    /// <summary>
    /// 現在ステートのインデックスを取得
    /// </summary>
    public int GetCurrentStateIdx(){ return ParentMachine_ != null ? ParentMachine_.GetCurrentStateIdx() : -1;}

    /// <summary>
    /// 現在ステートのクラスを取得
    /// </summary>
    public ChildStateBasic GetCurrentStateClass(){ return ParentMachine_ != null ? ParentMachine_.GetCurrentStateClass() : null;}
        
    /// <summary>
    /// 親ステートマシンを取得
    /// </summary>
    public T GetParentMachine<T>() where T : StateMachineBasic{ return ParentMachine_ as T;}
}
    
/// <summary>
/// ステートマシンベーシック基盤
/// </summary> 
public abstract class StateMachineBasic : IndexBase
{
    //デフォルトステートインデックス
    private int defaultStateIdx_ = -1;
        
    /// <summary>
    /// ステートマップ
    /// </summary    
    private Dictionary<int , ChildStateBasic> StateMap_{ get;set;} = new Dictionary<int , ChildStateBasic>( 32 );

    /// <summary>
    /// ステートステップ
    /// </summary>    
    private UpdateStep<int> StateStep_{ get;set;} = new UpdateStep<int>();
        
    /// <summary>
    /// ポーズ判断
    /// </summary>    
    public bool IsPause_{ get;set;} = false;
    
    /// <summary>
    /// アクティブ判断
    /// </summary>
    public bool IsActive_{ get; private set;} = false;
        
    /// <summary>
    /// 時間を加算
    /// ※「Timer」がステート遷移を発生した時、自動的にクリア処理を行う
    /// </summary>
    /// <param name="_dt">DeltaTime</param>
    public void AddTimer( float _dt ){ StateStep_.AddTimer( _dt );}
    
    /// <summary>
    /// 時間を取得
    /// ※「Timer」がステート遷移を発生した時、自動的にクリア処理を行う
    /// </summary>  
    public float GetTimer(){ return StateStep_.Timer;}

    /// <summary>
    /// 時間をクリア
    /// ※「Timer」がステート遷移を発生した時、自動的にクリア処理を行う
    /// </summary>
    public void ClearTimer(){ StateStep_.ClearTimer();}

    /// <summary>
    /// サブ時間を加算
    /// ※「SubTimer」がサブステップを変化した時、自動的にクリア処理を行う
    /// </summary>
    /// <param name="_dt">DeltaTime</param>
    public void AddSubTimer( float _dt ){ StateStep_.AddSubTimer( _dt );}

    /// <summary>
    /// サブ時間を取得
    /// ※「SubTimer」がサブステップを変化した時、自動的にクリア処理を行う
    /// </summary>    
    public float GetSubTimer(){ return StateStep_.SubTimer;}

    /// <summary>
    /// サブ時間をクリア
    /// ※「SubTimer」がサブステップを変化した時、自動的にクリア処理を行う
    /// </summary>        
    public void ClearSubTimer(){ StateStep_.ClearSubTimer();}

    /// <summary>
    /// サブステップを設定
    /// </summary>
    public void SetSubStep( int _subStep ){ StateStep_.SetSubStep( _subStep );}
    
    /// <summary>
    /// サブステップを取得
    /// </summary>   
    public int GetSubStep(){ return StateStep_.SubStep;}

    public StateMachineBasic(){}
    ~StateMachineBasic()
    {
        OnEnd();
    }

    /// <summary>
    /// 開始
    /// </summary>
    virtual protected void OnBegin(){}

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="_dt">DeltaTime</param>
    virtual protected void OnUpdate( float _dt ){}

    /// <summary>
    /// 終了
    /// </summary>
    virtual protected void OnEnd(){}

    /// <summary>
    /// ステート遷移した時呼ばれる
    /// </summary>
    /// <param name="_prevStateIdx">前ステートのインデックス</param>
    virtual protected void OnChangeState( int _prevStateIdx ){}

    /// <summary>
    /// 更新（外部実行）
    /// </summary>
    /// <param name="_dt">DeltaTime</param>
    public void Update( float _dt )
    {
        if( IsPause_ || !IsActive_ ) return;

        //更新
        OnUpdate( _dt );

        //ステップ更新
        StateStep_.Update( _dt );
    }

    /// <summary>
    /// すべてのステートをクリア
    /// </summary>
    public void ClearAllState()
    {
        StateMap_.Clear();
        StateStep_.ClearUpdateDelegate();
    }

    /// <summary>
    /// 現在ステートのインデックスを取得
    /// </summary>
    public int GetCurrentStateIdx()
    {
        return StateStep_.Step;
    }

    /// <summary>
    /// 現在ステートのクラスを取得
    /// </summary>
    public ChildStateBasic GetCurrentStateClass()
    {
        if( StateMap_.ContainsKey( StateStep_.Step ))
        {
            return StateMap_[ StateStep_.Step ];
        }
        return null;
    }

    /// <summary>
    /// ステートのクラスを取得
    /// </summary>
    public W GetStateIdxClass<W>( int _stateIdx ) where W : ChildStateBasic
    {
        return GetStateIdxClass( _stateIdx ) as W;
    }

    /// <summary>
    /// ステートのクラスを取得
    /// </summary>
    public ChildStateBasic GetStateIdxClass( int _stateIdx )
    {
        if (StateMap_.ContainsKey( _stateIdx ))
        {
            return StateMap_[ _stateIdx ];
        }
        return null;
    }

    /// <summary>
    /// デフォルトステートに遷移
    /// </summary>
    public void ToDefaultState()
    {
        if (defaultStateIdx_ >= 0)
        {
            SetStateIdx(defaultStateIdx_);
        }
        else
        {
            Debug.LogError("デフォルトステートが設定されてない");
        }
    }

    /// <summary>
    /// ステートインデックスを登録
    /// </summary>
    /// <param name="_stateIdx">ステートインデックス</param>
    /// <param name="_childState">ステートクラス</param>
    /// <param name="_isDefaultState">デフォルトステートにとして登録</param>
    protected void RegisterStateIdx( int _stateIdx , ChildStateBasic _childState , bool _isDefaultState )
    {
        if( !StateMap_.ContainsKey( _stateIdx ))
        {
            if( _isDefaultState){ defaultStateIdx_ = _stateIdx;}
            _childState.ParentMachine_ = this;
            StateMap_.Add( _stateIdx , _childState );
            StateStep_.AddUpdateDelegate( _stateIdx , _childState.OnUpdate );
        }
    }

    /// <summary>
    /// ステートインデックスを削除
    /// </summary>
    /// <param name="_stateIdx">ステートインデックス</param>
    protected void RemoveStateIdx( int _stateIdx )
    {
        if( StateMap_.ContainsKey( _stateIdx ))
        {
            if( _stateIdx == defaultStateIdx_ ) defaultStateIdx_ = -1;
            StateMap_.Remove( _stateIdx );
            StateStep_.RemoveUpdateDelegate( _stateIdx );
        }
    }

    /// <summary>
    /// 開始ステートインデックスを設定
    /// </summary>
    protected void BeginStateIdx( int _stateIdx )
    {
        if( StateMap_.ContainsKey( _stateIdx ))
        {
            IsActive_ = true;
            StateStep_.InitStep( _stateIdx );
            OnChangeState( -1 );
            ChildStateBasic stateClass = GetCurrentStateClass();
            if( stateClass != null ){ stateClass.OnBegin( -1 );}
        }
    }
        
    /// <summary>
    /// ステートインデックスを設定
    /// </summary>
    protected bool SetStateIdx( int _stateIdx )
    {
        if( !IsActive_ )
        {
            Debug.LogError( "BeginState処理を行った後、実行可能になる" );
            return false;
        }

        //ステート遷移可能かどうか（デフォルトステートに遷移する場合、判断不要）
        if( _stateIdx != defaultStateIdx_ )
        {
            bool isSuccess = CanChangeStateIdx( _stateIdx );
            if( !isSuccess ){ return false;}
        }
            
        //ステート終了処理を実行
        ChildStateBasic endStateClass = GetCurrentStateClass();
        if( endStateClass != null ){ endStateClass.OnEnd( StateStep_.Step );}

        //新しいステートに遷移
        int prevStateIdx = StateStep_.Step;
        StateStep_.SetStepOnly( _stateIdx );
        OnChangeState( prevStateIdx );

        //ステート開始処理を実行
        ChildStateBasic beginClass = GetCurrentStateClass();
        if( beginClass != null ){ beginClass.OnBegin( prevStateIdx );}

        return true;
    }

    /// <summary>
    /// 現在のステートインデックスと同じかどうか判断
    /// </summary>
    protected bool IsEqualsCurrentStateIdx( int _checkStateIdx )
    {
        return StateStep_.Step == _checkStateIdx;
    }

    /// <summary>
    /// ステート遷移可能かどうか判断
    /// </summary>
    protected bool CanChangeStateIdx( int _stateIdx )
    {
        ChildStateBasic stateClass = GetStateIdxClass( _stateIdx );
        if( stateClass == null || stateClass.CanChangeState() == false )
        {
            return false;
        }
        return true;
    }
}

/// <summary>
/// ステートベーシック
/// </summary>
public abstract class ChildStateBasic<ACTOR> : ChildStateBasic where ACTOR : MonoBehaviour
{
    /// <summary>
    /// オーナー
    /// </summary>    
    protected ACTOR Owner_{ get{ return GetParentMachine<StateMachineBasic<ACTOR>>() != null ? GetParentMachine<StateMachineBasic<ACTOR>>().Owner_ : null;}}
}

/// <summary>
/// ステートマシンベーシック
/// </summary>
public abstract class StateMachineBasic<ACTOR> : StateMachineBasic where ACTOR : MonoBehaviour
{
    /// <summary>
    /// オーナー
    /// </summary>    
    public ACTOR Owner_{ get; private set;} = null;
    
    public StateMachineBasic( ACTOR _owner ) : base()
    { 
        Owner_ = _owner;
        OnBegin();
    }
    ~StateMachineBasic(){}
}   
