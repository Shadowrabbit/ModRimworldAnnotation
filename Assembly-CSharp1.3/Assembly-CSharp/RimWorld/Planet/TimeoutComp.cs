using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017FB RID: 6139
	public class TimeoutComp : WorldObjectComp
	{
		// Token: 0x17001777 RID: 6007
		// (get) Token: 0x06008F4A RID: 36682 RVA: 0x003354EB File Offset: 0x003336EB
		public bool Active
		{
			get
			{
				return this.timeoutEndTick != -1;
			}
		}

		// Token: 0x17001778 RID: 6008
		// (get) Token: 0x06008F4B RID: 36683 RVA: 0x003354F9 File Offset: 0x003336F9
		public bool Passed
		{
			get
			{
				return this.Active && Find.TickManager.TicksGame >= this.timeoutEndTick;
			}
		}

		// Token: 0x17001779 RID: 6009
		// (get) Token: 0x06008F4C RID: 36684 RVA: 0x0033551A File Offset: 0x0033371A
		private bool ShouldRemoveWorldObjectNow
		{
			get
			{
				return this.Passed && !base.ParentHasMap;
			}
		}

		// Token: 0x1700177A RID: 6010
		// (get) Token: 0x06008F4D RID: 36685 RVA: 0x0033552F File Offset: 0x0033372F
		public int TicksLeft
		{
			get
			{
				if (!this.Active)
				{
					return 0;
				}
				return this.timeoutEndTick - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06008F4E RID: 36686 RVA: 0x0033554C File Offset: 0x0033374C
		public void StartTimeout(int ticks)
		{
			this.timeoutEndTick = Find.TickManager.TicksGame + ticks;
		}

		// Token: 0x06008F4F RID: 36687 RVA: 0x00335560 File Offset: 0x00333760
		public void StopTimeout()
		{
			this.timeoutEndTick = -1;
		}

		// Token: 0x06008F50 RID: 36688 RVA: 0x00335569 File Offset: 0x00333769
		public override void CompTick()
		{
			base.CompTick();
			if (this.ShouldRemoveWorldObjectNow)
			{
				this.parent.Destroy();
			}
		}

		// Token: 0x06008F51 RID: 36689 RVA: 0x00335584 File Offset: 0x00333784
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.timeoutEndTick, "timeoutEndTick", 0, false);
		}

		// Token: 0x06008F52 RID: 36690 RVA: 0x0033559E File Offset: 0x0033379E
		public override string CompInspectStringExtra()
		{
			if (this.Active && !base.ParentHasMap)
			{
				return "WorldObjectTimeout".Translate(this.TicksLeft.ToStringTicksToPeriod(true, false, true, true));
			}
			return null;
		}

		// Token: 0x04005A1C RID: 23068
		private int timeoutEndTick = -1;
	}
}
