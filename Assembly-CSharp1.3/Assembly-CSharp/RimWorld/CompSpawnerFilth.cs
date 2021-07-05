using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011A0 RID: 4512
	public class CompSpawnerFilth : ThingComp
	{
		// Token: 0x170012D1 RID: 4817
		// (get) Token: 0x06006CA9 RID: 27817 RVA: 0x00247A8A File Offset: 0x00245C8A
		private CompProperties_SpawnerFilth Props
		{
			get
			{
				return (CompProperties_SpawnerFilth)this.props;
			}
		}

		// Token: 0x170012D2 RID: 4818
		// (get) Token: 0x06006CAA RID: 27818 RVA: 0x00247A98 File Offset: 0x00245C98
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

		// Token: 0x06006CAB RID: 27819 RVA: 0x00247B02 File Offset: 0x00245D02
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.nextSpawnTimestamp, "nextSpawnTimestamp", -1, false);
		}

		// Token: 0x06006CAC RID: 27820 RVA: 0x00247B1C File Offset: 0x00245D1C
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

		// Token: 0x06006CAD RID: 27821 RVA: 0x00247B48 File Offset: 0x00245D48
		public override void CompTick()
		{
			base.CompTick();
			this.TickInterval(1);
		}

		// Token: 0x06006CAE RID: 27822 RVA: 0x00247B57 File Offset: 0x00245D57
		public override void CompTickRare()
		{
			base.CompTickRare();
			this.TickInterval(250);
		}

		// Token: 0x06006CAF RID: 27823 RVA: 0x00247B6C File Offset: 0x00245D6C
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

		// Token: 0x06006CB0 RID: 27824 RVA: 0x00247C0C File Offset: 0x00245E0C
		public void TrySpawnFilth()
		{
			if (this.parent.Map == null)
			{
				return;
			}
			IntVec3 c;
			if (!CellFinder.TryFindRandomReachableCellNear(this.parent.Position, this.parent.Map, this.Props.spawnRadius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false), (IntVec3 x) => x.Standable(this.parent.Map), (Region x) => true, out c, 999999))
			{
				return;
			}
			FilthMaker.TryMakeFilth(c, this.parent.Map, this.Props.filthDef, 1, FilthSourceFlags.None);
		}

		// Token: 0x04003C69 RID: 15465
		private int nextSpawnTimestamp = -1;
	}
}
