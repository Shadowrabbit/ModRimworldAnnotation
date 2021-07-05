using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200160A RID: 5642
	public class SymbolStack
	{
		// Token: 0x17001607 RID: 5639
		// (get) Token: 0x0600840D RID: 33805 RVA: 0x002F50E8 File Offset: 0x002F32E8
		public bool Empty
		{
			get
			{
				return this.stack.Count == 0;
			}
		}

		// Token: 0x17001608 RID: 5640
		// (get) Token: 0x0600840E RID: 33806 RVA: 0x002F50F8 File Offset: 0x002F32F8
		public int Count
		{
			get
			{
				return this.stack.Count;
			}
		}

		// Token: 0x0600840F RID: 33807 RVA: 0x002F5108 File Offset: 0x002F3308
		public void Push(string symbol, ResolveParams resolveParams, string customNameForPath = null)
		{
			string text = BaseGen.CurrentSymbolPath;
			if (!text.NullOrEmpty())
			{
				text += "_";
			}
			text += (customNameForPath ?? symbol);
			SymbolStack.Element item = default(SymbolStack.Element);
			item.symbol = symbol;
			item.resolveParams = resolveParams;
			item.symbolPath = text;
			this.stack.Push(item);
		}

		// Token: 0x06008410 RID: 33808 RVA: 0x002F5168 File Offset: 0x002F3368
		public void Push(string symbol, CellRect rect, string customNameForPath = null)
		{
			this.Push(symbol, new ResolveParams
			{
				rect = rect
			}, customNameForPath);
		}

		// Token: 0x06008411 RID: 33809 RVA: 0x002F5190 File Offset: 0x002F3390
		public void PushMany(ResolveParams resolveParams, params string[] symbols)
		{
			for (int i = 0; i < symbols.Length; i++)
			{
				this.Push(symbols[i], resolveParams, null);
			}
		}

		// Token: 0x06008412 RID: 33810 RVA: 0x002F51B8 File Offset: 0x002F33B8
		public void PushMany(CellRect rect, params string[] symbols)
		{
			for (int i = 0; i < symbols.Length; i++)
			{
				this.Push(symbols[i], rect, null);
			}
		}

		// Token: 0x06008413 RID: 33811 RVA: 0x002F51DE File Offset: 0x002F33DE
		public SymbolStack.Element Pop()
		{
			return this.stack.Pop();
		}

		// Token: 0x06008414 RID: 33812 RVA: 0x002F51EB File Offset: 0x002F33EB
		public void Clear()
		{
			this.stack.Clear();
		}

		// Token: 0x04005256 RID: 21078
		private Stack<SymbolStack.Element> stack = new Stack<SymbolStack.Element>();

		// Token: 0x020028F3 RID: 10483
		public struct Element
		{
			// Token: 0x04009A76 RID: 39542
			public string symbol;

			// Token: 0x04009A77 RID: 39543
			public ResolveParams resolveParams;

			// Token: 0x04009A78 RID: 39544
			public string symbolPath;
		}
	}
}
