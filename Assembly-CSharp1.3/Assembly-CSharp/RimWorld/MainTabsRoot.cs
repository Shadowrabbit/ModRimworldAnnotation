using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001374 RID: 4980
	public class MainTabsRoot
	{
		// Token: 0x17001558 RID: 5464
		// (get) Token: 0x06007926 RID: 31014 RVA: 0x002AEE38 File Offset: 0x002AD038
		public MainButtonDef OpenTab
		{
			get
			{
				MainTabWindow mainTabWindow = Find.WindowStack.WindowOfType<MainTabWindow>();
				if (mainTabWindow == null)
				{
					return null;
				}
				return mainTabWindow.def;
			}
		}

		// Token: 0x06007927 RID: 31015 RVA: 0x002AEE5C File Offset: 0x002AD05C
		public void HandleLowPriorityShortcuts()
		{
			this.AutoCloseInspectionTabIfNothingSelected(true);
			if (Find.Selector.NumSelected == 0 && Event.current.type == EventType.MouseDown && Event.current.button == 1 && !WorldRendererUtility.WorldRenderedNow)
			{
				Event.current.Use();
				MainButtonDefOf.Architect.Worker.InterfaceTryActivate();
			}
			if (this.OpenTab != null && this.OpenTab != MainButtonDefOf.Inspect && Event.current.type == EventType.MouseDown && Event.current.button != 2)
			{
				this.EscapeCurrentTab(true);
				if (Event.current.button == 0)
				{
					Find.Selector.ClearSelection();
					Find.WorldSelector.ClearSelection();
				}
			}
		}

		// Token: 0x06007928 RID: 31016 RVA: 0x002AEF0A File Offset: 0x002AD10A
		public void EscapeCurrentTab(bool playSound = true)
		{
			this.SetCurrentTab(null, playSound);
		}

		// Token: 0x06007929 RID: 31017 RVA: 0x002AEF14 File Offset: 0x002AD114
		public void SetCurrentTab(MainButtonDef tab, bool playSound = true)
		{
			if (tab == this.OpenTab)
			{
				return;
			}
			this.ToggleTab(tab, playSound);
		}

		// Token: 0x0600792A RID: 31018 RVA: 0x002AEF28 File Offset: 0x002AD128
		public void ToggleTab(MainButtonDef newTab, bool playSound = true)
		{
			if (this.OpenTab == null && newTab == null)
			{
				return;
			}
			if (this.OpenTab == newTab)
			{
				Find.WindowStack.TryRemove(this.OpenTab.TabWindow, true);
				if (playSound)
				{
					SoundDefOf.TabClose.PlayOneShotOnCamera(null);
					return;
				}
			}
			else
			{
				if (this.OpenTab != null)
				{
					Find.WindowStack.TryRemove(this.OpenTab.TabWindow, false);
				}
				if (newTab != null)
				{
					Find.WindowStack.Add(newTab.TabWindow);
				}
				if (playSound)
				{
					if (newTab == null)
					{
						SoundDefOf.TabClose.PlayOneShotOnCamera(null);
					}
					else
					{
						SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
					}
				}
				if (TutorSystem.TutorialMode && newTab != null)
				{
					TutorSystem.Notify_Event("Open-MainTab-" + newTab.defName);
				}
			}
		}

		// Token: 0x0600792B RID: 31019 RVA: 0x002AEFE7 File Offset: 0x002AD1E7
		public void Notify_SelectedObjectDespawned()
		{
			this.AutoCloseInspectionTabIfNothingSelected(false);
		}

		// Token: 0x0600792C RID: 31020 RVA: 0x002AEFF0 File Offset: 0x002AD1F0
		private void AutoCloseInspectionTabIfNothingSelected(bool playSound)
		{
			if (this.OpenTab == MainButtonDefOf.Inspect && (Find.Selector.NumSelected == 0 || WorldRendererUtility.WorldRenderedNow))
			{
				this.EscapeCurrentTab(playSound);
			}
		}
	}
}
