using System;

namespace Verse
{
	// Token: 0x02000223 RID: 547
	public static class ThingIDMaker
	{
		// Token: 0x06000F8F RID: 3983 RVA: 0x00058604 File Offset: 0x00056804
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
				}));
			}
			t.thingIDNumber = Find.UniqueIDsManager.GetNextThingID();
		}
	}
}
