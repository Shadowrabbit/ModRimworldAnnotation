using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200136E RID: 4974
	[StaticConstructorOnStartup]
	public class MainTabWindow_Quests : MainTabWindow
	{
		// Token: 0x1700154D RID: 5453
		// (get) Token: 0x060078C1 RID: 30913 RVA: 0x002A79B3 File Offset: 0x002A5BB3
		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(1010f, 640f);
			}
		}

		// Token: 0x060078C2 RID: 30914 RVA: 0x002A8FFC File Offset: 0x002A71FC
		public override void PreOpen()
		{
			base.PreOpen();
			if (MainTabWindow_Quests.RatingIcon == null)
			{
				MainTabWindow_Quests.RatingIcon = ContentFinder<Texture2D>.Get("UI/Icons/ChallengeRatingIcon", true);
			}
			this.tabs.Clear();
			this.tabs.Add(new TabRecord("AvailableQuests".Translate(), delegate()
			{
				this.curTab = MainTabWindow_Quests.QuestsTab.Available;
				this.selected = null;
			}, () => this.curTab == MainTabWindow_Quests.QuestsTab.Available));
			this.tabs.Add(new TabRecord("ActiveQuests".Translate(), delegate()
			{
				this.curTab = MainTabWindow_Quests.QuestsTab.Active;
				this.selected = null;
			}, () => this.curTab == MainTabWindow_Quests.QuestsTab.Active));
			this.tabs.Add(new TabRecord("HistoricalQuests".Translate(), delegate()
			{
				this.curTab = MainTabWindow_Quests.QuestsTab.Historical;
				this.selected = null;
			}, () => this.curTab == MainTabWindow_Quests.QuestsTab.Historical));
			this.Select(this.selected);
		}

		// Token: 0x060078C3 RID: 30915 RVA: 0x002A90E8 File Offset: 0x002A72E8
		public override void DoWindowContents(Rect rect)
		{
			Rect rect2 = rect;
			rect2.yMin += 4f;
			rect2.xMax = rect2.width * 0.36f;
			rect2.yMax -= this.DoRewardsPrefsButton(rect2).height + 4f;
			this.DoQuestsList(rect2);
			Rect rect3 = rect;
			rect3.yMin += 4f;
			rect3.xMin = rect2.xMax + 17f;
			this.DoSelectedQuestInfo(rect3);
		}

		// Token: 0x060078C4 RID: 30916 RVA: 0x002A9178 File Offset: 0x002A7378
		public Rect DoRewardsPrefsButton(Rect rect)
		{
			rect.yMin = rect.yMax - 40f;
			Text.Font = GameFont.Small;
			if (Widgets.ButtonText(rect, "ChooseRewards".Translate(), true, true, true))
			{
				Find.WindowStack.Add(new Dialog_RewardPrefsConfig());
			}
			return rect;
		}

		// Token: 0x060078C5 RID: 30917 RVA: 0x002A91CC File Offset: 0x002A73CC
		public void Select(Quest quest)
		{
			if (quest != this.selected)
			{
				this.selected = quest;
				this.selectedQuestScrollPosition = default(Vector2);
				this.selectedQuestLastHeight = 300f;
			}
			if (quest != null)
			{
				if (quest.dismissed)
				{
					this.curTab = MainTabWindow_Quests.QuestsTab.Historical;
					return;
				}
				if (quest.State == QuestState.NotYetAccepted)
				{
					this.curTab = MainTabWindow_Quests.QuestsTab.Available;
					return;
				}
				if (quest.State == QuestState.Ongoing)
				{
					this.curTab = MainTabWindow_Quests.QuestsTab.Active;
					return;
				}
				this.curTab = MainTabWindow_Quests.QuestsTab.Historical;
			}
		}

		// Token: 0x060078C6 RID: 30918 RVA: 0x002A923C File Offset: 0x002A743C
		private void DoQuestsList(Rect rect)
		{
			Rect rect2 = rect;
			rect2.yMin += 32f;
			Widgets.DrawMenuSection(rect2);
			TabDrawer.DrawTabs<TabRecord>(rect2, this.tabs, 200f);
			if (Prefs.DevMode)
			{
				Widgets.CheckboxLabeled(new Rect(rect.width - 135f, rect.height - 24f + 5f, 120f, 24f), "Dev: Show all", ref this.showAll, false, null, null, false);
			}
			else
			{
				this.showAll = false;
			}
			this.SortQuestsByTab();
			if (MainTabWindow_Quests.tmpQuestsToShow.Count != 0)
			{
				MainTabWindow_Quests.<>c__DisplayClass47_0 CS$<>8__locals1;
				CS$<>8__locals1.scrollOutRect = rect2;
				CS$<>8__locals1.scrollOutRect = CS$<>8__locals1.scrollOutRect.ContractedBy(10f);
				CS$<>8__locals1.scrollOutRect.xMax = CS$<>8__locals1.scrollOutRect.xMax + 6f;
				CS$<>8__locals1.scrollViewRect = new Rect(0f, 0f, CS$<>8__locals1.scrollOutRect.width - 16f, (float)MainTabWindow_Quests.tmpQuestsToShow.Count * 32f);
				CS$<>8__locals1.scrollPosition = default(Vector2);
				switch (this.curTab)
				{
				case MainTabWindow_Quests.QuestsTab.Available:
					Widgets.BeginScrollView(CS$<>8__locals1.scrollOutRect, ref this.scrollPosition_available, CS$<>8__locals1.scrollViewRect, true);
					CS$<>8__locals1.scrollPosition = this.scrollPosition_available;
					break;
				case MainTabWindow_Quests.QuestsTab.Active:
					Widgets.BeginScrollView(CS$<>8__locals1.scrollOutRect, ref this.scrollPosition_active, CS$<>8__locals1.scrollViewRect, true);
					CS$<>8__locals1.scrollPosition = this.scrollPosition_active;
					break;
				case MainTabWindow_Quests.QuestsTab.Historical:
					Widgets.BeginScrollView(CS$<>8__locals1.scrollOutRect, ref this.scrollPosition_historical, CS$<>8__locals1.scrollViewRect, true);
					CS$<>8__locals1.scrollPosition = this.scrollPosition_historical;
					break;
				}
				CS$<>8__locals1.curY = 0f;
				foreach (Quest quest in MainTabWindow_Quests.tmpQuestsToShow)
				{
					this.<DoQuestsList>g__DrawQuest|47_0(quest, 0, ref CS$<>8__locals1);
				}
				MainTabWindow_Quests.tmpQuestsVisited.Clear();
				MainTabWindow_Quests.tmpQuestsToShow.Clear();
				Widgets.EndScrollView();
				return;
			}
			Widgets.NoneLabel(rect2.y + 17f, rect2.width, null);
		}

		// Token: 0x060078C7 RID: 30919 RVA: 0x002A9474 File Offset: 0x002A7674
		private void SortQuestsByTab()
		{
			List<Quest> questsInDisplayOrder = Find.QuestManager.questsInDisplayOrder;
			MainTabWindow_Quests.tmpQuestsToShow.Clear();
			for (int i = 0; i < questsInDisplayOrder.Count; i++)
			{
				if (this.ShouldListNow(questsInDisplayOrder[i]))
				{
					MainTabWindow_Quests.tmpQuestsToShow.Add(questsInDisplayOrder[i]);
				}
			}
			switch (this.curTab)
			{
			case MainTabWindow_Quests.QuestsTab.Available:
				MainTabWindow_Quests.tmpQuestsToShow.SortBy((Quest q) => q.ticksUntilAcceptanceExpiry);
				return;
			case MainTabWindow_Quests.QuestsTab.Active:
				MainTabWindow_Quests.tmpQuestsToShow.SortBy((Quest q) => q.TicksSinceAccepted);
				return;
			case MainTabWindow_Quests.QuestsTab.Historical:
				MainTabWindow_Quests.tmpQuestsToShow.SortBy((Quest q) => q.TicksSinceCleanup);
				return;
			default:
				return;
			}
		}

		// Token: 0x060078C8 RID: 30920 RVA: 0x002A9560 File Offset: 0x002A7760
		private void DoRow(Rect rect, Quest quest)
		{
			Rect rect2 = rect;
			rect2.width -= 95f;
			Rect rect3 = rect;
			rect3.xMax -= 4f;
			rect3.xMin = rect3.xMax - 35f;
			Rect rect4 = rect;
			rect4.xMax = rect3.xMin;
			rect4.xMin = rect4.xMax - 60f;
			if (quest.Historical)
			{
				Rect position = rect.ContractedBy(2f);
				QuestState state = quest.State;
				if (state != QuestState.EndedSuccess)
				{
					if (state != QuestState.EndedFailed)
					{
						Widgets.DrawRectFast(position, MainTabWindow_Quests.QuestExpiredColor, null);
					}
					else
					{
						Widgets.DrawRectFast(position, MainTabWindow_Quests.QuestFailedColor, null);
					}
				}
				else
				{
					Widgets.DrawRectFast(position, MainTabWindow_Quests.QuestCompletedColor, null);
				}
			}
			if (this.selected == quest)
			{
				Widgets.DrawHighlightSelected(rect);
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect5 = new Rect(rect2.x + 4f, rect2.y, rect2.width - 4f, rect2.height);
			Widgets.Label(rect5, quest.name.Truncate(rect5.width, null));
			string timeTip;
			Color color;
			string shortTimeInfo = this.GetShortTimeInfo(quest, out timeTip, out color);
			if (!shortTimeInfo.NullOrEmpty())
			{
				GUI.color = color;
				Text.Anchor = TextAnchor.MiddleRight;
				Widgets.Label(rect3, shortTimeInfo);
				GUI.color = Color.white;
				if (Mouse.IsOver(rect3))
				{
					TooltipHandler.TipRegion(rect3, () => quest.name + (timeTip.NullOrEmpty() ? "" : ("\n" + timeTip)), quest.id ^ 875632098);
					Widgets.DrawHighlight(rect3);
				}
			}
			if (quest.dismissed && !quest.Historical)
			{
				rect4.x -= 25f;
				Rect rect6 = new Rect(rect4.xMax + 5f, rect4.y + rect4.height / 2f - 7f, 15f, 15f);
				GUI.DrawTexture(rect6, MainTabWindow_Quests.QuestDismissedIcon);
				rect6.height = rect5.height;
				rect6.y = rect5.y;
				if (Mouse.IsOver(rect6))
				{
					TooltipHandler.TipRegion(rect6, "QuestDismissed".Translate());
					Widgets.DrawHighlight(rect6);
				}
			}
			if (quest.charity && !quest.Historical && !quest.dismissed)
			{
				Rect rect7 = new Rect(rect4.x - 15f, rect4.y + rect4.height / 2f - 7f, 15f, 15f);
				GUI.DrawTexture(rect7, MainTabWindow_Quests.CharityQuestIcon);
				rect7.height = rect5.height;
				rect7.y = rect5.y;
				if (Mouse.IsOver(rect7))
				{
					TooltipHandler.TipRegion(rect7, "CharityQuestTip".Translate());
					Widgets.DrawHighlight(rect7);
				}
			}
			for (int i = 0; i < quest.challengeRating; i++)
			{
				GUI.DrawTexture(new Rect(rect4.xMax - (float)(15 * (i + 1)), rect4.y + rect4.height / 2f - 7f, 15f, 15f), MainTabWindow_Quests.RatingIcon);
			}
			if (Mouse.IsOver(rect4))
			{
				TooltipHandler.TipRegion(rect4, "QuestChallengeRatingTip".Translate());
				Widgets.DrawHighlight(rect4);
			}
			GenUI.ResetLabelAlign();
			if (Widgets.ButtonInvisible(rect, true))
			{
				this.Select(quest);
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x060078C9 RID: 30921 RVA: 0x002A9928 File Offset: 0x002A7B28
		private string GetShortTimeInfo(Quest quest, out string tip, out Color color)
		{
			color = Color.gray;
			if (quest.State == QuestState.NotYetAccepted)
			{
				if (quest.ticksUntilAcceptanceExpiry >= 0)
				{
					color = ColorLibrary.RedReadable;
					tip = "QuestExpiresIn".Translate(quest.ticksUntilAcceptanceExpiry.ToStringTicksToPeriod(true, false, true, true));
					return quest.ticksUntilAcceptanceExpiry.ToStringTicksToPeriod(true, true, true, true);
				}
			}
			else
			{
				if (quest.Historical)
				{
					tip = "QuestFinishedAgo".Translate(quest.TicksSinceCleanup.ToStringTicksToPeriod(true, false, true, true));
					return quest.TicksSinceCleanup.ToStringTicksToPeriod(false, true, true, true);
				}
				if (quest.EverAccepted)
				{
					foreach (QuestPart questPart in quest.PartsListForReading)
					{
						QuestPart_Delay questPart_Delay = questPart as QuestPart_Delay;
						if (questPart_Delay != null && questPart_Delay.State == QuestPartState.Enabled && questPart_Delay.isBad && !questPart_Delay.expiryInfoPart.NullOrEmpty())
						{
							color = ColorLibrary.RedReadable;
							tip = "QuestExpiresIn".Translate(questPart_Delay.TicksLeft.ToStringTicksToPeriod(true, false, true, true));
							return questPart_Delay.TicksLeft.ToStringTicksToPeriod(false, true, false, true);
						}
					}
					tip = this.GetAcceptedAgoByString(quest);
					return quest.TicksSinceAccepted.ToStringTicksToPeriod(false, true, true, true);
				}
			}
			tip = null;
			return null;
		}

		// Token: 0x060078CA RID: 30922 RVA: 0x002A9AA4 File Offset: 0x002A7CA4
		private void DoSelectedQuestInfo(Rect rect)
		{
			Widgets.DrawMenuSection(rect);
			if (this.selected == null)
			{
				Widgets.NoneLabelCenteredVertically(rect, "(" + "NoQuestSelected".Translate() + ")");
				return;
			}
			Rect rect2 = rect.ContractedBy(17f);
			Rect outRect = rect2;
			Rect innerRect = new Rect(0f, 0f, outRect.width, this.selectedQuestLastHeight);
			Rect rect3 = new Rect(0f, 0f, outRect.width - 16f, this.selectedQuestLastHeight);
			Rect rect4 = rect3;
			bool flag = rect3.height > rect2.height;
			if (flag)
			{
				rect3.width -= 4f;
				rect4.width -= 16f;
			}
			Widgets.BeginScrollView(outRect, ref this.selectedQuestScrollPosition, rect3, true);
			float num = 0f;
			this.DoTitle(rect3, ref num);
			this.DoDismissButton(rect3, ref num);
			this.DoCharityIcon(rect3);
			if (this.selected != null)
			{
				float curYBeforeAcceptButton = num;
				this.DoAcceptButton(rect3, ref num);
				this.DoRightAlignedInfo(rect3, ref num, curYBeforeAcceptButton);
				this.DoOutcomeInfo(rect3, ref num);
				this.DoDescription(rect3, ref num);
				this.DoAcceptanceRequirementInfo(innerRect, flag, ref num);
				this.DoIdeoCharityInfo(innerRect, flag, ref num);
				this.DoRewards(rect3, ref num);
				this.DoLookTargets(rect3, ref num);
				this.DoSelectTargets(rect3, ref num);
				float num2 = num;
				this.DoDefHyperlinks(rect3, ref num);
				float num3 = num;
				num = num2;
				if (!this.selected.root.hideInvolvedFactionsInfo)
				{
					this.DoFactionInfo(rect4, ref num);
				}
				this.DoDebugInfoToggle(rect3, ref num);
				if (num3 > num)
				{
					num = num3;
				}
				this.DoDebugInfo(rect3, ref num);
				this.selectedQuestLastHeight = num;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x060078CB RID: 30923 RVA: 0x002A9C64 File Offset: 0x002A7E64
		private void DoTitle(Rect innerRect, ref float curY)
		{
			Text.Font = GameFont.Medium;
			Rect rect = new Rect(innerRect.x, curY, innerRect.width, 100f);
			Widgets.Label(rect, this.selected.name.Truncate(rect.width, null));
			Text.Font = GameFont.Small;
			curY += Text.LineHeight;
			curY += 17f;
		}

		// Token: 0x060078CC RID: 30924 RVA: 0x002A9CCC File Offset: 0x002A7ECC
		private void DoDismissButton(Rect innerRect, ref float curY)
		{
			Rect rect = new Rect(innerRect.xMax - 32f - 4f, innerRect.y, 32f, 32f);
			Texture2D tex = (!this.selected.Historical && this.selected.dismissed) ? MainTabWindow_Quests.ResumeQuestIcon : MainTabWindow_Quests.DismissIcon;
			if (Widgets.ButtonImage(rect, tex, true))
			{
				if (this.selected.Historical)
				{
					this.selected.hiddenInUI = true;
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.Select(null);
					return;
				}
				this.selected.dismissed = !this.selected.dismissed;
				foreach (Quest quest in this.selected.GetSubquests(null))
				{
					quest.dismissed = this.selected.dismissed;
				}
				if (this.selected.dismissed)
				{
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					this.SortQuestsByTab();
					this.selected = MainTabWindow_Quests.tmpQuestsToShow.FirstOrDefault((Quest x) => this.ShouldListNow(x));
					MainTabWindow_Quests.tmpQuestsToShow.Clear();
					return;
				}
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				this.Select(this.selected);
			}
			if (Mouse.IsOver(rect))
			{
				string key = this.selected.Historical ? "DeleteQuest" : (this.selected.dismissed ? "UnDismissQuest" : "DismissQuest");
				TooltipHandler.TipRegion(rect, key.Translate());
			}
		}

		// Token: 0x060078CD RID: 30925 RVA: 0x002A9E78 File Offset: 0x002A8078
		private void DoCharityIcon(Rect innerRect)
		{
			if (this.selected != null && this.selected.charity && ModsConfig.IdeologyActive)
			{
				Rect rect = new Rect(innerRect.xMax - 32f - 26f - 32f - 4f, innerRect.y, 32f, 32f);
				GUI.DrawTexture(rect, MainTabWindow_Quests.CharityQuestIcon);
				if (Mouse.IsOver(rect))
				{
					TooltipHandler.TipRegion(rect, "CharityQuestTip".Translate());
				}
			}
		}

		// Token: 0x060078CE RID: 30926 RVA: 0x002A9F00 File Offset: 0x002A8100
		private void DoDebugInfoToggle(Rect innerRect, ref float curY)
		{
			if (!Prefs.DevMode)
			{
				this.showDebugInfo = false;
				return;
			}
			Widgets.CheckboxLabeled(new Rect(innerRect.xMax - 110f, curY, 110f, 30f), "Dev: Debug", ref this.showDebugInfo, false, null, null, false);
			curY += 30f;
		}

		// Token: 0x060078CF RID: 30927 RVA: 0x002A9F58 File Offset: 0x002A8158
		private void DoAcceptButton(Rect innerRect, ref float curY)
		{
			QuestPart_Choice questPart_Choice = null;
			List<QuestPart> partsListForReading = this.selected.PartsListForReading;
			for (int i = 0; i < partsListForReading.Count; i++)
			{
				questPart_Choice = (partsListForReading[i] as QuestPart_Choice);
				if (questPart_Choice != null)
				{
					break;
				}
			}
			if (questPart_Choice != null && !Prefs.DevMode)
			{
				return;
			}
			curY += 17f;
			if (this.selected.State == QuestState.NotYetAccepted)
			{
				float num = innerRect.x;
				if (questPart_Choice == null)
				{
					Rect rect = new Rect(num, curY, 180f, 40f);
					if (!QuestUtility.CanAcceptQuest(this.selected))
					{
						GUI.color = Color.grey;
					}
					if (Widgets.ButtonText(rect, "AcceptQuest".Translate(), true, true, true))
					{
						this.AcceptQuestByInterface(null, this.selected.RequiresAccepter);
					}
					num += rect.width + 10f;
					GUI.color = Color.white;
				}
				if (Prefs.DevMode && Widgets.ButtonText(new Rect(num, curY, 180f, 40f), "Dev: Accept instantly", true, true, true))
				{
					SoundDefOf.Quest_Accepted.PlayOneShotOnCamera(null);
					if (questPart_Choice.choices.Any<QuestPart_Choice.Choice>())
					{
						questPart_Choice.Choose(questPart_Choice.choices.RandomElement<QuestPart_Choice.Choice>());
					}
					this.selected.Accept((from p in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep
					where QuestUtility.CanPawnAcceptQuest(p, this.selected)
					select p).RandomElementWithFallback(null));
					this.selected.dismissed = false;
					this.Select(this.selected);
				}
				curY += 44f;
			}
		}

		// Token: 0x060078D0 RID: 30928 RVA: 0x002AA0D4 File Offset: 0x002A82D4
		private void DoRightAlignedInfo(Rect innerRect, ref float curY, float curYBeforeAcceptButton)
		{
			bool flag = false;
			Vector2 locForDates = QuestUtility.GetLocForDates();
			float num = curYBeforeAcceptButton;
			if (!this.selected.initiallyAccepted && this.selected.EverAccepted)
			{
				if (!flag)
				{
					num += 17f;
					flag = true;
				}
				Rect rect = new Rect(innerRect.x, num, innerRect.width, 25f);
				GUI.color = MainTabWindow_Quests.TimeLimitColor;
				Text.Anchor = TextAnchor.MiddleRight;
				string text = this.selected.Historical ? this.GetAcceptedOnByString(this.selected) : this.GetAcceptedAgoByString(this.selected);
				Widgets.Label(rect, text);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
				rect.xMin = rect.xMax - Text.CalcSize(text).x;
				if (Mouse.IsOver(rect))
				{
					TooltipHandler.TipRegion(rect, this.selected.Historical ? this.GetAcceptedAgoByString(this.selected) : this.GetAcceptedOnByString(this.selected));
				}
				if (this.selected.AccepterPawn != null && CameraJumper.CanJump(this.selected.AccepterPawn))
				{
					Widgets.DrawHighlightIfMouseover(rect);
					if (Widgets.ButtonInvisible(rect, true))
					{
						CameraJumper.TryJumpAndSelect(this.selected.AccepterPawn);
						Find.MainTabsRoot.EscapeCurrentTab(true);
					}
				}
				num += Text.LineHeight;
			}
			else if (this.selected.Historical)
			{
				if (!flag)
				{
					num += 17f;
					flag = true;
				}
				Rect rect2 = new Rect(innerRect.x, num, innerRect.width, 25f);
				GUI.color = MainTabWindow_Quests.TimeLimitColor;
				Text.Anchor = TextAnchor.MiddleRight;
				TaggedString taggedString = "AppearedOn".Translate(GenDate.DateFullStringWithHourAt((long)GenDate.TickGameToAbs(this.selected.appearanceTick), locForDates));
				Widgets.Label(rect2, taggedString);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
				rect2.xMin = rect2.xMax - Text.CalcSize(taggedString).x;
				if (Mouse.IsOver(rect2))
				{
					TooltipHandler.TipRegion(rect2, "AppearedDaysAgo".Translate(((float)this.selected.TicksSinceAppeared / 60000f).ToString("0.#")));
				}
				num += Text.LineHeight;
			}
			if (this.selected.State == QuestState.NotYetAccepted && this.selected.ticksUntilAcceptanceExpiry > 0)
			{
				if (!flag)
				{
					num += 17f;
					flag = true;
				}
				Rect rect3 = new Rect(innerRect.x, num, innerRect.width, 25f);
				GUI.color = MainTabWindow_Quests.TimeLimitColor;
				Text.Anchor = TextAnchor.MiddleRight;
				string text2 = "QuestExpiresIn".Translate(this.selected.ticksUntilAcceptanceExpiry.ToStringTicksToPeriod(true, false, true, true));
				Widgets.Label(rect3, text2);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
				rect3.xMin = rect3.xMax - Text.CalcSize(text2).x;
				if (Mouse.IsOver(rect3))
				{
					TooltipHandler.TipRegion(rect3, "QuestExpiresOn".Translate(GenDate.DateFullStringWithHourAt((long)(Find.TickManager.TicksAbs + this.selected.ticksUntilAcceptanceExpiry), locForDates)));
				}
				num += Text.LineHeight;
			}
			if (this.selected.State == QuestState.Ongoing)
			{
				this.tmpQuestParts.Clear();
				this.tmpQuestParts.AddRange(this.selected.PartsListForReading);
				this.tmpQuestParts.SortBy(delegate(QuestPart x)
				{
					if (!(x is QuestPartActivable))
					{
						return 0;
					}
					return ((QuestPartActivable)x).EnableTick;
				});
				for (int i = 0; i < this.tmpQuestParts.Count; i++)
				{
					QuestPartActivable questPartActivable = this.tmpQuestParts[i] as QuestPartActivable;
					if (questPartActivable != null && questPartActivable.State == QuestPartState.Enabled)
					{
						string expiryInfoPart = questPartActivable.ExpiryInfoPart;
						if (!expiryInfoPart.NullOrEmpty())
						{
							if (!flag)
							{
								num += 17f;
								flag = true;
							}
							Rect rect4 = new Rect(innerRect.x, num, innerRect.width, 25f);
							GUI.color = MainTabWindow_Quests.TimeLimitColor;
							Text.Anchor = TextAnchor.MiddleRight;
							Widgets.Label(rect4, expiryInfoPart);
							GUI.color = Color.white;
							Text.Anchor = TextAnchor.UpperLeft;
							rect4.xMin = rect4.xMax - Text.CalcSize(expiryInfoPart).x;
							if (Mouse.IsOver(rect4))
							{
								string expiryInfoPartTip = questPartActivable.ExpiryInfoPartTip;
								if (!expiryInfoPartTip.NullOrEmpty())
								{
									TooltipHandler.TipRegion(rect4, expiryInfoPartTip);
								}
							}
							num += Text.LineHeight;
						}
					}
				}
				this.tmpQuestParts.Clear();
			}
			curY = Mathf.Max(curY, num);
		}

		// Token: 0x060078D1 RID: 30929 RVA: 0x002AA59C File Offset: 0x002A879C
		private void DoAcceptanceRequirementInfo(Rect innerRect, bool scrollBarVisible, ref float curY)
		{
			if (this.selected.EverAccepted)
			{
				return;
			}
			IEnumerable<string> enumerable = this.ListUnmetAcceptRequirements();
			int num = enumerable.Count<string>();
			if (num == 0)
			{
				return;
			}
			bool flag = num > 1;
			string text = "QuestAcceptanceRequirementsDescription".Translate() + (flag ? ": " : " ") + (flag ? ("\n" + enumerable.ToLineList("  - ", true)) : (enumerable.First<string>() + "."));
			curY += 17f;
			this.DrawInfoBox(innerRect, scrollBarVisible, ref curY, text, MainTabWindow_Quests.acceptanceRequirementsBoxBgColor, MainTabWindow_Quests.AcceptanceRequirementsBoxColor, MainTabWindow_Quests.AcceptanceRequirementsColor);
			new LookTargets(this.ListUnmetAcceptRequirementCulprits()).TryHighlight(true, true, true);
		}

		// Token: 0x060078D2 RID: 30930 RVA: 0x002AA658 File Offset: 0x002A8858
		private void DoIdeoCharityInfo(Rect innerRect, bool scrollBarVisible, ref float curY)
		{
			if (!this.selected.charity || !ModsConfig.IdeologyActive)
			{
				return;
			}
			List<Pawn> allMaps_FreeColonistsSpawned = PawnsFinder.AllMaps_FreeColonistsSpawned;
			List<Ideo> ideosListForReading = Find.IdeoManager.IdeosListForReading;
			string text = "";
			for (int i = 0; i < ideosListForReading.Count; i++)
			{
				Ideo ideo = ideosListForReading[i];
				List<Precept> preceptsListForReading = ideo.PreceptsListForReading;
				bool flag = false;
				for (int j = 0; j < preceptsListForReading.Count; j++)
				{
					if (preceptsListForReading[j].def.issue == IssueDefOf.Charity)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					MainTabWindow_Quests.tmpColonistsForIdeo.Clear();
					for (int k = 0; k < allMaps_FreeColonistsSpawned.Count; k++)
					{
						Pawn pawn = allMaps_FreeColonistsSpawned[k];
						if (pawn != null && pawn.Ideo == ideo && !pawn.IsQuestReward(this.selected))
						{
							MainTabWindow_Quests.tmpColonistsForIdeo.Add(pawn);
						}
					}
					if (MainTabWindow_Quests.tmpColonistsForIdeo.Count != 0)
					{
						if (!text.NullOrEmpty())
						{
							text += "\n\n";
						}
						text += "IdeoCharityQuestInfo".Translate(ideo.name, GenThing.ThingsToCommaList(MainTabWindow_Quests.tmpColonistsForIdeo, false, true, -1));
					}
				}
			}
			if (text.NullOrEmpty())
			{
				return;
			}
			curY += 17f;
			this.DrawInfoBox(innerRect, scrollBarVisible, ref curY, text, MainTabWindow_Quests.IdeoCharityBoxBackgroundColor, MainTabWindow_Quests.IdeoCharityBoxBorderColor, MainTabWindow_Quests.IdeoCharityTextColor);
		}

		// Token: 0x060078D3 RID: 30931 RVA: 0x002AA7CC File Offset: 0x002A89CC
		private void DrawInfoBox(Rect innerRect, bool scrollBarVisible, ref float curY, string text, Color boxBackground, Color boxBorder, Color textColor)
		{
			float num = 0f;
			float x = innerRect.x + 8f;
			float num2 = innerRect.width - 16f;
			if (scrollBarVisible)
			{
				num2 -= 31f;
			}
			Rect rect = new Rect(x, curY, num2, 10000f);
			num += Text.CalcHeight(text, rect.width);
			Rect rect2 = new Rect(x, curY, num2, num).ExpandedBy(8f);
			Widgets.DrawBoxSolid(rect2, boxBackground);
			GUI.color = textColor;
			Widgets.Label(rect, text);
			GUI.color = boxBorder;
			Widgets.DrawBox(rect2, 2, null);
			curY += num;
			GUI.color = Color.white;
		}

		// Token: 0x060078D4 RID: 30932 RVA: 0x002AA870 File Offset: 0x002A8A70
		private IEnumerable<string> ListUnmetAcceptRequirements()
		{
			int num;
			for (int i = 0; i < this.selected.PartsListForReading.Count; i = num + 1)
			{
				QuestPart_RequirementsToAccept questPart_RequirementsToAccept = this.selected.PartsListForReading[i] as QuestPart_RequirementsToAccept;
				if (questPart_RequirementsToAccept != null)
				{
					AcceptanceReport acceptanceReport = questPart_RequirementsToAccept.CanAccept();
					if (!acceptanceReport.Accepted)
					{
						yield return acceptanceReport.Reason;
					}
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x060078D5 RID: 30933 RVA: 0x002AA880 File Offset: 0x002A8A80
		private IEnumerable<GlobalTargetInfo> ListUnmetAcceptRequirementCulprits()
		{
			int num;
			for (int i = 0; i < this.selected.PartsListForReading.Count; i = num + 1)
			{
				QuestPart_RequirementsToAccept questPart_RequirementsToAccept = this.selected.PartsListForReading[i] as QuestPart_RequirementsToAccept;
				if (questPart_RequirementsToAccept != null)
				{
					foreach (GlobalTargetInfo globalTargetInfo in questPart_RequirementsToAccept.Culprits)
					{
						yield return globalTargetInfo;
					}
					IEnumerator<GlobalTargetInfo> enumerator = null;
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x060078D6 RID: 30934 RVA: 0x002AA890 File Offset: 0x002A8A90
		private void DoOutcomeInfo(Rect innerRect, ref float curY)
		{
			if (!this.selected.Historical)
			{
				return;
			}
			string text;
			if (this.selected.State == QuestState.EndedOfferExpired)
			{
				text = "QuestOutcomeInfo_OfferExpired".Translate();
			}
			else if (this.selected.State == QuestState.EndedUnknownOutcome || this.selected.State == QuestState.EndedSuccess)
			{
				text = "QuestOutcomeInfo_UnknownOrSuccess".Translate();
			}
			else if (this.selected.State == QuestState.EndedFailed)
			{
				text = "QuestOutcomeInfo_Failed".Translate();
			}
			else if (this.selected.State == QuestState.EndedInvalid)
			{
				text = "QuestOutcomeInfo_Invalid".Translate();
			}
			else
			{
				text = null;
			}
			if (!text.NullOrEmpty())
			{
				curY += 17f;
				Widgets.Label(new Rect(innerRect.x, curY, innerRect.width, 25f), text);
				curY += Text.LineHeight;
			}
		}

		// Token: 0x060078D7 RID: 30935 RVA: 0x002AA978 File Offset: 0x002A8B78
		private void DoDescription(Rect innerRect, ref float curY)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!this.selected.description.RawText.NullOrEmpty())
			{
				string value = this.selected.description.Resolve();
				stringBuilder.Append(value);
			}
			this.tmpQuestParts.Clear();
			this.tmpQuestParts.AddRange(this.selected.PartsListForReading);
			this.tmpQuestParts.SortBy(delegate(QuestPart x)
			{
				if (!(x is QuestPartActivable))
				{
					return 0;
				}
				return ((QuestPartActivable)x).EnableTick;
			});
			for (int i = 0; i < this.tmpQuestParts.Count; i++)
			{
				QuestPartActivable questPartActivable = this.tmpQuestParts[i] as QuestPartActivable;
				if (questPartActivable == null || questPartActivable.State == QuestPartState.Enabled)
				{
					string descriptionPart = this.tmpQuestParts[i].DescriptionPart;
					if (!descriptionPart.NullOrEmpty())
					{
						if (stringBuilder.Length != 0)
						{
							stringBuilder.AppendLine();
							stringBuilder.AppendLine();
						}
						stringBuilder.Append(descriptionPart);
					}
				}
			}
			this.tmpQuestParts.Clear();
			if (stringBuilder.Length == 0)
			{
				return;
			}
			curY += 17f;
			Rect rect = new Rect(innerRect.x, curY, innerRect.width, 10000f);
			Widgets.Label(rect, stringBuilder.ToString());
			curY += Text.CalcHeight(stringBuilder.ToString(), rect.width);
		}

		// Token: 0x060078D8 RID: 30936 RVA: 0x002AAAD8 File Offset: 0x002A8CD8
		private void DoRewards(Rect innerRect, ref float curY)
		{
			QuestPart_Choice choice = null;
			List<QuestPart> partsListForReading = this.selected.PartsListForReading;
			for (int i = 0; i < partsListForReading.Count; i++)
			{
				choice = (partsListForReading[i] as QuestPart_Choice);
				if (choice != null)
				{
					break;
				}
			}
			if (choice == null)
			{
				return;
			}
			bool flag = this.selected.State == QuestState.NotYetAccepted;
			bool flag2 = true;
			if (Event.current.type == EventType.Layout)
			{
				MainTabWindow_Quests.layoutRewardsRects.Clear();
			}
			for (int j = 0; j < choice.choices.Count; j++)
			{
				MainTabWindow_Quests.tmpStackElements.Clear();
				float num = 0f;
				for (int k = 0; k < choice.choices[j].rewards.Count; k++)
				{
					MainTabWindow_Quests.tmpStackElements.AddRange(choice.choices[j].rewards[k].StackElements);
					num += choice.choices[j].rewards[k].TotalMarketValue;
				}
				if (MainTabWindow_Quests.tmpStackElements.Any<GenUI.AnonymousStackElement>())
				{
					Reward_Items reward_Items;
					if (num > 0f && (choice.choices[j].rewards.Count != 1 || (reward_Items = (choice.choices[j].rewards[0] as Reward_Items)) == null || reward_Items.items == null || reward_Items.items.Count != 1 || !(reward_Items.items[0].StyleSourcePrecept is Precept_Relic)))
					{
						TaggedString totalValueStr = "TotalValue".Translate(num.ToStringMoney("F0"));
						MainTabWindow_Quests.tmpStackElements.Add(new GenUI.AnonymousStackElement
						{
							drawer = delegate(Rect r)
							{
								GUI.color = new Color(0.7f, 0.7f, 0.7f);
								Widgets.Label(new Rect(r.x + 5f, r.y, r.width - 10f, r.height), totalValueStr);
								GUI.color = Color.white;
							},
							width = Text.CalcSize(totalValueStr).x + 10f
						});
					}
					if (flag2)
					{
						curY += 17f;
						flag2 = false;
					}
					else
					{
						curY += 10f;
					}
					Rect rect = new Rect(innerRect.x, curY, innerRect.width, 10000f);
					Rect rect2 = rect.ContractedBy(10f);
					if (flag)
					{
						rect2.xMin += 100f;
					}
					if (j < MainTabWindow_Quests.layoutRewardsRects.Count)
					{
						Widgets.DrawBoxSolid(MainTabWindow_Quests.layoutRewardsRects[j], new Color(0.13f, 0.13f, 0.13f));
						GUI.color = new Color(1f, 1f, 1f, 0.3f);
						Widgets.DrawHighlightIfMouseover(MainTabWindow_Quests.layoutRewardsRects[j]);
						GUI.color = Color.white;
					}
					rect.height = GenUI.DrawElementStack<GenUI.AnonymousStackElement>(rect2, 24f, MainTabWindow_Quests.tmpStackElements, delegate(Rect r, GenUI.AnonymousStackElement obj)
					{
						obj.drawer(r);
					}, (GenUI.AnonymousStackElement obj) => obj.width, 4f, 5f, false).height + 20f;
					if (Event.current.type == EventType.Layout)
					{
						MainTabWindow_Quests.layoutRewardsRects.Add(rect);
					}
					if (flag)
					{
						if (!QuestUtility.CanAcceptQuest(this.selected))
						{
							GUI.color = Color.grey;
						}
						Rect rect3 = new Rect(rect.x, rect.y, 100f, rect.height);
						if (Widgets.ButtonText(rect3, "AcceptQuestFor".Translate() + ":", true, true, true))
						{
							MainTabWindow_Quests.tmpRemainingQuestParts.Clear();
							MainTabWindow_Quests.tmpRemainingQuestParts.AddRange(this.selected.PartsListForReading);
							for (int l = 0; l < choice.choices.Count; l++)
							{
								if (j != l)
								{
									for (int m = 0; m < choice.choices[l].questParts.Count; m++)
									{
										QuestPart item = choice.choices[l].questParts[m];
										if (!choice.choices[j].questParts.Contains(item))
										{
											MainTabWindow_Quests.tmpRemainingQuestParts.Remove(item);
										}
									}
								}
							}
							bool requiresAccepter = false;
							for (int n = 0; n < MainTabWindow_Quests.tmpRemainingQuestParts.Count; n++)
							{
								if (MainTabWindow_Quests.tmpRemainingQuestParts[n].RequiresAccepter)
								{
									requiresAccepter = true;
									break;
								}
							}
							MainTabWindow_Quests.tmpRemainingQuestParts.Clear();
							QuestPart_Choice.Choice localChoice = choice.choices[j];
							this.AcceptQuestByInterface(delegate
							{
								choice.Choose(localChoice);
							}, requiresAccepter);
						}
						TooltipHandler.TipRegionByKey(rect3, "AcceptQuestForTip");
						GUI.color = Color.white;
					}
					curY += rect.height;
				}
			}
			if (Event.current.type == EventType.Repaint)
			{
				MainTabWindow_Quests.layoutRewardsRects.Clear();
			}
			MainTabWindow_Quests.tmpStackElements.Clear();
		}

		// Token: 0x060078D9 RID: 30937 RVA: 0x002AB078 File Offset: 0x002A9278
		private void DoLookTargets(Rect innerRect, ref float curY)
		{
			List<Map> maps = Find.Maps;
			int num = 0;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					num++;
				}
			}
			MainTabWindow_Quests.tmpLookTargets.Clear();
			MainTabWindow_Quests.tmpLookTargets.AddRange(this.selected.QuestLookTargets);
			MainTabWindow_Quests.tmpLookTargets.SortBy(delegate(GlobalTargetInfo x)
			{
				if (x.Thing is Pawn)
				{
					return 0;
				}
				if (x.HasThing)
				{
					return 1;
				}
				if (!x.IsWorldTarget)
				{
					return 2;
				}
				if (!(x.WorldObject is Settlement) || ((Settlement)x.WorldObject).Faction != Faction.OfPlayer)
				{
					return 3;
				}
				return 4;
			}, (GlobalTargetInfo x) => x.Label);
			bool flag = false;
			for (int j = 0; j < MainTabWindow_Quests.tmpLookTargets.Count; j++)
			{
				GlobalTargetInfo globalTargetInfo = MainTabWindow_Quests.tmpLookTargets[j];
				if (globalTargetInfo.HasWorldObject)
				{
					MapParent mapParent = globalTargetInfo.WorldObject as MapParent;
					if (mapParent != null && (!mapParent.HasMap || !mapParent.Map.IsPlayerHome))
					{
						flag = true;
						break;
					}
				}
			}
			bool flag2 = false;
			for (int k = 0; k < MainTabWindow_Quests.tmpLookTargets.Count; k++)
			{
				GlobalTargetInfo globalTargetInfo2 = MainTabWindow_Quests.tmpLookTargets[k];
				if (CameraJumper.CanJump(globalTargetInfo2) && (num != 1 || !(globalTargetInfo2 == Find.AnyPlayerHomeMap.Parent) || flag))
				{
					if (!flag2)
					{
						flag2 = true;
						curY += 17f;
					}
					if (Widgets.ButtonText(new Rect(innerRect.x, curY, innerRect.width, 25f), "JumpToTargetCustom".Translate(globalTargetInfo2.Label), false, true, true))
					{
						CameraJumper.TryJumpAndSelect(globalTargetInfo2);
						Find.MainTabsRoot.EscapeCurrentTab(true);
					}
					curY += 25f;
				}
			}
		}

		// Token: 0x060078DA RID: 30938 RVA: 0x002AB240 File Offset: 0x002A9440
		private void DoSelectTargets(Rect innerRect, ref float curY)
		{
			bool flag = false;
			for (int i = 0; i < this.selected.PartsListForReading.Count; i++)
			{
				QuestPart questPart = this.selected.PartsListForReading[i];
				MainTabWindow_Quests.tmpSelectTargets.Clear();
				MainTabWindow_Quests.tmpSelectTargets.AddRange(questPart.QuestSelectTargets);
				if (MainTabWindow_Quests.tmpSelectTargets.Count != 0)
				{
					if (!flag)
					{
						flag = true;
						curY += 4f;
					}
					if (Widgets.ButtonText(new Rect(innerRect.x, curY, innerRect.width, 25f), questPart.QuestSelectTargetsLabel, false, true, true))
					{
						Map map = null;
						int num = 0;
						Vector3 a = Vector3.zero;
						Find.Selector.ClearSelection();
						for (int j = 0; j < MainTabWindow_Quests.tmpSelectTargets.Count; j++)
						{
							GlobalTargetInfo target = MainTabWindow_Quests.tmpSelectTargets[j];
							if (CameraJumper.CanJump(target) && target.HasThing)
							{
								Find.Selector.Select(target.Thing, true, true);
								if (map == null)
								{
									map = target.Map;
								}
								else if (target.Map != map)
								{
									num = 0;
									break;
								}
								a += target.Cell.ToVector3();
								num++;
							}
						}
						if (num > 0)
						{
							CameraJumper.TryJump(new IntVec3(a / (float)num), map);
						}
						Find.MainTabsRoot.EscapeCurrentTab(true);
					}
					curY += 25f;
				}
			}
		}

		// Token: 0x060078DB RID: 30939 RVA: 0x002AB3B8 File Offset: 0x002A95B8
		private void DoFactionInfo(Rect rect, ref float curY)
		{
			curY += 15f;
			foreach (Faction faction in this.selected.InvolvedFactions)
			{
				if (faction != null && !faction.Hidden && !faction.IsPlayer)
				{
					FactionUIUtility.DrawRelatedFactionInfo(rect, faction, ref curY);
				}
			}
		}

		// Token: 0x060078DC RID: 30940 RVA: 0x002AB428 File Offset: 0x002A9628
		private void DoDefHyperlinks(Rect rect, ref float curY)
		{
			curY += 25f;
			foreach (Dialog_InfoCard.Hyperlink hyperlink in this.selected.Hyperlinks)
			{
				float num = Text.CalcHeight(hyperlink.Label, rect.width);
				float width = rect.width / 2f;
				Rect rect2 = new Rect(rect.x, curY, width, num);
				Color value = Widgets.NormalOptionColor;
				if (hyperlink.quest != null && (hyperlink.quest.IsSubquestOf(this.selected) || this.selected.IsSubquestOf(hyperlink.quest)))
				{
					if (!this.selected.hidden && !hyperlink.quest.hidden)
					{
						string text = "";
						if (hyperlink.quest.Historical)
						{
							text += "(" + "Finished".Translate().ToLower() + ") ";
							value = Color.gray;
						}
						text += (hyperlink.quest.IsSubquestOf(this.selected) ? "HasSubquest".Translate() : "SubquestOf".Translate());
						text = text + ": " + hyperlink.Label;
						Widgets.HyperlinkWithIcon(rect2, hyperlink, text, 2f, 6f, new Color?(value), true, null);
					}
				}
				else
				{
					Widgets.HyperlinkWithIcon(rect2, hyperlink, "ViewHyperlink".Translate(hyperlink.Label), 2f, 6f, new Color?(value), false, null);
				}
				curY += num;
			}
		}

		// Token: 0x060078DD RID: 30941 RVA: 0x002AB618 File Offset: 0x002A9818
		private void DoDebugInfo(Rect innerRect, ref float curY)
		{
			if (!this.showDebugInfo)
			{
				return;
			}
			curY += 17f;
			List<QuestPart> partsListForReading = this.selected.PartsListForReading;
			if (this.selected.State == QuestState.Ongoing)
			{
				for (int i = 0; i < partsListForReading.Count; i++)
				{
					partsListForReading[i].DoDebugWindowContents(innerRect, ref curY);
				}
			}
			if (this.selected.State == QuestState.Ongoing || this.selected.State == QuestState.NotYetAccepted)
			{
				Rect rect = new Rect(innerRect.x, curY, 210f, 25f);
				this.debugSendSignalTextField = Widgets.TextField(rect, this.debugSendSignalTextField);
				Rect rect2 = new Rect(innerRect.x + rect.width + 4f, curY, 117f, 25f);
				if (Widgets.ButtonText(rect2, "Send signal", true, true, true))
				{
					Find.SignalManager.SendSignal(new Signal(this.debugSendSignalTextField));
					this.debugSendSignalTextField = "";
				}
				if (Widgets.ButtonText(new Rect(rect2.xMax + 4f, curY, 165f, 25f), "Send defined signal...", true, true, true))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (string signalLocal2 in from x in this.DebugPossibleSignals(this.selected).Distinct<string>()
					orderby x
					select x)
					{
						string signalLocal = signalLocal2;
						list.Add(new FloatMenuOption(signalLocal, delegate()
						{
							Find.SignalManager.SendSignal(new Signal(signalLocal));
							this.debugSendSignalTextField = "";
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
				curY += rect.height + 4f;
			}
			string text = "-----------------";
			text = text + "\nId: " + this.selected.id;
			text = text + "\nState: " + this.selected.State;
			text += "\nData:";
			text = text + "\n" + Scribe.saver.DebugOutputFor(this.selected);
			text += "\n";
			text += "\nActive QuestParts:";
			bool flag = false;
			for (int j = 0; j < partsListForReading.Count; j++)
			{
				QuestPartActivable questPartActivable = partsListForReading[j] as QuestPartActivable;
				if (questPartActivable != null && questPartActivable.State == QuestPartState.Enabled)
				{
					text = text + "\n" + questPartActivable.ToString();
					flag = true;
				}
			}
			if (!flag)
			{
				text += "\nNone";
			}
			Rect rect3 = new Rect(innerRect.x, curY, 180f, 40f);
			if (Widgets.ButtonText(rect3, "Copy debug to clipboard", true, true, true))
			{
				GUIUtility.systemCopyBuffer = text;
			}
			curY += rect3.height + 4f;
			Widgets.LongLabel(innerRect.x, innerRect.width, text, ref curY, true);
		}

		// Token: 0x060078DE RID: 30942 RVA: 0x002AB954 File Offset: 0x002A9B54
		private bool ShouldListNow(Quest quest)
		{
			if (quest.hidden && !this.showAll)
			{
				return false;
			}
			switch (this.curTab)
			{
			case MainTabWindow_Quests.QuestsTab.Available:
				return quest.State == QuestState.NotYetAccepted && !quest.dismissed && !quest.hiddenInUI;
			case MainTabWindow_Quests.QuestsTab.Active:
				return quest.State == QuestState.Ongoing && !quest.dismissed && !quest.hiddenInUI;
			case MainTabWindow_Quests.QuestsTab.Historical:
				return !quest.hiddenInUI && (quest.Historical || quest.dismissed);
			default:
				return false;
			}
		}

		// Token: 0x060078DF RID: 30943 RVA: 0x002AB9E3 File Offset: 0x002A9BE3
		private IEnumerable<string> DebugPossibleSignals(Quest quest)
		{
			string input = Scribe.saver.DebugOutputFor(this.selected);
			foreach (object obj in Regex.Matches(input, ">(Quest" + quest.id + "\\.[a-zA-Z0-9/\\-\\.]*)<"))
			{
				string value = ((Match)obj).Groups[1].Value;
				yield return value;
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060078E0 RID: 30944 RVA: 0x002AB9FC File Offset: 0x002A9BFC
		private string GetAcceptedAgoByString(Quest quest)
		{
			string value = quest.TicksSinceAccepted.ToStringTicksToPeriod(true, false, true, true);
			if (!quest.AccepterPawnLabelCap.NullOrEmpty())
			{
				return "AcceptedAgoBy".Translate(value, quest.AccepterPawnLabelCap);
			}
			return "AcceptedAgo".Translate(value);
		}

		// Token: 0x060078E1 RID: 30945 RVA: 0x002ABA5C File Offset: 0x002A9C5C
		private string GetAcceptedOnByString(Quest quest)
		{
			Vector2 locForDates = QuestUtility.GetLocForDates();
			string value = GenDate.DateFullStringWithHourAt((long)GenDate.TickGameToAbs(quest.acceptanceTick), locForDates);
			if (!quest.AccepterPawnLabelCap.NullOrEmpty())
			{
				return "AcceptedOnBy".Translate(value, quest.AccepterPawnLabelCap);
			}
			return "AcceptedOn".Translate(value);
		}

		// Token: 0x060078E2 RID: 30946 RVA: 0x002ABAC8 File Offset: 0x002A9CC8
		private void AcceptQuestByInterface(Action preAcceptAction = null, bool requiresAccepter = false)
		{
			MainTabWindow_Quests.<>c__DisplayClass81_0 CS$<>8__locals1 = new MainTabWindow_Quests.<>c__DisplayClass81_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.preAcceptAction = preAcceptAction;
			if (!QuestUtility.CanAcceptQuest(this.selected))
			{
				Messages.Message("MessageCannotAcceptQuest".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			if (!requiresAccepter)
			{
				SoundDefOf.Quest_Accepted.PlayOneShotOnCamera(null);
				if (CS$<>8__locals1.preAcceptAction != null)
				{
					CS$<>8__locals1.preAcceptAction();
				}
				this.selected.Accept(null);
				this.Select(this.selected);
				return;
			}
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			using (List<Pawn>.Enumerator enumerator = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MainTabWindow_Quests.<>c__DisplayClass81_1 CS$<>8__locals2 = new MainTabWindow_Quests.<>c__DisplayClass81_1();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					CS$<>8__locals2.p = enumerator.Current;
					if (QuestUtility.CanPawnAcceptQuest(CS$<>8__locals2.p, this.selected))
					{
						Pawn pLocal = CS$<>8__locals2.p;
						string text = "AcceptWith".Translate(CS$<>8__locals2.p);
						if (CS$<>8__locals2.p.royalty != null && CS$<>8__locals2.p.royalty.AllTitlesInEffectForReading.Any<RoyalTitle>())
						{
							text = text + " (" + CS$<>8__locals2.p.royalty.MostSeniorTitle.def.GetLabelFor(pLocal) + ")";
						}
						list.Add(new FloatMenuOption(text, delegate()
						{
							if (!QuestUtility.CanPawnAcceptQuest(pLocal, CS$<>8__locals2.CS$<>8__locals1.<>4__this.selected))
							{
								return;
							}
							QuestPart_GiveRoyalFavor questPart_GiveRoyalFavor = CS$<>8__locals2.CS$<>8__locals1.<>4__this.selected.PartsListForReading.OfType<QuestPart_GiveRoyalFavor>().FirstOrDefault<QuestPart_GiveRoyalFavor>();
							if (questPart_GiveRoyalFavor == null || !questPart_GiveRoyalFavor.giveToAccepter)
							{
								base.<AcceptQuestByInterface>g__AcceptAction|1();
								return;
							}
							IEnumerable<Trait> conceitedTraits = RoyalTitleUtility.GetConceitedTraits(CS$<>8__locals2.p);
							IEnumerable<Trait> traitsAffectingPsylinkNegatively = RoyalTitleUtility.GetTraitsAffectingPsylinkNegatively(CS$<>8__locals2.p);
							bool totallyDisabled = CS$<>8__locals2.p.skills.GetSkill(SkillDefOf.Social).TotallyDisabled;
							bool flag = conceitedTraits.Any<Trait>();
							bool flag2 = !CS$<>8__locals2.p.HasPsylink && traitsAffectingPsylinkNegatively.Any<Trait>();
							if (totallyDisabled || flag || flag2)
							{
								NamedArgument arg = CS$<>8__locals2.p.Named("PAWN");
								NamedArgument arg2 = questPart_GiveRoyalFavor.faction.Named("FACTION");
								TaggedString t4 = null;
								if (totallyDisabled)
								{
									t4 = "RoyalIncapableOfSocial".Translate(arg, arg2);
								}
								TaggedString t2 = null;
								if (flag)
								{
									t2 = "RoyalWithConceitedTrait".Translate(arg, arg2, (from t in conceitedTraits
									select t.Label).ToCommaList(true, false));
								}
								TaggedString t3 = null;
								if (flag2)
								{
									t3 = "RoyalWithTraitAffectingPsylinkNegatively".Translate(arg, arg2, (from t in traitsAffectingPsylinkNegatively
									select t.Label).ToCommaList(true, false));
								}
								TaggedString taggedString = "QuestGivesRoyalFavor".Translate(arg, arg2);
								if (totallyDisabled)
								{
									taggedString += "\n\n" + t4;
								}
								if (flag)
								{
									taggedString += "\n\n" + t2;
								}
								if (flag2)
								{
									taggedString += "\n\n" + t3;
								}
								taggedString += "\n\n" + "WantToContinue".Translate();
								Find.WindowStack.Add(new Dialog_MessageBox(taggedString, "Confirm".Translate(), new Action(base.<AcceptQuestByInterface>g__AcceptAction|1), "GoBack".Translate(), null, null, false, null, null));
								return;
							}
							base.<AcceptQuestByInterface>g__AcceptAction|1();
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
				}
			}
			if (list.Count > 0)
			{
				Find.WindowStack.Add(new FloatMenu(list));
				return;
			}
			Messages.Message("MessageNoColonistCanAcceptQuest".Translate(Faction.OfPlayer.def.pawnsPlural), MessageTypeDefOf.RejectInput, false);
		}

		// Token: 0x060078EB RID: 30955 RVA: 0x002ABEF8 File Offset: 0x002AA0F8
		[CompilerGenerated]
		private void <DoQuestsList>g__DrawQuest|47_0(Quest quest, int indent, ref MainTabWindow_Quests.<>c__DisplayClass47_0 A_3)
		{
			if (MainTabWindow_Quests.tmpQuestsVisited.Contains(quest))
			{
				return;
			}
			if (quest.parent != null && MainTabWindow_Quests.tmpQuestsToShow.Contains(quest.parent) && !MainTabWindow_Quests.tmpQuestsVisited.Contains(quest.parent))
			{
				return;
			}
			float num = A_3.scrollPosition.y - 32f;
			float num2 = A_3.scrollPosition.y + A_3.scrollOutRect.height;
			if (A_3.curY > num && A_3.curY < num2)
			{
				float num3 = (float)indent * 10f;
				this.DoRow(new Rect(num3, A_3.curY, A_3.scrollViewRect.width - 4f - num3, 32f), quest);
			}
			A_3.curY += 32f;
			MainTabWindow_Quests.tmpQuestsVisited.Add(quest);
			indent++;
			foreach (Quest quest2 in quest.GetSubquests(null))
			{
				if (MainTabWindow_Quests.tmpQuestsToShow.Contains(quest2))
				{
					this.<DoQuestsList>g__DrawQuest|47_0(quest2, indent, ref A_3);
				}
			}
		}

		// Token: 0x04004313 RID: 17171
		private Quest selected;

		// Token: 0x04004314 RID: 17172
		private MainTabWindow_Quests.QuestsTab curTab;

		// Token: 0x04004315 RID: 17173
		private List<TabRecord> tabs = new List<TabRecord>();

		// Token: 0x04004316 RID: 17174
		private Vector2 scrollPosition_available;

		// Token: 0x04004317 RID: 17175
		private Vector2 scrollPosition_active;

		// Token: 0x04004318 RID: 17176
		private Vector2 scrollPosition_historical;

		// Token: 0x04004319 RID: 17177
		private Vector2 selectedQuestScrollPosition;

		// Token: 0x0400431A RID: 17178
		private float selectedQuestLastHeight;

		// Token: 0x0400431B RID: 17179
		private bool showDebugInfo;

		// Token: 0x0400431C RID: 17180
		private List<QuestPart> tmpQuestParts = new List<QuestPart>();

		// Token: 0x0400431D RID: 17181
		private string debugSendSignalTextField;

		// Token: 0x0400431E RID: 17182
		private bool showAll;

		// Token: 0x0400431F RID: 17183
		private const float LeftRectWidthFraction = 0.36f;

		// Token: 0x04004320 RID: 17184
		private const float RowHeight = 32f;

		// Token: 0x04004321 RID: 17185
		private const float CheckBoxHeight = 24f;

		// Token: 0x04004322 RID: 17186
		private const float ShowDebugInfoToggleWidth = 110f;

		// Token: 0x04004323 RID: 17187
		private const float DismisIconWidth = 32f;

		// Token: 0x04004324 RID: 17188
		private const float TimeInfoWidth = 35f;

		// Token: 0x04004325 RID: 17189
		private const float CharityIconWidth = 32f;

		// Token: 0x04004326 RID: 17190
		private static readonly Color TimeLimitColor = new Color(1f, 1f, 1f, 0.7f);

		// Token: 0x04004327 RID: 17191
		private static readonly Color AcceptanceRequirementsColor = new Color(1f, 0.25f, 0.25f);

		// Token: 0x04004328 RID: 17192
		private static readonly Color AcceptanceRequirementsBoxColor = new Color(0.62f, 0.18f, 0.18f);

		// Token: 0x04004329 RID: 17193
		private static readonly Color acceptanceRequirementsBoxBgColor = new Color(0.13f, 0.13f, 0.13f);

		// Token: 0x0400432A RID: 17194
		private static readonly Color IdeoCharityTextColor = new Color32(byte.MaxValue, 237, 38, byte.MaxValue);

		// Token: 0x0400432B RID: 17195
		private static readonly Color IdeoCharityBoxBorderColor = new Color32(205, 207, 18, byte.MaxValue);

		// Token: 0x0400432C RID: 17196
		private static readonly Color IdeoCharityBoxBackgroundColor = new Color(0.13f, 0.13f, 0.13f);

		// Token: 0x0400432D RID: 17197
		private static readonly Color QuestCompletedColor = GenColor.FromHex("1e591a");

		// Token: 0x0400432E RID: 17198
		private static readonly Color QuestFailedColor = GenColor.FromHex("5e2f2f");

		// Token: 0x0400432F RID: 17199
		private static readonly Color QuestExpiredColor = GenColor.FromHex("916e2d");

		// Token: 0x04004330 RID: 17200
		private const int RowIconSize = 15;

		// Token: 0x04004331 RID: 17201
		private const float RatingWidth = 60f;

		// Token: 0x04004332 RID: 17202
		private const float RewardsConfigButtonHeight = 40f;

		// Token: 0x04004333 RID: 17203
		private static Texture2D RatingIcon = null;

		// Token: 0x04004334 RID: 17204
		private static readonly Texture2D DismissIcon = ContentFinder<Texture2D>.Get("UI/Buttons/Dismiss", true);

		// Token: 0x04004335 RID: 17205
		private static readonly Texture2D ResumeQuestIcon = ContentFinder<Texture2D>.Get("UI/Buttons/UnDismiss", true);

		// Token: 0x04004336 RID: 17206
		private static readonly Texture2D QuestDismissedIcon = ContentFinder<Texture2D>.Get("UI/Icons/DismissedQuestIcon", true);

		// Token: 0x04004337 RID: 17207
		private static readonly Texture2D CharityQuestIcon = ContentFinder<Texture2D>.Get("UI/Icons/CharityQuestIcon", true);

		// Token: 0x04004338 RID: 17208
		private const float IndentWidth = 10f;

		// Token: 0x04004339 RID: 17209
		private static List<Quest> tmpQuestsToShow = new List<Quest>();

		// Token: 0x0400433A RID: 17210
		private static HashSet<Quest> tmpQuestsVisited = new HashSet<Quest>();

		// Token: 0x0400433B RID: 17211
		private static List<Thing> tmpColonistsForIdeo = new List<Thing>();

		// Token: 0x0400433C RID: 17212
		private static List<GenUI.AnonymousStackElement> tmpStackElements = new List<GenUI.AnonymousStackElement>();

		// Token: 0x0400433D RID: 17213
		private static List<Rect> layoutRewardsRects = new List<Rect>();

		// Token: 0x0400433E RID: 17214
		private static List<QuestPart> tmpRemainingQuestParts = new List<QuestPart>();

		// Token: 0x0400433F RID: 17215
		private static List<GlobalTargetInfo> tmpLookTargets = new List<GlobalTargetInfo>();

		// Token: 0x04004340 RID: 17216
		private static List<GlobalTargetInfo> tmpSelectTargets = new List<GlobalTargetInfo>();

		// Token: 0x02002777 RID: 10103
		private enum QuestsTab : byte
		{
			// Token: 0x04009571 RID: 38257
			Available,
			// Token: 0x04009572 RID: 38258
			Active,
			// Token: 0x04009573 RID: 38259
			Historical
		}
	}
}
