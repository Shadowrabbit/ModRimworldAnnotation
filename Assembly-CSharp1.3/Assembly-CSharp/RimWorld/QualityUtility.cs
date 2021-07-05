using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011D3 RID: 4563
	public static class QualityUtility
	{
		// Token: 0x06006E19 RID: 28185 RVA: 0x0024E50C File Offset: 0x0024C70C
		static QualityUtility()
		{
			foreach (object obj in Enum.GetValues(typeof(QualityCategory)))
			{
				QualityCategory item = (QualityCategory)obj;
				QualityUtility.AllQualityCategories.Add(item);
			}
		}

		// Token: 0x06006E1A RID: 28186 RVA: 0x0024E57C File Offset: 0x0024C77C
		public static bool TryGetQuality(this Thing t, out QualityCategory qc)
		{
			MinifiedThing minifiedThing = t as MinifiedThing;
			CompQuality compQuality = (minifiedThing != null) ? minifiedThing.InnerThing.TryGetComp<CompQuality>() : t.TryGetComp<CompQuality>();
			if (compQuality == null)
			{
				qc = QualityCategory.Normal;
				return false;
			}
			qc = compQuality.Quality;
			return true;
		}

		// Token: 0x06006E1B RID: 28187 RVA: 0x0024E5B8 File Offset: 0x0024C7B8
		public static string GetLabel(this QualityCategory cat)
		{
			switch (cat)
			{
			case QualityCategory.Awful:
				return "QualityCategory_Awful".Translate();
			case QualityCategory.Poor:
				return "QualityCategory_Poor".Translate();
			case QualityCategory.Normal:
				return "QualityCategory_Normal".Translate();
			case QualityCategory.Good:
				return "QualityCategory_Good".Translate();
			case QualityCategory.Excellent:
				return "QualityCategory_Excellent".Translate();
			case QualityCategory.Masterwork:
				return "QualityCategory_Masterwork".Translate();
			case QualityCategory.Legendary:
				return "QualityCategory_Legendary".Translate();
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x06006E1C RID: 28188 RVA: 0x0024E660 File Offset: 0x0024C860
		public static string GetLabelShort(this QualityCategory cat)
		{
			switch (cat)
			{
			case QualityCategory.Awful:
				return "QualityCategoryShort_Awful".Translate();
			case QualityCategory.Poor:
				return "QualityCategoryShort_Poor".Translate();
			case QualityCategory.Normal:
				return "QualityCategoryShort_Normal".Translate();
			case QualityCategory.Good:
				return "QualityCategoryShort_Good".Translate();
			case QualityCategory.Excellent:
				return "QualityCategoryShort_Excellent".Translate();
			case QualityCategory.Masterwork:
				return "QualityCategoryShort_Masterwork".Translate();
			case QualityCategory.Legendary:
				return "QualityCategoryShort_Legendary".Translate();
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x06006E1D RID: 28189 RVA: 0x0024E706 File Offset: 0x0024C906
		public static bool FollowQualityThingFilter(this ThingDef def)
		{
			return def.stackLimit == 1 || def.HasComp(typeof(CompQuality));
		}

		// Token: 0x06006E1E RID: 28190 RVA: 0x0024E728 File Offset: 0x0024C928
		public static QualityCategory GenerateQuality(QualityGenerator qualityGenerator)
		{
			switch (qualityGenerator)
			{
			case QualityGenerator.BaseGen:
				return QualityUtility.GenerateQualityBaseGen();
			case QualityGenerator.Reward:
				return QualityUtility.GenerateQualityReward();
			case QualityGenerator.Gift:
				return QualityUtility.GenerateQualityGift();
			case QualityGenerator.Super:
				return QualityUtility.GenerateQualitySuper();
			case QualityGenerator.Trader:
				return QualityUtility.GenerateQualityTraderItem();
			default:
				throw new NotImplementedException(qualityGenerator.ToString());
			}
		}

		// Token: 0x06006E1F RID: 28191 RVA: 0x0024E781 File Offset: 0x0024C981
		public static QualityCategory GenerateQualityRandomEqualChance()
		{
			return QualityUtility.AllQualityCategories.RandomElement<QualityCategory>();
		}

		// Token: 0x06006E20 RID: 28192 RVA: 0x0024E78D File Offset: 0x0024C98D
		public static QualityCategory GenerateQualityReward()
		{
			return QualityUtility.GenerateFromGaussian(1f, QualityCategory.Legendary, QualityCategory.Excellent, QualityCategory.Good);
		}

		// Token: 0x06006E21 RID: 28193 RVA: 0x0024E79C File Offset: 0x0024C99C
		public static QualityCategory GenerateQualityGift()
		{
			return QualityUtility.GenerateFromGaussian(1f, QualityCategory.Legendary, QualityCategory.Normal, QualityCategory.Normal);
		}

		// Token: 0x06006E22 RID: 28194 RVA: 0x0024E7AB File Offset: 0x0024C9AB
		public static QualityCategory GenerateQualitySuper()
		{
			return QualityUtility.GenerateFromGaussian(1f, QualityCategory.Legendary, QualityCategory.Masterwork, QualityCategory.Masterwork);
		}

		// Token: 0x06006E23 RID: 28195 RVA: 0x0024E7BA File Offset: 0x0024C9BA
		public static QualityCategory GenerateQualityTraderItem()
		{
			return QualityUtility.GenerateFromGaussian(1f, QualityCategory.Masterwork, QualityCategory.Normal, QualityCategory.Normal);
		}

		// Token: 0x06006E24 RID: 28196 RVA: 0x0024E7C9 File Offset: 0x0024C9C9
		public static QualityCategory GenerateQualityBaseGen()
		{
			if (Rand.Value < 0.3f)
			{
				return QualityCategory.Normal;
			}
			return QualityUtility.GenerateFromGaussian(1f, QualityCategory.Excellent, QualityCategory.Normal, QualityCategory.Awful);
		}

		// Token: 0x06006E25 RID: 28197 RVA: 0x0024E7E8 File Offset: 0x0024C9E8
		public static QualityCategory GenerateQualityGeneratingPawn(PawnKindDef pawnKind, ThingDef forThing)
		{
			if (pawnKind.forceNormalGearQuality)
			{
				return QualityCategory.Normal;
			}
			if (forThing.IsWeapon && pawnKind.forceWeaponQuality != null)
			{
				return pawnKind.forceWeaponQuality.Value;
			}
			if (!forThing.IsWeapon && pawnKind.specificApparelRequirements != null)
			{
				for (int i = 0; i < pawnKind.specificApparelRequirements.Count; i++)
				{
					if (pawnKind.specificApparelRequirements[i].Quality != null && PawnApparelGenerator.ApparelRequirementHandlesThing(pawnKind.specificApparelRequirements[i], forThing))
					{
						return pawnKind.specificApparelRequirements[i].Quality.Value;
					}
				}
			}
			int itemQuality = (int)pawnKind.itemQuality;
			float value = Rand.Value;
			int num;
			if (value < 0.1f)
			{
				num = itemQuality - 1;
			}
			else if (value < 0.2f)
			{
				num = itemQuality + 1;
			}
			else
			{
				num = itemQuality;
			}
			num = Mathf.Clamp(num, 0, 4);
			return (QualityCategory)num;
		}

		// Token: 0x06006E26 RID: 28198 RVA: 0x0024E8C8 File Offset: 0x0024CAC8
		public static QualityCategory GenerateQualityCreatedByPawn(int relevantSkillLevel, bool inspired)
		{
			float num = 0f;
			switch (relevantSkillLevel)
			{
			case 0:
				num += 0.7f;
				break;
			case 1:
				num += 1.1f;
				break;
			case 2:
				num += 1.5f;
				break;
			case 3:
				num += 1.8f;
				break;
			case 4:
				num += 2f;
				break;
			case 5:
				num += 2.2f;
				break;
			case 6:
				num += 2.4f;
				break;
			case 7:
				num += 2.6f;
				break;
			case 8:
				num += 2.8f;
				break;
			case 9:
				num += 2.95f;
				break;
			case 10:
				num += 3.1f;
				break;
			case 11:
				num += 3.25f;
				break;
			case 12:
				num += 3.4f;
				break;
			case 13:
				num += 3.5f;
				break;
			case 14:
				num += 3.6f;
				break;
			case 15:
				num += 3.7f;
				break;
			case 16:
				num += 3.8f;
				break;
			case 17:
				num += 3.9f;
				break;
			case 18:
				num += 4f;
				break;
			case 19:
				num += 4.1f;
				break;
			case 20:
				num += 4.2f;
				break;
			}
			int num2 = (int)Rand.GaussianAsymmetric(num, 0.6f, 0.8f);
			num2 = Mathf.Clamp(num2, 0, 5);
			if (num2 == 5 && Rand.Value < 0.5f)
			{
				num2 = (int)Rand.GaussianAsymmetric(num, 0.6f, 0.95f);
				num2 = Mathf.Clamp(num2, 0, 5);
			}
			QualityCategory qualityCategory = (QualityCategory)num2;
			if (inspired)
			{
				qualityCategory = QualityUtility.AddLevels(qualityCategory, 2);
			}
			return qualityCategory;
		}

		// Token: 0x06006E27 RID: 28199 RVA: 0x0024EA78 File Offset: 0x0024CC78
		public static QualityCategory GenerateQualityCreatedByPawn(Pawn pawn, SkillDef relevantSkill)
		{
			int level = pawn.skills.GetSkill(relevantSkill).Level;
			bool flag = pawn.InspirationDef == InspirationDefOf.Inspired_Creativity;
			QualityCategory qualityCategory = QualityUtility.GenerateQualityCreatedByPawn(level, flag);
			if (ModsConfig.IdeologyActive && pawn.Ideo != null)
			{
				Precept_Role role = pawn.Ideo.GetRole(pawn);
				if (role != null && role.def.roleEffects != null)
				{
					RoleEffect roleEffect = role.def.roleEffects.FirstOrDefault((RoleEffect eff) => eff is RoleEffect_ProductionQualityOffset);
					if (roleEffect != null)
					{
						qualityCategory = QualityUtility.AddLevels(qualityCategory, ((RoleEffect_ProductionQualityOffset)roleEffect).offset);
					}
				}
			}
			if (flag)
			{
				pawn.mindState.inspirationHandler.EndInspiration(InspirationDefOf.Inspired_Creativity);
			}
			return qualityCategory;
		}

		// Token: 0x06006E28 RID: 28200 RVA: 0x0024EB38 File Offset: 0x0024CD38
		private static QualityCategory GenerateFromGaussian(float widthFactor, QualityCategory max = QualityCategory.Legendary, QualityCategory center = QualityCategory.Normal, QualityCategory min = QualityCategory.Awful)
		{
			float num = Rand.Gaussian((float)center + 0.5f, widthFactor);
			if (num < (float)min)
			{
				num = (float)min;
			}
			if (num > (float)max)
			{
				num = (float)max;
			}
			return (QualityCategory)((int)num);
		}

		// Token: 0x06006E29 RID: 28201 RVA: 0x0024EB67 File Offset: 0x0024CD67
		private static QualityCategory AddLevels(QualityCategory quality, int levels)
		{
			return (QualityCategory)Mathf.Min((int)(quality + (byte)levels), 6);
		}

		// Token: 0x06006E2A RID: 28202 RVA: 0x0024EB74 File Offset: 0x0024CD74
		public static void SendCraftNotification(Thing thing, Pawn worker)
		{
			if (worker == null)
			{
				return;
			}
			CompQuality compQuality = thing.TryGetComp<CompQuality>();
			if (compQuality == null)
			{
				return;
			}
			CompArt compArt = thing.TryGetComp<CompArt>();
			if (compArt == null || compArt.Props.mustBeFullGrave)
			{
				if (compQuality.Quality == QualityCategory.Masterwork)
				{
					Find.LetterStack.ReceiveLetter("LetterCraftedMasterworkLabel".Translate(), "LetterCraftedMasterworkMessage".Translate(worker.LabelShort, thing.LabelShort, worker.Named("WORKER"), thing.Named("CRAFTED")), LetterDefOf.PositiveEvent, thing, null, null, null, null);
					return;
				}
				if (compQuality.Quality == QualityCategory.Legendary)
				{
					Find.LetterStack.ReceiveLetter("LetterCraftedLegendaryLabel".Translate(), "LetterCraftedLegendaryMessage".Translate(worker.LabelShort, thing.LabelShort, worker.Named("WORKER"), thing.Named("CRAFTED")), LetterDefOf.PositiveEvent, thing, null, null, null, null);
					return;
				}
			}
			else
			{
				if (compQuality.Quality == QualityCategory.Masterwork)
				{
					Find.LetterStack.ReceiveLetter("LetterCraftedMasterworkLabel".Translate(), "LetterCraftedMasterworkMessageArt".Translate(compArt.GenerateImageDescription(), worker.LabelShort, thing.LabelShort, worker.Named("WORKER"), thing.Named("CRAFTED")), LetterDefOf.PositiveEvent, thing, null, null, null, null);
					return;
				}
				if (compQuality.Quality == QualityCategory.Legendary)
				{
					Find.LetterStack.ReceiveLetter("LetterCraftedLegendaryLabel".Translate(), "LetterCraftedLegendaryMessageArt".Translate(compArt.GenerateImageDescription(), worker.LabelShort, thing.LabelShort, worker.Named("WORKER"), thing.Named("CRAFTED")), LetterDefOf.PositiveEvent, thing, null, null, null, null);
				}
			}
		}

		// Token: 0x06006E2B RID: 28203 RVA: 0x0024ED50 File Offset: 0x0024CF50
		[DebugOutput]
		private static void QualityGenerationData()
		{
			List<TableDataGetter<QualityCategory>> list = new List<TableDataGetter<QualityCategory>>();
			list.Add(new TableDataGetter<QualityCategory>("quality", (QualityCategory q) => q.ToString()));
			list.Add(new TableDataGetter<QualityCategory>("Rewards\n(quests,\netc...? )", (QualityCategory q) => QualityUtility.DebugQualitiesStringSingle(q, () => QualityUtility.GenerateQualityReward())));
			list.Add(new TableDataGetter<QualityCategory>("Trader\nitems", (QualityCategory q) => QualityUtility.DebugQualitiesStringSingle(q, () => QualityUtility.GenerateQualityTraderItem())));
			list.Add(new TableDataGetter<QualityCategory>("Map generation\nitems and\nbuildings\n(e.g. NPC bases)", (QualityCategory q) => QualityUtility.DebugQualitiesStringSingle(q, () => QualityUtility.GenerateQualityBaseGen())));
			list.Add(new TableDataGetter<QualityCategory>("Gifts", (QualityCategory q) => QualityUtility.DebugQualitiesStringSingle(q, () => QualityUtility.GenerateQualityGift())));
			for (int i = 0; i <= 20; i++)
			{
				int localLevel = i;
				Func<QualityCategory> <>9__10;
				list.Add(new TableDataGetter<QualityCategory>("Made\nat skill\n" + i, delegate(QualityCategory q)
				{
					Func<QualityCategory> qualityGenerator;
					if ((qualityGenerator = <>9__10) == null)
					{
						qualityGenerator = (<>9__10 = (() => QualityUtility.GenerateQualityCreatedByPawn(localLevel, false)));
					}
					return QualityUtility.DebugQualitiesStringSingle(q, qualityGenerator);
				}));
			}
			foreach (PawnKindDef localPk2 in from k in DefDatabase<PawnKindDef>.AllDefs
			orderby k.combatPower
			select k)
			{
				PawnKindDef localPk = localPk2;
				if (localPk.RaceProps.Humanlike)
				{
					Func<QualityCategory> <>9__13;
					list.Add(new TableDataGetter<QualityCategory>(string.Concat(new object[]
					{
						"Gear for\n",
						localPk.defName,
						"\nPower ",
						localPk.combatPower.ToString("F0"),
						"\nitemQuality:\n",
						localPk.itemQuality
					}), delegate(QualityCategory q)
					{
						Func<QualityCategory> qualityGenerator;
						if ((qualityGenerator = <>9__13) == null)
						{
							qualityGenerator = (<>9__13 = (() => QualityUtility.GenerateQualityGeneratingPawn(localPk, null)));
						}
						return QualityUtility.DebugQualitiesStringSingle(q, qualityGenerator);
					}));
				}
			}
			DebugTables.MakeTablesDialog<QualityCategory>(QualityUtility.AllQualityCategories, list.ToArray());
		}

		// Token: 0x06006E2C RID: 28204 RVA: 0x0024EF9C File Offset: 0x0024D19C
		private static string DebugQualitiesStringSingle(QualityCategory quality, Func<QualityCategory> qualityGenerator)
		{
			int num = 10000;
			List<QualityCategory> list = new List<QualityCategory>();
			for (int i = 0; i < num; i++)
			{
				list.Add(qualityGenerator());
			}
			return ((float)(from q in list
			where q == quality
			select q).Count<QualityCategory>() / (float)num).ToStringPercent();
		}

		// Token: 0x06006E2D RID: 28205 RVA: 0x0024EFFC File Offset: 0x0024D1FC
		private static string DebugQualitiesString(Func<QualityCategory> qualityGenerator)
		{
			int num = 10000;
			StringBuilder stringBuilder = new StringBuilder();
			List<QualityCategory> list = new List<QualityCategory>();
			for (int i = 0; i < num; i++)
			{
				list.Add(qualityGenerator());
			}
			using (List<QualityCategory>.Enumerator enumerator = QualityUtility.AllQualityCategories.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					QualityCategory qu = enumerator.Current;
					stringBuilder.AppendLine(qu.ToString() + " - " + ((float)(from q in list
					where q == qu
					select q).Count<QualityCategory>() / (float)num).ToStringPercent());
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04003D24 RID: 15652
		public static List<QualityCategory> AllQualityCategories = new List<QualityCategory>();
	}
}
