using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E86 RID: 3718
	public class Pawn_GuiltTracker : IExposable
	{
		// Token: 0x17000F26 RID: 3878
		// (get) Token: 0x06005717 RID: 22295 RVA: 0x001D8FF9 File Offset: 0x001D71F9
		public bool IsGuilty
		{
			get
			{
				return this.guiltyTicksLeft > 0 || this.pawn.InAggroMentalState;
			}
		}

		// Token: 0x17000F27 RID: 3879
		// (get) Token: 0x06005718 RID: 22296 RVA: 0x001D9011 File Offset: 0x001D7211
		public int TicksUntilInnocent
		{
			get
			{
				return this.guiltyTicksLeft;
			}
		}

		// Token: 0x17000F28 RID: 3880
		// (get) Token: 0x06005719 RID: 22297 RVA: 0x001D9019 File Offset: 0x001D7219
		public string Tip
		{
			get
			{
				return "GuiltyDesc".Translate() + ": " + this.TicksUntilInnocent.ToStringTicksToPeriod(true, false, true, true);
			}
		}

		// Token: 0x0600571A RID: 22298 RVA: 0x001D9048 File Offset: 0x001D7248
		public Pawn_GuiltTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600571B RID: 22299 RVA: 0x001D9057 File Offset: 0x001D7257
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.guiltyTicksLeft, "guiltyTicksLeft", 0, false);
			Scribe_Values.Look<bool>(ref this.awaitingExecution, "awaitingExecution", false, false);
		}

		// Token: 0x0600571C RID: 22300 RVA: 0x001D907D File Offset: 0x001D727D
		public void Notify_Guilty(int durationTicks = 60000)
		{
			this.guiltyTicksLeft = durationTicks;
		}

		// Token: 0x0600571D RID: 22301 RVA: 0x001D9086 File Offset: 0x001D7286
		public void GuiltTrackerTick()
		{
			if (this.guiltyTicksLeft > 0)
			{
				this.guiltyTicksLeft--;
				return;
			}
			this.awaitingExecution = false;
		}

		// Token: 0x04003385 RID: 13189
		private Pawn pawn;

		// Token: 0x04003386 RID: 13190
		public bool awaitingExecution;

		// Token: 0x04003387 RID: 13191
		private int guiltyTicksLeft;

		// Token: 0x04003388 RID: 13192
		private const int DefaultGuiltyDuration = 60000;
	}
}
