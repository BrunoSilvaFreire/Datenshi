using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Util.Services {
    public abstract class ServiceHandler : ITickable {
        public static Service<T> WithHighestPriority<T>(params ServiceHandler<T>[] handlers) where T : IComparable<T> {
            Service<T> highest = null;
            foreach (var handler in handlers) {
                var candidate = handler.WithGenericHighestPriority();
                if (highest == null || candidate.Priority > highest.Priority) {
                    highest = candidate;
                }
            }

            return highest;
        }

        public static Service WithHighestPriority(params ServiceHandler[] handlers) {
            Service highest = null;
            foreach (var handler in handlers) {
                var candidate = handler.WithHighestPriority();
                Debug.Log("Candidate of " + handler + " = " + highest + " vs " + highest);
                if (highest == null || candidate.Priority > highest.Priority) {
                    highest = candidate;
                }
            }

            return highest;
        }

        public abstract Service WithHighestPriority();

        public abstract void Tick();
    }

    public class ServiceHandler<T> : ServiceHandler where T : IComparable<T> {
        private static readonly Predicate<Service<T>> FinishedServiceChecker = service => service.IsFinished();

        [ShowInInspector]
        private readonly List<Service<T>> activeServices = new List<Service<T>>();

        public override void Tick() {
            foreach (var service in activeServices) {
                service.Tick();
            }

            activeServices.RemoveAll(FinishedServiceChecker);
        }

        public override Service WithHighestPriority() {
            return WithGenericHighestPriority();
        }

        public Service<T> WithGenericHighestPriority() {
            Service<T> highest = null;
            byte current = 0;
            foreach (var service in activeServices) {
                if (service.Priority < current) {
                    continue;
                }

                highest = service;
                current = service.Priority;
            }

            return highest;
        }

        public void RegisterService(Service<T> service) {
            activeServices.Add(service);
            service.Init();
        }

        public IndefiniteService<T> RegisterIndefiniteService(T meta, byte priority) {
            var service = new IndefiniteService<T>(meta, priority);
            RegisterService(service);
            return service;
        }

        public TimedService<T> RegisterTimedService(T meta, float duration, byte priority) {
            var service = new TimedService<T>(meta, priority, duration);
            RegisterService(service);
            return service;
        }
    }
}