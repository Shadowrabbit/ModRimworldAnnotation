using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FB9 RID: 4025
	public abstract class CompRitualEffect_IntervalSpawn : RitualVisualEffectComp
	{
		// Token: 0x1700104F RID: 4175
		// (get) Token: 0x06005EF6 RID: 24310 RVA: 0x00207FF4 File Offset: 0x002061F4
		protected CompProperties_RitualEffectIntervalSpawn Props
		{
			get
			{
				return (CompProperties_RitualEffectIntervalSpawn)this.props;
			}
		}

		// Token: 0x06005EF7 RID: 24311 RVA: 0x00208004 File Offset: 0x00206204
		public override bool ShouldSpawnNow(LordJob_Ritual ritual)
		{
			return (this.Props.delay <= 0 || this.ticksPassed >= this.Props.delay) && (this.Props.maxBursts <= 0 || this.burstsDone < this.Props.maxBursts) && (this.lastSpawnTick == -1 || GenTicks.TicksGame - this.lastSpawnTick >= this.Props.spawnIntervalTicks);
		}

		// Token: 0x06005EF8 RID: 24312 RVA: 0x0020807E File Offset: 0x0020627E
		public override void Tick()
		{
			base.Tick();
			this.ticksPassed++;
		}

		// Token: 0x06005EF9 RID: 24313 RVA: 0x00208094 File Offset: 0x00206294
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastSpawnTick, "lastSpawnTick", -1, false);
			Scribe_Values.Look<int>(ref this.burstsDone, "burstsDone", 0, false);
			Scribe_Values.Look<int>(ref this.ticksPassed, "ticksPassed", 0, false);
		}

		// Token: 0x040036BC RID: 14012
		public int lastSpawnTick = -1;

		// Token: 0x040036BD RID: 14013
		public int ticksPassed;

		// Token: 0x040036BE RID: 14014
		protected int burstsDone;
	}
}
