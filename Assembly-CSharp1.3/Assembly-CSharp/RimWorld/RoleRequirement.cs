using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FD9 RID: 4057
	public abstract class RoleRequirement
	{
		// Token: 0x06005F87 RID: 24455 RVA: 0x0020AD32 File Offset: 0x00208F32
		public virtual string GetLabel(Precept_Role role)
		{
			return this.labelKey.Translate();
		}

		// Token: 0x06005F88 RID: 24456 RVA: 0x0020AD44 File Offset: 0x00208F44
		public string GetLabelCap(Precept_Role role)
		{
			return this.GetLabel(role).CapitalizeFirst();
		}

		// Token: 0x06005F89 RID: 24457
		public abstract bool Met(Pawn p, Precept_Role role);

		// Token: 0x040036E4 RID: 14052
		[NoTranslate]
		public string labelKey;
	}
}
