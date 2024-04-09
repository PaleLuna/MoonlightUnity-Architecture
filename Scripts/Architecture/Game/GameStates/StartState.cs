using PaleLuna.Architecture.Loops;

namespace PaleLuna.Patterns.State.Game
{
    /**
 * @brief Состояние начала в игре.
 *
 * StartState представляет состояние начала в игре. При входе в это состояние вызывается метод OnStart для всех объектов,
 * зарегистрированных в startableHolder контроллера игры.
 */
    public class StartState : GameStateBase
    {
        /**
         * @brief Конструктор класса.
         *
         * Инициализирует объект StartState с заданным контекстом игры.
         *
         * @param context Объект GameController, представляющий текущий контекст игры.
         */
        public StartState(GameLoops context) : base(context)
        {
        }

        /**
       * @brief Метод, вызываемый при начале состояния.
       *
       */
        public override void StateStart() =>
            _context.startableHolder
                .ForEach(startable => startable.OnStart());
    }
}