using System;
using RimWorld.Planet;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001363 RID: 4963
	public class MainButtonWorker_ToggleWorld : MainButtonWorker
	{
		// Token: 0x06007864 RID: 30820 RVA: 0x002A6EF8 File Offset: 0x002A50F8
		public override void Activate()
		{
			if (Find.World.renderer.wantedMode == WorldRenderMode.None)
			{
				Find.World.renderer.wantedMode = WorldRenderMode.Planet;
				if (this.resetViewNextTime)
				{
					this.resetViewNextTime = false;
					Find.World.UI.Reset();
				}
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.FormCaravan, OpportunityType.Important);
				Find.MainTabsRoot.EscapeCurrentTab(false);
				SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
				return;
			}
			if (Find.MainTabsRoot.OpenTab != null && Find.MainTabsRoot.OpenTab != MainButtonDefOf.Inspect)
			{
				Find.MainTabsRoot.EscapeCurrentTab(false);
				SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
				return;
			}
			Find.World.renderer.wantedMode = WorldRenderMode.None;
			SoundDefOf.TabClose.PlayOneShotOnCamera(null);
		}

		// Token: 0x040042E4 RID: 17124
		public bool resetViewNextTime = true;
	}
}
