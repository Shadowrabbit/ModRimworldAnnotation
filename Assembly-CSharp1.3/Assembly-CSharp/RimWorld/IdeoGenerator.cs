using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ED4 RID: 3796
	public static class IdeoGenerator
	{
		// Token: 0x060059F1 RID: 23025 RVA: 0x001ED66E File Offset: 0x001EB86E
		public static Ideo GenerateIdeo(IdeoGenerationParms parms)
		{
			Ideo ideo = IdeoGenerator.MakeIdeo(DefDatabase<IdeoFoundationDef>.AllDefs.RandomElement<IdeoFoundationDef>());
			ideo.foundation.Init(parms);
			return ideo;
		}

		// Token: 0x060059F2 RID: 23026 RVA: 0x001ED68C File Offset: 0x001EB88C
		public static Ideo MakeIdeo(IdeoFoundationDef foundationDef)
		{
			Ideo ideo = new Ideo();
			ideo.id = Find.UniqueIDsManager.GetNextIdeoID();
			ideo.foundation = (IdeoFoundation)Activator.CreateInstance(foundationDef.foundationClass);
			ideo.foundation.def = foundationDef;
			ideo.foundation.ideo = ideo;
			return ideo;
		}

		// Token: 0x060059F3 RID: 23027 RVA: 0x001ED6E0 File Offset: 0x001EB8E0
		public static Ideo GenerateNoExpansionIdeo(CultureDef culture, IdeoGenerationParms genParms)
		{
			IdeoGenerator.<>c__DisplayClass2_0 CS$<>8__locals1;
			CS$<>8__locals1.culture = culture;
			Ideo ideo = new Ideo();
			ideo.id = Find.UniqueIDsManager.GetNextIdeoID();
			ideo.createdFromNoExpansionGame = true;
			ideo.culture = CS$<>8__locals1.culture;
			ideo.name = IdeoGenerator.<GenerateNoExpansionIdeo>g__ComputeIdeoName|2_0(ref CS$<>8__locals1);
			if (ModsConfig.IdeologyActive)
			{
				ideo.SetIcon(IdeoFoundation.GetRandomIconDef(ideo), IdeoFoundation.GetRandomColorDef(ideo));
			}
			List<PreceptDef> allDefsListForReading = DefDatabase<PreceptDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if ((genParms.disallowedPrecepts == null || !genParms.disallowedPrecepts.Contains(allDefsListForReading[i])) && allDefsListForReading[i].classic)
				{
					Precept precept = PreceptMaker.MakePrecept(allDefsListForReading[i]);
					ideo.AddPrecept(precept, true, null, precept.def.ritualPatternBase);
				}
			}
			return ideo;
		}

		// Token: 0x060059F4 RID: 23028 RVA: 0x001ED7B0 File Offset: 0x001EB9B0
		public static Ideo GenerateTutorialIdeo()
		{
			IdeoGenerationParms parms = new IdeoGenerationParms(Find.Scenario.playerFaction.factionDef, false, null, null);
			parms.disallowedPrecepts = (from x in DefDatabase<PreceptDef>.AllDefs
			where x.impact == PreceptImpact.High
			select x).ToList<PreceptDef>();
			parms.disallowedMemes = (from x in DefDatabase<MemeDef>.AllDefs
			where !x.allowDuringTutorial
			select x).ToList<MemeDef>();
			return IdeoGenerator.GenerateIdeo(parms);
		}

		// Token: 0x060059F5 RID: 23029 RVA: 0x001ED848 File Offset: 0x001EBA48
		[CompilerGenerated]
		internal static TaggedString <GenerateNoExpansionIdeo>g__ComputeIdeoName|2_0(ref IdeoGenerator.<>c__DisplayClass2_0 A_0)
		{
			int num = 1;
			string text;
			for (;;)
			{
				text = A_0.culture.LabelCap;
				if (num > 1)
				{
					text = text + " " + num;
				}
				if (!IdeoGenerator.<GenerateNoExpansionIdeo>g__NameUsed|2_1(text))
				{
					break;
				}
				num++;
			}
			return text;
		}

		// Token: 0x060059F6 RID: 23030 RVA: 0x001ED894 File Offset: 0x001EBA94
		[CompilerGenerated]
		internal static bool <GenerateNoExpansionIdeo>g__NameUsed|2_1(string name)
		{
			using (List<Ideo>.Enumerator enumerator = Find.IdeoManager.IdeosListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.name == name)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
