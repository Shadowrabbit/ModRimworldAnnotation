using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E96 RID: 3734
	public class Thought_IdeoRoleLost : Thought_Memory
	{
		// Token: 0x17000F42 RID: 3906
		// (get) Token: 0x060057BC RID: 22460 RVA: 0x001DDA40 File Offset: 0x001DBC40
		public Precept_Role Role
		{
			get
			{
				return (Precept_Role)this.sourcePrecept;
			}
		}

		// Token: 0x17000F43 RID: 3907
		// (get) Token: 0x060057BD RID: 22461 RVA: 0x001DDA4D File Offset: 0x001DBC4D
		public override bool ShouldDiscard
		{
			get
			{
				return this.Role == null || this.pawn.Ideo != this.Role.ideo || this.Role.IsAssigned(this.pawn) || base.ShouldDiscard;
			}
		}

		// Token: 0x17000F44 RID: 3908
		// (get) Token: 0x060057BE RID: 22462 RVA: 0x001DDA8A File Offset: 0x001DBC8A
		public override string LabelCap
		{
			get
			{
				return base.CurStage.LabelCap.Formatted(this.Role.Named("ROLE"));
			}
		}

		// Token: 0x17000F45 RID: 3909
		// (get) Token: 0x060057BF RID: 22463 RVA: 0x001DDAB1 File Offset: 0x001DBCB1
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.Role.Named("ROLE"));
			}
		}
	}
}
