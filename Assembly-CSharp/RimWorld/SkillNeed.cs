using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F4B RID: 3915
	public class SkillNeed
	{
		// Token: 0x0600561F RID: 22047 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public virtual float ValueFor(Pawn pawn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005620 RID: 22048 RVA: 0x0003BC38 File Offset: 0x00039E38
		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}

		// Token: 0x04003793 RID: 14227
		public SkillDef skill;
	}
}
