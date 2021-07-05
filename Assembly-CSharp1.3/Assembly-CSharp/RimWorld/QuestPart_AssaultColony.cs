using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B53 RID: 2899
	public class QuestPart_AssaultColony : QuestPart_MakeLord
	{
		// Token: 0x060043DA RID: 17370 RVA: 0x00168F54 File Offset: 0x00167154
		protected override Lord MakeLord()
		{
			LordJob_AssaultColony lordJob = new LordJob_AssaultColony(this.faction ?? this.pawns[0].Faction, true, this.canTimeoutOrFlee, false, false, true, false, false);
			return LordMaker.MakeNewLord(this.faction ?? this.pawns[0].Faction, lordJob, this.mapParent.Map, null);
		}

		// Token: 0x060043DB RID: 17371 RVA: 0x00168FBB File Offset: 0x001671BB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.canTimeoutOrFlee, "canTimeoutOrFlee", true, false);
		}

		// Token: 0x04002928 RID: 10536
		public bool canTimeoutOrFlee = true;
	}
}
