using Controllers;
using gRaFFit.Agar.Models.Timers;
using Models;
using UnityEngine;

namespace gRaFFit.Agar.Views {

	public class EnemyView : CharacterView {
		[SerializeField] private float _attackDistance;
		
		private CookieView _targetCookie;
		private CharacterView _targetCharacter;

		public override void MoveToTarget() {
			if (_collider2D.enabled) {
				var model = CharactersContainer.Instance.GetCharacter(ID);

				var normalizedWeight = model.Weight / Character.MaxWeight;

				var antiWeight = (1f - normalizedWeight);
				if (antiWeight <= 0.5f) {
					antiWeight = 0.5f;
				}

				if (_targetCharacter != null) {
					_rigidbody2D.velocity =
						antiWeight *
						_moveSpeed * ((Vector2) _targetCharacter.transform.position - (Vector2) transform.position)
						.normalized;

					FaceToPosition(_targetCharacter.transform.position);
				} else if (_targetCookie != null) {
					_rigidbody2D.velocity =
						antiWeight *
						_moveSpeed * ((Vector2) _targetCookie.transform.position - (Vector2) transform.position)
						.normalized;

					FaceToPosition(_targetCookie.transform.position);
				} else {
					_rigidbody2D.velocity = Vector3.Lerp(_rigidbody2D.velocity, Vector3.zero, 1f * Time.deltaTime);
				}
			}
		}

		private bool FindNearestCharacter() {
			CharacterView nearestCharacterView = null;
			float distanceToNearestCharacter = 0;

			var isFound = false;

			for (int i = 0; i < CharactersContainer.Instance.CharacterViews.Count; i++) {
				var currentCharacter = CharactersContainer.Instance.CharacterViews[i];
				if (currentCharacter.ID == ID) continue;

				if (nearestCharacterView == null) {
					nearestCharacterView = currentCharacter;
					distanceToNearestCharacter =
						Vector2.Distance(currentCharacter.transform.position, transform.position);
				} else {
					var distanceToCurrentCharacter =
						Vector2.Distance(currentCharacter.transform.position, transform.position);
					if (distanceToCurrentCharacter < distanceToNearestCharacter) {
						distanceToNearestCharacter = distanceToCurrentCharacter;
						nearestCharacterView = currentCharacter;
					}
				}
			}

			if (nearestCharacterView != null && !nearestCharacterView.IsStunned) {
				var nearestCharacter = CharactersContainer.Instance.GetCharacter(nearestCharacterView.ID);
				var meCharacterView = CharactersContainer.Instance.GetCharacterView(ID);
				var meCharacter = CharactersContainer.Instance.GetCharacter(ID);

				if (nearestCharacterView != null && nearestCharacter != null &&
				    meCharacterView != null && meCharacter != null) {
					var distance = Vector2.Distance(nearestCharacterView.transform.position, transform.position);
					if (distance < _attackDistance) {
						if (nearestCharacter.Weight < meCharacter.Weight) {
							isFound = true;
							_targetCharacter = nearestCharacterView;
							_targetCharacter.SignalOnCharactersCollided.AddListener(OnMyTargetCharacterCollided);
							PlayWalkAnimation();
						}
					}
				}
			}

			return isFound;
		}

		private void FindNearestCookie() {
			CookieView foundCookie = null;
			float distanceToNearestCookie = 0;

			if (CookieSpawner.Instance.Cookies != null) {
				for (int i = 0; i < CookieSpawner.Instance.Cookies.Count; i++) {
					var currentCookie = CookieSpawner.Instance.Cookies[i];
					if (foundCookie == null) {
						foundCookie = currentCookie;
						distanceToNearestCookie =
							Vector2.Distance(currentCookie.transform.position, transform.position);
					} else {
						var distanceToCurrentCookie =
							Vector2.Distance(currentCookie.transform.position, transform.position);
						if (distanceToCurrentCookie < distanceToNearestCookie) {
							distanceToNearestCookie = distanceToCurrentCookie;
							foundCookie = currentCookie;
						}
					}
				}
			}

			if (foundCookie != null) {
				_targetCookie = foundCookie;
				PlayWalkAnimation();
				foundCookie.SignalOnCookieEatenByCharacter.AddListener(OnMyCookieEaten);
			} else {
				Stop();
				Debug.LogError("Can't find cookies :(");
			}
		}

		private Timer _recheckEnemyTimer;

		public void FindNewTarget() {
			if (_targetCookie != null) {
				if (_targetCookie.SignalOnCookieEatenByCharacter != null) {
					_targetCookie.SignalOnCookieEatenByCharacter.RemoveListener(OnMyCookieEaten);
				}

				_targetCookie = null;
			}
			if (_targetCharacter != null) {
				if (_targetCharacter.SignalOnCharactersCollided != null) {
					_targetCharacter.SignalOnCharactersCollided.RemoveListener(OnMyTargetCharacterCollided);
				}

				_targetCharacter = null;
			}

			if (!FindNearestCharacter()) {
				FindNearestCookie();
			} else {
				if (_recheckEnemyTimer != null) {
					_recheckEnemyTimer.Stop();
				}

				_recheckEnemyTimer = TimerManager.Instance.SetTimeout(1f, FindNewTarget);
			}
		}

		public override void Dispose() {
			if (_recheckEnemyTimer != null) {
				_recheckEnemyTimer.Stop();
			}

			base.Dispose();
		}

		private void OnMyCookieEaten(CookieView cookie, int enemyID) {
			FindNewTarget();
		}

		private void OnMyTargetCharacterCollided(CharacterView arg1, CharacterView arg2) {
			FindNewTarget();
		}
		
		public override void EnableCollider() {
			FindNewTarget();
			
			base.EnableCollider();
		}
	}
}