using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200027E RID: 638
	public static class NameUseChecker
	{
		// Token: 0x1700030D RID: 781
		// (get) Token: 0x0600108C RID: 4236 RVA: 0x00012415 File Offset: 0x00010615
		public static IEnumerable<Name> AllPawnsNamesEverUsed
		{
			get
			{
				foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
				{
					if (pawn.Name != null)
					{
						yield return pawn.Name;
					}
				}
				List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x000BA678 File Offset: 0x000B8878
		public static bool NameWordIsUsed(string singleName)
		{
			foreach (Name name in NameUseChecker.AllPawnsNamesEverUsed)
			{
				NameTriple nameTriple = name as NameTriple;
				if (nameTriple != null && (singleName == nameTriple.First || singleName == nameTriple.Nick || singleName == nameTriple.Last))
				{
					return true;
				}
				NameSingle nameSingle = name as NameSingle;
				if (nameSingle != null && nameSingle.Name == singleName)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x000BA718 File Offset: 0x000B8918
		public static bool NameSingleIsUsed(string candidate)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
			{
				NameSingle nameSingle = pawn.Name as NameSingle;
				if (nameSingle != null && nameSingle.Name == candidate)
				{
					return true;
				}
			}
			return false;
		}
	}
}
