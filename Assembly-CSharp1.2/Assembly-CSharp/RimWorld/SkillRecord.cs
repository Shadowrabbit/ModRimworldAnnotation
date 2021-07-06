using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001542 RID: 5442
	public class SkillRecord : IExposable
	{
		// Token: 0x1700123C RID: 4668
		// (get) Token: 0x060075D5 RID: 30165 RVA: 0x0004F748 File Offset: 0x0004D948
		// (set) Token: 0x060075D6 RID: 30166 RVA: 0x0004F75A File Offset: 0x0004D95A
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

		// Token: 0x1700123D RID: 4669
		// (get) Token: 0x060075D7 RID: 30167 RVA: 0x0004F76B File Offset: 0x0004D96B
		public float XpRequiredForLevelUp
		{
			get
			{
				return SkillRecord.XpRequiredToLevelUpFrom(this.levelInt);
			}
		}

		// Token: 0x1700123E RID: 4670
		// (get) Token: 0x060075D8 RID: 30168 RVA: 0x0004F778 File Offset: 0x0004D978
		public float XpProgressPercent
		{
			get
			{
				return this.xpSinceLastLevel / this.XpRequiredForLevelUp;
			}
		}

		// Token: 0x1700123F RID: 4671
		// (get) Token: 0x060075D9 RID: 30169 RVA: 0x0023DC94 File Offset: 0x0023BE94
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

		// Token: 0x17001240 RID: 4672
		// (get) Token: 0x060075DA RID: 30170 RVA: 0x0004F787 File Offset: 0x0004D987
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

		// Token: 0x17001241 RID: 4673
		// (get) Token: 0x060075DB RID: 30171 RVA: 0x0023DCC4 File Offset: 0x0023BEC4
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

		// Token: 0x17001242 RID: 4674
		// (get) Token: 0x060075DC RID: 30172 RVA: 0x0004F7AD File Offset: 0x0004D9AD
		public bool LearningSaturatedToday
		{
			get
			{
				return this.xpSinceMidnight > 4000f;
			}
		}

		// Token: 0x060075DD RID: 30173 RVA: 0x0004F7BC File Offset: 0x0004D9BC
		public SkillRecord()
		{
		}

		// Token: 0x060075DE RID: 30174 RVA: 0x0004F7CB File Offset: 0x0004D9CB
		public SkillRecord(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060075DF RID: 30175 RVA: 0x0004F7E1 File Offset: 0x0004D9E1
		public SkillRecord(Pawn pawn, SkillDef def)
		{
			this.pawn = pawn;
			this.def = def;
		}

		// Token: 0x060075E0 RID: 30176 RVA: 0x0023DE8C File Offset: 0x0023C08C
		public void ExposeData()
		{
			Scribe_Defs.Look<SkillDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.levelInt, "level", 0, false);
			Scribe_Values.Look<float>(ref this.xpSinceLastLevel, "xpSinceLastLevel", 0f, false);
			Scribe_Values.Look<Passion>(ref this.passion, "passion", Passion.None, false);
			Scribe_Values.Look<float>(ref this.xpSinceMidnight, "xpSinceMidnight", 0f, false);
		}

		// Token: 0x060075E1 RID: 30177 RVA: 0x0023DEFC File Offset: 0x0023C0FC
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

		// Token: 0x060075E2 RID: 30178 RVA: 0x0004F7FE File Offset: 0x0004D9FE
		public static float XpRequiredToLevelUpFrom(int startingLevel)
		{
			return SkillRecord.XpForLevelUpCurve.Evaluate((float)startingLevel);
		}

		// Token: 0x060075E3 RID: 30179 RVA: 0x0023E014 File Offset: 0x0023C214
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

		// Token: 0x060075E4 RID: 30180 RVA: 0x0023E22C File Offset: 0x0023C42C
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
				if (this.LearningSaturatedToday)
				{
					num *= 0.2f;
				}
			}
			return num;
		}

		// Token: 0x060075E5 RID: 30181 RVA: 0x0023E2BC File Offset: 0x0023C4BC
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

		// Token: 0x060075E6 RID: 30182 RVA: 0x0004F80C File Offset: 0x0004DA0C
		public void Notify_SkillDisablesChanged()
		{
			this.cachedTotallyDisabled = BoolUnknown.Unknown;
		}

		// Token: 0x060075E7 RID: 30183 RVA: 0x0004F815 File Offset: 0x0004DA15
		private bool CalculateTotallyDisabled()
		{
			return this.def.IsDisabled(this.pawn.story.DisabledWorkTagsBackstoryAndTraits, this.pawn.GetDisabledWorkTypes(true));
		}

		// Token: 0x060075E8 RID: 30184 RVA: 0x0023E314 File Offset: 0x0023C514
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

		// Token: 0x04004DBA RID: 19898
		private Pawn pawn;

		// Token: 0x04004DBB RID: 19899
		public SkillDef def;

		// Token: 0x04004DBC RID: 19900
		public int levelInt;

		// Token: 0x04004DBD RID: 19901
		public Passion passion;

		// Token: 0x04004DBE RID: 19902
		public float xpSinceLastLevel;

		// Token: 0x04004DBF RID: 19903
		public float xpSinceMidnight;

		// Token: 0x04004DC0 RID: 19904
		private BoolUnknown cachedTotallyDisabled = BoolUnknown.Unknown;

		// Token: 0x04004DC1 RID: 19905
		public const int IntervalTicks = 200;

		// Token: 0x04004DC2 RID: 19906
		public const int MinLevel = 0;

		// Token: 0x04004DC3 RID: 19907
		public const int MaxLevel = 20;

		// Token: 0x04004DC4 RID: 19908
		public const int MaxFullRateXpPerDay = 4000;

		// Token: 0x04004DC5 RID: 19909
		public const int MasterSkillThreshold = 14;

		// Token: 0x04004DC6 RID: 19910
		public const float SaturatedLearningFactor = 0.2f;

		// Token: 0x04004DC7 RID: 19911
		public const float LearnFactorPassionNone = 0.35f;

		// Token: 0x04004DC8 RID: 19912
		public const float LearnFactorPassionMinor = 1f;

		// Token: 0x04004DC9 RID: 19913
		public const float LearnFactorPassionMajor = 1.5f;

		// Token: 0x04004DCA RID: 19914
		public const float MinXPAmount = -1000f;

		// Token: 0x04004DCB RID: 19915
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
