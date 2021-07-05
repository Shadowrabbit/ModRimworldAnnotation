using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014E3 RID: 5347
	public class StatPart_RoleConversionPower : StatPart
	{
		// Token: 0x06007F6A RID: 32618 RVA: 0x002D08B4 File Offset: 0x002CEAB4
		public override void TransformValue(StatRequest req, ref float val)
		{
			Pawn pawn;
			if (req.HasThing && (pawn = (req.Thing as Pawn)) != null && pawn.Ideo != null)
			{
				Precept_Role role = pawn.Ideo.GetRole(pawn);
				if (role != null)
				{
					val *= role.def.convertPowerFactor;
				}
			}
		}

		// Token: 0x06007F6B RID: 32619 RVA: 0x002D0904 File Offset: 0x002CEB04
		public override string ExplanationPart(StatRequest req)
		{
			Pawn pawn;
			if (req.HasThing && (pawn = (req.Thing as Pawn)) != null && pawn.Ideo != null)
			{
				Precept_Role role = pawn.Ideo.GetRole(pawn);
				if (role != null)
				{
					return "AbilityIdeoConvertBreakdownRole".Translate((req.Thing as Pawn).Named("PAWN"), role.Named("ROLE")) + ": " + role.def.convertPowerFactor.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor);
				}
			}
			return null;
		}
	}
}
