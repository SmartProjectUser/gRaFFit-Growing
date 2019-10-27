using System;
using System.Collections.Generic;
using UnityEngine;

namespace gRaFFit.Agar.Views.UIPanelSystem {
    
    /// <summary>
    /// Менеджер UI панелей
    /// </summary>
    public class UIManager : MonoBehaviour {
        #region Singleton

        private UIManager() {

        }

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(Instance.gameObject);
            }

            Instance = this;
        }

        /// <summary>
        /// Инстанс менеджера панелей
        /// </summary>
        public static UIManager Instance { get; private set; }

        #endregion

        #region Layout

        /// <summary>
        /// Список всех панелей, которые возможно вызвать через UIManager
        /// </summary>
        [SerializeField] private List<UIPanelView> _uiPanels = new List<UIPanelView>();

        #endregion

        #region Private Fields

        /// <summary>
        /// Текущие открытые панели
        /// </summary>
        private List<OpenedPanel> _currentOpenedBlackBGPanels = new List<OpenedPanel>();

        /// <summary>
        /// Список, с типами панелей, в порядке таком, какой должен быть у панелей, для правильной сортировки панелей
        /// после загрузки из бандлов
        /// </summary>
        private readonly List<Type> _overlayCanvasPanelTypesOrderList = new List<Type> {

        };

        #endregion

        #region Public Methods

        /// <summary>
        /// Находит и возвращает панель необходимого типа
        /// </summary>
        /// <typeparam name="T">Конкретный тип панели-наследника UIPanelView</typeparam>
        /// <returns>Панель</returns>
        public T GetPanel<T>() where T : UIPanelView {
            T resultPanel = null;
            var count = Instance._uiPanels.Count;
            for (int i = 0; i < count; i++) {
                var panel = Instance._uiPanels[i] as T;
                if (panel != null) {
                    resultPanel = panel;
                    break;
                }
            }

            return resultPanel;
        }

        /// <summary>
        /// Отображает панель неоходимого типа, и на всякий случай возвращает её
        /// </summary>
        /// <typeparam name="T">Конкретный тип панели-наследника UIPanelView</typeparam>
        /// <returns>Панель, которая была отображена</returns>
        public T ShowPanel<T>() where T : UIPanelView {
            var panel = GetPanel<T>();
            panel.Show();
            return panel;
        }

        /// <summary>
        /// Прячет панель неоходимого типа
        /// </summary>
        /// <param name="withoutEffects">Указать true если нужно скрыть панель моментально, без анимации</param>
        /// <typeparam name="T">Конкретный тип панели-наследника UIPanelView</typeparam>
        public void HidePanel<T>(bool withoutEffects = false) where T : UIPanelView {
            var panel = GetPanel<T>();
            // панель ещё может быть не добавлена в менеджер - в таком случае просто ничего не прячем, нам это не надо,
            // т.к. эта панель еще ниразу не отобразилась
            if (panel != null) {
                if (withoutEffects) {
                    panel.HideWithoutEffects();
                } else {
                    panel.Hide();
                }
            }
        }

        /// <summary>
        /// Вызывается когда было установлено затемнение
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="isLeavesActive">Активны-ли листья?</param>
        public void SetBlackBGToPanel(UIPanelView panel, bool isLeavesActive = true) {
            var blackBGPanel = GetPanel<BlackBGPanelView>();
            if (_currentOpenedBlackBGPanels.Count == 0) {
                blackBGPanel.Show();
            }

            blackBGPanel.SetBehindPanel(panel);

            if (!_currentOpenedBlackBGPanels.Exists(openedPanel => openedPanel.PanelView == panel)) {
                _currentOpenedBlackBGPanels.Add(new OpenedPanel(panel, isLeavesActive));
                panel.SignalOnHideAnimationStarted.AddListener(OnSomeBlackBGPanelHide);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Вызывается когда какая-то панель с затемненным фоном была спрятана
        /// </summary>
        /// <param name="panel">Панель</param>
        private void OnSomeBlackBGPanelHide(UIPanelView panel) {
            panel.SignalOnHideAnimationStarted.RemoveListener(OnSomeBlackBGPanelHide);

            var blackBGPanel = GetPanel<BlackBGPanelView>();

            // забираем со списка последнюю открытую с черным фоном панель
            var topPanel = _currentOpenedBlackBGPanels[_currentOpenedBlackBGPanels.Count - 1];
            var itemToRemove = _currentOpenedBlackBGPanels.Find(item => item.PanelView == panel);
            _currentOpenedBlackBGPanels.Remove(itemToRemove);

            // если панель была последней - скрываем затемняющую панель
            if (_currentOpenedBlackBGPanels.Count == 0) {
                blackBGPanel.Hide();
            } else {
                // если панелей больше чем одна, и та, которая только что закрылась - была топовой
                if (topPanel.PanelView == panel) {
                    // устанавливаем фон для следующей по порядку панели
                    topPanel = _currentOpenedBlackBGPanels[_currentOpenedBlackBGPanels.Count - 1];
                    blackBGPanel.SetBehindPanel(topPanel.PanelView);
                }

                // если же закрылась не топовая панель - всё ок, из списка мы её удалили, затемнение модифицировать не надо
            }
        }

        /// <summary>
        /// Обновляет sibling index панели blackBG 
        /// </summary>
        private void RefreshBlackBgSiblingIndex() {
            if (_currentOpenedBlackBGPanels.Count > 0) {
                GetPanel<BlackBGPanelView>().SetBehindPanel(
                    _currentOpenedBlackBGPanels[_currentOpenedBlackBGPanels.Count - 1]
                        .PanelView);
            }
        }

        #endregion
    }
}