using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000BE5 RID: 3045
	public class GameCondition_ForceWeather : GameCondition
	{
		// Token: 0x060047B7 RID: 18359 RVA: 0x0017B2C3 File Offset: 0x001794C3
		public override void Init()
		{
			base.Init();
			if (this.weather == null)
			{
				this.weather = this.def.weatherDef;
			}
		}

		// Token: 0x060047B8 RID: 18360 RVA: 0x0017B2E4 File Offset: 0x001794E4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<WeatherDef>(ref this.weather, "weather");
		}

		// Token: 0x060047B9 RID: 18361 RVA: 0x0017B2FC File Offset: 0x001794FC
		public override WeatherDef ForcedWeather()
		{
			return this.weather;
		}

		// Token: 0x060047BA RID: 18362 RVA: 0x0017B304 File Offset: 0x00179504
		public override void RandomizeSettings(float points, Map map, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.RandomizeSettings(points, map, outExtraDescriptionRules, outExtraDescriptionConstants);
			this.weather = (from def in DefDatabase<WeatherDef>.AllDefsListForReading
			where def.isBad
			select def).RandomElement<WeatherDef>();
			outExtraDescriptionRules.AddRange(GrammarUtility.RulesForDef("forcedWeather", this.weather));
		}

		// Token: 0x04002C01 RID: 11265
		public WeatherDef weather;
	}
}
