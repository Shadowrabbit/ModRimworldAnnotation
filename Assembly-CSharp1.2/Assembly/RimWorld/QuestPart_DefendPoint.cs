using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200108D RID: 4237
	public class QuestPart_DefendPoint : QuestPart_MakeLord
	{
		// Token: 0x06005C50 RID: 23632 RVA: 0x0004008E File Offset: 0x0003E28E
		protected override Lord MakeLord()
		{
			return LordMaker.MakeNewLord(this.faction, new LordJob_DefendPoint(this.point, this.wanderRadius, this.isCaravanSendable, this.addFleeToil), base.Map, null);
		}

		// Token: 0x06005C51 RID: 23633 RVA: 0x001DA294 File Offset: 0x001D8494
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.point, "point", default(IntVec3), false);
			Scribe_Values.Look<float?>(ref this.wanderRadius, "wanderRadius", null, false);
			Scribe_Values.Look<bool>(ref this.isCaravanSendable, "isCaravanSendable", false, false);
			Scribe_Values.Look<bool>(ref this.addFleeToil, "addFleeToil", false, false);
		}

		// Token: 0x04003DD2 RID: 15826
		public IntVec3 point;

		// Token: 0x04003DD3 RID: 15827
		public float? wanderRadius;

		// Token: 0x04003DD4 RID: 15828
		public bool isCaravanSendable;

		// Token: 0x04003DD5 RID: 15829
		public bool addFleeToil = true;
	}
}
