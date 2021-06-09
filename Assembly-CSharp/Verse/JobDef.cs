using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000146 RID: 326
	public class JobDef : Def
	{
		// Token: 0x0600087A RID: 2170 RVA: 0x0000CBAD File Offset: 0x0000ADAD
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.joySkill != null && this.joyXpPerTick == 0f)
			{
				yield return "funSkill is not null but funXpPerTick is zero";
			}
			yield break;
			yield break;
		}

		// Token: 0x04000698 RID: 1688
		public Type driverClass;

		// Token: 0x04000699 RID: 1689
		[MustTranslate]
		public string reportString = "Doing something.";

		// Token: 0x0400069A RID: 1690
		public bool playerInterruptible = true;

		// Token: 0x0400069B RID: 1691
		public CheckJobOverrideOnDamageMode checkOverrideOnDamage = CheckJobOverrideOnDamageMode.Always;

		// Token: 0x0400069C RID: 1692
		public bool alwaysShowWeapon;

		// Token: 0x0400069D RID: 1693
		public bool neverShowWeapon;

		// Token: 0x0400069E RID: 1694
		public bool suspendable = true;

		// Token: 0x0400069F RID: 1695
		public bool casualInterruptible = true;

		// Token: 0x040006A0 RID: 1696
		public bool allowOpportunisticPrefix;

		// Token: 0x040006A1 RID: 1697
		public bool collideWithPawns;

		// Token: 0x040006A2 RID: 1698
		public bool isIdle;

		// Token: 0x040006A3 RID: 1699
		public TaleDef taleOnCompletion;

		// Token: 0x040006A4 RID: 1700
		public bool neverFleeFromEnemies;

		// Token: 0x040006A5 RID: 1701
		public bool makeTargetPrisoner;

		// Token: 0x040006A6 RID: 1702
		public int waitAfterArriving;

		// Token: 0x040006A7 RID: 1703
		public int joyDuration = 4000;

		// Token: 0x040006A8 RID: 1704
		public int joyMaxParticipants = 1;

		// Token: 0x040006A9 RID: 1705
		public float joyGainRate = 1f;

		// Token: 0x040006AA RID: 1706
		public SkillDef joySkill;

		// Token: 0x040006AB RID: 1707
		public float joyXpPerTick;

		// Token: 0x040006AC RID: 1708
		public JoyKindDef joyKind;

		// Token: 0x040006AD RID: 1709
		public Rot4 faceDir = Rot4.Invalid;
	}
}
