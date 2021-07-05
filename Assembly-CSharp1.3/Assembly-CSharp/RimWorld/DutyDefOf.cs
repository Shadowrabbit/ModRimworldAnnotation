using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200141D RID: 5149
	[DefOf]
	public static class DutyDefOf
	{
		// Token: 0x06007D10 RID: 32016 RVA: 0x002C472C File Offset: 0x002C292C
		static DutyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DutyDefOf));
		}

		// Token: 0x04004951 RID: 18769
		public static DutyDef TravelOrLeave;

		// Token: 0x04004952 RID: 18770
		public static DutyDef TravelOrWait;

		// Token: 0x04004953 RID: 18771
		public static DutyDef Kidnap;

		// Token: 0x04004954 RID: 18772
		public static DutyDef Steal;

		// Token: 0x04004955 RID: 18773
		public static DutyDef TakeWoundedGuest;

		// Token: 0x04004956 RID: 18774
		public static DutyDef Follow;

		// Token: 0x04004957 RID: 18775
		public static DutyDef PrisonerEscape;

		// Token: 0x04004958 RID: 18776
		public static DutyDef PrisonerEscapeSapper;

		// Token: 0x04004959 RID: 18777
		public static DutyDef DefendAndExpandHive;

		// Token: 0x0400495A RID: 18778
		public static DutyDef DefendHiveAggressively;

		// Token: 0x0400495B RID: 18779
		public static DutyDef LoadAndEnterTransporters;

		// Token: 0x0400495C RID: 18780
		public static DutyDef EnterTransporter;

		// Token: 0x0400495D RID: 18781
		public static DutyDef ManClosestTurret;

		// Token: 0x0400495E RID: 18782
		public static DutyDef SleepForever;

		// Token: 0x0400495F RID: 18783
		public static DutyDef Idle;

		// Token: 0x04004960 RID: 18784
		[MayRequireRoyalty]
		public static DutyDef WanderClose;

		// Token: 0x04004961 RID: 18785
		public static DutyDef AssaultColony;

		// Token: 0x04004962 RID: 18786
		public static DutyDef Breaching;

		// Token: 0x04004963 RID: 18787
		public static DutyDef Sapper;

		// Token: 0x04004964 RID: 18788
		public static DutyDef Escort;

		// Token: 0x04004965 RID: 18789
		public static DutyDef Defend;

		// Token: 0x04004966 RID: 18790
		public static DutyDef Build;

		// Token: 0x04004967 RID: 18791
		public static DutyDef HuntEnemiesIndividual;

		// Token: 0x04004968 RID: 18792
		public static DutyDef DefendBase;

		// Token: 0x04004969 RID: 18793
		[MayRequireRoyalty]
		public static DutyDef AssaultThing;

		// Token: 0x0400496A RID: 18794
		public static DutyDef PrisonerAssaultColony;

		// Token: 0x0400496B RID: 18795
		public static DutyDef ExitMapRandom;

		// Token: 0x0400496C RID: 18796
		public static DutyDef ExitMapBest;

		// Token: 0x0400496D RID: 18797
		public static DutyDef ExitMapBestAndDefendSelf;

		// Token: 0x0400496E RID: 18798
		public static DutyDef ExitMapNearDutyTarget;

		// Token: 0x0400496F RID: 18799
		public static DutyDef MarryPawn;

		// Token: 0x04004970 RID: 18800
		public static DutyDef GiveSpeech;

		// Token: 0x04004971 RID: 18801
		public static DutyDef Spectate;

		// Token: 0x04004972 RID: 18802
		public static DutyDef Party;

		// Token: 0x04004973 RID: 18803
		public static DutyDef BestowingCeremony_MoveInPlace;

		// Token: 0x04004974 RID: 18804
		[MayRequireRoyalty]
		public static DutyDef Bestow;

		// Token: 0x04004975 RID: 18805
		[MayRequireIdeology]
		public static DutyDef Pilgrims_Spectate;

		// Token: 0x04004976 RID: 18806
		[MayRequireIdeology]
		public static DutyDef PlayTargetInstrument;

		// Token: 0x04004977 RID: 18807
		public static DutyDef PrepareCaravan_GatherItems;

		// Token: 0x04004978 RID: 18808
		public static DutyDef PrepareCaravan_Wait;

		// Token: 0x04004979 RID: 18809
		public static DutyDef PrepareCaravan_GatherAnimals;

		// Token: 0x0400497A RID: 18810
		public static DutyDef PrepareCaravan_CollectAnimals;

		// Token: 0x0400497B RID: 18811
		public static DutyDef PrepareCaravan_GatherDownedPawns;

		// Token: 0x0400497C RID: 18812
		public static DutyDef PrepareCaravan_Pause;

		// Token: 0x0400497D RID: 18813
		public static DutyDef ReturnedCaravan_PenAnimals;
	}
}
