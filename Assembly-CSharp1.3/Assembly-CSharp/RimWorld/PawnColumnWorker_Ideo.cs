using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001386 RID: 4998
	public class PawnColumnWorker_Ideo : PawnColumnWorker_Icon
	{
		// Token: 0x06007997 RID: 31127 RVA: 0x002AFEE4 File Offset: 0x002AE0E4
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			Ideo ideo = pawn.Ideo;
			if (ideo == null)
			{
				return null;
			}
			return ideo.Icon;
		}

		// Token: 0x06007998 RID: 31128 RVA: 0x002AFEF7 File Offset: 0x002AE0F7
		protected override Color GetIconColor(Pawn pawn)
		{
			if (pawn.Ideo == null)
			{
				return Color.white;
			}
			return pawn.Ideo.Color;
		}

		// Token: 0x06007999 RID: 31129 RVA: 0x002AFF12 File Offset: 0x002AE112
		protected override string GetIconTip(Pawn pawn)
		{
			if (pawn.Ideo != null)
			{
				return pawn.Ideo.name + "\n\n" + "ClickForMoreInfo".Translate();
			}
			return null;
		}

		// Token: 0x0600799A RID: 31130 RVA: 0x002AFF47 File Offset: 0x002AE147
		protected override void ClickedIcon(Pawn pawn)
		{
			if (pawn.Ideo != null)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Ideos, true);
				IdeoUIUtility.OpenIdeoInfo(pawn.Ideo);
			}
		}
	}
}
