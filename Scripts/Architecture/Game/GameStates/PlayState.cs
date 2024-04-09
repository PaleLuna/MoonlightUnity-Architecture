using PaleLuna.Architecture.Loops;

namespace PaleLuna.Patterns.State.Game
{
    /**
 * @brief Состояние воспроизведения в игре.
 *
 * PlayState представляет состояние воспроизведения в игре. При входе в это состояние вызывается метод OnResume для всех объектов,
 * зарегистрированных в pausablesHolder контроллера игры.
 */
    public class PlayState : GameStateBase
    {
        /**
         * @brief Конструктор класса.
         *
         * Инициализирует объект PlayState с заданным контекстом игры.
         *
         * @param context Объект GameController, представляющий текущий контекст игры.
         */
        public PlayState(GameLoops context) : base(context)
        {
        }

        /**
         * @brief Метод, вызываемый при начале состояния.
         *
         */
        public override void StateStart() =>
            _context.pausablesHolder
                .ForEach(pausable => pausable.OnResume());
    }
}