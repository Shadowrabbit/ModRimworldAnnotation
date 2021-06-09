using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B1E RID: 6942
	public class ITab_Storage : ITab
	{
		// Token: 0x1700180E RID: 6158
		// (get) Token: 0x060098B4 RID: 39092 RVA: 0x002CE1F8 File Offset: 0x002CC3F8
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

		// Token: 0x1700180F RID: 6159
		// (get) Token: 0x060098B5 RID: 39093 RVA: 0x002CE230 File Offset: 0x002CC430
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

		// Token: 0x17001810 RID: 6160
		// (get) Token: 0x060098B6 RID: 39094 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected virtual bool IsPrioritySettingVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001811 RID: 6161
		// (get) Token: 0x060098B7 RID: 39095 RVA: 0x00065CC6 File Offset: 0x00063EC6
		private float TopAreaHeight
		{
			get
			{
				return (float)(this.IsPrioritySettingVisible ? 35 : 20);
			}
		}

		// Token: 0x060098B8 RID: 39096 RVA: 0x00065CD7 File Offset: 0x00063ED7
		public ITab_Storage()
		{
			this.size = ITab_Storage.WinSize;
			this.labelKey = "TabStorage";
			this.tutorTag = "Storage";
		}

		// Token: 0x060098B9 RID: 39097 RVA: 0x002CE278 File Offset: 0x002CC478
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
							}, MenuOptionPriority.Default, null, null, 0f, null, null));
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
			ThingFilterUI.DoThingFilterConfigWindow(rect2, ref this.scrollPosition, settings.filter, parentFilter, 8, null, null, false, null, null);
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

		// Token: 0x060098BA RID: 39098 RVA: 0x002CE56C File Offset: 0x002CC76C
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

		// Token: 0x040061A1 RID: 24993
		private Vector2 scrollPosition;

		// Token: 0x040061A2 RID: 24994
		private static readonly Vector2 WinSize = new Vector2(300f, 480f);
	}
}
