using gRaFFit.Agar.Controllers.GameScene.MainControllers;
using gRaFFit.Agar.Controllers.InputSystem;
using gRaFFit.Agar.Models.ControllerSwitcherSystem;
using gRaFFit.Agar.Views;
using gRaFFit.Agar.Views.CameraControls;
using gRaFFit.Agar.Views.Pool;
using gRaFFit.Agar.Views.UIPanelSystem;
using UnityEngine;
using Character = Models.Character;

namespace Controllers {
	public class GameControllerView : AControllerView {
		[SerializeField] private MeshRenderer _bgMeshRenderer;
		[SerializeField] private int _targetCharactersCount;
		[SerializeField] private EnemyView _enemyViewPrefab;
		[SerializeField] private PlayerView _playerView;
		[SerializeField] private GameObject _hurtEffect;

		private Character _player;
		private Vector3 _playerStartPosition;
		private const string ENEMY_POOL_KEY = "ENEMY_POOL_KEY";

		public void Awake() {
			_playerStartPosition = _playerView.transform.position;
		}

		public override void Activate() {
			PoolService.Instance.InitPoolWithNewObject(ENEMY_POOL_KEY, _enemyViewPrefab);
			UIManager.Instance.ShowPanel<HudPanelView>();

			_bgMeshRenderer.gameObject.SetActive(true);
			CameraView.Instance.SetToPlayer(_playerView);
			CookieSpawner.Instance.Init();
			CookieSpawner.Instance.InstantiateAllCookies();
			
			InitPlayer();
			CameraView.Instance.ResetCamera();

			for (int i = 0; i < _targetCharactersCount; i++) {
				var enemyModel = new Character();

				var newEnemy = PoolService.Instance.PopObject(ENEMY_POOL_KEY) as EnemyView;
				if (newEnemy != null) {
					newEnemy.Init(enemyModel.ID);
					newEnemy.transform.position = CookieSpawner.Instance.GetRandomPositionInLevel();
					newEnemy.FindNewTarget();
					newEnemy.SignalOnCharactersCollided.AddListener(OnCharactersCollided);
				}

				CharactersContainer.Instance.PutCharacter(enemyModel, newEnemy);
			}

			AddListeners();
		}

		private void InitPlayer() {
			_player = new Character();
			_playerView.Init(_player.ID);
			_playerView.gameObject.SetActive(true);
			CharactersContainer.Instance.PutCharacter(_player, _playerView);
			RefreshCharacterWeight(_player.ID, true);
			_playerView.transform.position = _playerStartPosition;
		}

		private void OnCharactersCollided(CharacterView firstView, CharacterView secondView) {
			var first = CharactersContainer.Instance.GetCharacter(firstView.ID);
			var second = CharactersContainer.Instance.GetCharacter(secondView.ID);

			if (first == null) {
				Debug.LogError("first model is missing!");
			} else if (second == null) {
				Debug.LogError("second model is missing!");
			} else {
				if (first.Weight > second.Weight) {
					Punch(second, secondView, firstView, false);
				} else if (first.Weight < second.Weight) {
					Punch(first, firstView, secondView, false);
				} else {
					Punch(first, firstView, secondView, true);
					Punch(second, secondView, firstView, true);
				}
			}
		}

		private void Punch(Character characterToHit, CharacterView characterViewToHit, CharacterView characterThatHits,
			bool isDraw) {
			var damage = characterToHit.Hit(isDraw) * 0.5f;
			characterViewToHit.CookiesDropped();
			for (int i = 0; i < damage; i++) {
				CookieSpawner.Instance.SpawnCookieByHit(characterViewToHit.transform.position);
			}

			if (characterViewToHit is PlayerView) {
				CameraView.Instance.DoCollide(Random.Range(15f, 25f));
				ShowHurtEffect();
			}
			else if (characterThatHits is PlayerView) {
				CameraView.Instance.DoCollide(Random.Range(5f, 10f));
			}
			
			characterViewToHit.SetWeight(characterToHit.Weight);
			characterViewToHit.Punch(characterViewToHit.transform.position - characterThatHits.transform.position);
			RefreshCharacterWeight(characterToHit.ID, false);
		}

		private void ShowHurtEffect() {
			_hurtEffect.gameObject.SetActive(false);
			_hurtEffect.gameObject.SetActive(true);
		}

