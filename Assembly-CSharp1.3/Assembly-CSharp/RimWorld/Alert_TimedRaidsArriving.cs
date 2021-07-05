using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001286 RID: 4742
	public class Alert_TimedRaidsArriving : Alert
	{
		// Token: 0x170013C5 RID: 5061
		// (get) Token: 0x0600714A RID: 29002 RVA: 0x0025C1A2 File Offset: 0x0025A3A2
		private List<GlobalTargetInfo> Targets
		{
			get
			{
				this.CalculateTargets();
				return this.targets;
			}
		}

		// Token: 0x0600714B RID: 29003 RVA: 0x0025C1B0 File Offset: 0x0025A3B0
		private void CalculateTargets()
		{
			this.timedRaidsArrivingSoon.Clear();
			this.targets.Clear();
			foreach (WorldObject worldObject in Find.WorldObjects.AllWorldObjects)
			{
				using (List<WorldObjectComp>.Enumerator enumerator2 = worldObject.AllComps.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						TimedDetectionRaids timedDetectionRaids;
						if ((timedDetectionRaids = (enumerator2.Current as TimedDetectionRaids)) != null && this.ShouldAlertTimedRaids(timedDetectionRaids))
						{
							this.timedRaidsArrivingSoon.Add(timedDetectionRaids);
							if (!this.targets.Contains(worldObject))
							{
								this.targets.Add(worldObject);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600714C RID: 29004 RVA: 0x0025C298 File Offset: 0x0025A498
		private bool ShouldAlertTimedRaids(TimedDetectionRaids timedDetectionRaids)
		{
			return timedDetectionRaids.alertRaidsArrivingIn && timedDetectionRaids.DetectionCountdownStarted && timedDetectionRaids.RaidsSentCount == 0;
		}

		// Token: 0x0600714D RID: 29005 RVA: 0x0025C2B8 File Offset: 0x0025A4B8
		public override string GetLabel()
		{
			return "AlertTimedRaidsArrivingIn".Translate(this.timedRaidsArrivingSoon.MinBy((TimedDetectionRaids t) => t.TicksLeftToSendRaids).TicksLeftToSendRaids.ToStringTicksToPeriod(true, false, true, true));
		}

		// Token: 0x0600714E RID: 29006 RVA: 0x0025C314 File Offset: 0x0025A514
		public override TaggedString GetExplanation()
		{
			return "AlertTimedRaidsArrivingInDesc".Translate((from t in this.timedRaidsArrivingSoon
			select "AlertTimedRaidsArrivingAt".Translate(t.parent.Label, t.TicksLeftToSendRaids.ToStringTicksToPeriod(true, false, true, true)).Resolve()).ToLineList("- ", false));
		}

		// Token: 0x0600714F RID: 29007 RVA: 0x0025C365 File Offset: 0x0025A565
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x04003E4E RID: 15950
		private List<TimedDetectionRaids> timedRaidsArrivingSoon = new List<TimedDetectionRaids>();

		// Token: 0x04003E4F RID: 15951
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
