using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200184D RID: 6221
	public class CompSpawnerFilth : ThingComp
	{
		// Token: 0x170015AC RID: 5548
		// (get) Token: 0x06008A0B RID: 35339 RVA: 0x0005CA2B File Offset: 0x0005AC2B
		private CompProperties_SpawnerFilth Props
		{
			get
			{
				return (CompProperties_SpawnerFilth)this.props;
			}
		}

		// Token: 0x170015AD RID: 5549
		// (get) Token: 0x06008A0C RID: 35340 RVA: 0x00285B90 File Offset: 0x00283D90
		private bool CanSpawnFilth
		{
			get
			{
				Hive hive = this.parent as Hive;
				if (hive != null && !hive.CompDormant.Awake)
				{
					return false;
				}
				if (this.Props.requiredRotStage != null)
				{
					RotStage rotStage = this.parent.GetRotStage();
					RotStage? requiredRotStage = this.Props.requiredRotStage;
					if (!(rotStage == requiredRotStage.GetValueOrDefault() & requiredRotStage != null))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x06008A0D RID: 35341 RVA: 0x0005CA38 File Offset: 0x0005AC38
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.nextSpawnTimestamp, "nextSpawnTimestamp", -1, false);
		}

		// Token: 0x06008A0E RID: 35342 RVA: 0x00285BFC File Offset: 0x00283DFC
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				for (int i = 0; i < this.Props.spawnCountOnSpawn; i++)
				{
					this.TrySpawnFilth();
				}
			}
		}

		// Token: 0x06008A0F RID: 35343 RVA: 0x0005CA52 File Offset: 0x0005AC52
		public override void CompTick()
		{
			base.CompTick();
			this.TickInterval(1);
		}

		// Token: 0x06008A10 RID: 35344 RVA: 0x0005CA61 File Offset: 0x0005AC61
		public override void CompTickRare()
		{
			base.CompTickRare();
			this.TickInterval(250);
		}

		// Token: 0x06008A11 RID: 35345 RVA: 0x00285C28 File Offset: 0x00283E28
		private void TickInterval(int interval)
		{
			if (this.CanSpawnFilth)
			{
				if (this.Props.spawnMtbHours > 0f && Rand.MTBEventOccurs(this.Props.spawnMtbHours, 2500f, (float)interval))
				{
					this.TrySpawnFilth();
				}
				if (this.Props.spawnEveryDays >= 0f && Find.TickManager.TicksGame >= this.nextSpawnTimestamp)
				{
					if (this.nextSpawnTimestamp != -1)
					{
						this.TrySpawnFilth();
					}
					this.nextSpawnTimestamp = Find.TickManager.TicksGame + (int)(this.Props.spawnEveryDays * 60000f);
				}
			}
		}

		// Token: 0x06008A12 RID: 35346 RVA: 0x00285CC8 File Offset: 0x00283EC8
		public void TrySpawnFilth()
		{
			if (this.parent.Map == null)
			{
				return;
			}
			IntVec3 c;
			if (!CellFinder.TryFindRandomReachableCellNear(this.parent.Position, this.parent.Map, this.Props.spawnRadius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), (IntVec3 x) => x.Standable(this.parent.Map), (Region x) => true, out c, 999999))
			{
				return;
			}
			FilthMaker.TryMakeFilth(c, this.parent.Map, this.Props.filthDef, 1, FilthSourceFlags.None);
		}

		// Token: 0x04005888 RID: 22664
		private int nextSpawnTimestamp = -1;
	}
}
