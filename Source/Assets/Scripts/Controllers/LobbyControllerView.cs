using gRaFFit.Agar.Controllers.GameScene.MainControllers;
using gRaFFit.Agar.Models.ControllerSwitcherSystem;
using gRaFFit.Agar.Views.UIPanelSystem;

namespace Controllers {
	public class LobbyControllerView : AControllerView {

		public override ControllerType Type => ControllerType.Lobby;


		public override void Activate() {
			UIManager.Instance.ShowPanel<MainPanelView>();
			
			AddListeners();
		}

		public override void Deactivate() {
			UIManager.Instance.HidePanel<MainPanelView>();

			base.Deactivate();
		}
		
		protected override void AddListeners() {
			base.AddListeners();

			var mainPanel = UIManager.Instance.GetPanel<MainPanelView>();
			if (mainPanel != null) {
				mainPanel.SignalOnPlayButtonClicked.AddListener(OnPlayButtonClicked);
			}
		}
		
		protected override void RemoveListeners() {
			var mainPanel = UIManager.Instance.GetPanel<MainPanelView>();
			if (mainPanel != null && mainPanel.IsSignalsInited) {
				mainPanel.SignalOnPlayButtonClicked.RemoveListener(OnPlayButtonClicked);
			}

			base.RemoveListeners();
		}


		private void OnPlayButtonClicked() {
			ControllerSwitcher.Instance.SwitchController(ControllerType.Game);
		}
	}
}