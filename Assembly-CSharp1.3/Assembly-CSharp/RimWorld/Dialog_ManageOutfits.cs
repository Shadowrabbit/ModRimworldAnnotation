using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012F2 RID: 4850
	public class Dialog_ManageOutfits : Window
	{
		// Token: 0x17001478 RID: 5240
		// (get) Token: 0x0600748E RID: 29838 RVA: 0x00279A3E File Offset: 0x00277C3E
		// (set) Token: 0x0600748F RID: 29839 RVA: 0x00279A46 File Offset: 0x00277C46
		private Outfit SelectedOutfit
		{
			get
			{
				return this.selOutfitInt;
			}
			set
			{
				this.CheckSelectedOutfitHasName();
				this.selOutfitInt = value;
			}
		}

		// Token: 0x17001479 RID: 5241
		// (get) Token: 0x06007490 RID: 29840 RVA: 0x002795D5 File Offset: 0x002777D5
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(700f, 700f);
			}
		}

		// Token: 0x06007491 RID: 29841 RVA: 0x00279A55 File Offset: 0x00277C55
		private void CheckSelectedOutfitHasName()
		{
			if (this.SelectedOutfit != null && this.SelectedOutfit.label.NullOrEmpty())
			{
				this.SelectedOutfit.label = "Unnamed";
			}
		}

		// Token: 0x06007492 RID: 29842 RVA: 0x00279A84 File Offset: 0x00277C84
		public Dialog_ManageOutfits(Outfit selectedOutfit)
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
			if (Dialog_ManageOutfits.apparelGlobalFilter == null)
			{
				Dialog_ManageOutfits.apparelGlobalFilter = new ThingFilter();
				Dialog_ManageOutfits.apparelGlobalFilter.SetAllow(ThingCategoryDefOf.Apparel, true, null, null);
			}
			this.SelectedOutfit = selectedOutfit;
		}

		// Token: 0x06007493 RID: 29843 RVA: 0x00279AEF File Offset: 0x00277CEF
		public override void PreOpen()
		{
			base.PreOpen();
			this.thingFilterState.quickSearch.Reset();
		}

		// Token: 0x06007494 RID: 29844 RVA: 0x00279B08 File Offset: 0x00277D08
		public override void DoWindowContents(Rect inRect)
		{
			float num = 0f;
			Rect rect = new Rect(0f, 0f, 150f, 35f);
			num += 150f;
			if (Widgets.ButtonText(rect, "SelectOutfit".Translate(), true, true, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (Outfit localOut3 in Current.Game.outfitDatabase.AllOutfits)
				{
					Outfit localOut = localOut3;
					list.Add(new FloatMenuOption(localOut.label, delegate()
					{
						this.SelectedOutfit = localOut;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			num += 10f;
			Rect rect2 = new Rect(num, 0f, 150f, 35f);
			num += 150f;
			if (Widgets.ButtonText(rect2, "NewOutfit".Translate(), true, true, true))
			{
				this.SelectedOutfit = Current.Game.outfitDatabase.MakeNewOutfit();
			}
			num += 10f;
			Rect rect3 = new Rect(num, 0f, 150f, 35f);
			num += 150f;
			if (Widgets.ButtonText(rect3, "DeleteOutfit".Translate(), true, true, true))
			{
				List<FloatMenuOption> list2 = new List<FloatMenuOption>();
				foreach (Outfit localOut2 in Current.Game.outfitDatabase.AllOutfits)
				{
					Outfit localOut = localOut2;
					list2.Add(new FloatMenuOption(localOut.label, delegate()
					{
						AcceptanceReport acceptanceReport = Current.Game.outfitDatabase.TryDelete(localOut);
						if (!acceptanceReport.Accepted)
						{
							Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
							return;
						}
						if (localOut == this.SelectedOutfit)
						{
							this.SelectedOutfit = null;
						}
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list2));
			}
			Rect rect4 = new Rect(0f, 40f, inRect.width, inRect.height - 40f - Window.CloseButSize.y).ContractedBy(10f);
			if (this.SelectedOutfit == null)
			{
				GUI.color = Color.grey;
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect4, "NoOutfitSelected".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				return;
			}
			GUI.BeginGroup(rect4);
			Dialog_ManageOutfits.DoNameInputRect(new Rect(0f, 0f, 200f, 30f), ref this.SelectedOutfit.label);
			ThingFilterUI.DoThingFilterConfigWindow(new Rect(0f, 40f, 300f, rect4.height - 45f - 10f), this.thingFilterState, this.SelectedOutfit.filter, Dialog_ManageOutfits.apparelGlobalFilter, 16, null, this.HiddenSpecialThingFilters(), false, null, null);
			GUI.EndGroup();
		}

		// Token: 0x06007495 RID: 29845 RVA: 0x00279E30 File Offset: 0x00278030
		private IEnumerable<SpecialThingFilterDef> HiddenSpecialThingFilters()
		{
			yield return SpecialThingFilterDefOf.AllowNonDeadmansApparel;
			yield break;
		}

		// Token: 0x06007496 RID: 29846 RVA: 0x00279E39 File Offset: 0x00278039
		public override void PreClose()
		{
			base.PreClose();
			this.CheckSelectedOutfitHasName();
		}

		// Token: 0x06007497 RID: 29847 RVA: 0x00279A2B File Offset: 0x00277C2B
		public static void DoNameInputRect(Rect rect, ref string name)
		{
			name = Widgets.TextField(rect, name, 30, Outfit.ValidNameRegex);
		}

		// Token: 0x0400403F RID: 16447
		private ThingFilterUI.UIState thingFilterState = new ThingFilterUI.UIState();

		// Token: 0x04004040 RID: 16448
		private Outfit selOutfitInt;

		// Token: 0x04004041 RID: 16449
		public const float TopAreaHeight = 40f;

		// Token: 0x04004042 RID: 16450
		public const float TopButtonHeight = 35f;

		// Token: 0x04004043 RID: 16451
		public const float TopButtonWidth = 150f;

		// Token: 0x04004044 RID: 16452
		private static ThingFilter apparelGlobalFilter;
	}
}
