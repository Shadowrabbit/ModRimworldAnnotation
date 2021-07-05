using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200093A RID: 2362
	public class ThoughtWorker_Precept_Skullspike : ThoughtWorker_Precept
	{
		// Token: 0x06003CC1 RID: 15553 RVA: 0x001501CC File Offset: 0x0014E3CC
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (p.surroundings == null || p.IsSlave)
			{
				return false;
			}
			int val = p.surroundings.NumSkullspikeSightings();
			if (ThoughtWorker_Precept_Skullspike.LowRange.Includes(val))
			{
				return ThoughtState.ActiveAtStage(0);
			}
			if (ThoughtWorker_Precept_Skullspike.MediumRange.Includes(val))
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (ThoughtWorker_Precept_Skullspike.MaxRange.Includes(val))
			{
				return ThoughtState.ActiveAtStage(2);
			}
			return false;
		}

		// Token: 0x040020B9 RID: 8377
		public static readonly IntRange LowRange = new IntRange(1, 3);

		// Token: 0x040020BA RID: 8378
		public static readonly IntRange MediumRange = new IntRange(4, 8);

		// Token: 0x040020BB RID: 8379
		public static readonly IntRange MaxRange = new IntRange(9, int.MaxValue);
	}
}
