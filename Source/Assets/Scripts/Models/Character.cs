namespace Models {
	public class Character {
		private static int _currentFreeID;

		public Character() {
			ID = _currentFreeID;
			_currentFreeID++;
			Weight = 0.5f;
		}

		public int ID { get; private set; }
		public float Weight { get; private set; }
		public const float MaxWeight = 450;

		public void EatCookie(float eatenCookieScale) {
			Weight += eatenCookieScale;
			if (Weight > MaxWeight) {
				Weight = MaxWeight;
			}
		}

		public void ResetWeight() {
			Weight = 0.5f;
		}

		public float Hit(bool isDraw) {
			var damage = 0f;
			
			if (Weight > 0.5f) {
				damage = Weight * (isDraw ? 0.1f : 0.25f);
				Weight -= damage;

				if (Weight < 0.5f) {
					Weight = 0.5f;
				}
			}

			return damage;
		}
	}
}