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

using System;
using UnityEngine;

//============================================================
// 1個ENUMを持つ場合
//============================================================
/// <summary>
/// ステートベース
/// </summary>
public abstract class ChildStateBase<ENUM,ACTOR> : ChildStateBasic<ACTOR> where ENUM  : Enum
                                                                          where ACTOR : MonoBehaviour
{
    public ChildStateBase(){}
    ~ChildStateBase(){}   
       
    /// <summary>
    /// 指定ステートは現在のステートと同じかどうか判断
    /// </summary>
    public bool IsEqualsCurrentState( ENUM _state )
    {      
        return GetParentMachine<StateMachineBase<ENUM,ACTOR>>() != null ? GetParentMachine<StateMachineBase<ENUM,ACTOR>>().IsEqualsCurrentState( _state ) : false;
    }

    /// <summary>
    /// ステートとインデックスの一致性を判断
    /// </summary>
    public bool IsStateEqualsIdx( ENUM _state , int _idx )
    {
        return GetParentMachine<StateMachineBase<ENUM,ACTOR>>() != null ? GetParentMachine<StateMachineBase<ENUM,ACTOR>>().IsStateEqualsIdx( _state , _idx ) : false;
    }

    /// <summary>
    /// ステート遷移可能かどうか判断
    /// </summary>
    public bool CanChangeState( ENUM _state )
    {
        return GetParentMachine<StateMachineBase<ENUM,ACTOR>>() != null ? GetParentMachine<StateMachineBase<ENUM,ACTOR>>().CanChangeState( _state ) : false;
    }

    /// <summary>
    /// ステートを設定
    /// </summary>
    public bool SetState( ENUM _state )
    {
        return GetParentMachine<StateMachineBase<ENUM,ACTOR>>() != null ? GetParentMachine<StateMachineBase<ENUM,ACTOR>>().SetState( _state ) : false;
    }

    /// <summary>
    /// ステートクラスを取得
    /// </summary>
    public ChildStateBasic GetStateClass( ENUM _state )
    {
        return GetParentMachine<StateMachineBase<ENUM,ACTOR>>() != null ? GetParentMachine<StateMachineBase<ENUM,ACTOR>>().GetStateClass( _state ) : null; 
    }
        
    /// <summary>
    /// ステートクラスをを取得
    /// </summary>
    public CHILD_CLASS GetStateClass<CHILD_CLASS>( ENUM _state ) where CHILD_CLASS : ChildStateBasic
    {
        return GetParentMachine<StateMachineBase<ENUM,ACTOR>>() != null ? GetParentMachine<StateMachineBase<ENUM,ACTOR>>().GetStateClass<CHILD_CLASS>( _state ) : null; 
    }
        
    /// <summary>
    /// ステートを取得
    /// </summary>
    public bool GetState( out ENUM _state , int _idx )
    {
        if( GetParentMachine<StateMachineBase<ENUM, ACTOR>>() != null )
        {
            return GetParentMachine<StateMachineBase<ENUM, ACTOR>>().GetState( out _state , _idx );
        }
        _state = default( ENUM );
        return false;
    }
}

