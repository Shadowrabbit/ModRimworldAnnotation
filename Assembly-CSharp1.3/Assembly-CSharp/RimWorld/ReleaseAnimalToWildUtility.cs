using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012B7 RID: 4791
	public static class ReleaseAnimalToWildUtility
	{
		// Token: 0x06007279 RID: 29305 RVA: 0x00263658 File Offset: 0x00261858
		public static void CheckWarnAboutBondedAnimal(Pawn designated)
		{
			if (!designated.RaceProps.IsFlesh)
			{
				return;
			}
			Pawn firstDirectRelationPawn = designated.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, (Pawn x) => !x.Dead);
			if (firstDirectRelationPawn != null)
			{
				Messages.Message("MessageReleaseBondedAnimal".Translate(designated.LabelShort, firstDirectRelationPawn.LabelShort, designated.Named("DESIGNATED"), firstDirectRelationPawn.Named("BONDED")), designated, MessageTypeDefOf.CautionInput, false);
			}
		}

		// Token: 0x0600727A RID: 29306 RVA: 0x002636F4 File Offset: 0x002618F4
		public static void DoReleaseAnimal(Pawn animal, Pawn releasedBy)
		{
			PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(animal, null, PawnDiedOrDownedThoughtsKind.ReleasedToWild);
			releasedBy.interactions.TryInteractWith(animal, InteractionDefOf.ReleaseToWild);
			animal.Map.designationManager.RemoveDesignation(animal.Map.designationManager.DesignationOn(animal, DesignationDefOf.ReleaseAnimalToWild));
			animal.SetFaction(null, null);
			Pawn_Ownership ownership = animal.ownership;
			if (ownership != null)
			{
				ownership.UnclaimAll();
			}
			Messages.Message("MessageAnimalReturnedWildReleased".Translate(animal.LabelShort, animal), releasedBy, MessageTypeDefOf.NeutralEvent, true);
		}
	}
}
