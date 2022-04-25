using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiEnumStateMachine
{
    /// <summary>
    /// アクターのズーム状態
    /// </summary>
    public class ActorZoomState : ActorChildState<ACTOR_STATE,Actor>
	{   
        //ズーム最小値
        private const float ZOOM_MIN = 1.0f;

        //ズーム最大値
        private const float ZOOM_MAX = 1.5f;

        //ズーム時間
        private const float ZOOM_TIME = 1.0f;

        //ズーム開始、終了値
        private float zoomStart_ = ZOOM_MIN;
        private float zoomEnd_   = ZOOM_MAX;

        public ActorZoomState(){}
        ~ActorZoomState(){}

        /// <summary>
        /// このステートに遷移できるかどうか判断
        /// </summary>
        public override bool CanChangeState()
        {
            //現在ステートは自身の場合、遷移できないように
            return !IsEqualsCurrentState( ACTOR_STATE.ZOOM );
        }

        /// <summary>
        /// 開始
        /// ステートに入った時、1回だけ呼ばれる
        /// </summary>
        /// <param name="_prevStateIdx">前のステートインデックス</param>
        public override void OnBegin( int _prevStateIdx )        
        {
            zoomStart_ = ZOOM_MIN;
            zoomEnd_   = ZOOM_MAX;
        }

        /// <summary>
        /// 更新
        /// 毎フレーム呼ばれる
        /// </summary>
        /// <param name="_dt">DeltaTime</param>          
        override public void OnUpdate( float _dt )
        {
            if( ZOOM_TIME >= 0.0f )
            {
                switch( GetSubStep())
                {
                    //ズーム処理
                    case 0:
                    {
                        AddSubTimer( _dt );
                        if( GetSubTimer() >= ZOOM_TIME )
                        {
                            Owner_.transform.localScale = Vector3.one * zoomEnd_;
                            SetSubStep( 1 );
                        }
                        else
                        {
                            Owner_.transform.localScale = Vector3.one * zoomStart_ + Vector3.one * ( zoomEnd_ - zoomStart_ ) * ( GetSubTimer() / ZOOM_TIME );
                        }
                    }
                    break;
                    case 1:
                    {
                        float temValue = zoomStart_;
                        zoomStart_ = zoomEnd_;
                        zoomEnd_ = temValue;
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
            //サイズを戻す
            Owner_.transform.localScale = Vector3.one;
        }
	}
}
