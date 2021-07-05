using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001B34 RID: 6964
	public class MainTabsRoot
	{
		// Token: 0x17001831 RID: 6193
		// (get) Token: 0x06009958 RID: 39256 RVA: 0x002D0C78 File Offset: 0x002CEE78
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

		// Token: 0x06009959 RID: 39257 RVA: 0x002D0C9C File Offset: 0x002CEE9C
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

		// Token: 0x0600995A RID: 39258 RVA: 0x000662D9 File Offset: 0x000644D9
		public void EscapeCurrentTab(bool playSound = true)
		{
			this.SetCurrentTab(null, playSound);
		}

		// Token: 0x0600995B RID: 39259 RVA: 0x000662E3 File Offset: 0x000644E3
		public void SetCurrentTab(MainButtonDef tab, bool playSound = true)
		{
			if (tab == this.OpenTab)
			{
				return;
			}
			this.ToggleTab(tab, playSound);
		}

		// Token: 0x0600995C RID: 39260 RVA: 0x002D0D4C File Offset: 0x002CEF4C
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

		// Token: 0x0600995D RID: 39261 RVA: 0x000662F7 File Offset: 0x000644F7
		public void Notify_SelectedObjectDespawned()
		{
			this.AutoCloseInspectionTabIfNothingSelected(false);
		}

		// Token: 0x0600995E RID: 39262 RVA: 0x00066300 File Offset: 0x00064500
		private void AutoCloseInspectionTabIfNothingSelected(bool playSound)
		{
			if (this.OpenTab == MainButtonDefOf.Inspect && (Find.Selector.NumSelected == 0 || WorldRendererUtility.WorldRenderedNow))
			{
				this.EscapeCurrentTab(playSound);
			}
		}
	}
}
