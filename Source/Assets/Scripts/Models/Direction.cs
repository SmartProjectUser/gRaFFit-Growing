using UnityEngine;

namespace gRaFFit.Agar.Models.SharedHelpers {
    /// <summary>
    /// Перечисление направлений
    /// </summary>
    public enum Direction {
        /// <summary>
        /// Направление вверх
        /// </summary>
        Up,

        /// <summary>
        /// Направление вправо
        /// </summary>
        Right,

        /// <summary>
        /// Направление вниз
        /// </summary>
        Down,

        /// <summary>
        /// Направление влево
        /// </summary>
        Left
    }

    /// <summary>
    /// Расширения для Enum
    /// </summary>
    public static class EnumExtensions {

        public static Direction InvertDirection(this Direction direction) {

            switch (direction) {
                case Direction.Up:
                    direction = Direction.Down;
                    break;

                case Direction.Down:
                    direction = Direction.Up;
                    break;

                case Direction.Right:
                    direction = Direction.Left;
                    break;

                case Direction.Left:
                    direction = Direction.Right;
                    break;

                default:
                    Debug.LogError($"Direction.InvertDirection: Can't invert direction of {direction} type");
                    break;
            }

            return direction;
        }

        /// <summary>
        /// Возвращает вектор необходимого сдвига по клеткам для перемещения в необходимое направление
        /// </summary>
        /// <param name="direction">Направление</param>
        /// <returns>Ветктор сдвига по клеткам для перемещения в необходимое направление</returns>
        public static Vector2Int GetOffset(this Direction direction) {
            var result = Vector2Int.zero;

            switch (direction) {
                case Direction.Up:
                    result = new Vector2Int(0, 1);
                    break;
                case Direction.Right:
                    result = new Vector2Int(1, 0);
                    break;
                case Direction.Down:
                    result = new Vector2Int(0, -1);
                    break;
                case Direction.Left:
                    result = new Vector2Int(-1, 0);
                    break;
            }

            return result;
        }
    }
}