		public override void Deactivate() {
			_bgMeshRenderer.gameObject.SetActive(false);
			
			CharactersContainer.Instance.Clear();
			CookieSpawner.Instance.Clear();
			UIManager.Instance.HidePanel<HudPanelView>();
			_playerView.EnableCollider();
			_playerView.Stop();
			_playerView.gameObject.SetActive(false);
			
			base.Deactivate();
		}

		private void RefreshCharacterWeight(int id, bool immediately) {
			var character = CharactersContainer.Instance.GetCharacter(id);
			var characterView = CharactersContainer.Instance.GetCharacterView(id);
			if (characterView != null) {
				characterView.SetWeight(character.Weight);

				if (characterView is PlayerView) {
					UIManager.Instance.GetPanel<HudPanelView>().RefreshWeight(character.Weight, immediately);
					CameraView.Instance.SetTargetOrthoAccordingWithWeight(character.Weight);
				}
			}
		}

		protected override void AddListeners() {
			base.AddListeners();

			InputController.Instance.SignalOnTouchStart.AddListener(OnTouchStart);
			InputController.Instance.SignalOnTouchEnd.AddListener(OnTouchEnd);
			InputController.Instance.SignalOnTouch.AddListener(OnTouch);

			var hud = UIManager.Instance.GetPanel<HudPanelView>();
			if (hud != null) {
				hud.SignalOnRestartButtonClicked.AddListener(OnRestartButtonClicked);
				hud.SignalOnGoToLobbyButtonClicked.AddListener(OnGoToLobbyButtonClicked);
			}

			CookieSpawner.Instance.SignalOnCookieEatenByEnemy.AddListener(OnCookieEatenByCharacter);
		}

		protected override void RemoveListeners() {
			InputController.Instance.SignalOnTouchStart.RemoveListener(OnTouchStart);
			InputController.Instance.SignalOnTouchEnd.RemoveListener(OnTouchEnd);
			InputController.Instance.SignalOnTouch.RemoveListener(OnTouch);

			var hud = UIManager.Instance.GetPanel<HudPanelView>();
			if (hud != null && hud.IsSignalsInited) {
				hud.SignalOnRestartButtonClicked.RemoveListener(OnRestartButtonClicked);
				hud.SignalOnGoToLobbyButtonClicked.RemoveListener(OnGoToLobbyButtonClicked);
			}

			CookieSpawner.Instance.SignalOnCookieEatenByEnemy.RemoveListener(OnCookieEatenByCharacter);

			base.RemoveListeners();
		}

		private void OnGoToLobbyButtonClicked() {
			ControllerSwitcher.Instance.SwitchController(ControllerType.Lobby);
		}

		private void OnRestartButtonClicked() {
			ControllerSwitcher.Instance.RestartCurrentController();
		}

		private void OnCookieEatenByCharacter(float cookieWeight, int characterID) {
			var targetCharacter = CharactersContainer.Instance.GetCharacter(characterID);
			if (targetCharacter != null) {
				targetCharacter.EatCookie(cookieWeight);
				RefreshCharacterWeight(characterID, false);
				CookieSpawner.Instance.SpawnNewCookie();
			} else {
				Debug.LogError($"target character model with {characterID} is missing, wtf");
			}
		}

		private void OnTouchStart(Vector2 obj) {
			_playerView.PlayWalkAnimation();
		}

		private void OnTouchEnd(Vector2 obj) {
			_playerView.Stop();
		}

		private void OnTouch(Vector2 obj) {
			_playerView.FaceToTouch();
			_playerView.MoveByControls();

			_bgMeshRenderer.material.mainTextureOffset = _playerView.GetOffsetForBG();
			_bgMeshRenderer.transform.position = new Vector3(
				_playerView.transform.position.x,
				_playerView.transform.position.y,
				_bgMeshRenderer.transform.position.z);
		}

		private void Update() {
			if (CharactersContainer.Instance.CharacterViews != null) {
				for (int i = 0; i < CharactersContainer.Instance.CharacterViews.Count; i++) {
					CharactersContainer.Instance.CharacterViews[i].MoveToTarget();
				}
			}

			if (Input.GetKeyDown(KeyCode.Space)) {
				_player.EatCookie(20);
				RefreshCharacterWeight(_player.ID, false);
			}
		}

		public override ControllerType Type => ControllerType.Game;
	}
}