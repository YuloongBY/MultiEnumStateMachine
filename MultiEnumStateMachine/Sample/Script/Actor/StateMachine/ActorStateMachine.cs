using UnityEngine;

namespace MultiEnumStateMachine
{
    /// <summary>
    /// アクターステート
    /// </summary>
    public enum ACTOR_STATE
    {
        MOVE  = 0 , //移動
        COLOR     , //色変化
        ZOOM      , //ズーム
    }

	public class ActorStateMachine : ActorBaseStateMachine<ACTOR_STATE , Actor>  
	{   
        public ActorStateMachine(Actor _owner ):base( _owner ){}
        ~ActorStateMachine(){}

        /// <summary>
        /// 開始
        /// </summary>
        protected override void OnBegin()
        {
            base.OnBegin();

            //ステートを登録
            RegisterState( ACTOR_STATE.MOVE  , new ActorMoveState());        
            RegisterState( ACTOR_STATE.COLOR , new ActorColorState());        
            RegisterState( ACTOR_STATE.ZOOM  , new ActorZoomState());        
            
            //初期ステート設定
            BeginState( ACTOR_GENERAL_STATE.IDLE );
        }

        /// <summary>
        /// 更新
        /// </summary>
        protected override void OnUpdate( float _dt )
        {
            base.OnUpdate( _dt );
        }        
    }
}
