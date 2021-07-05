using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005E2 RID: 1506
	public class MentalState_Slaughterer : MentalState
	{
		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06002B8F RID: 11151 RVA: 0x001040C9 File Offset: 0x001022C9
		public bool SlaughteredRecently
		{
			get
			{
				return this.lastSlaughterTicks >= 0 && Find.TickManager.TicksGame - this.lastSlaughterTicks < 3750;
			}
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06002B90 RID: 11152 RVA: 0x001040EE File Offset: 0x001022EE
		protected override bool CanEndBeforeMaxDurationNow
		{
			get
			{
				return this.lastSlaughterTicks >= 0;
			}
		}

		// Token: 0x06002B91 RID: 11153 RVA: 0x001040FC File Offset: 0x001022FC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastSlaughterTicks, "lastSlaughterTicks", 0, false);
			Scribe_Values.Look<int>(ref this.animalsSlaughtered, "animalsSlaughtered", 0, false);
		}

		// Token: 0x06002B92 RID: 11154 RVA: 0x00104128 File Offset: 0x00102328
		public override void MentalStateTick()
		{
			base.MentalStateTick();
			if (this.pawn.IsHashIntervalTick(600) && (this.pawn.CurJob == null || this.pawn.CurJob.def != JobDefOf.Slaughter) && SlaughtererMentalStateUtility.FindAnimal(this.pawn) == null)
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x06002B93 RID: 11155 RVA: 0x00104184 File Offset: 0x00102384
		public override void Notify_SlaughteredAnimal()
		{
			this.lastSlaughterTicks = Find.TickManager.TicksGame;
			this.animalsSlaughtered++;
			if (this.animalsSlaughtered >= 4)
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x04001A90 RID: 6800
		private int lastSlaughterTicks = -1;

		// Token: 0x04001A91 RID: 6801
		private int animalsSlaughtered;

		// Token: 0x04001A92 RID: 6802
		private const int NoAnimalToSlaughterCheckInterval = 600;

		// Token: 0x04001A93 RID: 6803
		private const int MinTicksBetweenSlaughter = 3750;

		// Token: 0x04001A94 RID: 6804
		private const int MaxAnimalsSlaughtered = 4;
	}
}
