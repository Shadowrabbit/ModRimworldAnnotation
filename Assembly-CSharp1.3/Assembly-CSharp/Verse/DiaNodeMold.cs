using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000452 RID: 1106
	public class DiaNodeMold
	{
		// Token: 0x06002184 RID: 8580 RVA: 0x000D1688 File Offset: 0x000CF888
		public void PostLoad()
		{
			int num = 0;
			foreach (string text in this.texts.ListFullCopy<string>())
			{
				this.texts[num] = text.Replace("\\n", Environment.NewLine);
				num++;
			}
			foreach (DiaOptionMold diaOptionMold in this.optionList)
			{
				foreach (DiaNodeMold diaNodeMold in diaOptionMold.ChildNodes)
				{
					diaNodeMold.PostLoad();
				}
			}
		}

		// Token: 0x040014DF RID: 5343
		public string name = "Unnamed";

		// Token: 0x040014E0 RID: 5344
		public bool unique;

		// Token: 0x040014E1 RID: 5345
		public List<string> texts = new List<string>();

		// Token: 0x040014E2 RID: 5346
		public List<DiaOptionMold> optionList = new List<DiaOptionMold>();

		// Token: 0x040014E3 RID: 5347
		[Unsaved(false)]
		public bool isRoot = true;

		// Token: 0x040014E4 RID: 5348
		[Unsaved(false)]
		public bool used;

		// Token: 0x040014E5 RID: 5349
		[Unsaved(false)]
		public DiaNodeType nodeType;
	}
}
