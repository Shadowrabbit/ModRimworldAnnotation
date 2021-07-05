using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015FF RID: 5631
	public class SymbolResolver_Interior_SleepingAncientSoldiers : SymbolResolver
	{
		// Token: 0x060083EE RID: 33774 RVA: 0x002F3965 File Offset: 0x002F1B65
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && rp.threatPoints != null && rp.threatPoints.Value >= PawnKindDefOf.AncientSoldier.combatPower;
		}

		// Token: 0x060083EF RID: 33775 RVA: 0x002F399C File Offset: 0x002F1B9C
		public override void Resolve(ResolveParams rp)
		{
			int num = Mathf.RoundToInt(rp.threatPoints.Value / PawnKindDefOf.AncientSoldier.combatPower);
			SymbolResolver_Interior_SleepingAncientSoldiers.tmpCellRects.Clear();
			ThingDef ancientCryptosleepCasket = ThingDefOf.AncientCryptosleepCasket;
			CellRect cellRect = new CellRect(0, 0, ancientCryptosleepCasket.size.x, ancientCryptosleepCasket.size.z).ExpandedBy(1);
			for (int i = 0; i < num; i++)
			{
				CellRect item;
				if (rp.rect.TryFindRandomInnerRect(new IntVec2(cellRect.Width, cellRect.Height), out item, (CellRect rect) => !SymbolResolver_Interior_SleepingAncientSoldiers.tmpCellRects.Any((CellRect r) => r.Overlaps(rect))))
				{
					ResolveParams resolveParams = rp;
					resolveParams.rect = item.ExpandedBy(-1);
					resolveParams.thingRot = new Rot4?(ancientCryptosleepCasket.defaultPlacingRot);
					resolveParams.podContentsType = new PodContentsType?(PodContentsType.AncientHostile);
					resolveParams.faction = Faction.OfAncientsHostile;
					BaseGen.symbolStack.Push("ancientCryptosleepCasket", resolveParams, null);
					SymbolResolver_Interior_SleepingAncientSoldiers.tmpCellRects.Add(item);
				}
			}
			SymbolResolver_Interior_SleepingAncientSoldiers.tmpCellRects.Clear();
		}

		// Token: 0x04005251 RID: 21073
		private static List<CellRect> tmpCellRects = new List<CellRect>();
	}
}
