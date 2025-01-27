﻿using System;
using System.Linq;
using RaidBuffTracker.Tracker.Library;
using RaidBuffTracker.Utils;
using StatusInstance = Dalamud.Game.ClientState.Statuses.Status;

namespace RaidBuffTracker.Tracker.Track
{
    public sealed class ActionTrack
    {
        public readonly ActionTrackSource   source;
        public readonly ActionLibraryRecord record;

        public double lastInvoked;
        public double lastActivated;

        public bool   IsActive          => DurationRemaining > 0;
        public bool   IsReady           => CooldownRemaining < 0;
        public double DurationRemaining => lastActivated + record.duration - DateTime.Now.TimestampSeconds();
        public double CooldownRemaining => lastInvoked + record.cooldown - DateTime.Now.TimestampSeconds();

        public ActionTrack(ActionTrackSource? source, ActionLibraryRecord @record)
        {
            if (source != null)
                this.source = (ActionTrackSource) source;
            this.record = record;
        }

        public void UpdateWithInvocation(uint actionId)
        {
            if (record.cooldownActionIds.Contains(actionId))
            {
                lastInvoked = DateTime.Now.TimestampSeconds() + 0.55;
            }

            if (record.effectActionIds.Contains(actionId))
            {
                lastActivated = DateTime.Now.TimestampSeconds() + 0.55;
            }
        }

        public override string ToString()
        {
            return $"ActionTrack ({source.index}/{source.name} - {record.name})";
        }
    }
}