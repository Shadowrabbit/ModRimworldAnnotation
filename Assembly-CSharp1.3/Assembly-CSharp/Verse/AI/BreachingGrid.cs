using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;
using Verse.Noise;

namespace Verse.AI
{
	// Token: 0x0200065A RID: 1626
	public class BreachingGrid : IExposable
	{
		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x06002DE6 RID: 11750 RVA: 0x00113266 File Offset: 0x00111466
		public BoolGrid WalkGrid
		{
			get
			{
				return this.walkGrid;
			}
		}

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x06002DE7 RID: 11751 RVA: 0x0011326E File Offset: 0x0011146E
		public BoolGrid BreachGrid
		{
			get
			{
				return this.breachGrid;
			}
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x06002DE8 RID: 11752 RVA: 0x00113276 File Offset: 0x00111476
		public ByteGrid MarkerGrid
		{
			get
			{
				this.RegenerateCachedGridIfDirty();
				return this.markerGrid;
			}
		}

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x06002DE9 RID: 11753 RVA: 0x00113284 File Offset: 0x00111484
		public BoolGrid ReachableGrid
		{
			get
			{
				this.RegenerateCachedGridIfDirty();
				return this.reachableGrid;
			}
		}

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x06002DEA RID: 11754 RVA: 0x00113292 File Offset: 0x00111492
		public Map Map
		{
			get
			{
				return this.map;
			}
		}

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x06002DEB RID: 11755 RVA: 0x0011329A File Offset: 0x0011149A
		public int BreachRadius
		{
			get
			{
				return this.breachRadius;
			}
		}

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x06002DEC RID: 11756 RVA: 0x001132A2 File Offset: 0x001114A2
		public Perlin Noise
		{
			get
			{
				if (this.perlinCached == null)
				{
					this.perlinCached = BreachingGrid.CreatePerlinNoise(this.perlinSeed);
				}
				return this.perlinCached;
			}
		}

		// Token: 0x06002DED RID: 11757 RVA: 0x001132C3 File Offset: 0x001114C3
		public BreachingGrid()
		{
		}

		// Token: 0x06002DEE RID: 11758 RVA: 0x001132E4 File Offset: 0x001114E4
		public BreachingGrid(Map map, Lord lord)
		{
			this.map = map;
			this.lord = lord;
			this.walkGrid = new BoolGrid(map);
			this.breachGrid = new BoolGrid(map);
			this.perlinSeed = Rand.Int;
		}

		// Token: 0x06002DEF RID: 11759 RVA: 0x00113341 File Offset: 0x00111541
		public static Perlin CreatePerlinNoise(int seed)
		{
			return new Perlin((double)BreachingGrid.perlinFrequency, (double)BreachingGrid.perlinLacunarity, (double)BreachingGrid.perlinPersistence, (int)BreachingGrid.perlinOctaves, seed, QualityMode.Medium);
		}

		// Token: 0x06002DF0 RID: 11760 RVA: 0x00113362 File Offset: 0x00111562
		public void Notify_PawnStateChanged(Pawn pawn)
		{
			this.cachedGridsDirty = true;
		}

		// Token: 0x06002DF1 RID: 11761 RVA: 0x00113362 File Offset: 0x00111562
		public void Notify_BuildingStateChanged(Building b)
		{
			this.cachedGridsDirty = true;
		}

		// Token: 0x06002DF2 RID: 11762 RVA: 0x0011336B File Offset: 0x0011156B
		public bool WithinNoise(IntVec3 cell)
		{
			return this.Noise.GetValue(cell) >= BreachingGrid.perlinThres;
		}

		// Token: 0x06002DF3 RID: 11763 RVA: 0x00113384 File Offset: 0x00111584
		public void CreateBreachPath(IntVec3 start, IntVec3 end, int breachRadius, int walkMargin, bool useAvoidGrid = false)
		{
			if (!ModLister.CheckIdeology("Breach raid"))
			{
				return;
			}
			this.breachRadius = breachRadius;
			this.breachStart = start;
			this.SetupCostOffsets();
			PathFinderCostTuning pathFinderCostTuning = new PathFinderCostTuning();
			pathFinderCostTuning.costBlockedDoor = BreachingGrid.tweakWallCost;
			pathFinderCostTuning.costBlockedWallBase = BreachingGrid.tweakWallCost;
			pathFinderCostTuning.costBlockedDoorPerHitPoint = BreachingGrid.tweakWallHpCost;
			pathFinderCostTuning.costBlockedWallExtraPerHitPoint = BreachingGrid.tweakWallHpCost;
			pathFinderCostTuning.costOffLordWalkGrid = BreachingGrid.tweakOffWalkGridPathCost;
			pathFinderCostTuning.custom = new BreachingGrid.CustomTuning(breachRadius, this, pathFinderCostTuning);
			TraverseParms traverseParms = TraverseParms.For(TraverseMode.PassAllDestroyableThings, Danger.Deadly, false, useAvoidGrid, false);
			using (PawnPath pawnPath = this.map.pathFinder.FindPath(start, end, traverseParms, PathEndMode.OnCell, pathFinderCostTuning))
			{
				foreach (IntVec3 c in pawnPath.NodesReversed)
				{
					this.breachGrid[c] = true;
					this.walkGrid[c] = true;
				}
			}
			for (int i = 0; i < breachRadius; i++)
			{
				this.WidenGrid(this.breachGrid);
				this.WidenGrid(this.walkGrid);
			}
			for (int j = 0; j < walkMargin; j++)
			{
				this.WidenGrid(this.walkGrid);
			}
		}

		// Token: 0x06002DF4 RID: 11764 RVA: 0x001134DC File Offset: 0x001116DC
		public Thing FindBuildingToBreach()
		{
			if (!ModLister.CheckIdeology("Breach raid"))
			{
				return null;
			}
			Building bestBuilding = null;
			int bestBuildingDist = int.MaxValue;
			int bestBuildingReachableSideCount = 0;
			this.RegenerateCachedGridIfDirty();
			this.Map.floodFiller.FloodFill(this.breachStart, (IntVec3 c) => this.BreachGrid[c], delegate(IntVec3 c, int dist)
			{
				List<Thing> thingList = c.GetThingList(this.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Building building;
					if ((building = (thingList[i] as Building)) != null && BreachingUtility.ShouldBreachBuilding(building) && BreachingUtility.IsWorthBreachingBuilding(this, building))
					{
						int num = BreachingUtility.CountReachableAdjacentCells(this, building);
						if (num > 0 && num > bestBuildingReachableSideCount)
						{
							bestBuilding = building;
							bestBuildingDist = dist;
							bestBuildingReachableSideCount = num;
							break;
						}
					}
				}
				return dist - 2 > bestBuildingDist;
			}, int.MaxValue, false, null);
			return bestBuilding;
		}

		// Token: 0x06002DF5 RID: 11765 RVA: 0x00113560 File Offset: 0x00111760
		private void WidenGrid(BoolGrid grid)
		{
			BreachingGrid.tmpWidenGrid.ClearAndResizeTo(this.map);
			foreach (IntVec3 a in grid.ActiveCells)
			{
				for (int i = 0; i < 8; i++)
				{
					IntVec3 c = a + GenAdj.AdjacentCells[i];
					if (c.InBounds(this.map))
					{
						BreachingGrid.tmpWidenGrid[c] = true;
					}
				}
			}
			foreach (IntVec3 c2 in BreachingGrid.tmpWidenGrid.ActiveCells)
			{
				grid[c2] = true;
			}
		}

		// Token: 0x06002DF6 RID: 11766 RVA: 0x00113634 File Offset: 0x00111834
		public void Reset()
		{
			this.breachGrid.Clear();
			this.walkGrid.Clear();
			IntGrid intGrid = this.cellCostOffset;
			if (intGrid != null)
			{
				intGrid.Clear(0);
			}
			ByteGrid byteGrid = this.markerGrid;
			if (byteGrid != null)
			{
				byteGrid.Clear(0);
			}
			BoolGrid boolGrid = this.reachableGrid;
			if (boolGrid != null)
			{
				boolGrid.Clear();
			}
			this.cachedGridsDirty = true;
			BreachingGridDebug.ClearDebugPath();
		}

		// Token: 0x06002DF7 RID: 11767 RVA: 0x00113698 File Offset: 0x00111898
		private void RegenerateCachedGridIfDirty()
		{
			if (this.cachedGridsDirty)
			{
				this.RegenerateCachedGrid();
			}
		}

		// Token: 0x06002DF8 RID: 11768 RVA: 0x001136A8 File Offset: 0x001118A8
		private void RegenerateCachedGrid()
		{
			this.cachedGridsDirty = false;
			if (this.markerGrid == null)
			{
				this.markerGrid = new ByteGrid(this.Map);
			}
			else
			{
				this.markerGrid.Clear(0);
			}
			if (this.reachableGrid == null)
			{
				this.reachableGrid = new BoolGrid(this.Map);
			}
			else
			{
				this.reachableGrid.Clear();
			}
			BreachingGrid.cachedWalkReachabilityPainter.PaintWalkReachability(this);
			if (this.lord != null)
			{
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					Pawn pawn = this.lord.ownedPawns[i];
					if (pawn.mindState.breachingTarget != null && !pawn.mindState.breachingTarget.target.Destroyed)
					{
						this.PaintDangerFromPawn(pawn);
					}
				}
			}
		}

