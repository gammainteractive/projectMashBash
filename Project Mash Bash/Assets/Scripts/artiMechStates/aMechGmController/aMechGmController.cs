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
using System.Collections.Generic;

namespace Artimech
{
    public class aMechGmController : stateMachineGame
    {
        [Header("Control Links:")]
        [SerializeField]
        [Tooltip("0 is mid, 1 is left and 2 is right")]
        Button[] m_Buttons;
        [SerializeField]
        [Tooltip("Timer bar.  Show how long input can still be used.")]
        SimpleHealthBar m_TimerLeftBar;
        [SerializeField]
        [Tooltip("Win Input Secquence Dialog.")]
        aMechGameGUIBase m_WinDialog;
        [SerializeField]
        [Tooltip("Loose Input Secquence Dialog.")]
        aMechGameGUIBase m_LooseDialog;
        [SerializeField]
        [Tooltip("Error Sound.")]
        AudioSource m_ErrorSound;
        [SerializeField]
        [Tooltip("Repeat Dialog.")]
        aMechGameGUIBase m_RepeatDialog;
        [SerializeField]
        [Tooltip("Watch Dialog.")]
        aMechGameGUIBase m_WatchDialog;
        [SerializeField]
        [Tooltip("Time bar.")]
        aMechGameGUIBase m_TimerBar;
        [SerializeField]
        [Tooltip("Player Fighter.")]
        aMechFighter m_FighterPlayer;

        [Header("Puzzel Config:")]
        [SerializeField]
        [Tooltip("Puzzel Colors")]
        Color[] m_PuzzelColors;
        [SerializeField]
        [Tooltip("Number of sequences in a row to win.")]
        int m_PuzzelWinLimit;

        [Header("Timer Configs:")]
        [SerializeField]
        [Tooltip("Time it takes to cycle a new display color and button.")]
        float m_IncrementColorTimeLimit = 1.5f;
        [SerializeField]
        [Tooltip("Time it takes start the input game after the show symbol section of the game.")]
        float m_AfterSymbolShowTimeLimit = 1.0f;
        [SerializeField]
        [Tooltip("Time it takes to time out for input.")]
        float m_PerSymbolInputTimeLimit = 1.0f;
        [SerializeField]
        [Tooltip("Time it takes after a successful input.")]
        float m_PostGoodTimeLimit = 1.0f;
        [SerializeField]
        [Tooltip("Time it takes after a successful input.")]
        float m_PostErrorTimeLimit = 1.0f;

        [Header("Random:")]
        [SerializeField]
        [Tooltip("Random Seed")]
        bool m_RandomSeed;
        [SerializeField]
        [Tooltip("Random Seed Num")]
        int m_SeedNumForRandom = 42;

        [Header("Dialog Text:")]
        [SerializeField]
        [Tooltip("Fill in for success message")]
        string[] m_WinMessageStrings;


        bool m_AllButtonsOff = false;
        bool m_AddSymbolAndPlayGame = false;
        int m_CurrentPuzzelIndex = 0;
        bool m_OnClickGoodBool = false;
        bool m_OnClickBadBool = false;
        Button m_ButtonClicked;

        public class SymbolData
        {
            Button m_ButtonPtr;
            Color m_Color;

            public SymbolData(Button button, Color color)
            {
                m_ButtonPtr = button;
                m_Color = color;
            }

           
            public Color Color { get => m_Color; }
            public Button ButtonPtr { get => m_ButtonPtr;}
        }

        IList<SymbolData> m_SymbolDataList;

