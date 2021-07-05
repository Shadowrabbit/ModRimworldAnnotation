using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C9C RID: 3228
	public class GenStep_ScatterMechanoidRemains : GenStep_Scatterer
	{
		// Token: 0x17000CFC RID: 3324
		// (get) Token: 0x06004B58 RID: 19288 RVA: 0x0018F3B6 File Offset: 0x0018D5B6
		public override int SeedPart
		{
			get
			{
				return 344678634;
			}
		}

		// Token: 0x06004B59 RID: 19289 RVA: 0x00190464 File Offset: 0x0018E664
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckIdeology("Scatter mechanoid remains"))
			{
				return;
			}
			this.count = 1;
			this.allowInWaterBiome = false;
			base.Generate(map, parms);
		}

		// Token: 0x06004B5A RID: 19290 RVA: 0x00190489 File Offset: 0x0018E689
		protected override bool CanScatterAt(IntVec3 loc, Map map)
		{
			return base.CanScatterAt(loc, map) && this.CanPlaceDebrisAt(loc, map);
		}

		// Token: 0x06004B5B RID: 19291 RVA: 0x001904A4 File Offset: 0x0018E6A4
		private bool CanPlaceDebrisAt(IntVec3 loc, Map map)
		{
			return loc.InBounds(map) && !loc.Roofed(map) && loc.SupportsStructureType(map, TerrainAffordanceDefOf.Heavy) && !map.terrainGrid.TerrainAt(loc).IsWater;
		}

		// Token: 0x06004B5C RID: 19292 RVA: 0x001904E4 File Offset: 0x0018E6E4
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			foreach (IntVec3 c in GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.AncientMechDropBeacon, null), loc, map, Rot4.North, WipeMode.Vanish, false).OccupiedRect())
			{
				if (Rand.Bool && c.InBounds(map) && !c.Impassable(map))
				{
					FilthMaker.TryMakeFilth(c, map, ThingDefOf.Filth_MachineBits, 1, FilthSourceFlags.None);
				}
			}
			FilthMaker.TryMakeFilth(loc, map, ThingDefOf.Filth_RubbleBuilding, 1, FilthSourceFlags.None);
			CellRect cellRect = CellRect.CenteredOn(loc, 10);
			int randomInRange = GenStep_ScatterMechanoidRemains.MaxShellsRange.RandomInRange;
			int num = 0;
			foreach (IntVec3 intVec in cellRect.InRandomOrder(null))
			{
				if (this.CanPlaceDebrisAt(intVec, map) && GenSight.LineOfSight(intVec, loc, map, true, null, 0, 0))
				{
					GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.AncientMechanoidShell, null), intVec, map, Rot4.North, WipeMode.Vanish, false);
					FilthMaker.TryMakeFilth(intVec, map, ThingDefOf.Filth_Ash, 1, FilthSourceFlags.None);
					num++;
					if (num >= randomInRange)
					{
						break;
					}
				}
			}
		}

		// Token: 0x04002DA5 RID: 11685
		private const int ShellScatterRadius = 10;

		// Token: 0x04002DA6 RID: 11686
		private static readonly IntRange MaxShellsRange = new IntRange(1, 2);
	}
}
