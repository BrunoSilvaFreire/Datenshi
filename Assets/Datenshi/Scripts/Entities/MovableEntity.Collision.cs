using System.Collections.Generic;
using System.Linq;
using Datenshi.Scripts.Movement;
using Datenshi.Scripts.Movement.Config;
using Datenshi.Scripts.Util.Volatiles;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityUtilities;

namespace Datenshi.Scripts.Entities {
    public partial class MovableEntity {
        [ShowInInspector]
        public CollisionStatus CollisionStatus {
            get;
            set;
        }


        [ReadOnly, ShowInInspector]
        private readonly List<ContactPoint2D> currentContacts = new List<ContactPoint2D>();


        private void UpdateCollisionStatus() {
            var status = CollisionStatus;
            status.Down = currentContacts.Any(DownCollision);
            status.Up = currentContacts.Any(UpCollision);
            status.Left = currentContacts.Any(LeftCollision);
            status.Right = currentContacts.Any(RightCollision);
            CollisionStatus = status;
            foreach (var point in currentContacts) {
                DebugUtil.DrawWireCircle2D(point.point, 0.1F, Color.red);
                Debug.DrawRay(point.point, point.normal, Color.red);
            }

            currentContacts.Clear();
        }

        private static bool RightCollision(ContactPoint2D arg) {
            return arg.normal.x < 0;
        }

        private static bool LeftCollision(ContactPoint2D arg) {
            return arg.normal.x > 0;
        }

        private static bool UpCollision(ContactPoint2D arg) {
            return arg.normal.y < 0;
        }

        private static bool DownCollision(ContactPoint2D arg) {
            return arg.normal.y > 0;
        }


        private void OnCollisionExit2D(Collision2D other) {
            foreach (var contact in other.contacts) {
                currentContacts.RemoveAll(d => Mathf.Approximately(d.point.x, contact.point.x) &&
                                               Mathf.Approximately(d.point.y, contact.point.y)
                );
            }
        }

        private void OnCollisionEnter2D(Collision2D other) {
            foreach (var contact in other.contacts) {
                //Debug.Log($"Adding collision @ {contact.point}, {contact.collider}");
                currentContacts.Add(contact);
            }
        }

        private void OnCollisionStay2D(Collision2D other) {
            foreach (var contact in other.contacts) {
                //Debug.Log($"Adding collision @ {contact.point}, {contact.collider}");
                currentContacts.Add(contact);
            }
        }
    }
}