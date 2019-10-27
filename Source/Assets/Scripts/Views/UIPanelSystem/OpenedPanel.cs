namespace gRaFFit.Agar.Views.UIPanelSystem {
	
	/// <summary>
	/// Инфа об открытой менеджером панели
	/// </summary>
	public struct OpenedPanel {
		/// <summary>
		/// Вьюшка панели
		/// </summary>
		public readonly UIPanelView PanelView;

		public OpenedPanel(UIPanelView panelView, bool isLeavesActive) {
			PanelView = panelView;
		}
	}
}