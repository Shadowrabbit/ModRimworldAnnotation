using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200118A RID: 4490
	public class QueuedIncident : IExposable
	{
		// Token: 0x17000F89 RID: 3977
		// (get) Token: 0x060062F6 RID: 25334 RVA: 0x00044135 File Offset: 0x00042335
		public int FireTick
		{
			get
			{
				return this.fireTick;
			}
		}

		// Token: 0x17000F8A RID: 3978
		// (get) Token: 0x060062F7 RID: 25335 RVA: 0x0004413D File Offset: 0x0004233D
		public FiringIncident FiringIncident
		{
			get
			{
				return this.firingInc;
			}
		}

		// Token: 0x17000F8B RID: 3979
		// (get) Token: 0x060062F8 RID: 25336 RVA: 0x00044145 File Offset: 0x00042345
		public int RetryDurationTicks
		{
			get
			{
				return this.retryDurationTicks;
			}
		}

		// Token: 0x17000F8C RID: 3980
		// (get) Token: 0x060062F9 RID: 25337 RVA: 0x0004414D File Offset: 0x0004234D
		public bool TriedToFire
		{
			get
			{
				return this.triedToFire;
			}
		}

		// Token: 0x060062FA RID: 25338 RVA: 0x00044155 File Offset: 0x00042355
		public QueuedIncident()
		{
		}

		// Token: 0x060062FB RID: 25339 RVA: 0x00044164 File Offset: 0x00042364
		public QueuedIncident(FiringIncident firingInc, int fireTick, int retryDurationTicks = 0)
		{
			this.firingInc = firingInc;
			this.fireTick = fireTick;
			this.retryDurationTicks = retryDurationTicks;
		}

		// Token: 0x060062FC RID: 25340 RVA: 0x001EDADC File Offset: 0x001EBCDC
		public void ExposeData()
		{
			Scribe_Deep.Look<FiringIncident>(ref this.firingInc, "firingInc", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.fireTick, "fireTick", 0, false);
			Scribe_Values.Look<int>(ref this.retryDurationTicks, "retryDurationTicks", 0, false);
			Scribe_Values.Look<bool>(ref this.triedToFire, "triedToFire", false, false);
		}

		// Token: 0x060062FD RID: 25341 RVA: 0x00044188 File Offset: 0x00042388
		public void Notify_TriedToFire()
		{
			this.triedToFire = true;
		}

		// Token: 0x060062FE RID: 25342 RVA: 0x00044191 File Offset: 0x00042391
		public override string ToString()
		{
			return this.fireTick + "->" + this.firingInc.ToString();
		}

		// Token: 0x0400424C RID: 16972
		private FiringIncident firingInc;

		// Token: 0x0400424D RID: 16973
		private int fireTick = -1;

		// Token: 0x0400424E RID: 16974
		private int retryDurationTicks;

		// Token: 0x0400424F RID: 16975
		private bool triedToFire;

		// Token: 0x04004250 RID: 16976
		public const int RetryIntervalTicks = 833;
	}
}
