using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FCC RID: 4044
	public class IdeoRoleInstance : IExposable
	{
		// Token: 0x06005F29 RID: 24361 RVA: 0x00208C4C File Offset: 0x00206E4C
		public IdeoRoleInstance(Precept_Role sourceRole)
		{
			this.sourceRole = sourceRole;
		}

		// Token: 0x06005F2A RID: 24362 RVA: 0x00208C5C File Offset: 0x00206E5C
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Collections.Look<Ability>(ref this.abilities, "abilities", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.abilities != null)
			{
				foreach (Ability ability in this.abilities)
				{
					ability.pawn = this.pawn;
					ability.verb.caster = this.pawn;
					ability.sourcePrecept = this.sourceRole;
				}
			}
		}

		// Token: 0x040036D1 RID: 14033
		public Precept sourceRole;

		// Token: 0x040036D2 RID: 14034
		public Pawn pawn;

		// Token: 0x040036D3 RID: 14035
		public List<Ability> abilities;
	}
}
