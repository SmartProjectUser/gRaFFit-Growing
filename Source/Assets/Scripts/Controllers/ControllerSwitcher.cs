using System.Collections.Generic;
using Controllers;
using gRaFFit.Agar.Controllers.GameScene.MainControllers;
using UnityEngine;

namespace gRaFFit.Agar.Models.ControllerSwitcherSystem {
	/// <summary>
	/// Переключатель главных контроллеров игры
	/// </summary>
	public class ControllerSwitcher : MonoBehaviour {

		#region MonoSingleton

		private ControllerSwitcher() {
		}

		private void Awake() {
			if (Instance != null && Instance != this) {
				Destroy(Instance.gameObject);
			}

			Instance = this;
		}

		public static ControllerSwitcher Instance { get; private set; }

		#endregion

		#region Public Fields

		/// <summary>
		/// Тип текущего активного контроллера
		/// </summary>
		public ControllerType CurrentControllerType { get; private set; }

		#endregion

		#region Private Fields

		/// <summary>
		/// Список контроллеров
		/// </summary>
		[SerializeField] private List<AControllerView> _controllers = new List<AControllerView>();

		#endregion

		#region Public Methods

		/// <summary>
		/// Деактивирует текущий контроллер, и включает следующий
		/// </summary>
		/// <param name="type">Тип контроллера</param>
		public void SwitchController(ControllerType type) {
			var previousController = GetCurrentController();
			CurrentControllerType = type;

			if (previousController != null) {
				previousController.Deactivate();
			}

			GetCurrentController().Activate();
		}

		public void RestartCurrentController() {
			var currentController = GetCurrentController();
			currentController.Deactivate();
			currentController.Activate();
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Вызывается при старте работы контроллера
		/// </summary>
		private void Start() {
			Input.multiTouchEnabled = false;
			SwitchController(ControllerType.Lobby);
		}

		/// <summary>
		/// Возвращает текущий активированный контроллер
		/// </summary>
		private AControllerView GetCurrentController() {
			var result = _controllers.Find(controller => controller.Type == CurrentControllerType);
			if (result == null) {
				switch (CurrentControllerType) {
					case ControllerType.Lobby:
						result = gameObject.AddComponent<LobbyControllerView>();
						break;
					case ControllerType.Game:
						result = gameObject.AddComponent<GameControllerView>();
						break;
				}

				if (result != null) {
					_controllers.Add(result);
				}
			}

			return result;
		}

		#endregion
	}
}