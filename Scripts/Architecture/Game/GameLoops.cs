using PaleLuna.Architecture.GameComponent;
using PaleLuna.Architecture.Services;
using PaleLuna.DataHolder;
using PaleLuna.DataHolder.Updatables;
using PaleLuna.Patterns.State;
using PaleLuna.Patterns.State.Game;
using PaleLuna.Timers.Implementations;
using UnityEngine;

namespace PaleLuna.Architecture.Loops
{
    public class GameLoops : MonoBehaviour, IService, IStartable
    {
        public DataHolder<IPausable> pausablesHolder { get; private set; }
        public UpdatablesHolder updatablesHolder { get; private set; }
        public StateHolder<GameStateBase> stateHolder { get; private set; }

        public TickMachine tickMachine { get; private set; }

        public GameLoopsConfig GLConfig { get; private set; }


        private bool _isStarted;
        public bool IsStarted => _isStarted;

        public void OnStart()
        {
            if (_isStarted) return;

            stateHolder = new StateHolder<GameStateBase>();

            updatablesHolder = new UpdatablesHolder();
            pausablesHolder = new DataHolder<IPausable>();

            tickMachine = new();
            tickMachine.SetAction(Tick);

            _isStarted = true;
        }

        public void SetGLConfig(GameLoopsConfig config)
        {
            if (GLConfig != null) return;
            
            GLConfig = config;
            tickMachine.SetTimeForTick(GLConfig.timeForTickPerSeconds);
        }

        #region Registration
        
        #region AddToList
            public void Registration(IUpdatable component) => updatablesHolder.Registration(component);
            public void Registration(IFixedUpdatable component) => updatablesHolder.Registration(component);
            public void Registration(ILateUpdatable component) => updatablesHolder.Registration(component);
            public void Registration(ITickUpdatable component) => updatablesHolder.Registration(component);



            public void Registration(IUpdatable component, int order) => updatablesHolder.Registration(component, order);
            public void Registration(IFixedUpdatable component, int order) => updatablesHolder.Registration(component, order);
            public void Registration(ILateUpdatable component, int order) => updatablesHolder.Registration(component, order);
            public void Registration(ITickUpdatable component, int order) => updatablesHolder.Registration(component, order);
        #endregion

        #region RemoveFromList
            public void Unregistration(IUpdatable component)  => updatablesHolder.UnRegistration(component);
            public void Unregistration(IFixedUpdatable component) => updatablesHolder.UnRegistration(component);
            public void Unregistration(ILateUpdatable component) => updatablesHolder.UnRegistration(component);
            public void Unregistration(ITickUpdatable component) => updatablesHolder.UnRegistration(component);
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