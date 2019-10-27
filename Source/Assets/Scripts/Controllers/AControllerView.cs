using gRaFFit.Agar.Models.ControllerSwitcherSystem;
using UnityEngine;

namespace gRaFFit.Agar.Controllers.GameScene.MainControllers {
	/// <summary>
	/// Базовый класс контроллера
	/// </summary>
	public abstract class AControllerView : MonoBehaviour {
		/// <summary>
		/// Включение контроллера
		/// </summary>
		public abstract void Activate();

		/// <summary>
		/// Отключение контроллера
		/// </summary>
		public virtual void Deactivate() {
			RemoveListeners();
		}

		/// <summary>
		/// Тип контроллера
		/// </summary>
		public abstract ControllerType Type { get; }

		/// <summary>
		/// Подписка методов на сигналы
		/// </summary>
		protected virtual void AddListeners() {
			RemoveListeners();
		}

		/// <summary>
		/// Отписка методов от сигналов
		/// </summary>
		protected virtual void RemoveListeners() {
		}
	}
}