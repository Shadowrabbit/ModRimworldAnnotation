using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RimWorld;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Verse
{
	// Token: 0x02000077 RID: 119
	public static class LongEventHandler
	{
		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000480 RID: 1152 RVA: 0x0000A108 File Offset: 0x00008308
		public static bool ShouldWaitForEvent
		{
			get
			{
				return LongEventHandler.AnyEventNowOrWaiting && ((LongEventHandler.currentEvent != null && !LongEventHandler.currentEvent.UseStandardWindow) || (Find.UIRoot == null || Find.WindowStack == null));
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000481 RID: 1153 RVA: 0x0000A139 File Offset: 0x00008339
		public static bool AnyEventNowOrWaiting
		{
			get
			{
				return LongEventHandler.currentEvent != null || LongEventHandler.eventQueue.Count > 0;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x00087C38 File Offset: 0x00085E38
		public static bool AnyEventWhichDoesntUseStandardWindowNowOrWaiting
		{
			get
			{
				LongEventHandler.QueuedLongEvent queuedLongEvent = LongEventHandler.currentEvent;
				if (queuedLongEvent != null && !queuedLongEvent.UseStandardWindow)
				{
					return true;
				}
				return LongEventHandler.eventQueue.Any((LongEventHandler.QueuedLongEvent x) => !x.UseStandardWindow);
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000483 RID: 1155 RVA: 0x0000A151 File Offset: 0x00008351
		public static bool ForcePause
		{
			get
			{
				return LongEventHandler.AnyEventNowOrWaiting;
			}
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00087C84 File Offset: 0x00085E84
		public static void QueueLongEvent(Action action, string textKey, bool doAsynchronously, Action<Exception> exceptionHandler, bool showExtraUIInfo = true)
		{
			LongEventHandler.QueuedLongEvent queuedLongEvent = new LongEventHandler.QueuedLongEvent();
			queuedLongEvent.eventAction = action;
			queuedLongEvent.eventTextKey = textKey;
			queuedLongEvent.doAsynchronously = doAsynchronously;
			queuedLongEvent.exceptionHandler = exceptionHandler;
			queuedLongEvent.canEverUseStandardWindow = !LongEventHandler.AnyEventWhichDoesntUseStandardWindowNowOrWaiting;
			queuedLongEvent.showExtraUIInfo = showExtraUIInfo;
			LongEventHandler.eventQueue.Enqueue(queuedLongEvent);
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00087CD4 File Offset: 0x00085ED4
		public static void QueueLongEvent(IEnumerable action, string textKey, Action<Exception> exceptionHandler = null, bool showExtraUIInfo = true)
		{
			LongEventHandler.QueuedLongEvent queuedLongEvent = new LongEventHandler.QueuedLongEvent();
			queuedLongEvent.eventActionEnumerator = action.GetEnumerator();
			queuedLongEvent.eventTextKey = textKey;
			queuedLongEvent.doAsynchronously = false;
			queuedLongEvent.exceptionHandler = exceptionHandler;
			queuedLongEvent.canEverUseStandardWindow = !LongEventHandler.AnyEventWhichDoesntUseStandardWindowNowOrWaiting;
			queuedLongEvent.showExtraUIInfo = showExtraUIInfo;
			LongEventHandler.eventQueue.Enqueue(queuedLongEvent);
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00087D28 File Offset: 0x00085F28
		public static void QueueLongEvent(Action preLoadLevelAction, string levelToLoad, string textKey, bool doAsynchronously, Action<Exception> exceptionHandler, bool showExtraUIInfo = true)
		{
			LongEventHandler.QueuedLongEvent queuedLongEvent = new LongEventHandler.QueuedLongEvent();
			queuedLongEvent.eventAction = preLoadLevelAction;
			queuedLongEvent.levelToLoad = levelToLoad;
			queuedLongEvent.eventTextKey = textKey;
			queuedLongEvent.doAsynchronously = doAsynchronously;
			queuedLongEvent.exceptionHandler = exceptionHandler;
			queuedLongEvent.canEverUseStandardWindow = !LongEventHandler.AnyEventWhichDoesntUseStandardWindowNowOrWaiting;
			queuedLongEvent.showExtraUIInfo = showExtraUIInfo;
			LongEventHandler.eventQueue.Enqueue(queuedLongEvent);
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0000A158 File Offset: 0x00008358
		public static void ClearQueuedEvents()
		{
			LongEventHandler.eventQueue.Clear();
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00087D80 File Offset: 0x00085F80
		public static void LongEventsOnGUI()
		{
			if (LongEventHandler.currentEvent == null)
			{
				GameplayTipWindow.ResetTipTimer();
				return;
			}
			float num = LongEventHandler.StatusRectSize.x;
			object currentEventTextLock = LongEventHandler.CurrentEventTextLock;
			lock (currentEventTextLock)
			{
				Text.Font = GameFont.Small;
				num = Mathf.Max(num, Text.CalcSize(LongEventHandler.currentEvent.eventText + "...").x + 40f);
			}
			bool flag2 = Find.UIRoot != null && !LongEventHandler.currentEvent.UseStandardWindow && LongEventHandler.currentEvent.showExtraUIInfo;
			bool flag3 = Find.UIRoot != null && Current.Game != null && !LongEventHandler.currentEvent.UseStandardWindow && LongEventHandler.currentEvent.showExtraUIInfo;
			Vector2 vector = flag3 ? ModSummaryWindow.GetEffectiveSize() : Vector2.zero;
			float num2 = LongEventHandler.StatusRectSize.y;
			if (flag3)
			{
				num2 += 17f + vector.y;
			}
			if (flag2)
			{
				num2 += 17f + GameplayTipWindow.WindowSize.y;
			}
			float num3 = ((float)UI.screenHeight - num2) / 2f;
			Vector2 vector2 = new Vector2(((float)UI.screenWidth - GameplayTipWindow.WindowSize.x) / 2f, num3 + LongEventHandler.StatusRectSize.y + 17f);
			Vector2 offset = new Vector2(((float)UI.screenWidth - vector.x) / 2f, vector2.y + GameplayTipWindow.WindowSize.y + 17f);
			Rect rect = new Rect(((float)UI.screenWidth - num) / 2f, num3, num, LongEventHandler.StatusRectSize.y);
			rect = rect.Rounded();
			if (!LongEventHandler.currentEvent.UseStandardWindow || Find.UIRoot == null || Find.WindowStack == null)
			{
				if (UIMenuBackgroundManager.background == null)
				{
					UIMenuBackgroundManager.background = new UI_BackgroundMain();
				}
				UIMenuBackgroundManager.background.BackgroundOnGUI();
				Widgets.DrawShadowAround(rect);
				Widgets.DrawWindowBackground(rect);
				LongEventHandler.DrawLongEventWindowContents(rect);
				if (flag2)
				{
					GameplayTipWindow.DrawWindow(vector2, false);
				}
				if (flag3)
				{
					ModSummaryWindow.DrawWindow(offset, false);
					TooltipHandler.DoTooltipGUI();
					return;
				}
			}
			else
			{
				LongEventHandler.DrawLongEventWindow(rect);
				if (flag2)
				{
					GameplayTipWindow.DrawWindow(vector2, true);
				}
			}
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00087FB0 File Offset: 0x000861B0
		private static void DrawLongEventWindow(Rect statusRect)
		{
			Find.WindowStack.ImmediateWindow(62893994, statusRect, WindowLayer.Super, delegate
			{
				LongEventHandler.DrawLongEventWindowContents(statusRect.AtZero());
			}, true, false, 1f);
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00087FF4 File Offset: 0x000861F4
		public static void LongEventsUpdate(out bool sceneChanged)
		{
			sceneChanged = false;
			if (LongEventHandler.currentEvent != null)
			{
				if (LongEventHandler.currentEvent.eventActionEnumerator != null)
				{
					LongEventHandler.UpdateCurrentEnumeratorEvent();
				}
				else if (LongEventHandler.currentEvent.doAsynchronously)
				{
					LongEventHandler.UpdateCurrentAsynchronousEvent();
				}
				else
				{
					LongEventHandler.UpdateCurrentSynchronousEvent(out sceneChanged);
				}
			}
			if (LongEventHandler.currentEvent == null && LongEventHandler.eventQueue.Count > 0)
			{
				LongEventHandler.currentEvent = LongEventHandler.eventQueue.Dequeue();
				if (LongEventHandler.currentEvent.eventTextKey == null)
				{
					LongEventHandler.currentEvent.eventText = "";
					return;
				}
				LongEventHandler.currentEvent.eventText = LongEventHandler.currentEvent.eventTextKey.Translate();
			}
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0000A164 File Offset: 0x00008364
		public static void ExecuteWhenFinished(Action action)
		{
			LongEventHandler.toExecuteWhenFinished.Add(action);
			if ((LongEventHandler.currentEvent == null || LongEventHandler.currentEvent.ShouldWaitUntilDisplayed) && !LongEventHandler.executingToExecuteWhenFinished)
			{
				LongEventHandler.ExecuteToExecuteWhenFinished();
			}
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00088094 File Offset: 0x00086294
		public static void SetCurrentEventText(string newText)
		{
			object currentEventTextLock = LongEventHandler.CurrentEventTextLock;
			lock (currentEventTextLock)
			{
				if (LongEventHandler.currentEvent != null)
				{
					LongEventHandler.currentEvent.eventText = newText;
				}
			}
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x000880E4 File Offset: 0x000862E4
		private static void UpdateCurrentEnumeratorEvent()
		{
			try
			{
				float num = Time.realtimeSinceStartup + 0.1f;
				while (LongEventHandler.currentEvent.eventActionEnumerator.MoveNext())
				{
					if (num <= Time.realtimeSinceStartup)
					{
						return;
					}
				}
				IDisposable disposable = LongEventHandler.currentEvent.eventActionEnumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
				LongEventHandler.currentEvent = null;
				LongEventHandler.eventThread = null;
				LongEventHandler.levelLoadOp = null;
				LongEventHandler.ExecuteToExecuteWhenFinished();
			}
			catch (Exception ex)
			{
				Log.Error("Exception from long event: " + ex, false);
				if (LongEventHandler.currentEvent != null)
				{
					IDisposable disposable2 = LongEventHandler.currentEvent.eventActionEnumerator as IDisposable;
					if (disposable2 != null)
					{
						disposable2.Dispose();
					}
					if (LongEventHandler.currentEvent.exceptionHandler != null)
					{
						LongEventHandler.currentEvent.exceptionHandler(ex);
					}
				}
				LongEventHandler.currentEvent = null;
				LongEventHandler.eventThread = null;
				LongEventHandler.levelLoadOp = null;
			}
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x000881BC File Offset: 0x000863BC
		private static void UpdateCurrentAsynchronousEvent()
		{
			if (LongEventHandler.eventThread == null)
			{
				LongEventHandler.eventThread = new Thread(delegate()
				{
					LongEventHandler.RunEventFromAnotherThread(LongEventHandler.currentEvent.eventAction);
				});
				LongEventHandler.eventThread.Start();
				return;
			}
			if (!LongEventHandler.eventThread.IsAlive)
			{
				bool flag = false;
				if (!LongEventHandler.currentEvent.levelToLoad.NullOrEmpty())
				{
					if (LongEventHandler.levelLoadOp == null)
					{
						LongEventHandler.levelLoadOp = SceneManager.LoadSceneAsync(LongEventHandler.currentEvent.levelToLoad);
					}
					else if (LongEventHandler.levelLoadOp.isDone)
					{
						flag = true;
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					LongEventHandler.currentEvent = null;
					LongEventHandler.eventThread = null;
					LongEventHandler.levelLoadOp = null;
					LongEventHandler.ExecuteToExecuteWhenFinished();
				}
			}
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0008826C File Offset: 0x0008646C
		private static void UpdateCurrentSynchronousEvent(out bool sceneChanged)
		{
			sceneChanged = false;
			if (LongEventHandler.currentEvent.ShouldWaitUntilDisplayed)
			{
				return;
			}
			try
			{
				if (LongEventHandler.currentEvent.eventAction != null)
				{
					LongEventHandler.currentEvent.eventAction();
				}
				if (!LongEventHandler.currentEvent.levelToLoad.NullOrEmpty())
				{
					SceneManager.LoadScene(LongEventHandler.currentEvent.levelToLoad);
					sceneChanged = true;
				}
				LongEventHandler.currentEvent = null;
				LongEventHandler.eventThread = null;
				LongEventHandler.levelLoadOp = null;
				LongEventHandler.ExecuteToExecuteWhenFinished();
			}
			catch (Exception ex)
			{
				Log.Error("Exception from long event: " + ex, false);
				if (LongEventHandler.currentEvent != null && LongEventHandler.currentEvent.exceptionHandler != null)
				{
					LongEventHandler.currentEvent.exceptionHandler(ex);
				}
				LongEventHandler.currentEvent = null;
				LongEventHandler.eventThread = null;
				LongEventHandler.levelLoadOp = null;
			}
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0008833C File Offset: 0x0008653C
		private static void RunEventFromAnotherThread(Action action)
		{
			CultureInfoUtility.EnsureEnglish();
			try
			{
				if (action != null)
				{
					action();
				}
			}
			catch (Exception ex)
			{
				Log.Error("Exception from asynchronous event: " + ex, false);
				try
				{
					if (LongEventHandler.currentEvent != null && LongEventHandler.currentEvent.exceptionHandler != null)
					{
						LongEventHandler.currentEvent.exceptionHandler(ex);
					}
				}
				catch (Exception arg)
				{
					Log.Error("Exception was thrown while trying to handle exception. Exception: " + arg, false);
				}
			}
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x000883C4 File Offset: 0x000865C4
		private static void ExecuteToExecuteWhenFinished()
		{
			if (LongEventHandler.executingToExecuteWhenFinished)
			{
				Log.Warning("Already executing.", false);
				return;
			}
			LongEventHandler.executingToExecuteWhenFinished = true;
			if (LongEventHandler.toExecuteWhenFinished.Count > 0)
			{
				DeepProfiler.Start("ExecuteToExecuteWhenFinished()");
			}
			for (int i = 0; i < LongEventHandler.toExecuteWhenFinished.Count; i++)
			{
				DeepProfiler.Start(LongEventHandler.toExecuteWhenFinished[i].Method.DeclaringType.ToString() + " -> " + LongEventHandler.toExecuteWhenFinished[i].Method.ToString());
				try
				{
					LongEventHandler.toExecuteWhenFinished[i]();
				}
				catch (Exception arg)
				{
					Log.Error("Could not execute post-long-event action. Exception: " + arg, false);
				}
				finally
				{
					DeepProfiler.End();
				}
			}
			if (LongEventHandler.toExecuteWhenFinished.Count > 0)
			{
				DeepProfiler.End();
			}
			LongEventHandler.toExecuteWhenFinished.Clear();
			LongEventHandler.executingToExecuteWhenFinished = false;
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x000884C0 File Offset: 0x000866C0
		private static void DrawLongEventWindowContents(Rect rect)
		{
			if (LongEventHandler.currentEvent == null)
			{
				return;
			}
			if (Event.current.type == EventType.Repaint)
			{
				LongEventHandler.currentEvent.alreadyDisplayed = true;
			}
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			float num = 0f;
			if (LongEventHandler.levelLoadOp != null)
			{
				float f = 1f;
				if (!LongEventHandler.levelLoadOp.isDone)
				{
					f = LongEventHandler.levelLoadOp.progress;
				}
				TaggedString taggedString = "LoadingAssets".Translate() + " " + f.ToStringPercent();
				num = Text.CalcSize(taggedString).x;
				Widgets.Label(rect, taggedString);
			}
			else
			{
				object currentEventTextLock = LongEventHandler.CurrentEventTextLock;
				lock (currentEventTextLock)
				{
					num = Text.CalcSize(LongEventHandler.currentEvent.eventText).x;
					Widgets.Label(rect, LongEventHandler.currentEvent.eventText);
				}
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			rect.xMin = rect.center.x + num / 2f;
			Widgets.Label(rect, (!LongEventHandler.currentEvent.UseAnimatedDots) ? "..." : GenText.MarchingEllipsis(0f));
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0400020A RID: 522
		private static Queue<LongEventHandler.QueuedLongEvent> eventQueue = new Queue<LongEventHandler.QueuedLongEvent>();

		// Token: 0x0400020B RID: 523
		private static LongEventHandler.QueuedLongEvent currentEvent = null;

		// Token: 0x0400020C RID: 524
		private static Thread eventThread = null;

		// Token: 0x0400020D RID: 525
		private static AsyncOperation levelLoadOp = null;

		// Token: 0x0400020E RID: 526
		private static List<Action> toExecuteWhenFinished = new List<Action>();

		// Token: 0x0400020F RID: 527
		private static bool executingToExecuteWhenFinished = false;

		// Token: 0x04000210 RID: 528
		private static readonly object CurrentEventTextLock = new object();

		// Token: 0x04000211 RID: 529
		private static readonly Vector2 StatusRectSize = new Vector2(240f, 75f);

		// Token: 0x02000078 RID: 120
		private class QueuedLongEvent
		{
			// Token: 0x170000CF RID: 207
			// (get) Token: 0x06000494 RID: 1172 RVA: 0x0000A190 File Offset: 0x00008390
			public bool UseAnimatedDots
			{
				get
				{
					return this.doAsynchronously || this.eventActionEnumerator != null;
				}
			}

			// Token: 0x170000D0 RID: 208
			// (get) Token: 0x06000495 RID: 1173 RVA: 0x0000A1A5 File Offset: 0x000083A5
			public bool ShouldWaitUntilDisplayed
			{
				get
				{
					return !this.alreadyDisplayed && this.UseStandardWindow && !this.eventText.NullOrEmpty();
				}
			}

			// Token: 0x170000D1 RID: 209
			// (get) Token: 0x06000496 RID: 1174 RVA: 0x0000A1C7 File Offset: 0x000083C7
			public bool UseStandardWindow
			{
				get
				{
					return this.canEverUseStandardWindow && !this.doAsynchronously && this.eventActionEnumerator == null;
				}
			}

			// Token: 0x04000212 RID: 530
			public Action eventAction;

			// Token: 0x04000213 RID: 531
			public IEnumerator eventActionEnumerator;

			// Token: 0x04000214 RID: 532
			public string levelToLoad;

			// Token: 0x04000215 RID: 533
			public string eventTextKey = "";

			// Token: 0x04000216 RID: 534
			public string eventText = "";

			// Token: 0x04000217 RID: 535
			public bool doAsynchronously;

			// Token: 0x04000218 RID: 536
			public Action<Exception> exceptionHandler;

			// Token: 0x04000219 RID: 537
			public bool alreadyDisplayed;

			// Token: 0x0400021A RID: 538
			public bool canEverUseStandardWindow = true;

			// Token: 0x0400021B RID: 539
			public bool showExtraUIInfo = true;
		}
	}
}
