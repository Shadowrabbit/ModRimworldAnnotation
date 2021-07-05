using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020012C1 RID: 4801
	public class GenStep_ConditionCauser : GenStep_Scatterer
	{
		// Token: 0x17001009 RID: 4105
		// (get) Token: 0x0600681D RID: 26653 RVA: 0x00046E90 File Offset: 0x00045090
		public override int SeedPart
		{
			get
			{
				return 1068345639;
			}
		}

		// Token: 0x0600681E RID: 26654 RVA: 0x00201F60 File Offset: 0x00200160
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

		// Token: 0x0600681F RID: 26655 RVA: 0x00046E97 File Offset: 0x00045097
		public override void Generate(Map map, GenStepParams parms)
		{
			this.currentParams = parms;
			this.count = 1;
			base.Generate(map, parms);
		}

		// Token: 0x06006820 RID: 26656 RVA: 0x00202078 File Offset: 0x00200278
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

		// Token: 0x04004554 RID: 17748
		private const int Size = 10;

		// Token: 0x04004555 RID: 17749
		private GenStepParams currentParams;
	}
}
