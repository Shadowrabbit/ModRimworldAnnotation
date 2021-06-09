using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006CD RID: 1741
	public class Dialog_DebugOptionListLister : Dialog_DebugOptionLister
	{
		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x06002CC6 RID: 11462 RVA: 0x001305E8 File Offset: 0x0012E7E8
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

		// Token: 0x06002CC7 RID: 11463 RVA: 0x000237A5 File Offset: 0x000219A5
		public Dialog_DebugOptionListLister(IEnumerable<DebugMenuOption> options)
		{
			this.options = options.ToList<DebugMenuOption>();
		}

		// Token: 0x06002CC8 RID: 11464 RVA: 0x00130668 File Offset: 0x0012E868
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
					base.DebugAction_NewTmp(debugMenuOption.label, debugMenuOption.method, highlight);
				}
				if (debugMenuOption.mode == DebugMenuOptionMode.Tool)
				{
					base.DebugToolMap_NewTmp(debugMenuOption.label, debugMenuOption.method, highlight);
				}
			}
		}

		// Token: 0x06002CC9 RID: 11465 RVA: 0x001306E4 File Offset: 0x0012E8E4
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

		// Token: 0x06002CCA RID: 11466 RVA: 0x00130744 File Offset: 0x0012E944
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

		// Token: 0x06002CCB RID: 11467 RVA: 0x001307E0 File Offset: 0x0012E9E0
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

		// Token: 0x04001E57 RID: 7767
		protected List<DebugMenuOption> options;
	}
}
