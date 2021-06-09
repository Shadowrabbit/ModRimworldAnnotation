using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001EB3 RID: 7859
	public class SymbolStack
	{
		// Token: 0x17001970 RID: 6512
		// (get) Token: 0x0600A8B8 RID: 43192 RVA: 0x0006F085 File Offset: 0x0006D285
		public bool Empty
		{
			get
			{
				return this.stack.Count == 0;
			}
		}

		// Token: 0x17001971 RID: 6513
		// (get) Token: 0x0600A8B9 RID: 43193 RVA: 0x0006F095 File Offset: 0x0006D295
		public int Count
		{
			get
			{
				return this.stack.Count;
			}
		}

		// Token: 0x0600A8BA RID: 43194 RVA: 0x00313388 File Offset: 0x00311588
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

		// Token: 0x0600A8BB RID: 43195 RVA: 0x003133E8 File Offset: 0x003115E8
		public void Push(string symbol, CellRect rect, string customNameForPath = null)
		{
			this.Push(symbol, new ResolveParams
			{
				rect = rect
			}, customNameForPath);
		}

		// Token: 0x0600A8BC RID: 43196 RVA: 0x00313410 File Offset: 0x00311610
		public void PushMany(ResolveParams resolveParams, params string[] symbols)
		{
			for (int i = 0; i < symbols.Length; i++)
			{
				this.Push(symbols[i], resolveParams, null);
			}
		}

		// Token: 0x0600A8BD RID: 43197 RVA: 0x00313438 File Offset: 0x00311638
		public void PushMany(CellRect rect, params string[] symbols)
		{
			for (int i = 0; i < symbols.Length; i++)
			{
				this.Push(symbols[i], rect, null);
			}
		}

		// Token: 0x0600A8BE RID: 43198 RVA: 0x0006F0A2 File Offset: 0x0006D2A2
		public SymbolStack.Element Pop()
		{
			return this.stack.Pop();
		}

		// Token: 0x0600A8BF RID: 43199 RVA: 0x0006F0AF File Offset: 0x0006D2AF
		public void Clear()
		{
			this.stack.Clear();
		}

		// Token: 0x0400726A RID: 29290
		private Stack<SymbolStack.Element> stack = new Stack<SymbolStack.Element>();

		// Token: 0x02001EB4 RID: 7860
		public struct Element
		{
			// Token: 0x0400726B RID: 29291
			public string symbol;

			// Token: 0x0400726C RID: 29292
			public ResolveParams resolveParams;

			// Token: 0x0400726D RID: 29293
			public string symbolPath;
		}
	}
}
