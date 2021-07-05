using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A29 RID: 2601
	public class MentalState_Slaughterer : MentalState
	{
		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x06003E28 RID: 15912 RVA: 0x0002EC2F File Offset: 0x0002CE2F
		public bool SlaughteredRecently
		{
			get
			{
				return this.lastSlaughterTicks >= 0 && Find.TickManager.TicksGame - this.lastSlaughterTicks < 3750;
			}
		}

		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x06003E29 RID: 15913 RVA: 0x0002EC54 File Offset: 0x0002CE54
		protected override bool CanEndBeforeMaxDurationNow
		{
			get
			{
				return this.lastSlaughterTicks >= 0;
			}
		}

		// Token: 0x06003E2A RID: 15914 RVA: 0x0002EC62 File Offset: 0x0002CE62
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastSlaughterTicks, "lastSlaughterTicks", 0, false);
			Scribe_Values.Look<int>(ref this.animalsSlaughtered, "animalsSlaughtered", 0, false);
		}

		// Token: 0x06003E2B RID: 15915 RVA: 0x00177F10 File Offset: 0x00176110
		public override void MentalStateTick()
		{
			base.MentalStateTick();
			if (this.pawn.IsHashIntervalTick(600) && (this.pawn.CurJob == null || this.pawn.CurJob.def != JobDefOf.Slaughter) && SlaughtererMentalStateUtility.FindAnimal(this.pawn) == null)
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x06003E2C RID: 15916 RVA: 0x0002EC8E File Offset: 0x0002CE8E
		public override void Notify_SlaughteredAnimal()
		{
			this.lastSlaughterTicks = Find.TickManager.TicksGame;
			this.animalsSlaughtered++;
			if (this.animalsSlaughtered >= 4)
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x04002AF0 RID: 10992
		private int lastSlaughterTicks = -1;

		// Token: 0x04002AF1 RID: 10993
		private int animalsSlaughtered;

		// Token: 0x04002AF2 RID: 10994
		private const int NoAnimalToSlaughterCheckInterval = 600;

		// Token: 0x04002AF3 RID: 10995
		private const int MinTicksBetweenSlaughter = 3750;

		// Token: 0x04002AF4 RID: 10996
		private const int MaxAnimalsSlaughtered = 4;
	}
}
