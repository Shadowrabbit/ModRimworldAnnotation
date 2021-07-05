using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001029 RID: 4137
	public static class TaleFactory
	{
		// Token: 0x060061AD RID: 25005 RVA: 0x00212E34 File Offset: 0x00211034
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
				select arg.ToStringSafe<object>()).ToCommaList(false, false), arg2));
				result = null;
			}
			return result;
		}

		// Token: 0x060061AE RID: 25006 RVA: 0x00212ED0 File Offset: 0x002110D0
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