		// Token: 0x06002DF9 RID: 11769 RVA: 0x00113774 File Offset: 0x00111974
		private void PaintDangerFromPawn(Pawn pawn)
		{
			BreachingTargetData breachingTarget = pawn.mindState.breachingTarget;
			if (breachingTarget == null)
			{
				return;
			}
			IntVec3 position = breachingTarget.target.Position;
			if (!position.IsValid)
			{
				return;
			}
			Verb verb = BreachingUtility.FindVerbToUseForBreaching(pawn);
			if (verb != null)
			{
				IntVec3 firingPosition = breachingTarget.firingPosition;
				if (firingPosition.IsValid)
				{
					if (this.markerGrid[firingPosition] == 0)
					{
						this.markerGrid[firingPosition] = 180;
					}
					this.VisitDangerousCellsOfAttack(firingPosition, position, verb, new Action<IntVec3>(this.<PaintDangerFromPawn>g__DangerSetter|56_0));
				}
			}
		}

		// Token: 0x06002DFA RID: 11770 RVA: 0x001137F5 File Offset: 0x001119F5
		public void VisitDangerousCellsOfAttack(IntVec3 firingPosition, IntVec3 targetPosition, Verb verb, Action<IntVec3> visitor)
		{
			if (!verb.IsMeleeAttack)
			{
				BreachingGrid.cachedDangerLineOfSightPainter.PaintLoS(this.map, firingPosition, targetPosition, visitor);
				this.PaintSplashDamage(verb, targetPosition, visitor);
			}
		}

