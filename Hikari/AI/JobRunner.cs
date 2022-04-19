using Unity.Jobs;
using UnityEngine;

namespace Hikari.AI {
    public struct JobRunner {
        private JobHandle handle;
        public bool Scheduled { get; private set; }

        public delegate void Schedular(ref JobHandle handle);

        public void ScheduleNext(Schedular schedular) {
            if (Scheduled) {
                Debug.LogWarning("Job is not completed");
            }

            handle = default;
            schedular(ref handle);
            JobHandle.ScheduleBatchedJobs();
            Scheduled = true;
        }

        public void Complete() {
            if (!Scheduled) {
                Debug.LogError("Job is not scheduled");
                return;
            }

            handle.Complete();
            handle = default;
            Scheduled = false;
        }
    }
}