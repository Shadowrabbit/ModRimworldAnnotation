using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020017B3 RID: 6067
	public class CompDestroyAfterDelay : ThingComp
	{
		// Token: 0x170014C5 RID: 5317
		// (get) Token: 0x06008621 RID: 34337 RVA: 0x0005A008 File Offset: 0x00058208
		public CompProperties_DestroyAfterDelay Props
		{
			get
			{
				return (CompProperties_DestroyAfterDelay)this.props;
			}
		}

		// Token: 0x06008622 RID: 34338 RVA: 0x00277A20 File Offset: 0x00275C20
		public override void CompTick()
		{
			base.CompTick();
			if (Find.TickManager.TicksGame > this.spawnTick + this.Props.delayTicks && !this.parent.Destroyed)
			{
				this.parent.Destroy(this.Props.destroyMode);
			}
		}

		// Token: 0x06008623 RID: 34339 RVA: 0x00277A74 File Offset: 0x00275C74
		public override string CompInspectStringExtra()
		{
			if (this.Props.countdownLabel.NullOrEmpty())
			{
				return "";
			}
			int numTicks = Mathf.Max(0, this.spawnTick + this.Props.delayTicks - Find.TickManager.TicksGame);
			return this.Props.countdownLabel + ": " + numTicks.ToStringSecondsFromTicks();
		}

		// Token: 0x06008624 RID: 34340 RVA: 0x0005A015 File Offset: 0x00058215
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.spawnTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06008625 RID: 34341 RVA: 0x0005A031 File Offset: 0x00058231
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.spawnTick, "spawnTick", 0, false);
		}

		// Token: 0x04005673 RID: 22131
		public int spawnTick;
	}
}
