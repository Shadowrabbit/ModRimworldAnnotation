using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A1D RID: 2589
	public class MentalBreakWorker_Catatonic : MentalBreakWorker
	{
		// Token: 0x06003DD8 RID: 15832 RVA: 0x0002E978 File Offset: 0x0002CB78
		public override bool BreakCanOccur(Pawn pawn)
		{
			return pawn.IsColonist && pawn.Spawned && base.BreakCanOccur(pawn);
		}

		// Token: 0x06003DD9 RID: 15833 RVA: 0x00176C2C File Offset: 0x00174E2C
		public override bool TryStart(Pawn pawn, string reason, bool causedByMood)
		{
			pawn.health.AddHediff(HediffDefOf.CatatonicBreakdown, null, null, null);
			base.TrySendLetter(pawn, "LetterCatatonicMentalBreak", reason);
			return true;
		}
	}
}
