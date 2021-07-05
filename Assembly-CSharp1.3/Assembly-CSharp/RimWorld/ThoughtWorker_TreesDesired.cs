using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02000976 RID: 2422
	public class ThoughtWorker_TreesDesired : ThoughtWorker_Precept
	{
		// Token: 0x06003D67 RID: 15719 RVA: 0x00151910 File Offset: 0x0014FB10
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return true;
		}

		// Token: 0x06003D68 RID: 15720 RVA: 0x00152070 File Offset: 0x00150270
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!base.CurrentStateInternal(p).Active || p.surroundings == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(this.ThoughtStageIndex(p));
		}

		// Token: 0x06003D69 RID: 15721 RVA: 0x001520A8 File Offset: 0x001502A8
		private int ThoughtStageIndex(Pawn p)
		{
			ThoughtWorker_TreesDesired.<>c__DisplayClass8_0 CS$<>8__locals1;
			CS$<>8__locals1.p = p;
			int num = ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__SightingsInRange|8_0(TreeCategory.Super, 2500, ref CS$<>8__locals1);
			if (num > 1)
			{
				return 0;
			}
			if (num == 1)
			{
				return 1;
			}
			int num2 = ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__SightingsInRange|8_0(TreeCategory.Super, 15000, ref CS$<>8__locals1);
			int num3 = ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__SightingsInRange|8_0(TreeCategory.Full, 15000, ref CS$<>8__locals1);
			if (num2 >= 1 && num3 >= 3)
			{
				return 2;
			}
			if (num2 >= 1)
			{
				return 3;
			}
			if (num3 >= 5)
			{
				return 4;
			}
			if (num3 >= 3)
			{
				return 5;
			}
			int num4 = ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__SightingsInRange|8_0(TreeCategory.Mini, 30000, ref CS$<>8__locals1);
			if (num3 >= 1 && num4 >= 1)
			{
				return 6;
			}
			if (num3 == 1)
			{
				return 7;
			}
			if (num4 >= 5)
			{
				return 8;
			}
			if (ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__SightingsInRange|8_0(TreeCategory.Mini, 60000, ref CS$<>8__locals1) >= 1)
			{
				return 9;
			}
			if (ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__AnySightingsInRange|8_1(120000, ref CS$<>8__locals1))
			{
				return 10;
			}
			if (ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__AnySightingsInRange|8_1(180000, ref CS$<>8__locals1))
			{
				return 11;
			}
			if (ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__AnySightingsInRange|8_1(240000, ref CS$<>8__locals1))
			{
				return 12;
			}
			if (ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__AnySightingsInRange|8_1(300000, ref CS$<>8__locals1))
			{
				return 13;
			}
			return 14;
		}

		// Token: 0x06003D6B RID: 15723 RVA: 0x0015218F File Offset: 0x0015038F
		[CompilerGenerated]
		internal static int <ThoughtStageIndex>g__SightingsInRange|8_0(TreeCategory category, int ticks, ref ThoughtWorker_TreesDesired.<>c__DisplayClass8_0 A_2)
		{
			return A_2.p.surroundings.NumSightingsInRange(category, ticks);
		}

		// Token: 0x06003D6C RID: 15724 RVA: 0x001521A3 File Offset: 0x001503A3
		[CompilerGenerated]
		internal static bool <ThoughtStageIndex>g__AnySightingsInRange|8_1(int ticks, ref ThoughtWorker_TreesDesired.<>c__DisplayClass8_0 A_1)
		{
			return ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__SightingsInRange|8_0(TreeCategory.Super, ticks, ref A_1) > 0 || ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__SightingsInRange|8_0(TreeCategory.Full, ticks, ref A_1) > 0 || ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__SightingsInRange|8_0(TreeCategory.Mini, ticks, ref A_1) > 0;
		}

		// Token: 0x040020D5 RID: 8405
		private const int RecentlySawTree = 1800;

		// Token: 0x040020D6 RID: 8406
		private const int SawTree = 15000;

		// Token: 0x040020D7 RID: 8407
		private const int NoTree = 30000;

		// Token: 0x040020D8 RID: 8408
		private const int NoTreeToday = 60000;

		// Token: 0x040020D9 RID: 8409
		private const int TreesMissed = 120000;

		// Token: 0x040020DA RID: 8410
		private const int TreesSorelyMissed = 300000;
	}
}
