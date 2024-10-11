using PaleLuna.Architecture.GameComponent;
using PaleLuna.Architecture.Services;
using PaleLuna.DataHolder;
using PaleLuna.DataHolder.Updatables;
using PaleLuna.Patterns.State;
using PaleLuna.Patterns.State.Game;
using UnityEngine;

namespace PaleLuna.Architecture.Loops
{
    public class GameLoops : MonoBehaviour, IService, IStartable
    {
        private bool _isStarted;

        public DataHolder<IPausable> pausablesHolder { get; private set; }
        public UpdatablesHolder updatablesHolder { get; private set; }
        public StateHolder<GameStateBase> stateHolder { get; private set; }

        public bool IsStarted => _isStarted;

        public void OnStart()
        {
            if (_isStarted) return;

            stateHolder = new StateHolder<GameStateBase>();

            updatablesHolder = new UpdatablesHolder();
            pausablesHolder = new DataHolder<IPausable>();

            _isStarted = true;
        }

        #region Registration
        
        #region AddToList
            public void Registration(IUpdatable component) => updatablesHolder.Registration(component);
            public void Registration(IFixedUpdatable component) => updatablesHolder.Registration(component);
            public void Registration(ILateUpdatable component) => updatablesHolder.Registration(component);
    
            public void Registration(IUpdatable component, int order) => updatablesHolder.Registration(component, order);
            public void Registration(IFixedUpdatable component, int order) => updatablesHolder.Registration(component, order);
            public void Registration(ILateUpdatable component, int order) => updatablesHolder.Registration(component, order);
        #endregion

        #region RemoveFromList
            public void Unregistration(IUpdatable component)  => updatablesHolder.UnRegistration(component);
            public void Unregistration(IFixedUpdatable component) => updatablesHolder.UnRegistration(component);
            public void Unregistration(ILateUpdatable component) => updatablesHolder.UnRegistration(component);
        #endregion

        #endregion

        #region MonoEvents

        private void Update()
        {
            updatablesHolder.everyFrameUpdatablesHolder
                .ForEach(updatable => updatable.EveryFrameRun());
        }

        private void FixedUpdate()
        {
            updatablesHolder.fixedUpdatablesHolder
                .ForEach(updatable => updatable.FixedFrameRun());
        }

        private void LateUpdate()
        {
            updatablesHolder.lateUpdatablesHolder
                .ForEach(updatable => updatable.LateUpdateRun());
        }

        private void Tick()
        {
            updatablesHolder.tickUpdatableHolder
                .ForEach(updatable => updatable.EveryTickRun());
        }

        #endregion
    }
}