using System;

namespace Verse
{
	// Token: 0x020002F5 RID: 757
	public abstract class Stance : IExposable
	{
		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06001600 RID: 5632 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool StanceBusy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06001601 RID: 5633 RVA: 0x000803AA File Offset: 0x0007E5AA
		protected Pawn Pawn
		{
			get
			{
				return this.stanceTracker.pawn;
			}
		}

		// Token: 0x06001602 RID: 5634 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void StanceTick()
		{
		}

		// Token: 0x06001603 RID: 5635 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void StanceDraw()
		{
		}

		// Token: 0x06001604 RID: 5636 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ExposeData()
		{
		}

		// Token: 0x04000F49 RID: 3913
		public Pawn_StanceTracker stanceTracker;
	}
}
