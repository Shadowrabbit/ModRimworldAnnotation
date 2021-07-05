using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001494 RID: 5268
	public static class NativeVerbPropertiesDatabase
	{
		// Token: 0x06007DE9 RID: 32233 RVA: 0x002C9AF0 File Offset: 0x002C7CF0
		public static VerbProperties VerbWithCategory(VerbCategory id)
		{
			VerbProperties verbProperties = (from v in NativeVerbPropertiesDatabase.allVerbDefs
			where v.category == id
			select v).FirstOrDefault<VerbProperties>();
			if (verbProperties == null)
			{
				Log.Error("Failed to find Verb with id " + id);
			}
			return verbProperties;
		}

		// Token: 0x04004E7A RID: 20090
		public static List<VerbProperties> allVerbDefs = VerbDefsHardcodedNative.AllVerbDefs().ToList<VerbProperties>();
	}
}
