using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.BaseGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CC0 RID: 3264
	public class GenStep_Turrets : GenStep
	{
		// Token: 0x17000D1D RID: 3357
		// (get) Token: 0x06004BFC RID: 19452 RVA: 0x001953D1 File Offset: 0x001935D1
		public override int SeedPart
		{
			get
			{
				return 895502705;
			}
		}

		// Token: 0x06004BFD RID: 19453 RVA: 0x001953D8 File Offset: 0x001935D8
		public override void Generate(Map map, GenStepParams parms)
		{
			int num = 0;
			CellRect cellRect;
			if (!MapGenerator.TryGetVar<CellRect>("RectOfInterest", out cellRect))
			{
				cellRect = this.FindRandomRectToDefend(map);
			}
			else
			{
				int num2;
				if (!MapGenerator.TryGetVar<int>("RectOfInterestTurretsGenStepsCount", out num2))
				{
					num2 = 0;
				}
				num += num2 * 4;
				num2++;
				MapGenerator.SetVar<int>("RectOfInterestTurretsGenStepsCount", num2);
			}
			Faction faction;
			if (map.ParentFaction == null || map.ParentFaction == Faction.OfPlayer)
			{
				faction = (from x in Find.FactionManager.AllFactions
				where !x.defeated && x.HostileTo(Faction.OfPlayer) && !x.Hidden && x.def.techLevel >= TechLevel.Industrial && !x.temporary
				select x).RandomElementWithFallback(Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined));
			}
			else
			{
				faction = map.ParentFaction;
			}
			int randomInRange = this.widthRange.RandomInRange;
			CellRect rect = cellRect.ExpandedBy(7 + randomInRange + num).ClipInsideMap(map);
			int value;
			int value2;
			if (parms.sitePart != null)
			{
				value = parms.sitePart.parms.turretsCount;
				value2 = parms.sitePart.parms.mortarsCount;
			}
			else
			{
				value = this.defaultTurretsCountRange.RandomInRange;
				value2 = this.defaultMortarsCountRange.RandomInRange;
			}
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = rect;
			resolveParams.faction = faction;
			resolveParams.edgeDefenseWidth = new int?(randomInRange);
			resolveParams.edgeDefenseTurretsCount = new int?(value);
			resolveParams.edgeDefenseMortarsCount = new int?(value2);
			resolveParams.edgeDefenseGuardsCount = new int?(this.guardsCountRange.RandomInRange);
			resolveParams.edgeThingMustReachMapEdge = new bool?(true);
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("edgeDefense", resolveParams, null);
			BaseGen.Generate();
			ResolveParams resolveParams2 = default(ResolveParams);
			resolveParams2.rect = rect;
			resolveParams2.faction = faction;
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("outdoorLighting", resolveParams2, null);
			BaseGen.Generate();
		}

		// Token: 0x06004BFE RID: 19454 RVA: 0x001955B8 File Offset: 0x001937B8
		private CellRect FindRandomRectToDefend(Map map)
		{
			List<CellRect> usedRects;
			if (!MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out usedRects))
			{
				usedRects = null;
			}
			int rectRadius = Mathf.Max(Mathf.RoundToInt((float)Mathf.Min(map.Size.x, map.Size.z) * 0.07f), 1);
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false);
			IntVec3 center;
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith(delegate(IntVec3 x)
			{
				if (!map.reachability.CanReachMapEdge(x, traverseParams))
				{
					return false;
				}
				CellRect cellRect = CellRect.CenteredOn(x, rectRadius);
				int num = 0;
				using (CellRect.Enumerator enumerator = cellRect.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IntVec3 c = enumerator.Current;
						if (!c.InBounds(map))
						{
							return false;
						}
						if (usedRects != null && cellRect.IsOnEdge(c) && usedRects.Any((CellRect y) => y.Contains(c)))
						{
							return false;
						}
						if (c.Standable(map) || c.GetPlant(map) != null)
						{
							num++;
						}
					}
				}
				return (float)num / (float)cellRect.Area >= 0.6f;
			}, map, out center))
			{
				return CellRect.CenteredOn(center, rectRadius);
			}
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => x.Standable(map), map, out center))
			{
				return CellRect.CenteredOn(center, rectRadius);
			}
			return CellRect.CenteredOn(CellFinder.RandomCell(map), rectRadius).ClipInsideMap(map);
		}

		// Token: 0x04002DFB RID: 11771
		public IntRange defaultTurretsCountRange = new IntRange(4, 5);

		// Token: 0x04002DFC RID: 11772
		public IntRange defaultMortarsCountRange = new IntRange(0, 1);

		// Token: 0x04002DFD RID: 11773
		public IntRange widthRange = new IntRange(3, 4);

		// Token: 0x04002DFE RID: 11774
		public IntRange guardsCountRange = new IntRange(1, 1);

		// Token: 0x04002DFF RID: 11775
		private const int Padding = 7;

		// Token: 0x04002E00 RID: 11776
		public const int DefaultGuardsCount = 1;
	}
}
