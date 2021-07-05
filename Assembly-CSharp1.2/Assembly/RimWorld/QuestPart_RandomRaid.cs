using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001156 RID: 4438
	public class QuestPart_RandomRaid : QuestPart
	{
		// Token: 0x17000F48 RID: 3912
		// (get) Token: 0x06006176 RID: 24950 RVA: 0x000431D9 File Offset: 0x000413D9
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.mapParent != null)
				{
					yield return this.mapParent;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000F49 RID: 3913
		// (get) Token: 0x06006177 RID: 24951 RVA: 0x000431E9 File Offset: 0x000413E9
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in base.InvolvedFactions)
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.faction != null)
				{
					yield return this.faction;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06006178 RID: 24952 RVA: 0x001E7FF8 File Offset: 0x001E61F8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.mapParent != null && this.mapParent.HasMap)
			{
				IncidentParms incidentParms = new IncidentParms();
				incidentParms.forced = true;
				incidentParms.quest = this.quest;
				incidentParms.target = this.mapParent.Map;
				incidentParms.points = (this.useCurrentThreatPoints ? StorytellerUtility.DefaultThreatPointsNow(this.mapParent.Map) : this.pointsRange.RandomInRange);
				incidentParms.faction = this.faction;
				IncidentDef incidentDef;
				if (this.faction == null || this.faction.HostileTo(Faction.OfPlayer))
				{
					incidentDef = IncidentDefOf.RaidEnemy;
				}
				else
				{
					incidentDef = IncidentDefOf.RaidFriendly;
				}
				if (incidentDef.Worker.CanFireNow(incidentParms, true))
				{
					incidentDef.Worker.TryExecute(incidentParms);
				}
			}
		}

		// Token: 0x06006179 RID: 24953 RVA: 0x001E80E4 File Offset: 0x001E62E4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<FloatRange>(ref this.pointsRange, "pointsRange", default(FloatRange), false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<bool>(ref this.useCurrentThreatPoints, "useCurrentThreatPoints", false, false);
		}

		// Token: 0x0600617A RID: 24954 RVA: 0x001E8158 File Offset: 0x001E6358
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			if (Find.AnyPlayerHomeMap != null)
			{
				this.mapParent = Find.RandomPlayerHomeMap.Parent;
				this.pointsRange = new FloatRange(500f, 1500f);
			}
		}

		// Token: 0x04004122 RID: 16674
		public string inSignal;

		// Token: 0x04004123 RID: 16675
		public MapParent mapParent;

		// Token: 0x04004124 RID: 16676
		public FloatRange pointsRange;

		// Token: 0x04004125 RID: 16677
		public Faction faction;

		// Token: 0x04004126 RID: 16678
		public bool useCurrentThreatPoints;
	}
}
