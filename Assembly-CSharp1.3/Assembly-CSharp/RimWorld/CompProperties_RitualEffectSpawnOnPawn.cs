using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FC7 RID: 4039
	public class CompProperties_RitualEffectSpawnOnPawn : CompProperties_RitualEffectIntervalSpawn
	{
		// Token: 0x040036CB RID: 14027
		public Vector3 westRotationOffset;

		// Token: 0x040036CC RID: 14028
		public Vector3 eastRotationOffset;

		// Token: 0x040036CD RID: 14029
		public Vector3 northRotationOffset;

		// Token: 0x040036CE RID: 14030
		public Vector3 southRotationOffset;

		// Token: 0x040036CF RID: 14031
		[NoTranslate]
		public string requiredTag;
	}
}
