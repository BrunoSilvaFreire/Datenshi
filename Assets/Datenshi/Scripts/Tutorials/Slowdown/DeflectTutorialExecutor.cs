using Datenshi.Scripts.Combat.Game.Ranged;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Movement;
using Datenshi.Scripts.Util.Services;
using Datenshi.Scripts.Util.Time;
using UnityEngine;

namespace Datenshi.Scripts.Tutorials.Slowdown {
    public class DeflectTutorialExecutor : SlowdownTutorialExecutor {
        public LivingEntity CombatantToDeflect;
        public AnimationCurve TimeScaleCurve;
        //private IndefiniteService<BlackAndWhiteMeta> bawService;

        public override void Init(IndefiniteService<TimeMeta> meta) {
            //bawService = GraphicsSingleton.Instance.BlackAndWhite.RequestIndefiniteService(1, 1);
        }


        public override void Tick(IndefiniteService<TimeMeta> service) {
            var entity = PlayerController.Instance.CurrentEntity;
            if (entity == null) {
                return;
            }

            var closestProjectile = Projectile.GetClosestFromTo(CombatantToDeflect, entity);
            float scale;
            if (closestProjectile == null) {
                scale = 1;
            } else {
                var distance = entity.DistanceTo(closestProjectile);
                scale = TimeScaleCurve.Evaluate(distance);
            }
            //bawService.Metadata.DesaturateAmount = 1 - scale;
            service.Metadata.Scale = scale;
        }
    }
}