using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001121 RID: 4385
	public class CompDestroyAfterDelay : ThingComp
	{
		// Token: 0x17001206 RID: 4614
		// (get) Token: 0x0600694F RID: 26959 RVA: 0x00237F05 File Offset: 0x00236105
		public CompProperties_DestroyAfterDelay Props
		{
			get
			{
				return (CompProperties_DestroyAfterDelay)this.props;
			}
		}

		// Token: 0x06006950 RID: 26960 RVA: 0x00237F14 File Offset: 0x00236114
		public override void CompTick()
		{
			base.CompTick();
			if (Find.TickManager.TicksGame > this.spawnTick + this.Props.delayTicks && !this.parent.Destroyed)
			{
				this.parent.Destroy(this.Props.destroyMode);
			}
		}

		// Token: 0x06006951 RID: 26961 RVA: 0x00237F68 File Offset: 0x00236168
		public override string CompInspectStringExtra()
		{
			if (this.Props.countdownLabel.NullOrEmpty())
			{
				return "";
			}
			int numTicks = Mathf.Max(0, this.spawnTick + this.Props.delayTicks - Find.TickManager.TicksGame);
			return this.Props.countdownLabel + ": " + numTicks.ToStringSecondsFromTicks();
		}

		// Token: 0x06006952 RID: 26962 RVA: 0x00237FCC File Offset: 0x002361CC
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.spawnTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06006953 RID: 26963 RVA: 0x00237FE8 File Offset: 0x002361E8
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.spawnTick, "spawnTick", 0, false);
		}

		// Token: 0x04003AEE RID: 15086
		public int spawnTick;
	}
}
