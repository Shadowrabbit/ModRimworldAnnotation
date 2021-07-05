using System;

namespace RimWorld
{
	// Token: 0x02001C92 RID: 7314
	[DefOf]
	public static class StoryEventDefOf
	{
		// Token: 0x06009F95 RID: 40853 RVA: 0x0006A652 File Offset: 0x00068852
		static StoryEventDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(StoryEventDefOf));
		}

		// Token: 0x04006C00 RID: 27648
		public static StoryEventDef DamageTaken;

		// Token: 0x04006C01 RID: 27649
		public static StoryEventDef DamageDealt;

		// Token: 0x04006C02 RID: 27650
		public static StoryEventDef AttackedPlayer;

		// Token: 0x04006C03 RID: 27651
		public static StoryEventDef KilledPlayer;

		// Token: 0x04006C04 RID: 27652
		public static StoryEventDef TendedByPlayer;

		// Token: 0x04006C05 RID: 27653
		public static StoryEventDef Seen;

		// Token: 0x04006C06 RID: 27654
		public static StoryEventDef TaleCreated;
	}
}
