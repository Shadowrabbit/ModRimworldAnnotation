using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FD6 RID: 4054
	public class RoleEffect_ProductionQualityOffset : RoleEffect
	{
		// Token: 0x06005F80 RID: 24448 RVA: 0x0020AC21 File Offset: 0x00208E21
		public RoleEffect_ProductionQualityOffset()
		{
			this.labelKey = "RoleEffectProductionQualityOffset";
		}

		// Token: 0x06005F81 RID: 24449 RVA: 0x0020AC34 File Offset: 0x00208E34
		public override string Label(Pawn pawn, Precept_Role role)
		{
			return this.labelKey.Translate(this.offset.ToStringWithSign());
		}

		// Token: 0x040036E1 RID: 14049
		public int offset;
	}
}
