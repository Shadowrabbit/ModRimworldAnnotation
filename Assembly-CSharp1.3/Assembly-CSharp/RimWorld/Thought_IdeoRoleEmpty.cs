using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E97 RID: 3735
	public class Thought_IdeoRoleEmpty : Thought_Situational
	{
		// Token: 0x17000F46 RID: 3910
		// (get) Token: 0x060057C1 RID: 22465 RVA: 0x001DDA40 File Offset: 0x001DBC40
		public Precept_Role Role
		{
			get
			{
				return (Precept_Role)this.sourcePrecept;
			}
		}

		// Token: 0x17000F47 RID: 3911
		// (get) Token: 0x060057C2 RID: 22466 RVA: 0x001DDAD8 File Offset: 0x001DBCD8
		public override string LabelCap
		{
			get
			{
				return base.CurStage.LabelCap.Formatted(this.Role.Named("ROLE"));
			}
		}

		// Token: 0x17000F48 RID: 3912
		// (get) Token: 0x060057C3 RID: 22467 RVA: 0x001DDAFF File Offset: 0x001DBCFF
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.Role.ideo.memberName, this.Role.Named("ROLE"));
			}
		}

		// Token: 0x060057C4 RID: 22468 RVA: 0x001DDB3C File Offset: 0x001DBD3C
		protected override ThoughtState CurrentStateInternal()
		{
			if (this.pawn.IsSlave)
			{
				return false;
			}
			if (GenDate.DaysPassed < 10)
			{
				return false;
			}
			if (this.Role.def.leaderRole && !Faction.OfPlayer.ideos.IsPrimary(this.Role.ideo))
			{
				return false;
			}
			return this.Role.Active && this.pawn.Ideo == this.Role.ideo && this.Role.ChosenPawnSingle() == null;
		}

		// Token: 0x060057C5 RID: 22469 RVA: 0x001DDBE0 File Offset: 0x001DBDE0
		public override bool GroupsWith(Thought other)
		{
			Thought_IdeoRoleEmpty thought_IdeoRoleEmpty;
			return (thought_IdeoRoleEmpty = (other as Thought_IdeoRoleEmpty)) != null && this.Role == thought_IdeoRoleEmpty.Role;
		}
	}
}
