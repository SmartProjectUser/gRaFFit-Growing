using System;
using gRaFFit.Agar.Utils.Signals;
using gRaFFit.Agar.Views.UIPanelSystem;
using UnityEngine;
using UnityEngine.UI;

public class HudPanelView : UIPanelView {
	[SerializeField] private Text _yourWeightText;
	[SerializeField] private Button _restartButton;
	[SerializeField] private Button _goToLobbyButton;
	[SerializeField] private float _weightRefreshSpeed;
	
	public Signal SignalOnRestartButtonClicked;
	public Signal SignalOnGoToLobbyButtonClicked;

	protected override void InitSignals() {
		if (!IsSignalsInited) {
			SignalOnRestartButtonClicked = new Signal();
			SignalOnGoToLobbyButtonClicked = new Signal();
			
			base.InitSignals();
		}
	}

	protected override void DestroySignals() {
		if (IsSignalsInited) {
			SignalOnRestartButtonClicked.RemoveAllListeners();
			SignalOnRestartButtonClicked = null;

			SignalOnGoToLobbyButtonClicked.RemoveAllListeners();
			SignalOnGoToLobbyButtonClicked = null;

			base.DestroySignals();
		}
	}

	protected override void AddListeners() {
		base.AddListeners();

		_restartButton.onClick.AddListener(OnRestartButtonClicked);
		_goToLobbyButton.onClick.AddListener(OnGoToLobbyButtonClicked);
	}


	public override void RemoveListeners() {
		base.RemoveListeners();

		_restartButton.onClick.RemoveListener(OnRestartButtonClicked);
		_goToLobbyButton.onClick.RemoveListener(OnGoToLobbyButtonClicked);
	}

	private float _targetWeight;
	private float _displayingWeight;

	public void RefreshWeight(float newWeight, bool immediately) {
		_targetWeight = newWeight;
		if (immediately) {
			_displayingWeight = newWeight;
			_yourWeightText.text = _displayingWeight.ToString();
		}
	}

	public void Update() {
		_displayingWeight = Mathf.Lerp(_displayingWeight, _targetWeight, Time.deltaTime * _weightRefreshSpeed);
		_yourWeightText.text = $"{_displayingWeight:F}";
	}

	private void OnRestartButtonClicked() {
		SignalOnRestartButtonClicked.Dispatch();
	}

	private void OnGoToLobbyButtonClicked() {
		SignalOnGoToLobbyButtonClicked.Dispatch();
	}
}