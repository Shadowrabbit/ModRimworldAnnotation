using System;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200180D RID: 6157
	public abstract class WITab : InspectTabBase
	{
		// Token: 0x170017A4 RID: 6052
		// (get) Token: 0x06009013 RID: 36883 RVA: 0x00339E4F File Offset: 0x0033804F
		protected WorldObject SelObject
		{
			get
			{
				return Find.WorldSelector.SingleSelectedObject;
			}
		}

		// Token: 0x170017A5 RID: 6053
		// (get) Token: 0x06009014 RID: 36884 RVA: 0x0031996E File Offset: 0x00317B6E
		protected int SelTileID
		{
			get
			{
				return Find.WorldSelector.selectedTile;
			}
		}

		// Token: 0x170017A6 RID: 6054
		// (get) Token: 0x06009015 RID: 36885 RVA: 0x00339E5B File Offset: 0x0033805B
		protected Tile SelTile
		{
			get
			{
				return Find.WorldGrid[this.SelTileID];
			}
		}

		// Token: 0x170017A7 RID: 6055
		// (get) Token: 0x06009016 RID: 36886 RVA: 0x00339E6D File Offset: 0x0033806D
		protected Caravan SelCaravan
		{
			get
			{
				return this.SelObject as Caravan;
			}
		}

		// Token: 0x170017A8 RID: 6056
		// (get) Token: 0x06009017 RID: 36887 RVA: 0x00339E7A File Offset: 0x0033807A
		private WorldInspectPane InspectPane
		{
			get
			{
				return Find.World.UI.inspectPane;
			}
		}

		// Token: 0x170017A9 RID: 6057
		// (get) Token: 0x06009018 RID: 36888 RVA: 0x00339E8B File Offset: 0x0033808B
		protected override bool StillValid
		{
			get
			{
				return WorldRendererUtility.WorldRenderedNow && Find.WindowStack.IsOpen<WorldInspectPane>() && this.InspectPane.CurTabs != null && this.InspectPane.CurTabs.Contains(this);
			}
		}

		// Token: 0x170017AA RID: 6058
		// (get) Token: 0x06009019 RID: 36889 RVA: 0x00339EC4 File Offset: 0x003380C4
		protected override float PaneTopY
		{
			get
			{
				return this.InspectPane.PaneTopY;
			}
		}

		// Token: 0x0600901A RID: 36890 RVA: 0x00339ED1 File Offset: 0x003380D1
		protected override void CloseTab()
		{
			this.InspectPane.CloseOpenTab();
		}
	}
}
