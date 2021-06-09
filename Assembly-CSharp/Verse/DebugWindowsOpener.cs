using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006A7 RID: 1703
	public class DebugWindowsOpener
	{
		// Token: 0x06002C5A RID: 11354 RVA: 0x00023409 File Offset: 0x00021609
		public DebugWindowsOpener()
		{
			this.drawButtonsCached = new Action(this.DrawButtons);
		}

		// Token: 0x06002C5B RID: 11355 RVA: 0x0012ECA8 File Offset: 0x0012CEA8
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
			Find.WindowStack.ImmediateWindow(1593759361, new Rect(vector.x, vector.y, (float)num * 28f - 4f + 1f, num2).Rounded(), WindowLayer.GameUI, this.drawButtonsCached, false, false, 0f);
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

		// Token: 0x06002C5C RID: 11356 RVA: 0x0012EDF8 File Offset: 0x0012CFF8
		private void DrawButtons()
		{
			this.widgetRow.Init(0f, 0f, UIDirection.RightThenUp, 99999f, 4f);
			if (this.widgetRow.ButtonIcon(TexButton.ToggleLog, "Open the debug log.", null, true))
			{
				this.ToggleLogWindow();
			}
			if (this.widgetRow.ButtonIcon(TexButton.ToggleTweak, "Open tweakvalues menu.\n\nThis lets you change internal values.", null, true))
			{
				this.ToggleTweakValuesMenu();
			}
			if (this.widgetRow.ButtonIcon(TexButton.OpenInspectSettings, "Open the view settings.\n\nThis lets you see special debug visuals.", null, true))
			{
				this.ToggleDebugSettingsMenu();
			}
			if (this.widgetRow.ButtonIcon(TexButton.OpenDebugActionsMenu, "Open debug actions menu.\n\nThis lets you spawn items and force various events.", null, true))
			{
				this.ToggleDebugActionsMenu();
			}
			if (this.widgetRow.ButtonIcon(TexButton.OpenDebugActionsMenu, "Open debug logging menu.", null, true))
			{
				this.ToggleDebugLogMenu();
			}
			if (this.widgetRow.ButtonIcon(TexButton.OpenInspector, "Open the inspector.\n\nThis lets you inspect what's happening in the game, down to individual variables.", null, true))
			{
				this.ToggleDebugInspector();
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (this.widgetRow.ButtonIcon(TexButton.ToggleGodMode, "Toggle god mode.\n\nWhen god mode is on, you can build stuff instantly, for free, and sell things that aren't yours.", null, true))
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

		// Token: 0x06002C5D RID: 11357 RVA: 0x0002342E File Offset: 0x0002162E
		private void ToggleLogWindow()
		{
			if (!Find.WindowStack.TryRemove(typeof(EditWindow_Log), true))
			{
				Find.WindowStack.Add(new EditWindow_Log());
			}
		}

		// Token: 0x06002C5E RID: 11358 RVA: 0x00023456 File Offset: 0x00021656
		private void ToggleDebugSettingsMenu()
		{
			if (!Find.WindowStack.TryRemove(typeof(Dialog_DebugSettingsMenu), true))
			{
				Find.WindowStack.Add(new Dialog_DebugSettingsMenu());
			}
		}

		// Token: 0x06002C5F RID: 11359 RVA: 0x0002347E File Offset: 0x0002167E
		private void ToggleDebugActionsMenu()
		{
			if (!Find.WindowStack.TryRemove(typeof(Dialog_DebugActionsMenu), true))
			{
				Find.WindowStack.Add(new Dialog_DebugActionsMenu());
			}
		}

		// Token: 0x06002C60 RID: 11360 RVA: 0x000234A6 File Offset: 0x000216A6
		private void ToggleTweakValuesMenu()
		{
			if (!Find.WindowStack.TryRemove(typeof(EditWindow_TweakValues), true))
			{
				Find.WindowStack.Add(new EditWindow_TweakValues());
			}
		}

		// Token: 0x06002C61 RID: 11361 RVA: 0x000234CE File Offset: 0x000216CE
		private void ToggleDebugLogMenu()
		{
			if (!Find.WindowStack.TryRemove(typeof(Dialog_DebugOutputMenu), true))
			{
				Find.WindowStack.Add(new Dialog_DebugOutputMenu());
			}
		}

		// Token: 0x06002C62 RID: 11362 RVA: 0x000234F6 File Offset: 0x000216F6
		private void ToggleDebugInspector()
		{
			if (!Find.WindowStack.TryRemove(typeof(EditWindow_DebugInspector), true))
			{
				Find.WindowStack.Add(new EditWindow_DebugInspector());
			}
		}

		// Token: 0x06002C63 RID: 11363 RVA: 0x0002351E File Offset: 0x0002171E
		private void ToggleGodMode()
		{
			DebugSettings.godMode = !DebugSettings.godMode;
		}

		// Token: 0x04001DFC RID: 7676
		private Action drawButtonsCached;

		// Token: 0x04001DFD RID: 7677
		private WidgetRow widgetRow = new WidgetRow();
	}
}
