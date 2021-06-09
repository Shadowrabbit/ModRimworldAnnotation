using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001762 RID: 5986
	public class ThingSetMaker_RefugeePod : ThingSetMaker
	{
		// Token: 0x06008401 RID: 33793 RVA: 0x00271718 File Offset: 0x0026F918
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, DownedRefugeeQuestUtility.GetRandomFactionForRefugee(), PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 20f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null));
			outThings.Add(pawn);
			HealthUtility.DamageUntilDowned(pawn, true);
		}

		// Token: 0x06008402 RID: 33794 RVA: 0x00058860 File Offset: 0x00056A60
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			yield return PawnKindDefOf.SpaceRefugee.race;
			yield break;
		}

		// Token: 0x04005590 RID: 21904
		private const float RelationWithColonistWeight = 20f;
	}
}
