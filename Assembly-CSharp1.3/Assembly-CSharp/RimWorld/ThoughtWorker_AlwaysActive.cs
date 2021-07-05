using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009BF RID: 2495
	public class ThoughtWorker_AlwaysActive : ThoughtWorker
	{
		// Token: 0x06003E0E RID: 15886 RVA: 0x00151910 File Offset: 0x0014FB10
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return true;
		}

		// Token: 0x06003E0F RID: 15887 RVA: 0x00151910 File Offset: 0x0014FB10
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
		{
			return true;
		}
	}
}
