using System;

namespace Verse
{
	// Token: 0x0200014C RID: 332
	public class ManeuverDef : Def
	{
		// Token: 0x040006CB RID: 1739
		public ToolCapacityDef requiredCapacity;

		// Token: 0x040006CC RID: 1740
		public VerbProperties verb;

		// Token: 0x040006CD RID: 1741
		public RulePackDef combatLogRulesHit;

		// Token: 0x040006CE RID: 1742
		public RulePackDef combatLogRulesDeflect;

		// Token: 0x040006CF RID: 1743
		public RulePackDef combatLogRulesMiss;

		// Token: 0x040006D0 RID: 1744
		public RulePackDef combatLogRulesDodge;

		// Token: 0x040006D1 RID: 1745
		public LogEntryDef logEntryDef;
	}
}
