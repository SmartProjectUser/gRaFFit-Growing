namespace gRaFFit.Agar.Utils.Signals {
    /// <summary>
    /// Глобальные сигналы
    /// </summary>
    public class GlobalSignals {
        #region Singleton

        private GlobalSignals() {

        }

        private static GlobalSignals _instance;

        public static GlobalSignals Instance {
            get { return _instance ?? (_instance = new GlobalSignals()); }
        }

        #endregion
    }
}