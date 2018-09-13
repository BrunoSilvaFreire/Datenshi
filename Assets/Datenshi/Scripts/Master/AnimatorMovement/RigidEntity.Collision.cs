using System.Collections.Generic;
using System.Linq;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;
using UPM;

namespace Datenshi.Scripts.Master.AnimatorMovement {
    public partial class RigidEntity {
        [ShowInInspector]
        public CollisionStatus CollisionStatus {
            get;
            private set;
        }

        [ReadOnly, ShowInInspector]
        private readonly List<ContactPoint2D> currentContacts = new List<ContactPoint2D>();

        internal void UpdateCollisionStatus() {
           // Debug.Log("Updating collision status");
            var status = CollisionStatus;
            status.Down = currentContacts.Any(DownCollision);
            status.Up = currentContacts.Any(UpCollision);
            status.Left = currentContacts.Any(LeftCollosion);
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

        private static bool LeftCollosion(ContactPoint2D arg) {
            return arg.normal.x > 0;
        }

        private static bool UpCollision(ContactPoint2D arg) {
            return arg.normal.y < 0;
        }

        private static bool DownCollision(ContactPoint2D arg) {
            return arg.normal.y > 0;
        }


        private void OnCollisionExit2D(Collision2D other) {
            UpdateCollisionStatus();
            //Debug.Log("Collision exit");
            foreach (var contact in other.contacts) {
                currentContacts.RemoveAll(delegate(ContactPoint2D d) {
                        var r = Mathf.Approximately(d.point.x, contact.point.x) &&
                                Mathf.Approximately(d.point.y, contact.point.y);
                        if (r) {
                            //Debug.Log($"Removing collision @ {d.point}, {d.collider}");
                        }

                        return r;
                    }
/*&&
                                               Mathf.Approximately(d.normal.x, contact.normal.x) &&
                                               Mathf.Approximately(d.normal.y, contact.normal.y)*/
                );
            }
        }

        private void OnCollisionEnter2D(Collision2D other) {
            UpdateCollisionStatus();
            //Debug.Log("Collision enter");
            foreach (var contact in other.contacts) {
                //Debug.Log($"Adding collision @ {contact.point}, {contact.collider}");
                currentContacts.Add(contact);
            }
        }

        private void OnCollisionStay2D(Collision2D other) {
            //Debug.Log("Collision Stay");
            foreach (var contact in other.contacts) {
                //Debug.Log($"Adding collision @ {contact.point}, {contact.collider}");
                currentContacts.Add(contact);
            }
        }
    }
}