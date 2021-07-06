using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FB1 RID: 4017
	public class JoyKindDef : Def
	{
		// Token: 0x060057E3 RID: 22499 RVA: 0x001CEA58 File Offset: 0x001CCC58
		public bool PawnCanDo(Pawn pawn)
		{
			if (pawn.royalty != null)
			{
				foreach (RoyalTitle royalTitle in pawn.royalty.AllTitlesInEffectForReading)
				{
					if (royalTitle.conceited && royalTitle.def.JoyKindDisabled(this))
					{
						return false;
					}
				}
				if (this.titleRequiredAny == null)
				{
					return true;
				}
				bool flag = false;
				foreach (RoyalTitle royalTitle2 in pawn.royalty.AllTitlesInEffectForReading)
				{
					if (this.titleRequiredAny.Contains(royalTitle2.def))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
				return true;
			}
			return true;
		}

		// Token: 0x040039CE RID: 14798
		public List<RoyalTitleDef> titleRequiredAny;

		// Token: 0x040039CF RID: 14799
		public bool needsThing = true;
	}
}
