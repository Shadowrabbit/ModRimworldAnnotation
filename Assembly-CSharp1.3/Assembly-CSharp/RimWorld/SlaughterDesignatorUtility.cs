using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012BB RID: 4795
	public static class SlaughterDesignatorUtility
	{
		// Token: 0x0600728E RID: 29326 RVA: 0x00263BFC File Offset: 0x00261DFC
		public static void CheckWarnAboutBondedAnimal(Pawn designated)
		{
			if (!designated.RaceProps.IsFlesh)
			{
				return;
			}
			Pawn firstDirectRelationPawn = designated.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, (Pawn x) => !x.Dead);
			if (firstDirectRelationPawn != null)
			{
				Messages.Message("MessageSlaughteringBondedAnimal".Translate(designated.LabelShort, firstDirectRelationPawn.LabelShort, designated.Named("DESIGNATED"), firstDirectRelationPawn.Named("BONDED")), designated, MessageTypeDefOf.CautionInput, false);
			}
		}

		// Token: 0x0600728F RID: 29327 RVA: 0x00263C98 File Offset: 0x00261E98
		public static void CheckWarnAboutVeneratedAnimal(Pawn pawn)
		{
			bool flag = true;
			foreach (Pawn pawn2 in pawn.Map.mapPawns.FreeColonistsSpawned)
			{
				if (!pawn2.WorkTypeIsDisabled(WorkTypeDefOf.Hunting) && (pawn2.Ideo == null || !pawn2.Ideo.IsVeneratedAnimal(pawn)))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				Messages.Message("MessageAnimalIsVeneratedForAllColonists".Translate(pawn.GetKindLabelPlural(-1).CapitalizeFirst().Named("PAWNKINDLABELPLURAL"), Faction.OfPlayer.def.pawnsPlural.Named("PAWNS")), pawn, MessageTypeDefOf.CautionInput, false);
			}
		}
	}
}
