using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C9B RID: 3227
	public class GenStep_ScatterCaveDebris : GenStep_Scatterer
	{
		// Token: 0x17000CFB RID: 3323
		// (get) Token: 0x06004B51 RID: 19281 RVA: 0x0019007D File Offset: 0x0018E27D
		public override int SeedPart
		{
			get
			{
				return 85037593;
			}
		}

		// Token: 0x06004B52 RID: 19282 RVA: 0x00190084 File Offset: 0x0018E284
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckIdeology("Scatter cave debris"))
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

		// Token: 0x06004B53 RID: 19283 RVA: 0x001900B8 File Offset: 0x0018E2B8
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			RoofDef roof = c.GetRoof(map);
			if (roof == null || !roof.isNatural)
			{
				return false;
			}
			int num = Rand.RangeInclusive(1, 4);
			for (int i = 0; i < 4; i++)
			{
				this.rotation = new Rot4(i % num);
				if (this.CanPlace(c, this.rotation, map))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004B54 RID: 19284 RVA: 0x00190110 File Offset: 0x0018E310
		private bool CanPlace(IntVec3 c, Rot4 r, Map map)
		{
			if (!map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false)))
			{
				return false;
			}
			CellRect cellRect = GenAdj.OccupiedRect(c, r, ThingDefOf.AncientBed.size);
			using (CellRect.Enumerator enumerator = cellRect.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetEdifice(map) != null)
					{
						return false;
					}
				}
			}
			GenStep_ScatterCaveDebris.edgesToCheck.Clear();
			if (cellRect.Width > cellRect.Height)
			{
				GenStep_ScatterCaveDebris.edgesToCheck.Add(Rot4.North);
				GenStep_ScatterCaveDebris.edgesToCheck.Add(Rot4.South);
			}
			else
			{
				GenStep_ScatterCaveDebris.edgesToCheck.Add(Rot4.East);
				GenStep_ScatterCaveDebris.edgesToCheck.Add(Rot4.West);
			}
			GenStep_ScatterCaveDebris.edgesToCheck.Shuffle<Rot4>();
			foreach (Rot4 dir in GenStep_ScatterCaveDebris.edgesToCheck)
			{
				bool flag = true;
				foreach (IntVec3 a in cellRect.GetEdgeCells(dir))
				{
					IntVec3 c2 = a + dir.FacingCell;
					if (c2.InBounds(map) && c2.GetEdifice(map) != null && c2.GetEdifice(map).def.building.isNaturalRock)
					{
						flag = false;
						break;
					}
				}
				foreach (IntVec3 a2 in cellRect.GetEdgeCells(dir.Opposite))
				{
					IntVec3 c3 = a2 + dir.Opposite.FacingCell;
					if (c3.InBounds(map) && c3.GetEdifice(map) == null)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					GenStep_ScatterCaveDebris.edgesToCheck.Clear();
					return true;
				}
			}
			GenStep_ScatterCaveDebris.edgesToCheck.Clear();
			return false;
		}

		// Token: 0x06004B55 RID: 19285 RVA: 0x00190374 File Offset: 0x0018E574
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			GenStep_ScatterCaveDebris.<>c__DisplayClass7_0 CS$<>8__locals1 = new GenStep_ScatterCaveDebris.<>c__DisplayClass7_0();
			CS$<>8__locals1.map = map;
			GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.AncientBed, null), loc, CS$<>8__locals1.map, this.rotation, WipeMode.Vanish, false);
			IntVec3 invalid = IntVec3.Invalid;
			if (CellFinder.TryFindRandomCellNear(loc, CS$<>8__locals1.map, 5, new Predicate<IntVec3>(CS$<>8__locals1.<ScatterAt>g__CellPredicate|0), out invalid, -1))
			{
				FilthMaker.TryMakeFilth(invalid, CS$<>8__locals1.map, ThingDefOf.Filth_MoldyUniform, 1, FilthSourceFlags.None);
			}
			if (Rand.Bool && CellFinder.TryFindRandomCellNear(loc, CS$<>8__locals1.map, 5, new Predicate<IntVec3>(CS$<>8__locals1.<ScatterAt>g__CellPredicate|0), out invalid, -1))
			{
				FilthMaker.TryMakeFilth(invalid, CS$<>8__locals1.map, ThingDefOf.Filth_ScatteredDocuments, 1, FilthSourceFlags.None);
			}
			if (Rand.Bool && CellFinder.TryFindRandomCellNear(loc, CS$<>8__locals1.map, 5, new Predicate<IntVec3>(CS$<>8__locals1.<ScatterAt>g__CellPredicate|0), out invalid, -1))
			{
				FilthMaker.TryMakeFilth(invalid, CS$<>8__locals1.map, ThingDefOf.Filth_DriedBlood, 1, FilthSourceFlags.None);
			}
		}

		// Token: 0x04002DA3 RID: 11683
		private Rot4 rotation;

		// Token: 0x04002DA4 RID: 11684
		private static List<Rot4> edgesToCheck = new List<Rot4>();
	}
}
