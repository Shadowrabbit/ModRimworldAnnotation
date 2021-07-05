using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011D7 RID: 4567
	public class CompDeepScanner : CompScanner
	{
		// Token: 0x17001322 RID: 4898
		// (get) Token: 0x06006E3C RID: 28220 RVA: 0x0024F437 File Offset: 0x0024D637
		public new CompProperties_ScannerMineralsDeep Props
		{
			get
			{
				return this.props as CompProperties_ScannerMineralsDeep;
			}
		}

		// Token: 0x06006E3D RID: 28221 RVA: 0x0024F444 File Offset: 0x0024D644
		public override void PostDrawExtraSelectionOverlays()
		{
			if (this.ShouldShowDeepResourceOverlay())
			{
				this.parent.Map.deepResourceGrid.MarkForDraw();
			}
		}

		// Token: 0x06006E3E RID: 28222 RVA: 0x0024F463 File Offset: 0x0024D663
		public bool ShouldShowDeepResourceOverlay()
		{
			return this.powerComp != null && this.powerComp.PowerOn;
		}

		// Token: 0x06006E3F RID: 28223 RVA: 0x0024F47C File Offset: 0x0024D67C
		protected override void DoFind(Pawn worker)
		{
			Map map = this.parent.Map;
			IntVec3 intVec;
			if (!CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => this.CanScatterAt(x, map), map, out intVec))
			{
				Log.Error("Could not find a center cell for deep scanning lump generation!");
			}
			ThingDef thingDef = this.ChooseLumpThingDef();
			int numCells = Mathf.CeilToInt((float)thingDef.deepLumpSizeRange.RandomInRange);
			foreach (IntVec3 intVec2 in GridShapeMaker.IrregularLump(intVec, map, numCells))
			{
				if (this.CanScatterAt(intVec2, map) && !intVec2.InNoBuildEdgeArea(map))
				{
					map.deepResourceGrid.SetAt(intVec2, thingDef, thingDef.deepCountPerCell);
				}
			}
			string key;
			if ("LetterDeepScannerFoundLump".CanTranslate())
			{
				key = "LetterDeepScannerFoundLump";
			}
			else if ("DeepScannerFoundLump".CanTranslate())
			{
				key = "DeepScannerFoundLump";
			}
			else
			{
				key = "LetterDeepScannerFoundLump";
			}
			Find.LetterStack.ReceiveLetter("LetterLabelDeepScannerFoundLump".Translate() + ": " + thingDef.LabelCap, key.Translate(thingDef.label, worker.Named("FINDER")), LetterDefOf.PositiveEvent, new LookTargets(intVec, map), null, null, null, null);
		}

		// Token: 0x06006E40 RID: 28224 RVA: 0x0024F5F4 File Offset: 0x0024D7F4
		private bool CanScatterAt(IntVec3 pos, Map map)
		{
			int num = CellIndicesUtility.CellToIndex(pos, map.Size.x);
			TerrainDef terrainDef = map.terrainGrid.TerrainAt(num);
			return (terrainDef == null || !terrainDef.IsWater || terrainDef.passability != Traversability.Impassable) && terrainDef.affordances.Contains(ThingDefOf.DeepDrill.terrainAffordanceNeeded) && !map.deepResourceGrid.GetCellBool(num);
		}

		// Token: 0x06006E41 RID: 28225 RVA: 0x0024F65C File Offset: 0x0024D85C
		protected ThingDef ChooseLumpThingDef()
		{
			return DefDatabase<ThingDef>.AllDefs.RandomElementByWeight((ThingDef def) => def.deepCommonality);
		}
	}
}
