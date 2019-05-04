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
    public class aMechFighter : stateMachineGame
    {
        [Header("aMechFighter:")]
        [SerializeField]
        [Tooltip("Fade in curve in seconds.")]
        AnimationCurve m_FadeInCurve;
        [SerializeField]
        [Tooltip("Sprite Renderer.")]
        SpriteRenderer m_SpriteRenderer;

        [Header("Movement Data:")]
        [SerializeField]
        [Tooltip("Move onto screen curve.")]
        AnimationCurve m_MoveIntoScreenCurve;
        [SerializeField]
        [Tooltip("Move onto screen curve.")]
        Transform m_StartTrans;
        [SerializeField]
        [Tooltip("Punch Offset Position.")]
        Vector3 m_PunchOffsetPos;

        Vector3 m_StartPos;

        public AnimationCurve FadeInCurve { get => m_FadeInCurve;}
        public SpriteRenderer SpriteRenderer { get => m_SpriteRenderer; set => m_SpriteRenderer = value; }
        public AnimationCurve MoveIntoScreenCurve { get => m_MoveIntoScreenCurve;}
        public Vector3 SpawnPos { get => m_StartTrans.position; }
        public Vector3 StartPos { get => m_StartPos;}
        public Vector3 PunchOffsetPos { get => m_PunchOffsetPos;}

        new void Awake()
        {
            base.Awake();
            CreateStates();
        }

        // Use this for initialization
        new void Start()
        {
            m_StartPos = transform.position;
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

            m_CurrentState = AddState(new fighterInit(this.gameObject), "fighterInit");

            //<ArtiMechStates>
            AddState(new fighterStart(this.gameObject),"fighterStart");
            AddState(new fighterFadeInAndMoveOntoScreen(this.gameObject),"fighterFadeInAndMoveOntoScreen");

        }
    }
}