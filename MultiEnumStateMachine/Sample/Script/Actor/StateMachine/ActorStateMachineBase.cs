using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiEnumStateMachine
{
    /// <summary>
    /// アクター汎用ステート
    /// </summary>
    public enum ACTOR_GENERAL_STATE
    {
        IDLE = 0,   //待機                
    }

    /// <summary>
    /// アクター汎用ステートベース
    /// </summary>
    public abstract class ActorGeneralChildState<ACTOR> : ChildStateBase<ACTOR_GENERAL_STATE, ACTOR> where ACTOR : ActorBase
    {
        /// <summary>
        /// ステートマシンを取得
        /// </summary>
        public IActorBaseStateMachine GetActorBaseStateMachine(){ return ParentMachine_ as IActorBaseStateMachine;}

        public ActorGeneralChildState(){}
        ~ActorGeneralChildState(){}        
    }

    /// <summary>
    /// アクターステートベース
    /// </summary>
    public abstract class ActorChildState<ENUM,ACTOR> : ChildStateBase<ACTOR_GENERAL_STATE,ENUM,ACTOR> where ENUM  : Enum
                                                                                                       where ACTOR : ActorBase
    {
        /// <summary>
        /// ステートマシンを取得
        /// </summary>
        public IActorBaseStateMachine GetActorBaseStateMachine(){ return ParentMachine_ as IActorBaseStateMachine;}

        public ActorChildState(){}
        ~ActorChildState(){}           
    }

    /// <summary>
    /// アクターステートマシンベース
    /// </summary>
    public abstract class ActorBaseStateMachine<ENUM,ACTOR> : StateMachineBase<ACTOR_GENERAL_STATE,ENUM,ACTOR> , IActorBaseStateMachine where ENUM  : Enum
                                                                                                                                        where ACTOR : ActorBase
	{
        public ActorBaseStateMachine( ACTOR _owner ) : base( _owner ){}
        ~ActorBaseStateMachine(){}

        /// <summary>
        /// 開始
        /// </summary>
        protected override void OnBegin()
        {
            base.OnBegin();
            
            //汎用ステートを登録し、デフォルトステートに設定
            RegisterState( ACTOR_GENERAL_STATE.IDLE , new ActorIdleState() , true );
        }   
    }

    /// <summary>
    /// アクターステートマシンインタフェース
    /// </summary>
    public interface IActorBaseStateMachine : IStateMachineBase<ACTOR_GENERAL_STATE>
    {        
    }
}
