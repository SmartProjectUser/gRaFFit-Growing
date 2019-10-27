namespace gRaFFit.Agar.Views.UIPanelSystem {

    /// <summary>
    /// Вспомогательная панель, затемняющая фон и блокирующая ввод
    /// </summary>
    public class BlackBGPanelView : UIPanelView {

        #region Public Methods

        /// <summary>
        /// Установка позиции сортировки для конкретной панели
        /// </summary>
        /// <param name="targetPanel">Целевая панель</param>
        public void SetBehindPanel(UIPanelView targetPanel) {
            var panelSiblingIndex = targetPanel.transform.GetSiblingIndex();
            transform.SetParent(targetPanel.transform.parent);
            if (panelSiblingIndex == 0) {
                transform.SetAsFirstSibling();
            } else {
                transform.SetSiblingIndex(panelSiblingIndex - 1);
            }
        }

        #endregion
    }
}