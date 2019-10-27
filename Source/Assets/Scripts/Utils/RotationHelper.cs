using UnityEngine;

namespace gRaFFit.Agar.Utils {
	public static class RotationHelper {

		/// <summary>
		/// Возвращает поворот при просмотре объекта с позиции from в позицию to
		/// </summary>
		/// <param name="fromPosition">Позиция, из которой нужно смотреть</param>
		/// <param name="toPosition">Позиция, в которую нужно смотреть</param>
		/// <returns>Правильный поворот при взгляде с позиции from в позицию to</returns>
		public static Quaternion FaceObject(Vector3 fromPosition, Vector3 toPosition, float degreesToLook) {
			var direction = toPosition - fromPosition;
			var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - degreesToLook;
			return Quaternion.AngleAxis(angle, Vector3.forward);
		}
	}
}