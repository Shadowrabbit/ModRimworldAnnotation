using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014BB RID: 5307
	public abstract class SpecialThingFilterWorker_AllowGender : SpecialThingFilterWorker
	{
		// Token: 0x06007EC7 RID: 32455 RVA: 0x002CE847 File Offset: 0x002CCA47
		protected SpecialThingFilterWorker_AllowGender(Gender targetGender)
		{
			this.targetGender = targetGender;
		}

		// Token: 0x06007EC8 RID: 32456 RVA: 0x002CE858 File Offset: 0x002CCA58
		public override bool Matches(Thing t)
		{
			Pawn pawn = t as Pawn;
			return pawn != null && pawn.gender == this.targetGender;
		}

		// Token: 0x06007EC9 RID: 32457 RVA: 0x002CE87F File Offset: 0x002CCA7F
		public override bool CanEverMatch(ThingDef def)
		{
			return def.category == ThingCategory.Pawn;
		}

		// Token: 0x04004EC4 RID: 20164
		private Gender targetGender;
	}
}
