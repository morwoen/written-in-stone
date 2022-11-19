using System;
using System.Collections.Generic;
using UnityEngine;

namespace CooldownManagement
{
    /// <summary>
    /// Cooldown management class
    /// Originally created for Hanger Management
    /// Improved for Scenery
    /// </summary>
    public class Cooldown
    {
        private static CooldownManager instance;
        private static CooldownManager globalInstance;

        private static CooldownManager Instance {
            get {
                if (!instance) {
                    GameObject go = new GameObject();
                    go.name = "[Cooldowns Manager]";
                    instance = go.AddComponent<CooldownManager>();
                }

                return instance;
            }
        }

        private static CooldownManager GlobalInstance {
            get {
                if (!globalInstance) {
                    GameObject go = new GameObject();
                    go.name = "[Global Cooldowns Manager]";
                    UnityEngine.Object.DontDestroyOnLoad(go);
                    globalInstance = go.AddComponent<CooldownManager>();
                }

                return globalInstance;
            }
        }

        public static Cooldown Wait(float timeInSeconds) {
            return Wait(timeInSeconds, 1);
        }

        public static Cooldown Wait(float timeInSeconds, float timeModifier) {
            Cooldown cooldown = new Cooldown(timeInSeconds, timeModifier);
            Instance.cooldowns.Add(cooldown);
            return cooldown;
        }

        public static Cooldown WaitGlobal(float timeInSeconds) {
            Cooldown cooldown = new Cooldown(timeInSeconds);
            GlobalInstance.cooldowns.Add(cooldown);
            return cooldown;
        }

        protected float time;
        protected float elapsed;
        internal Action action;
        protected Action onStop;
        protected Action<float, float, float> onProgress;
        internal Action always;
        private float timeModifier;

        internal Cooldown(float time, float timeModifier = 1) {
            this.time = time;
            this.elapsed = 0;
            this.timeModifier = timeModifier;
        }

        public Cooldown OnComplete(Action action) {
            this.action = action;
            return this;
        }

        public Cooldown OnProgress(Action<float, float, float> action) {
            this.onProgress = action;
            return this;
        }

        public Cooldown OnProgress(Action<float, float> action) {
            this.onProgress = (elapsed, total, delta) => action(elapsed, total);
            return this;
        }

        public Cooldown OnStop(Action action) {
            this.onStop = action;
            return this;
        }

        public Cooldown Always(Action action) {
            this.always = action;
            return this;
        }

        public void Stop() {
            onStop?.Invoke();
            always?.Invoke();
            action = null;
            always = null; // Making sure Cooldowns doesn't call it a frame later
            time = elapsed;
        }

        internal bool Progress(float delta) {
            elapsed += delta * timeModifier;

            if (elapsed > time) {
                elapsed = time;
            }

            onProgress?.Invoke(elapsed, time, delta);
            return Ongoing();
        }

        internal bool Ongoing() {
            return elapsed < time;
        }

        public static bool operator !(Cooldown cd) {
            return cd == null || cd.elapsed >= cd.time;
        }

        public static implicit operator bool(Cooldown cd) {
            return cd != null && cd.elapsed < cd.time;
        }

        private class CooldownManager : MonoBehaviour
        {
            public List<Cooldown> cooldowns = new();

            private void Update() {
                int i = 0;
                while (i < cooldowns.Count) {
                    if (cooldowns[i].Ongoing() && cooldowns[i].Progress(Time.deltaTime)) {
                        i += 1;
                    } else {
                        cooldowns[i].action?.Invoke();
                        cooldowns[i].always?.Invoke();
                        cooldowns.RemoveAt(i);
                    }
                }
            }
        }
    }
}
