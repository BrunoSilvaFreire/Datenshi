using System.Collections.Generic;
using UnityEngine;

namespace Datenshi.Scripts.Misc.Ghosting {
    /// <summary>
    /// container for ghosting sprites. triggers a new ghosting object over a set amount of time, referencing a sprite renderer's current sprite
    /// combined, this forms a trailing effect behind the player 
    /// </summary>
    public class GhostingContainer : MonoBehaviour {
        private List<GhostingSprite> inactiveGhostingSpritesPool; //a list of sprites stored in memory
//
        //  [SerializeField]
        // private int _spacing;

        [SerializeField]
        private float spawnRate;

        private float nextSpawnTime;

        [SerializeField]
        private int trailLength = 1; //effect duration

        // private int _frameCount;
        [SerializeField]
        private int sortingLayer;

        [SerializeField]
        private float desiredAlpha = 0.8f;

        private Queue<GhostingSprite> ghostingSpritesQueue;

        public bool HasStarted;

        //private float _triggerTimer = 0.2f; // how often to trigger a ghosting image?
        [SerializeField]
        private float effectDuration = 1f;

        private SpriteRenderer refSpriteRenderer; //sprite renderer to reference

        //public Shader GhostShader;
        [SerializeField]
        private Color desiredColor;

        private GameObject ghostSpritesParent;
        private bool useTint;

        /// <summary>
        /// Average ms per frame
        /// </summary>

        public List<GhostingSprite> InactiveGhostSpritePool {
            get {
                if (inactiveGhostingSpritesPool == null) {
                    inactiveGhostingSpritesPool = new List<GhostingSprite>(5);
                }
                return inactiveGhostingSpritesPool;
            }
            set {
                inactiveGhostingSpritesPool = value;
            }
        }

        public Queue<GhostingSprite> GhostingSpritesQueue {
            get {
                if (ghostingSpritesQueue == null) {
                    ghostingSpritesQueue = new Queue<GhostingSprite>(trailLength);
                }
                return ghostingSpritesQueue;
            } //idito
            set {
                ghostingSpritesQueue = value;
            }
        }

        public GameObject GhostSpritesParent {
            get {
                if (ghostSpritesParent == null) {
                    ghostSpritesParent = new GameObject();
                    ghostSpritesParent.transform.position = Vector3.zero;
                    ghostSpritesParent.name = "GhostspriteParent";
                }
                return ghostSpritesParent;
            }
            set {
                ghostSpritesParent = value;
            }
        }


        /// <summary>
        /// Initializes and starts the ghosting effect with the given params but with an option to tint. Please note that the effect duration no longer has an effect on the object in question.  
        /// </summary>
        /// <param name="maxNumberOfGhosts"></param>
        /// <param name="spawnRate"></param>
        /// <param name="refSpriteRenderer"></param>
        /// <param name="effectDuration"></param>
        /// <param name="desiredColor"></param>
        public void Init(int maxNumberOfGhosts, float spawnRate, SpriteRenderer refSpriteRenderer, float effectDuration, Color desiredColor) {
            this.desiredColor = desiredColor;
            trailLength = maxNumberOfGhosts;
            // _spacing = spacing;
            this.spawnRate = spawnRate;
            //_effectDuration = effectDuration;
            this.effectDuration = maxNumberOfGhosts * spawnRate * 0.95f; //5% variance
            this.refSpriteRenderer = refSpriteRenderer;

            nextSpawnTime = Time.time + this.spawnRate;
            sortingLayer = this.refSpriteRenderer.sortingLayerID;
            HasStarted = true;
            useTint = true;
        }

        /// <summary>
        /// Initializes and starts the ghosting effect with the given params. Please note that the effect duration no longer has an effect on the object in question.
        /// </summary>
        /// <param name="maxNumberOfGhosts"></param>
        /// <param name="spawnRate"></param>
        /// <param name="refSpriteRenderer"></param>
        /// <param name="effectDuration"></param>
        public void Init(int maxNumberOfGhosts, float spawnRate, SpriteRenderer refSpriteRenderer, float effectDuration) {
            trailLength = maxNumberOfGhosts;
            this.spawnRate = spawnRate;
            //	_effectDuration = effectDuration;
            this.effectDuration = maxNumberOfGhosts * spawnRate * 0.95f; //5% variance
            this.refSpriteRenderer = refSpriteRenderer;
            sortingLayer = this.refSpriteRenderer.sortingLayerID;
            nextSpawnTime = Time.time + this.spawnRate;
            useTint = false;
            HasStarted = true;
        }

