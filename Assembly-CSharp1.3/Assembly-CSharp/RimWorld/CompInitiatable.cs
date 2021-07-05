using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011F9 RID: 4601
	public class CompInitiatable : ThingComp
	{
		// Token: 0x17001335 RID: 4917
		// (get) Token: 0x06006EAD RID: 28333 RVA: 0x00250B6C File Offset: 0x0024ED6C
		public bool Initiated
		{
			get
			{
				return this.Delay <= 0 || (this.spawnedTick >= 0 && Find.TickManager.TicksGame >= this.spawnedTick + this.Delay);
			}
		}

		// Token: 0x17001336 RID: 4918
		// (get) Token: 0x06006EAE RID: 28334 RVA: 0x00250BA0 File Offset: 0x0024EDA0
		private int Delay
		{
			get
			{
				if (this.initiationDelayTicksOverride <= 0)
				{
					return this.Props.initiationDelayTicks;
				}
				return this.initiationDelayTicksOverride;
			}
		}

		// Token: 0x17001337 RID: 4919
		// (get) Token: 0x06006EAF RID: 28335 RVA: 0x00250BBD File Offset: 0x0024EDBD
		private CompProperties_Initiatable Props
		{
			get
			{
				return (CompProperties_Initiatable)this.props;
			}
		}

		// Token: 0x06006EB0 RID: 28336 RVA: 0x00250BCA File Offset: 0x0024EDCA
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.spawnedTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06006EB1 RID: 28337 RVA: 0x00250BE8 File Offset: 0x0024EDE8
		public override string CompInspectStringExtra()
		{
			if (!this.Initiated)
			{
				return "InitiatesIn".Translate() + ": " + (this.spawnedTick + this.Delay - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true);
			}
			return base.CompInspectStringExtra();
		}

		// Token: 0x06006EB2 RID: 28338 RVA: 0x00250C43 File Offset: 0x0024EE43
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.spawnedTick, "spawnedTick", -1, false);
			Scribe_Values.Look<int>(ref this.initiationDelayTicksOverride, "initiationDelayTicksOverride", 0, false);
		}

		// Token: 0x04003D48 RID: 15688
		private int spawnedTick = -1;

		// Token: 0x04003D49 RID: 15689
		public int initiationDelayTicksOverride;
	}
}
