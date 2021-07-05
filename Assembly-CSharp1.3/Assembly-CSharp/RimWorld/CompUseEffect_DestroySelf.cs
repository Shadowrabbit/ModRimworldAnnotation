using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001207 RID: 4615
	public class CompUseEffect_DestroySelf : CompUseEffect
	{
		// Token: 0x17001345 RID: 4933
		// (get) Token: 0x06006EE6 RID: 28390 RVA: 0x0025154D File Offset: 0x0024F74D
		public override float OrderPriority
		{
			get
			{
				return -1000f;
			}
		}

		// Token: 0x06006EE7 RID: 28391 RVA: 0x00251554 File Offset: 0x0024F754
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
		}
	}
}
