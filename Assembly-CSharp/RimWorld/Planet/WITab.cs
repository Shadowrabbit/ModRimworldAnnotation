using System;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020021D0 RID: 8656
	public abstract class WITab : InspectTabBase
	{
		// Token: 0x17001B84 RID: 7044
		// (get) Token: 0x0600B945 RID: 47429 RVA: 0x00077F62 File Offset: 0x00076162
		protected WorldObject SelObject
		{
			get
			{
				return Find.WorldSelector.SingleSelectedObject;
			}
		}

		// Token: 0x17001B85 RID: 7045
		// (get) Token: 0x0600B946 RID: 47430 RVA: 0x000722C9 File Offset: 0x000704C9
		protected int SelTileID
		{
			get
			{
				return Find.WorldSelector.selectedTile;
			}
		}

		// Token: 0x17001B86 RID: 7046
		// (get) Token: 0x0600B947 RID: 47431 RVA: 0x00077F6E File Offset: 0x0007616E
		protected Tile SelTile
		{
			get
			{
				return Find.WorldGrid[this.SelTileID];
			}
		}

		// Token: 0x17001B87 RID: 7047
		// (get) Token: 0x0600B948 RID: 47432 RVA: 0x00077F80 File Offset: 0x00076180
		protected Caravan SelCaravan
		{
			get
			{
				return this.SelObject as Caravan;
			}
		}

		// Token: 0x17001B88 RID: 7048
		// (get) Token: 0x0600B949 RID: 47433 RVA: 0x00077F8D File Offset: 0x0007618D
		private WorldInspectPane InspectPane
		{
			get
			{
				return Find.World.UI.inspectPane;
			}
		}

		// Token: 0x17001B89 RID: 7049
		// (get) Token: 0x0600B94A RID: 47434 RVA: 0x00077F9E File Offset: 0x0007619E
		protected override bool StillValid
		{
			get
			{
				return WorldRendererUtility.WorldRenderedNow && Find.WindowStack.IsOpen<WorldInspectPane>() && this.InspectPane.CurTabs != null && this.InspectPane.CurTabs.Contains(this);
			}
		}

		// Token: 0x17001B8A RID: 7050
		// (get) Token: 0x0600B94B RID: 47435 RVA: 0x00077FD7 File Offset: 0x000761D7
		protected override float PaneTopY
		{
			get
			{
				return this.InspectPane.PaneTopY;
			}
		}

		// Token: 0x0600B94C RID: 47436 RVA: 0x00077FE4 File Offset: 0x000761E4
		protected override void CloseTab()
		{
			this.InspectPane.CloseOpenTab();
		}
	}
}
