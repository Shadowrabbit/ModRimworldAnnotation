using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000E2 RID: 226
	public class OrderedTakeGroupDef : Def
	{
		// Token: 0x06000636 RID: 1590 RVA: 0x0001F005 File Offset: 0x0001D205
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.max <= 0)
			{
				yield return "Max should be greather than zero.";
			}
			yield break;
			yield break;
		}

		// Token: 0x0400050A RID: 1290
		public int max = 3;
	}
}
