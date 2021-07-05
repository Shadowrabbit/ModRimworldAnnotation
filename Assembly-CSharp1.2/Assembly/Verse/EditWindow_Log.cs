using System;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006EF RID: 1775
	[StaticConstructorOnStartup]
	public class EditWindow_Log : EditWindow
	{
		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x06002D20 RID: 11552 RVA: 0x00023A6F File Offset: 0x00021C6F
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth / 2f, (float)UI.screenHeight / 2f);
			}
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06002D21 RID: 11553 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06002D22 RID: 11554 RVA: 0x00023A8E File Offset: 0x00021C8E
		// (set) Token: 0x06002D23 RID: 11555 RVA: 0x00023A95 File Offset: 0x00021C95
		private static LogMessage SelectedMessage
		{
			get
			{
				return EditWindow_Log.selectedMessage;
			}
			set
			{
				if (EditWindow_Log.selectedMessage == value)
				{
					return;
				}
				EditWindow_Log.selectedMessage = value;
				if (UnityData.IsInMainThread && GUI.GetNameOfFocusedControl() == EditWindow_Log.MessageDetailsControlName)
				{
					UI.UnfocusCurrentControl();
				}
			}
		}

		// Token: 0x06002D24 RID: 11556 RVA: 0x00023AC3 File Offset: 0x00021CC3
		public EditWindow_Log()
		{
			this.optionalTitle = "Debug log";
		}

		// Token: 0x06002D25 RID: 11557 RVA: 0x00023AD6 File Offset: 0x00021CD6
		public static void TryAutoOpen()
		{
			if (EditWindow_Log.canAutoOpen)
			{
				EditWindow_Log.wantsToOpen = true;
			}
		}

		// Token: 0x06002D26 RID: 11558 RVA: 0x00023AE5 File Offset: 0x00021CE5
		public static void ClearSelectedMessage()
		{
			EditWindow_Log.SelectedMessage = null;
			EditWindow_Log.detailsScrollPosition = Vector2.zero;
		}

		// Token: 0x06002D27 RID: 11559 RVA: 0x00023AF7 File Offset: 0x00021CF7
		public static void SelectLastMessage(bool expandDetailsPane = false)
		{
			EditWindow_Log.ClearSelectedMessage();
			EditWindow_Log.SelectedMessage = Log.Messages.LastOrDefault<LogMessage>();
			EditWindow_Log.messagesScrollPosition.y = (float)Log.Messages.Count<LogMessage>() * 30f;
			if (expandDetailsPane)
			{
				EditWindow_Log.detailsPaneHeight = 9999f;
			}
		}

		// Token: 0x06002D28 RID: 11560 RVA: 0x00023B35 File Offset: 0x00021D35
		public static void ClearAll()
		{
			EditWindow_Log.ClearSelectedMessage();
			EditWindow_Log.messagesScrollPosition = Vector2.zero;
		}

		// Token: 0x06002D29 RID: 11561 RVA: 0x00023B46 File Offset: 0x00021D46
		public override void PostClose()
		{
			base.PostClose();
			EditWindow_Log.wantsToOpen = false;
		}

		// Token: 0x06002D2A RID: 11562 RVA: 0x00131F68 File Offset: 0x00130168
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Tiny;
			WidgetRow widgetRow = new WidgetRow(0f, 0f, UIDirection.RightThenUp, 99999f, 4f);
			if (widgetRow.ButtonText("Clear", "Clear all log messages.", true, true))
			{
				Log.Clear();
				EditWindow_Log.ClearAll();
			}
			if (widgetRow.ButtonText("Trace big", "Set the stack trace to be large on screen.", true, true))
			{
				EditWindow_Log.detailsPaneHeight = 700f;
			}
			if (widgetRow.ButtonText("Trace medium", "Set the stack trace to be medium-sized on screen.", true, true))
			{
				EditWindow_Log.detailsPaneHeight = 300f;
			}
			if (widgetRow.ButtonText("Trace small", "Set the stack trace to be small on screen.", true, true))
			{
				EditWindow_Log.detailsPaneHeight = 100f;
			}
			if (EditWindow_Log.canAutoOpen)
			{
				if (widgetRow.ButtonText("Auto-open is ON", "", true, true))
				{
					EditWindow_Log.canAutoOpen = false;
				}
			}
			else if (widgetRow.ButtonText("Auto-open is OFF", "", true, true))
			{
				EditWindow_Log.canAutoOpen = true;
			}
			if (widgetRow.ButtonText("Copy to clipboard", "Copy all messages to the clipboard.", true, true))
			{
				this.CopyAllMessagesToClipboard();
			}
			Text.Font = GameFont.Small;
			Rect rect = new Rect(inRect);
			rect.yMin += 26f;
			rect.yMax = inRect.height;
			if (EditWindow_Log.selectedMessage != null)
			{
				rect.yMax -= EditWindow_Log.detailsPaneHeight;
			}
			Rect detailsRect = new Rect(inRect);
			detailsRect.yMin = rect.yMax;
			this.DoMessagesListing(rect);
			this.DoMessageDetails(detailsRect, inRect);
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Mouse.IsOver(rect))
			{
				EditWindow_Log.ClearSelectedMessage();
			}
			EditWindow_Log.detailsPaneHeight = Mathf.Max(EditWindow_Log.detailsPaneHeight, 10f);
			EditWindow_Log.detailsPaneHeight = Mathf.Min(EditWindow_Log.detailsPaneHeight, inRect.height - 80f);
		}

		// Token: 0x06002D2B RID: 11563 RVA: 0x00023B54 File Offset: 0x00021D54
		public static void Notify_MessageDequeued(LogMessage oldMessage)
		{
			if (EditWindow_Log.SelectedMessage == oldMessage)
			{
				EditWindow_Log.SelectedMessage = null;
			}
		}

		// Token: 0x06002D2C RID: 11564 RVA: 0x00132128 File Offset: 0x00130328
		private void DoMessagesListing(Rect listingRect)
		{
			Rect viewRect = new Rect(0f, 0f, listingRect.width - 16f, this.listingViewHeight + 100f);
			Widgets.BeginScrollView(listingRect, ref EditWindow_Log.messagesScrollPosition, viewRect, true);
			float width = viewRect.width - 28f;
			Text.Font = GameFont.Tiny;
			float num = 0f;
			bool flag = false;
			foreach (LogMessage logMessage in Log.Messages)
			{
				string text = logMessage.text;
				if (text.Length > 1000)
				{
					text = text.Substring(0, 1000);
				}
				float num2 = Math.Min(30f, Text.CalcHeight(text, width));
				GUI.color = new Color(1f, 1f, 1f, 0.7f);
				Widgets.Label(new Rect(4f, num, 28f, num2), logMessage.repeats.ToStringCached());
				Rect rect = new Rect(28f, num, width, num2);
				if (EditWindow_Log.selectedMessage == logMessage)
				{
					GUI.DrawTexture(rect, EditWindow_Log.SelectedMessageTex);
				}
				else if (flag)
				{
					GUI.DrawTexture(rect, EditWindow_Log.AltMessageTex);
				}
				if (Widgets.ButtonInvisible(rect, true))
				{
					EditWindow_Log.ClearSelectedMessage();
					EditWindow_Log.SelectedMessage = logMessage;
				}
				GUI.color = logMessage.Color;
				Widgets.Label(rect, text);
				num += num2;
				flag = !flag;
			}
			if (Event.current.type == EventType.Layout)
			{
				this.listingViewHeight = num;
			}
			Widgets.EndScrollView();
			GUI.color = Color.white;
		}

		// Token: 0x06002D2D RID: 11565 RVA: 0x001322E4 File Offset: 0x001304E4
		private void DoMessageDetails(Rect detailsRect, Rect outRect)
		{
			if (EditWindow_Log.selectedMessage == null)
			{
				return;
			}
			Rect rect = detailsRect;
			rect.height = 7f;
			Rect rect2 = detailsRect;
			rect2.yMin = rect.yMax;
			GUI.DrawTexture(rect, EditWindow_Log.StackTraceBorderTex);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			if (Event.current.type == EventType.MouseDown && Mouse.IsOver(rect))
			{
				this.borderDragging = true;
				Event.current.Use();
			}
			if (this.borderDragging)
			{
				EditWindow_Log.detailsPaneHeight = outRect.height + Mathf.Round(3.5f) - Event.current.mousePosition.y;
			}
			if (Event.current.rawType == EventType.MouseUp)
			{
				this.borderDragging = false;
			}
			GUI.DrawTexture(rect2, EditWindow_Log.StackTraceAreaTex);
			string text = EditWindow_Log.selectedMessage.text + "\n" + EditWindow_Log.selectedMessage.StackTrace;
			GUI.SetNextControlName(EditWindow_Log.MessageDetailsControlName);
			if (text.Length > 15000)
			{
				Widgets.LabelScrollable(rect2, text, ref EditWindow_Log.detailsScrollPosition, false, true, true);
				return;
			}
			Widgets.TextAreaScrollable(rect2, text, ref EditWindow_Log.detailsScrollPosition, true);
		}

		// Token: 0x06002D2E RID: 11566 RVA: 0x001323F8 File Offset: 0x001305F8
		private void CopyAllMessagesToClipboard()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (LogMessage logMessage in Log.Messages)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.AppendLine(logMessage.text);
				stringBuilder.Append(logMessage.StackTrace);
				if (stringBuilder[stringBuilder.Length - 1] != '\n')
				{
					stringBuilder.AppendLine();
				}
			}
			GUIUtility.systemCopyBuffer = stringBuilder.ToString();
		}

		// Token: 0x04001E8D RID: 7821
		private static LogMessage selectedMessage = null;

		// Token: 0x04001E8E RID: 7822
		private static Vector2 messagesScrollPosition;

		// Token: 0x04001E8F RID: 7823
		private static Vector2 detailsScrollPosition;

		// Token: 0x04001E90 RID: 7824
		private static float detailsPaneHeight = 100f;

		// Token: 0x04001E91 RID: 7825
		private static bool canAutoOpen = true;

		// Token: 0x04001E92 RID: 7826
		public static bool wantsToOpen = false;

		// Token: 0x04001E93 RID: 7827
		private float listingViewHeight;

		// Token: 0x04001E94 RID: 7828
		private bool borderDragging;

		// Token: 0x04001E95 RID: 7829
		private const float CountWidth = 28f;

		// Token: 0x04001E96 RID: 7830
		private const float Yinc = 25f;

		// Token: 0x04001E97 RID: 7831
		private const float DetailsPaneBorderHeight = 7f;

		// Token: 0x04001E98 RID: 7832
		private const float DetailsPaneMinHeight = 10f;

		// Token: 0x04001E99 RID: 7833
		private const float ListingMinHeight = 80f;

		// Token: 0x04001E9A RID: 7834
		private const float TopAreaHeight = 26f;

		// Token: 0x04001E9B RID: 7835
		private const float MessageMaxHeight = 30f;

		// Token: 0x04001E9C RID: 7836
		private static readonly Texture2D AltMessageTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.17f, 0.17f, 0.17f, 0.85f));

		// Token: 0x04001E9D RID: 7837
		private static readonly Texture2D SelectedMessageTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.25f, 0.25f, 0.17f, 0.85f));

		// Token: 0x04001E9E RID: 7838
		private static readonly Texture2D StackTraceAreaTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.1f, 0.1f, 0.1f, 0.5f));

		// Token: 0x04001E9F RID: 7839
		private static readonly Texture2D StackTraceBorderTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.3f, 0.3f, 0.3f, 1f));

		// Token: 0x04001EA0 RID: 7840
		private static readonly string MessageDetailsControlName = "MessageDetailsTextArea";
	}
}
