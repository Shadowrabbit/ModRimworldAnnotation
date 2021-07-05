using System;

namespace Verse
{
	// Token: 0x020000DB RID: 219
	public class ManeuverDef : Def
	{
		// Token: 0x040004CF RID: 1231
		public ToolCapacityDef requiredCapacity;

		// Token: 0x040004D0 RID: 1232
		public VerbProperties verb;

		// Token: 0x040004D1 RID: 1233
		public RulePackDef combatLogRulesHit;

		// Token: 0x040004D2 RID: 1234
		public RulePackDef combatLogRulesDeflect;

		// Token: 0x040004D3 RID: 1235
		public RulePackDef combatLogRulesMiss;

		// Token: 0x040004D4 RID: 1236
		public RulePackDef combatLogRulesDodge;

		// Token: 0x040004D5 RID: 1237
		public LogEntryDef logEntryDef;
	}
}
