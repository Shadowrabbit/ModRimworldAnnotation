using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005C2 RID: 1474
	public class MentalBreakWorker_Catatonic : MentalBreakWorker
	{
		// Token: 0x06002AF2 RID: 10994 RVA: 0x00101825 File Offset: 0x000FFA25
		public override bool BreakCanOccur(Pawn pawn)
		{
			return pawn.IsColonist && pawn.Spawned && base.BreakCanOccur(pawn);
		}

		// Token: 0x06002AF3 RID: 10995 RVA: 0x00101840 File Offset: 0x000FFA40
		public override bool TryStart(Pawn pawn, string reason, bool causedByMood)
		{
			pawn.health.AddHediff(HediffDefOf.CatatonicBreakdown, null, null, null);
			base.TrySendLetter(pawn, "LetterCatatonicMentalBreak", reason);
			return true;
		}
	}
}
