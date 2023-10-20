using System;
using _Scripts.Architecture.DataHolders.Implementations;
using _Scripts.Other.Patterns.StatePattern;
using UnityEngine;

public class GameController : MonoBehaviour, IService
{
    public UpdatablesHolder updatablesHolder { get; private set; }
    public DataHolder<IPausable> pausablesHolder { get; private set; }
    public DataHolder<IStartable> startableHolder { get; private set; }
    
    public StateHolder<GameStateBase> stateHolder { get; private set; }


    #region MonoEvents

    private void Start()
    {
        stateHolder = new StateHolder<GameStateBase>();
        
        updatablesHolder = new UpdatablesHolder();
        pausablesHolder = new DataHolder<IPausable>();
        startableHolder = new DataHolder<IStartable>();
    }


    private void Update() => 
        updatablesHolder.everyFrameUpdatablesHolder
            .ForEach(updatable => updatable.EveryFrameRun());

    private void FixedUpdate() => 
        updatablesHolder.fixedUpdatablesHolder
            .ForEach(updatable => updatable.FixedFrameRun());

    private void LateUpdate() =>
        updatablesHolder.lateUpdatablesHolder
            .ForEach(updatable => updatable.LateUpdateRun());

    private void Tick() =>
        updatablesHolder.tickUpdatableHolder
            .ForEach(updatable => updatable.EveryTickRun());

    #endregion
}