/// <summary>
/// ステートマシンベース
/// </summary>
public abstract class StateMachineBase<ENUM,ACTOR> : StateMachineBasic<ACTOR> where ENUM  : Enum
                                                                              where ACTOR : MonoBehaviour
{
    //インデックス転換用
    private IdxConverter<ENUM> idxConverter_ = null;
    private IdxConverter<ENUM> IdxConverter_
    {
        get
        {
            if (idxConverter_ == null)
            {
                //インデックス転換処理
                idxConverter_ = new IdxConverter<ENUM>( this );
            }
            return idxConverter_;
        }
    }
        
    public StateMachineBase( ACTOR _owner ) : base( _owner ){}
    ~StateMachineBase(){}

    /// <summary>
    /// ステートを登録
    /// </summary>
    /// <param name="_state">ステート</param>
    /// <param name="_childState">ステートクラス</param>
    /// <param name="_isDefaultState">デフォルトステートにとして利用</param>
    public void RegisterState( ENUM _state , ChildStateBasic _childState , bool _isDefaultState = false )
    {
        //ステートを登録し、コンテントのインデックスを返す  
        int stateIdx = IdxConverter_.RegisterAndReturnIdx( _state );
        //インデックスを登録
        RegisterStateIdx( stateIdx , _childState , _isDefaultState );
    }

    /// <summary>
    /// ステートを削除
    /// </summary>
    public void RemoveState( ENUM _state )
    {
        int stateIdx = GetStateIdx( _state );
        RemoveStateIdx( stateIdx );
        IdxConverter_.RemoveContent( stateIdx );
    }

    /// <summary>
    /// ステートクラスを取得
    /// </summary>
    public ChildStateBasic GetStateClass( ENUM _state )
    {
        int stateIdx = GetStateIdx( _state );
        return GetStateIdxClass( stateIdx );            
    }

    /// <summary>
    /// ステートクラスを取得
    /// </summary>
    public CHILD_CLASS GetStateClass<CHILD_CLASS>( ENUM _state ) where CHILD_CLASS : ChildStateBasic
    {
        int stateIdx = GetStateIdx( _state );
        return GetStateIdxClass<CHILD_CLASS>( stateIdx );
    }
        
    /// <summary>
    /// 初期ステートを設定
    /// </summary>
    public void BeginState( ENUM _state )
    {
        int stateIdx = GetStateIdx( _state );
        BeginStateIdx( stateIdx );
    }

    /// <summary>
    /// ステートを設定
    /// </summary>
    public bool SetState( ENUM _state )
    {
        int stateIdx = GetStateIdx( _state );
        return SetStateIdx( stateIdx );
    }

    /// <summary>
    /// 指定したステートは現在のステートと同じかどうか判断
    /// </summary>
    public bool IsEqualsCurrentState( ENUM _state )
    {
        int stateIdx = GetStateIdx( _state );
        return IsEqualsCurrentStateIdx( stateIdx );
    }

    /// <summary>
    /// ステート遷移可能かどうか判断
    /// </summary>
    public bool CanChangeState( ENUM _state )
    {
        int stateIdx = GetStateIdx( _state );
        return CanChangeStateIdx( stateIdx );
    }

    /// <summary>
    /// ステートとインデックスの一致性を判断
    /// </summary>
    public bool IsStateEqualsIdx( ENUM _state , int _idx )
    {
        int stateIdx = GetStateIdx( _state );
        return stateIdx == _idx;
    }        

    /// <summary>
    /// ステートインデックスを取得
    /// </summary>
    public int GetStateIdx( ENUM _state ){ return IdxConverter_.GetIdxByContent( _state );}

    /// <summary>
    /// ステートを取得
    /// </summary>
    public bool GetState( out ENUM _state , int _idx )
    {
        return idxConverter_.GetContentByIdx( out _state , _idx );
    }
}

