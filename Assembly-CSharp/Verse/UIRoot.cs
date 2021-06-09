using System;
using RimWorld;
using UnityEngine;
using Verse.Noise;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000762 RID: 1890
	public abstract class UIRoot
	{
		// Token: 0x06002FA4 RID: 12196 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Init()
		{
		}

		// Token: 0x06002FA5 RID: 12197 RVA: 0x0013BD74 File Offset: 0x00139F74
		public virtual void UIRootOnGUI()
		{
			UnityGUIBugsFixer.OnGUI();
			Text.StartOfOnGUI();
			this.CheckOpenLogWindow();
			DelayedErrorWindowRequest.DelayedErrorWindowRequestOnGUI();
			DebugInputLogger.InputLogOnGUI();
			if (!this.screenshotMode.FiltersCurrentEvent)
			{
				this.debugWindowOpener.DevToolStarterOnGUI();
			}
			this.windows.HandleEventsHighPriority();
			this.screenshotMode.ScreenshotModesOnGUI();
			if (!this.screenshotMode.FiltersCurrentEvent)
			{
				TooltipHandler.DoTooltipGUI();
				this.feedbackFloaters.FeedbackOnGUI();
				DragSliderManager.DragSlidersOnGUI();
				Messages.MessagesDoGUI();
			}
			this.shortcutKeys.ShortcutKeysOnGUI();
			NoiseDebugUI.NoiseDebugOnGUI();
			Debug.developerConsoleVisible = false;
			if (Current.Game != null)
			{
				GameComponentUtility.GameComponentOnGUI();
			}
		}

		// Token: 0x06002FA6 RID: 12198 RVA: 0x0002570F File Offset: 0x0002390F
		public virtual void UIRootUpdate()
		{
			ScreenshotTaker.Update();
			DragSliderManager.DragSlidersUpdate();
			this.windows.WindowsUpdate();
			MouseoverSounds.ResolveFrame();
			UIHighlighter.UIHighlighterUpdate();
			Messages.Update();
		}

		// Token: 0x06002FA7 RID: 12199 RVA: 0x00025735 File Offset: 0x00023935
		private void CheckOpenLogWindow()
		{
			if (EditWindow_Log.wantsToOpen && !Find.WindowStack.IsOpen(typeof(EditWindow_Log)))
			{
				Find.WindowStack.Add(new EditWindow_Log());
				EditWindow_Log.wantsToOpen = false;
			}
		}

		// Token: 0x0400204B RID: 8267
		public WindowStack windows = new WindowStack();

		// Token: 0x0400204C RID: 8268
		protected DebugWindowsOpener debugWindowOpener = new DebugWindowsOpener();

		// Token: 0x0400204D RID: 8269
		public ScreenshotModeHandler screenshotMode = new ScreenshotModeHandler();

		// Token: 0x0400204E RID: 8270
		private ShortcutKeys shortcutKeys = new ShortcutKeys();

		// Token: 0x0400204F RID: 8271
		public FeedbackFloaters feedbackFloaters = new FeedbackFloaters();
	}
}
