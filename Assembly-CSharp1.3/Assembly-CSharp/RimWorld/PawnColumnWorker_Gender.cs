using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001383 RID: 4995
	public class PawnColumnWorker_Gender : PawnColumnWorker_Icon
	{
		// Token: 0x06007982 RID: 31106 RVA: 0x002AFCD9 File Offset: 0x002ADED9
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			return pawn.gender.GetIcon();
		}

		// Token: 0x06007983 RID: 31107 RVA: 0x002AFCE6 File Offset: 0x002ADEE6
		protected override string GetIconTip(Pawn pawn)
		{
			return pawn.GetGenderLabel().CapitalizeFirst();
		}
	}
}
