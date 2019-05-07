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
using UnityEngine.UI;

namespace Artimech
{
    public class SimMgr : stateMachineGame
    {
        [Header("SimMgr:")]
        [SerializeField]
        [Tooltip("Link the game controller that does that actual tap symbol game.")]
        aMechGmController m_GameController;

        [Header("Linked Gfx Objects:")]
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
        [SerializeField]
        [Tooltip("Begin popup box.")]
        aMechGameGUIBase m_BeginInstructions;

        /*
        [SerializeField]
        [Tooltip("Begin popup box.")]
        aMechGameGUIBase m_WatchMessage;
        [SerializeField]
        [Tooltip("Repeat popup box.")]
        aMechGameGUIBase m_RepeatMessage;
        */


        [Header("Timers:")]
        [SerializeField]
        [Tooltip("How long to wait till the intro message gets displayed.")]
        float m_IntroMessageWarmUpTime = 0.5f;
        [SerializeField]
        [Tooltip("How long to wait till the intro message gets faded out after fade in starts.")]
        float m_IntroMessageHoldAfterFadeIn = 1.5f;
        [SerializeField]
        [Tooltip("How long it takes for the start game information to come up.")]
        float m_BeginInfoMessageTime = 0.25f;
        [SerializeField]
        [Tooltip("How long it takes for the start game information to go away.")]
        float m_BeginInfoAfterMessageTime = 0.75f;
        [SerializeField]
        [Tooltip("From Watch to Symbol playback.")]
        float m_WatchToShowSymbolsTime = 0.75f;

        [Header("Sim Audio:")]

        [SerializeField]
        [Tooltip("Does the game play an intro soundtrack.")]
        bool m_PlayIntroGameSound = true;
        [SerializeField]
        [Tooltip("How long to wait till the intro message gets faded out after fade in starts.")]
        AudioSource m_IntroGameSound;

        [Header("Buttons:")]
        [SerializeField]
        [Tooltip("Button that starts the game.")]
        private Button m_TapToStartButton;
        bool m_TapToStartBool = false;

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

        public Button TapToStartButton { get => m_TapToStartButton; set => m_TapToStartButton = value; }
        public bool TapToStartBool { get => m_TapToStartBool; set => m_TapToStartBool = value; }
        public aMechGameGUIBase BeginInstructions { get => m_BeginInstructions; set => m_BeginInstructions = value; }
        public float BeginInfoMessageTime { get => m_BeginInfoMessageTime;}
        public float BeginInfoAfterMessageTime { get => m_BeginInfoAfterMessageTime;}
        //public aMechGameGUIBase WatchMessage { get => m_WatchMessage; set => m_WatchMessage = value; }
        public float WatchToShowSymbolsTime { get => m_WatchToShowSymbolsTime;}
        public aMechGmController GameController { get => m_GameController; set => m_GameController = value; }
        //public aMechGameGUIBase RepeatMessage { get => m_RepeatMessage; set => m_RepeatMessage = value; }

        void OnClickTapToStartButton()
        {
            TapToStartBool = true;
        }

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
            TapToStartButton.onClick.AddListener(OnClickTapToStartButton);
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
            AddState(new simMgrSymbolInput(this.gameObject),"simMgrSymbolInput");
            AddState(new simMgrStartSymbolPlayBack(this.gameObject),"simMgrStartSymbolPlayBack");
            AddState(new simMgrStartGameCycle(this.gameObject),"simMgrStartGameCycle");
            AddState(new simMgrShowBeginGameInstructions(this.gameObject),"simMgrShowBeginGameInstructions");
            AddState(new simMgrStartGame(this.gameObject),"simMgrStartGame");
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