using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200110C RID: 4364
	public class QuestPart_SendShuttleAway : QuestPart
	{
		// Token: 0x17000ED2 RID: 3794
		// (get) Token: 0x06005F54 RID: 24404 RVA: 0x00041F39 File Offset: 0x00040139
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				yield return this.shuttle;
				yield break;
			}
		}

		// Token: 0x06005F55 RID: 24405 RVA: 0x00041F49 File Offset: 0x00040149
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.shuttle != null)
			{
				SendShuttleAwayQuestPartUtility.SendAway(this.shuttle, this.dropEverything);
			}
		}

		// Token: 0x06005F56 RID: 24406 RVA: 0x00041F7E File Offset: 0x0004017E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Values.Look<bool>(ref this.dropEverything, "dropEverything", false, false);
		}

		// Token: 0x06005F57 RID: 24407 RVA: 0x001E20A0 File Offset: 0x001E02A0
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			if (Find.AnyPlayerHomeMap != null)
			{
				Map randomPlayerHomeMap = Find.RandomPlayerHomeMap;
				IntVec3 center = DropCellFinder.RandomDropSpot(randomPlayerHomeMap);
				this.shuttle = ThingMaker.MakeThing(ThingDefOf.Shuttle, null);
				GenPlace.TryPlaceThing(SkyfallerMaker.MakeSkyfaller(ThingDefOf.ShuttleIncoming, this.shuttle), center, randomPlayerHomeMap, ThingPlaceMode.Near, null, null, default(Rot4));
			}
		}

		// Token: 0x06005F58 RID: 24408 RVA: 0x00041FBB File Offset: 0x000401BB
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.shuttle != null)
			{
				this.shuttle.TryGetComp<CompShuttle>().requiredPawns.Replace(replace, with);
			}
		}

		// Token: 0x04003FC2 RID: 16322
		public string inSignal;

		// Token: 0x04003FC3 RID: 16323
		public Thing shuttle;

		// Token: 0x04003FC4 RID: 16324
		public bool dropEverything;
	}
}
