using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012F0 RID: 4848
	[StaticConstructorOnStartup]
	public class Dialog_ManageDrugPolicies : Window
	{
		// Token: 0x17001474 RID: 5236
		// (get) Token: 0x06007476 RID: 29814 RVA: 0x00278A94 File Offset: 0x00276C94
		// (set) Token: 0x06007477 RID: 29815 RVA: 0x00278A9C File Offset: 0x00276C9C
		private DrugPolicy SelectedPolicy
		{
			get
			{
				return this.selPolicy;
			}
			set
			{
				this.CheckSelectedPolicyHasName();
				this.selPolicy = value;
			}
		}

		// Token: 0x17001475 RID: 5237
		// (get) Token: 0x06007478 RID: 29816 RVA: 0x00278AAB File Offset: 0x00276CAB
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(900f, 700f);
			}
		}

		// Token: 0x06007479 RID: 29817 RVA: 0x00278ABC File Offset: 0x00276CBC
		private void CheckSelectedPolicyHasName()
		{
			if (this.SelectedPolicy != null && this.SelectedPolicy.label.NullOrEmpty())
			{
				this.SelectedPolicy.label = "Unnamed";
			}
		}

		// Token: 0x0600747A RID: 29818 RVA: 0x00278AE8 File Offset: 0x00276CE8
		public Dialog_ManageDrugPolicies(DrugPolicy selectedAssignedDrugs)
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
			this.SelectedPolicy = selectedAssignedDrugs;
		}

		// Token: 0x0600747B RID: 29819 RVA: 0x00278B1C File Offset: 0x00276D1C
		public override void DoWindowContents(Rect inRect)
		{
			float num = 0f;
			Rect rect = new Rect(0f, 0f, 150f, 35f);
			num += 150f;
			if (Widgets.ButtonText(rect, "SelectDrugPolicy".Translate(), true, true, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (DrugPolicy localAssignedDrugs3 in Current.Game.drugPolicyDatabase.AllPolicies)
				{
					DrugPolicy localAssignedDrugs = localAssignedDrugs3;
					list.Add(new FloatMenuOption(localAssignedDrugs.label, delegate()
					{
						this.SelectedPolicy = localAssignedDrugs;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			num += 10f;
			Rect rect2 = new Rect(num, 0f, 150f, 35f);
			num += 150f;
			if (Widgets.ButtonText(rect2, "NewDrugPolicy".Translate(), true, true, true))
			{
				this.SelectedPolicy = Current.Game.drugPolicyDatabase.MakeNewDrugPolicy();
			}
			num += 10f;
			Rect rect3 = new Rect(num, 0f, 150f, 35f);
			num += 150f;
			if (Widgets.ButtonText(rect3, "DeleteDrugPolicy".Translate(), true, true, true))
			{
				List<FloatMenuOption> list2 = new List<FloatMenuOption>();
				foreach (DrugPolicy localAssignedDrugs2 in Current.Game.drugPolicyDatabase.AllPolicies)
				{
					DrugPolicy localAssignedDrugs = localAssignedDrugs2;
					list2.Add(new FloatMenuOption(localAssignedDrugs.label, delegate()
					{
						AcceptanceReport acceptanceReport = Current.Game.drugPolicyDatabase.TryDelete(localAssignedDrugs);
						if (!acceptanceReport.Accepted)
						{
							Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
							return;
						}
						if (localAssignedDrugs == this.SelectedPolicy)
						{
							this.SelectedPolicy = null;
						}
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list2));
			}
			Rect rect4 = new Rect(0f, 40f, inRect.width, inRect.height - 40f - Window.CloseButSize.y).ContractedBy(10f);
			if (this.SelectedPolicy == null)
			{
				GUI.color = Color.grey;
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect4, "NoDrugPolicySelected".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				return;
			}
			GUI.BeginGroup(rect4);
			Dialog_ManageDrugPolicies.DoNameInputRect(new Rect(0f, 0f, 200f, 30f), ref this.SelectedPolicy.label);
			Rect rect5 = new Rect(0f, 40f, rect4.width, rect4.height - 45f - 10f);
			this.DoPolicyConfigArea(rect5);
			GUI.EndGroup();
		}

		// Token: 0x0600747C RID: 29820 RVA: 0x00278E28 File Offset: 0x00277028
		public override void PostOpen()
		{
			base.PostOpen();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.DrugPolicies, KnowledgeAmount.Total);
		}

		// Token: 0x0600747D RID: 29821 RVA: 0x00278E3B File Offset: 0x0027703B
		public override void PreClose()
		{
			base.PreClose();
			this.CheckSelectedPolicyHasName();
		}

		// Token: 0x0600747E RID: 29822 RVA: 0x00278E49 File Offset: 0x00277049
		public static void DoNameInputRect(Rect rect, ref string name)
		{
			name = Widgets.TextField(rect, name, 30, Dialog_ManageDrugPolicies.ValidNameRegex);
		}

		// Token: 0x0600747F RID: 29823 RVA: 0x00278E5C File Offset: 0x0027705C
		private void DoPolicyConfigArea(Rect rect)
		{
			Rect rect2 = rect;
			rect2.height = 54f;
			Rect rect3 = rect;
			rect3.yMin = rect2.yMax;
			rect3.height -= 50f;
			Rect rect4 = rect;
			rect4.yMin = rect4.yMax - 50f;
			this.DoColumnLabels(rect2);
			Widgets.DrawMenuSection(rect3);
			if (this.SelectedPolicy.Count == 0)
			{
				GUI.color = Color.grey;
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect3, "NoDrugs".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				return;
			}
			float height = (float)this.SelectedPolicy.Count * 35f;
			Rect viewRect = new Rect(0f, 0f, rect3.width - 16f, height);
			Widgets.BeginScrollView(rect3, ref this.scrollPosition, viewRect, true);
			DrugPolicy selectedPolicy = this.SelectedPolicy;
			for (int i = 0; i < selectedPolicy.Count; i++)
			{
				Rect rect5 = new Rect(0f, (float)i * 35f, viewRect.width, 35f);
				this.DoEntryRow(rect5, selectedPolicy[i]);
			}
			Widgets.EndScrollView();
		}

		// Token: 0x06007480 RID: 29824 RVA: 0x00278F94 File Offset: 0x00277194
		private void CalculateColumnsWidths(Rect rect, out float addictionWidth, out float allowJoyWidth, out float scheduledWidth, out float drugIconWidth, out float drugNameWidth, out float frequencyWidth, out float moodThresholdWidth, out float joyThresholdWidth, out float takeToInventoryWidth)
		{
			float num = rect.width - 27f - 108f;
			drugIconWidth = 27f;
			drugNameWidth = num * 0.2f;
			addictionWidth = 36f;
			allowJoyWidth = 36f;
			scheduledWidth = 36f;
			frequencyWidth = num * 0.35f;
			moodThresholdWidth = num * 0.15f;
			joyThresholdWidth = num * 0.15f;
			takeToInventoryWidth = num * 0.15f;
		}

		// Token: 0x06007481 RID: 29825 RVA: 0x00279008 File Offset: 0x00277208
		private void DoColumnLabels(Rect rect)
		{
			rect.width -= 16f;
			float num;
			float num2;
			float num3;
			float num4;
			float num5;
			float num6;
			float num7;
			float num8;
			float num9;
			this.CalculateColumnsWidths(rect, out num, out num2, out num3, out num4, out num5, out num6, out num7, out num8, out num9);
			float num10 = rect.x;
			Text.Anchor = TextAnchor.LowerCenter;
			Rect rect2 = new Rect(num10 + 4f, rect.y, num5 + num4, rect.height);
			Widgets.Label(rect2, "DrugColumnLabel".Translate());
			TooltipHandler.TipRegionByKey(rect2, "DrugNameColumnDesc");
			num10 += num5 + num4;
			Text.Anchor = TextAnchor.UpperCenter;
			Rect rect3 = new Rect(num10, rect.y, num9, rect.height);
			Widgets.Label(rect3, "TakeToInventoryColumnLabel".Translate());
			TooltipHandler.TipRegionByKey(rect3, "TakeToInventoryColumnDesc");
			num10 += num9;
			Rect rect4 = new Rect(num10, rect.y, num2 + num2, rect.height / 2f);
			Widgets.Label(rect4, "DrugUsageColumnLabel".Translate());
			TooltipHandler.TipRegionByKey(rect4, "DrugUsageColumnDesc");
			Rect rect5 = new Rect(num10, rect.yMax - 24f, 24f, 24f);
			GUI.DrawTexture(rect5, Dialog_ManageDrugPolicies.IconForAddiction);
			TooltipHandler.TipRegionByKey(rect5, "DrugUsageTipForAddiction");
			num10 += num;
			Rect rect6 = new Rect(num10, rect.yMax - 24f, 24f, 24f);
			GUI.DrawTexture(rect6, Dialog_ManageDrugPolicies.IconForJoy);
			TooltipHandler.TipRegionByKey(rect6, "DrugUsageTipForJoy");
			num10 += num2;
			Rect rect7 = new Rect(num10, rect.yMax - 24f, 24f, 24f);
			GUI.DrawTexture(rect7, Dialog_ManageDrugPolicies.IconScheduled);
			TooltipHandler.TipRegionByKey(rect7, "DrugUsageTipScheduled");
			num10 += num3;
			Text.Anchor = TextAnchor.LowerCenter;
			Rect rect8 = new Rect(num10, rect.y, num6, rect.height);
			Widgets.Label(rect8, "FrequencyColumnLabel".Translate());
			TooltipHandler.TipRegionByKey(rect8, "FrequencyColumnDesc");
			num10 += num6;
			Rect rect9 = new Rect(num10, rect.y, num7, rect.height);
			Widgets.Label(rect9, "MoodThresholdColumnLabel".Translate());
			TooltipHandler.TipRegionByKey(rect9, "MoodThresholdColumnDesc");
			num10 += num7;
			Rect rect10 = new Rect(num10, rect.y, num8, rect.height);
			Widgets.Label(rect10, "JoyThresholdColumnLabel".Translate());
			TooltipHandler.TipRegionByKey(rect10, "JoyThresholdColumnDesc");
			num10 += num8;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06007482 RID: 29826 RVA: 0x00279268 File Offset: 0x00277468
		private void DoEntryRow(Rect rect, DrugPolicyEntry entry)
		{
			float num;
			float num2;
			float num3;
			float num4;
			float num5;
			float num6;
			float num7;
			float num8;
			float num9;
			this.CalculateColumnsWidths(rect, out num, out num2, out num3, out num4, out num5, out num6, out num7, out num8, out num9);
			Text.Anchor = TextAnchor.MiddleLeft;
			float num10 = rect.x;
			float num11 = (rect.height - num4) / 2f;
			Widgets.ThingIcon(new Rect(num10, rect.y + num11, num4, num4), entry.drug, null, null, 1f, null);
			num10 += num4;
			Widgets.Label(new Rect(num10, rect.y, num5, rect.height).ContractedBy(4f), entry.drug.LabelCap);
			Widgets.InfoCardButton(num10 + Text.CalcSize(entry.drug.LabelCap).x + 5f, rect.y + (rect.height - 24f) / 2f, entry.drug);
			num10 += num5;
			Widgets.TextFieldNumeric<int>(new Rect(num10, rect.y, num9, rect.height).ContractedBy(4f), ref entry.takeToInventory, ref entry.takeToInventoryTempBuffer, 0f, 15f);
			num10 += num9;
			if (entry.drug.IsAddictiveDrug)
			{
				Widgets.Checkbox(num10, rect.y, ref entry.allowedForAddiction, 24f, false, true, null, null);
			}
			num10 += num;
			if (entry.drug.IsPleasureDrug)
			{
				Widgets.Checkbox(num10, rect.y, ref entry.allowedForJoy, 24f, false, true, null, null);
			}
			num10 += num2;
			Widgets.Checkbox(num10, rect.y, ref entry.allowScheduled, 24f, false, true, null, null);
			num10 += num3;
			if (entry.allowScheduled)
			{
				entry.daysFrequency = Widgets.FrequencyHorizontalSlider(new Rect(num10, rect.y, num6, rect.height).ContractedBy(4f), entry.daysFrequency, 0.1f, 25f, true);
				num10 += num6;
				string label;
				if (entry.onlyIfMoodBelow < 1f)
				{
					label = entry.onlyIfMoodBelow.ToStringPercent();
				}
				else
				{
					label = "NoDrugUseRequirement".Translate();
				}
				entry.onlyIfMoodBelow = Widgets.HorizontalSlider(new Rect(num10, rect.y, num7, rect.height).ContractedBy(4f), entry.onlyIfMoodBelow, 0.01f, 1f, true, label, null, null, -1f);
				num10 += num7;
				string label2;
				if (entry.onlyIfJoyBelow < 1f)
				{
					label2 = entry.onlyIfJoyBelow.ToStringPercent();
				}
				else
				{
					label2 = "NoDrugUseRequirement".Translate();
				}
				entry.onlyIfJoyBelow = Widgets.HorizontalSlider(new Rect(num10, rect.y, num8, rect.height).ContractedBy(4f), entry.onlyIfJoyBelow, 0.01f, 1f, true, label2, null, null, -1f);
				num10 += num8;
			}
			else
			{
				num10 += num6 + num7 + num8;
			}
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0400402A RID: 16426
		private Vector2 scrollPosition;

		// Token: 0x0400402B RID: 16427
		private DrugPolicy selPolicy;

		// Token: 0x0400402C RID: 16428
		private const float TopAreaHeight = 40f;

		// Token: 0x0400402D RID: 16429
		private const float TopButtonHeight = 35f;

		// Token: 0x0400402E RID: 16430
		private const float TopButtonWidth = 150f;

		// Token: 0x0400402F RID: 16431
		private const float DrugEntryRowHeight = 35f;

		// Token: 0x04004030 RID: 16432
		private const float BottomButtonsAreaHeight = 50f;

		// Token: 0x04004031 RID: 16433
		private const float AddEntryButtonHeight = 35f;

		// Token: 0x04004032 RID: 16434
		private const float AddEntryButtonWidth = 150f;

		// Token: 0x04004033 RID: 16435
		private const float CellsPadding = 4f;

		// Token: 0x04004034 RID: 16436
		private static readonly Texture2D IconForAddiction = ContentFinder<Texture2D>.Get("UI/Icons/DrugPolicy/ForAddiction", true);

		// Token: 0x04004035 RID: 16437
		private static readonly Texture2D IconForJoy = ContentFinder<Texture2D>.Get("UI/Icons/DrugPolicy/ForJoy", true);

		// Token: 0x04004036 RID: 16438
		private static readonly Texture2D IconScheduled = ContentFinder<Texture2D>.Get("UI/Icons/DrugPolicy/Scheduled", true);

		// Token: 0x04004037 RID: 16439
		private static readonly Regex ValidNameRegex = Outfit.ValidNameRegex;

		// Token: 0x04004038 RID: 16440
		private const float UsageSpacing = 12f;
	}
}
