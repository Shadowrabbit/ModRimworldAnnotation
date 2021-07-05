using System;

namespace Verse
{
	// Token: 0x020002C1 RID: 705
	public static class HediffMaker
	{
		// Token: 0x0600130E RID: 4878 RVA: 0x0006C8B0 File Offset: 0x0006AAB0
		public static Hediff MakeHediff(HediffDef def, Pawn pawn, BodyPartRecord partRecord = null)
		{
			if (pawn == null)
			{
				Log.Error("Cannot make hediff " + def + " for null pawn.");
				return null;
			}
			Hediff hediff = (Hediff)Activator.CreateInstance(def.hediffClass);
			hediff.def = def;
			hediff.pawn = pawn;
			hediff.Part = partRecord;
			hediff.loadID = Find.UniqueIDsManager.GetNextHediffID();
			hediff.PostMake();
			return hediff;
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x0006C912 File Offset: 0x0006AB12
		public static Hediff Debug_MakeConcreteExampleHediff(HediffDef def)
		{
			Hediff hediff = (Hediff)Activator.CreateInstance(def.hediffClass);
			hediff.def = def;
			hediff.loadID = Find.UniqueIDsManager.GetNextHediffID();
			hediff.PostMake();
			return hediff;
		}
	}
}
