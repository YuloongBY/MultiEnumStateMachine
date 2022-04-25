using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiEnumStateMachine
{
    /// <summary>
    /// アクターの色変化状態
    /// </summary>
    public class ActorColorState : ActorChildState<ACTOR_STATE,Actor>
	{   
        //色変化間隔
        private const float COLOR_CHANGE_INV = 0.5f;

        public ActorColorState(){}
        ~ActorColorState(){}

        /// <summary>
        /// このステートに遷移できるかどうか判断
        /// </summary>
        public override bool CanChangeState()
        {
            //現在ステートは自身の場合、遷移できないように
            return !IsEqualsCurrentState( ACTOR_STATE.COLOR );
        }

        /// <summary>
        /// 開始
        /// ステートに入った時、1回だけ呼ばれる
        /// </summary>
        /// <param name="_prevStateIdx">前のステートインデックス</param>
        public override void OnBegin( int _prevStateIdx )        
        {
            //まず白色に設定
            Owner_.SetColor( Color.white );           
        }

        /// <summary>
        /// 更新
        /// 毎フレーム呼ばれる
        /// </summary>
        /// <param name="_dt">DeltaTime</param>        
        override public void OnUpdate( float _dt )
        {   
            switch( GetSubStep())
            {
                //一定時間後、色をランダムで変化する
                case 0:
                {
                    AddSubTimer( _dt );
                    if( GetSubTimer() >= COLOR_CHANGE_INV )
                    {
                        SetSubStep( 1 );
                    }
                }
                break;
                case 1:
                {
                    Color rmColor = new Color( Random.value , Random.value , Random.value , 1.0f );
                    Owner_.SetColor( rmColor );
                    SetSubStep( 0 );
                }
                break;
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
            //デフォルト色に戻す
            Owner_.BacktoDefaultColor();   
        }
	}
}
