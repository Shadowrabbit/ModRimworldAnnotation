using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200138A RID: 5002
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Predator : PawnColumnWorker_Icon
	{
		// Token: 0x060079A8 RID: 31144 RVA: 0x002B017E File Offset: 0x002AE37E
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			if (pawn.RaceProps.predator)
			{
				return PawnColumnWorker_Predator.Icon;
			}
			return null;
		}

		// Token: 0x060079A9 RID: 31145 RVA: 0x002B0194 File Offset: 0x002AE394
		protected override string GetIconTip(Pawn pawn)
		{
			return "IsPredator".Translate();
		}

		// Token: 0x04004384 RID: 17284
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Predator", true);
	}
}
