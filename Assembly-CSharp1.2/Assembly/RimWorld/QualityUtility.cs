using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200189C RID: 6300
	public static class QualityUtility
	{
		// Token: 0x06008BCF RID: 35791 RVA: 0x0028AFF4 File Offset: 0x002891F4
		static QualityUtility()
		{
			foreach (object obj in Enum.GetValues(typeof(QualityCategory)))
			{
				QualityCategory item = (QualityCategory)obj;
				QualityUtility.AllQualityCategories.Add(item);
			}
		}

		// Token: 0x06008BD0 RID: 35792 RVA: 0x0028B064 File Offset: 0x00289264
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

		// Token: 0x06008BD1 RID: 35793 RVA: 0x0028B0A0 File Offset: 0x002892A0
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

		// Token: 0x06008BD2 RID: 35794 RVA: 0x0028B148 File Offset: 0x00289348
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

		// Token: 0x06008BD3 RID: 35795 RVA: 0x0005DC54 File Offset: 0x0005BE54
		public static bool FollowQualityThingFilter(this ThingDef def)
		{
			return def.stackLimit == 1 || def.HasComp(typeof(CompQuality));
		}

		// Token: 0x06008BD4 RID: 35796 RVA: 0x0028B1F0 File Offset: 0x002893F0
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

		// Token: 0x06008BD5 RID: 35797 RVA: 0x0005DC76 File Offset: 0x0005BE76
		public static QualityCategory GenerateQualityRandomEqualChance()
		{
			return QualityUtility.AllQualityCategories.RandomElement<QualityCategory>();
		}

		// Token: 0x06008BD6 RID: 35798 RVA: 0x0005DC82 File Offset: 0x0005BE82
		public static QualityCategory GenerateQualityReward()
		{
			return QualityUtility.GenerateFromGaussian(1f, QualityCategory.Legendary, QualityCategory.Excellent, QualityCategory.Good);
		}

		// Token: 0x06008BD7 RID: 35799 RVA: 0x0005DC91 File Offset: 0x0005BE91
		public static QualityCategory GenerateQualityGift()
		{
			return QualityUtility.GenerateFromGaussian(1f, QualityCategory.Legendary, QualityCategory.Normal, QualityCategory.Normal);
		}

		// Token: 0x06008BD8 RID: 35800 RVA: 0x0005DCA0 File Offset: 0x0005BEA0
		public static QualityCategory GenerateQualitySuper()
		{
			return QualityUtility.GenerateFromGaussian(1f, QualityCategory.Legendary, QualityCategory.Masterwork, QualityCategory.Masterwork);
		}

		// Token: 0x06008BD9 RID: 35801 RVA: 0x0005DCAF File Offset: 0x0005BEAF
		public static QualityCategory GenerateQualityTraderItem()
		{
			return QualityUtility.GenerateFromGaussian(1f, QualityCategory.Masterwork, QualityCategory.Normal, QualityCategory.Normal);
		}

		// Token: 0x06008BDA RID: 35802 RVA: 0x0005DCBE File Offset: 0x0005BEBE
		public static QualityCategory GenerateQualityBaseGen()
		{
			if (Rand.Value < 0.3f)
			{
				return QualityCategory.Normal;
			}
			return QualityUtility.GenerateFromGaussian(1f, QualityCategory.Excellent, QualityCategory.Normal, QualityCategory.Awful);
		}

		// Token: 0x06008BDB RID: 35803 RVA: 0x0028B24C File Offset: 0x0028944C
		public static QualityCategory GenerateQualityGeneratingPawn(PawnKindDef pawnKind)
		{
			if (pawnKind.forceNormalGearQuality)
			{
				return QualityCategory.Normal;
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

		// Token: 0x06008BDC RID: 35804 RVA: 0x0028B29C File Offset: 0x0028949C
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

		// Token: 0x06008BDD RID: 35805 RVA: 0x0028B44C File Offset: 0x0028964C
		public static QualityCategory GenerateQualityCreatedByPawn(Pawn pawn, SkillDef relevantSkill)
		{
			int level = pawn.skills.GetSkill(relevantSkill).Level;
			bool flag = pawn.InspirationDef == InspirationDefOf.Inspired_Creativity;
			QualityCategory result = QualityUtility.GenerateQualityCreatedByPawn(level, flag);
			if (flag)
			{
				pawn.mindState.inspirationHandler.EndInspiration(InspirationDefOf.Inspired_Creativity);
			}
			return result;
		}

		// Token: 0x06008BDE RID: 35806 RVA: 0x0028B498 File Offset: 0x00289698
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

		// Token: 0x06008BDF RID: 35807 RVA: 0x0005DCDB File Offset: 0x0005BEDB
		private static QualityCategory AddLevels(QualityCategory quality, int levels)
		{
			return (QualityCategory)Mathf.Min((int)(quality + (byte)levels), 6);
		}

		// Token: 0x06008BE0 RID: 35808 RVA: 0x0028B4C8 File Offset: 0x002896C8
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

		// Token: 0x06008BE1 RID: 35809 RVA: 0x0028B6A4 File Offset: 0x002898A4
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
							qualityGenerator = (<>9__13 = (() => QualityUtility.GenerateQualityGeneratingPawn(localPk)));
						}
						return QualityUtility.DebugQualitiesStringSingle(q, qualityGenerator);
					}));
				}
			}
			DebugTables.MakeTablesDialog<QualityCategory>(QualityUtility.AllQualityCategories, list.ToArray());
		}

		// Token: 0x06008BE2 RID: 35810 RVA: 0x0028B8F0 File Offset: 0x00289AF0
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

		// Token: 0x06008BE3 RID: 35811 RVA: 0x0028B950 File Offset: 0x00289B50
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

		// Token: 0x04005999 RID: 22937
		public static List<QualityCategory> AllQualityCategories = new List<QualityCategory>();
	}
}
