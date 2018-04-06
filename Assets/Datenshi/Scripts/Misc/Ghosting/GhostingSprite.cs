using System.Collections;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Misc.Ghosting {
    /// <summary>
    /// This class sets a sprites's alpha to zero over a period of time. 
    /// </summary>
    public class GhostingSprite : MonoBehaviour {
        private const string GhostMaterialPath = "Art/Materials/GhostMaterial";
        private Material material;
        private float dissapearTimer;
        private Sprite sprite;
        private SpriteRenderer renderer;
        private float startingAlpha;
        private Vector3 offset;
        private Vector3 originalScaling;
        private Vector3 originalPosition;

        private bool hasBeenInitiated;
        private bool canBeReused;
        private IEnumerator startDissapearing; //reference to the routine in case it needs to be stopped
        private bool useTint;

        public Material GhostMaterial {
            get {
                if (material == null) {
                    var m = UnityEngine.Resources.Load<Material>(GhostMaterialPath);
                    material = new Material(m);
                    SpriteRenderer.material = material;
                }
                return material;
            }
        }

        private SpriteRenderer SpriteRenderer {
            get {
                if (renderer == null) {
                    renderer = this.GetOrAddComponent<SpriteRenderer>();
                }
                return renderer;
            }
        }

        public void Init(float dissapearTimer, float startingAlpha, Sprite sprite, int sortingId, int sortingOrder, Transform referencedTransform, Vector3 offset) {
            this.startingAlpha = startingAlpha;
            var color = SpriteRenderer.color;
            color.a = this.startingAlpha;
            SpriteRenderer.color = color;

            this.dissapearTimer = dissapearTimer;
            SpriteRenderer.sortingLayerID = sortingId;
            SpriteRenderer.sortingOrder = sortingOrder;
            this.sprite = sprite;
            SpriteRenderer.sprite = sprite;

            this.offset = offset;
            originalPosition = referencedTransform.position;
            transform.position = originalPosition;
            /*_originalScaling = Vector3.one;
            _originalScaling.x = Mathf.Sign(referencedTransform.localScale.x);*/
            originalScaling = referencedTransform.localScale;
            transform.localScale = originalScaling;
            hasBeenInitiated = true;

            gameObject.SetActive(true);
            useTint = false;
            startDissapearing = StartDissapearing(useTint);
            StartCoroutine(startDissapearing);
        }

        public void Init(
            float dissapearTimer,
            float startingAlpha,
            Sprite sprite,
            int sortingId,
            int sortingOrder,
            Transform referencedTransform,
            Vector3 offset,
            Color desiredColor) {
            startingAlpha = startingAlpha;
            /*    Color color = SpriteRenderer.color;
                color.a = _startingAlpha;
                SpriteRenderer.color = color;*/
            DesiredColor = desiredColor;
            DesiredColor.a = this.startingAlpha;
            GhostMaterial.SetColor("_Color", DesiredColor);

            SpriteRenderer.color = desiredColor;
            this.dissapearTimer = dissapearTimer;
            SpriteRenderer.sortingLayerID = sortingId;
            SpriteRenderer.sortingOrder = sortingOrder;
            this.sprite = sprite;
            SpriteRenderer.sprite = sprite;

            this.offset = offset;
            originalPosition = referencedTransform.position;
            transform.position = originalPosition;

            originalScaling = referencedTransform.localScale;
            transform.localScale = originalScaling;
            hasBeenInitiated = true;

            gameObject.SetActive(true);
            useTint = true;
            startDissapearing = StartDissapearing(useTint);
            StartCoroutine(startDissapearing);
        }

        public Color DesiredColor;

        public bool CanBeReused() {
            return canBeReused;
        }

        private void Update() {
            if (hasBeenInitiated) {
                transform.position = originalPosition + offset; //this prevents it from moving with its parent
                //GhostMaterial.SetColor("_Color", _desiredColor);
                transform.localScale = originalScaling;
            }
        }


        /// <summary>
        /// From the class field, dissapear timer, slowly disspear the sprite renderer . Pass in a bool to use the provided ghost shader which 
        /// behaves 
        /// </summary>
        /// <returns></returns>
        private IEnumerator StartDissapearing(bool changeColor) {
            var finishedLerping = false;
            var startLerpTime = Time.time;
            var color = SpriteRenderer.color;
            var blackWithZeroAlpha = Color.black;
            var startColor = GhostMaterial.color;
            blackWithZeroAlpha.a = 0;
            if (!changeColor) {
                GhostMaterial.shader = Shader.Find("Sprites/Default");
            } else {
                GhostMaterial.shader = Shader.Find("Spine/SkeletonGhost");
                GhostMaterial.SetFloat("_TextureFade", 0.25f);
            }
            while (!finishedLerping) {
                var timeSinceLerpStart = Time.time - startLerpTime;
                var percentComplete = timeSinceLerpStart / dissapearTimer;
                if (percentComplete >= 1) {
                    finishedLerping = true;
                    if (!changeColor) {
                        color.a = 0;
                        GhostMaterial.color = color;
                    } else {
                        GhostMaterial.SetColor("_Color", blackWithZeroAlpha);
                    }
                }
                if (!changeColor) {
                    var newAlphaValue = Mathf.Lerp(startingAlpha, 0, percentComplete);
                    color.a = newAlphaValue;
                    GhostMaterial.color = color;
                } else {
                    var newColor = Color.Lerp(startColor, blackWithZeroAlpha, percentComplete);
                    GhostMaterial.SetColor("_Color", newColor);
                }

                yield return null;
            }

            canBeReused = true;
            gameObject.SetActive(false);
            hasBeenInitiated = false;
        }

//		public void KillAnimationAndSpeedUpDissapearing()
//		{
//			StopCoroutine (_startDissapearing);
//			if (!_useTint)
//			{
//				Color color =  SpriteRenderer.color; 
//				color.a = 0;
//				GhostMaterial.color = color;  
//			}
//			else
//			{ 
//				Color newColor = Color.Lerp(startColor,blackWithZeroAlpha,percentComplete);
//				GhostMaterial.SetColor("_Color", newColor); 
//            }
//            
//            _dissapearTimer /= 4f;
//            _startDissapearing = StartDissapearing (_useTint);
//            StartCoroutine(_startDissapearing);
//    	}
    }
}