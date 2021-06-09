using System;

namespace Verse
{
	// Token: 0x0200075D RID: 1885
	public struct TipSignal
	{
		// Token: 0x06002F84 RID: 12164 RVA: 0x000253D0 File Offset: 0x000235D0
		public TipSignal(string text, int uniqueId)
		{
			this.text = text;
			this.textGetter = null;
			this.uniqueId = uniqueId;
			this.priority = TooltipPriority.Default;
			this.delay = 0.45f;
		}

		// Token: 0x06002F85 RID: 12165 RVA: 0x000253F9 File Offset: 0x000235F9
		public TipSignal(string text, int uniqueId, TooltipPriority priority)
		{
			this.text = text;
			this.textGetter = null;
			this.uniqueId = uniqueId;
			this.priority = priority;
			this.delay = 0.45f;
		}

		// Token: 0x06002F86 RID: 12166 RVA: 0x00025422 File Offset: 0x00023622
		public TipSignal(string text)
		{
			if (text == null)
			{
				text = "";
			}
			this.text = text;
			this.textGetter = null;
			this.uniqueId = text.GetHashCode();
			this.priority = TooltipPriority.Default;
			this.delay = 0.45f;
		}

		// Token: 0x06002F87 RID: 12167 RVA: 0x0002545A File Offset: 0x0002365A
		public TipSignal(string text, float delay)
		{
			if (text == null)
			{
				text = "";
			}
			this.text = text;
			this.textGetter = null;
			this.uniqueId = text.GetHashCode();
			this.priority = TooltipPriority.Default;
			this.delay = delay;
		}

		// Token: 0x06002F88 RID: 12168 RVA: 0x0013B680 File Offset: 0x00139880
		public TipSignal(TaggedString text)
		{
			if (text == null)
			{
				text = "";
			}
			this.text = text.Resolve();
			this.textGetter = null;
			this.uniqueId = text.GetHashCode();
			this.priority = TooltipPriority.Default;
			this.delay = 0.45f;
		}

		// Token: 0x06002F89 RID: 12169 RVA: 0x0002548E File Offset: 0x0002368E
		public TipSignal(Func<string> textGetter, int uniqueId)
		{
			this.text = "";
			this.textGetter = textGetter;
			this.uniqueId = uniqueId;
			this.priority = TooltipPriority.Default;
			this.delay = 0.45f;
		}

		// Token: 0x06002F8A RID: 12170 RVA: 0x000254BB File Offset: 0x000236BB
		public TipSignal(Func<string> textGetter, int uniqueId, TooltipPriority priority)
		{
			this.text = "";
			this.textGetter = textGetter;
			this.uniqueId = uniqueId;
			this.priority = priority;
			this.delay = 0.45f;
		}

		// Token: 0x06002F8B RID: 12171 RVA: 0x000254E8 File Offset: 0x000236E8
		public TipSignal(TipSignal cloneSource)
		{
			this.text = cloneSource.text;
			this.textGetter = null;
			this.priority = cloneSource.priority;
			this.uniqueId = cloneSource.uniqueId;
			this.delay = 0.45f;
		}

		// Token: 0x06002F8C RID: 12172 RVA: 0x00025520 File Offset: 0x00023720
		public static implicit operator TipSignal(string str)
		{
			return new TipSignal(str);
		}

		// Token: 0x06002F8D RID: 12173 RVA: 0x00025528 File Offset: 0x00023728
		public static implicit operator TipSignal(TaggedString str)
		{
			return new TipSignal(str);
		}

		// Token: 0x06002F8E RID: 12174 RVA: 0x00025530 File Offset: 0x00023730
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Tip(",
				this.text,
				", ",
				this.uniqueId,
				")"
			});
		}

		// Token: 0x04002039 RID: 8249
		public const float DefaultDelay = 0.45f;

		// Token: 0x0400203A RID: 8250
		public string text;

		// Token: 0x0400203B RID: 8251
		public Func<string> textGetter;

		// Token: 0x0400203C RID: 8252
		public int uniqueId;

		// Token: 0x0400203D RID: 8253
		public TooltipPriority priority;

		// Token: 0x0400203E RID: 8254
		public float delay;
	}
}
