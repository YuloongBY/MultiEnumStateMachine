using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiEnumStateMachine
{
    /// <summary>
    /// アクターの待機状態
    /// </summary>
    public class ActorIdleState : ActorGeneralChildState<ActorBase>
	{   
        public ActorIdleState(){}
        ~ActorIdleState(){}

        /// <summary>
        /// このステートに遷移できるかどうか判断
        /// </summary>
        public override bool CanChangeState()
        {
            //現在ステートは自身の場合、遷移できないように
            return !IsEqualsCurrentState( ACTOR_GENERAL_STATE.IDLE );            
        }

        /// <summary>
        /// 開始
        /// ステートに入った時、1回だけ呼ばれる
        /// </summary>
        /// <param name="_prevStateIdx">前のステートインデックス</param>
        public override void OnBegin( int _prevStateIdx )        
        {
        }

        /// <summary>
        /// 更新
        /// 毎フレーム呼ばれる
        /// </summary>
        /// <param name="_dt">DeltaTime</param>     
        override public void OnUpdate( float _dt )
        {   
        }

        /// <summary>
        /// 終了
        /// ステートから出た時、1回だけ呼ばれる
        /// </summary>
        /// <param name="_nextStateIdx">次のステートインデックス</param>        
        public override void OnEnd( int _nextStateIdx )
        {
        }
	}
}
