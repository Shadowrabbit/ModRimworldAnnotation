using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200163E RID: 5694
	public static class TaleFactory
	{
		// Token: 0x06007BCC RID: 31692 RVA: 0x002523BC File Offset: 0x002505BC
		public static Tale MakeRawTale(TaleDef def, params object[] args)
		{
			Tale result;
			try
			{
				Tale tale = (Tale)Activator.CreateInstance(def.taleClass, args);
				tale.def = def;
				tale.id = Find.UniqueIDsManager.GetNextTaleID();
				tale.date = Find.TickManager.TicksAbs;
				result = tale;
			}
			catch (Exception arg)
			{
				Exception arg2;
				Log.Error(string.Format("Failed to create tale object {0} with parameters {1}: {2}", def, (from arg in args
				select arg.ToStringSafe<object>()).ToCommaList(false), arg2), false);
				result = null;
			}
			return result;
		}

		// Token: 0x06007BCD RID: 31693 RVA: 0x00252458 File Offset: 0x00250658
		public static Tale MakeRandomTestTale(TaleDef def = null)
		{
			if (def == null)
			{
				def = (from d in DefDatabase<TaleDef>.AllDefs
				where d.usableForArt
				select d).RandomElement<TaleDef>();
			}
			Tale tale = TaleFactory.MakeRawTale(def, Array.Empty<object>());
			tale.GenerateTestData();
			return tale;
		}
	}
}
