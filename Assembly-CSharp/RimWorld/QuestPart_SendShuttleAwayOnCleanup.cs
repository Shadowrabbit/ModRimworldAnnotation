using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200110F RID: 4367
	public class QuestPart_SendShuttleAwayOnCleanup : QuestPart
	{
		// Token: 0x17000ED5 RID: 3797
		// (get) Token: 0x06005F63 RID: 24419 RVA: 0x00042014 File Offset: 0x00040214
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				yield return this.shuttle;
				yield break;
			}
		}

		// Token: 0x06005F64 RID: 24420 RVA: 0x00042024 File Offset: 0x00040224
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.shuttle != null)
			{
				SendShuttleAwayQuestPartUtility.SendAway(this.shuttle, this.dropEverything);
			}
		}

		// Token: 0x06005F65 RID: 24421 RVA: 0x00042045 File Offset: 0x00040245
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Values.Look<bool>(ref this.dropEverything, "dropEverything", false, false);
		}

		// Token: 0x06005F66 RID: 24422 RVA: 0x00042070 File Offset: 0x00040270
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.shuttle != null)
			{
				this.shuttle.TryGetComp<CompShuttle>().requiredPawns.Replace(replace, with);
			}
		}

		// Token: 0x04003FC9 RID: 16329
		public Thing shuttle;

		// Token: 0x04003FCA RID: 16330
		public bool dropEverything;
	}
}
