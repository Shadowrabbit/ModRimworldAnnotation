using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FDB RID: 4059
	public class RoleRequirement_SupremeGender : RoleRequirement
	{
		// Token: 0x06005F8E RID: 24462 RVA: 0x0020AD99 File Offset: 0x00208F99
		private bool ActiveFor(Precept_Role role)
		{
			return role.restrictToSupremeGender && role.ideo.SupremeGender > Gender.None;
		}

		// Token: 0x06005F8F RID: 24463 RVA: 0x0020ADB3 File Offset: 0x00208FB3
		public override string GetLabel(Precept_Role role)
		{
			if (!this.ActiveFor(role))
			{
				return string.Empty;
			}
			return this.labelKey.Translate(role.ideo.SupremeGender.GetLabel(false));
		}

		// Token: 0x06005F90 RID: 24464 RVA: 0x0020ADEA File Offset: 0x00208FEA
		public override bool Met(Pawn pawn, Precept_Role role)
		{
			return !this.ActiveFor(role) || pawn.gender == role.ideo.SupremeGender;
		}
	}
}
