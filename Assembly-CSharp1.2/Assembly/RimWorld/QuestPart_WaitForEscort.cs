using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001093 RID: 4243
	public class QuestPart_WaitForEscort : QuestPart_MakeLord
	{
		// Token: 0x06005C6E RID: 23662 RVA: 0x00040257 File Offset: 0x0003E457
		protected override Lord MakeLord()
		{
			return LordMaker.MakeNewLord(this.faction, new LordJob_WaitForEscort(this.point, this.addFleeToil), base.Map, null);
		}

		// Token: 0x06005C6F RID: 23663 RVA: 0x001DA6E8 File Offset: 0x001D88E8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.point, "point", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.addFleeToil, "addFleeToil", false, false);
		}

		// Token: 0x04003DE9 RID: 15849
		public IntVec3 point;

		// Token: 0x04003DEA RID: 15850
		public bool addFleeToil = true;
	}
}
