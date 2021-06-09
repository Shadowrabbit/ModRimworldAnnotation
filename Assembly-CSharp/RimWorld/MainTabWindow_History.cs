using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001B3B RID: 6971
	[StaticConstructorOnStartup]
	public class MainTabWindow_History : MainTabWindow
	{
		// Token: 0x17001838 RID: 6200
		// (get) Token: 0x0600997B RID: 39291 RVA: 0x000664D4 File Offset: 0x000646D4
		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(1010f, 640f);
			}
		}

		// Token: 0x0600997C RID: 39292 RVA: 0x002D0FD0 File Offset: 0x002CF1D0
		public override void PreOpen()
		{
			base.PreOpen();
			this.tabs.Clear();
			this.tabs.Add(new TabRecord("Graph".Translate(), delegate()
			{
				MainTabWindow_History.curTab = MainTabWindow_History.HistoryTab.Graph;
			}, () => MainTabWindow_History.curTab == MainTabWindow_History.HistoryTab.Graph));
			this.tabs.Add(new TabRecord("Messages".Translate(), delegate()
			{
				MainTabWindow_History.curTab = MainTabWindow_History.HistoryTab.Messages;
			}, () => MainTabWindow_History.curTab == MainTabWindow_History.HistoryTab.Messages));
			this.tabs.Add(new TabRecord("Statistics".Translate(), delegate()
			{
				MainTabWindow_History.curTab = MainTabWindow_History.HistoryTab.Statistics;
			}, () => MainTabWindow_History.curTab == MainTabWindow_History.HistoryTab.Statistics));
			this.historyAutoRecorderGroup = Find.History.Groups().FirstOrDefault<HistoryAutoRecorderGroup>();
			if (this.historyAutoRecorderGroup != null)
			{
				this.graphSection = new FloatRange(0f, (float)Find.TickManager.TicksGame / 60000f);
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].wealthWatcher.ForceRecount(false);
			}
		}

		// Token: 0x0600997D RID: 39293 RVA: 0x002D116C File Offset: 0x002CF36C
		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
			Rect rect2 = rect;
			rect2.yMin += 45f;
			TabDrawer.DrawTabs(rect2, this.tabs, 200f);
			switch (MainTabWindow_History.curTab)
			{
			case MainTabWindow_History.HistoryTab.Graph:
				this.DoGraphPage(rect2);
				return;
			case MainTabWindow_History.HistoryTab.Messages:
				this.DoMessagesPage(rect2);
				return;
			case MainTabWindow_History.HistoryTab.Statistics:
				this.DoStatisticsPage(rect2);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600997E RID: 39294 RVA: 0x002D11D8 File Offset: 0x002CF3D8
		private void DoStatisticsPage(Rect rect)
		{
			rect.yMin += 17f;
			GUI.BeginGroup(rect);
			StringBuilder stringBuilder = new StringBuilder();
			TimeSpan timeSpan = new TimeSpan(0, 0, (int)Find.GameInfo.RealPlayTimeInteracting);
			stringBuilder.AppendLine("Playtime".Translate() + ": " + timeSpan.Days + "LetterDay".Translate() + " " + timeSpan.Hours + "LetterHour".Translate() + " " + timeSpan.Minutes + "LetterMinute".Translate() + " " + timeSpan.Seconds + "LetterSecond".Translate());
			stringBuilder.AppendLine("Storyteller".Translate() + ": " + Find.Storyteller.def.LabelCap);
			stringBuilder.AppendLine("Difficulty".Translate() + ": " + Find.Storyteller.difficulty.LabelCap);
			if (Find.CurrentMap != null)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("ThisMapColonyWealthTotal".Translate() + ": " + Find.CurrentMap.wealthWatcher.WealthTotal.ToString("F0"));
				stringBuilder.AppendLine("ThisMapColonyWealthItems".Translate() + ": " + Find.CurrentMap.wealthWatcher.WealthItems.ToString("F0"));
				stringBuilder.AppendLine("ThisMapColonyWealthBuildings".Translate() + ": " + Find.CurrentMap.wealthWatcher.WealthBuildings.ToString("F0"));
				stringBuilder.AppendLine("ThisMapColonyWealthColonistsAndTameAnimals".Translate() + ": " + Find.CurrentMap.wealthWatcher.WealthPawns.ToString("F0"));
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("NumThreatBigs".Translate() + ": " + Find.StoryWatcher.statsRecord.numThreatBigs);
			stringBuilder.AppendLine("NumEnemyRaids".Translate() + ": " + Find.StoryWatcher.statsRecord.numRaidsEnemy);
			stringBuilder.AppendLine();
			if (Find.CurrentMap != null)
			{
				stringBuilder.AppendLine("ThisMapDamageTaken".Translate() + ": " + Find.CurrentMap.damageWatcher.DamageTakenEver);
			}
			stringBuilder.AppendLine("ColonistsKilled".Translate() + ": " + Find.StoryWatcher.statsRecord.colonistsKilled);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("ColonistsLaunched".Translate() + ": " + Find.StoryWatcher.statsRecord.colonistsLaunched);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.Label(new Rect(0f, 0f, 400f, 400f), stringBuilder.ToString());
			GUI.EndGroup();
		}

		// Token: 0x0600997F RID: 39295 RVA: 0x002D15D8 File Offset: 0x002CF7D8
		private void DoMessagesPage(Rect rect)
		{
			rect.yMin += 10f;
			Widgets.CheckboxLabeled(new Rect(rect.x, rect.y, 200f, 30f), "ShowLetters".Translate(), ref MainTabWindow_History.showLetters, false, null, null, true);
			Widgets.CheckboxLabeled(new Rect(rect.x + 200f, rect.y, 200f, 30f), "ShowMessages".Translate(), ref MainTabWindow_History.showMessages, false, null, null, true);
			rect.yMin += 40f;
			bool flag = false;
			Rect outRect = rect;
			Rect viewRect = new Rect(0f, 0f, outRect.width / 2f - 16f, this.messagesLastHeight);
			List<IArchivable> archivablesListForReading = Find.Archive.ArchivablesListForReading;
			Rect rect2 = new Rect(rect.x + rect.width / 2f + 10f, rect.y, rect.width / 2f - 10f - 16f, rect.height);
			this.displayedMessageIndex = -1;
			Widgets.BeginScrollView(outRect, ref this.messagesScrollPos, viewRect, true);
			float num = 0f;
			for (int i = archivablesListForReading.Count - 1; i >= 0; i--)
			{
				if ((MainTabWindow_History.showLetters || (!(archivablesListForReading[i] is Letter) && !(archivablesListForReading[i] is ArchivedDialog))) && (MainTabWindow_History.showMessages || !(archivablesListForReading[i] is Message)))
				{
					flag = true;
					if (i > this.displayedMessageIndex)
					{
						this.displayedMessageIndex = i;
					}
					if (num + 30f >= this.messagesScrollPos.y && num <= this.messagesScrollPos.y + outRect.height)
					{
						this.DoArchivableRow(new Rect(0f, num, viewRect.width - 5f, 30f), archivablesListForReading[i], i);
					}
					num += 30f;
				}
			}
			this.messagesLastHeight = num;
			Widgets.EndScrollView();
			if (flag)
			{
				if (this.displayedMessageIndex >= 0)
				{
					TaggedString label = archivablesListForReading[this.displayedMessageIndex].ArchivedTooltip.TruncateHeight(rect2.width - 10f, rect2.height - 10f, this.truncationCache);
					Widgets.Label(rect2.ContractedBy(5f), label);
					return;
				}
			}
			else
			{
				Widgets.NoneLabel(rect.yMin + 3f, rect.width, "(" + "NoMessages".Translate() + ")");
			}
		}

		// Token: 0x06009980 RID: 39296 RVA: 0x002D18A4 File Offset: 0x002CFAA4
		private void DoArchivableRow(Rect rect, IArchivable archivable, int index)
		{
			if (index % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}
			Widgets.DrawHighlightIfMouseover(rect);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Rect rect2 = rect;
			Rect rect3 = rect2;
			rect3.width = 30f;
			rect2.xMin += 35f;
			float num;
			if (Find.Archive.IsPinned(archivable))
			{
				num = 1f;
			}
			else if (Mouse.IsOver(rect3))
			{
				num = 0.25f;
			}
			else
			{
				num = 0f;
			}
			Rect position = new Rect(rect3.x + (rect3.width - 22f) / 2f, rect3.y + (rect3.height - 22f) / 2f, 22f, 22f).Rounded();
			if (num > 0f)
			{
				GUI.color = new Color(1f, 1f, 1f, num);
				GUI.DrawTexture(position, MainTabWindow_History.PinTex);
			}
			else
			{
				GUI.color = MainTabWindow_History.PinOutlineColor;
				GUI.DrawTexture(position, MainTabWindow_History.PinOutlineTex);
			}
			GUI.color = Color.white;
			Rect rect4 = rect2;
			Rect outerRect = rect2;
			outerRect.width = 30f;
			rect2.xMin += 35f;
			Texture archivedIcon = archivable.ArchivedIcon;
			if (archivedIcon != null)
			{
				GUI.color = archivable.ArchivedIconColor;
				Widgets.DrawTextureFitted(outerRect, archivedIcon, 0.8f);
				GUI.color = Color.white;
			}
			Rect rect5 = rect2;
			rect5.width = 80f;
			rect2.xMin += 85f;
			Vector2 location = (Find.CurrentMap != null) ? Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile) : default(Vector2);
			GUI.color = new Color(0.75f, 0.75f, 0.75f);
			string str = GenDate.DateShortStringAt((long)GenDate.TickGameToAbs(archivable.CreatedTicksGame), location);
			Widgets.Label(rect5, str.Truncate(rect5.width, null));
			GUI.color = Color.white;
			Rect rect6 = rect2;
			Widgets.Label(rect6, archivable.ArchivedLabel.Truncate(rect6.width, null));
			GenUI.ResetLabelAlign();
			Text.WordWrap = true;
			TooltipHandler.TipRegionByKey(rect3, "PinArchivableTip", 200);
			if (Mouse.IsOver(rect4))
			{
				this.displayedMessageIndex = index;
			}
			if (Widgets.ButtonInvisible(rect3, true))
			{
				if (Find.Archive.IsPinned(archivable))
				{
					Find.Archive.Unpin(archivable);
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
				else
				{
					Find.Archive.Pin(archivable);
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
			}
			if (Widgets.ButtonInvisible(rect4, true))
			{
				if (Event.current.button == 1)
				{
					LookTargets lookTargets = archivable.LookTargets;
					if (CameraJumper.CanJump(lookTargets.TryGetPrimaryTarget()))
					{
						CameraJumper.TryJumpAndSelect(lookTargets.TryGetPrimaryTarget());
						Find.MainTabsRoot.EscapeCurrentTab(true);
						return;
					}
				}
				else
				{
					archivable.OpenArchived();
				}
			}
		}

		// Token: 0x06009981 RID: 39297 RVA: 0x002D1B88 File Offset: 0x002CFD88
		private void DoGraphPage(Rect rect)
		{
			rect.yMin += 17f;
			GUI.BeginGroup(rect);
			Rect graphRect = new Rect(0f, 0f, rect.width, 450f);
			Rect legendRect = new Rect(0f, graphRect.yMax, rect.width / 2f, 40f);
			Rect rect2 = new Rect(0f, legendRect.yMax, rect.width, 40f);
			if (this.historyAutoRecorderGroup != null)
			{
				MainTabWindow_History.marks.Clear();
				List<Tale> allTalesListForReading = Find.TaleManager.AllTalesListForReading;
				for (int i = 0; i < allTalesListForReading.Count; i++)
				{
					Tale tale = allTalesListForReading[i];
					if (tale.def.type == TaleType.PermanentHistorical)
					{
						float x = (float)GenDate.TickAbsToGame(tale.date) / 60000f;
						MainTabWindow_History.marks.Add(new CurveMark(x, tale.ShortSummary, tale.def.historyGraphColor));
					}
				}
				this.historyAutoRecorderGroup.DrawGraph(graphRect, legendRect, this.graphSection, MainTabWindow_History.marks);
			}
			Text.Font = GameFont.Small;
			float num = (float)Find.TickManager.TicksGame / 60000f;
			if (Widgets.ButtonText(new Rect(legendRect.xMin + legendRect.width, legendRect.yMin, 110f, 40f), "Last30Days".Translate(), true, true, true))
			{
				this.graphSection = new FloatRange(Mathf.Max(0f, num - 30f), num);
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(legendRect.xMin + legendRect.width + 110f + 4f, legendRect.yMin, 110f, 40f), "Last100Days".Translate(), true, true, true))
			{
				this.graphSection = new FloatRange(Mathf.Max(0f, num - 100f), num);
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(legendRect.xMin + legendRect.width + 228f, legendRect.yMin, 110f, 40f), "Last300Days".Translate(), true, true, true))
			{
				this.graphSection = new FloatRange(Mathf.Max(0f, num - 300f), num);
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(legendRect.xMin + legendRect.width + 342f, legendRect.yMin, 110f, 40f), "AllDays".Translate(), true, true, true))
			{
				this.graphSection = new FloatRange(0f, num);
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(rect2.x, rect2.y, 110f, 40f), "SelectGraph".Translate(), true, true, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				List<HistoryAutoRecorderGroup> list2 = Find.History.Groups();
				for (int j = 0; j < list2.Count; j++)
				{
					HistoryAutoRecorderGroup groupLocal = list2[j];
					if (!groupLocal.def.devModeOnly || Prefs.DevMode)
					{
						list.Add(new FloatMenuOption(groupLocal.def.LabelCap, delegate()
						{
							this.historyAutoRecorderGroup = groupLocal;
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
				}
				FloatMenu window = new FloatMenu(list, "SelectGraph".Translate(), false);
				Find.WindowStack.Add(window);
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.HistoryTab, KnowledgeAmount.Total);
			}
			GUI.EndGroup();
		}

		// Token: 0x04006203 RID: 25091
		private HistoryAutoRecorderGroup historyAutoRecorderGroup;

		// Token: 0x04006204 RID: 25092
		private FloatRange graphSection;

		// Token: 0x04006205 RID: 25093
		private Vector2 messagesScrollPos;

		// Token: 0x04006206 RID: 25094
		private float messagesLastHeight;

		// Token: 0x04006207 RID: 25095
		private List<TabRecord> tabs = new List<TabRecord>();

		// Token: 0x04006208 RID: 25096
		private int displayedMessageIndex;

		// Token: 0x04006209 RID: 25097
		private static MainTabWindow_History.HistoryTab curTab = MainTabWindow_History.HistoryTab.Graph;

		// Token: 0x0400620A RID: 25098
		private static bool showLetters = true;

		// Token: 0x0400620B RID: 25099
		private static bool showMessages;

		// Token: 0x0400620C RID: 25100
		private const float MessagesRowHeight = 30f;

		// Token: 0x0400620D RID: 25101
		private const float PinColumnSize = 30f;

		// Token: 0x0400620E RID: 25102
		private const float PinSize = 22f;

		// Token: 0x0400620F RID: 25103
		private const float IconColumnSize = 30f;

		// Token: 0x04006210 RID: 25104
		private const float DateSize = 80f;

		// Token: 0x04006211 RID: 25105
		private const float SpaceBetweenColumns = 5f;

		// Token: 0x04006212 RID: 25106
		private static readonly Texture2D PinTex = ContentFinder<Texture2D>.Get("UI/Icons/Pin", true);

		// Token: 0x04006213 RID: 25107
		private static readonly Texture2D PinOutlineTex = ContentFinder<Texture2D>.Get("UI/Icons/Pin-Outline", true);

		// Token: 0x04006214 RID: 25108
		private static readonly Color PinOutlineColor = new Color(0.25f, 0.25f, 0.25f, 0.5f);

		// Token: 0x04006215 RID: 25109
		private Dictionary<string, string> truncationCache = new Dictionary<string, string>();

		// Token: 0x04006216 RID: 25110
		private static List<CurveMark> marks = new List<CurveMark>();

		// Token: 0x02001B3C RID: 6972
		private enum HistoryTab : byte
		{
			// Token: 0x04006218 RID: 25112
			Graph,
			// Token: 0x04006219 RID: 25113
			Messages,
			// Token: 0x0400621A RID: 25114
			Statistics
		}
	}
}
