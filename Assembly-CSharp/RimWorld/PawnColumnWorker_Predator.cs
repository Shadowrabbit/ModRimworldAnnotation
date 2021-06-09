using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B74 RID: 7028
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Predator : PawnColumnWorker_Icon
	{
		// Token: 0x06009ADE RID: 39646 RVA: 0x00067154 File Offset: 0x00065354
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			if (pawn.RaceProps.predator)
			{
				return PawnColumnWorker_Predator.Icon;
			}
			return null;
		}

		// Token: 0x06009ADF RID: 39647 RVA: 0x0006716A File Offset: 0x0006536A
		protected override string GetIconTip(Pawn pawn)
		{
			return "IsPredator".Translate();
		}

		// Token: 0x040062CF RID: 25295
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Predator", true);
	}
}
