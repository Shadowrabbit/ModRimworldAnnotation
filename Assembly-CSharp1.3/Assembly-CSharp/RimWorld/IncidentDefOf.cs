using System;

namespace RimWorld
{
	// Token: 0x02001435 RID: 5173
	[DefOf]
	public static class IncidentDefOf
	{
		// Token: 0x06007D28 RID: 32040 RVA: 0x002C48C4 File Offset: 0x002C2AC4
		static IncidentDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(IncidentDefOf));
		}

		// Token: 0x04004BBF RID: 19391
		public static IncidentDef RaidEnemy;

		// Token: 0x04004BC0 RID: 19392
		public static IncidentDef RaidFriendly;

		// Token: 0x04004BC1 RID: 19393
		public static IncidentDef VisitorGroup;

		// Token: 0x04004BC2 RID: 19394
		public static IncidentDef TravelerGroup;

		// Token: 0x04004BC3 RID: 19395
		public static IncidentDef TraderCaravanArrival;

		// Token: 0x04004BC4 RID: 19396
		public static IncidentDef Eclipse;

		// Token: 0x04004BC5 RID: 19397
		public static IncidentDef ToxicFallout;

		// Token: 0x04004BC6 RID: 19398
		public static IncidentDef SolarFlare;

		// Token: 0x04004BC7 RID: 19399
		public static IncidentDef ManhunterPack;

		// Token: 0x04004BC8 RID: 19400
		public static IncidentDef ShipChunkDrop;

		// Token: 0x04004BC9 RID: 19401
		public static IncidentDef OrbitalTraderArrival;

		// Token: 0x04004BCA RID: 19402
		public static IncidentDef WandererJoin;

		// Token: 0x04004BCB RID: 19403
		public static IncidentDef Infestation;

		// Token: 0x04004BCC RID: 19404
		public static IncidentDef GiveQuest_Random;

		// Token: 0x04004BCD RID: 19405
		public static IncidentDef MechCluster;

		// Token: 0x04004BCE RID: 19406
		public static IncidentDef FarmAnimalsWanderIn;

		// Token: 0x04004BCF RID: 19407
		[MayRequireIdeology]
		public static IncidentDef WanderersSkylantern;

		// Token: 0x04004BD0 RID: 19408
		[MayRequireIdeology]
		public static IncidentDef GauranlenPodSpawn;

		// Token: 0x04004BD1 RID: 19409
		[MayRequireIdeology]
		public static IncidentDef Infestation_Jelly;
	}
}
