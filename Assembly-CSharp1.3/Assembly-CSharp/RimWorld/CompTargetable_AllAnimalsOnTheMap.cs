using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011F3 RID: 4595
	public class CompTargetable_AllAnimalsOnTheMap : CompTargetable_AllPawnsOnTheMap
	{
		// Token: 0x06006E98 RID: 28312 RVA: 0x00250A0E File Offset: 0x0024EC0E
		protected override TargetingParameters GetTargetingParameters()
		{
			TargetingParameters targetingParameters = base.GetTargetingParameters();
			targetingParameters.validator = delegate(TargetInfo targ)
			{
				if (!base.BaseTargetValidator(targ.Thing))
				{
					return false;
				}
				Pawn pawn = targ.Thing as Pawn;
				return pawn != null && pawn.RaceProps.Animal;
			};
			return targetingParameters;
		}
	}
}
