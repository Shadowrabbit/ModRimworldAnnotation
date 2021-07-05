using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020000C6 RID: 198
	public class GameConditionDef : Def
	{
		// Token: 0x060005EA RID: 1514 RVA: 0x0001E3E9 File Offset: 0x0001C5E9
		public bool CanCoexistWith(GameConditionDef other)
		{
			return this != other && (this.exclusiveConditions == null || !this.exclusiveConditions.Contains(other));
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x0001E40A File Offset: 0x0001C60A
		public static GameConditionDef Named(string defName)
		{
			return DefDatabase<GameConditionDef>.GetNamed(defName, true);
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0001E413 File Offset: 0x0001C613
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.conditionClass == null)
			{
				yield return "conditionClass is null";
			}
			yield break;
			yield break;
		}

		// Token: 0x04000418 RID: 1048
		public Type conditionClass = typeof(GameCondition);

		// Token: 0x04000419 RID: 1049
		private List<GameConditionDef> exclusiveConditions;

		// Token: 0x0400041A RID: 1050
		[MustTranslate]
		public string endMessage;

		// Token: 0x0400041B RID: 1051
		[MustTranslate]
		public string letterText;

		// Token: 0x0400041C RID: 1052
		public List<ThingDef> letterHyperlinks;

		// Token: 0x0400041D RID: 1053
		public LetterDef letterDef;

		// Token: 0x0400041E RID: 1054
		public bool canBePermanent;

		// Token: 0x0400041F RID: 1055
		[MustTranslate]
		public string descriptionFuture;

		// Token: 0x04000420 RID: 1056
		[NoTranslate]
		public string jumpToSourceKey = "ClickToJumpToSource";

		// Token: 0x04000421 RID: 1057
		public PsychicDroneLevel defaultDroneLevel = PsychicDroneLevel.BadMedium;

		// Token: 0x04000422 RID: 1058
		public bool preventRain;

		// Token: 0x04000423 RID: 1059
		public WeatherDef weatherDef;

		// Token: 0x04000424 RID: 1060
		public float temperatureOffset = -10f;
	}
}