		// Token: 0x06002DFB RID: 11771 RVA: 0x00113820 File Offset: 0x00111A20
		private void PaintSplashDamage(Verb verb, IntVec3 center, Action<IntVec3> visitor)
		{
			float num = 2f;
			ThingDef projectile = verb.GetProjectile();
			if (projectile != null && projectile.projectile.explosionRadius > 0f)
			{
				num = Mathf.Max(num, projectile.projectile.explosionRadius);
			}
			int num2 = GenRadial.NumCellsInRadius(num);
			for (int i = 0; i < num2; i++)
			{
				IntVec3 obj = (center + GenRadial.RadialPattern[i]).ClampInsideMap(this.map);
				visitor(obj);
			}
		}

		// Token: 0x06002DFC RID: 11772 RVA: 0x001138A0 File Offset: 0x00111AA0
		private void SetupCostOffsets()
		{
			if (this.cellCostOffset == null)
			{
				this.cellCostOffset = new IntGrid(this.map);
			}
			this.cellCostOffset.Clear(0);
			if (!BreachingGrid.tweakAvoidDangerousRooms)
			{
				return;
			}
			foreach (Room room in this.map.regionGrid.allRooms)
			{
				int num = this.DangerousRoomCost(room);
				if (num != 0)
				{
					foreach (IntVec3 c in room.Cells)
					{
						this.cellCostOffset[c] = num;
					}
					foreach (IntVec3 c2 in room.BorderCells)
					{
						this.cellCostOffset[c2] = num;
					}
				}
			}
		}

