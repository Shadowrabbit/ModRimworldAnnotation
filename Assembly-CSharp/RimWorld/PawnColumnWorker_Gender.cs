using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B6F RID: 7023
	public class PawnColumnWorker_Gender : PawnColumnWorker_Icon
	{
		// Token: 0x06009AC1 RID: 39617 RVA: 0x0006702A File Offset: 0x0006522A
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			return pawn.gender.GetIcon();
		}

		// Token: 0x06009AC2 RID: 39618 RVA: 0x00067037 File Offset: 0x00065237
		protected override string GetIconTip(Pawn pawn)
		{
			return pawn.GetGenderLabel().CapitalizeFirst();
		}
	}
}
