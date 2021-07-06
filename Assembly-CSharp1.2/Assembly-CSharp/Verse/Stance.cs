using System;

namespace Verse
{
	// Token: 0x02000465 RID: 1125
	public abstract class Stance : IExposable
	{
		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06001C94 RID: 7316 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool StanceBusy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06001C95 RID: 7317 RVA: 0x00019D5A File Offset: 0x00017F5A
		protected Pawn Pawn
		{
			get
			{
				return this.stanceTracker.pawn;
			}
		}

		// Token: 0x06001C96 RID: 7318 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void StanceTick()
		{
		}

		// Token: 0x06001C97 RID: 7319 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void StanceDraw()
		{
		}

		// Token: 0x06001C98 RID: 7320 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void ExposeData()
		{
		}

		// Token: 0x04001477 RID: 5239
		public Pawn_StanceTracker stanceTracker;
	}
}
