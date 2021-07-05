using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000374 RID: 884
	public static class ThingMaker
	{
		// Token: 0x06001970 RID: 6512 RVA: 0x00095CE8 File Offset: 0x00093EE8
		public static Thing MakeThing(ThingDef def, ThingDef stuff = null)
		{
			if (stuff != null && !stuff.IsStuff)
			{
				Log.Error(string.Concat(new object[]
				{
					"MakeThing error: Tried to make ",
					def,
					" from ",
					stuff,
					" which is not a stuff. Assigning default."
				}));
				stuff = GenStuff.DefaultStuffFor(def);
			}
			if (def.MadeFromStuff && stuff == null)
			{
				Log.Error("MakeThing error: " + def + " is madeFromStuff but stuff=null. Assigning default.");
				stuff = GenStuff.DefaultStuffFor(def);
			}
			if (!def.MadeFromStuff && stuff != null)
			{
				Log.Error(string.Concat(new object[]
				{
					"MakeThing error: ",
					def,
					" is not madeFromStuff but stuff=",
					stuff,
					". Setting to null."
				}));
				stuff = null;
			}
			Thing thing = (Thing)Activator.CreateInstance(def.thingClass);
			thing.def = def;
			thing.SetStuffDirect(stuff);
			thing.PostMake();
			thing.PostPostMake();
			return thing;
		}
	}
}