//============================================================
// 2個ENUMを持つ場合
//============================================================
/// <summary>
/// ステートベース
/// </summary>
public abstract class ChildStateBase<ENUM_1,ENUM_2,ACTOR> : ChildStateBase<ENUM_1 , ACTOR> where ENUM_1 : Enum
                                                                                           where ENUM_2 : Enum
                                                                                           where ACTOR  : MonoBehaviour
{
    public ChildStateBase(){}
    ~ChildStateBase(){}   
       
    /// <summary>
    /// 指定ステートは現在のステートと同じかどうか判断
    /// </summary>
    public bool IsEqualsCurrentState( ENUM_2 _state )
    {      
        return GetParentMachine<StateMachineBase<ENUM_1,ENUM_2,ACTOR>>() != null ? GetParentMachine<StateMachineBase<ENUM_1,ENUM_2,ACTOR>>().IsEqualsCurrentState( _state ) : false;
    }

    /// <summary>
    /// ステートとインデックスの一致性を判断
    /// </summary>
    public bool IsStateEqualsIdx( ENUM_2 _state, int _idx )
    {
        return GetParentMachine<StateMachineBase<ENUM_1,ENUM_2,ACTOR>>() != null ? GetParentMachine<StateMachineBase<ENUM_1,ENUM_2,ACTOR>>().IsStateEqualsIdx(_state, _idx) : false;
    }

    /// <summary>
    /// ステート遷移可能かどうか判断
    /// </summary>
    public bool CanChangeState( ENUM_2 _state )
    {
        return GetParentMachine<StateMachineBase<ENUM_1,ENUM_2,ACTOR>>() != null ? GetParentMachine<StateMachineBase<ENUM_1,ENUM_2,ACTOR>>().CanChangeState( _state ) : false;
    }

    /// <summary>
    /// ステートを設定
    /// </summary>
    public bool SetState( ENUM_2 _state )
    {
        return GetParentMachine<StateMachineBase<ENUM_1,ENUM_2,ACTOR>>() != null ? GetParentMachine<StateMachineBase<ENUM_1,ENUM_2,ACTOR>>().SetState( _state ) : false;
    }

    /// <summary>
    /// ステートクラスをを取得
    /// </summary>
    public ChildStateBasic GetStateIdxClass( ENUM_2 _state )
    {
        return GetParentMachine<StateMachineBase<ENUM_1,ENUM_2,ACTOR>>() != null ? GetParentMachine<StateMachineBase<ENUM_1,ENUM_2,ACTOR>>().GetStateClass( _state ) : null; 
    }
        
    /// <summary>
    /// ステートクラスをを取得
    /// </summary>
    public CHILD_CLASS GetStateClass<CHILD_CLASS>( ENUM_2 _state ) where CHILD_CLASS : ChildStateBasic
    {
        return GetParentMachine<StateMachineBase<ENUM_1,ENUM_2,ACTOR>>() != null ? GetParentMachine<StateMachineBase<ENUM_1,ENUM_2,ACTOR>>().GetStateClass<CHILD_CLASS>( _state ) : null; 
    }

    /// <summary>
    /// ステートを取得
    /// </summary>
    public bool GetState( out ENUM_2 _state , int _idx )
    {
        if (GetParentMachine<StateMachineBase<ENUM_1 , ENUM_2, ACTOR>>() != null)
        {
            return GetParentMachine<StateMachineBase<ENUM_1 , ENUM_2 , ACTOR>>().GetState( out _state , _idx );
        }
        _state = default( ENUM_2 );
        return false;
    }
}

/// <summary>
/// ステートマシンベース
/// </summary>
public abstract class StateMachineBase<ENUM_1,ENUM_2,ACTOR> : StateMachineBase<ENUM_1,ACTOR> where ENUM_1 : Enum
                                                                                             where ENUM_2 : Enum
                                                                                             where ACTOR  : MonoBehaviour
{
    //インデックス転換用
    private IdxConverter<ENUM_2> idxConverter_ = null;
    private IdxConverter<ENUM_2> IdxConverter_
    {
        get
        {
            if (idxConverter_ == null)
            {
                //インデックス転換処理
                idxConverter_ = new IdxConverter<ENUM_2>( this );
            }
            return idxConverter_;
        }
    }

    public StateMachineBase( ACTOR _owner ) : base( _owner ){}
    ~StateMachineBase(){}

    /// <summary>
    /// ステートを登録
    /// </summary>
    /// <param name="_state">ステート</param>
    /// <param name="_childState">ステートクラス</param>
    /// <param name="_isDefaultState">デフォルトステートにとして利用</param>
    public void RegisterState( ENUM_2 _state , ChildStateBasic _childState , bool _isDefaultState = false )
    {
        //ステートを登録し、コンテントのインデックスを返す
        int stateIdx = IdxConverter_.RegisterAndReturnIdx( _state );
        //インデックスを登録
        RegisterStateIdx( stateIdx , _childState , _isDefaultState );
    }

    /// <summary>
    /// ステートを削除
    /// </summary>
    public void RemoveState( ENUM_2 _state )
    {
        int stateIdx = GetStateIdx( _state );
        RemoveStateIdx( stateIdx );
        IdxConverter_.RemoveContent( stateIdx );
    }

    /// <summary>
    /// ステートクラスを取得
    /// </summary>
    public ChildStateBasic GetStateClass( ENUM_2 _state )
    {
        int stateIdx = GetStateIdx( _state );
        return GetStateIdxClass( stateIdx );            
    }

    /// <summary>
    /// ステートクラスを取得
    /// </summary>
    public CHILD_CLASS GetStateClass<CHILD_CLASS>( ENUM_2 _state ) where CHILD_CLASS : ChildStateBasic
    {
        int stateIdx = GetStateIdx( _state );
        return GetStateIdxClass<CHILD_CLASS>( stateIdx );
    }
        
    /// <summary>
    /// 初期ステートを設定
    /// </summary>
    public void BeginState( ENUM_2 _state )
    {
        int stateIdx = GetStateIdx( _state );
        BeginStateIdx( stateIdx );
    }

    /// <summary>
    /// ステートを設定
    /// </summary>
    public bool SetState( ENUM_2 _state )
    {
        int stateIdx = GetStateIdx( _state );
        return SetStateIdx( stateIdx );
    }

    /// <summary>
    /// 指定したステートは現在のステートと同じかどうか判断
    /// </summary>
    public bool IsEqualsCurrentState( ENUM_2 _state )
    {
        int stateIdx = GetStateIdx( _state );
        return IsEqualsCurrentStateIdx( stateIdx );
    }

    /// <summary>
    /// ステート遷移可能かどうか判断
    /// </summary>
    public bool CanChangeState( ENUM_2 _state )
    {
        int stateIdx = GetStateIdx( _state );
        return CanChangeStateIdx( stateIdx );
    }

    /// <summary>
    /// ステートとインデックスの一致性を判断
    /// </summary>
    public bool IsStateEqualsIdx( ENUM_2 _state , int _idx )
    {
        int stateIdx = GetStateIdx(_state);
        return stateIdx == _idx;
    }

    /// <summary>
    /// ステートインデックスを取得
    /// </summary>
    public int GetStateIdx( ENUM_2 _state ){ return IdxConverter_.GetIdxByContent( _state );}

    /// <summary>
    /// ステートを取得
    /// </summary>
    public bool GetState( out ENUM_2 _state , int _idx )
    {
        return idxConverter_.GetContentByIdx( out _state , _idx );
    }
}
    
