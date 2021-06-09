using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020021D9 RID: 8665
	public class WITab_Caravan_Items : WITab
	{
		// Token: 0x0600B984 RID: 47492 RVA: 0x00078194 File Offset: 0x00076394
		public WITab_Caravan_Items()
		{
			this.labelKey = "TabCaravanItems";
		}

		// Token: 0x0600B985 RID: 47493 RVA: 0x00356264 File Offset: 0x00354464
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

		// Token: 0x0600B986 RID: 47494 RVA: 0x000781B2 File Offset: 0x000763B2
		protected override void UpdateSize()
		{
			base.UpdateSize();
			this.CheckCacheItems();
			this.size = CaravanItemsTabUtility.GetSize(this.cachedItems, this.PaneTopY, true);
		}

		// Token: 0x0600B987 RID: 47495 RVA: 0x00356390 File Offset: 0x00354590
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

		// Token: 0x0600B988 RID: 47496 RVA: 0x003563F4 File Offset: 0x003545F4
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

		// Token: 0x0600B989 RID: 47497 RVA: 0x000781D8 File Offset: 0x000763D8
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

		// Token: 0x04007EC0 RID: 32448
		private Vector2 scrollPosition;

		// Token: 0x04007EC1 RID: 32449
		private float scrollViewHeight;

		// Token: 0x04007EC2 RID: 32450
		private TransferableSorterDef sorter1;

		// Token: 0x04007EC3 RID: 32451
		private TransferableSorterDef sorter2;

		// Token: 0x04007EC4 RID: 32452
		private List<TransferableImmutable> cachedItems = new List<TransferableImmutable>();

		// Token: 0x04007EC5 RID: 32453
		private int cachedItemsHash;

		// Token: 0x04007EC6 RID: 32454
		private int cachedItemsCount;

		// Token: 0x04007EC7 RID: 32455
		private const float SortersSpace = 25f;

		// Token: 0x04007EC8 RID: 32456
		private const float AssignDrugPoliciesButtonHeight = 27f;
	}
}
