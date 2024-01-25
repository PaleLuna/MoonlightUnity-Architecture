using PaleLuna.Architecture.Services;
using PaleLuna.DataHolder;
using PaleLuna.Patterns.Singletone;

/**
* @brief Класс для регистрации, отмены регистрации и получения сервисов в приложении.
*
* ServiceLocator является синглтоном и предоставляет удобный способ регистрации, отмены регистрации и получения сервисов в приложении.
*/

public class ServiceLocator
{
    /** @brief Хранилище компонентов, зарегистрированных в сервис-локаторе. */
    protected DictionaryDataHolder<IService> _componentsMap;

    /**
* @brief Конструктор класса.
*
* Создает экземпляр класса и инициализирует хранилище компонентов.
*/
    public ServiceLocator()
    {
        _componentsMap = new DictionaryDataHolder<IService>();
    }

    /**
 * @brief Регистрация компонента в сервис-локаторе.
 *
 * @tparam TP Тип компонента для регистрации.
 * @param item Компонент для регистрации.
 * @return Зарегистрированный компонент.
 *
 * Пример использования:
 * @code
 * ServiceLocator.Instance.Registration<MyService>(new MyService());
 * @endcode
 */
    public TP Registarion<TP>(TP item)
        where TP : IService
    {
        return _componentsMap.Registration<TP>(item);
    }

    /**
 * @brief Отмена регистрации компонента из сервис-локатора.
 *
 * @tparam TP Тип компонента для отмены регистрации.
 * @return Отмененный компонент.
 *
 * Пример использования:
 * @code
 * ServiceLocator.Instance.Unregistration<MyService>();
 * @endcode
 */
    public TP Unregistration<TP>()
        where TP : IService
    {
        return _componentsMap.Unregistration<TP>();
    }

    /**
 * @brief Получение компонента указанного типа из сервис-локатора.
 *
 * @tparam TP Тип компонента для получения.
 * @return Компонент указанного типа или значение по умолчанию, если компонент не зарегистрирован.
 *
 * Пример использования:
 * @code
 * MyService service = ServiceLocator.Instance.Get<MyService>();
 * @endcode
 */
    public TP Get<TP>()
        where TP : IService => _componentsMap.GetByType<TP>();
}
