using System;

namespace RimWorld
{
	// Token: 0x02000E65 RID: 3685
	public struct ApparelRequirementWithSource
	{
		// Token: 0x17000EC1 RID: 3777
		// (get) Token: 0x06005575 RID: 21877 RVA: 0x001CEEF2 File Offset: 0x001CD0F2
		public ApparelRequirementSource Source
		{
			get
			{
				if (this.sourceRole != null)
				{
					return ApparelRequirementSource.Role;
				}
				if (this.sourceTitle != null)
				{
					return ApparelRequirementSource.Title;
				}
				return ApparelRequirementSource.Invalid;
			}
		}

		// Token: 0x17000EC2 RID: 3778
		// (get) Token: 0x06005576 RID: 21878 RVA: 0x001CEF09 File Offset: 0x001CD109
		public string SourceLabelCap
		{
			get
			{
				if (this.sourceTitle != null)
				{
					return this.sourceTitle.def.GetLabelCapFor(this.sourceTitle.pawn);
				}
				return this.sourceRole.LabelCap;
			}
		}

		// Token: 0x06005577 RID: 21879 RVA: 0x001CEF3A File Offset: 0x001CD13A
		public ApparelRequirementWithSource(ApparelRequirement requirement, Precept_Role role)
		{
			this.requirement = requirement;
			this.sourceRole = role;
			this.sourceTitle = null;
		}

		// Token: 0x06005578 RID: 21880 RVA: 0x001CEF51 File Offset: 0x001CD151
		public ApparelRequirementWithSource(ApparelRequirement requirement, RoyalTitle title)
		{
			this.requirement = requirement;
			this.sourceTitle = title;
			this.sourceRole = null;
		}

		// Token: 0x04003293 RID: 12947
		public ApparelRequirement requirement;

		// Token: 0x04003294 RID: 12948
		public Precept_Role sourceRole;

		// Token: 0x04003295 RID: 12949
		public RoyalTitle sourceTitle;
	}
}
