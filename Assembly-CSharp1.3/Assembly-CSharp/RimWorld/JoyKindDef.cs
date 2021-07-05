using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A87 RID: 2695
	public class JoyKindDef : Def
	{
		// Token: 0x0600405B RID: 16475 RVA: 0x0015C3AC File Offset: 0x0015A5AC
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

		// Token: 0x040024E7 RID: 9447
		public List<RoyalTitleDef> titleRequiredAny;

		// Token: 0x040024E8 RID: 9448
		public bool needsThing = true;
	}
}
