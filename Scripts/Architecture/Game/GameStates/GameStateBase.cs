using PaleLuna.Architecture.Loops;

namespace PaleLuna.Patterns.State.Game
{
    /**
 * @brief Базовый абстрактный класс для состояний игры.
 *
 * GameStateBase предоставляет базовый функционал для всех состояний игры. Он содержит ссылку на объект GameController,
 * который представляет текущий контекст игры.
 */
    public abstract class GameStateBase : State
    {
        /** @brief Контекст игры, представляющий GameController. */
        protected GameLoops _context;

        /**
     * @brief Конструктор класса.
     *
     * Инициализирует поле _context объектом GameController, представляющим текущий контекст игры.
     *
     * @param context Объект GameController, представляющий текущий контекст игры.
     */
        public GameStateBase(GameLoops context) =>
            _context = context;
    }
}