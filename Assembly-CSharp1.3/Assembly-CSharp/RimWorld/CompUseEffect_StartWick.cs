using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001210 RID: 4624
	public class CompUseEffect_StartWick : CompUseEffect
	{
		// Token: 0x06006F03 RID: 28419 RVA: 0x00251B3A File Offset: 0x0024FD3A
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			this.parent.GetComp<CompExplosive>().StartWick(null);
		}
	}
}
