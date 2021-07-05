using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E84 RID: 3716
	public class SkillRecord : IExposable
	{
		// Token: 0x17000F1F RID: 3871
		// (get) Token: 0x06005702 RID: 22274 RVA: 0x001D87A8 File Offset: 0x001D69A8
		// (set) Token: 0x06005703 RID: 22275 RVA: 0x001D87BA File Offset: 0x001D69BA
		public int Level
		{
			get
			{
				if (this.TotallyDisabled)
				{
					return 0;
				}
				return this.levelInt;
			}
			set
			{
				this.levelInt = Mathf.Clamp(value, 0, 20);
			}
		}

		// Token: 0x17000F20 RID: 3872
		// (get) Token: 0x06005704 RID: 22276 RVA: 0x001D87CB File Offset: 0x001D69CB
		public float XpRequiredForLevelUp
		{
			get
			{
				return SkillRecord.XpRequiredToLevelUpFrom(this.levelInt);
			}
		}

		// Token: 0x17000F21 RID: 3873
		// (get) Token: 0x06005705 RID: 22277 RVA: 0x001D87D8 File Offset: 0x001D69D8
		public float XpProgressPercent
		{
			get
			{
				return this.xpSinceLastLevel / this.XpRequiredForLevelUp;
			}
		}

		// Token: 0x17000F22 RID: 3874
		// (get) Token: 0x06005706 RID: 22278 RVA: 0x001D87E8 File Offset: 0x001D69E8
		public float XpTotalEarned
		{
			get
			{
				float num = 0f;
				for (int i = 0; i < this.levelInt; i++)
				{
					num += SkillRecord.XpRequiredToLevelUpFrom(i);
				}
				return num;
			}
		}

		// Token: 0x17000F23 RID: 3875
		// (get) Token: 0x06005707 RID: 22279 RVA: 0x001D8816 File Offset: 0x001D6A16
		public bool TotallyDisabled
		{
			get
			{
				if (this.cachedTotallyDisabled == BoolUnknown.Unknown)
				{
					this.cachedTotallyDisabled = (this.CalculateTotallyDisabled() ? BoolUnknown.True : BoolUnknown.False);
				}
				return this.cachedTotallyDisabled == BoolUnknown.True;
			}
		}

		// Token: 0x17000F24 RID: 3876
		// (get) Token: 0x06005708 RID: 22280 RVA: 0x001D883C File Offset: 0x001D6A3C
		public string LevelDescriptor
		{
			get
			{
				switch (this.levelInt)
				{
				case 0:
					return "Skill0".Translate();
				case 1:
					return "Skill1".Translate();
				case 2:
					return "Skill2".Translate();
				case 3:
					return "Skill3".Translate();
				case 4:
					return "Skill4".Translate();
				case 5:
					return "Skill5".Translate();
				case 6:
					return "Skill6".Translate();
				case 7:
					return "Skill7".Translate();
				case 8:
					return "Skill8".Translate();
				case 9:
					return "Skill9".Translate();
				case 10:
					return "Skill10".Translate();
				case 11:
					return "Skill11".Translate();
				case 12:
					return "Skill12".Translate();
				case 13:
					return "Skill13".Translate();
				case 14:
					return "Skill14".Translate();
				case 15:
					return "Skill15".Translate();
				case 16:
					return "Skill16".Translate();
				case 17:
					return "Skill17".Translate();
				case 18:
					return "Skill18".Translate();
				case 19:
					return "Skill19".Translate();
				case 20:
					return "Skill20".Translate();
				default:
					return "Unknown";
				}
			}
		}

		// Token: 0x17000F25 RID: 3877
		// (get) Token: 0x06005709 RID: 22281 RVA: 0x001D8A04 File Offset: 0x001D6C04
		public bool LearningSaturatedToday
		{
			get
			{
				return this.xpSinceMidnight > 4000f;
			}
		}

		// Token: 0x0600570A RID: 22282 RVA: 0x001D8A13 File Offset: 0x001D6C13
		public SkillRecord()
		{
		}

		// Token: 0x0600570B RID: 22283 RVA: 0x001D8A22 File Offset: 0x001D6C22
		public SkillRecord(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600570C RID: 22284 RVA: 0x001D8A38 File Offset: 0x001D6C38
		public SkillRecord(Pawn pawn, SkillDef def)
		{
			this.pawn = pawn;
			this.def = def;
		}

		// Token: 0x0600570D RID: 22285 RVA: 0x001D8A58 File Offset: 0x001D6C58
		public void ExposeData()
		{
			Scribe_Defs.Look<SkillDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.levelInt, "level", 0, false);
			Scribe_Values.Look<float>(ref this.xpSinceLastLevel, "xpSinceLastLevel", 0f, false);
			Scribe_Values.Look<Passion>(ref this.passion, "passion", Passion.None, false);
			Scribe_Values.Look<float>(ref this.xpSinceMidnight, "xpSinceMidnight", 0f, false);
		}

		// Token: 0x0600570E RID: 22286 RVA: 0x001D8AC8 File Offset: 0x001D6CC8
		public void Interval()
		{
			float num = this.pawn.story.traits.HasTrait(TraitDefOf.GreatMemory) ? 0.5f : 1f;
			switch (this.levelInt)
			{
			case 10:
				this.Learn(-0.1f * num, false);
				return;
			case 11:
				this.Learn(-0.2f * num, false);
				return;
			case 12:
				this.Learn(-0.4f * num, false);
				return;
			case 13:
				this.Learn(-0.6f * num, false);
				return;
			case 14:
				this.Learn(-1f * num, false);
				return;
			case 15:
				this.Learn(-1.8f * num, false);
				return;
			case 16:
				this.Learn(-2.8f * num, false);
				return;
			case 17:
				this.Learn(-4f * num, false);
				return;
			case 18:
				this.Learn(-6f * num, false);
				return;
			case 19:
				this.Learn(-8f * num, false);
				return;
			case 20:
				this.Learn(-12f * num, false);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600570F RID: 22287 RVA: 0x001D8BDF File Offset: 0x001D6DDF
		public static float XpRequiredToLevelUpFrom(int startingLevel)
		{
			return SkillRecord.XpForLevelUpCurve.Evaluate((float)startingLevel);
		}

		// Token: 0x06005710 RID: 22288 RVA: 0x001D8BF0 File Offset: 0x001D6DF0
		public void Learn(float xp, bool direct = false)
		{
			if (this.TotallyDisabled)
			{
				return;
			}
			if (xp < 0f && this.levelInt == 0)
			{
				return;
			}
			bool flag = false;
			if (xp > 0f)
			{
				xp *= this.LearnRateFactor(direct);
			}
			this.xpSinceLastLevel += xp;
			if (!direct)
			{
				this.xpSinceMidnight += xp;
			}
			if (this.levelInt == 20 && this.xpSinceLastLevel > this.XpRequiredForLevelUp - 1f)
			{
				this.xpSinceLastLevel = this.XpRequiredForLevelUp - 1f;
			}
			while (this.xpSinceLastLevel >= this.XpRequiredForLevelUp)
			{
				this.xpSinceLastLevel -= this.XpRequiredForLevelUp;
				this.levelInt++;
				flag = true;
				if (this.levelInt == 14)
				{
					if (this.passion == Passion.None)
					{
						TaleRecorder.RecordTale(TaleDefOf.GainedMasterSkillWithoutPassion, new object[]
						{
							this.pawn,
							this.def
						});
					}
					else
					{
						TaleRecorder.RecordTale(TaleDefOf.GainedMasterSkillWithPassion, new object[]
						{
							this.pawn,
							this.def
						});
					}
				}
				if (this.levelInt >= 20)
				{
					this.levelInt = 20;
					this.xpSinceLastLevel = Mathf.Clamp(this.xpSinceLastLevel, 0f, this.XpRequiredForLevelUp - 1f);
					IL_18D:
					while (this.xpSinceLastLevel <= -1000f)
					{
						this.levelInt--;
						this.xpSinceLastLevel += this.XpRequiredForLevelUp;
						if (this.levelInt <= 0)
						{
							this.levelInt = 0;
							this.xpSinceLastLevel = 0f;
							break;
						}
					}
					if (flag && this.pawn.IsColonist && this.pawn.Spawned)
					{
						MoteMaker.ThrowText(this.pawn.DrawPos, this.pawn.Map, this.def.LabelCap + "\n" + "TextMote_SkillUp".Translate(this.levelInt), -1f);
					}
					return;
				}
			}
			goto IL_18D;
		}

		// Token: 0x06005711 RID: 22289 RVA: 0x001D8E08 File Offset: 0x001D7008
		public float LearnRateFactor(bool direct = false)
		{
			if (DebugSettings.fastLearning)
			{
				return 200f;
			}
			float num;
			switch (this.passion)
			{
			case Passion.None:
				num = 0.35f;
				break;
			case Passion.Minor:
				num = 1f;
				break;
			case Passion.Major:
				num = 1.5f;
				break;
			default:
				throw new NotImplementedException("Passion level " + this.passion);
			}
			if (!direct)
			{
				num *= this.pawn.GetStatValue(StatDefOf.GlobalLearningFactor, true);
				if (this.def == SkillDefOf.Animals)
				{
					num *= this.pawn.GetStatValue(StatDefOf.AnimalsLearningFactor, true);
				}
				if (this.LearningSaturatedToday)
				{
					num *= 0.2f;
				}
			}
			return num;
		}

		// Token: 0x06005712 RID: 22290 RVA: 0x001D8EBC File Offset: 0x001D70BC
		public void EnsureMinLevelWithMargin(int minLevel)
		{
			if (this.TotallyDisabled)
			{
				return;
			}
			if (this.Level < minLevel || (this.Level == minLevel && this.xpSinceLastLevel < this.XpRequiredForLevelUp / 2f))
			{
				this.Level = minLevel;
				this.xpSinceLastLevel = this.XpRequiredForLevelUp / 2f;
			}
		}

		// Token: 0x06005713 RID: 22291 RVA: 0x001D8F11 File Offset: 0x001D7111
		public void Notify_SkillDisablesChanged()
		{
			this.cachedTotallyDisabled = BoolUnknown.Unknown;
		}

		// Token: 0x06005714 RID: 22292 RVA: 0x001D8F1A File Offset: 0x001D711A
		private bool CalculateTotallyDisabled()
		{
			return this.def.IsDisabled(this.pawn.story.DisabledWorkTagsBackstoryAndTraits, this.pawn.GetDisabledWorkTypes(true));
		}

		// Token: 0x06005715 RID: 22293 RVA: 0x001D8F44 File Offset: 0x001D7144
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				this.def.defName,
				": ",
				this.levelInt,
				" (",
				this.xpSinceLastLevel,
				"xp)"
			});
		}

		// Token: 0x0400335E RID: 13150
		private Pawn pawn;

		// Token: 0x0400335F RID: 13151
		public SkillDef def;

		// Token: 0x04003360 RID: 13152
		public int levelInt;

		// Token: 0x04003361 RID: 13153
		public Passion passion;

		// Token: 0x04003362 RID: 13154
		public float xpSinceLastLevel;

		// Token: 0x04003363 RID: 13155
		public float xpSinceMidnight;

		// Token: 0x04003364 RID: 13156
		private BoolUnknown cachedTotallyDisabled = BoolUnknown.Unknown;

		// Token: 0x04003365 RID: 13157
		public const int IntervalTicks = 200;

		// Token: 0x04003366 RID: 13158
		public const int MinLevel = 0;

		// Token: 0x04003367 RID: 13159
		public const int MaxLevel = 20;

		// Token: 0x04003368 RID: 13160
		public const int MaxFullRateXpPerDay = 4000;

		// Token: 0x04003369 RID: 13161
		public const int MasterSkillThreshold = 14;

		// Token: 0x0400336A RID: 13162
		public const float SaturatedLearningFactor = 0.2f;

		// Token: 0x0400336B RID: 13163
		public const float LearnFactorPassionNone = 0.35f;

		// Token: 0x0400336C RID: 13164
		public const float LearnFactorPassionMinor = 1f;

		// Token: 0x0400336D RID: 13165
		public const float LearnFactorPassionMajor = 1.5f;

		// Token: 0x0400336E RID: 13166
		public const float MinXPAmount = -1000f;

		// Token: 0x0400336F RID: 13167
		private static readonly SimpleCurve XpForLevelUpCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1000f),
				true
			},
			{
				new CurvePoint(9f, 10000f),
				true
			},
			{
				new CurvePoint(19f, 30000f),
				true
			}
		};
	}
}
