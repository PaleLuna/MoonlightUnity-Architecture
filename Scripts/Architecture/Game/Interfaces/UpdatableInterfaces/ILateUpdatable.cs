﻿namespace PaleLuna.Architecture.GameComponent
{
    /**
 * @brief Интерфейс для компонентов игры, которые должны выполнять обновление в каждом позднем кадре.
 *
 * ILateUpdatable представляет интерфейс для компонентов игры, которые должны выполнять свои действия в каждом позднем кадре.
 * Реализующие этот интерфейс компоненты должны предоставить реализацию метода LateUpdateRun.
 */
    public interface ILateUpdatable : IGameComponent
    {
        /**
         * @brief Метод, вызываемый в каждом позднем кадре.
         *
         * Метод LateUpdateRun вызывается в каждом позднем кадре, и компонент должен выполнить свои действия в этот момент времени.
         */
        void LateUpdateRun();
    }
}