using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001491 RID: 5265
	public static class PawnRelationUtility
	{
		// Token: 0x0600718D RID: 29069 RVA: 0x0004C6A4 File Offset: 0x0004A8A4
		public static IEnumerable<PawnRelationDef> GetRelations(this Pawn me, Pawn other)
		{
			if (me == other)
			{
				yield break;
			}
			if (!me.RaceProps.IsFlesh || !other.RaceProps.IsFlesh)
			{
				yield break;
			}
			if (!me.relations.RelatedToAnyoneOrAnyoneRelatedToMe || !other.relations.RelatedToAnyoneOrAnyoneRelatedToMe)
			{
				yield break;
			}
			bool anyNonKinFamilyByBloodRelation = false;
			List<PawnRelationDef> defs = DefDatabase<PawnRelationDef>.AllDefsListForReading;
			int i = 0;
			int count = defs.Count;
			while (i < count)
			{
				PawnRelationDef pawnRelationDef = defs[i];
				if (pawnRelationDef != PawnRelationDefOf.Kin && pawnRelationDef.Worker.InRelation(me, other))
				{
					if (pawnRelationDef.familyByBloodRelation)
					{
						anyNonKinFamilyByBloodRelation = true;
					}
					yield return pawnRelationDef;
				}
				int num = i;
				i = num + 1;
			}
			if (!anyNonKinFamilyByBloodRelation && PawnRelationDefOf.Kin.Worker.InRelation(me, other))
			{
				yield return PawnRelationDefOf.Kin;
			}
			defs = null;
			yield break;
			yield break;
		}

		// Token: 0x0600718E RID: 29070 RVA: 0x0022C5A0 File Offset: 0x0022A7A0
		public static PawnRelationDef GetMostImportantRelation(this Pawn me, Pawn other)
		{
			PawnRelationDef pawnRelationDef = null;
			foreach (PawnRelationDef pawnRelationDef2 in me.GetRelations(other))
			{
				if (pawnRelationDef == null || pawnRelationDef2.importance > pawnRelationDef.importance)
				{
					pawnRelationDef = pawnRelationDef2;
				}
			}
			return pawnRelationDef;
		}

		// Token: 0x0600718F RID: 29071 RVA: 0x0022C600 File Offset: 0x0022A800
		public static void Notify_PawnsSeenByPlayer(IEnumerable<Pawn> seenPawns, out string pawnRelationsInfo, bool informEvenIfSeenBefore = false, bool writeSeenPawnsNames = true)
		{
			StringBuilder stringBuilder = new StringBuilder();
			IEnumerable<Pawn> enumerable = from x in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners
			where x.relations.everSeenByPlayer
			select x;
			bool flag = false;
			foreach (Pawn pawn in seenPawns)
			{
				if (pawn.RaceProps.IsFlesh && (informEvenIfSeenBefore || !pawn.relations.everSeenByPlayer))
				{
					pawn.relations.everSeenByPlayer = true;
					bool flag2 = false;
					foreach (Pawn pawn2 in enumerable)
					{
						if (pawn != pawn2)
						{
							PawnRelationDef mostImportantRelation = pawn2.GetMostImportantRelation(pawn);
							if (mostImportantRelation != null)
							{
								if (!flag2)
								{
									flag2 = true;
									if (flag)
									{
										stringBuilder.AppendLine();
									}
									if (writeSeenPawnsNames)
									{
										stringBuilder.AppendLine(pawn.KindLabel.CapitalizeFirst() + " " + pawn.NameShortColored.Resolve() + ":");
									}
								}
								flag = true;
								stringBuilder.AppendLine("  - " + "Relationship".Translate(mostImportantRelation.GetGenderSpecificLabelCap(pawn), pawn2.KindLabel + " " + pawn2.NameShortColored.Resolve(), pawn2));
							}
						}
					}
				}
			}
			if (flag)
			{
				pawnRelationsInfo = stringBuilder.ToString().TrimEndNewlines();
				return;
			}
			pawnRelationsInfo = null;
		}

		// Token: 0x06007190 RID: 29072 RVA: 0x0022C7D8 File Offset: 0x0022A9D8
		public static void Notify_PawnsSeenByPlayer_Letter(IEnumerable<Pawn> seenPawns, ref TaggedString letterLabel, ref TaggedString letterText, string relationsInfoHeader, bool informEvenIfSeenBefore = false, bool writeSeenPawnsNames = true)
		{
			string text;
			PawnRelationUtility.Notify_PawnsSeenByPlayer(seenPawns, out text, informEvenIfSeenBefore, writeSeenPawnsNames);
			if (!text.NullOrEmpty())
			{
				if (letterLabel.NullOrEmpty())
				{
					letterLabel = "LetterLabelNoticedRelatedPawns".Translate();
				}
				else
				{
					letterLabel += ": " + "RelationshipAppendedLetterSuffix".Translate().CapitalizeFirst();
				}
				if (!letterText.NullOrEmpty())
				{
					letterText += "\n\n";
				}
				letterText += relationsInfoHeader + "\n\n" + text;
			}
		}

		// Token: 0x06007191 RID: 29073 RVA: 0x0022C880 File Offset: 0x0022AA80
		public static void Notify_PawnsSeenByPlayer_Letter_Send(IEnumerable<Pawn> seenPawns, string relationsInfoHeader, LetterDef letterDef, bool informEvenIfSeenBefore = false, bool writeSeenPawnsNames = true)
		{
			TaggedString label = "";
			TaggedString text = "";
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(seenPawns, ref label, ref text, relationsInfoHeader, informEvenIfSeenBefore, writeSeenPawnsNames);
			if (!text.NullOrEmpty())
			{
				Pawn pawn = null;
				foreach (Pawn pawn2 in seenPawns)
				{
					if (PawnRelationUtility.GetMostImportantColonyRelative(pawn2) != null)
					{
						pawn = pawn2;
						break;
					}
				}
				if (pawn == null)
				{
					pawn = seenPawns.FirstOrDefault<Pawn>();
				}
				Find.LetterStack.ReceiveLetter(label, text, letterDef, pawn, null, null, null, null);
			}
		}

		// Token: 0x06007192 RID: 29074 RVA: 0x0022C920 File Offset: 0x0022AB20
		public static bool TryAppendRelationsWithColonistsInfo(ref TaggedString text, Pawn pawn)
		{
			TaggedString taggedString = null;
			return PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref taggedString, pawn);
		}

		// Token: 0x06007193 RID: 29075 RVA: 0x0022C940 File Offset: 0x0022AB40
		public static bool TryAppendRelationsWithColonistsInfo(ref TaggedString text, ref TaggedString title, Pawn pawn)
		{
			Pawn mostImportantColonyRelative = PawnRelationUtility.GetMostImportantColonyRelative(pawn);
			if (mostImportantColonyRelative == null)
			{
				return false;
			}
			if (title != null)
			{
				title += " (" + "RelationshipAppendedLetterSuffix".Translate() + ")";
			}
			string genderSpecificLabel = mostImportantColonyRelative.GetMostImportantRelation(pawn).GetGenderSpecificLabel(pawn);
			if (mostImportantColonyRelative.IsColonist)
			{
				text += "\n\n" + "RelationshipAppendedLetterTextColonist".Translate(mostImportantColonyRelative.LabelShort, genderSpecificLabel, mostImportantColonyRelative.Named("RELATIVE"), pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", false);
			}
			else
			{
				text += "\n\n" + "RelationshipAppendedLetterTextPrisoner".Translate(mostImportantColonyRelative.LabelShort, genderSpecificLabel, mostImportantColonyRelative.Named("RELATIVE"), pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", false);
			}
			return true;
		}

		// Token: 0x06007194 RID: 29076 RVA: 0x0022CA64 File Offset: 0x0022AC64
		public static Pawn GetMostImportantColonyRelative(Pawn pawn)
		{
			if (pawn.relations == null || !pawn.relations.RelatedToAnyoneOrAnyoneRelatedToMe)
			{
				return null;
			}
			IEnumerable<Pawn> enumerable = from x in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners
			where x.relations.everSeenByPlayer
			select x;
			float num = 0f;
			Pawn pawn2 = null;
			foreach (Pawn pawn3 in enumerable)
			{
				PawnRelationDef mostImportantRelation = pawn.GetMostImportantRelation(pawn3);
				if (mostImportantRelation != null && (pawn2 == null || mostImportantRelation.importance > num))
				{
					num = mostImportantRelation.importance;
					pawn2 = pawn3;
				}
			}
			return pawn2;
		}

		// Token: 0x06007195 RID: 29077 RVA: 0x0022CB14 File Offset: 0x0022AD14
		public static float MaxPossibleBioAgeAt(float myBiologicalAge, float myChronologicalAge, float atChronologicalAge)
		{
			float num = Mathf.Min(myBiologicalAge, myChronologicalAge - atChronologicalAge);
			if (num < 0f)
			{
				return -1f;
			}
			return num;
		}

		// Token: 0x06007196 RID: 29078 RVA: 0x0004C6BB File Offset: 0x0004A8BB
		public static float MinPossibleBioAgeAt(float myBiologicalAge, float atChronologicalAge)
		{
			return Mathf.Max(myBiologicalAge - atChronologicalAge, 0f);
		}
	}
}
