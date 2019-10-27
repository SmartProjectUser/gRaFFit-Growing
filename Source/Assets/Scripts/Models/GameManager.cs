namespace gRaFFit.Agar.Models {
	public class GameManager {
		#region Singleton

		private GameManager() {

		}

		private static GameManager _instance;

		public static GameManager Instance {
			get { return _instance ?? (_instance = new GameManager()); }
		}

		#endregion

		public bool IsGamePaused { get; set; }
	}
}