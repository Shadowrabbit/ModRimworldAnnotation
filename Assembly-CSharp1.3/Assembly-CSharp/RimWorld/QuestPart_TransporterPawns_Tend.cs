using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BB6 RID: 2998
	public class QuestPart_TransporterPawns_Tend : QuestPart_TransporterPawns
	{
		// Token: 0x060045F2 RID: 17906 RVA: 0x001725BC File Offset: 0x001707BC
		public override void Process(Pawn pawn)
		{
			int num = 0;
			while (pawn.health.HasHediffsNeedingTend(false))
			{
				num++;
				if (num > 10000)
				{
					Log.Error("Too many iterations.");
					return;
				}
				this.DoTend(pawn);
			}
		}

		// Token: 0x060045F3 RID: 17907 RVA: 0x001725F9 File Offset: 0x001707F9
		protected virtual void DoTend(Pawn pawn)
		{
			TendUtility.DoTend(null, pawn, null);
		}
	}
}
