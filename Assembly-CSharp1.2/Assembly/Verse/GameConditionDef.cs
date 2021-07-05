using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000132 RID: 306
	public class GameConditionDef : Def
	{
		// Token: 0x06000829 RID: 2089 RVA: 0x0000C867 File Offset: 0x0000AA67
		public bool CanCoexistWith(GameConditionDef other)
		{
			return this != other && (this.exclusiveConditions == null || !this.exclusiveConditions.Contains(other));
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x0000C888 File Offset: 0x0000AA88
		public static GameConditionDef Named(string defName)
		{
			return DefDatabase<GameConditionDef>.GetNamed(defName, true);
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x0000C891 File Offset: 0x0000AA91
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

		// Token: 0x04000601 RID: 1537
		public Type conditionClass = typeof(GameCondition);

		// Token: 0x04000602 RID: 1538
		private List<GameConditionDef> exclusiveConditions;

		// Token: 0x04000603 RID: 1539
		[MustTranslate]
		public string endMessage;

		// Token: 0x04000604 RID: 1540
		[MustTranslate]
		public string letterText;

		// Token: 0x04000605 RID: 1541
		public List<ThingDef> letterHyperlinks;

		// Token: 0x04000606 RID: 1542
		public LetterDef letterDef;

		// Token: 0x04000607 RID: 1543
		public bool canBePermanent;

		// Token: 0x04000608 RID: 1544
		[MustTranslate]
		public string descriptionFuture;

		// Token: 0x04000609 RID: 1545
		[NoTranslate]
		public string jumpToSourceKey = "ClickToJumpToSource";

		// Token: 0x0400060A RID: 1546
		public PsychicDroneLevel defaultDroneLevel = PsychicDroneLevel.BadMedium;

		// Token: 0x0400060B RID: 1547
		public bool preventRain;

		// Token: 0x0400060C RID: 1548
		public WeatherDef weatherDef;

		// Token: 0x0400060D RID: 1549
		public float temperatureOffset = -10f;
	}
}
