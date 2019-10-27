using System.Collections.Generic;
using gRaFFit.Agar.Utils.Signals;
using gRaFFit.Agar.Views.Pool;
using UnityEngine;

namespace gRaFFit.Agar.Views {
	public class CookieSpawner : MonoBehaviour {
		#region MonoSingleton

		private CookieSpawner() {
		}

		private void Awake() {
			if (Instance != null && Instance != this) {
				Debug.LogError($"Multiple instances of {GetType()} on scene found, fix this!!!");
				Destroy(Instance);
			}
		}

		public static CookieSpawner Instance {
			get {
				if (_instance == null) {
					_instance = FindObjectOfType<CookieSpawner>();
				}

				return _instance;
			}
		}

		private static CookieSpawner _instance;

		#endregion
		
		[SerializeField] private Transform _spawnMinPoint;
		[SerializeField] private Transform _spawnMaxPoint;
		[SerializeField] private CookieView _cookiePrefab;
		[SerializeField] private int _targetCookiesCount;

		[SerializeField] private float _minCookieScale;
		[SerializeField] private float _maxCookieScale;
		
		
		
		private List<CookieView> _currentCookiesInstances;
		public Signal<float, int> SignalOnCookieEatenByEnemy = new Signal<float, int>();

		public List<CookieView> Cookies => _currentCookiesInstances;
		
		public const string COOKIE_POOL_ID = "COOKIE_POOL_ID";

		public void Init() {
			PoolService.Instance.InitPoolWithNewObject(COOKIE_POOL_ID, _cookiePrefab);
		}

		public void InstantiateAllCookies() {
			Clear();
			_currentCookiesInstances = new List<CookieView>();
			
			for (int i = 0; i < _targetCookiesCount; i++) {
				SpawnNewCookie();
			}
		}

		public Vector3 GetRandomPositionInLevel() {
			return new Vector3(
				Random.Range(_spawnMinPoint.position.x, _spawnMaxPoint.position.x),
				Random.Range(_spawnMinPoint.position.y, _spawnMaxPoint.position.y));
		}

		public void SpawnNewCookie() {
			if (_currentCookiesInstances.Count < _targetCookiesCount) {
				var newCookie = PoolService.Instance.PopObject(COOKIE_POOL_ID, transform) as CookieView;
				if (newCookie != null) {
					newCookie.transform.position = GetRandomPositionInLevel();
					newCookie.SetScale(Random.Range(_minCookieScale, _maxCookieScale));
					newCookie.SignalOnCookieEatenByCharacter.AddListener(OnCookieEatenByEnemy);
					_currentCookiesInstances.Add(newCookie);
				} else {
					Debug.LogError("ERROR SPAWNING COOKIE");
				}
			}
		}

		public void SpawnCookieByHit(Vector3 targetPosition) {
			var newCookie = PoolService.Instance.PopObject(COOKIE_POOL_ID, transform) as CookieView;
			if (newCookie != null) {
				newCookie.transform.position = targetPosition;
				newCookie.SetScale(Random.Range(_minCookieScale, _maxCookieScale));
				newCookie.SignalOnCookieEatenByCharacter.AddListener(OnCookieEatenByEnemy);
				_currentCookiesInstances.Add(newCookie);
				newCookie.Punch();
			} else {
				Debug.LogError("ERROR SPAWNING COOKIE");
			}
		}

		private void OnCookieEatenByEnemy(CookieView cookieView, int enemyID) {
			_currentCookiesInstances.Remove(cookieView);

			var scale = cookieView.CookieScale;
			cookieView.Dispose();
			SignalOnCookieEatenByEnemy.Dispatch(scale, enemyID);
		}

		public void Clear() {
			if (_currentCookiesInstances != null) {
				for (int i = 0; i < _currentCookiesInstances.Count; i++) {
					_currentCookiesInstances[i].Dispose();
				}

				_currentCookiesInstances.Clear();
				_currentCookiesInstances = null;
			}
		}
	}
}