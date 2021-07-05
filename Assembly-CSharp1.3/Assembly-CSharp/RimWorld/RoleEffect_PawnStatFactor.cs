using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FD5 RID: 4053
	public class RoleEffect_PawnStatFactor : RoleEffect_PawnStatModifier
	{
		// Token: 0x06005F7E RID: 24446 RVA: 0x0020ABED File Offset: 0x00208DED
		public override string Label(Pawn pawn, Precept_Role role)
		{
			return this.statDef.LabelCap + ": " + this.statDef.ValueToString(this.modifier, ToStringNumberSense.Factor, false);
		}
	}
}
