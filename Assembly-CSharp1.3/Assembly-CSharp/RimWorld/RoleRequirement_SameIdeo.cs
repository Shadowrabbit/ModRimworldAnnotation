using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FDA RID: 4058
	public class RoleRequirement_SameIdeo : RoleRequirement
	{
		// Token: 0x06005F8B RID: 24459 RVA: 0x0020AD52 File Offset: 0x00208F52
		public override string GetLabel(Precept_Role role)
		{
			return this.labelKey.Translate(Find.ActiveLanguageWorker.WithIndefiniteArticle(role.ideo.memberName, Gender.None, false, false));
		}

		// Token: 0x06005F8C RID: 24460 RVA: 0x0020AD81 File Offset: 0x00208F81
		public override bool Met(Pawn p, Precept_Role role)
		{
			return p.Ideo == role.ideo;
		}
	}
}
