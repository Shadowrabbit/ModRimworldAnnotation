using System;
using System.Collections.Generic;
using System.Xml;

namespace Verse
{
	// Token: 0x02000255 RID: 597
	public class PatchOperationFindMod : PatchOperation
	{
		// Token: 0x060010FF RID: 4351 RVA: 0x00060390 File Offset: 0x0005E590
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool flag = false;
			for (int i = 0; i < this.mods.Count; i++)
			{
				if (ModLister.HasActiveModWithName(this.mods[i]))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				if (this.match != null)
				{
					return this.match.Apply(xml);
				}
			}
			else if (this.nomatch != null)
			{
				return this.nomatch.Apply(xml);
			}
			return true;
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x000603FA File Offset: 0x0005E5FA
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.mods.ToCommaList(false, false));
		}

		// Token: 0x04000CE8 RID: 3304
		private List<string> mods;

		// Token: 0x04000CE9 RID: 3305
		private PatchOperation match;

		// Token: 0x04000CEA RID: 3306
		private PatchOperation nomatch;
	}
}
