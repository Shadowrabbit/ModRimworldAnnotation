using System;

namespace Verse
{
	// Token: 0x02000314 RID: 788
	public static class ThingIDMaker
	{
		// Token: 0x06001415 RID: 5141 RVA: 0x000CD2DC File Offset: 0x000CB4DC
		public static void GiveIDTo(Thing t)
		{
			if (!t.def.HasThingIDNumber)
			{
				return;
			}
			if (t.thingIDNumber != -1)
			{
				Log.Error(string.Concat(new object[]
				{
					"Giving ID to ",
					t,
					" which already has id ",
					t.thingIDNumber
				}), false);
			}
			t.thingIDNumber = Find.UniqueIDsManager.GetNextThingID();
		}
	}
}