		// Token: 0x06002DFD RID: 11773 RVA: 0x001139BC File Offset: 0x00111BBC
		private int DangerousRoomCost(Room room)
		{
			if (!room.Fogged)
			{
				return 0;
			}
			foreach (Thing thing in room.ContainedAndAdjacentThings)
			{
				Pawn pawn;
				if ((pawn = (thing as Pawn)) != null && pawn.mindState != null && !pawn.mindState.Active)
				{
					return 600;
				}
				if (thing.def == ThingDefOf.Hive)
				{
					return 600;
				}
				if (thing.def == ThingDefOf.AncientCryptosleepCasket)
				{
					return 600;
				}
			}
			return 0;
		}

		// Token: 0x06002DFE RID: 11774 RVA: 0x00113A68 File Offset: 0x00111C68
		public void ExposeData()
		{
			Scribe_Deep.Look<BoolGrid>(ref this.walkGrid, "walkGrid", Array.Empty<object>());
			Scribe_Deep.Look<BoolGrid>(ref this.breachGrid, "breachGrid", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.perlinSeed, "perlinSeed", 0, false);
			Scribe_Values.Look<int>(ref this.breachRadius, "breachRadius", 0, false);
			Scribe_Values.Look<IntVec3>(ref this.breachStart, "breachStart", default(IntVec3), false);
			Scribe_References.Look<Map>(ref this.map, "map", false);
			Scribe_References.Look<Lord>(ref this.lord, "lord", false);
		}

		// Token: 0x06002E00 RID: 11776 RVA: 0x00113B8B File Offset: 0x00111D8B
		[CompilerGenerated]
		private void <PaintDangerFromPawn>g__DangerSetter|56_0(IntVec3 cell)
		{
			this.markerGrid[cell] = 10;
		}

		// Token: 0x04001C33 RID: 7219
		public const byte Marker_FiringPosition = 180;

		// Token: 0x04001C34 RID: 7220
		public const byte Marker_Dangerous = 10;

		// Token: 0x04001C35 RID: 7221
		public const byte Marker_UnUsed = 0;

		// Token: 0x04001C36 RID: 7222
		private const int DangerousRoomPathCost = 600;

		// Token: 0x04001C37 RID: 7223
		private static BreachingGrid.WalkReachabilityPainter cachedWalkReachabilityPainter = new BreachingGrid.WalkReachabilityPainter();

		// Token: 0x04001C38 RID: 7224
		private static BreachingGrid.DangerLineOfSightPainter cachedDangerLineOfSightPainter = new BreachingGrid.DangerLineOfSightPainter();

		// Token: 0x04001C39 RID: 7225
		private BoolGrid walkGrid;

		// Token: 0x04001C3A RID: 7226
		private BoolGrid breachGrid;

		// Token: 0x04001C3B RID: 7227
		private int perlinSeed;

		// Token: 0x04001C3C RID: 7228
		private int breachRadius = 1;

		// Token: 0x04001C3D RID: 7229
		private IntVec3 breachStart = IntVec3.Invalid;

