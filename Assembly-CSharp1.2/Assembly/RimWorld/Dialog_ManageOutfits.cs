using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019FC RID: 6652
	public class Dialog_ManageOutfits : Window
	{
		// Token: 0x17001766 RID: 5990
		// (get) Token: 0x06009321 RID: 37665 RVA: 0x000628CF File Offset: 0x00060ACF
		// (set) Token: 0x06009322 RID: 37666 RVA: 0x000628D7 File Offset: 0x00060AD7
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

		// Token: 0x17001767 RID: 5991
		// (get) Token: 0x06009323 RID: 37667 RVA: 0x0006282B File Offset: 0x00060A2B
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(700f, 700f);
			}
		}

		// Token: 0x06009324 RID: 37668 RVA: 0x000628E6 File Offset: 0x00060AE6
		private void CheckSelectedOutfitHasName()
		{
			if (this.SelectedOutfit != null && this.SelectedOutfit.label.NullOrEmpty())
			{
				this.SelectedOutfit.label = "Unnamed";
			}
		}

		// Token: 0x06009325 RID: 37669 RVA: 0x002A6594 File Offset: 0x002A4794
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

		// Token: 0x06009326 RID: 37670 RVA: 0x002A65F4 File Offset: 0x002A47F4
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
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
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
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list2));
			}
			Rect rect4 = new Rect(0f, 40f, inRect.width, inRect.height - 40f - this.CloseButSize.y).ContractedBy(10f);
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
			ThingFilterUI.DoThingFilterConfigWindow(new Rect(0f, 40f, 300f, rect4.height - 45f - 10f), ref this.scrollPosition, this.SelectedOutfit.filter, Dialog_ManageOutfits.apparelGlobalFilter, 16, null, this.HiddenSpecialThingFilters(), false, null, null);
			GUI.EndGroup();
		}

		// Token: 0x06009327 RID: 37671 RVA: 0x00062912 File Offset: 0x00060B12
		private IEnumerable<SpecialThingFilterDef> HiddenSpecialThingFilters()
		{
			yield return SpecialThingFilterDefOf.AllowNonDeadmansApparel;
			yield break;
		}

		// Token: 0x06009328 RID: 37672 RVA: 0x0006291B File Offset: 0x00060B1B
		public override void PreClose()
		{
			base.PreClose();
			this.CheckSelectedOutfitHasName();
		}

		// Token: 0x06009329 RID: 37673 RVA: 0x0006287F File Offset: 0x00060A7F
		public static void DoNameInputRect(Rect rect, ref string name)
		{
			name = Widgets.TextField(rect, name, 30, Outfit.ValidNameRegex);
		}

		// Token: 0x04005D3B RID: 23867
		private Vector2 scrollPosition;

		// Token: 0x04005D3C RID: 23868
		private Outfit selOutfitInt;

		// Token: 0x04005D3D RID: 23869
		public const float TopAreaHeight = 40f;

		// Token: 0x04005D3E RID: 23870
		public const float TopButtonHeight = 35f;

		// Token: 0x04005D3F RID: 23871
		public const float TopButtonWidth = 150f;

		// Token: 0x04005D40 RID: 23872
		private static ThingFilter apparelGlobalFilter;
	}
}
