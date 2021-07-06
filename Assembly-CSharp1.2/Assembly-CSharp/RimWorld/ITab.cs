using System;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001AF9 RID: 6905
	public abstract class ITab : InspectTabBase
	{
		// Token: 0x170017EC RID: 6124
		// (get) Token: 0x06009806 RID: 38918 RVA: 0x000653F1 File Offset: 0x000635F1
		protected object SelObject
		{
			get
			{
				return Find.Selector.SingleSelectedObject;
			}
		}

		// Token: 0x170017ED RID: 6125
		// (get) Token: 0x06009807 RID: 38919 RVA: 0x000653FD File Offset: 0x000635FD
		protected Thing SelThing
		{
			get
			{
				return Find.Selector.SingleSelectedThing;
			}
		}

		// Token: 0x170017EE RID: 6126
		// (get) Token: 0x06009808 RID: 38920 RVA: 0x00065409 File Offset: 0x00063609
		protected Pawn SelPawn
		{
			get
			{
				return this.SelThing as Pawn;
			}
		}

		// Token: 0x170017EF RID: 6127
		// (get) Token: 0x06009809 RID: 38921 RVA: 0x00065416 File Offset: 0x00063616
		private MainTabWindow_Inspect InspectPane
		{
			get
			{
				return (MainTabWindow_Inspect)MainButtonDefOf.Inspect.TabWindow;
			}
		}

		// Token: 0x170017F0 RID: 6128
		// (get) Token: 0x0600980A RID: 38922 RVA: 0x002CA094 File Offset: 0x002C8294
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

		// Token: 0x170017F1 RID: 6129
		// (get) Token: 0x0600980B RID: 38923 RVA: 0x00065427 File Offset: 0x00063627
		protected override float PaneTopY
		{
			get
			{
				return this.InspectPane.PaneTopY;
			}
		}

		// Token: 0x0600980C RID: 38924 RVA: 0x00065434 File Offset: 0x00063634
		protected override void CloseTab()
		{
			this.InspectPane.CloseOpenTab();
			SoundDefOf.TabClose.PlayOneShotOnCamera(null);
		}
	}
}