//============================================================
// 3個ENUMを持つ場合
//============================================================
/// <summary>
/// ステートベース
/// </summary>
public abstract class ChildStateBase<ENUM_1,ENUM_2,ENUM_3,ACTOR> : ChildStateBase< ENUM_1,ENUM_2,ACTOR> where ENUM_1 : Enum
                                                                                                        where ENUM_2 : Enum
                                                                                                        where ENUM_3 : Enum
                                                                                                        where ACTOR  : MonoBehaviour
{
    public ChildStateBase(){}
    ~ChildStateBase(){}   
       
    /// <summary>
    /// 指定ステートは現在のステートと同じかどうか判断
    /// </summary>
    public bool IsEqualsCurrentState( ENUM_3 _state )
    {      
        return GetParentMachine<StateMachineBase<ENUM_1,ENUM_2,ENUM_3,ACTOR>>() != null ? GetParentMachine<StateMachineBase<ENUM_1,ENUM_2,ENUM_3,ACTOR>>().IsEqualsCurrentState( _state ) : false;
    }

    /// <summary>
    /// ステートとインデックスの一致性を判断
    /// </summary>
    public bool IsStateEqualsIdx( ENUM_3 _state, int _idx)
    {
        return GetParentMachine<StateMachineBase<ENUM_1,ENUM_2,ENUM_3,ACTOR>>() != null ? GetParentMachine<StateMachineBase<ENUM_1,ENUM_2,ENUM_3,ACTOR>>().IsStateEqualsIdx(_state, _idx) : false;
    }

    /// <summary>
    /// ステート遷移可能かどうか判断
    /// </summary>
    public bool CanChangeState( ENUM_3 _state )
    {
        return GetParentMachine<StateMachineBase<ENUM_1,ENUM_2,ENUM_3,ACTOR>>() != null ? GetParentMachine<StateMachineBase<ENUM_1,ENUM_2,ENUM_3,ACTOR>>().CanChangeState( _state ) : false;
    }

    /// <summary>
    /// ステートを設定
    /// </summary>
    public bool SetState( ENUM_3 _state )
    {
        return GetParentMachine<StateMachineBase<ENUM_1,ENUM_2,ENUM_3,ACTOR>>() != null ? GetParentMachine<StateMachineBase<ENUM_1,ENUM_2,ENUM_3,ACTOR>>().SetState( _state ) : false;
    }

    /// <summary>
    /// ステートクラスをを取得
    /// </summary>
    public ChildStateBasic GetStateIdxClass( ENUM_3 _state )
    {
        return GetParentMachine<StateMachineBase<ENUM_1,ENUM_2,ENUM_3,ACTOR>>() != null ? GetParentMachine<StateMachineBase<ENUM_1,ENUM_2,ENUM_3,ACTOR>>().GetStateClass( _state ) : null; 
    }
        
    /// <summary>
    /// ステートクラスをを取得
    /// </summary>
    public CHILD_CLASS GetStateClass<CHILD_CLASS>( ENUM_3 _state ) where CHILD_CLASS : ChildStateBasic
    {
        return GetParentMachine<StateMachineBase<ENUM_1,ENUM_2,ENUM_3,ACTOR>>() != null ? GetParentMachine<StateMachineBase<ENUM_1,ENUM_2,ENUM_3,ACTOR>>().GetStateClass<CHILD_CLASS>( _state ) : null; 
    }

    /// <summary>
    /// ステートを取得
    /// </summary>
    public bool GetState( out ENUM_3 _state , int _idx )
    {
        if( GetParentMachine<StateMachineBase<ENUM_1, ENUM_2, ENUM_3, ACTOR>>() != null )
        {
            return GetParentMachine<StateMachineBase<ENUM_1, ENUM_2, ENUM_3, ACTOR>>().GetState( out _state , _idx );
        }
        _state = default( ENUM_3 );
        return false;
    }
}

