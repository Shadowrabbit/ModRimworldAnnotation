using System;
using RimWorld;
using UnityEngine;
using Verse.Noise;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200042A RID: 1066
	public abstract class UIRoot
	{
		// Token: 0x06002008 RID: 8200 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Init()
		{
		}

		// Token: 0x06002009 RID: 8201 RVA: 0x000C6578 File Offset: 0x000C4778
		public virtual void UIRootOnGUI()
		{
			UnityGUIBugsFixer.OnGUI();
			OriginalEventUtility.RecordOriginalEvent(Event.current);
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
			OriginalEventUtility.Reset();
		}

		// Token: 0x0600200A RID: 8202 RVA: 0x000C6625 File Offset: 0x000C4825
		public virtual void UIRootUpdate()
		{
			ScreenshotTaker.Update();
			DragSliderManager.DragSlidersUpdate();
			this.windows.WindowsUpdate();
			MouseoverSounds.ResolveFrame();
			UIHighlighter.UIHighlighterUpdate();
			Messages.Update();
		}

		// Token: 0x0600200B RID: 8203 RVA: 0x000C664B File Offset: 0x000C484B
		private void CheckOpenLogWindow()
		{
			if (EditWindow_Log.wantsToOpen && !Find.WindowStack.IsOpen(typeof(EditWindow_Log)))
			{
				Find.WindowStack.Add(new EditWindow_Log());
				EditWindow_Log.wantsToOpen = false;
			}
		}

		// Token: 0x0400136C RID: 4972
		public WindowStack windows = new WindowStack();

		// Token: 0x0400136D RID: 4973
		protected DebugWindowsOpener debugWindowOpener = new DebugWindowsOpener();

		// Token: 0x0400136E RID: 4974
		public ScreenshotModeHandler screenshotMode = new ScreenshotModeHandler();

		// Token: 0x0400136F RID: 4975
		private ShortcutKeys shortcutKeys = new ShortcutKeys();

		// Token: 0x04001370 RID: 4976
		public FeedbackFloaters feedbackFloaters = new FeedbackFloaters();
	}
}
