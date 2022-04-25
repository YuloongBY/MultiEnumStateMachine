using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiEnumStateMachine
{
    /// <summary>
    /// サンプル
    /// </summary>
    public class StateMachineSample : MonoBehaviour
    {
        //アクター
        private Actor actor_ = null;

        /// <summary>
        /// 開始
        /// </summary>
        void Start()
        {
            //アクターを作成
            actor_ = Actor.CreateActor();
        }

        /// <summary>
        /// 更新
        /// </summary>
        void Update(){}

        /// <summary>
        /// GUI描画
        /// </summary>
        void OnGUI()
        {
            if( actor_ == null ) return;

            float guiWidthRate =  Screen.width / 1920.0f;
            float guiHeightRate = Screen.height / 1080.0f;

            int guiWidth     = ( int )( 200 * guiWidthRate );
            int guiHeight    = ( int )( 50  * guiHeightRate );
            int guiXLocation = Screen.width - guiWidth;
            int guiYLocation = 0;
            
            int btnFontSize = ( int )( 25 * guiHeightRate );            
            GUIStyle btnStyle = new GUIStyle( GUI.skin.button );
            btnStyle.fontSize = btnFontSize;

            int lblFontSize = ( int )( 20 * guiHeightRate );
            int stateLblFontSize = ( int )( 50 * guiHeightRate );            
            GUIStyle lblStyle = new GUIStyle( GUI.skin.label );
            lblStyle.fontSize = lblFontSize;
            
            //待機状態に遷移
            if( GUI.Button( new Rect( guiXLocation , guiYLocation , guiWidth , guiHeight ) , "To IdleState" , btnStyle ))
            {
                actor_.SetState( ACTOR_GENERAL_STATE.IDLE );
            }

            //移動状態に遷移
            guiYLocation += guiHeight;
            if( GUI.Button( new Rect( guiXLocation , guiYLocation , guiWidth , guiHeight ) , "To MoveState" , btnStyle ))
            {
                actor_.SetState( ACTOR_STATE.MOVE );                
            }

            //色変化状態に遷移
            guiYLocation += guiHeight;
            if( GUI.Button( new Rect( guiXLocation , guiYLocation , guiWidth , guiHeight ) , "To ColorState" , btnStyle ))
            {
                actor_.SetState( ACTOR_STATE.COLOR );                
            }
            
            //ズーム状態に遷移
            guiYLocation += guiHeight;
            if( GUI.Button( new Rect( guiXLocation , guiYLocation , guiWidth , guiHeight ) , "To ZoomState" , btnStyle ))
            {
                actor_.SetState( ACTOR_STATE.ZOOM );                
            }

            //自動的にデフォルトステートに戻るまでかかる時間
            {
                string backtoDefaultStateStr = actor_.AutoBackToDefaultStatTime_ > 0.0f ? actor_.AutoBackToDefaultStatTime_.ToString() + " Sec" : "Not";
                guiYLocation += guiHeight + 20;
                GUI.Label( new Rect( guiXLocation , guiYLocation , guiWidth , guiHeight ) , string.Format( "Back to defaultState \nTime: {0}" , backtoDefaultStateStr ) , lblStyle );
                guiYLocation += guiHeight;            
                actor_.AutoBackToDefaultStatTime_ = Mathf.Round( GUI.HorizontalSlider( new Rect( guiXLocation , guiYLocation , guiWidth * 0.8f , guiHeight * 0.5f ), actor_.AutoBackToDefaultStatTime_ , 0.0f , 5.0f ));
            }
            

            //ステート表示
            {
                float lblWidth  =  ( int )( 800 * guiWidthRate );
                float lblHeight =  ( int )( 100 * guiHeightRate );
                float lblXLocation = ( Screen.width - lblWidth ) * 0.5f;
                float lblYLocation = ( Screen.height - lblHeight ) * 0.5f - ( Screen.height * 0.2f );
             
                lblStyle.alignment = TextAnchor.MiddleCenter;       
                lblStyle.fontSize = stateLblFontSize;
                GUI.Label( new Rect( lblXLocation , lblYLocation , lblWidth , lblHeight ) , string.Format( "State: {0}" , actor_.GetCurrentStateGuiName()) , lblStyle );        
                lblStyle.fontSize = lblFontSize;
                lblStyle.alignment = default;
            }
        }
    }
}
