﻿using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace _Scripts.Architecture.DataHolders.Implementations
{
    public class UpdatablesHolder
    {
        public DataHolder<IUpdatable> updatablesHolder { get; private set; }
        public DataHolder<IFixedUpdatable> fixedUpdatablesHolder { get; private set; }
        public DataHolder<ILateUpdatable> lateUpdatablesHolder { get; private set; }
        public DataHolder<ITickUpdatable> tickUpdatableHolder { get; private set; }

        public void Registration(IUpdatable item) =>
            updatablesHolder.Registration(item);
        public void Registration(IFixedUpdatable item) =>
            fixedUpdatablesHolder.Registration(item);
        public void Registration(ILateUpdatable item) =>
            lateUpdatablesHolder.Registration(item);
        public void Registration(ITickUpdatable item) =>
            tickUpdatableHolder.Registration(item);
        
        public void UnRegistration(IUpdatable item) =>
            updatablesHolder.UnRegistration(item);
        public void UnRegistration(IFixedUpdatable item) =>
            fixedUpdatablesHolder.UnRegistration(item);
        public void UnRegistration(ILateUpdatable item) =>
            lateUpdatablesHolder.UnRegistration(item);
        public void UnRegistration(ITickUpdatable item) =>
            tickUpdatableHolder.UnRegistration(item);
        
    }
}