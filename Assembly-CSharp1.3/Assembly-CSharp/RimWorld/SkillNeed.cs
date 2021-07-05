using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A31 RID: 2609
	public class SkillNeed
	{
		// Token: 0x06003F47 RID: 16199 RVA: 0x0002974C File Offset: 0x0002794C
		public virtual float ValueFor(Pawn pawn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06003F48 RID: 16200 RVA: 0x001589FC File Offset: 0x00156BFC
		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}

		// Token: 0x040022D3 RID: 8915
		public SkillDef skill;
	}
}
