using PaleLuna.Patterns.Singletone;

namespace Services
{
    /**
    * @brief Класс, содержащий глобольный и локальный ServiceLocator'ы
    *
    * Этот класс предоставляет доступ к глобальном и локальным сервисам.
*/
    public class ServiceManager : Singletone<ServiceManager>
    {
        private ServiceLocator _globalServices;
        private ServiceLocator _localServices;

        public ServiceLocator GlobalServices
        {
            get => _globalServices;
            set
            {
                if (_globalServices == null)
                    _globalServices = value;
            }
        }

        public ServiceLocator LocalServices
        {
            get => _localServices;
            set => _localServices = value;
        }
    }
}
