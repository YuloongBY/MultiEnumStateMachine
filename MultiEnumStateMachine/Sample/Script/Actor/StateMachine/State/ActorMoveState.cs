using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiEnumStateMachine
{
    /// <summary>
    /// アクターの移動状態
    /// </summary>
    public class ActorMoveState : ActorChildState<ACTOR_STATE,Actor>
	{   
        //左端点
        private const float LEFT_LOCATION = -300.0f;

        //右端点
        private const float RIGHT_LOCATION = 300.0f;

        //移動時間
        private const float MOVE_TIME = 1.0f;

        //移動開始、終了値
        private float moveStart_ = LEFT_LOCATION;
        private float moveEnd_   = RIGHT_LOCATION;

        public ActorMoveState(){}
        ~ActorMoveState(){}

        /// <summary>
        /// このステートに遷移できるかどうか判断
        /// </summary>
        public override bool CanChangeState()
        {
            //現在ステートは自身の場合、遷移できないように
            return !IsEqualsCurrentState( ACTOR_STATE.MOVE );
        }

        /// <summary>
        /// 開始
        /// ステートに入った時、1回だけ呼ばれる
        /// </summary>
        /// <param name="_prevStateIdx">前のステートインデックス</param>
        public override void OnBegin( int _prevStateIdx )        
        {
            moveStart_ = LEFT_LOCATION;
            moveEnd_   = RIGHT_LOCATION;
        }

        /// <summary>
        /// 更新
        /// 毎フレーム呼ばれる
        /// </summary>
        /// <param name="_dt">DeltaTime</param>          
        override public void OnUpdate( float _dt )
        {   
            if( MOVE_TIME >= 0.0f )
            {
                switch( GetSubStep())
                {
                    //移動処理
                    case 0:
                    {
                        Vector3 start = new Vector3( moveStart_ , 0.0f , 0.0f );
                        Vector3 end   = new Vector3( moveEnd_ , 0.0f , 0.0f );

                        AddSubTimer( _dt );
                        if( GetSubTimer() >= MOVE_TIME )
                        {
                            Owner_.transform.position = end;
                            SetSubStep( 1 );
                        }
                        else
                        {
                            Owner_.transform.position = start + ( end - start ) * ( GetSubTimer() / MOVE_TIME );
                        }
                    }
                    break;
                    case 1:
                    {
                        float temValue = moveStart_;
                        moveStart_ = moveEnd_;
                        moveEnd_ = temValue;
                        SetSubStep( 0 );
                    }
                    break;
                }
            }
            
            //一定時間後、デフォルトステートに戻る
            if( Owner_.AutoBackToDefaultStatTime_ > 0.0f )
            {
                AddTimer( _dt );
                if( GetTimer() >= Owner_.AutoBackToDefaultStatTime_ )
                {
                    ToDefaultState();
                }
            }
        }

        /// <summary>
        /// 終了
        /// ステートから出た時、1回だけ呼ばれる
        /// </summary>
        /// <param name="_nextStateIdx">次のステートインデックス</param>       
        public override void OnEnd( int _nextStateIdx )
        {
            //位置を戻す
            Owner_.transform.position = Vector3.zero;
        }
	}
}
