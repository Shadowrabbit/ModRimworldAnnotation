using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020000D6 RID: 214
	public class JobDef : Def
	{
		// Token: 0x06000616 RID: 1558 RVA: 0x0001E9BC File Offset: 0x0001CBBC
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

		// Token: 0x0400049F RID: 1183
		public Type driverClass;

		// Token: 0x040004A0 RID: 1184
		[MustTranslate]
		public string reportString = "Doing something.";

		// Token: 0x040004A1 RID: 1185
		public bool playerInterruptible = true;

		// Token: 0x040004A2 RID: 1186
		public CheckJobOverrideOnDamageMode checkOverrideOnDamage = CheckJobOverrideOnDamageMode.Always;

		// Token: 0x040004A3 RID: 1187
		public bool alwaysShowWeapon;

		// Token: 0x040004A4 RID: 1188
		public bool neverShowWeapon;

		// Token: 0x040004A5 RID: 1189
		public bool suspendable = true;

		// Token: 0x040004A6 RID: 1190
		public bool casualInterruptible = true;

		// Token: 0x040004A7 RID: 1191
		public bool allowOpportunisticPrefix;

		// Token: 0x040004A8 RID: 1192
		public bool collideWithPawns;

		// Token: 0x040004A9 RID: 1193
		public bool isIdle;

		// Token: 0x040004AA RID: 1194
		public TaleDef taleOnCompletion;

		// Token: 0x040004AB RID: 1195
		public bool neverFleeFromEnemies;

		// Token: 0x040004AC RID: 1196
		public bool makeTargetPrisoner;

		// Token: 0x040004AD RID: 1197
		public int waitAfterArriving;

		// Token: 0x040004AE RID: 1198
		public bool carryThingAfterJob;

		// Token: 0x040004AF RID: 1199
		public bool dropThingBeforeJob = true;

		// Token: 0x040004B0 RID: 1200
		public int joyDuration = 4000;

		// Token: 0x040004B1 RID: 1201
		public int joyMaxParticipants = 1;

		// Token: 0x040004B2 RID: 1202
		public float joyGainRate = 1f;

		// Token: 0x040004B3 RID: 1203
		public SkillDef joySkill;

		// Token: 0x040004B4 RID: 1204
		public float joyXpPerTick;

		// Token: 0x040004B5 RID: 1205
		public JoyKindDef joyKind;

		// Token: 0x040004B6 RID: 1206
		public Rot4 faceDir = Rot4.Invalid;
	}
}
