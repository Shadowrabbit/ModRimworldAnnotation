using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200138B RID: 5003
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Pregnant : PawnColumnWorker_Icon
	{
		// Token: 0x060079AC RID: 31148 RVA: 0x002B01B7 File Offset: 0x002AE3B7
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			if (PawnColumnWorker_Pregnant.GetPregnantHediff(pawn) == null)
			{
				return null;
			}
			return PawnColumnWorker_Pregnant.Icon;
		}

		// Token: 0x060079AD RID: 31149 RVA: 0x002B01C8 File Offset: 0x002AE3C8
		protected override string GetIconTip(Pawn pawn)
		{
			return PawnColumnWorker_Pregnant.GetTooltipText(pawn);
		}

		// Token: 0x060079AE RID: 31150 RVA: 0x002B01D0 File Offset: 0x002AE3D0
		public static string GetTooltipText(Pawn pawn)
		{
			float gestationProgress = PawnColumnWorker_Pregnant.GetPregnantHediff(pawn).GestationProgress;
			int num = (int)(pawn.RaceProps.gestationPeriodDays * 60000f);
			int numTicks = (int)(gestationProgress * (float)num);
			return "PregnantIconDesc".Translate(numTicks.ToStringTicksToDays("F0"), num.ToStringTicksToDays("F0"));
		}

		// Token: 0x060079AF RID: 31151 RVA: 0x002B022F File Offset: 0x002AE42F
		private static Hediff_Pregnant GetPregnantHediff(Pawn pawn)
		{
			return (Hediff_Pregnant)pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Pregnant, true);
		}

		// Token: 0x04004385 RID: 17285
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Pregnant", true);
	}
}
