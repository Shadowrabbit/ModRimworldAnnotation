using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B0 RID: 2480
	public class ThoughtWorker_Pain : ThoughtWorker
	{
		// Token: 0x06003DEE RID: 15854 RVA: 0x00153C7C File Offset: 0x00151E7C
		public static ThoughtState CurrentThoughtState(Pawn p)
		{
			float painTotal = p.health.hediffSet.PainTotal;
			if (painTotal < 0.0001f)
			{
				return ThoughtState.Inactive;
			}
			if (painTotal < 0.15f)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			if (painTotal < 0.4f)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (painTotal < 0.8f)
			{
				return ThoughtState.ActiveAtStage(2);
			}
			return ThoughtState.ActiveAtStage(3);
		}

		// Token: 0x06003DEF RID: 15855 RVA: 0x00153CDB File Offset: 0x00151EDB
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (ThoughtUtility.ThoughtNullified(p, this.def))
			{
				return ThoughtState.Inactive;
			}
			return ThoughtWorker_Pain.CurrentThoughtState(p);
		}
	}
}
