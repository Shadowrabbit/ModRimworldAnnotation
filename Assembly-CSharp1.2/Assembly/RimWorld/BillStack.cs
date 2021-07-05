using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B1A RID: 2842
	public class BillStack : IExposable
	{
		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x06004281 RID: 17025 RVA: 0x000317AE File Offset: 0x0002F9AE
		public List<Bill> Bills
		{
			get
			{
				return this.bills;
			}
		}

		// Token: 0x06004282 RID: 17026 RVA: 0x000317B6 File Offset: 0x0002F9B6
		public IEnumerator<Bill> GetEnumerator()
		{
			return this.bills.GetEnumerator();
		}

		// Token: 0x17000A57 RID: 2647
		public Bill this[int index]
		{
			get
			{
				return this.bills[index];
			}
		}

		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x06004284 RID: 17028 RVA: 0x000317D6 File Offset: 0x0002F9D6
		public int Count
		{
			get
			{
				return this.bills.Count;
			}
		}

		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x06004285 RID: 17029 RVA: 0x0018A628 File Offset: 0x00188828
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

		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x06004286 RID: 17030 RVA: 0x0018A668 File Offset: 0x00188868
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

		// Token: 0x06004287 RID: 17031 RVA: 0x000317E3 File Offset: 0x0002F9E3
		public BillStack(IBillGiver giver)
		{
			this.billGiver = giver;
		}

		// Token: 0x06004288 RID: 17032 RVA: 0x000317FD File Offset: 0x0002F9FD
		public void AddBill(Bill bill)
		{
			bill.billStack = this;
			this.bills.Add(bill);
		}

		// Token: 0x06004289 RID: 17033 RVA: 0x00031812 File Offset: 0x0002FA12
		public void Delete(Bill bill)
		{
			bill.deleted = true;
			this.bills.Remove(bill);
		}

		// Token: 0x0600428A RID: 17034 RVA: 0x00031828 File Offset: 0x0002FA28
		public void Clear()
		{
			this.bills.Clear();
		}

		// Token: 0x0600428B RID: 17035 RVA: 0x0018A69C File Offset: 0x0018889C
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

		// Token: 0x0600428C RID: 17036 RVA: 0x0018A6D8 File Offset: 0x001888D8
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

		// Token: 0x0600428D RID: 17037 RVA: 0x00031835 File Offset: 0x0002FA35
		public int IndexOf(Bill bill)
		{
			return this.bills.IndexOf(bill);
		}

		// Token: 0x0600428E RID: 17038 RVA: 0x0018A728 File Offset: 0x00188928
		public void ExposeData()
		{
			Scribe_Collections.Look<Bill>(ref this.bills, "bills", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				if (this.bills.RemoveAll((Bill x) => x == null) != 0)
				{
					Log.Error("Some bills were null after loading.", false);
				}
				if (this.bills.RemoveAll((Bill x) => x.recipe == null) != 0)
				{
					Log.Error("Some bills had null recipe after loading.", false);
				}
				for (int i = 0; i < this.bills.Count; i++)
				{
					this.bills[i].billStack = this;
				}
			}
		}

		// Token: 0x0600428F RID: 17039 RVA: 0x0018A7EC File Offset: 0x001889EC
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

		// Token: 0x04002D9B RID: 11675
		[Unsaved(false)]
		public IBillGiver billGiver;

		// Token: 0x04002D9C RID: 11676
		private List<Bill> bills = new List<Bill>();

		// Token: 0x04002D9D RID: 11677
		public const int MaxCount = 15;

		// Token: 0x04002D9E RID: 11678
		private const float TopAreaHeight = 35f;

		// Token: 0x04002D9F RID: 11679
		private const float BillInterfaceSpacing = 6f;

		// Token: 0x04002DA0 RID: 11680
		private const float ExtraViewHeight = 60f;
	}
}
