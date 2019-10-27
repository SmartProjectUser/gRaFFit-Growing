using gRaFFit.Agar.Utils.Signals;
using gRaFFit.Agar.Views.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace gRaFFit.Agar.Views {
	public class CookieView : GameObjectPoolable {
		public Signal<CookieView, int> SignalOnCookieEatenByCharacter;

		public float CookieScale { get; private set; }

		private void OnTriggerEnter2D(Collider2D other) {
			if (other.CompareTag("Enemy") || other.CompareTag("Player")) {
				if (SignalOnCookieEatenByCharacter != null) {
					SignalOnCookieEatenByCharacter.Dispatch(this, other.GetComponent<CharacterView>().ID);
				}
			}
		}

		public override void Instantiate() {
			base.Instantiate();
			SetScale(1f);

			SignalOnCookieEatenByCharacter = new Signal<CookieView, int>();
		}

		public override void Dispose() {
			base.Dispose();

			if (SignalOnCookieEatenByCharacter != null) {
				SignalOnCookieEatenByCharacter.RemoveAllListeners();
				SignalOnCookieEatenByCharacter = null;
			}

			_isPunch = false;
		}

		public void SetScale(float scale) {
			CookieScale = scale;
			transform.localScale = new Vector3(CookieScale, CookieScale, CookieScale);
		}

		private bool _isPunch;
		private Vector2 _punchPosition;

		private const float CookiePunchIntensity = 2f;

		public void Punch() {
			_isPunch = true;
			_punchPosition = transform.position + new Vector3(
				                 Random.Range(-CookiePunchIntensity, CookiePunchIntensity),
				                 Random.Range(-CookiePunchIntensity, CookiePunchIntensity), 0);
		}

		public void Update() {
			if (_isPunch) {
				transform.position = Vector3.Lerp(transform.position, _punchPosition, Time.deltaTime * 10);
				if (Vector3.Distance(transform.position, _punchPosition) < 0.1f) {
					_isPunch = false;
				}
			}
		}
	}
}