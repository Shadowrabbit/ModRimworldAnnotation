using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001354 RID: 4948
	public class ITab_Storage : ITab
	{
		// Token: 0x1700150B RID: 5387
		// (get) Token: 0x060077C4 RID: 30660 RVA: 0x002A3264 File Offset: 0x002A1464
		protected virtual IStoreSettingsParent SelStoreSettingsParent
		{
			get
			{
				Thing thing = base.SelObject as Thing;
				if (thing == null)
				{
					return base.SelObject as IStoreSettingsParent;
				}
				IStoreSettingsParent thingOrThingCompStoreSettingsParent = this.GetThingOrThingCompStoreSettingsParent(thing);
				if (thingOrThingCompStoreSettingsParent != null)
				{
					return thingOrThingCompStoreSettingsParent;
				}
				return null;
			}
		}

		// Token: 0x1700150C RID: 5388
		// (get) Token: 0x060077C5 RID: 30661 RVA: 0x002A329C File Offset: 0x002A149C
		public override bool IsVisible
		{
			get
			{
				Thing thing = base.SelObject as Thing;
				if (thing != null && thing.Faction != null && thing.Faction != Faction.OfPlayer)
				{
					return false;
				}
				IStoreSettingsParent selStoreSettingsParent = this.SelStoreSettingsParent;
				return selStoreSettingsParent != null && selStoreSettingsParent.StorageTabVisible;
			}
		}

		// Token: 0x1700150D RID: 5389
		// (get) Token: 0x060077C6 RID: 30662 RVA: 0x000126F5 File Offset: 0x000108F5
		protected virtual bool IsPrioritySettingVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700150E RID: 5390
		// (get) Token: 0x060077C7 RID: 30663 RVA: 0x002A32E1 File Offset: 0x002A14E1
		private float TopAreaHeight
		{
			get
			{
				return (float)(this.IsPrioritySettingVisible ? 35 : 20);
			}
		}

		// Token: 0x060077C8 RID: 30664 RVA: 0x002A32F2 File Offset: 0x002A14F2
		public ITab_Storage()
		{
			this.size = ITab_Storage.WinSize;
			this.labelKey = "TabStorage";
			this.tutorTag = "Storage";
		}

		// Token: 0x060077C9 RID: 30665 RVA: 0x002A3326 File Offset: 0x002A1526
		public override void OnOpen()
		{
			base.OnOpen();
			this.thingFilterState.quickSearch.Reset();
		}

		// Token: 0x060077CA RID: 30666 RVA: 0x002A3340 File Offset: 0x002A1540
		protected override void FillTab()
		{
			IStoreSettingsParent storeSettingsParent = this.SelStoreSettingsParent;
			StorageSettings settings = storeSettingsParent.GetStoreSettings();
			Rect position = new Rect(0f, 0f, ITab_Storage.WinSize.x, ITab_Storage.WinSize.y).ContractedBy(10f);
			GUI.BeginGroup(position);
			if (this.IsPrioritySettingVisible)
			{
				Text.Font = GameFont.Small;
				Rect rect = new Rect(0f, 0f, 160f, this.TopAreaHeight - 6f);
				if (Widgets.ButtonText(rect, "Priority".Translate() + ": " + settings.Priority.Label().CapitalizeFirst(), true, true, true))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (object obj in Enum.GetValues(typeof(StoragePriority)))
					{
						StoragePriority storagePriority = (StoragePriority)obj;
						if (storagePriority != StoragePriority.Unstored)
						{
							StoragePriority localPr = storagePriority;
							list.Add(new FloatMenuOption(localPr.Label().CapitalizeFirst(), delegate()
							{
								settings.Priority = localPr;
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
				UIHighlighter.HighlightOpportunity(rect, "StoragePriority");
			}
			ThingFilter parentFilter = null;
			if (storeSettingsParent.GetParentStoreSettings() != null)
			{
				parentFilter = storeSettingsParent.GetParentStoreSettings().filter;
			}
			Rect rect2 = new Rect(0f, this.TopAreaHeight, position.width, position.height - this.TopAreaHeight);
			Bill[] first = (from b in BillUtility.GlobalBills()
			where b is Bill_Production && b.GetStoreZone() == storeSettingsParent && b.recipe.WorkerCounter.CanPossiblyStoreInStockpile((Bill_Production)b, b.GetStoreZone())
			select b).ToArray<Bill>();
			ThingFilterUI.DoThingFilterConfigWindow(rect2, this.thingFilterState, settings.filter, parentFilter, 8, null, null, false, null, null);
			Bill[] second = (from b in BillUtility.GlobalBills()
			where b is Bill_Production && b.GetStoreZone() == storeSettingsParent && b.recipe.WorkerCounter.CanPossiblyStoreInStockpile((Bill_Production)b, b.GetStoreZone())
			select b).ToArray<Bill>();
			foreach (Bill bill in first.Except(second))
			{
				Messages.Message("MessageBillValidationStoreZoneInsufficient".Translate(bill.LabelCap, bill.billStack.billGiver.LabelShort.CapitalizeFirst(), bill.GetStoreZone().label), bill.billStack.billGiver as Thing, MessageTypeDefOf.RejectInput, false);
			}
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.StorageTab, KnowledgeAmount.FrameDisplayed);
			GUI.EndGroup();
		}

		// Token: 0x060077CB RID: 30667 RVA: 0x002A3638 File Offset: 0x002A1838
		protected IStoreSettingsParent GetThingOrThingCompStoreSettingsParent(Thing t)
		{
			IStoreSettingsParent storeSettingsParent = t as IStoreSettingsParent;
			if (storeSettingsParent != null)
			{
				return storeSettingsParent;
			}
			ThingWithComps thingWithComps = t as ThingWithComps;
			if (thingWithComps != null)
			{
				List<ThingComp> allComps = thingWithComps.AllComps;
				for (int i = 0; i < allComps.Count; i++)
				{
					storeSettingsParent = (allComps[i] as IStoreSettingsParent);
					if (storeSettingsParent != null)
					{
						return storeSettingsParent;
					}
				}
			}
			return null;
		}

		// Token: 0x060077CC RID: 30668 RVA: 0x002A3686 File Offset: 0x002A1886
		public override void Notify_ClickOutsideWindow()
		{
			base.Notify_ClickOutsideWindow();
			this.thingFilterState.quickSearch.Unfocus();
		}

		// Token: 0x0400429D RID: 17053
		private ThingFilterUI.UIState thingFilterState = new ThingFilterUI.UIState();

		// Token: 0x0400429E RID: 17054
		private static readonly Vector2 WinSize = new Vector2(300f, 480f);
	}
}
