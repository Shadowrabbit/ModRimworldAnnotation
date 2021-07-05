using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001297 RID: 4759
	public static class PlayerPawnsDisplayOrderUtility
	{
		// Token: 0x060071BB RID: 29115 RVA: 0x00260570 File Offset: 0x0025E770
		public static void Sort(List<Pawn> pawns)
		{
			pawns.SortBy(PlayerPawnsDisplayOrderUtility.displayOrderGetter, PlayerPawnsDisplayOrderUtility.thingIDNumberGetter);
		}

		// Token: 0x060071BC RID: 29116 RVA: 0x00260582 File Offset: 0x0025E782
		public static IEnumerable<Pawn> InOrder(IEnumerable<Pawn> pawns)
		{
			return pawns.OrderBy(PlayerPawnsDisplayOrderUtility.displayOrderGetter).ThenBy(PlayerPawnsDisplayOrderUtility.thingIDNumberGetter);
		}

		// Token: 0x04003EA9 RID: 16041
		private static Func<Pawn, int> displayOrderGetter = delegate(Pawn x)
		{
			if (x.playerSettings == null)
			{
				return 999999;
			}
			return x.playerSettings.displayOrder;
		};

		// Token: 0x04003EAA RID: 16042
		private static Func<Pawn, int> thingIDNumberGetter = (Pawn x) => x.thingIDNumber;
	}
}
