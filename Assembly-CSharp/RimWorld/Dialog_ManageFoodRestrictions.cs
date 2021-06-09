using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019F8 RID: 6648
	public class Dialog_ManageFoodRestrictions : Window
	{
		// Token: 0x17001762 RID: 5986
		// (get) Token: 0x0600930C RID: 37644 RVA: 0x00062814 File Offset: 0x00060A14
		// (set) Token: 0x0600930D RID: 37645 RVA: 0x0006281C File Offset: 0x00060A1C
		private FoodRestriction SelectedFoodRestriction
		{
			get
			{
				return this.selFoodRestrictionInt;
			}
			set
			{
				this.CheckSelectedFoodRestrictionHasName();
				this.selFoodRestrictionInt = value;
			}
		}

		// Token: 0x17001763 RID: 5987
		// (get) Token: 0x0600930E RID: 37646 RVA: 0x0006282B File Offset: 0x00060A2B
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(700f, 700f);
			}
		}

		// Token: 0x0600930F RID: 37647 RVA: 0x0006283C File Offset: 0x00060A3C
		private void CheckSelectedFoodRestrictionHasName()
		{
			if (this.SelectedFoodRestriction != null && this.SelectedFoodRestriction.label.NullOrEmpty())
			{
				this.SelectedFoodRestriction.label = "Unnamed";
			}
		}

		// Token: 0x06009310 RID: 37648 RVA: 0x002A6114 File Offset: 0x002A4314
		public Dialog_ManageFoodRestrictions(FoodRestriction selectedFoodRestriction)
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
			if (Dialog_ManageFoodRestrictions.foodGlobalFilter == null)
			{
				Dialog_ManageFoodRestrictions.foodGlobalFilter = new ThingFilter();
				Dialog_ManageFoodRestrictions.foodGlobalFilter.SetAllow(ThingCategoryDefOf.Foods, true, null, null);
				Dialog_ManageFoodRestrictions.foodGlobalFilter.SetAllow(ThingCategoryDefOf.CorpsesHumanlike, true, null, null);
				Dialog_ManageFoodRestrictions.foodGlobalFilter.SetAllow(ThingCategoryDefOf.CorpsesAnimal, true, null, null);
			}
			this.SelectedFoodRestriction = selectedFoodRestriction;
		}

		// Token: 0x06009311 RID: 37649 RVA: 0x002A6198 File Offset: 0x002A4398
		public override void DoWindowContents(Rect inRect)
		{
			float num = 0f;
			Rect rect = new Rect(0f, 0f, 150f, 35f);
			num += 150f;
			if (Widgets.ButtonText(rect, "SelectFoodRestriction".Translate(), true, true, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (FoodRestriction localRestriction3 in Current.Game.foodRestrictionDatabase.AllFoodRestrictions)
				{
					FoodRestriction localRestriction = localRestriction3;
					list.Add(new FloatMenuOption(localRestriction.label, delegate()
					{
						this.SelectedFoodRestriction = localRestriction;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			num += 10f;
			Rect rect2 = new Rect(num, 0f, 150f, 35f);
			num += 150f;
			if (Widgets.ButtonText(rect2, "NewFoodRestriction".Translate(), true, true, true))
			{
				this.SelectedFoodRestriction = Current.Game.foodRestrictionDatabase.MakeNewFoodRestriction();
			}
			num += 10f;
			Rect rect3 = new Rect(num, 0f, 150f, 35f);
			num += 150f;
			if (Widgets.ButtonText(rect3, "DeleteFoodRestriction".Translate(), true, true, true))
			{
				List<FloatMenuOption> list2 = new List<FloatMenuOption>();
				foreach (FoodRestriction localRestriction2 in Current.Game.foodRestrictionDatabase.AllFoodRestrictions)
				{
					FoodRestriction localRestriction = localRestriction2;
					list2.Add(new FloatMenuOption(localRestriction.label, delegate()
					{
						AcceptanceReport acceptanceReport = Current.Game.foodRestrictionDatabase.TryDelete(localRestriction);
						if (!acceptanceReport.Accepted)
						{
							Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
							return;
						}
						if (localRestriction == this.SelectedFoodRestriction)
						{
							this.SelectedFoodRestriction = null;
						}
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list2));
			}
			Rect rect4 = new Rect(0f, 40f, inRect.width, inRect.height - 40f - this.CloseButSize.y).ContractedBy(10f);
			if (this.SelectedFoodRestriction == null)
			{
				GUI.color = Color.grey;
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect4, "NoFoodRestrictionSelected".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				return;
			}
			GUI.BeginGroup(rect4);
			Dialog_ManageFoodRestrictions.DoNameInputRect(new Rect(0f, 0f, 200f, 30f), ref this.SelectedFoodRestriction.label);
			ThingFilterUI.DoThingFilterConfigWindow(new Rect(0f, 40f, 300f, rect4.height - 45f - 10f), ref this.scrollPosition, this.SelectedFoodRestriction.filter, Dialog_ManageFoodRestrictions.foodGlobalFilter, 1, null, this.HiddenSpecialThingFilters(), true, null, null);
			GUI.EndGroup();
		}

		// Token: 0x06009312 RID: 37650 RVA: 0x00062868 File Offset: 0x00060A68
		private IEnumerable<SpecialThingFilterDef> HiddenSpecialThingFilters()
		{
			yield return SpecialThingFilterDefOf.AllowFresh;
			yield break;
		}

		// Token: 0x06009313 RID: 37651 RVA: 0x00062871 File Offset: 0x00060A71
		public override void PreClose()
		{
			base.PreClose();
			this.CheckSelectedFoodRestrictionHasName();
		}

		// Token: 0x06009314 RID: 37652 RVA: 0x0006287F File Offset: 0x00060A7F
		public static void DoNameInputRect(Rect rect, ref string name)
		{
			name = Widgets.TextField(rect, name, 30, Outfit.ValidNameRegex);
		}

		// Token: 0x04005D2E RID: 23854
		private Vector2 scrollPosition;

		// Token: 0x04005D2F RID: 23855
		private FoodRestriction selFoodRestrictionInt;

		// Token: 0x04005D30 RID: 23856
		private const float TopAreaHeight = 40f;

		// Token: 0x04005D31 RID: 23857
		private const float TopButtonHeight = 35f;

		// Token: 0x04005D32 RID: 23858
		private const float TopButtonWidth = 150f;

		// Token: 0x04005D33 RID: 23859
		private static ThingFilter foodGlobalFilter;
	}
}
