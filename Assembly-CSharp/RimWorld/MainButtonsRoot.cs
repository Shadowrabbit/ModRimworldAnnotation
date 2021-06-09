using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B2F RID: 6959
	public class MainButtonsRoot
	{
		// Token: 0x1700182F RID: 6191
		// (get) Token: 0x06009949 RID: 39241 RVA: 0x002D08CC File Offset: 0x002CEACC
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

		// Token: 0x0600994A RID: 39242 RVA: 0x002D090C File Offset: 0x002CEB0C
		public MainButtonsRoot()
		{
			this.allButtonsInOrder = (from x in DefDatabase<MainButtonDef>.AllDefs
			orderby x.order
			select x).ToList<MainButtonDef>();
		}

		// Token: 0x0600994B RID: 39243 RVA: 0x002D0960 File Offset: 0x002CEB60
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

		// Token: 0x0600994C RID: 39244 RVA: 0x002D09FC File Offset: 0x002CEBFC
		public void HandleLowPriorityShortcuts()
		{
			this.tabs.HandleLowPriorityShortcuts();
			if (WorldRendererUtility.WorldRenderedNow && Current.ProgramState == ProgramState.Playing && Find.CurrentMap != null && KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				Event.current.Use();
				Find.World.renderer.wantedMode = WorldRenderMode.None;
			}
		}

		// Token: 0x0600994D RID: 39245 RVA: 0x002D0A50 File Offset: 0x002CEC50
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

		// Token: 0x040061F2 RID: 25074
		public MainTabsRoot tabs = new MainTabsRoot();

		// Token: 0x040061F3 RID: 25075
		private List<MainButtonDef> allButtonsInOrder;
	}
}
