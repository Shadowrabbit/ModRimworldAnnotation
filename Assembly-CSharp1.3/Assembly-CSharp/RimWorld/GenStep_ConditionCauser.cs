using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CB6 RID: 3254
	public class GenStep_ConditionCauser : GenStep_Scatterer
	{
		// Token: 0x17000D14 RID: 3348
		// (get) Token: 0x06004BD5 RID: 19413 RVA: 0x00193F34 File Offset: 0x00192134
		public override int SeedPart
		{
			get
			{
				return 1068345639;
			}
		}

		// Token: 0x06004BD6 RID: 19414 RVA: 0x00193F3C File Offset: 0x0019213C
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			CellRect rect = CellRect.CenteredOn(c, 10, 10).ClipInsideMap(map);
			List<CellRect> list;
			if (MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out list) && list.Any((CellRect x) => rect.Overlaps(x)))
			{
				return false;
			}
			foreach (IntVec3 c2 in rect.Cells)
			{
				if (!GenConstruct.CanBuildOnTerrain(this.currentParams.sitePart.conditionCauser.def, c2, map, Rot4.North, null, null))
				{
					return false;
				}
				using (List<Thing>.Enumerator enumerator2 = c2.GetThingList(map).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.def.building != null)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06004BD7 RID: 19415 RVA: 0x00194054 File Offset: 0x00192254
		public override void Generate(Map map, GenStepParams parms)
		{
			this.currentParams = parms;
			this.count = 1;
			base.Generate(map, parms);
		}

		// Token: 0x06004BD8 RID: 19416 RVA: 0x0019406C File Offset: 0x0019226C
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			Faction faction;
			if (map.ParentFaction == null || map.ParentFaction == Faction.OfPlayer)
			{
				faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			}
			else
			{
				faction = map.ParentFaction;
			}
			List<CellRect> list;
			if (!MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out list))
			{
				list = new List<CellRect>();
				MapGenerator.SetVar<List<CellRect>>("UsedRects", list);
			}
			CellRect cellRect = CellRect.CenteredOn(loc, 10, 10).ClipInsideMap(map);
			SitePart sitePart = this.currentParams.sitePart;
			sitePart.conditionCauserWasSpawned = true;
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = cellRect;
			resolveParams.faction = faction;
			resolveParams.conditionCauser = sitePart.conditionCauser;
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("conditionCauserRoom", resolveParams, null);
			BaseGen.Generate();
			MapGenerator.SetVar<CellRect>("RectOfInterest", cellRect);
			list.Add(cellRect);
		}

		// Token: 0x04002DE3 RID: 11747
		private const int Size = 10;

		// Token: 0x04002DE4 RID: 11748
		private GenStepParams currentParams;
	}
}
