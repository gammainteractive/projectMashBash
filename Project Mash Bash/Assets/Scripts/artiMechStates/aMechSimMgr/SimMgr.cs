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

namespace Artimech
{
    public class SimMgr : stateMachineGame
    {
        [Header("SimMgr:")]
        [SerializeField]
        [Tooltip("Intro Screen Text.")]
        aMechGameGUIBase m_InfoText;
        [SerializeField]
        [Tooltip("The tap instruction text object link.")]
        aMechGameGUIBase m_TapToStartText;
        [SerializeField]
        [Tooltip("Button Gui Parent.")]
        aMechGameGUIBase m_ButtonGuiParent;
        [SerializeField]
        [Tooltip("Button Gui Diamond.")]
        aMechGameGUIBase m_ButtonGuiDiamond;

        [Header("Timers:")]
        [SerializeField]
        [Tooltip("How long to wait till the intro message gets displayed.")]
        float m_IntroMessageWarmUpTime = 0.5f;
        [SerializeField]
        [Tooltip("How long to wait till the intro message gets faded out after fade in starts.")]
        float m_IntroMessageHoldAfterFadeIn = 1.5f;

        [Header("Sim Audio:")]

        [SerializeField]
        [Tooltip("Does the game play an intro soundtrack.")]
        bool m_PlayIntroGameSound = true;
        [SerializeField]
        [Tooltip("How long to wait till the intro message gets faded out after fade in starts.")]
        AudioSource m_IntroGameSound;

        public delegate void StartGame();
        public static event StartGame OnStartGame;

        private static SimMgr m_Instance = null;
        /// <summary>Returns an instance of SimMgr </summary>
        public static SimMgr Inst { get { return m_Instance; } }

        public aMechGameGUIBase InfoText { get => m_InfoText; set => m_InfoText = value; }
        public float IntroMessageWarmUpTime { get => m_IntroMessageWarmUpTime; }
        public float IntroMessageHoldAfterFadeIn { get => m_IntroMessageHoldAfterFadeIn; set => m_IntroMessageHoldAfterFadeIn = value; }
        public aMechGameGUIBase TapToStartText { get => m_TapToStartText; set => m_TapToStartText = value; }
        public AudioSource IntroGameSound { get => m_IntroGameSound; set => m_IntroGameSound = value; }
        public aMechGameGUIBase ButtonGuiParent { get => m_ButtonGuiParent; set => m_ButtonGuiParent = value; }
        public aMechGameGUIBase ButtonGuiDiamond { get => m_ButtonGuiDiamond; set => m_ButtonGuiDiamond = value; }
        public bool PlayIntroGameSound { get => m_PlayIntroGameSound; set => m_PlayIntroGameSound = value; }

        new void Awake()
        {
            if (m_Instance != null)
            {
                Debug.LogError("There was already an instance of SimMgr.");
                return;
            }
            m_Instance = GetComponent<SimMgr>();

            base.Awake();
            CreateStates();
        }

        // Use this for initialization
        new void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        new void Update()
        {
            base.Update();
        }

        new void FixedUpdate()
        {
            base.FixedUpdate();
        }

        /// <summary>
        /// Autogenerated state are created here inside this function.
        /// </summary>
        void CreateStates()
        {

            m_CurrentState = AddState(new simMgrInit(this.gameObject), "simMgrInit");

            //<ArtiMechStates>
            AddState(new simMgrFadeInGetReady(this.gameObject),"simMgrFadeInGetReady");
            AddState(new simMgrFadeOutStartScreen(this.gameObject),"simMgrFadeOutStartScreen");
            AddState(new simMgrWin(this.gameObject),"simMgrWin");
            AddState(new simMgrLoose(this.gameObject),"simMgrLoose");
            AddState(new simMgrInitWin(this.gameObject),"simMgrInitWin");
            AddState(new simMgrInitLoose(this.gameObject),"simMgrInitLoose");
            AddState(new simMgrPlayGame(this.gameObject),"simMgrPlayGame");
            AddState(new simMgrFadeInStartScreen(this.gameObject),"simMgrFadeInStartScreen");

        }
    }
}