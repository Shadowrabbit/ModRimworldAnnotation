using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000D0 RID: 208
	public class Editable
	{
		// Token: 0x06000637 RID: 1591 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void ResolveReferences()
		{
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostLoad()
		{
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x0000B385 File Offset: 0x00009585
		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}
	}
}