/// <summary>
/// ステートマシンベース
/// </summary>
public abstract class StateMachineBase<ENUM_1,ENUM_2,ENUM_3,ACTOR> : StateMachineBase<ENUM_1,ENUM_2,ACTOR> where ENUM_1 : Enum
                                                                                                           where ENUM_2 : Enum
                                                                                                           where ENUM_3 : Enum
                                                                                                           where ACTOR  : MonoBehaviour
{
    //インデックス転換用
    private IdxConverter<ENUM_3> idxConverter_ = null;
    private IdxConverter<ENUM_3> IdxConverter_
    {
        get
        {
            if (idxConverter_ == null)
            {
                //インデックス転換処理
                idxConverter_ = new IdxConverter<ENUM_3>( this );
            }
            return idxConverter_;
        }
    }

    public StateMachineBase( ACTOR _owner ) : base( _owner ){}
    ~StateMachineBase(){}

    /// <summary>
    /// ステートを登録
    /// </summary>
    /// <param name="_state">ステート</param>
    /// <param name="_childState">ステートクラス</param>
    /// <param name="_isDefaultState">デフォルトステートにとして利用</param>
    public void RegisterState( ENUM_3 _state , ChildStateBasic _childState , bool _isDefaultState = false )
    {
        //ステートを登録し、コンテントのインデックスを返す
        int stateIdx = IdxConverter_.RegisterAndReturnIdx( _state );
        //インデックスを登録
        RegisterStateIdx( stateIdx , _childState , _isDefaultState );
    }

    /// <summary>
    /// ステートを削除
    /// </summary>
    public void RemoveState( ENUM_3 _state )
    {
        int stateIdx = GetStateIdx( _state );
        RemoveStateIdx( stateIdx );
        IdxConverter_.RemoveContent( stateIdx );
    }

    /// <summary>
    /// ステートクラスを取得
    /// </summary>
    public ChildStateBasic GetStateClass( ENUM_3 _state )
    {
        int stateIdx = GetStateIdx( _state );
        return GetStateIdxClass( stateIdx );            
    }

    /// <summary>
    /// ステートクラスを取得
    /// </summary>
    public CHILD_CLASS GetStateClass<CHILD_CLASS>( ENUM_3 _state ) where CHILD_CLASS : ChildStateBasic
    {
        int stateIdx = GetStateIdx( _state );
        return GetStateIdxClass<CHILD_CLASS>( stateIdx );
    }
        
    /// <summary>
    /// 初期ステートを設定
    /// </summary>
    public void BeginState( ENUM_3 _state )
    {
        int stateIdx = GetStateIdx( _state );
        BeginStateIdx( stateIdx );
    }

    /// <summary>
    /// ステートを設定
    /// </summary>
    public bool SetState( ENUM_3 _state )
    {
        int stateIdx = GetStateIdx( _state );
        return SetStateIdx( stateIdx );
    }

    /// <summary>
    /// 指定したステートは現在のステートと同じかどうか判断
    /// </summary>
    public bool IsEqualsCurrentState( ENUM_3 _state )
    {
        int stateIdx = GetStateIdx( _state );
        return IsEqualsCurrentStateIdx( stateIdx );
    }

    /// <summary>
    /// ステート遷移可能かどうか判断
    /// </summary>
    public bool CanChangeState( ENUM_3 _state )
    {
        int stateIdx = GetStateIdx( _state );
        return CanChangeStateIdx( stateIdx );
    }
        
    /// <summary>
    /// ステートとインデックスの一致性を判断
    /// </summary>
    public bool IsStateEqualsIdx( ENUM_3 _state , int _idx )
    {
        int stateIdx = GetStateIdx(_state);
        return stateIdx == _idx;
    }

    /// <summary>
    /// ステートインデックスを取得
    /// </summary>
    public int GetStateIdx( ENUM_3 _state ){ return IdxConverter_.GetIdxByContent( _state );}

    /// <summary>
    /// ステートを取得
    /// </summary>
    public bool GetState( out ENUM_3 _state , int _idx )
    {
        return idxConverter_.GetContentByIdx( out _state , _idx );
    }
}