        /// <summary>
        /// Stop the ghosting effect
        /// </summary>
        public void StopEffect() {
            HasStarted = false;
        }

        void Update() {
            if (HasStarted) {
                //check for spawn rate
                //check if we're ok to spawn a new ghost
                if (Time.time >= nextSpawnTime) {
                    //is the queue count number equal than the trail length? 
                    if (GhostingSpritesQueue.Count == trailLength) {
                        var peekedGhostingSprite = GhostingSpritesQueue.Peek();
                        //is it ok to use? 
                        var canBeReused = peekedGhostingSprite.CanBeReused();
                        if (canBeReused) {
                            //pop the queue
                            GhostingSpritesQueue.Dequeue();
                            GhostingSpritesQueue.Enqueue(peekedGhostingSprite);

                            //initialize the ghosting sprite
                            if (!useTint) {
                                peekedGhostingSprite.Init(effectDuration, desiredAlpha, refSpriteRenderer.sprite, sortingLayer, refSpriteRenderer.sortingOrder - 1, transform, Vector3.zero);
                            } else {
                                peekedGhostingSprite.Init(effectDuration, desiredAlpha, refSpriteRenderer.sprite, sortingLayer, refSpriteRenderer.sortingOrder - 1, transform, Vector3.zero, desiredColor);
                            }
                            nextSpawnTime += spawnRate;
                        } else //not ok, wait until next frame to try again
                        {
                            //peekedGhostingSprite.KillAnimationAndSpeedUpDissapearing();
                            return;
                        }
                    }
                    //check if the count is less than the trail length, we need to create a new ghosting sprite
                    if (GhostingSpritesQueue.Count < trailLength) {
                        var newGhostingSprite = Get();
                        GhostingSpritesQueue.Enqueue(newGhostingSprite); //queue it up!
                        //newGhostingSprite.Init(_effectDuration, _desiredAlpha, _refSpriteRenderer.sprite, _sortingLayer,_refSpriteRenderer.sortingOrder-1, transform, Vector3.zero );

                        if (!useTint) {
                            newGhostingSprite.Init(effectDuration, desiredAlpha, refSpriteRenderer.sprite, sortingLayer, refSpriteRenderer.sortingOrder - 1, transform, Vector3.zero);
                        } else {
                            newGhostingSprite.Init(effectDuration, desiredAlpha, refSpriteRenderer.sprite, sortingLayer, refSpriteRenderer.sortingOrder - 1, transform, Vector3.zero, desiredColor);
                        }
                        nextSpawnTime += spawnRate;
                    }
                    //check if the queue count is greater than the trail length. Dequeue these items off the queue, as they are no longer needed
                    if (GhostingSpritesQueue.Count > trailLength) {
                        var difference = GhostingSpritesQueue.Count - trailLength;
                        for (var i = 1; i < difference; i++) {
                            var gs = GhostingSpritesQueue.Dequeue();
                            InactiveGhostSpritePool.Add(gs);
                        }
                        return;
                    }
                }
            }
        }


        /// <summary>
        /// Returns a ghosting sprite 
        /// </summary>
        /// <returns></returns>
        private GhostingSprite Get() {
            for (var i = 0; i < InactiveGhostSpritePool.Count; i++) {
                if (InactiveGhostSpritePool[i].CanBeReused()) {
                    return InactiveGhostSpritePool[i];
                }
            }
            return BuildNewGhostingSprite();
        }

        private GhostingSprite BuildNewGhostingSprite() {
            //create a gameobject and set the current transform as a parent
            var go = new GameObject();
            go.transform.position = transform.position;
            go.transform.parent = GhostSpritesParent.transform;

            var gs = go.AddComponent<GhostingSprite>();

            return gs;
        }
    }
}