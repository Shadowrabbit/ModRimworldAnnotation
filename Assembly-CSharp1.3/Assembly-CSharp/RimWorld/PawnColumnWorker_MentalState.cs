using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001389 RID: 5001
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_MentalState : PawnColumnWorker_Icon
	{
		// Token: 0x060079A4 RID: 31140 RVA: 0x002B0107 File Offset: 0x002AE307
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			if (!pawn.InMentalState)
			{
				return null;
			}
			if (!pawn.InAggroMentalState)
			{
				return PawnColumnWorker_MentalState.IconNonAggro;
			}
			return PawnColumnWorker_MentalState.IconAggro;
		}

		// Token: 0x060079A5 RID: 31141 RVA: 0x002B0126 File Offset: 0x002AE326
		protected override string GetIconTip(Pawn pawn)
		{
			return pawn.InMentalState ? "IsInMentalState".Translate(pawn.MentalState.def.LabelCap) : null;
		}

		// Token: 0x04004382 RID: 17282
		private static readonly Texture2D IconNonAggro = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MentalStateNonAggro", true);

		// Token: 0x04004383 RID: 17283
		private static readonly Texture2D IconAggro = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MentalStateAggro", true);
	}
}
