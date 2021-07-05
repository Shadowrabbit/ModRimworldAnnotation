using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001C5D RID: 7261
	[DefOf]
	public static class DutyDefOf
	{
		// Token: 0x06009F60 RID: 40800 RVA: 0x0006A2CD File Offset: 0x000684CD
		static DutyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DutyDefOf));
		}

		// Token: 0x040068DC RID: 26844
		public static DutyDef TravelOrLeave;

		// Token: 0x040068DD RID: 26845
		public static DutyDef TravelOrWait;

		// Token: 0x040068DE RID: 26846
		public static DutyDef Kidnap;

		// Token: 0x040068DF RID: 26847
		public static DutyDef Steal;

		// Token: 0x040068E0 RID: 26848
		public static DutyDef TakeWoundedGuest;

		// Token: 0x040068E1 RID: 26849
		public static DutyDef Follow;

		// Token: 0x040068E2 RID: 26850
		public static DutyDef PrisonerEscape;

		// Token: 0x040068E3 RID: 26851
		public static DutyDef PrisonerEscapeSapper;

		// Token: 0x040068E4 RID: 26852
		public static DutyDef DefendAndExpandHive;

		// Token: 0x040068E5 RID: 26853
		public static DutyDef DefendHiveAggressively;

		// Token: 0x040068E6 RID: 26854
		public static DutyDef LoadAndEnterTransporters;

		// Token: 0x040068E7 RID: 26855
		public static DutyDef EnterTransporter;

		// Token: 0x040068E8 RID: 26856
		public static DutyDef ManClosestTurret;

		// Token: 0x040068E9 RID: 26857
		public static DutyDef SleepForever;

		// Token: 0x040068EA RID: 26858
		public static DutyDef Idle;

		// Token: 0x040068EB RID: 26859
		[MayRequireRoyalty]
		public static DutyDef WanderClose;

		// Token: 0x040068EC RID: 26860
		public static DutyDef AssaultColony;

		// Token: 0x040068ED RID: 26861
		public static DutyDef Sapper;

		// Token: 0x040068EE RID: 26862
		public static DutyDef Escort;

		// Token: 0x040068EF RID: 26863
		public static DutyDef Defend;

		// Token: 0x040068F0 RID: 26864
		public static DutyDef Build;

		// Token: 0x040068F1 RID: 26865
		public static DutyDef HuntEnemiesIndividual;

		// Token: 0x040068F2 RID: 26866
		public static DutyDef DefendBase;

		// Token: 0x040068F3 RID: 26867
		[MayRequireRoyalty]
		public static DutyDef AssaultThing;

		// Token: 0x040068F4 RID: 26868
		public static DutyDef ExitMapRandom;

		// Token: 0x040068F5 RID: 26869
		public static DutyDef ExitMapBest;

		// Token: 0x040068F6 RID: 26870
		public static DutyDef ExitMapBestAndDefendSelf;

		// Token: 0x040068F7 RID: 26871
		public static DutyDef ExitMapNearDutyTarget;

		// Token: 0x040068F8 RID: 26872
		public static DutyDef MarryPawn;

		// Token: 0x040068F9 RID: 26873
		public static DutyDef GiveSpeech;

		// Token: 0x040068FA RID: 26874
		public static DutyDef Spectate;

		// Token: 0x040068FB RID: 26875
		public static DutyDef Party;

		// Token: 0x040068FC RID: 26876
		public static DutyDef BestowingCeremony_MoveInPlace;

		// Token: 0x040068FD RID: 26877
		public static DutyDef PrepareCaravan_GatherItems;

		// Token: 0x040068FE RID: 26878
		public static DutyDef PrepareCaravan_Wait;

		// Token: 0x040068FF RID: 26879
		public static DutyDef PrepareCaravan_GatherPawns;

		// Token: 0x04006900 RID: 26880
		public static DutyDef PrepareCaravan_GatherDownedPawns;

		// Token: 0x04006901 RID: 26881
		public static DutyDef PrepareCaravan_Pause;
	}
}
