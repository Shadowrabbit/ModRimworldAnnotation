using System;
using System.Collections.Generic;
using System.Xml;

namespace Verse
{
	// Token: 0x02000370 RID: 880
	public class PatchOperationFindMod : PatchOperation
	{
		// Token: 0x06001638 RID: 5688 RVA: 0x000D5770 File Offset: 0x000D3970
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

		// Token: 0x06001639 RID: 5689 RVA: 0x00015BF5 File Offset: 0x00013DF5
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.mods.ToCommaList(false));
		}

		// Token: 0x04001108 RID: 4360
		private List<string> mods;

		// Token: 0x04001109 RID: 4361
		private PatchOperation match;

		// Token: 0x0400110A RID: 4362
		private PatchOperation nomatch;
	}
}
