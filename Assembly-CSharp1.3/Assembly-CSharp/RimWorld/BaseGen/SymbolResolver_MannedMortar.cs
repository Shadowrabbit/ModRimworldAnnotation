using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen
{
	// Token: 0x020015E6 RID: 5606
	public class SymbolResolver_MannedMortar : SymbolResolver
	{
		// Token: 0x060083A3 RID: 33699 RVA: 0x002F067C File Offset: 0x002EE87C
		public override bool CanResolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			if (!base.CanResolve(rp))
			{
				return false;
			}
			int num = 0;
			using (CellRect.Enumerator enumerator = rp.rect.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Standable(map))
					{
						num++;
					}
				}
			}
			return num >= 2;
		}

		// Token: 0x060083A4 RID: 33700 RVA: 0x002F06F4 File Offset: 0x002EE8F4
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			Faction faction;
			if ((faction = rp.faction) == null)
			{
				faction = (Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Industrial) ?? Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined));
			}
			Faction faction2 = faction;
			Rot4 rot = rp.thingRot ?? Rot4.Random;
			ThingDef thingDef;
			if ((thingDef = rp.mortarDef) == null)
			{
				thingDef = (from x in DefDatabase<ThingDef>.AllDefsListForReading
				where x.category == ThingCategory.Building && x.building.IsMortar && x.building.buildingTags.Contains("Artillery_MannedMortar")
				select x).RandomElement<ThingDef>();
			}
			ThingDef thingDef2 = thingDef;
			IntVec3 intVec;
			if (!this.TryFindMortarSpawnCell(rp.rect, rot, thingDef2, out intVec))
			{
				return;
			}
			if (thingDef2.HasComp(typeof(CompMannable)))
			{
				IntVec3 c = ThingUtility.InteractionCellWhenAt(thingDef2, intVec, rot, map);
				Lord singlePawnLord = LordMaker.MakeNewLord(faction2, new LordJob_ManTurrets(), map, null);
				PawnGenerationRequest value = new PawnGenerationRequest(faction2.RandomPawnKind(), faction2, PawnGenerationContext.NonPlayer, map.Tile, false, false, false, false, true, true, 1f, false, true, true, true, true, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, null, false, false);
				ResolveParams resolveParams = rp;
				resolveParams.faction = faction2;
				resolveParams.singlePawnGenerationRequest = new PawnGenerationRequest?(value);
				resolveParams.rect = CellRect.SingleCell(c);
				resolveParams.singlePawnLord = singlePawnLord;
				BaseGen.symbolStack.Push("pawn", resolveParams, null);
			}
			ThingDef thingDef3 = TurretGunUtility.TryFindRandomShellDef(thingDef2, false, true, faction2.def.techLevel, false, 250f);
			if (thingDef3 != null)
			{
				ResolveParams resolveParams2 = rp;
				resolveParams2.faction = faction2;
				resolveParams2.singleThingDef = thingDef3;
				resolveParams2.singleThingStackCount = new int?(Rand.RangeInclusive(5, Mathf.Min(8, thingDef3.stackLimit)));
				BaseGen.symbolStack.Push("thing", resolveParams2, null);
			}
			ResolveParams resolveParams3 = rp;
			resolveParams3.faction = faction2;
			resolveParams3.singleThingDef = thingDef2;
			resolveParams3.rect = CellRect.SingleCell(intVec);
			resolveParams3.thingRot = new Rot4?(rot);
			BaseGen.symbolStack.Push("thing", resolveParams3, null);
		}

		// Token: 0x060083A5 RID: 33701 RVA: 0x002F0930 File Offset: 0x002EEB30
		private bool TryFindMortarSpawnCell(CellRect rect, Rot4 rot, ThingDef mortarDef, out IntVec3 cell)
		{
			Map map = BaseGen.globalSettings.map;
			Predicate<CellRect> edgeTouchCheck;
			if (rot == Rot4.North)
			{
				Func<IntVec3, bool> <>9__5;
				edgeTouchCheck = delegate(CellRect x)
				{
					IEnumerable<IntVec3> cells = x.Cells;
					Func<IntVec3, bool> predicate;
					if ((predicate = <>9__5) == null)
					{
						predicate = (<>9__5 = ((IntVec3 y) => y.z == rect.maxZ));
					}
					return cells.Any(predicate);
				};
			}
			else if (rot == Rot4.South)
			{
				Func<IntVec3, bool> <>9__6;
				edgeTouchCheck = delegate(CellRect x)
				{
					IEnumerable<IntVec3> cells = x.Cells;
					Func<IntVec3, bool> predicate;
					if ((predicate = <>9__6) == null)
					{
						predicate = (<>9__6 = ((IntVec3 y) => y.z == rect.minZ));
					}
					return cells.Any(predicate);
				};
			}
			else if (rot == Rot4.West)
			{
				Func<IntVec3, bool> <>9__7;
				edgeTouchCheck = delegate(CellRect x)
				{
					IEnumerable<IntVec3> cells = x.Cells;
					Func<IntVec3, bool> predicate;
					if ((predicate = <>9__7) == null)
					{
						predicate = (<>9__7 = ((IntVec3 y) => y.x == rect.minX));
					}
					return cells.Any(predicate);
				};
			}
			else
			{
				Func<IntVec3, bool> <>9__8;
				edgeTouchCheck = delegate(CellRect x)
				{
					IEnumerable<IntVec3> cells = x.Cells;
					Func<IntVec3, bool> predicate;
					if ((predicate = <>9__8) == null)
					{
						predicate = (<>9__8 = ((IntVec3 y) => y.x == rect.maxX));
					}
					return cells.Any(predicate);
				};
			}
			return CellFinder.TryFindRandomCellInsideWith(rect, delegate(IntVec3 x)
			{
				CellRect obj = GenAdj.OccupiedRect(x, rot, mortarDef.size);
				return ThingUtility.InteractionCellWhenAt(mortarDef, x, rot, map).Standable(map) && obj.FullyContainedWithin(rect) && edgeTouchCheck(obj);
			}, out cell);
		}

		// Token: 0x0400522C RID: 21036
		private const float MaxShellDefMarketValue = 250f;
	}
}