		// Token: 0x04001C3E RID: 7230
		private Map map;

		// Token: 0x04001C3F RID: 7231
		private Lord lord;

		// Token: 0x04001C40 RID: 7232
		private Perlin perlinCached;

		// Token: 0x04001C41 RID: 7233
		private BoolGrid reachableGrid;

		// Token: 0x04001C42 RID: 7234
		private ByteGrid markerGrid;

		// Token: 0x04001C43 RID: 7235
		private bool cachedGridsDirty = true;

		// Token: 0x04001C44 RID: 7236
		private IntGrid cellCostOffset;

		// Token: 0x04001C45 RID: 7237
		[TweakValue("Breaching", 0f, 70f)]
		private static int tweakWallCost = 10;

		// Token: 0x04001C46 RID: 7238
		[TweakValue("Breaching", 0f, 1f)]
		private static float tweakWallHpCost = 0.01f;

		// Token: 0x04001C47 RID: 7239
		[TweakValue("Breaching", 0f, 100f)]
		private static bool tweakUsePerlin = true;

		// Token: 0x04001C48 RID: 7240
		[TweakValue("Breaching", -70f, 70f)]
		private static int tweakPerlinCost = 30;

		// Token: 0x04001C49 RID: 7241
		[TweakValue("Breaching", 1f, 7f)]
		public static int tweakOffWalkGridPathCost = 140;

		// Token: 0x04001C4A RID: 7242
		[TweakValue("Breaching", 0f, 100f)]
		private static bool tweakAvoidDangerousRooms = true;

		// Token: 0x04001C4B RID: 7243
		[TweakValue("Breaching", 0f, 0.1f)]
		private static float perlinFrequency = 0.06581f;

		// Token: 0x04001C4C RID: 7244
		[TweakValue("Breaching", 1f, 2f)]
		private static float perlinLacunarity = 1.5516f;

		// Token: 0x04001C4D RID: 7245
		[TweakValue("Breaching", 0f, 2f)]
		private static float perlinPersistence = 1.6569f;

		// Token: 0x04001C4E RID: 7246
		[TweakValue("Breaching", 1f, 5f)]
		private static float perlinOctaves = 4f;

		// Token: 0x04001C4F RID: 7247
		[TweakValue("Breaching", 0f, 1f)]
		private static float perlinThres = 0.5f;

		// Token: 0x04001C50 RID: 7248
		private static BoolGrid tmpWidenGrid = new BoolGrid();

		// Token: 0x02001DD1 RID: 7633
		private class CustomTuning : PathFinderCostTuning.ICustomizer
		{
			// Token: 0x0600ABE9 RID: 44009 RVA: 0x0039259B File Offset: 0x0039079B
			public CustomTuning(int breachRadius, BreachingGrid grid, PathFinderCostTuning tuning)
			{
				this.breachRadius = breachRadius;
				this.grid = grid;
				this.tuning = tuning;
			}

			// Token: 0x0600ABEA RID: 44010 RVA: 0x003925B8 File Offset: 0x003907B8
			public int CostOffset(IntVec3 from, IntVec3 to)
			{
				IntVec3 a = (to - from).RotatedBy(Rot4.East);
				int num = 0;
				for (int i = -this.breachRadius; i <= this.breachRadius; i++)
				{
					IntVec3 intVec = to + a * i;
					if (intVec.InBounds(this.grid.Map) && i != 0)
					{
						num += this.CostOffAdjacent(intVec) + this.grid.cellCostOffset[intVec];
					}
				}
				if (BreachingGrid.tweakUsePerlin && this.grid.WithinNoise(to))
				{
					num += BreachingGrid.tweakPerlinCost;
				}
				return num;
			}

