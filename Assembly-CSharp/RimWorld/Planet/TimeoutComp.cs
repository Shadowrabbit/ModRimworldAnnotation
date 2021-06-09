using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020021A4 RID: 8612
	public class TimeoutComp : WorldObjectComp
	{
		// Token: 0x17001B45 RID: 6981
		// (get) Token: 0x0600B7FE RID: 47102 RVA: 0x000775D8 File Offset: 0x000757D8
		public bool Active
		{
			get
			{
				return this.timeoutEndTick != -1;
			}
		}

		// Token: 0x17001B46 RID: 6982
		// (get) Token: 0x0600B7FF RID: 47103 RVA: 0x000775E6 File Offset: 0x000757E6
		public bool Passed
		{
			get
			{
				return this.Active && Find.TickManager.TicksGame >= this.timeoutEndTick;
			}
		}

		// Token: 0x17001B47 RID: 6983
		// (get) Token: 0x0600B800 RID: 47104 RVA: 0x00077607 File Offset: 0x00075807
		private bool ShouldRemoveWorldObjectNow
		{
			get
			{
				return this.Passed && !base.ParentHasMap;
			}
		}

		// Token: 0x17001B48 RID: 6984
		// (get) Token: 0x0600B801 RID: 47105 RVA: 0x0007761C File Offset: 0x0007581C
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

		// Token: 0x0600B802 RID: 47106 RVA: 0x00077639 File Offset: 0x00075839
		public void StartTimeout(int ticks)
		{
			this.timeoutEndTick = Find.TickManager.TicksGame + ticks;
		}

		// Token: 0x0600B803 RID: 47107 RVA: 0x0007764D File Offset: 0x0007584D
		public void StopTimeout()
		{
			this.timeoutEndTick = -1;
		}

		// Token: 0x0600B804 RID: 47108 RVA: 0x00077656 File Offset: 0x00075856
		public override void CompTick()
		{
			base.CompTick();
			if (this.ShouldRemoveWorldObjectNow)
			{
				this.parent.Destroy();
			}
		}

		// Token: 0x0600B805 RID: 47109 RVA: 0x00077671 File Offset: 0x00075871
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.timeoutEndTick, "timeoutEndTick", 0, false);
		}

		// Token: 0x0600B806 RID: 47110 RVA: 0x0007768B File Offset: 0x0007588B
		public override string CompInspectStringExtra()
		{
			if (this.Active && !base.ParentHasMap)
			{
				return "WorldObjectTimeout".Translate(this.TicksLeft.ToStringTicksToPeriod(true, false, true, true));
			}
			return null;
		}

		// Token: 0x04007DB9 RID: 32185
		private int timeoutEndTick = -1;
	}
}
