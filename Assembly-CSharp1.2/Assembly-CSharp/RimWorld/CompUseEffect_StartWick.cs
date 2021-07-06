using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018DF RID: 6367
	public class CompUseEffect_StartWick : CompUseEffect
	{
		// Token: 0x06008D04 RID: 36100 RVA: 0x0005E889 File Offset: 0x0005CA89
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			this.parent.GetComp<CompExplosive>().StartWick(null);
		}
	}
}
