using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001810 RID: 6160
	public class WITab_Caravan_Items : WITab
	{
		// Token: 0x06009043 RID: 36931 RVA: 0x0033B9F2 File Offset: 0x00339BF2
		public WITab_Caravan_Items()
		{
			this.labelKey = "TabCaravanItems";
		}

		// Token: 0x06009044 RID: 36932 RVA: 0x0033BA10 File Offset: 0x00339C10
		protected override void FillTab()
		{
			this.CheckCreateSorters();
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y);
			if (Widgets.ButtonText(new Rect(rect.x + 10f, rect.y + 10f, 200f, 27f), "AssignDrugPolicies".Translate(), true, true, true))
			{
				Find.WindowStack.Add(new Dialog_AssignCaravanDrugPolicies(base.SelCaravan));
			}
			rect.yMin += 37f;
			GUI.BeginGroup(rect.ContractedBy(10f));
			TransferableUIUtility.DoTransferableSorters(this.sorter1, this.sorter2, delegate(TransferableSorterDef x)
			{
				this.sorter1 = x;
				this.CacheItems();
			}, delegate(TransferableSorterDef x)
			{
				this.sorter2 = x;
				this.CacheItems();
			});
			GUI.EndGroup();
			rect.yMin += 25f;
			GUI.BeginGroup(rect);
			this.CheckCacheItems();
			CaravanItemsTabUtility.DoRows(rect.size, this.cachedItems, base.SelCaravan, ref this.scrollPosition, ref this.scrollViewHeight);
			GUI.EndGroup();
		}

		// Token: 0x06009045 RID: 36933 RVA: 0x0033BB3A File Offset: 0x00339D3A
		protected override void UpdateSize()
		{
			base.UpdateSize();
			this.CheckCacheItems();
			this.size = CaravanItemsTabUtility.GetSize(this.cachedItems, this.PaneTopY, true);
		}

		// Token: 0x06009046 RID: 36934 RVA: 0x0033BB60 File Offset: 0x00339D60
		private void CheckCacheItems()
		{
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(base.SelCaravan);
			if (list.Count != this.cachedItemsCount)
			{
				this.CacheItems();
				return;
			}
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				num = Gen.HashCombineInt(num, list[i].GetHashCode());
			}
			if (num != this.cachedItemsHash)
			{
				this.CacheItems();
			}
		}

		// Token: 0x06009047 RID: 36935 RVA: 0x0033BBC4 File Offset: 0x00339DC4
		private void CacheItems()
		{
			this.CheckCreateSorters();
			this.cachedItems.Clear();
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(base.SelCaravan);
			int seed = 0;
			for (int i = 0; i < list.Count; i++)
			{
				TransferableImmutable transferableImmutable = TransferableUtility.TransferableMatching<TransferableImmutable>(list[i], this.cachedItems, TransferAsOneMode.Normal);
				if (transferableImmutable == null)
				{
					transferableImmutable = new TransferableImmutable();
					this.cachedItems.Add(transferableImmutable);
				}
				transferableImmutable.things.Add(list[i]);
				seed = Gen.HashCombineInt(seed, list[i].GetHashCode());
			}
			this.cachedItems = this.cachedItems.OrderBy((TransferableImmutable tr) => tr, this.sorter1.Comparer).ThenBy((TransferableImmutable tr) => tr, this.sorter2.Comparer).ThenBy((TransferableImmutable tr) => TransferableUIUtility.DefaultListOrderPriority(tr)).ToList<TransferableImmutable>();
			this.cachedItemsCount = list.Count;
			this.cachedItemsHash = seed;
		}

		// Token: 0x06009048 RID: 36936 RVA: 0x0033BCF5 File Offset: 0x00339EF5
		private void CheckCreateSorters()
		{
			if (this.sorter1 == null)
			{
				this.sorter1 = TransferableSorterDefOf.Category;
			}
			if (this.sorter2 == null)
			{
				this.sorter2 = TransferableSorterDefOf.MarketValue;
			}
		}

		// Token: 0x04005ABC RID: 23228
		private Vector2 scrollPosition;

		// Token: 0x04005ABD RID: 23229
		private float scrollViewHeight;

		// Token: 0x04005ABE RID: 23230
		private TransferableSorterDef sorter1;

		// Token: 0x04005ABF RID: 23231
		private TransferableSorterDef sorter2;

		// Token: 0x04005AC0 RID: 23232
		private List<TransferableImmutable> cachedItems = new List<TransferableImmutable>();

		// Token: 0x04005AC1 RID: 23233
		private int cachedItemsHash;

		// Token: 0x04005AC2 RID: 23234
		private int cachedItemsCount;

		// Token: 0x04005AC3 RID: 23235
		private const float SortersSpace = 25f;

		// Token: 0x04005AC4 RID: 23236
		private const float AssignDrugPoliciesButtonHeight = 27f;
	}
}
