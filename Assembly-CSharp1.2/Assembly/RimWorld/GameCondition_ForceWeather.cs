using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200117D RID: 4477
	public class GameCondition_ForceWeather : GameCondition
	{
		// Token: 0x060062BD RID: 25277 RVA: 0x00043ED3 File Offset: 0x000420D3
		public override void Init()
		{
			base.Init();
			if (this.weather == null)
			{
				this.weather = this.def.weatherDef;
			}
		}

		// Token: 0x060062BE RID: 25278 RVA: 0x00043EF4 File Offset: 0x000420F4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<WeatherDef>(ref this.weather, "weather");
		}

		// Token: 0x060062BF RID: 25279 RVA: 0x00043F0C File Offset: 0x0004210C
		public override WeatherDef ForcedWeather()
		{
			return this.weather;
		}

		// Token: 0x060062C0 RID: 25280 RVA: 0x001ED060 File Offset: 0x001EB260
		public override void RandomizeSettings(float points, Map map, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.RandomizeSettings(points, map, outExtraDescriptionRules, outExtraDescriptionConstants);
			this.weather = (from def in DefDatabase<WeatherDef>.AllDefsListForReading
			where def.isBad
			select def).RandomElement<WeatherDef>();
			outExtraDescriptionRules.AddRange(GrammarUtility.RulesForDef("forcedWeather", this.weather));
		}

		// Token: 0x0400421C RID: 16924
		public WeatherDef weather;
	}
}
