using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010E7 RID: 4327
	public class ThingSetMaker_RefugeePod : ThingSetMaker
	{
		// Token: 0x06006784 RID: 26500 RVA: 0x0022FD2C File Offset: 0x0022DF2C
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, DownedRefugeeQuestUtility.GetRandomFactionForRefugee(0.6f), PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 20f, false, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, null, false, false));
			outThings.Add(pawn);
			HealthUtility.DamageUntilDowned(pawn, true);
		}

		// Token: 0x06006785 RID: 26501 RVA: 0x0022FDBD File Offset: 0x0022DFBD
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			yield return PawnKindDefOf.SpaceRefugee.race;
			yield break;
		}

		// Token: 0x04003A5C RID: 14940
		private const float RelationWithColonistWeight = 20f;
	}
}
