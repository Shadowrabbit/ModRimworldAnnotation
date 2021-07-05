using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B5D RID: 2909
	public class QuestPart_WaitForEscort : QuestPart_MakeLord
	{
		// Token: 0x06004401 RID: 17409 RVA: 0x0016997D File Offset: 0x00167B7D
		protected override Lord MakeLord()
		{
			return LordMaker.MakeNewLord(this.faction, new LordJob_WaitForEscort(this.point, this.addFleeToil), base.Map, null);
		}

		// Token: 0x06004402 RID: 17410 RVA: 0x001699A4 File Offset: 0x00167BA4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.point, "point", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.addFleeToil, "addFleeToil", false, false);
		}

		// Token: 0x04002947 RID: 10567
		public IntVec3 point;

		// Token: 0x04002948 RID: 10568
		public bool addFleeToil = true;
	}
}
