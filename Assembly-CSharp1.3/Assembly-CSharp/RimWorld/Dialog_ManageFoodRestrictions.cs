using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012F1 RID: 4849
	public class Dialog_ManageFoodRestrictions : Window
	{
		// Token: 0x17001476 RID: 5238
		// (get) Token: 0x06007484 RID: 29828 RVA: 0x002795BE File Offset: 0x002777BE
		// (set) Token: 0x06007485 RID: 29829 RVA: 0x002795C6 File Offset: 0x002777C6
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

		// Token: 0x17001477 RID: 5239
		// (get) Token: 0x06007486 RID: 29830 RVA: 0x002795D5 File Offset: 0x002777D5
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(700f, 700f);
			}
		}

		// Token: 0x06007487 RID: 29831 RVA: 0x002795E6 File Offset: 0x002777E6
		private void CheckSelectedFoodRestrictionHasName()
		{
			if (this.SelectedFoodRestriction != null && this.SelectedFoodRestriction.label.NullOrEmpty())
			{
				this.SelectedFoodRestriction.label = "Unnamed";
			}
		}

		// Token: 0x06007488 RID: 29832 RVA: 0x00279614 File Offset: 0x00277814
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
				foreach (ThingDef thingDef in from x in DefDatabase<ThingDef>.AllDefs
				where x.GetStatValueAbstract(StatDefOf.Nutrition, null) > 0f
				select x)
				{
					Dialog_ManageFoodRestrictions.foodGlobalFilter.SetAllow(thingDef, true);
				}
			}
			this.SelectedFoodRestriction = selectedFoodRestriction;
		}

		// Token: 0x06007489 RID: 29833 RVA: 0x002796D8 File Offset: 0x002778D8
		public override void PreOpen()
		{
			base.PreOpen();
			this.thingFilterState.quickSearch.Reset();
		}

		// Token: 0x0600748A RID: 29834 RVA: 0x002796F0 File Offset: 0x002778F0
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
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
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
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list2));
			}
			Rect rect4 = new Rect(0f, 40f, inRect.width, inRect.height - 40f - Window.CloseButSize.y).ContractedBy(10f);
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
			ThingFilterUI.DoThingFilterConfigWindow(new Rect(0f, 40f, 300f, rect4.height - 45f - 10f), this.thingFilterState, this.SelectedFoodRestriction.filter, Dialog_ManageFoodRestrictions.foodGlobalFilter, 1, null, this.HiddenSpecialThingFilters(), true, null, null);
			GUI.EndGroup();
		}

		// Token: 0x0600748B RID: 29835 RVA: 0x00279A14 File Offset: 0x00277C14
		private IEnumerable<SpecialThingFilterDef> HiddenSpecialThingFilters()
		{
			yield return SpecialThingFilterDefOf.AllowFresh;
			yield break;
		}

		// Token: 0x0600748C RID: 29836 RVA: 0x00279A1D File Offset: 0x00277C1D
		public override void PreClose()
		{
			base.PreClose();
			this.CheckSelectedFoodRestrictionHasName();
		}

		// Token: 0x0600748D RID: 29837 RVA: 0x00279A2B File Offset: 0x00277C2B
		public static void DoNameInputRect(Rect rect, ref string name)
		{
			name = Widgets.TextField(rect, name, 30, Outfit.ValidNameRegex);
		}

		// Token: 0x04004039 RID: 16441
		private ThingFilterUI.UIState thingFilterState = new ThingFilterUI.UIState();

		// Token: 0x0400403A RID: 16442
		private FoodRestriction selFoodRestrictionInt;

		// Token: 0x0400403B RID: 16443
		private const float TopAreaHeight = 40f;

		// Token: 0x0400403C RID: 16444
		private const float TopButtonHeight = 35f;

		// Token: 0x0400403D RID: 16445
		private const float TopButtonWidth = 150f;

		// Token: 0x0400403E RID: 16446
		private static ThingFilter foodGlobalFilter;
	}
}
