using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018A7 RID: 6311
	public class CompDeepScanner : CompScanner
	{
		// Token: 0x17001603 RID: 5635
		// (get) Token: 0x06008C18 RID: 35864 RVA: 0x0005DEF2 File Offset: 0x0005C0F2
		public new CompProperties_ScannerMineralsDeep Props
		{
			get
			{
				return this.props as CompProperties_ScannerMineralsDeep;
			}
		}

		// Token: 0x06008C19 RID: 35865 RVA: 0x0005DEFF File Offset: 0x0005C0FF
		public override void PostDrawExtraSelectionOverlays()
		{
			if (this.ShouldShowDeepResourceOverlay())
			{
				this.parent.Map.deepResourceGrid.MarkForDraw();
			}
		}

		// Token: 0x06008C1A RID: 35866 RVA: 0x0005DF1E File Offset: 0x0005C11E
		public bool ShouldShowDeepResourceOverlay()
		{
			return this.powerComp != null && this.powerComp.PowerOn;
		}

		// Token: 0x06008C1B RID: 35867 RVA: 0x0028BE90 File Offset: 0x0028A090
		protected override void DoFind(Pawn worker)
		{
			Map map = this.parent.Map;
			IntVec3 intVec;
			if (!CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => this.CanScatterAt(x, map), map, out intVec))
			{
				Log.Error("Could not find a center cell for deep scanning lump generation!", false);
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

		// Token: 0x06008C1C RID: 35868 RVA: 0x0028C008 File Offset: 0x0028A208
		private bool CanScatterAt(IntVec3 pos, Map map)
		{
			int num = CellIndicesUtility.CellToIndex(pos, map.Size.x);
			TerrainDef terrainDef = map.terrainGrid.TerrainAt(num);
			return (terrainDef == null || !terrainDef.IsWater || terrainDef.passability != Traversability.Impassable) && terrainDef.affordances.Contains(ThingDefOf.DeepDrill.terrainAffordanceNeeded) && !map.deepResourceGrid.GetCellBool(num);
		}

		// Token: 0x06008C1D RID: 35869 RVA: 0x0005DF35 File Offset: 0x0005C135
		protected ThingDef ChooseLumpThingDef()
		{
			return DefDatabase<ThingDef>.AllDefs.RandomElementByWeight((ThingDef def) => def.deepCommonality);
		}
	}
}
