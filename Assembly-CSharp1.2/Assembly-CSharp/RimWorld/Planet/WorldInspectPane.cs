using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020021E6 RID: 8678
	public class WorldInspectPane : Window, IInspectPane
	{
		// Token: 0x17001B9D RID: 7069
		// (get) Token: 0x0600B9C6 RID: 47558 RVA: 0x0007852C File Offset: 0x0007672C
		// (set) Token: 0x0600B9C7 RID: 47559 RVA: 0x00078534 File Offset: 0x00076734
		public Type OpenTabType
		{
			get
			{
				return this.openTabType;
			}
			set
			{
				this.openTabType = value;
			}
		}

		// Token: 0x17001B9E RID: 7070
		// (get) Token: 0x0600B9C8 RID: 47560 RVA: 0x0007853D File Offset: 0x0007673D
		// (set) Token: 0x0600B9C9 RID: 47561 RVA: 0x00078545 File Offset: 0x00076745
		public float RecentHeight
		{
			get
			{
				return this.recentHeight;
			}
			set
			{
				this.recentHeight = value;
			}
		}

		// Token: 0x17001B9F RID: 7071
		// (get) Token: 0x0600B9CA RID: 47562 RVA: 0x00016647 File Offset: 0x00014847
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17001BA0 RID: 7072
		// (get) Token: 0x0600B9CB RID: 47563 RVA: 0x0006657A File Offset: 0x0006477A
		public override Vector2 InitialSize
		{
			get
			{
				return InspectPaneUtility.PaneSizeFor(this);
			}
		}

		// Token: 0x17001BA1 RID: 7073
		// (get) Token: 0x0600B9CC RID: 47564 RVA: 0x0007854E File Offset: 0x0007674E
		private List<WorldObject> Selected
		{
			get
			{
				return Find.WorldSelector.SelectedObjects;
			}
		}

		// Token: 0x17001BA2 RID: 7074
		// (get) Token: 0x0600B9CD RID: 47565 RVA: 0x0007855A File Offset: 0x0007675A
		private int NumSelectedObjects
		{
			get
			{
				return Find.WorldSelector.NumSelectedObjects;
			}
		}

		// Token: 0x17001BA3 RID: 7075
		// (get) Token: 0x0600B9CE RID: 47566 RVA: 0x00357560 File Offset: 0x00355760
		public float PaneTopY
		{
			get
			{
				float num = (float)UI.screenHeight - 165f;
				if (Current.ProgramState == ProgramState.Playing)
				{
					num -= 35f;
				}
				return num;
			}
		}

		// Token: 0x17001BA4 RID: 7076
		// (get) Token: 0x0600B9CF RID: 47567 RVA: 0x00078566 File Offset: 0x00076766
		public bool AnythingSelected
		{
			get
			{
				return Find.WorldSelector.AnyObjectOrTileSelected;
			}
		}

		// Token: 0x17001BA5 RID: 7077
		// (get) Token: 0x0600B9D0 RID: 47568 RVA: 0x000722C9 File Offset: 0x000704C9
		private int SelectedTile
		{
			get
			{
				return Find.WorldSelector.selectedTile;
			}
		}

		// Token: 0x17001BA6 RID: 7078
		// (get) Token: 0x0600B9D1 RID: 47569 RVA: 0x00078572 File Offset: 0x00076772
		private bool SelectedSingleObjectOrTile
		{
			get
			{
				return this.NumSelectedObjects == 1 || (this.NumSelectedObjects == 0 && this.SelectedTile >= 0);
			}
		}

		// Token: 0x17001BA7 RID: 7079
		// (get) Token: 0x0600B9D2 RID: 47570 RVA: 0x00078595 File Offset: 0x00076795
		public bool ShouldShowSelectNextInCellButton
		{
			get
			{
				return this.SelectedSingleObjectOrTile;
			}
		}

		// Token: 0x17001BA8 RID: 7080
		// (get) Token: 0x0600B9D3 RID: 47571 RVA: 0x00078595 File Offset: 0x00076795
		public bool ShouldShowPaneContents
		{
			get
			{
				return this.SelectedSingleObjectOrTile;
			}
		}

		// Token: 0x17001BA9 RID: 7081
		// (get) Token: 0x0600B9D4 RID: 47572 RVA: 0x0007859D File Offset: 0x0007679D
		public IEnumerable<InspectTabBase> CurTabs
		{
			get
			{
				if (this.NumSelectedObjects == 1)
				{
					return Find.WorldSelector.SingleSelectedObject.GetInspectTabs();
				}
				if (this.NumSelectedObjects == 0 && this.SelectedTile >= 0)
				{
					return WorldInspectPane.TileTabs;
				}
				return null;
			}
		}

		// Token: 0x17001BAA RID: 7082
		// (get) Token: 0x0600B9D5 RID: 47573 RVA: 0x0035758C File Offset: 0x0035578C
		private string TileInspectString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				Vector2 vector = Find.WorldGrid.LongLatOf(this.SelectedTile);
				stringBuilder.Append(vector.y.ToStringLatitude());
				stringBuilder.Append(" ");
				stringBuilder.Append(vector.x.ToStringLongitude());
				Tile tile = Find.WorldGrid[this.SelectedTile];
				if (!tile.biome.impassable)
				{
					stringBuilder.AppendLine();
					stringBuilder.Append(tile.hilliness.GetLabelCap());
				}
				if (tile.Roads != null)
				{
					stringBuilder.AppendLine();
					stringBuilder.Append((from rl in tile.Roads
					select rl.road).MaxBy((RoadDef road) => road.priority).LabelCap);
				}
				if (!Find.World.Impassable(this.SelectedTile))
				{
					string t = (WorldPathGrid.CalculatedMovementDifficultyAt(this.SelectedTile, false, null, null) * Find.WorldGrid.GetRoadMovementDifficultyMultiplier(this.SelectedTile, -1, null)).ToString("0.#");
					stringBuilder.AppendLine();
					stringBuilder.Append("MovementDifficulty".Translate() + ": " + t);
				}
				stringBuilder.AppendLine();
				stringBuilder.Append("AvgTemp".Translate() + ": " + GenTemperature.GetAverageTemperatureLabel(this.SelectedTile));
				return stringBuilder.ToString();
			}
		}

		// Token: 0x0600B9D6 RID: 47574 RVA: 0x000785D0 File Offset: 0x000767D0
		public WorldInspectPane()
		{
			this.layer = WindowLayer.GameUI;
			this.soundAppear = null;
			this.soundClose = null;
			this.closeOnClickedOutside = false;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.preventCameraMotion = false;
		}

		// Token: 0x0600B9D7 RID: 47575 RVA: 0x00078609 File Offset: 0x00076809
		protected override void SetInitialSizeAndPosition()
		{
			base.SetInitialSizeAndPosition();
			this.windowRect.x = 0f;
			this.windowRect.y = this.PaneTopY;
		}

		// Token: 0x0600B9D8 RID: 47576 RVA: 0x0035773C File Offset: 0x0035593C
		public void DrawInspectGizmos()
		{
			WorldInspectPane.tmpObjectsList.Clear();
			WorldRoutePlanner worldRoutePlanner = Find.WorldRoutePlanner;
			List<WorldObject> selected = this.Selected;
			for (int i = 0; i < selected.Count; i++)
			{
				if (!worldRoutePlanner.Active || selected[i] is RoutePlannerWaypoint)
				{
					WorldInspectPane.tmpObjectsList.Add(selected[i]);
				}
			}
			InspectGizmoGrid.DrawInspectGizmoGridFor(WorldInspectPane.tmpObjectsList, out this.mouseoverGizmo);
			WorldInspectPane.tmpObjectsList.Clear();
		}

		// Token: 0x0600B9D9 RID: 47577 RVA: 0x003577B4 File Offset: 0x003559B4
		public string GetLabel(Rect rect)
		{
			if (this.NumSelectedObjects > 0)
			{
				return WorldInspectPaneUtility.AdjustedLabelFor(this.Selected, rect);
			}
			if (this.SelectedTile >= 0)
			{
				return Find.WorldGrid[this.SelectedTile].biome.LabelCap;
			}
			return "error";
		}

		// Token: 0x0600B9DA RID: 47578 RVA: 0x00078632 File Offset: 0x00076832
		public void SelectNextInCell()
		{
			if (!this.AnythingSelected)
			{
				return;
			}
			if (this.NumSelectedObjects > 0)
			{
				Find.WorldSelector.SelectFirstOrNextAt(this.Selected[0].Tile);
				return;
			}
			Find.WorldSelector.SelectFirstOrNextAt(this.SelectedTile);
		}

		// Token: 0x0600B9DB RID: 47579 RVA: 0x00078672 File Offset: 0x00076872
		public void DoPaneContents(Rect rect)
		{
			if (this.NumSelectedObjects > 0)
			{
				InspectPaneFiller.DoPaneContentsFor(Find.WorldSelector.FirstSelectedObject, rect);
				return;
			}
			if (this.SelectedTile >= 0)
			{
				InspectPaneFiller.DrawInspectString(this.TileInspectString, rect);
			}
		}

		// Token: 0x0600B9DC RID: 47580 RVA: 0x00357808 File Offset: 0x00355A08
		public void DoInspectPaneButtons(Rect rect, ref float lineEndWidth)
		{
			WorldObject singleSelectedObject = Find.WorldSelector.SingleSelectedObject;
			if (singleSelectedObject != null || this.SelectedTile >= 0)
			{
				float x = rect.width - 48f;
				if (singleSelectedObject != null)
				{
					Widgets.InfoCardButton(x, 0f, singleSelectedObject);
				}
				else
				{
					Widgets.InfoCardButton(x, 0f, Find.WorldGrid[this.SelectedTile].biome);
				}
				lineEndWidth += 24f;
			}
		}

		// Token: 0x0600B9DD RID: 47581 RVA: 0x000786A3 File Offset: 0x000768A3
		public override void DoWindowContents(Rect rect)
		{
			InspectPaneUtility.InspectPaneOnGUI(rect, this);
		}

		// Token: 0x0600B9DE RID: 47582 RVA: 0x000786AC File Offset: 0x000768AC
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			InspectPaneUtility.UpdateTabs(this);
			if (this.mouseoverGizmo != null)
			{
				this.mouseoverGizmo.GizmoUpdateOnMouseover();
			}
		}

		// Token: 0x0600B9DF RID: 47583 RVA: 0x000786CD File Offset: 0x000768CD
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			InspectPaneUtility.ExtraOnGUI(this);
		}

		// Token: 0x0600B9E0 RID: 47584 RVA: 0x000786DB File Offset: 0x000768DB
		public void CloseOpenTab()
		{
			this.openTabType = null;
		}

		// Token: 0x0600B9E1 RID: 47585 RVA: 0x000786DB File Offset: 0x000768DB
		public void Reset()
		{
			this.openTabType = null;
		}

		// Token: 0x04007EF0 RID: 32496
		private static readonly WITab[] TileTabs = new WITab[]
		{
			new WITab_Terrain(),
			new WITab_Planet()
		};

		// Token: 0x04007EF1 RID: 32497
		private Type openTabType;

		// Token: 0x04007EF2 RID: 32498
		private float recentHeight;

		// Token: 0x04007EF3 RID: 32499
		public Gizmo mouseoverGizmo;

		// Token: 0x04007EF4 RID: 32500
		private static List<object> tmpObjectsList = new List<object>();
	}
}
