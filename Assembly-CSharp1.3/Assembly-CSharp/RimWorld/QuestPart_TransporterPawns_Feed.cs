using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BB5 RID: 2997
	public class QuestPart_TransporterPawns_Feed : QuestPart_TransporterPawns
	{
		// Token: 0x060045F0 RID: 17904 RVA: 0x00172592 File Offset: 0x00170792
		public override void Process(Pawn pawn)
		{
			pawn.needs.food.CurLevel = pawn.needs.food.MaxLevel;
		}
	}
}
