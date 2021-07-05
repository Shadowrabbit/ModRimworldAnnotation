using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BA3 RID: 2979
	public class QuestPart_SendShuttleAway : QuestPart
	{
		// Token: 0x17000C26 RID: 3110
		// (get) Token: 0x0600458A RID: 17802 RVA: 0x001709CB File Offset: 0x0016EBCB
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				yield return this.shuttle;
				yield break;
			}
		}

		// Token: 0x0600458B RID: 17803 RVA: 0x001709DB File Offset: 0x0016EBDB
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.shuttle != null)
			{
				SendShuttleAwayQuestPartUtility.SendAway(this.shuttle, this.dropEverything);
			}
		}

		// Token: 0x0600458C RID: 17804 RVA: 0x00170A10 File Offset: 0x0016EC10
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Values.Look<bool>(ref this.dropEverything, "dropEverything", false, false);
		}

		// Token: 0x0600458D RID: 17805 RVA: 0x00170A50 File Offset: 0x0016EC50
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			if (Find.AnyPlayerHomeMap != null)
			{
				Map randomPlayerHomeMap = Find.RandomPlayerHomeMap;
				IntVec3 center = DropCellFinder.RandomDropSpot(randomPlayerHomeMap, true);
				this.shuttle = ThingMaker.MakeThing(ThingDefOf.Shuttle, null);
				GenPlace.TryPlaceThing(SkyfallerMaker.MakeSkyfaller(ThingDefOf.ShuttleIncoming, this.shuttle), center, randomPlayerHomeMap, ThingPlaceMode.Near, null, null, default(Rot4));
			}
		}

		// Token: 0x0600458E RID: 17806 RVA: 0x00170AC7 File Offset: 0x0016ECC7
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.shuttle != null)
			{
				this.shuttle.TryGetComp<CompShuttle>().requiredPawns.Replace(replace, with);
			}
		}

		// Token: 0x04002A57 RID: 10839
		public string inSignal;

		// Token: 0x04002A58 RID: 10840
		public Thing shuttle;

		// Token: 0x04002A59 RID: 10841
		public bool dropEverything;
	}
}
