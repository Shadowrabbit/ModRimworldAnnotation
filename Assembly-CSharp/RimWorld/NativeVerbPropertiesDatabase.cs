using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CD5 RID: 7381
	public static class NativeVerbPropertiesDatabase
	{
		// Token: 0x0600A06B RID: 41067 RVA: 0x002EEE84 File Offset: 0x002ED084
		public static VerbProperties VerbWithCategory(VerbCategory id)
		{
			VerbProperties verbProperties = (from v in NativeVerbPropertiesDatabase.allVerbDefs
			where v.category == id
			select v).FirstOrDefault<VerbProperties>();
			if (verbProperties == null)
			{
				Log.Error("Failed to find Verb with id " + id, false);
			}
			return verbProperties;
		}

		// Token: 0x04006CFA RID: 27898
		public static List<VerbProperties> allVerbDefs = VerbDefsHardcodedNative.AllVerbDefs().ToList<VerbProperties>();
	}
}
