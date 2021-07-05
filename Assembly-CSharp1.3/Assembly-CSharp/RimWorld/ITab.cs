using System;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200133C RID: 4924
	public abstract class ITab : InspectTabBase
	{
		// Token: 0x170014E4 RID: 5348
		// (get) Token: 0x0600772F RID: 30511 RVA: 0x0029D814 File Offset: 0x0029BA14
		protected object SelObject
		{
			get
			{
				return Find.Selector.SingleSelectedObject;
			}
		}

		// Token: 0x170014E5 RID: 5349
		// (get) Token: 0x06007730 RID: 30512 RVA: 0x0029D820 File Offset: 0x0029BA20
		protected Thing SelThing
		{
			get
			{
				return Find.Selector.SingleSelectedThing;
			}
		}

		// Token: 0x170014E6 RID: 5350
		// (get) Token: 0x06007731 RID: 30513 RVA: 0x0029D82C File Offset: 0x0029BA2C
		protected Pawn SelPawn
		{
			get
			{
				return this.SelThing as Pawn;
			}
		}

		// Token: 0x170014E7 RID: 5351
		// (get) Token: 0x06007732 RID: 30514 RVA: 0x0029D839 File Offset: 0x0029BA39
		private MainTabWindow_Inspect InspectPane
		{
			get
			{
				return (MainTabWindow_Inspect)MainButtonDefOf.Inspect.TabWindow;
			}
		}

		// Token: 0x170014E8 RID: 5352
		// (get) Token: 0x06007733 RID: 30515 RVA: 0x0029D84C File Offset: 0x0029BA4C
		protected override bool StillValid
		{
			get
			{
				if (Find.MainTabsRoot.OpenTab != MainButtonDefOf.Inspect)
				{
					return false;
				}
				MainTabWindow_Inspect mainTabWindow_Inspect = (MainTabWindow_Inspect)Find.MainTabsRoot.OpenTab.TabWindow;
				return mainTabWindow_Inspect.CurTabs != null && mainTabWindow_Inspect.CurTabs.Contains(this);
			}
		}

		// Token: 0x170014E9 RID: 5353
		// (get) Token: 0x06007734 RID: 30516 RVA: 0x0029D897 File Offset: 0x0029BA97
		protected override float PaneTopY
		{
			get
			{
				return this.InspectPane.PaneTopY;
			}
		}

		// Token: 0x06007735 RID: 30517 RVA: 0x0029D8A4 File Offset: 0x0029BAA4
		protected override void CloseTab()
		{
			this.InspectPane.CloseOpenTab();
			SoundDefOf.TabClose.PlayOneShotOnCamera(null);
		}
	}
}
