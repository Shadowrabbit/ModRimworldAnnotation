using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B75 RID: 7029
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Pregnant : PawnColumnWorker_Icon
	{
		// Token: 0x06009AE2 RID: 39650 RVA: 0x0006718D File Offset: 0x0006538D
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			if (PawnColumnWorker_Pregnant.GetPregnantHediff(pawn) == null)
			{
				return null;
			}
			return PawnColumnWorker_Pregnant.Icon;
		}

		// Token: 0x06009AE3 RID: 39651 RVA: 0x0006719E File Offset: 0x0006539E
		protected override string GetIconTip(Pawn pawn)
		{
			return PawnColumnWorker_Pregnant.GetTooltipText(pawn);
		}

		// Token: 0x06009AE4 RID: 39652 RVA: 0x002D803C File Offset: 0x002D623C
		public static string GetTooltipText(Pawn pawn)
		{
			float gestationProgress = PawnColumnWorker_Pregnant.GetPregnantHediff(pawn).GestationProgress;
			int num = (int)(pawn.RaceProps.gestationPeriodDays * 60000f);
			int numTicks = (int)(gestationProgress * (float)num);
			return "PregnantIconDesc".Translate(numTicks.ToStringTicksToDays("F0"), num.ToStringTicksToDays("F0"));
		}

		// Token: 0x06009AE5 RID: 39653 RVA: 0x000671A6 File Offset: 0x000653A6
		private static Hediff_Pregnant GetPregnantHediff(Pawn pawn)
		{
			return (Hediff_Pregnant)pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Pregnant, true);
		}

		// Token: 0x040062D0 RID: 25296
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Pregnant", true);
	}
}
