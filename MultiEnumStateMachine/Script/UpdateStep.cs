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

//============================================================
//      ステップ処理
//
//      switch( updateStep_.SubStep )
//      {
//          case ( int )SUB_STEP.SUB_STEP_INIT:         
//          break;
//          case 0:
//          {
//          }                    
//          break;
//          case ( int )SUB_STEP.SUB_STEP_FINISH:          
//          break;
//      }
//============================================================

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステップ状態
/// </summary>
public enum SUB_STEP : int
{
    SUB_STEP_INIT        = -1  ,    //初期化
    SUB_STEP_FINISH      = 128 ,    //終了      
};

/// <summary>
/// ステップ処理
/// </summary>
public class UpdateStep<T> where T : struct, IConvertible
{
    //....变量....//      
    private T     step_         = default(T);    //ステップ
    private T     prevStep_     = default(T);    //前ステップ
    private int   subStep_      = 0;             //サブステップ
    private float timer_        = 0.0f;          //時間
    private float subTimer_     = 0.0f;          //サブ時間
    private bool  isInitFinish_ = false;         //初期化終了？
        
    private Dictionary< T , Action<float>> updateDelegateDic_;

    //....Bean....//
    public T     Step      { get{ return step_    ;}}
    public T     PrevStep  { get{ return prevStep_;}}
    public int   SubStep   { get{ return subStep_ ;}}
    public float Timer     { get{ return timer_   ;}}
    public float SubTimer  { get{ return subTimer_;}}
        
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public UpdateStep()
    {
        if( !typeof(T).IsEnum && !typeof(T).Equals( typeof(int)))
        {
            Debug.LogError("Enum形とInt形だけ使えます");                
            return;
        }
        updateDelegateDic_ = new Dictionary<T, Action<float>>();   
    }
       
    /// <summary>
    /// ステップ初期化
    /// </summary>
    public void InitStep( T _initStep )
    {
        prevStep_ = _initStep;
        step_     = _initStep;
        ClearTimer();
        SetSubStep(( int )SUB_STEP.SUB_STEP_INIT );
        Update( 0.0f );
        SetSubStep( 0 );         
        isInitFinish_ = true;
    }

    /// <summary>
    /// ステップを設定
    /// </summary>
    public void SetStep( T _nextStep )
    {
        if( !EqualityComparer<T>.Default.Equals( step_ , _nextStep ))
        {
            SetSubStep(( int ) SUB_STEP.SUB_STEP_FINISH );
            Update( 0.0f );                            
            prevStep_ = step_;
            step_     = _nextStep;
            ClearTimer();                
            SetSubStep(( int )SUB_STEP.SUB_STEP_INIT );
            Update( 0.0f );                
            SetSubStep( 0 );  
        }
    }

    /// <summary>
    /// ステップを設定
    /// ※SUB_STEP_INITとSUB_STEP_FINISH呼ばない
    /// </summary>
    public void SetStepOnly( T _nextStep )
    {
        if( !EqualityComparer<T>.Default.Equals( step_ , _nextStep ))
        {                          
            prevStep_ = step_;
            step_     = _nextStep;
            ClearTimer();              
            SetSubStep( 0 );  
        }
    }
        
    /// <summary>
    /// サブステップを設定
    /// </summary>
    public void SetSubStep( int _subStep )
    {
        subStep_ = _subStep;
        ClearSubTimer();
    }

    /// <summary>
    /// 時間を加算
    /// </summary>
    public void AddTimer( float _dt )
    {
        timer_ += _dt;
    }

    /// <summary>
    /// サブ時間を加算
    /// </summary>
    public void AddSubTimer( float _dt )
    {
        subTimer_ += _dt;
    }

    /// <summary>
    /// 時間を設定
    /// </summary>
    public void SetTimer( float _time )
    {
        timer_ = _time;
    }

    /// <summary>
    /// サブ時間を設定
    /// </summary>
    public void SetSubTimer( float _time )
    {
        subTimer_ = _time;
    }

    /// <summary>
    /// 時間クリア
    /// </summary>
    public void ClearTimer()
    {
        timer_ = 0.0f;
    }

    /// <summary>
    /// サブ時間クリア
    /// </summary>
    public void ClearSubTimer()
    {
        subTimer_ = 0.0f;
    }
        
    /// <summary>
    /// コールバックを追加
    /// </summary>
    public void AddUpdateDelegate( T _step , Action<float> _delegate )
    {
        if( !updateDelegateDic_.ContainsKey( _step ))
        {
            updateDelegateDic_.Add( _step , _delegate );
        }           
    }

    /// <summary>
    /// コールバックを削除
    /// </summary>
    public void RemoveUpdateDelegate( T _step )
    {
        if( updateDelegateDic_.ContainsKey( _step ))
        {
            updateDelegateDic_.Remove( _step );
        }
    }

    /// <summary>
    /// すべてのコールバックを削除
    /// </summary>
    public void ClearUpdateDelegate()
    {
        updateDelegateDic_.Clear();
    }
        
    /// <summary>
    /// 更新
    /// </summary>
    public void Update( float _dt )
    {
        if( !isInitFinish_ ){ return;}

        //更新
        if( updateDelegateDic_.ContainsKey( step_ ))
        {
            T saveStep = step_; //ステップを前ステップに代入
            updateDelegateDic_[ step_ ].Invoke( _dt );                
        }
    }
}
