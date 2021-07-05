using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x020002D1 RID: 721
	public static class HediffGiverUtility
	{
		// Token: 0x06001387 RID: 4999 RVA: 0x0006EC38 File Offset: 0x0006CE38
		public static bool TryApply(Pawn pawn, HediffDef hediff, IEnumerable<BodyPartDef> partsToAffect, bool canAffectAnyLivePart = false, int countToAffect = 1, List<Hediff> outAddedHediffs = null)
		{
			if (canAffectAnyLivePart || partsToAffect != null)
			{
				bool result = false;
				Func<BodyPartRecord, bool> <>9__0;
				Func<BodyPartRecord, bool> <>9__2;
				for (int i = 0; i < countToAffect; i++)
				{
					IEnumerable<BodyPartRecord> enumerable = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null);
					if (partsToAffect != null)
					{
						IEnumerable<BodyPartRecord> source = enumerable;
						Func<BodyPartRecord, bool> predicate;
						if ((predicate = <>9__0) == null)
						{
							predicate = (<>9__0 = ((BodyPartRecord p) => partsToAffect.Contains(p.def)));
						}
						enumerable = source.Where(predicate);
					}
					if (canAffectAnyLivePart)
					{
						enumerable = from p in enumerable
						where p.def.alive
						select p;
					}
					IEnumerable<BodyPartRecord> source2 = enumerable;
					Func<BodyPartRecord, bool> predicate2;
					if ((predicate2 = <>9__2) == null)
					{
						predicate2 = (<>9__2 = ((BodyPartRecord p) => !pawn.health.hediffSet.HasHediff(hediff, p, false) && !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(p)));
					}
					enumerable = source2.Where(predicate2);
					if (!enumerable.Any<BodyPartRecord>())
					{
						break;
					}
					BodyPartRecord partRecord = enumerable.RandomElementByWeight((BodyPartRecord x) => x.coverageAbs);
					Hediff hediff2 = HediffMaker.MakeHediff(hediff, pawn, partRecord);
					pawn.health.AddHediff(hediff2, null, null, null);
					if (outAddedHediffs != null)
					{
						outAddedHediffs.Add(hediff2);
					}
					result = true;
				}
				return result;
			}
			if (!pawn.health.hediffSet.HasHediff(hediff, false))
			{
				Hediff hediff3 = HediffMaker.MakeHediff(hediff, pawn, null);
				pawn.health.AddHediff(hediff3, null, null, null);
				if (outAddedHediffs != null)
				{
					outAddedHediffs.Add(hediff3);
				}
				return true;
			}
			return false;
		}
	}
}
