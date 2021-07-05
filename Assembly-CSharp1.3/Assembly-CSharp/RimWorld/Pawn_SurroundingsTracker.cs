using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001520 RID: 5408
	public class Pawn_SurroundingsTracker : IExposable
	{
		// Token: 0x060080A3 RID: 32931 RVA: 0x002D91E2 File Offset: 0x002D73E2
		public Pawn_SurroundingsTracker()
		{
		}

		// Token: 0x060080A4 RID: 32932 RVA: 0x002D9216 File Offset: 0x002D7416
		public Pawn_SurroundingsTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060080A5 RID: 32933 RVA: 0x002D9254 File Offset: 0x002D7454
		private int NumSightingsInRange(ref List<TreeSighting> sightings, int ticks)
		{
			int num = 0;
			for (int i = 0; i < sightings.Count; i++)
			{
				if (sightings[i].TicksSinceSighting <= ticks)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060080A6 RID: 32934 RVA: 0x002D928C File Offset: 0x002D748C
		public int NumSightingsInRange(TreeCategory treeCategory, int ticks)
		{
			switch (treeCategory)
			{
			case TreeCategory.Mini:
				return this.NumSightingsInRange(ref this.miniTreeSightings, ticks);
			case TreeCategory.Full:
				return this.NumSightingsInRange(ref this.fullTreeSightings, ticks);
			case TreeCategory.Super:
				return this.NumSightingsInRange(ref this.superTreeSightings, ticks);
			default:
				return 0;
			}
		}

		// Token: 0x060080A7 RID: 32935 RVA: 0x002D92DA File Offset: 0x002D74DA
		public int NumSkullspikeSightings()
		{
			return this.skullspikeSightings.Count;
		}

		// Token: 0x060080A8 RID: 32936 RVA: 0x002D92E8 File Offset: 0x002D74E8
		public void SurroundingsTrackerTick()
		{
			if (this.pawn.IsHashIntervalTick(2500) && ModsConfig.IdeologyActive && this.pawn.Ideo != null && this.pawn.Ideo.cachedPossibleSituationalThoughts.Contains(ThoughtDefOf.TreesDesired) && this.pawn.Awake() && this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
			{
				if (this.pawn.Spawned)
				{
					this.GetSpawnedTreeSightings();
				}
				else
				{
					this.GetCaravanTreeSightings();
				}
			}
			if (this.pawn.IsHashIntervalTick(60) && ModsConfig.IdeologyActive && this.pawn.Ideo != null && this.pawn.Spawned && this.pawn.Awake() && (this.pawn.Ideo.cachedPossibleSituationalThoughts.Contains(ThoughtDefOf.Skullspike_Desired) || this.pawn.Ideo.cachedPossibleSituationalThoughts.Contains(ThoughtDefOf.Skullspike_Disapproved)) && this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
			{
				for (int i = this.skullspikeSightings.Count - 1; i >= 0; i--)
				{
					if (this.skullspikeSightings[i].TicksSinceSighting > 1800)
					{
						this.skullspikeSightings.RemoveAt(i);
					}
				}
				if (this.pawn.Map.listerThings.ThingsOfDef(ThingDefOf.Skullspike).Count != 0)
				{
					IntVec3 positionHeld = this.pawn.PositionHeld;
					int num = GenRadial.NumCellsInRadius(10f);
					for (int j = 0; j < num; j++)
					{
						IntVec3 c = positionHeld + GenRadial.RadialPattern[j];
						if (c.InBounds(this.pawn.Map) && !c.Fogged(this.pawn.Map))
						{
							Thing firstThing = c.GetFirstThing(this.pawn.Map, ThingDefOf.Skullspike);
							if (firstThing != null && GenSight.LineOfSight(this.pawn.PositionHeld, firstThing.PositionHeld, firstThing.Map, true, null, 0, 0))
							{
								bool flag = false;
								for (int k = 0; k < this.skullspikeSightings.Count; k++)
								{
									if (this.skullspikeSightings[k].skullspike == firstThing)
									{
										SkullspikeSighting value = this.skullspikeSightings[k];
										value.tickSighted = Find.TickManager.TicksGame;
										this.skullspikeSightings[k] = value;
										flag = true;
										break;
									}
								}
								if (!flag)
								{
									this.skullspikeSightings.Add(new SkullspikeSighting
									{
										skullspike = firstThing,
										tickSighted = Find.TickManager.TicksGame
									});
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060080A9 RID: 32937 RVA: 0x002D95D4 File Offset: 0x002D77D4
		private void GetSpawnedTreeSightings()
		{
			foreach (TreeSighting treeSighting in IdeoUtility.TreeSightingsNearPawn(this.pawn.Position, this.pawn.Map, this.pawn.Ideo))
			{
				if (treeSighting.Tree != null)
				{
					TreeCategory treeCategory = treeSighting.Tree.def.plant.treeCategory;
				}
				switch (treeSighting.Tree.def.plant.treeCategory)
				{
				case TreeCategory.Mini:
					this.AddSighting(ref this.miniTreeSightings, treeSighting);
					break;
				case TreeCategory.Full:
					this.AddSighting(ref this.fullTreeSightings, treeSighting);
					break;
				case TreeCategory.Super:
					this.AddSighting(ref this.superTreeSightings, treeSighting);
					break;
				}
			}
		}

		// Token: 0x060080AA RID: 32938 RVA: 0x002D96B8 File Offset: 0x002D78B8
		private void GetCaravanTreeSightings()
		{
			Caravan caravan = this.pawn.GetCaravan();
			if (caravan == null)
			{
				return;
			}
			int treeSightingsPerHourFromCaravan = caravan.Biome.TreeSightingsPerHourFromCaravan;
			for (int i = 0; i < treeSightingsPerHourFromCaravan; i++)
			{
				this.AddSighting(ref this.fullTreeSightings, new TreeSighting(null, Find.TickManager.TicksGame));
			}
		}

		// Token: 0x060080AB RID: 32939 RVA: 0x002D970C File Offset: 0x002D790C
		private void AddSighting(ref List<TreeSighting> list, TreeSighting newSighting)
		{
			if (newSighting.tree != null)
			{
				for (int i = list.Count - 1; i >= 0; i--)
				{
					if (list[i].tree != null && list[i].tree == newSighting.tree)
					{
						list.RemoveAt(i);
					}
				}
			}
			list.Add(newSighting);
			for (int j = 0; j < list.Count - 15; j++)
			{
				list.RemoveAt(0);
			}
		}

		// Token: 0x060080AC RID: 32940 RVA: 0x002D9786 File Offset: 0x002D7986
		public void Clear()
		{
			this.miniTreeSightings.Clear();
			this.fullTreeSightings.Clear();
			this.superTreeSightings.Clear();
		}

		// Token: 0x060080AD RID: 32941 RVA: 0x002D97AC File Offset: 0x002D79AC
		public void ExposeData()
		{
			Scribe_Collections.Look<TreeSighting>(ref this.miniTreeSightings, "miniTreeSightings", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<TreeSighting>(ref this.fullTreeSightings, "fullTreeSightings", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<TreeSighting>(ref this.superTreeSightings, "superTreeSightings", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x0400501D RID: 20509
		public Pawn pawn;

		// Token: 0x0400501E RID: 20510
		public List<TreeSighting> miniTreeSightings = new List<TreeSighting>();

		// Token: 0x0400501F RID: 20511
		public List<TreeSighting> fullTreeSightings = new List<TreeSighting>();

		// Token: 0x04005020 RID: 20512
		public List<TreeSighting> superTreeSightings = new List<TreeSighting>();

		// Token: 0x04005021 RID: 20513
		public List<SkullspikeSighting> skullspikeSightings = new List<SkullspikeSighting>();

		// Token: 0x04005022 RID: 20514
		private const int MaxSightingsPerCategory = 15;

		// Token: 0x04005023 RID: 20515
		private const int TreeCheckInterval = 2500;
	}
}
