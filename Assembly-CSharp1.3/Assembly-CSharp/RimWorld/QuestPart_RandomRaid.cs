using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BCB RID: 3019
	public class QuestPart_RandomRaid : QuestPart
	{
		// Token: 0x17000C6A RID: 3178
		// (get) Token: 0x060046B4 RID: 18100 RVA: 0x00175F0E File Offset: 0x0017410E
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

		// Token: 0x17000C6B RID: 3179
		// (get) Token: 0x060046B5 RID: 18101 RVA: 0x00175F1E File Offset: 0x0017411E
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

		// Token: 0x060046B6 RID: 18102 RVA: 0x00175F30 File Offset: 0x00174130
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.mapParent != null && this.mapParent.HasMap)
			{
				IncidentParms incidentParms = new IncidentParms();
				incidentParms.forced = true;
				incidentParms.quest = this.quest;
				incidentParms.target = this.mapParent.Map;
				incidentParms.points = (this.useCurrentThreatPoints ? (StorytellerUtility.DefaultThreatPointsNow(this.mapParent.Map) * this.currentThreatPointsFactor) : this.pointsRange.RandomInRange);
				incidentParms.faction = this.faction;
				incidentParms.customLetterLabel = signal.args.GetFormattedText(this.customLetterLabel);
				incidentParms.customLetterText = signal.args.GetFormattedText(this.customLetterText);
				incidentParms.attackTargets = this.attackTargets;
				incidentParms.generateFightersOnly = this.generateFightersOnly;
				incidentParms.sendLetter = this.sendLetter;
				if (this.arrivalMode != null)
				{
					incidentParms.raidArrivalMode = this.arrivalMode;
				}
				IncidentDef incidentDef;
				if (this.faction == null || this.faction.HostileTo(Faction.OfPlayer))
				{
					incidentDef = IncidentDefOf.RaidEnemy;
				}
				else
				{
					incidentDef = IncidentDefOf.RaidFriendly;
				}
				if (this.raidStrategy != null)
				{
					incidentParms.raidStrategy = this.raidStrategy;
				}
				if (this.faction != null)
				{
					incidentParms.points = Mathf.Max(incidentParms.points, this.faction.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null));
				}
				if (incidentDef.Worker.CanFireNow(incidentParms))
				{
					incidentDef.Worker.TryExecute(incidentParms);
				}
			}
		}

		// Token: 0x060046B7 RID: 18103 RVA: 0x001760E0 File Offset: 0x001742E0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<FloatRange>(ref this.pointsRange, "pointsRange", default(FloatRange), false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<bool>(ref this.useCurrentThreatPoints, "useCurrentThreatPoints", false, false);
			Scribe_Values.Look<float>(ref this.currentThreatPointsFactor, "currentThreatPointsFactor", 1f, false);
			Scribe_Defs.Look<PawnsArrivalModeDef>(ref this.arrivalMode, "arrivalMode");
			Scribe_Values.Look<string>(ref this.customLetterLabel, "customLetterLabel", null, false);
			Scribe_Values.Look<string>(ref this.customLetterText, "customLetterText", null, false);
			Scribe_Defs.Look<RaidStrategyDef>(ref this.raidStrategy, "raidStrategy");
			Scribe_Collections.Look<Thing>(ref this.attackTargets, "attackTargets", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.generateFightersOnly, "generateFightersOnly", false, false);
			Scribe_Values.Look<bool>(ref this.sendLetter, "sendLetter", true, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.attackTargets != null)
			{
				this.attackTargets.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x060046B8 RID: 18104 RVA: 0x00176224 File Offset: 0x00174424
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

		// Token: 0x04002B26 RID: 11046
		public string inSignal;

		// Token: 0x04002B27 RID: 11047
		public MapParent mapParent;

		// Token: 0x04002B28 RID: 11048
		public FloatRange pointsRange;

		// Token: 0x04002B29 RID: 11049
		public Faction faction;

		// Token: 0x04002B2A RID: 11050
		public bool useCurrentThreatPoints;

		// Token: 0x04002B2B RID: 11051
		public float currentThreatPointsFactor = 1f;

		// Token: 0x04002B2C RID: 11052
		public PawnsArrivalModeDef arrivalMode;

		// Token: 0x04002B2D RID: 11053
		public RaidStrategyDef raidStrategy;

		// Token: 0x04002B2E RID: 11054
		public string customLetterLabel;

		// Token: 0x04002B2F RID: 11055
		public string customLetterText;

		// Token: 0x04002B30 RID: 11056
		public List<Thing> attackTargets;

		// Token: 0x04002B31 RID: 11057
		public bool generateFightersOnly;

		// Token: 0x04002B32 RID: 11058
		public bool sendLetter = true;
	}
}
