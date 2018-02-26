using System.Collections;
using Datenshi.Scripts.UI.Narrator;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;
using Shiroi.Cutscenes.Util;
using UnityEngine;

namespace Datenshi.Scripts.Cutscenes.Tokens {
/*    public class CreateNarratorToken : IToken {
        public Narrator Prefab;
        public Reference<RectTransform> Parent;

        [EmptyStringSupported]
        public string Text;

        public Rect Rect;
        public Vector2 AnchorMin;
        public Vector2 AnchorMax;
        public Vector2 Pivot;

        public IEnumerator Execute(CutscenePlayer player) {
            var obj = Object.Instantiate(Prefab);
            obj.transform.SetParent(Parent.Resolve(player));
            var rectTransform = (RectTransform) obj.transform;
            rectTransform.anchorMin = AnchorMin;
            rectTransform.anchorMax = AnchorMax;
            rectTransform.pivot = Pivot;
            rectTransform.sizeDelta = Rect.size;
            rectTransform.anchoredPosition = Rect.position;
            yield break;
        }
    }*/
}