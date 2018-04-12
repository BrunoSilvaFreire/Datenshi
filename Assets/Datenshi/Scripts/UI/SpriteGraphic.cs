﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Datenshi.Scripts.UI {
    public class SpriteGraphic : UIBehaviour, ILayoutElement {
        public SpriteRenderer Renderer;
        public int LayoutPriority;
        private CanvasGroup g;

        public void CalculateLayoutInputHorizontal() { }

        public void CalculateLayoutInputVertical() { }

        public T Fetch<T>(Func<Sprite, T> func) {
            if (Renderer == null) {
                return default(T);
            }
            var sprite = Renderer.sprite;
            return sprite == null ? default(T) : func(sprite);
        }

        public float minWidth {
            get {
                return Fetch(sprite => sprite.rect.width);
            }
        }

        public float preferredWidth {
            get {
                return minWidth;
            }
        }

        public float flexibleWidth {
            get {
                return 0;
            }
        }

        public float minHeight {
            get {
                return Fetch(sprite => sprite.rect.height);
            }
        }

        public float preferredHeight {
            get {
                return minHeight;
            }
        }

        public float flexibleHeight {
            get {
                return 0;
            }
        }

        public int layoutPriority {
            get {
                return LayoutPriority;
            }
        }
    }
}