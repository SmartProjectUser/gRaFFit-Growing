using Controllers;
using gRaFFit.Agar.Controllers.InputSystem;
using Models;
using UnityEngine;

namespace gRaFFit.Agar.Views {
	public class PlayerView : CharacterView {

		private Vector3 _startPosition;

		public void Awake() {
			_startPosition = transform.position;
		}

		public void FaceToTouch() {
			FaceToPosition(InputController.Instance.GetTouchWorldPosition());
		}

		public Vector3 GetOffsetForBG() {
			return (_startPosition - transform.position) * 2;
		}

		public Vector2 GetTouchNormalizedOffset() {
			return (InputController.Instance.GetTouchWorldPosition() - (Vector2) transform.position).normalized;
		}

		public void MoveByControls() {
			var model = CharactersContainer.Instance.GetCharacter(ID);
			var normalizedWeight = model.Weight / Character.MaxWeight;

			var antiWeight = (1f - normalizedWeight);
			if (antiWeight <= 0.5f) {
				antiWeight = 0.5f;
			}

			if (_collider2D.enabled) {
				_rigidbody2D.velocity = antiWeight * _moveSpeed * GetTouchNormalizedOffset();
			} else {
				_rigidbody2D.velocity = Vector3.Lerp(_rigidbody2D.velocity, Vector3.zero, 1f * Time.deltaTime);
			}
		}
	}
}