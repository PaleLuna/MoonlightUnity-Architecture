﻿namespace PaleLuna.Architecture.GameComponent
{
    /**
 * @brief Интерфейс для компонентов игры, которые должны выполнять обновление в каждый такт игрового времени.
 *
 * ITickUpdatable представляет интерфейс для компонентов игры, которые должны выполнять свои действия в каждый такт игрового времени.
 * Реализующие этот интерфейс компоненты должны предоставить реализацию метода EveryTickRun.
 */
    public interface ITickUpdatable : IGameComponent
    {
        /**
        * @brief Метод, вызываемый в каждый такт игрового времени.
        *
        * Метод EveryTickRun вызывается в каждый такт игрового времени, и компонент должен выполнить свои действия в этот момент времени.
        */
        void EveryTickRun();
    }
}