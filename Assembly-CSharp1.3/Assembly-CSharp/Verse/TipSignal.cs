using System;

namespace Verse
{
	// Token: 0x02000426 RID: 1062
	public struct TipSignal
	{
		// Token: 0x06001FEB RID: 8171 RVA: 0x000C5B66 File Offset: 0x000C3D66
		public TipSignal(string text, int uniqueId)
		{
			this.text = text;
			this.textGetter = null;
			this.uniqueId = uniqueId;
			this.priority = TooltipPriority.Default;
			this.delay = 0.45f;
		}

		// Token: 0x06001FEC RID: 8172 RVA: 0x000C5B8F File Offset: 0x000C3D8F
		public TipSignal(string text, int uniqueId, TooltipPriority priority)
		{
			this.text = text;
			this.textGetter = null;
			this.uniqueId = uniqueId;
			this.priority = priority;
			this.delay = 0.45f;
		}

		// Token: 0x06001FED RID: 8173 RVA: 0x000C5BB8 File Offset: 0x000C3DB8
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

		// Token: 0x06001FEE RID: 8174 RVA: 0x000C5BF0 File Offset: 0x000C3DF0
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

		// Token: 0x06001FEF RID: 8175 RVA: 0x000C5C24 File Offset: 0x000C3E24
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

		// Token: 0x06001FF0 RID: 8176 RVA: 0x000C5C7E File Offset: 0x000C3E7E
		public TipSignal(Func<string> textGetter, int uniqueId)
		{
			this.text = "";
			this.textGetter = textGetter;
			this.uniqueId = uniqueId;
			this.priority = TooltipPriority.Default;
			this.delay = 0.45f;
		}

		// Token: 0x06001FF1 RID: 8177 RVA: 0x000C5CAB File Offset: 0x000C3EAB
		public TipSignal(Func<string> textGetter, int uniqueId, TooltipPriority priority)
		{
			this.text = "";
			this.textGetter = textGetter;
			this.uniqueId = uniqueId;
			this.priority = priority;
			this.delay = 0.45f;
		}

		// Token: 0x06001FF2 RID: 8178 RVA: 0x000C5CD8 File Offset: 0x000C3ED8
		public TipSignal(TipSignal cloneSource)
		{
			this.text = cloneSource.text;
			this.textGetter = null;
			this.priority = cloneSource.priority;
			this.uniqueId = cloneSource.uniqueId;
			this.delay = 0.45f;
		}

		// Token: 0x06001FF3 RID: 8179 RVA: 0x000C5D10 File Offset: 0x000C3F10
		public static implicit operator TipSignal(string str)
		{
			return new TipSignal(str);
		}

		// Token: 0x06001FF4 RID: 8180 RVA: 0x000C5D18 File Offset: 0x000C3F18
		public static implicit operator TipSignal(TaggedString str)
		{
			return new TipSignal(str);
		}

		// Token: 0x06001FF5 RID: 8181 RVA: 0x000C5D20 File Offset: 0x000C3F20
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

		// Token: 0x0400135C RID: 4956
		public const float DefaultDelay = 0.45f;

		// Token: 0x0400135D RID: 4957
		public string text;

		// Token: 0x0400135E RID: 4958
		public Func<string> textGetter;

		// Token: 0x0400135F RID: 4959
		public int uniqueId;

		// Token: 0x04001360 RID: 4960
		public TooltipPriority priority;

		// Token: 0x04001361 RID: 4961
		public float delay;
	}
}
