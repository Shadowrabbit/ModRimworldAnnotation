using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200120E RID: 4622
	public abstract class StorytellerComp
	{
		// Token: 0x06006518 RID: 25880 RVA: 0x00045396 File Offset: 0x00043596
		public virtual IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			yield break;
		}

		// Token: 0x06006519 RID: 25881 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_PawnEvent(Pawn p, AdaptationEvent ev, DamageInfo? dinfo = null)
		{
		}

		// Token: 0x0600651A RID: 25882 RVA: 0x0004539F File Offset: 0x0004359F
		public virtual IncidentParms GenerateParms(IncidentCategoryDef incCat, IIncidentTarget target)
		{
			return StorytellerUtility.DefaultParmsNow(incCat, target);
		}

		// Token: 0x0600651B RID: 25883 RVA: 0x001F5B88 File Offset: 0x001F3D88
		[Obsolete("Use IncidentParms argument instead")]
		protected IEnumerable<IncidentDef> UsableIncidentsInCategory(IncidentCategoryDef cat, IIncidentTarget target)
		{
			return this.UsableIncidentsInCategory(cat, (IncidentDef x) => this.GenerateParms(cat, target));
		}

		// Token: 0x0600651C RID: 25884 RVA: 0x001F5BC8 File Offset: 0x001F3DC8
		protected IEnumerable<IncidentDef> UsableIncidentsInCategory(IncidentCategoryDef cat, IncidentParms parms)
		{
			return this.UsableIncidentsInCategory(cat, (IncidentDef x) => parms);
		}

		// Token: 0x0600651D RID: 25885 RVA: 0x001F5BF8 File Offset: 0x001F3DF8
		protected virtual IEnumerable<IncidentDef> UsableIncidentsInCategory(IncidentCategoryDef cat, Func<IncidentDef, IncidentParms> parmsGetter)
		{
			return from x in DefDatabase<IncidentDef>.AllDefsListForReading
			where x.category == cat && x.Worker.CanFireNow(parmsGetter(x), false)
			select x;
		}

		// Token: 0x0600651E RID: 25886 RVA: 0x001F5C30 File Offset: 0x001F3E30
		protected float IncidentChanceFactor_CurrentPopulation(IncidentDef def)
		{
			if (def.chanceFactorByPopulationCurve == null)
			{
				return 1f;
			}
			int num = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Count<Pawn>();
			return def.chanceFactorByPopulationCurve.Evaluate((float)num);
		}

		// Token: 0x0600651F RID: 25887 RVA: 0x001F5C64 File Offset: 0x001F3E64
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

		// Token: 0x06006520 RID: 25888 RVA: 0x001F5CD4 File Offset: 0x001F3ED4
		protected float IncidentChanceFinal(IncidentDef def)
		{
			float num = def.Worker.BaseChanceThisGame;
			num *= this.IncidentChanceFactor_CurrentPopulation(def);
			num *= this.IncidentChanceFactor_PopulationIntent(def);
			return Mathf.Max(0f, num);
		}

		// Token: 0x06006521 RID: 25889 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Initialize()
		{
		}

		// Token: 0x06006522 RID: 25890 RVA: 0x001F5D0C File Offset: 0x001F3F0C
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
				select x.ToString()).ToCommaList(false) + ")";
			}
			return text;
		}

		// Token: 0x06006523 RID: 25891 RVA: 0x001F5DB0 File Offset: 0x001F3FB0
		public virtual void DebugTablesIncidentChances()
		{
			IEnumerable<IncidentDef> dataSources = from d in DefDatabase<IncidentDef>.AllDefs
			orderby d.category.defName descending, this.IncidentChanceFinal(d) descending
			select d;
			TableDataGetter<IncidentDef>[] array = new TableDataGetter<IncidentDef>[14];
			array[0] = new TableDataGetter<IncidentDef>("defName", (IncidentDef d) => d.defName);
			array[1] = new TableDataGetter<IncidentDef>("category", (IncidentDef d) => d.category);
			array[2] = new TableDataGetter<IncidentDef>("can fire", (IncidentDef d) => StorytellerComp.<DebugTablesIncidentChances>g__CanFireLocal|12_1(d).ToStringCheckBlank());
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
				if (!StorytellerComp.<DebugTablesIncidentChances>g__CanFireLocal|12_1(d))
				{
					return "-";
				}
				return this.IncidentChanceFinal(d).ToString("F2");
			});
			array[8] = new TableDataGetter<IncidentDef>("Factor from:\ncurrent pop", (IncidentDef d) => this.IncidentChanceFactor_CurrentPopulation(d).ToString());
			array[9] = new TableDataGetter<IncidentDef>("Factor from:\npop intent", (IncidentDef d) => this.IncidentChanceFactor_PopulationIntent(d).ToString());
			array[10] = new TableDataGetter<IncidentDef>("default target", delegate(IncidentDef d)
			{
				if (StorytellerComp.<DebugTablesIncidentChances>g__GetDefaultTarget|12_0(d) == null)
				{
					return "-";
				}
				return StorytellerComp.<DebugTablesIncidentChances>g__GetDefaultTarget|12_0(d).ToString();
			});
			array[11] = new TableDataGetter<IncidentDef>("current\npop", (IncidentDef d) => PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Count<Pawn>().ToString());
			array[12] = new TableDataGetter<IncidentDef>("pop\nintent", (IncidentDef d) => StorytellerUtilityPopulation.PopulationIntent.ToString("F2"));
			array[13] = new TableDataGetter<IncidentDef>("cur\npoints", delegate(IncidentDef d)
			{
				if (StorytellerComp.<DebugTablesIncidentChances>g__GetDefaultTarget|12_0(d) == null)
				{
					return "-";
				}
				return StorytellerUtility.DefaultThreatPointsNow(StorytellerComp.<DebugTablesIncidentChances>g__GetDefaultTarget|12_0(d)).ToString("F0");
			});
			DebugTables.MakeTablesDialog<IncidentDef>(dataSources, array);
		}

		// Token: 0x06006525 RID: 25893 RVA: 0x000453A8 File Offset: 0x000435A8
		[CompilerGenerated]
		internal static IIncidentTarget <DebugTablesIncidentChances>g__GetDefaultTarget|12_0(IncidentDef d)
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

		// Token: 0x06006526 RID: 25894 RVA: 0x001F6024 File Offset: 0x001F4224
		[CompilerGenerated]
		internal static bool <DebugTablesIncidentChances>g__CanFireLocal|12_1(IncidentDef d)
		{
			IIncidentTarget incidentTarget = StorytellerComp.<DebugTablesIncidentChances>g__GetDefaultTarget|12_0(d);
			if (incidentTarget == null)
			{
				return false;
			}
			IncidentParms parms = StorytellerUtility.DefaultParmsNow(d.category, incidentTarget);
			return d.Worker.CanFireNow(parms, false);
		}

		// Token: 0x04004350 RID: 17232
		public StorytellerCompProperties props;
	}
}
