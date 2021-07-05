using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003B8 RID: 952
	public class DebugWindowsOpener
	{
		// Token: 0x06001D75 RID: 7541 RVA: 0x000B7BE7 File Offset: 0x000B5DE7
		public DebugWindowsOpener()
		{
			this.drawButtonsCached = new Action(this.DrawButtons);
		}

		// Token: 0x06001D76 RID: 7542 RVA: 0x000B7C0C File Offset: 0x000B5E0C
		public void DevToolStarterOnGUI()
		{
			if (!Prefs.DevMode)
			{
				return;
			}
			Vector2 vector = new Vector2((float)UI.screenWidth * 0.5f, 3f);
			int num = 6;
			if (Current.ProgramState == ProgramState.Playing)
			{
				num += 2;
			}
			float num2 = 25f;
			if (Current.ProgramState == ProgramState.Playing && DebugSettings.godMode)
			{
				num2 += 15f;
			}
			Find.WindowStack.ImmediateWindow(1593759361, new Rect(vector.x, vector.y, (float)num * 28f - 4f + 1f, num2).Rounded(), WindowLayer.GameUI, this.drawButtonsCached, false, false, 0f, null);
			if (KeyBindingDefOf.Dev_ToggleDebugLog.KeyDownEvent)
			{
				this.ToggleLogWindow();
				Event.current.Use();
			}
			if (KeyBindingDefOf.Dev_ToggleDebugActionsMenu.KeyDownEvent)
			{
				this.ToggleDebugActionsMenu();
				Event.current.Use();
			}
			if (KeyBindingDefOf.Dev_ToggleDebugLogMenu.KeyDownEvent)
			{
				this.ToggleDebugLogMenu();
				Event.current.Use();
			}
			if (KeyBindingDefOf.Dev_ToggleDebugSettingsMenu.KeyDownEvent)
			{
				this.ToggleDebugSettingsMenu();
				Event.current.Use();
			}
			if (KeyBindingDefOf.Dev_ToggleDebugInspector.KeyDownEvent)
			{
				this.ToggleDebugInspector();
				Event.current.Use();
			}
			if (Current.ProgramState == ProgramState.Playing && KeyBindingDefOf.Dev_ToggleGodMode.KeyDownEvent)
			{
				this.ToggleGodMode();
				Event.current.Use();
			}
		}

		// Token: 0x06001D77 RID: 7543 RVA: 0x000B7D60 File Offset: 0x000B5F60
		private void DrawButtons()
		{
			this.widgetRow.Init(0f, 0f, UIDirection.RightThenUp, 99999f, 4f);
			if (this.widgetRow.ButtonIcon(TexButton.ToggleLog, "Open the debug log.", null, null, null, true))
			{
				this.ToggleLogWindow();
			}
			if (this.widgetRow.ButtonIcon(TexButton.ToggleTweak, "Open tweakvalues menu.\n\nThis lets you change internal values.", null, null, null, true))
			{
				this.ToggleTweakValuesMenu();
			}
			if (this.widgetRow.ButtonIcon(TexButton.OpenInspectSettings, "Open the view settings.\n\nThis lets you see special debug visuals.", null, null, null, true))
			{
				this.ToggleDebugSettingsMenu();
			}
			if (this.widgetRow.ButtonIcon(TexButton.OpenDebugActionsMenu, "Open debug actions menu.\n\nThis lets you spawn items and force various events.", null, null, null, true))
			{
				this.ToggleDebugActionsMenu();
			}
			if (this.widgetRow.ButtonIcon(TexButton.OpenDebugActionsMenu, "Open debug logging menu.", null, null, null, true))
			{
				this.ToggleDebugLogMenu();
			}
			if (this.widgetRow.ButtonIcon(TexButton.OpenInspector, "Open the inspector.\n\nThis lets you inspect what's happening in the game, down to individual variables.", null, null, null, true))
			{
				this.ToggleDebugInspector();
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (this.widgetRow.ButtonIcon(TexButton.ToggleGodMode, "Toggle god mode.\n\nWhen god mode is on, you can build stuff instantly, for free, and sell things that aren't yours.", null, null, null, true))
				{
					this.ToggleGodMode();
				}
				if (DebugSettings.godMode)
				{
					Text.Font = GameFont.Tiny;
					Widgets.Label(new Rect(0f, 25f, 200f, 100f), "God mode");
				}
				bool pauseOnError = Prefs.PauseOnError;
				this.widgetRow.ToggleableIcon(ref pauseOnError, TexButton.TogglePauseOnError, "Pause the game when an error is logged.", null, null);
				Prefs.PauseOnError = pauseOnError;
			}
		}

		// Token: 0x06001D78 RID: 7544 RVA: 0x000B7F7C File Offset: 0x000B617C
		private void ToggleLogWindow()
		{
			if (!Find.WindowStack.TryRemove(typeof(EditWindow_Log), true))
			{
				Find.WindowStack.Add(new EditWindow_Log());
			}
		}

		// Token: 0x06001D79 RID: 7545 RVA: 0x000B7FA4 File Offset: 0x000B61A4
		private void ToggleDebugSettingsMenu()
		{
			if (!Find.WindowStack.TryRemove(typeof(Dialog_DebugSettingsMenu), true))
			{
				Find.WindowStack.Add(new Dialog_DebugSettingsMenu());
			}
		}

		// Token: 0x06001D7A RID: 7546 RVA: 0x000B7FCC File Offset: 0x000B61CC
		private void ToggleDebugActionsMenu()
		{
			if (!Find.WindowStack.TryRemove(typeof(Dialog_DebugActionsMenu), true))
			{
				Find.WindowStack.Add(new Dialog_DebugActionsMenu());
			}
		}

		// Token: 0x06001D7B RID: 7547 RVA: 0x000B7FF4 File Offset: 0x000B61F4
		private void ToggleTweakValuesMenu()
		{
			if (!Find.WindowStack.TryRemove(typeof(EditWindow_TweakValues), true))
			{
				Find.WindowStack.Add(new EditWindow_TweakValues());
			}
		}

		// Token: 0x06001D7C RID: 7548 RVA: 0x000B801C File Offset: 0x000B621C
		private void ToggleDebugLogMenu()
		{
			if (!Find.WindowStack.TryRemove(typeof(Dialog_DebugOutputMenu), true))
			{
				Find.WindowStack.Add(new Dialog_DebugOutputMenu());
			}
		}

		// Token: 0x06001D7D RID: 7549 RVA: 0x000B8044 File Offset: 0x000B6244
		private void ToggleDebugInspector()
		{
			if (!Find.WindowStack.TryRemove(typeof(EditWindow_DebugInspector), true))
			{
				Find.WindowStack.Add(new EditWindow_DebugInspector());
			}
		}

		// Token: 0x06001D7E RID: 7550 RVA: 0x000B806C File Offset: 0x000B626C
		private void ToggleGodMode()
		{
			DebugSettings.godMode = !DebugSettings.godMode;
		}

		// Token: 0x040011AA RID: 4522
		private Action drawButtonsCached;

		// Token: 0x040011AB RID: 4523
		private WidgetRow widgetRow = new WidgetRow();
	}
}
