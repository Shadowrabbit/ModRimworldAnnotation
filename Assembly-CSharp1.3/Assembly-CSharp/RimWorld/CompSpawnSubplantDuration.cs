using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200119D RID: 4509
	public class CompSpawnSubplantDuration : ThingComp
	{
		// Token: 0x170012CE RID: 4814
		// (get) Token: 0x06006C92 RID: 27794 RVA: 0x00246D86 File Offset: 0x00244F86
		public CompProperties_SpawnSubplant Props
		{
			get
			{
				return (CompProperties_SpawnSubplant)this.props;
			}
		}

		// Token: 0x06006C93 RID: 27795 RVA: 0x00247291 File Offset: 0x00245491
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				this.SetupNextSubplantTick();
			}
		}

		// Token: 0x06006C94 RID: 27796 RVA: 0x0024729C File Offset: 0x0024549C
		public override void CompTick()
		{
			if (Find.TickManager.TicksGame >= this.nextSubplantTick)
			{
				this.DoGrowSubplant(false);
				this.SetupNextSubplantTick();
			}
		}

		// Token: 0x06006C95 RID: 27797 RVA: 0x002472BD File Offset: 0x002454BD
		public void SetupNextSubplantTick()
		{
			this.nextSubplantTick = Find.TickManager.TicksGame + (int)(60000f * this.Props.subplantSpawnDays);
		}

		// Token: 0x06006C96 RID: 27798 RVA: 0x002472E4 File Offset: 0x002454E4
		public void DoGrowSubplant(bool force = false)
		{
			if (!ModLister.CheckIdeology("Subplant duration spawning"))
			{
				return;
			}
			if (!force && ((Plant)this.parent).Growth < this.Props.minGrowthForSpawn)
			{
				return;
			}
			IntVec3 position = this.parent.Position;
			int num = GenRadial.NumCellsInRadius(this.Props.maxRadius);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(this.parent.Map) && WanderUtility.InSameRoom(position, intVec, this.parent.Map))
				{
					bool flag = false;
					List<Thing> thingList = intVec.GetThingList(this.parent.Map);
					using (List<Thing>.Enumerator enumerator = thingList.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.def == this.Props.subplant)
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						if (!this.Props.canSpawnOverPlayerSownPlants)
						{
							Plant plant = intVec.GetPlant(this.parent.Map);
							Zone zone = this.parent.Map.zoneManager.ZoneAt(intVec);
							if (plant != null && plant.sown && zone != null && zone is Zone_Growing)
							{
								goto IL_1FB;
							}
						}
						if (this.Props.subplant.CanEverPlantAt(intVec, this.parent.Map, true))
						{
							for (int j = thingList.Count - 1; j >= 0; j--)
							{
								if (thingList[j].def.category == ThingCategory.Plant)
								{
									thingList[j].Destroy(DestroyMode.Vanish);
								}
							}
							Plant plant2 = (Plant)GenSpawn.Spawn(this.Props.subplant, intVec, this.parent.Map, WipeMode.Vanish);
							if (this.Props.initialGrowthRange != null)
							{
								plant2.Growth = this.Props.initialGrowthRange.Value.RandomInRange;
							}
							return;
						}
					}
				}
				IL_1FB:;
			}
		}

		// Token: 0x06006C97 RID: 27799 RVA: 0x00247508 File Offset: 0x00245708
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.nextSubplantTick, "nextSubplantTick", 0, false);
		}

		// Token: 0x04003C5D RID: 15453
		private int nextSubplantTick;
	}
}
