using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C99 RID: 3225
	public class GenStep_ScatterAncientTurret : GenStep_Scatterer
	{
		// Token: 0x17000CF9 RID: 3321
		// (get) Token: 0x06004B44 RID: 19268 RVA: 0x0018F3B6 File Offset: 0x0018D5B6
		public override int SeedPart
		{
			get
			{
				return 344678634;
			}
		}

		// Token: 0x06004B45 RID: 19269 RVA: 0x0018FD0C File Offset: 0x0018DF0C
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckIdeology("Scatter ancient turret"))
			{
				return;
			}
			if (!Find.World.HasCaves(map.Tile))
			{
				return;
			}
			this.count = 1;
			base.Generate(map, parms);
		}

		// Token: 0x06004B46 RID: 19270 RVA: 0x0018FD40 File Offset: 0x0018DF40
		protected override bool CanScatterAt(IntVec3 loc, Map map)
		{
			if (!base.CanScatterAt(loc, map) || !loc.InBounds(map))
			{
				return false;
			}
			if (loc.GetTerrain(map).IsWater)
			{
				return false;
			}
			RoofDef roof = loc.GetRoof(map);
			if (roof == null || !roof.isNatural)
			{
				return false;
			}
			if (loc.GetEdifice(map) != null)
			{
				return false;
			}
			if (!map.reachability.CanReachMapEdge(loc, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false)))
			{
				return false;
			}
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(loc, 2f, false))
			{
				if (intVec.InBounds(map) && !intVec.Roofed(map) && intVec.GetEdifice(map) == null && GenSight.LineOfSight(loc, intVec, map, false, null, 0, 0))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004B47 RID: 19271 RVA: 0x0018FE1C File Offset: 0x0018E01C
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			ScatterDebrisUtility.ScatterFilthAroundThing(GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.AncientSecurityTurret, null), loc, map, Rot4.North, WipeMode.Vanish, false), map, ThingDefOf.Filth_DriedBlood, 0.5f, 1, 3, null);
		}

		// Token: 0x04002D9F RID: 11679
		private const int ShellScatterRadius = 10;

		// Token: 0x04002DA0 RID: 11680
		private static readonly IntRange MaxShellsRange = new IntRange(1, 2);
	}
}
