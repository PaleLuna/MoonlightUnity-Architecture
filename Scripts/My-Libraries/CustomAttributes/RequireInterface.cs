using System;
using UnityEngine;

namespace PaleLuna.Attributes
{
    /**
 * @brief Атрибут RequireInterface используется для указания типа интерфейса, который должен реализовываться объектами в Unity Inspector.
 *
 * RequireInterface - это настраиваемый атрибут, предназначенный для использования в Unity Inspector.
 * Он позволяет указать тип интерфейса (RequireType), который должен быть реализован объектами, а также опционально разрешает использование объектов сцены (allowSceneObject).
 */
    public class RequireInterface : PropertyAttribute
    {
        /**
         * @brief Тип интерфейса, который должен быть реализован объектами.
         */
        public readonly Type RequireType;
        /**
        * @brief Разрешение использования объектов сцены.
        */
        public bool allowSceneObject;

        /**
         * @brief Конструктор RequireInterface.
         *
         * @param requireType Тип интерфейса, который должен быть реализован объектами.
         * @param allowSceneObject Опциональный параметр для разрешения использования объектов сцены (по умолчанию true).
         */
        public RequireInterface(Type requireType, bool allowSceneObject = true)
        {
            RequireType = requireType;
            this.allowSceneObject = allowSceneObject;
        }
    }
}