using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200177A RID: 6010
	public class CompCauseGameCondition_ForceWeather : CompCauseGameCondition
	{
		// Token: 0x06008483 RID: 33923 RVA: 0x00058BE8 File Offset: 0x00056DE8
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.weather = base.Props.conditionDef.weatherDef;
		}

		// Token: 0x06008484 RID: 33924 RVA: 0x00058C07 File Offset: 0x00056E07
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<WeatherDef>(ref this.weather, "weather");
		}

		// Token: 0x06008485 RID: 33925 RVA: 0x00058C1F File Offset: 0x00056E1F
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

		// Token: 0x06008486 RID: 33926 RVA: 0x00058C2F File Offset: 0x00056E2F
		protected override void SetupCondition(GameCondition condition, Map map)
		{
			base.SetupCondition(condition, map);
			((GameCondition_ForceWeather)condition).weather = this.weather;
		}

		// Token: 0x06008487 RID: 33927 RVA: 0x00273FCC File Offset: 0x002721CC
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + "Weather".Translate() + ": " + this.weather.LabelCap;
		}

		// Token: 0x06008488 RID: 33928 RVA: 0x00058C4A File Offset: 0x00056E4A
		public override void RandomizeSettings_NewTemp_NewTemp(Site site)
		{
			this.weather = (from x in DefDatabase<WeatherDef>.AllDefsListForReading
			where x.isBad
			select x).RandomElement<WeatherDef>();
		}

		// Token: 0x040055D8 RID: 21976
		public WeatherDef weather;
	}
}
