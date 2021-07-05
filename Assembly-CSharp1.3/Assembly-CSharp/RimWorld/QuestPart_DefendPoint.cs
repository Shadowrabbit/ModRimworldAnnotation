using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B57 RID: 2903
	public class QuestPart_DefendPoint : QuestPart_MakeLord
	{
		// Token: 0x060043E9 RID: 17385 RVA: 0x0016943F File Offset: 0x0016763F
		protected override Lord MakeLord()
		{
			return LordMaker.MakeNewLord(this.faction, new LordJob_DefendPoint(this.point, this.wanderRadius, this.isCaravanSendable, this.addFleeToil), base.Map, null);
		}

		// Token: 0x060043EA RID: 17386 RVA: 0x00169470 File Offset: 0x00167670
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.point, "point", default(IntVec3), false);
			Scribe_Values.Look<float?>(ref this.wanderRadius, "wanderRadius", null, false);
			Scribe_Values.Look<bool>(ref this.isCaravanSendable, "isCaravanSendable", false, false);
			Scribe_Values.Look<bool>(ref this.addFleeToil, "addFleeToil", false, false);
		}

		// Token: 0x04002933 RID: 10547
		public IntVec3 point;

		// Token: 0x04002934 RID: 10548
		public float? wanderRadius;

		// Token: 0x04002935 RID: 10549
		public bool isCaravanSendable;

		// Token: 0x04002936 RID: 10550
		public bool addFleeToil = true;
	}
}
