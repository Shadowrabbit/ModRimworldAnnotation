using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BF1 RID: 3057
	public class QueuedIncident : IExposable
	{
		// Token: 0x17000C9D RID: 3229
		// (get) Token: 0x060047ED RID: 18413 RVA: 0x0017C10A File Offset: 0x0017A30A
		public int FireTick
		{
			get
			{
				return this.fireTick;
			}
		}

		// Token: 0x17000C9E RID: 3230
		// (get) Token: 0x060047EE RID: 18414 RVA: 0x0017C112 File Offset: 0x0017A312
		public FiringIncident FiringIncident
		{
			get
			{
				return this.firingInc;
			}
		}

		// Token: 0x17000C9F RID: 3231
		// (get) Token: 0x060047EF RID: 18415 RVA: 0x0017C11A File Offset: 0x0017A31A
		public int RetryDurationTicks
		{
			get
			{
				return this.retryDurationTicks;
			}
		}

		// Token: 0x17000CA0 RID: 3232
		// (get) Token: 0x060047F0 RID: 18416 RVA: 0x0017C122 File Offset: 0x0017A322
		public bool TriedToFire
		{
			get
			{
				return this.triedToFire;
			}
		}

		// Token: 0x060047F1 RID: 18417 RVA: 0x0017C12A File Offset: 0x0017A32A
		public QueuedIncident()
		{
		}

		// Token: 0x060047F2 RID: 18418 RVA: 0x0017C139 File Offset: 0x0017A339
		public QueuedIncident(FiringIncident firingInc, int fireTick, int retryDurationTicks = 0)
		{
			this.firingInc = firingInc;
			this.fireTick = fireTick;
			this.retryDurationTicks = retryDurationTicks;
		}

		// Token: 0x060047F3 RID: 18419 RVA: 0x0017C160 File Offset: 0x0017A360
		public void ExposeData()
		{
			Scribe_Deep.Look<FiringIncident>(ref this.firingInc, "firingInc", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.fireTick, "fireTick", 0, false);
			Scribe_Values.Look<int>(ref this.retryDurationTicks, "retryDurationTicks", 0, false);
			Scribe_Values.Look<bool>(ref this.triedToFire, "triedToFire", false, false);
		}

		// Token: 0x060047F4 RID: 18420 RVA: 0x0017C1B8 File Offset: 0x0017A3B8
		public void Notify_TriedToFire()
		{
			this.triedToFire = true;
		}

		// Token: 0x060047F5 RID: 18421 RVA: 0x0017C1C1 File Offset: 0x0017A3C1
		public override string ToString()
		{
			return this.fireTick + "->" + this.firingInc.ToString();
		}

		// Token: 0x04002C38 RID: 11320
		private FiringIncident firingInc;

		// Token: 0x04002C39 RID: 11321
		private int fireTick = -1;

		// Token: 0x04002C3A RID: 11322
		private int retryDurationTicks;

		// Token: 0x04002C3B RID: 11323
		private bool triedToFire;

		// Token: 0x04002C3C RID: 11324
		public const int RetryIntervalTicks = 833;
	}
}
