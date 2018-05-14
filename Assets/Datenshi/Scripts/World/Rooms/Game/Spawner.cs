﻿using System;
using System.Collections;
using System.Linq;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Graphics;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.World.Rooms.Doors;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Datenshi.Scripts.World.Rooms.Game {
    public class Spawner : AbstractRoomMember {
        public enum SpawnMode {
            Random,
            PredefinedLocation
        }

        public SpawnMode Mode;
        public Wave Wave;

        private void Start() {
            Room.OnObjectEnter.AddListener(OnEnter);
        }

        private void OnEnter(Collider2D arg0) {
            if (started) {
                return;
            }

            var e = arg0.GetComponentInParent<Entity>();
            if (e == PlayerController.Instance.CurrentEntity) {
                started = true;
                Begin();
            }
        }

        private bool started;

        [ShowIf(nameof(IsPredefinedLocation))]
        public Vector2[] Locations = new Vector2[0];

        public bool IsRandom => Mode == SpawnMode.Random;
        public bool IsPredefinedLocation => Mode == SpawnMode.PredefinedLocation;

        [ShowInInspector, ReadOnly]
        private sbyte currentGroup;

        [ShowInInspector, ReadOnly]
        private float lastTimeMark;

        [ShowInInspector, ReadOnly]
        private bool playing;

        [ShowInInspector, ReadOnly]
        private bool countdownStarted;

        public Door ToOpen;
        public bool PlayWarning;
        public byte SyrenCounts;
        public AudioClip SyrenClip;
        public AudioSource Source;
        public Color SyrenColor;
        public float SyrenColorAmount = .5F;

        private void PlaySyren() {
            StartCoroutine(DoSyren());
        }

        private IEnumerator DoSyren() {
            var clipLength = SyrenClip.length;
            var clipLengthHalf = clipLength / 2;
            var fx = GraphicsSingleton.Instance.OverrideColor;
            fx.Color = SyrenColor;
            for (byte i = 0; i < SyrenCounts; i++) {
                Source.PlayOneShot(SyrenClip);
                fx.DOAmount(SyrenColorAmount, clipLengthHalf);
                yield return new WaitForSeconds(clipLengthHalf);
                fx.DOAmount(0, clipLengthHalf);
                yield return new WaitForSeconds(clipLengthHalf);
            }

            DoBegin();
        }

        private void Update() {
            if (!playing) {
                return;
            }

            if (currentGroup >= Wave.TotalGroups - 1 && !CurrentGroup.HasAnyAlive) {
                playing = false;
                OnWaveCompleted.Invoke();
                if (ToOpen) {
                    ToOpen.Open();
                }

                return;
            }

            if (CurrentGroup.IsTimeBased) {
                if (CurrentGroup.HasDelayPassed(lastTimeMark)) {
                    Spawn();
                }

                return;
            }

            if (CurrentGroup.IsWaitForPrevious && currentGroup >= 0) {
                if (!countdownStarted) {
                    if (CurrentGroup.HasAnyAlive) {
                        return;
                    }

                    countdownStarted = true;
                    lastTimeMark = Time.time;
                }

                if (!CurrentGroup.HasDelayPassed(lastTimeMark)) {
                    return;
                }

                Spawn();
                countdownStarted = false;
            }
        }

        public void Begin() {
            if (PlayWarning) {
                PlaySyren();
            } else {
                DoBegin();
            }
        }

        private void DoBegin() {
            Debug.Log("Begining");
            playing = true;
            currentGroup = -1;
            Spawn();
        }

        public void Spawn() {
            Debug.Log("Spawning");
            currentGroup++;
            CurrentGroup.Spawn(this);
            lastTimeMark = Time.time;
            if (currentGroup < Wave.TotalGroups - 1 && NextGroup.IsSimultaneous) {
                Spawn();
            }
        }

        public WaveGroup CurrentGroup => Wave.Groups[currentGroup];
        public WaveGroup NextGroup => Wave.Groups[currentGroup + 1];
        public WaveGroup LastGroup => Wave.Groups[currentGroup - 1];

        public UnityEvent OnWaveCompleted;

        public Vector2 GetSpawnLocation() {
            switch (Mode) {
                case SpawnMode.Random:
                    return GetRandomSpawnLocation();
                case SpawnMode.PredefinedLocation:
                    return GetRandomPredefinedLocation();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDrawGizmos() {
            if (!IsRandom) {
                return;
            }


            foreach (var location in Locations) {
                GizmosUtil.DrawBox2DWire(location, Vector2.one, Color.red);
            }
        }

        private Vector2 GetRandomPredefinedLocation() {
            return Locations.RandomElement();
        }

        private Vector2 GetRandomSpawnLocation() {
            var w = Room.Width;
            var h = Room.Height;
            var xIncrement = (Random.value * w) - w / 2;
            var yIncrement = Random.value * h - h / 2;
            var pos = (Vector2) Room.transform.position + Room.Area.offset;
            pos.x += xIncrement;
            pos.y += yIncrement;
            return pos;
        }
    }

    [Serializable]
    public class Wave {
        public WaveGroup[] Groups;

        public int TotalGroups => Groups.Length;
    }

    [Serializable]
    public class WaveGroup {
        public enum WaveSpawnMode {
            WaitForPrevious,
            TimeBased,
            Simultaneous
        }

        public WaveSpawnMode Mode;

        public GameObject Prefab;

        public bool IsTimeBased => Mode == WaveSpawnMode.TimeBased;
        public bool IsWaitForPrevious => Mode == WaveSpawnMode.WaitForPrevious;
        public bool IsSimultaneous => Mode == WaveSpawnMode.Simultaneous;

        public byte Count = 3;

        [ShowInInspector, ReadOnly]
        public GameObject[] Spawned {
            get;
            private set;
        }

        public bool HasAnyAlive {
            get {
                if (Spawned == null) {
                    return true;
                }

                var combatants = from o in Spawned where o != null select o.GetComponent<ICombatant>();
                return combatants.Any(c => !c.Dead);
            }
        }

        [HideIf(nameof(IsSimultaneous))]
        public float SecondsDelay;

        public void Spawn(Spawner spawner) {
            Spawned = new GameObject[Count];
            for (byte i = 0; i < Count; i++) {
                var loc = spawner.GetSpawnLocation();
                var obj = Prefab.Clone(loc);
                var c = obj.GetComponentInChildren<ICombatant>();
                c?.AnimatorUpdater.TriggerSpawn();
                Spawned[i] = obj;
            }
        }

        public bool HasDelayPassed(float lastTimeMark) {
            return Time.time - lastTimeMark > SecondsDelay;
        }
    }
}