using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B72 RID: 7026
	public class PawnColumnWorker_LifeStage : PawnColumnWorker_Icon
	{
		// Token: 0x06009AD7 RID: 39639 RVA: 0x000670B3 File Offset: 0x000652B3
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			return pawn.ageTracker.CurLifeStageRace.GetIcon(pawn);
		}

		// Token: 0x06009AD8 RID: 39640 RVA: 0x000670C6 File Offset: 0x000652C6
		protected override string GetIconTip(Pawn pawn)
		{
			return pawn.ageTracker.CurLifeStage.LabelCap;
		}
	}
}
