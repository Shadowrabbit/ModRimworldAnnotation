using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018D1 RID: 6353
	public class CompInitiatable : ThingComp
	{
		// Token: 0x1700161D RID: 5661
		// (get) Token: 0x06008CC6 RID: 36038 RVA: 0x0005E5B4 File Offset: 0x0005C7B4
		public bool Initiated
		{
			get
			{
				return this.Delay <= 0 || (this.spawnedTick >= 0 && Find.TickManager.TicksGame >= this.spawnedTick + this.Delay);
			}
		}

		// Token: 0x1700161E RID: 5662
		// (get) Token: 0x06008CC7 RID: 36039 RVA: 0x0005E5E8 File Offset: 0x0005C7E8
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

		// Token: 0x1700161F RID: 5663
		// (get) Token: 0x06008CC8 RID: 36040 RVA: 0x0005E605 File Offset: 0x0005C805
		private CompProperties_Initiatable Props
		{
			get
			{
				return (CompProperties_Initiatable)this.props;
			}
		}

		// Token: 0x06008CC9 RID: 36041 RVA: 0x0005E612 File Offset: 0x0005C812
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.spawnedTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06008CCA RID: 36042 RVA: 0x0028DA68 File Offset: 0x0028BC68
		public override string CompInspectStringExtra()
		{
			if (!this.Initiated)
			{
				return "InitiatesIn".Translate() + ": " + (this.spawnedTick + this.Delay - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true);
			}
			return base.CompInspectStringExtra();
		}

		// Token: 0x06008CCB RID: 36043 RVA: 0x0005E62E File Offset: 0x0005C82E
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.spawnedTick, "spawnedTick", -1, false);
			Scribe_Values.Look<int>(ref this.initiationDelayTicksOverride, "initiationDelayTicksOverride", 0, false);
		}

		// Token: 0x040059FF RID: 23039
		private int spawnedTick = -1;

		// Token: 0x04005A00 RID: 23040
		public int initiationDelayTicksOverride;
	}
}
