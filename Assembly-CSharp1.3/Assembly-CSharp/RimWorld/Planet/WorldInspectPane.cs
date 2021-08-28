using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001818 RID: 6168
	public class WorldInspectPane : Window, IInspectPane
	{
		// Token: 0x170017BD RID: 6077
		// (get) Token: 0x06009078 RID: 36984 RVA: 0x0033D752 File Offset: 0x0033B952
		// (set) Token: 0x06009079 RID: 36985 RVA: 0x0033D75A File Offset: 0x0033B95A
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

		// Token: 0x170017BE RID: 6078
		// (get) Token: 0x0600907A RID: 36986 RVA: 0x0033D763 File Offset: 0x0033B963
		// (set) Token: 0x0600907B RID: 36987 RVA: 0x0033D76B File Offset: 0x0033B96B
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

		// Token: 0x170017BF RID: 6079
		// (get) Token: 0x0600907C RID: 36988 RVA: 0x000682C5 File Offset: 0x000664C5
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170017C0 RID: 6080
		// (get) Token: 0x0600907D RID: 36989 RVA: 0x002A8A69 File Offset: 0x002A6C69
		public override Vector2 InitialSize
		{
			get
			{
				return InspectPaneUtility.PaneSizeFor(this);
			}
		}

		// Token: 0x170017C1 RID: 6081
		// (get) Token: 0x0600907E RID: 36990 RVA: 0x0033D774 File Offset: 0x0033B974
		private List<WorldObject> Selected
		{
			get
			{
				return Find.WorldSelector.SelectedObjects;
			}
		}

		// Token: 0x170017C2 RID: 6082
		// (get) Token: 0x0600907F RID: 36991 RVA: 0x0033D780 File Offset: 0x0033B980
		private int NumSelectedObjects
		{
			get
			{
				return Find.WorldSelector.NumSelectedObjects;
			}
		}

		// Token: 0x170017C3 RID: 6083
		// (get) Token: 0x06009080 RID: 36992 RVA: 0x0033D78C File Offset: 0x0033B98C
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

		// Token: 0x170017C4 RID: 6084
		// (get) Token: 0x06009081 RID: 36993 RVA: 0x0033D7B7 File Offset: 0x0033B9B7
		public bool AnythingSelected
		{
			get
			{
				return Find.WorldSelector.AnyObjectOrTileSelected;
			}
		}

		// Token: 0x170017C5 RID: 6085
		// (get) Token: 0x06009082 RID: 36994 RVA: 0x0031996E File Offset: 0x00317B6E
		private int SelectedTile
		{
			get
			{
				return Find.WorldSelector.selectedTile;
			}
		}

		// Token: 0x170017C6 RID: 6086
		// (get) Token: 0x06009083 RID: 36995 RVA: 0x0033D7C3 File Offset: 0x0033B9C3
		private bool SelectedSingleObjectOrTile
		{
			get
			{
				return this.NumSelectedObjects == 1 || (this.NumSelectedObjects == 0 && this.SelectedTile >= 0);
			}
		}

		// Token: 0x170017C7 RID: 6087
		// (get) Token: 0x06009084 RID: 36996 RVA: 0x0033D7E6 File Offset: 0x0033B9E6
		public bool ShouldShowSelectNextInCellButton
		{
			get
			{
				return this.SelectedSingleObjectOrTile;
			}
		}

		// Token: 0x170017C8 RID: 6088
		// (get) Token: 0x06009085 RID: 36997 RVA: 0x0033D7E6 File Offset: 0x0033B9E6
		public bool ShouldShowPaneContents
		{
			get
			{
				return this.SelectedSingleObjectOrTile;
			}
		}

		// Token: 0x170017C9 RID: 6089
		// (get) Token: 0x06009086 RID: 36998 RVA: 0x0033D7EE File Offset: 0x0033B9EE
		public IEnumerable<InspectTabBase> WorldInspectPaneCurTabs
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

		// Token: 0x170017CA RID: 6090
		// (get) Token: 0x06009087 RID: 36999 RVA: 0x0033D824 File Offset: 0x0033BA24
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

		// Token: 0x06009088 RID: 37000 RVA: 0x0033D9D2 File Offset: 0x0033BBD2
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

		// Token: 0x06009089 RID: 37001 RVA: 0x0033DA0B File Offset: 0x0033BC0B
		protected override void SetInitialSizeAndPosition()
		{
			base.SetInitialSizeAndPosition();
			this.windowRect.x = 0f;
			this.windowRect.y = this.PaneTopY;
		}

		// Token: 0x0600908A RID: 37002 RVA: 0x0033DA34 File Offset: 0x0033BC34
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

		// Token: 0x0600908B RID: 37003 RVA: 0x0033DAAC File Offset: 0x0033BCAC
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

		// Token: 0x0600908C RID: 37004 RVA: 0x0033DAFD File Offset: 0x0033BCFD
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

		// Token: 0x0600908D RID: 37005 RVA: 0x0033DB3D File Offset: 0x0033BD3D
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

		// Token: 0x0600908E RID: 37006 RVA: 0x0000313F File Offset: 0x0000133F
		public void DoLabelIcons(string label)
		{
		}

		// Token: 0x0600908F RID: 37007 RVA: 0x0033DB70 File Offset: 0x0033BD70
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

		// Token: 0x06009090 RID: 37008 RVA: 0x0033DBDF File Offset: 0x0033BDDF
		public override void DoWindowContents(Rect rect)
		{
			InspectPaneUtility.InspectPaneOnGUI(rect, this);
		}

		// Token: 0x06009091 RID: 37009 RVA: 0x0033DBE8 File Offset: 0x0033BDE8
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			InspectPaneUtility.UpdateTabs(this);
			if (this.mouseoverGizmo != null)
			{
				this.mouseoverGizmo.GizmoUpdateOnMouseover();
			}
		}

		// Token: 0x06009092 RID: 37010 RVA: 0x0033DC09 File Offset: 0x0033BE09
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			InspectPaneUtility.ExtraOnGUI(this);
		}

		// Token: 0x06009093 RID: 37011 RVA: 0x0033DC17 File Offset: 0x0033BE17
		public void CloseOpenTab()
		{
			this.openTabType = null;
		}

		// Token: 0x06009094 RID: 37012 RVA: 0x0033DC17 File Offset: 0x0033BE17
		public void Reset()
		{
			this.openTabType = null;
		}

		// Token: 0x04005AE5 RID: 23269
		private static readonly WITab[] TileTabs = new WITab[]
		{
			new WITab_Terrain(),
			new WITab_Planet()
		};

		// Token: 0x04005AE6 RID: 23270
		private Type openTabType;

		// Token: 0x04005AE7 RID: 23271
		private float recentHeight;

		// Token: 0x04005AE8 RID: 23272
		public Gizmo mouseoverGizmo;

		// Token: 0x04005AE9 RID: 23273
		private static List<object> tmpObjectsList = new List<object>();
	}
}
