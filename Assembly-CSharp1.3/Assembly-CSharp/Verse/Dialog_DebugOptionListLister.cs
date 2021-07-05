using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003C5 RID: 965
	public class Dialog_DebugOptionListLister : Dialog_DebugOptionLister
	{
		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06001D9D RID: 7581 RVA: 0x000B92BC File Offset: 0x000B74BC
		protected override int HighlightedIndex
		{
			get
			{
				if (base.FilterAllows(this.options[this.prioritizedHighlightedIndex].label))
				{
					return this.prioritizedHighlightedIndex;
				}
				if (this.filter.NullOrEmpty())
				{
					return 0;
				}
				for (int i = 0; i < this.options.Count; i++)
				{
					if (base.FilterAllows(this.options[i].label))
					{
						this.currentHighlightIndex = i;
						break;
					}
				}
				return this.currentHighlightIndex;
			}
		}

		// Token: 0x06001D9E RID: 7582 RVA: 0x000B933B File Offset: 0x000B753B
		public Dialog_DebugOptionListLister(IEnumerable<DebugMenuOption> options)
		{
			this.options = options.ToList<DebugMenuOption>();
		}

		// Token: 0x06001D9F RID: 7583 RVA: 0x000B9350 File Offset: 0x000B7550
		protected override void DoListingItems()
		{
			base.DoListingItems();
			int highlightedIndex = this.HighlightedIndex;
			for (int i = 0; i < this.options.Count; i++)
			{
				DebugMenuOption debugMenuOption = this.options[i];
				bool highlight = highlightedIndex == i;
				if (debugMenuOption.mode == DebugMenuOptionMode.Action)
				{
					base.DebugAction(debugMenuOption.label, debugMenuOption.method, highlight);
				}
				if (debugMenuOption.mode == DebugMenuOptionMode.Tool)
				{
					base.DebugToolMap(debugMenuOption.label, debugMenuOption.method, highlight);
				}
			}
		}

		// Token: 0x06001DA0 RID: 7584 RVA: 0x000B93CC File Offset: 0x000B75CC
		protected override void ChangeHighlightedOption()
		{
			int highlightedIndex = this.HighlightedIndex;
			for (int i = 0; i < this.options.Count; i++)
			{
				int num = (highlightedIndex + i + 1) % this.options.Count;
				if (base.FilterAllows(this.options[num].label))
				{
					this.prioritizedHighlightedIndex = num;
					return;
				}
			}
		}

		// Token: 0x06001DA1 RID: 7585 RVA: 0x000B942C File Offset: 0x000B762C
		public static void ShowSimpleDebugMenu<T>(IEnumerable<T> elements, Func<T, string> label, Action<T> chosen)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<T> enumerator = elements.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					T t = enumerator.Current;
					list.Add(new DebugMenuOption(label(t), DebugMenuOptionMode.Action, delegate()
					{
						chosen(t);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001DA2 RID: 7586 RVA: 0x000B94C8 File Offset: 0x000B76C8
		public override void OnAcceptKeyPressed()
		{
			if (GUI.GetNameOfFocusedControl() == "DebugFilter")
			{
				int highlightedIndex = this.HighlightedIndex;
				if (highlightedIndex >= 0)
				{
					this.Close(true);
					if (this.options[highlightedIndex].mode == DebugMenuOptionMode.Action)
					{
						this.options[highlightedIndex].method();
					}
					else
					{
						DebugTools.curTool = new DebugTool(this.options[highlightedIndex].label, this.options[highlightedIndex].method, null);
					}
				}
				Event.current.Use();
			}
		}

		// Token: 0x040011D2 RID: 4562
		protected List<DebugMenuOption> options;
	}
}
