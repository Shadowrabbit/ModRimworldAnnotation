using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010EE RID: 4334
	public class CompCauseGameCondition_ForceWeather : CompCauseGameCondition
	{
		// Token: 0x060067C5 RID: 26565 RVA: 0x00231FC7 File Offset: 0x002301C7
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.weather = base.Props.conditionDef.weatherDef;
		}

		// Token: 0x060067C6 RID: 26566 RVA: 0x00231FE6 File Offset: 0x002301E6
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<WeatherDef>(ref this.weather, "weather");
		}

		// Token: 0x060067C7 RID: 26567 RVA: 0x00231FFE File Offset: 0x002301FE
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!Prefs.DevMode)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = this.weather.LabelCap,
				action = delegate()
				{
					List<WeatherDef> allDefsListForReading = DefDatabase<WeatherDef>.AllDefsListForReading;
					int num = allDefsListForReading.FindIndex((WeatherDef w) => w == this.weather);
					num++;
					if (num >= allDefsListForReading.Count)
					{
						num = 0;
					}
					this.weather = allDefsListForReading[num];
					base.ReSetupAllConditions();
				},
				hotKey = KeyBindingDefOf.Misc1
			};
			yield break;
		}

		// Token: 0x060067C8 RID: 26568 RVA: 0x0023200E File Offset: 0x0023020E
		protected override void SetupCondition(GameCondition condition, Map map)
		{
			base.SetupCondition(condition, map);
			((GameCondition_ForceWeather)condition).weather = this.weather;
		}

		// Token: 0x060067C9 RID: 26569 RVA: 0x0023202C File Offset: 0x0023022C
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + "Weather".Translate() + ": " + this.weather.LabelCap;
		}

		// Token: 0x060067CA RID: 26570 RVA: 0x00232083 File Offset: 0x00230283
		public override void RandomizeSettings(Site site)
		{
			this.weather = (from x in DefDatabase<WeatherDef>.AllDefsListForReading
			where x.isBad
			select x).RandomElement<WeatherDef>();
		}

		// Token: 0x04003A6E RID: 14958
		public WeatherDef weather;
	}
}
