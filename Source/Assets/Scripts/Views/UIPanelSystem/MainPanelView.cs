using gRaFFit.Agar.Utils.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace gRaFFit.Agar.Views.UIPanelSystem {
	public class MainPanelView : UIPanelView {
		[SerializeField] private Button _playButton;

		public Signal SignalOnPlayButtonClicked;

		protected override void InitSignals() {
			if (!IsSignalsInited) {
				SignalOnPlayButtonClicked = new Signal();

				base.InitSignals();
			}
		}

		protected override void DestroySignals() {
			if (IsSignalsInited) {
				SignalOnPlayButtonClicked.RemoveAllListeners();
				SignalOnPlayButtonClicked = null;

				base.DestroySignals();
			}
		}

		protected override void AddListeners() {
			base.AddListeners();

			_playButton.onClick.AddListener(OnPlayButtonClicked);
		}

		public override void RemoveListeners() {
			_playButton.onClick.RemoveAllListeners();

			base.RemoveListeners();
		}

		private void OnPlayButtonClicked() {
			SignalOnPlayButtonClicked.Dispatch();
		}
	}
}