//============================================================
// ステートマシンインタフェース
//============================================================
/// <summary>
/// ステートマシンインタフェース
/// </summary>
public interface IStateMachineBase<ENUM> where ENUM : Enum
{   
    /// <summary>
    /// 初期ステートを設定
    /// </summary>
    void BeginState( ENUM _state );

    /// <summary>
    /// ステートを設定
    /// </summary>
    bool SetState( ENUM _state );

    /// <summary>
    /// 指定したステートは現在のステートと同じかどうか判断
    /// </summary>
    bool IsEqualsCurrentState( ENUM _state );

    /// <summary>
    /// ステート遷移可能かどうか判断
    /// </summary>
    bool CanChangeState( ENUM _state );

    /// <summary>
    /// ステートとインデックスの一致性を判断
    /// </summary>
    bool IsStateEqualsIdx( ENUM _state , int _idx );

    /// <summary>
    /// 更新
    /// </summary>
    void Update( float _dt );

    /// <summary>
    /// ステートクラスを取得
    /// </summary>
    ChildStateBasic GetStateClass( ENUM _state );

    /// <summary>
    /// ステートクラスを取得
    /// </summary>
    CHILD_CLASS GetStateClass<CHILD_CLASS>( ENUM _state ) where CHILD_CLASS : ChildStateBasic;

    /// <summary>
    /// ステートを取得
    /// </summary>
    public bool GetState( out ENUM _state , int _idx );
}
    
/// <summary>
/// ステートマシンインタフェース
/// </summary>
public interface IStateMachineBase<ENUM_1,ENUM_2,ACTOR> : IStateMachineBase<ENUM_1> where ENUM_1 : Enum
                                                                                    where ENUM_2 : Enum
{
    /// <summary>
    /// 初期ステートを設定
    /// </summary>
    void BeginState( ENUM_2 _state );
            
    /// <summary>
    /// ステートを設定
    /// </summary>
    bool SetState( ENUM_2 _state );

    /// <summary>
    /// 指定したステートは現在のステートと同じかどうか判断
    /// </summary>
    bool IsEqualsCurrentState( ENUM_2 _state );

    /// <summary>
    /// ステート遷移可能かどうか判断
    /// </summary>
    bool CanChangeState( ENUM_2 _state );

    /// <summary>
    /// ステートとインデックスの一致性を判断
    /// </summary>
    bool IsStateEqualsIdx( ENUM_2 _state , int _idx );

    /// <summary>
    /// ステートクラスを取得
    /// </summary>
    ChildStateBasic GetStateClass( ENUM_2 _state );

    /// <summary>
    /// ステートクラスを取得
    /// </summary>
    CHILD_CLASS GetStateClass<CHILD_CLASS>( ENUM_2 _state ) where CHILD_CLASS : ChildStateBasic;

    /// <summary>
    /// ステートを取得
    /// </summary>
    public bool GetState( out ENUM_2 _state , int _idx );
} 

