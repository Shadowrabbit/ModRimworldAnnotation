using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F7E RID: 3966
	public abstract class RitualPosition : IExposable
	{
		// Token: 0x06005DF6 RID: 24054
		public abstract PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual);

		// Token: 0x06005DF7 RID: 24055 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool CanUse(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			return true;
		}

		// Token: 0x06005DF8 RID: 24056 RVA: 0x002041F8 File Offset: 0x002023F8
		public virtual void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.highlight, "highlight", false, false);
		}

		// Token: 0x06005DF9 RID: 24057 RVA: 0x0020420C File Offset: 0x0020240C
		protected IntVec3 GetFallbackSpot(CellRect rect, IntVec3 spot, Pawn p, LordJob_Ritual ritual, Func<IntVec3, bool> Validator)
		{
			foreach (IntVec3 intVec in rect.AdjacentCellsCardinal)
			{
				if (Validator(intVec))
				{
					return intVec;
				}
			}
			foreach (IntVec3 intVec2 in rect.AdjacentCells)
			{
				if (Validator(intVec2))
				{
					return intVec2;
				}
			}
			IntVec3 result;
			CellFinder.TryFindRandomCellNear(spot, p.Map, 3, new Predicate<IntVec3>(Validator.Invoke), out result, -1);
			return result;
		}

		// Token: 0x04003657 RID: 13911
		public bool highlight;
	}
}
