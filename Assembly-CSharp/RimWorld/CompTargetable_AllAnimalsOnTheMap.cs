using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018BB RID: 6331
	public class CompTargetable_AllAnimalsOnTheMap : CompTargetable_AllPawnsOnTheMap
	{
		// Token: 0x06008C7E RID: 35966 RVA: 0x0005E2E0 File Offset: 0x0005C4E0
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
