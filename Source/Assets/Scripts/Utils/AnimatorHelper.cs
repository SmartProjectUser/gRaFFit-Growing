using UnityEngine;

namespace gRaFFit.Agar.Utils {
    /// <summary>
    /// Помощник работы с анимациями
    /// </summary>
    public class AnimatorHelper {
        #region Singleton

        private AnimatorHelper() {

        }

        private static AnimatorHelper _instance;

        public static AnimatorHelper Instance {
            get { return _instance ?? (_instance = new AnimatorHelper()); }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Запускает анимацию у переданного аниматора с переданным тригером, и возвращает время её отработки, или 0f,
        /// если такого анимационного клипа с таким триггером не найдено
        /// </summary>
        /// <param name="targetAnimator"></param>
        /// <param name="triggerKey"></param>
        /// <returns>Время анимации в секундах. Если анимации с желаемым триггером нет в аниматоре - значение будет 0f</returns>
        public float PlayAnimation(Animator targetAnimator, string triggerKey) {
            var animationTime = 0f;

            if (targetAnimator != null && targetAnimator.gameObject.activeInHierarchy) {
                var clip = GetAnimationClip(targetAnimator, triggerKey);
                if (clip != null) {
                    targetAnimator.SetTrigger(Animator.StringToHash(triggerKey));
                    animationTime = clip.length;
                }
            }

            return animationTime;
        }

        /// <summary>
        /// Возвращает время анимации
        /// </summary>
        /// <param name="targetAnimator">Аниматор, в котором находится анимация</param>
        /// <param name="triggerKey">Ключ к анимации</param>
        /// <returns>Время анимации, или 0f, если клипа с заданным именем в анимации нет</returns>
        public float GetAnimationTime(Animator targetAnimator, string triggerKey) {
            var result = 0f;
            var clip = GetAnimationClip(targetAnimator, triggerKey);
            if (clip != null) {
                result = clip.length;
            }

            return result;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Получение клипа анимации по аниматору и триггеру
        /// </summary>
        /// <param name="targetAnimator">Целевой аниматор</param>
        /// <param name="triggerKey">Ключ к анимации</param>
        /// <returns>Клип анимации, или null, если такого клипа в аниматоре нет</returns>
        private AnimationClip GetAnimationClip(Animator targetAnimator, string triggerKey) {
            AnimationClip result = null;

            var clips = targetAnimator.runtimeAnimatorController.animationClips;
            var clipsCount = clips.Length;
            for (int i = 0; i < clipsCount; i++) {
                var clip = clips[i];
                if (clip.name == triggerKey) {
                    result = clip;
                    break;
                }
            }

            return result;
        }

        #endregion
    }
}