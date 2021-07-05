using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200007E RID: 126
	public class Editable
	{
		// Token: 0x060004B2 RID: 1202 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ResolveReferences()
		{
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostLoad()
		{
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0001962C File Offset: 0x0001782C
		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}
	}
}
