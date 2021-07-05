using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001AF RID: 431
	public sealed class DeepResourceGrid : ICellBoolGiver, IExposable
	{
		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000C18 RID: 3096 RVA: 0x0001A4C7 File Offset: 0x000186C7
		public Color Color
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x00041410 File Offset: 0x0003F610
		public DeepResourceGrid(Map map)
		{
			this.map = map;
			this.defGrid = new ushort[map.cellIndices.NumGridCells];
			this.countGrid = new ushort[map.cellIndices.NumGridCells];
			this.drawer = new CellBoolDrawer(this, map.Size.x, map.Size.z, 3640, 1f);
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x00041484 File Offset: 0x0003F684
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

		// Token: 0x06000C1B RID: 3099 RVA: 0x000414E1 File Offset: 0x0003F6E1
		public ThingDef ThingDefAt(IntVec3 c)
		{
			return DefDatabase<ThingDef>.GetByShortHash(this.defGrid[this.map.cellIndices.CellToIndex(c)]);
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x00041500 File Offset: 0x0003F700
		public int CountAt(IntVec3 c)
		{
			return (int)this.countGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x0004151C File Offset: 0x0003F71C
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
				Log.Error("Cannot store count " + count + " in DeepResourceGrid: out of ushort range.");
				num2 = ushort.MaxValue;
			}
			if (count < 0)
			{
				Log.Error("Cannot store count " + count + " in DeepResourceGrid: out of ushort range.");
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

		// Token: 0x06000C1E RID: 3102 RVA: 0x000415CE File Offset: 0x0003F7CE
		public void DeepResourceGridUpdate()
		{
			this.drawer.CellBoolDrawerUpdate();
			if (DebugViewSettings.drawDeepResources)
			{
				this.MarkForDraw();
			}
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x000415E8 File Offset: 0x0003F7E8
		public void MarkForDraw()
		{
			if (this.map == Find.CurrentMap)
			{
				this.drawer.MarkForDraw();
			}
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x00041604 File Offset: 0x0003F804
		public void DrawPlacingMouseAttachments(BuildableDef placingDef)
		{
			ThingDef thingDef;
			if ((thingDef = (placingDef as ThingDef)) != null && thingDef.CompDefFor<CompDeepDrill>() != null && this.AnyActiveDeepScannersOnMap())
			{
				this.RenderMouseAttachments();
			}
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x00041634 File Offset: 0x0003F834
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

		// Token: 0x06000C22 RID: 3106 RVA: 0x00041674 File Offset: 0x0003F874
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

		// Token: 0x06000C23 RID: 3107 RVA: 0x000416E4 File Offset: 0x0003F8E4
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
			Widgets.ThingIcon(rect, thingDef, null, null, 1f, null);
			Widgets.Label(new Rect(rect.xMax + 4f, rect.y, 999f, 29f), "DeepResourceRemaining".Translate(thingDef.Named("RESOURCE"), num.Named("COUNT")));
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x000417FE File Offset: 0x0003F9FE
		public bool GetCellBool(int index)
		{
			return this.CountAt(this.map.cellIndices.IndexToCell(index)) > 0;
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x0004181C File Offset: 0x0003FA1C
		public Color GetCellExtraColor(int index)
		{
			IntVec3 c = this.map.cellIndices.IndexToCell(index);
			float num = (float)this.CountAt(c);
			ThingDef thingDef = this.ThingDefAt(c);
			return DebugMatsSpectrum.Mat(Mathf.RoundToInt(num / (float)thingDef.deepCountPerCell / 2f * 100f) % 100, true).color;
		}

		// Token: 0x040009F4 RID: 2548
		private const float LineSpacing = 29f;

		// Token: 0x040009F5 RID: 2549
		private const float IconPaddingRight = 4f;

		// Token: 0x040009F6 RID: 2550
		private const float IconSize = 27f;

		// Token: 0x040009F7 RID: 2551
		private Map map;

		// Token: 0x040009F8 RID: 2552
		private CellBoolDrawer drawer;

		// Token: 0x040009F9 RID: 2553
		private ushort[] defGrid;

		// Token: 0x040009FA RID: 2554
		private ushort[] countGrid;
	}
}
