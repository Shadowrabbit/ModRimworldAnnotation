using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200096B RID: 2411
	public class ThoughtWorker_Precept_ArtificialBlind : ThoughtWorker_Precept
	{
		// Token: 0x06003D4C RID: 15692 RVA: 0x00151BA1 File Offset: 0x0014FDA1
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_Precept_ArtificialBlind.IsArtificiallyBlind(p);
		}

		// Token: 0x06003D4D RID: 15693 RVA: 0x00151BB0 File Offset: 0x0014FDB0
		public static bool IsArtificiallyBlind(Pawn p)
		{
			if (ThoughtWorker_Precept_Blind.IsBlind(p))
			{
				return false;
			}
			if (p.apparel != null)
			{
				using (List<Apparel>.Enumerator enumerator = p.apparel.WornApparel.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def.apparel.blocksVision)
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}
	}
}
