using System.Collections;
using Boo.Lang;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.Util.Singleton;
using DG.Tweening;
using UnityEngine;

namespace Datenshi.Scripts.Tutorials.Popup {
    public class PopUpTutorialDisplay : Singleton<PopUpTutorialDisplay> {
        private readonly List<PopUpTutorialConfig> activeConfig = new List<PopUpTutorialConfig>();
        public float FadeDuration = 0.25F;
        public float ResizeDuration = .25F;
        public RectTransform Holder;
        public CanvasGroup Group;

        public PopUpTutorialConfig CurrentlyShownConfig {
            get;
            private set;
        }

        private PopUpTutorialContent CurrentContent {
            get;
            set;
        }

        private Coroutine currentRoutine;


        private IEnumerator TransitionTo(PopUpTutorialConfig config) {
            CurrentlyShownConfig = config;
            var oldContent = CurrentContent;
            CurrentContent = config.Prefab.Clone(Holder);
            if (oldContent != null) {
                yield return oldContent.FadeOut(FadeDuration);
                yield return Resize(CurrentContent.Size, FadeDuration);
            } else {
                yield return FadeAndResize(1, CurrentContent.Size, FadeDuration);
            }

            yield return CurrentContent.FadeIn(FadeDuration);
        }

        private IEnumerator Resize(Vector2 currentContentSize, float fadeDuration) {
            Holder.DOKill();
            Holder.DOSizeDelta(currentContentSize, fadeDuration);
            yield return new WaitForSeconds(fadeDuration);
        }

        private IEnumerator FadeAndResize(float alpha, Vector2 currentContentSize, float fadeDuration) {
            Group.DOKill();
            Group.DOFade(alpha, fadeDuration);
            yield return Resize(currentContentSize, fadeDuration);
        }


        private IEnumerator Hide() {
            var old = CurrentContent;
            CurrentContent = null;
            if (old != null) {
                yield return old.FadeOut(FadeDuration);
            }

            yield return FadeAndResize(0, Vector2.zero, FadeDuration);
        }

        public bool IsCurrentlyShowingConfig => CurrentlyShownConfig != null;


        private void OnConfigRemoved(PopUpTutorialConfig config) {
            if (CurrentlyShownConfig == null || CurrentlyShownConfig != config) {
                return;
            }

            CoroutineUtil.ReplaceCoroutine(ref currentRoutine, this, Hide());
        }


        public void AddConfig(PopUpTutorialConfig config) {
            if (activeConfig.Contains(config)) {
                return;
            }

            activeConfig.Add(config);
            OnConfigAdded(config);
        }


        private void OnConfigAdded(PopUpTutorialConfig content) {
            CoroutineUtil.ReplaceCoroutine(ref currentRoutine, this, TransitionTo(content));
        }

        public void RemoveConfig(PopUpTutorialConfig content) {
            if (!activeConfig.Contains(content)) {
                return;
            }


            activeConfig.Remove(content);
            OnConfigRemoved(content);
        }
    }
}