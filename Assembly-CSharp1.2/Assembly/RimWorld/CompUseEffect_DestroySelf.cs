using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018D5 RID: 6357
	public class CompUseEffect_DestroySelf : CompUseEffect
	{
		// Token: 0x17001623 RID: 5667
		// (get) Token: 0x06008CD8 RID: 36056 RVA: 0x0005E6A9 File Offset: 0x0005C8A9
		public override float OrderPriority
		{
			get
			{
				return -1000f;
			}
		}

		// Token: 0x06008CD9 RID: 36057 RVA: 0x0005E6B0 File Offset: 0x0005C8B0
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
		}
	}
}
