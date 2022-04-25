using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiEnumStateMachine
{
    /// <summary>
    /// アクター
    /// </summary>
    public class Actor : ActorBase
    {
        //デフォルト色
        private static readonly Color DEFAULT_COLOR = new Color( 0.9f , 0.8f , 0.2f , 1.0f ); 

        //メッシュ
        private MeshFilter mesh_ = null;

        //ステートマシン
        private ActorStateMachine stateMachine_ = null;

        //デフォルトステートに戻るまでかかる時間(0以下戻らない)
        public float AutoBackToDefaultStatTime_{ get;set;} = 0.0f;

        /// <summary>
        /// 開始
        /// </summary>
        void Start()
        {
            //ステートマシンを作成
            stateMachine_ = new ActorStateMachine( this );
        }

        /// <summary>
        /// 更新
        /// </summary>
        void Update()
        {
            //ステートマシンを更新
            stateMachine_.Update( Time.deltaTime );
        }

        /// <summary>
        /// 色設定
        /// </summary>
        public void SetColor( Color _color )
        {
            if( mesh_ != null ){ mesh_.SetColor( _color );}
        }

        /// <summary>
        /// デフォルト色に戻す
        /// </summary>
        public void BacktoDefaultColor()
        {
            if( mesh_ != null ){ mesh_.SetColor( DEFAULT_COLOR );}
        }

        /// <summary>
        /// ステートを設定
        /// </summary>
        public void SetState( ACTOR_GENERAL_STATE _state )
        {
            if( stateMachine_ != null ){ stateMachine_.SetState( _state );}
        }

        /// <summary>
        /// ステートを設定
        /// </summary>
        public void SetState( ACTOR_STATE _state )
        {
            if( stateMachine_ != null ){ stateMachine_.SetState( _state );}
        }

        /// <summary>
        /// 現在のステート表示名を取得
        /// </summary>
        public string GetCurrentStateGuiName()
        {
            string stateGuiName = null;
            if( stateMachine_ != null )
            {
                //ステートインデックスを取得
                int currentStateIdx = stateMachine_.GetCurrentStateIdx();
                
                //現在のステートは「ACTOR_GENERAL_STATE」の中に存在なら、取得する
                if( stateMachine_.GetState( out ACTOR_GENERAL_STATE generalState , currentStateIdx ))
                {
                    switch( generalState )
                    {
                        case ACTOR_GENERAL_STATE.IDLE:
                        stateGuiName = "Idle (general)";
                        break;
                    }
                }
                //現在のステートは「ACTOR_STATE」の中に存在なら、取得する
                else if( stateMachine_.GetState( out ACTOR_STATE state , currentStateIdx ))
                {
                    switch( state )
                    {
                        case ACTOR_STATE.COLOR:
                        stateGuiName = "Color";
                        break;
                        case ACTOR_STATE.MOVE:
                        stateGuiName = "Move";
                        break;
                        case ACTOR_STATE.ZOOM:
                        stateGuiName = "Zoom";
                        break;
                    }
                }
            }
            return stateGuiName;
        }

        /// <summary>
        /// アクターを作成
        /// </summary>
        public static Actor CreateActor()
        {
            GameObject actorGO = new GameObject( "Actor" );

            //位置を設定
            actorGO.transform.position = Vector3.zero;

            var actor = actorGO.AddComponent<Actor>();
            
            //描画
            {
                float actorRadius = 100.0f;
                Color color = DEFAULT_COLOR;
                actor.mesh_ = MeshRender.RenderRound( actorGO ,
                                                      Vector3.zero ,
                                                      actorRadius ,
                                                      color );
            }

            return actor;
        }
    }
}
