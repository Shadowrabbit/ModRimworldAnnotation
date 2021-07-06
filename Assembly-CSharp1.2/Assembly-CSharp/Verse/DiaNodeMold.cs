using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020007A6 RID: 1958
	public class DiaNodeMold
	{
		// Token: 0x0600314C RID: 12620 RVA: 0x00145588 File Offset: 0x00143788
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

		// Token: 0x04002209 RID: 8713
		public string name = "Unnamed";

		// Token: 0x0400220A RID: 8714
		public bool unique;

		// Token: 0x0400220B RID: 8715
		public List<string> texts = new List<string>();

		// Token: 0x0400220C RID: 8716
		public List<DiaOptionMold> optionList = new List<DiaOptionMold>();

		// Token: 0x0400220D RID: 8717
		[Unsaved(false)]
		public bool isRoot = true;

		// Token: 0x0400220E RID: 8718
		[Unsaved(false)]
		public bool used;

		// Token: 0x0400220F RID: 8719
		[Unsaved(false)]
		public DiaNodeType nodeType;
	}
}
