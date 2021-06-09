using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001850 RID: 6224
	public class CompSpawnerHives : ThingComp
	{
		// Token: 0x170015AE RID: 5550
		// (get) Token: 0x06008A19 RID: 35353 RVA: 0x0005CAE2 File Offset: 0x0005ACE2
		private CompProperties_SpawnerHives Props
		{
			get
			{
				return (CompProperties_SpawnerHives)this.props;
			}
		}

		// Token: 0x170015AF RID: 5551
		// (get) Token: 0x06008A1A RID: 35354 RVA: 0x0005CAEF File Offset: 0x0005ACEF
		private bool CanSpawnChildHive
		{
			get
			{
				return this.canSpawnHives && HiveUtility.TotalSpawnedHivesCount(this.parent.Map) < 30 && Find.Storyteller.difficultyValues.enemyReproductionRateFactor > 0f;
			}
		}

		// Token: 0x06008A1B RID: 35355 RVA: 0x0005CB25 File Offset: 0x0005AD25
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				this.CalculateNextHiveSpawnTick();
			}
		}

		// Token: 0x06008A1C RID: 35356 RVA: 0x00285D68 File Offset: 0x00283F68
		public override void CompTick()
		{
			base.CompTick();
			CompCanBeDormant comp = this.parent.GetComp<CompCanBeDormant>();
			if ((comp == null || comp.Awake) && !this.wasActivated)
			{
				this.CalculateNextHiveSpawnTick();
				this.wasActivated = true;
			}
			if ((comp == null || comp.Awake) && Find.TickManager.TicksGame >= this.nextHiveSpawnTick)
			{
				Hive t;
				if (this.TrySpawnChildHive(false, out t))
				{
					Messages.Message("MessageHiveReproduced".Translate(), t, MessageTypeDefOf.NegativeEvent, true);
					return;
				}
				this.CalculateNextHiveSpawnTick();
			}
		}

		// Token: 0x06008A1D RID: 35357 RVA: 0x00285DFC File Offset: 0x00283FFC
		public override string CompInspectStringExtra()
		{
			if (!this.canSpawnHives || Find.Storyteller.difficultyValues.enemyReproductionRateFactor <= 0f)
			{
				return "DormantHiveNotReproducing".Translate();
			}
			if (this.CanSpawnChildHive)
			{
				return "HiveReproducesIn".Translate() + ": " + (this.nextHiveSpawnTick - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true);
			}
			return null;
		}

		// Token: 0x06008A1E RID: 35358 RVA: 0x00285E7C File Offset: 0x0028407C
		public void CalculateNextHiveSpawnTick()
		{
			Room room = this.parent.GetRoom(RegionType.Set_Passable);
			int num = 0;
			int num2 = GenRadial.NumCellsInRadius(9f);
			for (int i = 0; i < num2; i++)
			{
				IntVec3 intVec = this.parent.Position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(this.parent.Map) && intVec.GetRoom(this.parent.Map, RegionType.Set_Passable) == room)
				{
					if (intVec.GetThingList(this.parent.Map).Any((Thing t) => t is Hive))
					{
						num++;
					}
				}
			}
			float num3 = this.Props.ReproduceRateFactorFromNearbyHiveCountCurve.Evaluate((float)num);
			if (Find.Storyteller.difficultyValues.enemyReproductionRateFactor > 0f)
			{
				this.nextHiveSpawnTick = Find.TickManager.TicksGame + (int)(this.Props.HiveSpawnIntervalDays.RandomInRange * 60000f / (num3 * Find.Storyteller.difficultyValues.enemyReproductionRateFactor));
				return;
			}
			this.nextHiveSpawnTick = Find.TickManager.TicksGame + (int)this.Props.HiveSpawnIntervalDays.RandomInRange * 60000;
		}

		// Token: 0x06008A1F RID: 35359 RVA: 0x00285FCC File Offset: 0x002841CC
		public bool TrySpawnChildHive(bool ignoreRoofedRequirement, out Hive newHive)
		{
			if (!this.CanSpawnChildHive)
			{
				newHive = null;
				return false;
			}
			IntVec3 loc = CompSpawnerHives.FindChildHiveLocation(this.parent.Position, this.parent.Map, this.parent.def, this.Props, ignoreRoofedRequirement, false);
			if (!loc.IsValid)
			{
				newHive = null;
				return false;
			}
			newHive = (Hive)ThingMaker.MakeThing(this.parent.def, null);
			if (newHive.Faction != this.parent.Faction)
			{
				newHive.SetFaction(this.parent.Faction, null);
			}
			Hive hive = this.parent as Hive;
			if (hive != null)
			{
				if (hive.CompDormant.Awake)
				{
					newHive.CompDormant.WakeUp();
				}
				newHive.questTags = hive.questTags;
			}
			GenSpawn.Spawn(newHive, loc, this.parent.Map, WipeMode.FullRefund);
			this.CalculateNextHiveSpawnTick();
			return true;
		}

		// Token: 0x06008A20 RID: 35360 RVA: 0x002860B4 File Offset: 0x002842B4
		public static IntVec3 FindChildHiveLocation(IntVec3 pos, Map map, ThingDef parentDef, CompProperties_SpawnerHives props, bool ignoreRoofedRequirement, bool allowUnreachable)
		{
			IntVec3 intVec = IntVec3.Invalid;
			for (int i = 0; i < 3; i++)
			{
				float minDist = props.HiveSpawnPreferredMinDist;
				bool flag;
				if (i < 2)
				{
					if (i == 1)
					{
						minDist = 0f;
					}
					flag = CellFinder.TryFindRandomReachableCellNear(pos, map, props.HiveSpawnRadius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), (IntVec3 c) => CompSpawnerHives.CanSpawnHiveAt(c, map, pos, parentDef, minDist, ignoreRoofedRequirement), null, out intVec, 999999);
				}
				else
				{
					flag = (allowUnreachable && CellFinder.TryFindRandomCellNear(pos, map, (int)props.HiveSpawnRadius, (IntVec3 c) => CompSpawnerHives.CanSpawnHiveAt(c, map, pos, parentDef, minDist, ignoreRoofedRequirement), out intVec, -1));
				}
				if (flag)
				{
					intVec = CellFinder.FindNoWipeSpawnLocNear(intVec, map, parentDef, Rot4.North, 2, (IntVec3 c) => CompSpawnerHives.CanSpawnHiveAt(c, map, pos, parentDef, minDist, ignoreRoofedRequirement));
					break;
				}
			}
			return intVec;
		}

		// Token: 0x06008A21 RID: 35361 RVA: 0x002861DC File Offset: 0x002843DC
		private static bool CanSpawnHiveAt(IntVec3 c, Map map, IntVec3 parentPos, ThingDef parentDef, float minDist, bool ignoreRoofedRequirement)
		{
			if ((!ignoreRoofedRequirement && !c.Roofed(map)) || (!c.Walkable(map) || (minDist != 0f && (float)c.DistanceToSquared(parentPos) < minDist * minDist)) || c.GetFirstThing(map, ThingDefOf.InsectJelly) != null || c.GetFirstThing(map, ThingDefOf.GlowPod) != null)
			{
				return false;
			}
			for (int i = 0; i < 9; i++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCellsAndInside[i];
				if (c2.InBounds(map))
				{
					List<Thing> thingList = c2.GetThingList(map);
					for (int j = 0; j < thingList.Count; j++)
					{
						if (thingList[j] is Hive || thingList[j] is TunnelHiveSpawner)
						{
							return false;
						}
					}
				}
			}
			List<Thing> thingList2 = c.GetThingList(map);
			for (int k = 0; k < thingList2.Count; k++)
			{
				Thing thing = thingList2[k];
				if (thing.def.category == ThingCategory.Building && thing.def.passability == Traversability.Impassable && GenSpawn.SpawningWipes(parentDef, thing.def))
				{
					return true;
				}
			}
			return true;
		}

		// Token: 0x06008A22 RID: 35362 RVA: 0x0005CB30 File Offset: 0x0005AD30
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "Dev: Reproduce",
					icon = TexCommand.GatherSpotActive,
					action = delegate()
					{
						Hive hive;
						this.TrySpawnChildHive(false, out hive);
					}
				};
			}
			yield break;
		}

		// Token: 0x06008A23 RID: 35363 RVA: 0x0005CB40 File Offset: 0x0005AD40
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.nextHiveSpawnTick, "nextHiveSpawnTick", 0, false);
			Scribe_Values.Look<bool>(ref this.canSpawnHives, "canSpawnHives", true, false);
			Scribe_Values.Look<bool>(ref this.wasActivated, "wasActivated", true, false);
		}

		// Token: 0x04005891 RID: 22673
		private int nextHiveSpawnTick = -1;

		// Token: 0x04005892 RID: 22674
		public bool canSpawnHives = true;

		// Token: 0x04005893 RID: 22675
		private bool wasActivated;

		// Token: 0x04005894 RID: 22676
		public const int MaxHivesPerMap = 30;
	}
}
