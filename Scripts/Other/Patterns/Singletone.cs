using UnityEngine;

namespace PaleLuna.Patterns.Singletone
{
    
    /**
     * @brief Класс Singletone<T> реализует шаблон одиночки для компонента Unity.
     *
     * Singletone<T> обеспечивает доступ к единственному экземпляру компонента T в рамках сцены.
     * Если экземпляр не найден, Singletone<T> пытается найти его в сцене или создать новый.
     *
     * @typeparam T Тип компонента, который должен быть производным от Component.
     */
    public class Singletone<T> : MonoBehaviour where T : Component
    {
        /**
        * @brief Статическая переменная, содержащая единственный экземпляр компонента T.
        */
        private static T _instance;

        /**
         * @brief Статическое свойство, предоставляющее доступ к единственному экземпляру компонента T.
         */
        public static T Instance => GetInstance();

        /**
        * @brief Получение единственного экземпляра компонента T.
        *
        * Если экземпляр не был найден, Singletone<T> пытается найти его в сцене или создать новый.
        *
        * @return Единственный экземпляр компонента T.
        */
        private static T GetInstance()
        {
            if (!_instance)
            {
                _instance = FindFirstObjectByType<T>();

                if (!_instance)
                {
                    GameObject gObj = GameObject.Find("DontDestroy");
                    _instance = gObj?.AddComponent<T>();
                }
            }

            return _instance;
        }

        /**
        * @brief Метод Awake, вызываемый Unity при активации объекта.
        *
        * Если экземпляр еще не был установлен, устанавливает текущий объект в качестве единственного экземпляра.
        * В противном случае уничтожает текущий объект.
        */
        private void Awake()
        {
            if (!_instance)
                _instance = this as T;
            else
                Destroy(this);
        }
    }
}