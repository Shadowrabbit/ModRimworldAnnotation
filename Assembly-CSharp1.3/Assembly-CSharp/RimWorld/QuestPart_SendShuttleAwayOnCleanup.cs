using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BA5 RID: 2981
	public class QuestPart_SendShuttleAwayOnCleanup : QuestPart
	{
		// Token: 0x17000C27 RID: 3111
		// (get) Token: 0x06004591 RID: 17809 RVA: 0x00170B4E File Offset: 0x0016ED4E
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				yield return this.shuttle;
				yield break;
			}
		}

		// Token: 0x06004592 RID: 17810 RVA: 0x00170B5E File Offset: 0x0016ED5E
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.shuttle != null)
			{
				SendShuttleAwayQuestPartUtility.SendAway(this.shuttle, this.dropEverything);
			}
		}

		// Token: 0x06004593 RID: 17811 RVA: 0x00170B7F File Offset: 0x0016ED7F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Values.Look<bool>(ref this.dropEverything, "dropEverything", false, false);
		}

		// Token: 0x06004594 RID: 17812 RVA: 0x00170BAA File Offset: 0x0016EDAA
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.shuttle != null)
			{
				this.shuttle.TryGetComp<CompShuttle>().requiredPawns.Replace(replace, with);
			}
		}

		// Token: 0x04002A5A RID: 10842
		public Thing shuttle;

		// Token: 0x04002A5B RID: 10843
		public bool dropEverything;
	}
}
