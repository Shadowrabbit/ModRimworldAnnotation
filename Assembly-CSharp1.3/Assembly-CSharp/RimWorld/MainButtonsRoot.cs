using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001364 RID: 4964
	public class MainButtonsRoot
	{
		// Token: 0x1700152E RID: 5422
		// (get) Token: 0x06007866 RID: 30822 RVA: 0x002A6FC4 File Offset: 0x002A51C4
		private int VisibleButtonsCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.allButtonsInOrder.Count; i++)
				{
					if (this.allButtonsInOrder[i].buttonVisible)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x06007867 RID: 30823 RVA: 0x002A7004 File Offset: 0x002A5204
		public MainButtonsRoot()
		{
			this.allButtonsInOrder = (from x in DefDatabase<MainButtonDef>.AllDefs
			orderby x.order
			select x).ToList<MainButtonDef>();
		}

		// Token: 0x06007868 RID: 30824 RVA: 0x002A7058 File Offset: 0x002A5258
		public void MainButtonsOnGUI()
		{
			if (Event.current.type == EventType.Layout)
			{
				return;
			}
			this.DoButtons();
			for (int i = 0; i < this.allButtonsInOrder.Count; i++)
			{
				if (!this.allButtonsInOrder[i].Worker.Disabled && this.allButtonsInOrder[i].hotKey != null && this.allButtonsInOrder[i].hotKey.KeyDownEvent)
				{
					Event.current.Use();
					this.allButtonsInOrder[i].Worker.InterfaceTryActivate();
					return;
				}
			}
		}

		// Token: 0x06007869 RID: 30825 RVA: 0x002A70F4 File Offset: 0x002A52F4
		public void HandleLowPriorityShortcuts()
		{
			this.tabs.HandleLowPriorityShortcuts();
			if (WorldRendererUtility.WorldRenderedNow && Current.ProgramState == ProgramState.Playing && Find.CurrentMap != null && KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				Event.current.Use();
				Find.World.renderer.wantedMode = WorldRenderMode.None;
			}
		}

		// Token: 0x0600786A RID: 30826 RVA: 0x002A7148 File Offset: 0x002A5348
		private void DoButtons()
		{
			float num = 0f;
			for (int i = 0; i < this.allButtonsInOrder.Count; i++)
			{
				if (this.allButtonsInOrder[i].buttonVisible)
				{
					num += (this.allButtonsInOrder[i].minimized ? 0.5f : 1f);
				}
			}
			GUI.color = Color.white;
			int num2 = (int)((float)UI.screenWidth / num);
			int num3 = num2 / 2;
			int num4 = this.allButtonsInOrder.FindLastIndex((MainButtonDef x) => x.buttonVisible);
			int num5 = 0;
			for (int j = 0; j < this.allButtonsInOrder.Count; j++)
			{
				if (this.allButtonsInOrder[j].buttonVisible)
				{
					int num6 = this.allButtonsInOrder[j].minimized ? num3 : num2;
					if (j == num4)
					{
						num6 = UI.screenWidth - num5;
					}
					Rect rect = new Rect((float)num5, (float)(UI.screenHeight - 35), (float)num6, 35f);
					this.allButtonsInOrder[j].Worker.DoButton(rect);
					num5 += num6;
				}
			}
		}

		// Token: 0x040042E5 RID: 17125
		public MainTabsRoot tabs = new MainTabsRoot();

		// Token: 0x040042E6 RID: 17126
		private List<MainButtonDef> allButtonsInOrder;
	}
}
