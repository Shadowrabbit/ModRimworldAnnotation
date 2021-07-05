using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C35 RID: 3125
	public abstract class StorytellerComp
	{
		// Token: 0x0600495F RID: 18783 RVA: 0x00184B48 File Offset: 0x00182D48
		public virtual IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			yield break;
		}

		// Token: 0x06004960 RID: 18784 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_PawnEvent(Pawn p, AdaptationEvent ev, DamageInfo? dinfo = null)
		{
		}

		// Token: 0x06004961 RID: 18785 RVA: 0x00184B51 File Offset: 0x00182D51
		public virtual IncidentParms GenerateParms(IncidentCategoryDef incCat, IIncidentTarget target)
		{
			return StorytellerUtility.DefaultParmsNow(incCat, target);
		}

		// Token: 0x06004962 RID: 18786 RVA: 0x00184B5C File Offset: 0x00182D5C
		protected IEnumerable<IncidentDef> UsableIncidentsInCategory(IncidentCategoryDef cat, IncidentParms parms)
		{
			return this.UsableIncidentsInCategory(cat, (IncidentDef x) => parms);
		}

		// Token: 0x06004963 RID: 18787 RVA: 0x00184B8C File Offset: 0x00182D8C
		protected virtual IEnumerable<IncidentDef> UsableIncidentsInCategory(IncidentCategoryDef cat, Func<IncidentDef, IncidentParms> parmsGetter)
		{
			return from x in DefDatabase<IncidentDef>.AllDefsListForReading
			where x.category == cat && x.Worker.CanFireNow(parmsGetter(x))
			select x;
		}

		// Token: 0x06004964 RID: 18788 RVA: 0x00184BC4 File Offset: 0x00182DC4
		protected float IncidentChanceFactor_CurrentPopulation(IncidentDef def)
		{
			if (def.chanceFactorByPopulationCurve == null)
			{
				return 1f;
			}
			int num = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Count<Pawn>();
			return def.chanceFactorByPopulationCurve.Evaluate((float)num);
		}

		// Token: 0x06004965 RID: 18789 RVA: 0x00184BF8 File Offset: 0x00182DF8
		protected float IncidentChanceFactor_PopulationIntent(IncidentDef def)
		{
			if (def.populationEffect == IncidentPopulationEffect.None)
			{
				return 1f;
			}
			float num;
			switch (def.populationEffect)
			{
			case IncidentPopulationEffect.IncreaseHard:
				num = 0.4f;
				break;
			case IncidentPopulationEffect.IncreaseMedium:
				num = 0f;
				break;
			case IncidentPopulationEffect.IncreaseEasy:
				num = -0.4f;
				break;
			default:
				throw new Exception();
			}
			return Mathf.Max(StorytellerUtilityPopulation.PopulationIntent + num, this.props.minIncChancePopulationIntentFactor);
		}

		// Token: 0x06004966 RID: 18790 RVA: 0x00184C68 File Offset: 0x00182E68
		protected bool TrySelectRandomIncident(IEnumerable<IncidentDef> incidents, out IncidentDef foundDef)
		{
			bool flag = Rand.Chance(StorytellerComp.IncreasesPopChanceByPopIntentCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntent));
			foundDef = null;
			if (flag)
			{
				foundDef = (from i in incidents
				where i.populationEffect > IncidentPopulationEffect.None
				select i).RandomElementByWeightWithFallback((IncidentDef i) => this.IncidentChanceFinal(i), null);
			}
			else
			{
				foundDef = (from i in incidents
				where i.populationEffect == IncidentPopulationEffect.None
				select i).RandomElementByWeightWithFallback((IncidentDef i) => this.IncidentChanceFinal(i), null);
			}
			if (foundDef == null)
			{
				foundDef = incidents.RandomElementByWeightWithFallback((IncidentDef i) => this.IncidentChanceFinal(i), null);
			}
			return foundDef != null;
		}

		// Token: 0x06004967 RID: 18791 RVA: 0x00184D20 File Offset: 0x00182F20
		protected float IncidentChanceFinal(IncidentDef def)
		{
			float num = def.Worker.BaseChanceThisGame;
			num *= this.IncidentChanceFactor_CurrentPopulation(def);
			num *= this.IncidentChanceFactor_PopulationIntent(def);
			return Mathf.Max(0f, num);
		}

		// Token: 0x06004968 RID: 18792 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Initialize()
		{
		}

		// Token: 0x06004969 RID: 18793 RVA: 0x00184D58 File Offset: 0x00182F58
		public override string ToString()
		{
			string text = base.GetType().Name;
			string text2 = typeof(StorytellerComp).Name + "_";
			if (text.StartsWith(text2))
			{
				text = text.Substring(text2.Length);
			}
			if (!this.props.allowedTargetTags.NullOrEmpty<IncidentTargetTagDef>())
			{
				text = text + " (" + (from x in this.props.allowedTargetTags
				select x.ToString()).ToCommaList(false, false) + ")";
			}
			return text;
		}

		// Token: 0x0600496A RID: 18794 RVA: 0x00184DFC File Offset: 0x00182FFC
		public virtual void DebugTablesIncidentChances()
		{
			IEnumerable<IncidentDef> dataSources = from d in DefDatabase<IncidentDef>.AllDefs
			orderby d.category.defName descending, this.IncidentChanceFinal(d) descending
			select d;
			TableDataGetter<IncidentDef>[] array = new TableDataGetter<IncidentDef>[14];
			array[0] = new TableDataGetter<IncidentDef>("defName", (IncidentDef d) => d.defName);
			array[1] = new TableDataGetter<IncidentDef>("category", (IncidentDef d) => d.category);
			array[2] = new TableDataGetter<IncidentDef>("can fire", (IncidentDef d) => StorytellerComp.<DebugTablesIncidentChances>g__CanFireLocal|13_1(d).ToStringCheckBlank());
			array[3] = new TableDataGetter<IncidentDef>("base\nchance", (IncidentDef d) => d.baseChance.ToString("F2"));
			array[4] = new TableDataGetter<IncidentDef>("base\nchance\nwith\nRoyalty", delegate(IncidentDef d)
			{
				if (d.baseChanceWithRoyalty < 0f)
				{
					return "-";
				}
				return d.baseChanceWithRoyalty.ToString("F2");
			});
			array[5] = new TableDataGetter<IncidentDef>("base\nchance\nthis\ngame", (IncidentDef d) => d.Worker.BaseChanceThisGame.ToString("F2"));
			array[6] = new TableDataGetter<IncidentDef>("final\nchance", (IncidentDef d) => this.IncidentChanceFinal(d).ToString("F2"));
			array[7] = new TableDataGetter<IncidentDef>("final\nchance\npossible", delegate(IncidentDef d)
			{
				if (!StorytellerComp.<DebugTablesIncidentChances>g__CanFireLocal|13_1(d))
				{
					return "-";
				}
				return this.IncidentChanceFinal(d).ToString("F2");
			});
			array[8] = new TableDataGetter<IncidentDef>("Factor from:\ncurrent pop", (IncidentDef d) => this.IncidentChanceFactor_CurrentPopulation(d).ToString());
			array[9] = new TableDataGetter<IncidentDef>("Factor from:\npop intent", (IncidentDef d) => this.IncidentChanceFactor_PopulationIntent(d).ToString());
			array[10] = new TableDataGetter<IncidentDef>("default target", delegate(IncidentDef d)
			{
				if (StorytellerComp.<DebugTablesIncidentChances>g__GetDefaultTarget|13_0(d) == null)
				{
					return "-";
				}
				return StorytellerComp.<DebugTablesIncidentChances>g__GetDefaultTarget|13_0(d).ToString();
			});
			array[11] = new TableDataGetter<IncidentDef>("current\npop", (IncidentDef d) => PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Count<Pawn>().ToString());
			array[12] = new TableDataGetter<IncidentDef>("pop\nintent", (IncidentDef d) => StorytellerUtilityPopulation.PopulationIntent.ToString("F2"));
			array[13] = new TableDataGetter<IncidentDef>("cur\npoints", delegate(IncidentDef d)
			{
				if (StorytellerComp.<DebugTablesIncidentChances>g__GetDefaultTarget|13_0(d) == null)
				{
					return "-";
				}
				return StorytellerUtility.DefaultThreatPointsNow(StorytellerComp.<DebugTablesIncidentChances>g__GetDefaultTarget|13_0(d)).ToString("F0");
			});
			DebugTables.MakeTablesDialog<IncidentDef>(dataSources, array);
		}

		// Token: 0x06004970 RID: 18800 RVA: 0x001850D2 File Offset: 0x001832D2
		[CompilerGenerated]
		internal static IIncidentTarget <DebugTablesIncidentChances>g__GetDefaultTarget|13_0(IncidentDef d)
		{
			if (d.TargetAllowed(Find.CurrentMap))
			{
				return Find.CurrentMap;
			}
			if (d.TargetAllowed(Find.World))
			{
				return Find.World;
			}
			return null;
		}

		// Token: 0x06004971 RID: 18801 RVA: 0x001850FC File Offset: 0x001832FC
		[CompilerGenerated]
		internal static bool <DebugTablesIncidentChances>g__CanFireLocal|13_1(IncidentDef d)
		{
			IIncidentTarget incidentTarget = StorytellerComp.<DebugTablesIncidentChances>g__GetDefaultTarget|13_0(d);
			if (incidentTarget == null)
			{
				return false;
			}
			IncidentParms parms = StorytellerUtility.DefaultParmsNow(d.category, incidentTarget);
			return d.Worker.CanFireNow(parms);
		}

		// Token: 0x04002CA5 RID: 11429
		public StorytellerCompProperties props;

		// Token: 0x04002CA6 RID: 11430
		public static readonly SimpleCurve IncreasesPopChanceByPopIntentCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.05f),
				true
			},
			{
				new CurvePoint(1f, 0.3f),
				true
			},
			{
				new CurvePoint(3f, 0.45f),
				true
			}
		};
	}
}
