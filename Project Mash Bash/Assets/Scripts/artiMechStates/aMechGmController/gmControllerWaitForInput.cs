/// Artimech
/// 
/// Copyright © <2017> <George A Lancaster>
/// Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
/// and associated documentation files (the "Software"), to deal in the Software without restriction, 
/// including without limitation the rights to use, copy, modify, merge, publish, distribute, 
/// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software 
/// is furnished to do so, subject to the following conditions:
/// The above copyright notice and this permission notice shall be included in all copies 
/// or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
/// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS 
/// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
/// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
/// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
/// OTHER DEALINGS IN THE SOFTWARE.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

#region XML_DATA

#if ARTIMECH_META_DATA
<!-- Atrimech metadata for positioning and other info using the visual editor.  -->
<!-- The format is XML. -->
<!-- __________________________________________________________________________ -->
<!-- Note: Never make ARTIMECH_META_DATA true since this is just metadata       -->
<!-- Note: for the visual editor to work.                                       -->

<stateMetaData>
  <State>
    <alias>Wait For Input</alias>
    <comment></comment>
    <posX>288</posX>
    <posY>547</posY>
    <sizeX>142</sizeX>
    <sizeY>42</sizeY>
  </State>
</stateMetaData>

#endif

#endregion
namespace Artimech
{
    public class gmControllerWaitForInput : stateGameBase
    {

        /// <summary>
        /// State constructor.
        /// </summary>
        /// <param name="gameobject"></param>
        public gmControllerWaitForInput(GameObject gameobject) : base(gameobject)
        {
            //<ArtiMechConditions>
            m_ConditionalList.Add(new gmControllerWaitForInput_To_gmControllerInputBad("gmControllerInputBad"));
            m_ConditionalList.Add(new gmControllerWaitForInput_To_gmControllerInputIsGood("gmControllerInputIsGood"));
            m_ConditionalList.Add(new gmControllerWaitForInput_To_gmControllerInputTimeOut("gmControllerInputTimeOut"));
        }

        /// <summary>
        /// Updates from the game object.
        /// </summary>
        public override void Update()
        {
            aMechGmController script = StateGameObject.GetComponent<aMechGmController>();
            script.TimerLeftBar.UpdateBar(script.PerSymbolInputTimeLimit - StateTime, script.PerSymbolInputTimeLimit);
            base.Update();
        }

        /// <summary>
        /// Fixed Update for physics and such from the game object.
        /// </summary>
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        /// <summary>
        /// For updateing the unity gui.
        /// </summary>
        public override void UpdateEditorGUI()
        {
            base.UpdateEditorGUI();
        }

        /// <summary>
        /// When the state becomes active Enter() is called once.
        /// </summary>
        public override void Enter()
        {
            aMechGmController script = StateGameObject.GetComponent<aMechGmController>();

            script.OnClickBadBool = false;
            script.OnClickGoodBool = false;

            //turn on the buttons
            for (int i = 0; i < script.Buttons.Length; i++)
            {

                script.Buttons[i].gameObject.GetComponent<Image>().raycastTarget = true;
                ColorBlock cb = script.Buttons[i].colors;
                if(script.Buttons[i]!=script.SymbolDataList[script.CurrentPuzzelIndex].ButtonPtr)
                    cb.pressedColor = new Color(1, 0, 0, 1);
                else
                    cb.pressedColor = new Color(script.SymbolDataList[script.CurrentPuzzelIndex].Color.r, 
                        script.SymbolDataList[script.CurrentPuzzelIndex].Color.g, 
                        script.SymbolDataList[script.CurrentPuzzelIndex].Color.b, 
                        script.SymbolDataList[script.CurrentPuzzelIndex].Color.a);
                script.Buttons[i].colors = cb;


                //script.Buttons[i].colors = new Color(script.SymbolDataList[i].Color.r, script.SymbolDataList[i].Color.g, script.SymbolDataList[i].Color.b, script.SymbolDataList[i].Color.a);
                //script.Buttons[i].gameObject.GetComponent<Button>().colors;
            }

            script.RepeatDialog.Action = aMechGameGUIBase.eActionType.kFadeIn;
            script.WatchDialog.Action = aMechGameGUIBase.eActionType.kFadeOut;
            script.TimerBar.Action = aMechGameGUIBase.eActionType.kFadeIn;

            base.Enter();
        }

        /// <summary>
        /// When the state becomes inactive Exit() is called once.
        /// </summary>
        public override void Exit()
        {
            base.Exit();
        }
    }
}
