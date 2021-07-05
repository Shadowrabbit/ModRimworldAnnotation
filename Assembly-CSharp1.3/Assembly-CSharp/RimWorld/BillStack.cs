using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006B6 RID: 1718
	public class BillStack : IExposable
	{
		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x06002FAE RID: 12206 RVA: 0x0011A1D4 File Offset: 0x001183D4
		public List<Bill> Bills
		{
			get
			{
				return this.bills;
			}
		}

		// Token: 0x06002FAF RID: 12207 RVA: 0x0011A1DC File Offset: 0x001183DC
		public IEnumerator<Bill> GetEnumerator()
		{
			return this.bills.GetEnumerator();
		}

		// Token: 0x170008EE RID: 2286
		public Bill this[int index]
		{
			get
			{
				return this.bills[index];
			}
		}

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06002FB1 RID: 12209 RVA: 0x0011A1FC File Offset: 0x001183FC
		public int Count
		{
			get
			{
				return this.bills.Count;
			}
		}

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x06002FB2 RID: 12210 RVA: 0x0011A20C File Offset: 0x0011840C
		public Bill FirstShouldDoNow
		{
			get
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this.bills[i].ShouldDoNow())
					{
						return this.bills[i];
					}
				}
				return null;
			}
		}

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06002FB3 RID: 12211 RVA: 0x0011A24C File Offset: 0x0011844C
		public bool AnyShouldDoNow
		{
			get
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this.bills[i].ShouldDoNow())
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06002FB4 RID: 12212 RVA: 0x0011A280 File Offset: 0x00118480
		public BillStack(IBillGiver giver)
		{
			this.billGiver = giver;
		}

		// Token: 0x06002FB5 RID: 12213 RVA: 0x0011A29A File Offset: 0x0011849A
		public void AddBill(Bill bill)
		{
			bill.billStack = this;
			this.bills.Add(bill);
		}

		// Token: 0x06002FB6 RID: 12214 RVA: 0x0011A2AF File Offset: 0x001184AF
		public void Delete(Bill bill)
		{
			bill.deleted = true;
			this.bills.Remove(bill);
		}

		// Token: 0x06002FB7 RID: 12215 RVA: 0x0011A2C5 File Offset: 0x001184C5
		public void Clear()
		{
			this.bills.Clear();
		}

		// Token: 0x06002FB8 RID: 12216 RVA: 0x0011A2D4 File Offset: 0x001184D4
		public void Reorder(Bill bill, int offset)
		{
			int num = this.bills.IndexOf(bill);
			num += offset;
			if (num >= 0)
			{
				this.bills.Remove(bill);
				this.bills.Insert(num, bill);
			}
		}

		// Token: 0x06002FB9 RID: 12217 RVA: 0x0011A310 File Offset: 0x00118510
		public void RemoveIncompletableBills()
		{
			for (int i = this.bills.Count - 1; i >= 0; i--)
			{
				if (!this.bills[i].CompletableEver)
				{
					this.bills.Remove(this.bills[i]);
				}
			}
		}

		// Token: 0x06002FBA RID: 12218 RVA: 0x0011A360 File Offset: 0x00118560
		public int IndexOf(Bill bill)
		{
			return this.bills.IndexOf(bill);
		}

		// Token: 0x06002FBB RID: 12219 RVA: 0x0011A370 File Offset: 0x00118570
		public void ExposeData()
		{
			Scribe_Collections.Look<Bill>(ref this.bills, "bills", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				if (this.bills.RemoveAll((Bill x) => x == null) != 0)
				{
					Log.Error("Some bills were null after loading.");
				}
				if (this.bills.RemoveAll((Bill x) => x.recipe == null) != 0)
				{
					Log.Error("Some bills had null recipe after loading.");
				}
				for (int i = 0; i < this.bills.Count; i++)
				{
					this.bills[i].billStack = this;
				}
			}
		}

		// Token: 0x06002FBC RID: 12220 RVA: 0x0011A434 File Offset: 0x00118634
		public Bill DoListing(Rect rect, Func<List<FloatMenuOption>> recipeOptionsMaker, ref Vector2 scrollPosition, ref float viewHeight)
		{
			Bill result = null;
			GUI.BeginGroup(rect);
			Text.Font = GameFont.Small;
			if (this.Count < 15)
			{
				Rect rect2 = new Rect(0f, 0f, 150f, 29f);
				if (Widgets.ButtonText(rect2, "AddBill".Translate(), true, true, true))
				{
					Find.WindowStack.Add(new FloatMenu(recipeOptionsMaker()));
				}
				UIHighlighter.HighlightOpportunity(rect2, "AddBill");
			}
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
			Rect outRect = new Rect(0f, 35f, rect.width, rect.height - 35f);
			Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, viewHeight);
			Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect, true);
			float num = 0f;
			for (int i = 0; i < this.Count; i++)
			{
				Bill bill = this.bills[i];
				Rect rect3 = bill.DoInterface(0f, num, viewRect.width, i);
				if (!bill.DeletedOrDereferenced && Mouse.IsOver(rect3))
				{
					result = bill;
				}
				num += rect3.height + 6f;
			}
			if (Event.current.type == EventType.Layout)
			{
				viewHeight = num + 60f;
			}
			Widgets.EndScrollView();
			GUI.EndGroup();
			return result;
		}

		// Token: 0x04001D11 RID: 7441
		[Unsaved(false)]
		public IBillGiver billGiver;

		// Token: 0x04001D12 RID: 7442
		private List<Bill> bills = new List<Bill>();

		// Token: 0x04001D13 RID: 7443
		public const int MaxCount = 15;

		// Token: 0x04001D14 RID: 7444
		private const float TopAreaHeight = 35f;

		// Token: 0x04001D15 RID: 7445
		private const float BillInterfaceSpacing = 6f;

		// Token: 0x04001D16 RID: 7446
		private const float ExtraViewHeight = 60f;
	}
}
