using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000266 RID: 614
	public sealed class DeepResourceGrid : ICellBoolGiver, IExposable
	{
		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000F96 RID: 3990 RVA: 0x0000BBC0 File Offset: 0x00009DC0
		public Color Color
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x000B6F64 File Offset: 0x000B5164
		public DeepResourceGrid(Map map)
		{
			this.map = map;
			this.defGrid = new ushort[map.cellIndices.NumGridCells];
			this.countGrid = new ushort[map.cellIndices.NumGridCells];
			this.drawer = new CellBoolDrawer(this, map.Size.x, map.Size.z, 3640, 1f);
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x000B6FD8 File Offset: 0x000B51D8
		public void ExposeData()
		{
			MapExposeUtility.ExposeUshort(this.map, (IntVec3 c) => this.defGrid[this.map.cellIndices.CellToIndex(c)], delegate(IntVec3 c, ushort val)
			{
				this.defGrid[this.map.cellIndices.CellToIndex(c)] = val;
			}, "defGrid");
			MapExposeUtility.ExposeUshort(this.map, (IntVec3 c) => this.countGrid[this.map.cellIndices.CellToIndex(c)], delegate(IntVec3 c, ushort val)
			{
				this.countGrid[this.map.cellIndices.CellToIndex(c)] = val;
			}, "countGrid");
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x00011B35 File Offset: 0x0000FD35
		public ThingDef ThingDefAt(IntVec3 c)
		{
			return DefDatabase<ThingDef>.GetByShortHash(this.defGrid[this.map.cellIndices.CellToIndex(c)]);
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x00011B54 File Offset: 0x0000FD54
		public int CountAt(IntVec3 c)
		{
			return (int)this.countGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x000B7038 File Offset: 0x000B5238
		public void SetAt(IntVec3 c, ThingDef def, int count)
		{
			if (count == 0)
			{
				def = null;
			}
			ushort num;
			if (def == null)
			{
				num = 0;
			}
			else
			{
				num = def.shortHash;
			}
			ushort num2 = (ushort)count;
			if (count > 65535)
			{
				Log.Error("Cannot store count " + count + " in DeepResourceGrid: out of ushort range.", false);
				num2 = ushort.MaxValue;
			}
			if (count < 0)
			{
				Log.Error("Cannot store count " + count + " in DeepResourceGrid: out of ushort range.", false);
				num2 = 0;
			}
			int num3 = this.map.cellIndices.CellToIndex(c);
			if (this.defGrid[num3] == num && this.countGrid[num3] == num2)
			{
				return;
			}
			this.defGrid[num3] = num;
			this.countGrid[num3] = num2;
			this.drawer.SetDirty();
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x00011B6E File Offset: 0x0000FD6E
		public void DeepResourceGridUpdate()
		{
			this.drawer.CellBoolDrawerUpdate();
			if (DebugViewSettings.drawDeepResources)
			{
				this.MarkForDraw();
			}
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x00011B88 File Offset: 0x0000FD88
		public void MarkForDraw()
		{
			if (this.map == Find.CurrentMap)
			{
				this.drawer.MarkForDraw();
			}
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x000B70EC File Offset: 0x000B52EC
		public void DrawPlacingMouseAttachments(BuildableDef placingDef)
		{
			ThingDef thingDef;
			if ((thingDef = (placingDef as ThingDef)) != null && thingDef.CompDefFor<CompDeepDrill>() != null && this.AnyActiveDeepScannersOnMap())
			{
				this.RenderMouseAttachments();
			}
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x000B711C File Offset: 0x000B531C
		public void DeepResourcesOnGUI()
		{
			Thing singleSelectedThing = Find.Selector.SingleSelectedThing;
			if (singleSelectedThing == null)
			{
				return;
			}
			CompDeepScanner compDeepScanner = singleSelectedThing.TryGetComp<CompDeepScanner>();
			CompDeepDrill compDeepDrill = singleSelectedThing.TryGetComp<CompDeepDrill>();
			if (compDeepScanner == null && compDeepDrill == null)
			{
				return;
			}
			if (!this.AnyActiveDeepScannersOnMap())
			{
				return;
			}
			this.RenderMouseAttachments();
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x000B715C File Offset: 0x000B535C
		public bool AnyActiveDeepScannersOnMap()
		{
			foreach (Building thing in this.map.listerBuildings.allBuildingsColonist)
			{
				CompDeepScanner compDeepScanner = thing.TryGetComp<CompDeepScanner>();
				if (compDeepScanner != null && compDeepScanner.ShouldShowDeepResourceOverlay())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x000B71CC File Offset: 0x000B53CC
		private void RenderMouseAttachments()
		{
			IntVec3 c = UI.MouseCell();
			if (!c.InBounds(this.map))
			{
				return;
			}
			ThingDef thingDef = this.map.deepResourceGrid.ThingDefAt(c);
			if (thingDef == null)
			{
				return;
			}
			int num = this.map.deepResourceGrid.CountAt(c);
			if (num <= 0)
			{
				return;
			}
			Vector2 vector = c.ToVector3().MapToUIPosition();
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleLeft;
			float num2 = (UI.CurUICellSize() - 27f) / 2f;
			Rect rect = new Rect(vector.x + num2, vector.y - UI.CurUICellSize() + num2, 27f, 27f);
			Widgets.ThingIcon(rect, thingDef, null, 1f);
			Widgets.Label(new Rect(rect.xMax + 4f, rect.y, 999f, 29f), "DeepResourceRemaining".Translate(thingDef.Named("RESOURCE"), num.Named("COUNT")));
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x00011BA2 File Offset: 0x0000FDA2
		public bool GetCellBool(int index)
		{
			return this.CountAt(this.map.cellIndices.IndexToCell(index)) > 0;
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x000B72DC File Offset: 0x000B54DC
		public Color GetCellExtraColor(int index)
		{
			IntVec3 c = this.map.cellIndices.IndexToCell(index);
			float num = (float)this.CountAt(c);
			ThingDef thingDef = this.ThingDefAt(c);
			return DebugMatsSpectrum.Mat(Mathf.RoundToInt(num / (float)thingDef.deepCountPerCell / 2f * 100f) % 100, true).color;
		}

		// Token: 0x04000CB7 RID: 3255
		private const float LineSpacing = 29f;

		// Token: 0x04000CB8 RID: 3256
		private const float IconPaddingRight = 4f;

		// Token: 0x04000CB9 RID: 3257
		private const float IconSize = 27f;

		// Token: 0x04000CBA RID: 3258
		private Map map;

		// Token: 0x04000CBB RID: 3259
		private CellBoolDrawer drawer;

		// Token: 0x04000CBC RID: 3260
		private ushort[] defGrid;

		// Token: 0x04000CBD RID: 3261
		private ushort[] countGrid;
	}
}