			// Token: 0x0600ABEB RID: 44011 RVA: 0x00392650 File Offset: 0x00390850
			private int CostOffAdjacent(IntVec3 cell)
			{
				Building edifice = cell.GetEdifice(this.grid.Map);
				if (edifice != null && PathFinder.IsDestroyable(edifice))
				{
					return this.tuning.costBlockedWallBase + (int)((float)edifice.HitPoints * this.tuning.costBlockedWallExtraPerHitPoint);
				}
				return 0;
			}

			// Token: 0x0400727E RID: 29310
			private readonly int breachRadius;

			// Token: 0x0400727F RID: 29311
			private readonly BreachingGrid grid;

			// Token: 0x04007280 RID: 29312
			private readonly PathFinderCostTuning tuning;
		}

		// Token: 0x02001DD2 RID: 7634
		private class DangerLineOfSightPainter
		{
			// Token: 0x0600ABEC RID: 44012 RVA: 0x0039269C File Offset: 0x0039089C
			public DangerLineOfSightPainter()
			{
				this.skipThenVisitFunc = new Action<IntVec3>(this.SkipThenVisit);
			}

			// Token: 0x0600ABED RID: 44013 RVA: 0x003926B6 File Offset: 0x003908B6
			private void SkipThenVisit(IntVec3 cell)
			{
				if (this.skipCount <= 0)
				{
					this.visitor(cell);
				}
				this.skipCount--;
			}

			// Token: 0x0600ABEE RID: 44014 RVA: 0x003926DB File Offset: 0x003908DB
			public void PaintLoS(Map map, IntVec3 start, IntVec3 end, Action<IntVec3> visitor)
			{
				if (!start.InBounds(map) || !end.InBounds(map))
				{
					return;
				}
				this.visitor = visitor;
				this.skipCount = Mathf.FloorToInt(5f);
				GenSight.PointsOnLineOfSight(start, end, this.skipThenVisitFunc);
			}

			// Token: 0x04007281 RID: 29313
			private Action<IntVec3> visitor;

			// Token: 0x04007282 RID: 29314
			private int skipCount;

			// Token: 0x04007283 RID: 29315
			private Action<IntVec3> skipThenVisitFunc;
		}

		// Token: 0x02001DD3 RID: 7635
		private class WalkReachabilityPainter
		{
			// Token: 0x0600ABEF RID: 44015 RVA: 0x00392715 File Offset: 0x00390915
			public WalkReachabilityPainter()
			{
				this.floodFillPassCheckFunc = new Predicate<IntVec3>(this.FloodFillPassCheck);
				this.floodFillProcessorFunc = new Func<IntVec3, int, bool>(this.FloodFillProcessor);
			}

			// Token: 0x0600ABF0 RID: 44016 RVA: 0x00392741 File Offset: 0x00390941
			public void PaintWalkReachability(BreachingGrid breachingGrid)
			{
				this.breachingGrid = breachingGrid;
				breachingGrid.map.floodFiller.FloodFill(this.breachingGrid.breachStart, this.floodFillPassCheckFunc, this.floodFillProcessorFunc, int.MaxValue, false, null);
			}

			// Token: 0x0600ABF1 RID: 44017 RVA: 0x00392778 File Offset: 0x00390978
			private bool FloodFillProcessor(IntVec3 c, int traversalDist)
			{
				this.breachingGrid.reachableGrid[c] = true;
				return false;
			}

			// Token: 0x0600ABF2 RID: 44018 RVA: 0x0039278D File Offset: 0x0039098D
			private bool FloodFillPassCheck(IntVec3 c)
			{
				return this.breachingGrid.WalkGrid[c] && !BreachingUtility.BlocksBreaching(this.breachingGrid.map, c);
			}

			// Token: 0x04007284 RID: 29316
			private BreachingGrid breachingGrid;

			// Token: 0x04007285 RID: 29317
			private Predicate<IntVec3> floodFillPassCheckFunc;

			// Token: 0x04007286 RID: 29318
			private Func<IntVec3, int, bool> floodFillProcessorFunc;
		}
	}
}
