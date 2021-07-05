using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020001C2 RID: 450
	public static class NameUseChecker
	{
		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000CFC RID: 3324 RVA: 0x00045334 File Offset: 0x00043534
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

		// Token: 0x06000CFD RID: 3325 RVA: 0x00045340 File Offset: 0x00043540
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

		// Token: 0x06000CFE RID: 3326 RVA: 0x000453E0 File Offset: 0x000435E0
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
