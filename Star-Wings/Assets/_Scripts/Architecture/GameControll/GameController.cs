using System;
using _Scripts.Architecture.DataHolders.Implementations;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public UpdatablesHolder updatablesHolder { get; private set; }
    public DataHolder<IPausable> pausablesHolder { get; private set; }
    public DataHolder<IStartable> startableHolder { get; private set; }

    public void Start()
    {
        updatablesHolder = new UpdatablesHolder();
        pausablesHolder = new DataHolder<IPausable>();
        startableHolder = new DataHolder<IStartable>();
    }
}
