using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001369 RID: 4969
	[StaticConstructorOnStartup]
	public class MainTabWindow_History : MainTabWindow
	{
		// Token: 0x17001535 RID: 5429
		// (get) Token: 0x06007883 RID: 30851 RVA: 0x002A79B3 File Offset: 0x002A5BB3
		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(1010f, 640f);
			}
		}

		// Token: 0x06007884 RID: 30852 RVA: 0x002A79C4 File Offset: 0x002A5BC4
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

		// Token: 0x06007885 RID: 30853 RVA: 0x002A7B60 File Offset: 0x002A5D60
		public override void DoWindowContents(Rect rect)
		{
			Rect rect2 = rect;
			rect2.yMin += 45f;
			TabDrawer.DrawTabs<TabRecord>(rect2, this.tabs, 200f);
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

		// Token: 0x06007886 RID: 30854 RVA: 0x002A7BC4 File Offset: 0x002A5DC4
		private void DoStatisticsPage(Rect rect)
		{
			rect.yMin += 17f;
			GUI.BeginGroup(rect);
			StringBuilder stringBuilder = new StringBuilder();
			TimeSpan timeSpan = new TimeSpan(0, 0, (int)Find.GameInfo.RealPlayTimeInteracting);
			stringBuilder.AppendLine("Playtime".Translate() + ": " + timeSpan.Days + "LetterDay".Translate() + " " + timeSpan.Hours + "LetterHour".Translate() + " " + timeSpan.Minutes + "LetterMinute".Translate() + " " + timeSpan.Seconds + "LetterSecond".Translate());
			stringBuilder.AppendLine("Storyteller".Translate() + ": " + Find.Storyteller.def.LabelCap);
			stringBuilder.AppendLine("Difficulty".Translate() + ": " + Find.Storyteller.difficultyDef.LabelCap);
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

		// Token: 0x06007887 RID: 30855 RVA: 0x002A7FC4 File Offset: 0x002A61C4
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

		// Token: 0x06007888 RID: 30856 RVA: 0x002A8290 File Offset: 0x002A6490
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

		// Token: 0x06007889 RID: 30857 RVA: 0x002A8574 File Offset: 0x002A6774
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
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
				}
				FloatMenu window = new FloatMenu(list, "SelectGraph".Translate(), false);
				Find.WindowStack.Add(window);
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.HistoryTab, KnowledgeAmount.Total);
			}
			GUI.EndGroup();
		}

		// Token: 0x040042F3 RID: 17139
		private HistoryAutoRecorderGroup historyAutoRecorderGroup;

		// Token: 0x040042F4 RID: 17140
		private FloatRange graphSection;

		// Token: 0x040042F5 RID: 17141
		private Vector2 messagesScrollPos;

		// Token: 0x040042F6 RID: 17142
		private float messagesLastHeight;

		// Token: 0x040042F7 RID: 17143
		private List<TabRecord> tabs = new List<TabRecord>();

		// Token: 0x040042F8 RID: 17144
		private int displayedMessageIndex;

		// Token: 0x040042F9 RID: 17145
		private static MainTabWindow_History.HistoryTab curTab = MainTabWindow_History.HistoryTab.Graph;

		// Token: 0x040042FA RID: 17146
		private static bool showLetters = true;

		// Token: 0x040042FB RID: 17147
		private static bool showMessages;

		// Token: 0x040042FC RID: 17148
		private const float MessagesRowHeight = 30f;

		// Token: 0x040042FD RID: 17149
		private const float PinColumnSize = 30f;

		// Token: 0x040042FE RID: 17150
		private const float PinSize = 22f;

		// Token: 0x040042FF RID: 17151
		private const float IconColumnSize = 30f;

		// Token: 0x04004300 RID: 17152
		private const float DateSize = 80f;

		// Token: 0x04004301 RID: 17153
		private const float SpaceBetweenColumns = 5f;

		// Token: 0x04004302 RID: 17154
		private static readonly Texture2D PinTex = ContentFinder<Texture2D>.Get("UI/Icons/Pin", true);

		// Token: 0x04004303 RID: 17155
		private static readonly Texture2D PinOutlineTex = ContentFinder<Texture2D>.Get("UI/Icons/Pin-Outline", true);

		// Token: 0x04004304 RID: 17156
		private static readonly Color PinOutlineColor = new Color(0.25f, 0.25f, 0.25f, 0.5f);

		// Token: 0x04004305 RID: 17157
		private Dictionary<string, string> truncationCache = new Dictionary<string, string>();

		// Token: 0x04004306 RID: 17158
		private static List<CurveMark> marks = new List<CurveMark>();

		// Token: 0x02002773 RID: 10099
		private enum HistoryTab : byte
		{
			// Token: 0x04009563 RID: 38243
			Graph,
			// Token: 0x04009564 RID: 38244
			Messages,
			// Token: 0x04009565 RID: 38245
			Statistics
		}
	}
}
