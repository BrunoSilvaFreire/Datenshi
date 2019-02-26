using System.Collections;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Util;
using DG.Tweening;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;
using UnityEngine.UI;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class SpawnTextToken : Token {
        public Text TextPrefab;
        public float Fade = 1;
        public float Stay = 5;

        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            var prefab = Instantiate(TextPrefab, PlayerController.Instance.Canvas.transform);
            prefab.SetAlpha(0);
            prefab.DOFade(1, Fade);
            yield return new WaitForSeconds(Fade);
            yield return new WaitForSeconds(Stay);
            prefab.DOFade(0, Fade);
            yield return new WaitForSeconds(Fade);
        }
    }
}