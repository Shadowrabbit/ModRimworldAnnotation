using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001384 RID: 4996
	public class PawnColumnWorker_Guest : PawnColumnWorker_Icon
	{
		// Token: 0x06007985 RID: 31109 RVA: 0x002AFCF3 File Offset: 0x002ADEF3
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			if (pawn == null)
			{
				return null;
			}
			return pawn.guest.GetIcon();
		}

		// Token: 0x06007986 RID: 31110 RVA: 0x002AFD08 File Offset: 0x002ADF08
		protected override string GetIconTip(Pawn pawn)
		{
			string str = (pawn != null) ? pawn.guest.GetLabel() : null;
			if (!str.NullOrEmpty())
			{
				return str.CapitalizeFirst();
			}
			return null;
		}
	}
}
