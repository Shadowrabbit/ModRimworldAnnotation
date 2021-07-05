using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FD4 RID: 4052
	public class RoleEffect_PawnStatOffset : RoleEffect_PawnStatModifier
	{
		// Token: 0x06005F7C RID: 24444 RVA: 0x0020ABB1 File Offset: 0x00208DB1
		public override string Label(Pawn pawn, Precept_Role role)
		{
			return this.statDef.LabelCap + ": " + this.statDef.ValueToString(this.modifier, ToStringNumberSense.Offset, false);
		}
	}
}