        public Button[] Buttons { get => m_Buttons; set => m_Buttons = value; }
        public bool AllButtonsOff { get => m_AllButtonsOff; set => m_AllButtonsOff = value; }
        public bool AddSymbolAndPlayGame { get => m_AddSymbolAndPlayGame; set => m_AddSymbolAndPlayGame = value; }
        public IList<SymbolData> SymbolDataList { get => m_SymbolDataList; set => m_SymbolDataList = value; }
        public bool RandomSeed { get => m_RandomSeed; }
        public int SeedNumForRandom { get => m_SeedNumForRandom; }
        public Color[] PuzzelColors { get => m_PuzzelColors; }
        public int CurrentPuzzelIndex { get => m_CurrentPuzzelIndex; set => m_CurrentPuzzelIndex = value; }
        public float IncrementColorTimeLimit { get => m_IncrementColorTimeLimit; }
        public float AfterSymbolShowTimeLimit { get => m_AfterSymbolShowTimeLimit; }
        public SimpleHealthBar TimerLeftBar { get => m_TimerLeftBar; set => m_TimerLeftBar = value; }
        public float PerSymbolInputTimeLimit { get => m_PerSymbolInputTimeLimit; set => m_PerSymbolInputTimeLimit = value; }
        public bool OnClickGoodBool { get => m_OnClickGoodBool; set => m_OnClickGoodBool = value; }
        public bool OnClickBadBool { get => m_OnClickBadBool; set => m_OnClickBadBool = value; }
        public Button ButtonClicked { get => m_ButtonClicked;}
        public int PuzzelWinLimit { get => m_PuzzelWinLimit;}
        public aMechGameGUIBase WinDialog { get => m_WinDialog; set => m_WinDialog = value; }
        public aMechGameGUIBase LooseDialog { get => m_LooseDialog; set => m_LooseDialog = value; }
        public AudioSource ErrorSound { get => m_ErrorSound; set => m_ErrorSound = value; }
        public float PostGoodTimeLimit { get => m_PostGoodTimeLimit; }
        public float PostErrorTimeLimit { get => m_PostErrorTimeLimit;}
        public aMechGameGUIBase RepeatDialog { get => m_RepeatDialog; set => m_RepeatDialog = value; }
        public aMechGameGUIBase WatchDialog { get => m_WatchDialog; set => m_WatchDialog = value; }
        public aMechGameGUIBase TimerBar { get => m_TimerBar; set => m_TimerBar = value; }
        public string[] WinMessageStrings { get => m_WinMessageStrings; }
        public aMechFighter FighterPlayer { get => m_FighterPlayer; set => m_FighterPlayer = value; }

        public void OnButtonClick(Button button)
        {
            if (CurrentPuzzelIndex >= 0 && CurrentPuzzelIndex<SymbolDataList.Count)
            {
                if (button == SymbolDataList[CurrentPuzzelIndex].ButtonPtr)
                    OnClickGoodBool = true;
                else
                    OnClickBadBool = true;
                m_ButtonClicked = button;
            }

        }

        new void Awake()
        {
            m_SymbolDataList = new List<SymbolData>();
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

        public bool IsInInputGameState()
        {
            if (m_CurrentState is gmControllerWaitForInput)
                return true;
            return false;
        }

        /// <summary>
        /// Autogenerated state are created here inside this function.
        /// </summary>
        void CreateStates()
        {

            m_CurrentState = AddState(new gmControllerInit(this.gameObject), "gmControllerInit");

            //<ArtiMechStates>
            AddState(new gmControllerResetPuzzelIndex(this.gameObject),"gmControllerResetPuzzelIndex");
            AddState(new gmControllerInputBad(this.gameObject),"gmControllerInputBad");
            AddState(new gmControllerInputTimeOut(this.gameObject), "gmControllerInputTimeOut");
            AddState(new gmControllerWaitForInput(this.gameObject), "gmControllerWaitForInput");
            AddState(new gmControllerIncrementSymbol(this.gameObject), "gmControllerIncrementSymbol");
            AddState(new gmTurnOffAllButtonInput(this.gameObject), "gmTurnOffAllButtonInput");
            AddState(new gmControllerTurnOffSideButtonInput(this.gameObject), "gmControllerTurnOffSideButtonInput");
            AddState(new gmControllerSuccessInput(this.gameObject), "gmControllerSuccessInput");
            AddState(new gmControllerFailedInput(this.gameObject), "gmControllerFailedInput");
            AddState(new gmControllerInputIsGood(this.gameObject), "gmControllerInputIsGood");
            AddState(new gmControllerAddSymbol(this.gameObject), "gmControllerAddSymbol");
            AddState(new gmControllerPlayBackSymbol(this.gameObject), "gmControllerPlayBackSymbol");
            AddState(new gmControllerWait(this.gameObject), "gmControllerWait");

        }
    }
}
