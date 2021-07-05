using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000450 RID: 1104
	public class DiaNode
	{
		// Token: 0x06002181 RID: 8577 RVA: 0x000D1597 File Offset: 0x000CF797
		public DiaNode(TaggedString text)
		{
			this.text = text;
		}

		// Token: 0x06002182 RID: 8578 RVA: 0x000D15B4 File Offset: 0x000CF7B4
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

		// Token: 0x06002183 RID: 8579 RVA: 0x0000313F File Offset: 0x0000133F
		public void PreClose()
		{
		}

		// Token: 0x040014D7 RID: 5335
		public TaggedString text;

		// Token: 0x040014D8 RID: 5336
		public List<DiaOption> options = new List<DiaOption>();

		// Token: 0x040014D9 RID: 5337
		protected DiaNodeMold def;
	}
}
