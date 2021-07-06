using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020007A4 RID: 1956
	public class DiaNode
	{
		// Token: 0x06003149 RID: 12617 RVA: 0x00026DDA File Offset: 0x00024FDA
		public DiaNode(TaggedString text)
		{
			this.text = text;
		}

		// Token: 0x0600314A RID: 12618 RVA: 0x001454B4 File Offset: 0x001436B4
		public DiaNode(DiaNodeMold newDef)
		{
			this.def = newDef;
			this.def.used = true;
			this.text = this.def.texts.RandomElement<string>();
			if (this.def.optionList.Count > 0)
			{
				using (List<DiaOptionMold>.Enumerator enumerator = this.def.optionList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DiaOptionMold diaOptionMold = enumerator.Current;
						this.options.Add(new DiaOption(diaOptionMold));
					}
					return;
				}
			}
			this.options.Add(new DiaOption("OK".Translate()));
		}

		// Token: 0x0600314B RID: 12619 RVA: 0x00006A05 File Offset: 0x00004C05
		public void PreClose()
		{
		}

		// Token: 0x04002201 RID: 8705
		public TaggedString text;

		// Token: 0x04002202 RID: 8706
		public List<DiaOption> options = new List<DiaOption>();

		// Token: 0x04002203 RID: 8707
		protected DiaNodeMold def;
	}
}
