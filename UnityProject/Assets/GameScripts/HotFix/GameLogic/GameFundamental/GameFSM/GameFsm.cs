using System;
using System.Collections.Generic;

namespace GameLogic.GameFSM
{
    public class GameFsm : LogicBase
    {
        
        public GameFsmStateBase lastFsmState;
        public GameFsmStateBase currentFsmState;
        public object[] stateEnterParams;
    
        public Type[] fsmStateTypes =
        {
            typeof(GameStateLoading),
            typeof(GameStateLobby),
          
        };
        private List<GameFsmStateBase> _gameStates;
        public override void InitializeOnCreate()
        {
            base.InitializeOnCreate();

            _gameStates = new List<GameFsmStateBase>();
            
            for (var i = 0; i < fsmStateTypes.Length; i++)
            {
                _gameStates.Add((GameFsmStateBase) Activator.CreateInstance(fsmStateTypes[i]));
            }
            
            
            foreach (var gameState in _gameStates)
            {
                gameState.OnInit();
            }
        }
        
        /// <summary>
        /// 开始流程。
        /// </summary>
        /// <typeparam name="T">要开始的流程类型。</typeparam>
        public void StartState<T>() where T : GameFsmStateBase
        {
            currentFsmState = _gameStates[0];
            _gameStates[0].OnEnter(null);
        }
 

        public async void ToState<T>(params object[] enterParams) where T : GameFsmStateBase
        {
            GameFsmStateBase nextState = null;

            foreach (var state in _gameStates)
            {
                if (state.GetType() == typeof(T))
                {
                    nextState = state;
                }
            }

            if (nextState == null)
            {
                return;
            }

            if (currentFsmState != null)
            {
                lastFsmState = currentFsmState;
                await currentFsmState.PreOnLeave(nextState);
                currentFsmState.OnLeave();
                await nextState.PreOnEnter(currentFsmState);
                
                currentFsmState = nextState;
                nextState.OnEnter(enterParams);
            }
            else
            {
                currentFsmState = nextState;
                nextState.OnEnter(enterParams);
            }
        }
 
        
    }
}