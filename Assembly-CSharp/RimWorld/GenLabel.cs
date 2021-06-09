using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CC3 RID: 7363
	public static class GenLabel
	{
		// Token: 0x0600A02E RID: 41006 RVA: 0x0006ACA6 File Offset: 0x00068EA6
		public static void ClearCache()
		{
			GenLabel.labelDictionary.Clear();
		}

		// Token: 0x0600A02F RID: 41007 RVA: 0x002ED51C File Offset: 0x002EB71C
		public static string ThingLabel(BuildableDef entDef, ThingDef stuffDef, int stackCount = 1)
		{
			GenLabel.LabelRequest key = default(GenLabel.LabelRequest);
			key.entDef = entDef;
			key.stuffDef = stuffDef;
			key.stackCount = stackCount;
			string text;
			if (!GenLabel.labelDictionary.TryGetValue(key, out text))
			{
				if (GenLabel.labelDictionary.Count > 2000)
				{
					GenLabel.labelDictionary.Clear();
				}
				text = GenLabel.NewThingLabel(entDef, stuffDef, stackCount);
				GenLabel.labelDictionary.Add(key, text);
			}
			return text;
		}

		// Token: 0x0600A030 RID: 41008 RVA: 0x002ED58C File Offset: 0x002EB78C
		private static string NewThingLabel(BuildableDef entDef, ThingDef stuffDef, int stackCount)
		{
			string text;
			if (stuffDef == null)
			{
				text = entDef.label;
			}
			else
			{
				text = "ThingMadeOfStuffLabel".Translate(stuffDef.LabelAsStuff, entDef.label);
			}
			if (stackCount > 1)
			{
				text = text + " x" + stackCount.ToStringCached();
			}
			return text;
		}

		// Token: 0x0600A031 RID: 41009 RVA: 0x002ED5E4 File Offset: 0x002EB7E4
		public static string ThingLabel(Thing t, int stackCount, bool includeHp = true)
		{
			GenLabel.LabelRequest key = default(GenLabel.LabelRequest);
			key.entDef = t.def;
			key.stuffDef = t.Stuff;
			key.stackCount = stackCount;
			t.TryGetQuality(out key.quality);
			if (t.def.useHitPoints && includeHp)
			{
				key.health = t.HitPoints;
				key.maxHealth = t.MaxHitPoints;
			}
			Apparel apparel = t as Apparel;
			if (apparel != null)
			{
				key.wornByCorpse = apparel.WornByCorpse;
			}
			string text;
			if (!GenLabel.labelDictionary.TryGetValue(key, out text))
			{
				if (GenLabel.labelDictionary.Count > 2000)
				{
					GenLabel.labelDictionary.Clear();
				}
				text = GenLabel.NewThingLabel(t, stackCount, includeHp);
				GenLabel.labelDictionary.Add(key, text);
			}
			return text;
		}

		// Token: 0x0600A032 RID: 41010 RVA: 0x002ED6AC File Offset: 0x002EB8AC
		private static string NewThingLabel(Thing t, int stackCount, bool includeHp)
		{
			string text = GenLabel.ThingLabel(t.def, t.Stuff, 1);
			QualityCategory cat;
			bool flag = t.TryGetQuality(out cat);
			int hitPoints = t.HitPoints;
			int maxHitPoints = t.MaxHitPoints;
			bool flag2 = t.def.useHitPoints && hitPoints < maxHitPoints && t.def.stackLimit == 1 && includeHp;
			Apparel apparel = t as Apparel;
			bool flag3 = apparel != null && apparel.WornByCorpse;
			if (flag || flag2 || flag3)
			{
				text += " (";
				if (flag)
				{
					text += cat.GetLabel();
				}
				if (flag2)
				{
					if (flag)
					{
						text += " ";
					}
					text += ((float)hitPoints / (float)maxHitPoints).ToStringPercent();
				}
				if (flag3)
				{
					if (flag || flag2)
					{
						text += " ";
					}
					text += "WornByCorpseChar".Translate();
				}
				text += ")";
			}
			if (stackCount > 1)
			{
				text = text + " x" + stackCount.ToStringCached();
			}
			return text;
		}

		// Token: 0x0600A033 RID: 41011 RVA: 0x002ED7BC File Offset: 0x002EB9BC
		public static string ThingsLabel(IEnumerable<Thing> things, string prefix = "  - ")
		{
			GenLabel.tmpThingCounts.Clear();
			IList<Thing> list = things as IList<Thing>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					GenLabel.tmpThingCounts.Add(new ThingCount(list[i], list[i].stackCount));
				}
			}
			else
			{
				foreach (Thing thing in things)
				{
					GenLabel.tmpThingCounts.Add(new ThingCount(thing, thing.stackCount));
				}
			}
			string result = GenLabel.ThingsLabel(GenLabel.tmpThingCounts, prefix, false);
			GenLabel.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x0600A034 RID: 41012 RVA: 0x002ED874 File Offset: 0x002EBA74
		public static string ThingsLabel(List<ThingCount> things, string prefix = "  - ", bool ignoreStackLimit = false)
		{
			GenLabel.tmpThingsLabelElements.Clear();
			using (List<ThingCount>.Enumerator enumerator = things.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingCount thing = enumerator.Current;
					GenLabel.LabelElement labelElement = (from elem in GenLabel.tmpThingsLabelElements
					where (thing.Thing.def.stackLimit > 1 | ignoreStackLimit) && elem.thingTemplate.def == thing.Thing.def && elem.thingTemplate.Stuff == thing.Thing.Stuff
					select elem).FirstOrDefault<GenLabel.LabelElement>();
					if (labelElement != null)
					{
						labelElement.count += thing.Count;
					}
					else
					{
						GenLabel.tmpThingsLabelElements.Add(new GenLabel.LabelElement
						{
							thingTemplate = thing.Thing,
							count = thing.Count
						});
					}
				}
			}
			GenLabel.tmpThingsLabelElements.Sort(delegate(GenLabel.LabelElement lhs, GenLabel.LabelElement rhs)
			{
				int num = TransferableComparer_Category.Compare(lhs.thingTemplate.def, rhs.thingTemplate.def);
				if (num != 0)
				{
					return num;
				}
				return lhs.thingTemplate.MarketValue.CompareTo(rhs.thingTemplate.MarketValue);
			});
			StringBuilder stringBuilder = new StringBuilder();
			foreach (GenLabel.LabelElement labelElement2 in GenLabel.tmpThingsLabelElements)
			{
				string str = "";
				if (labelElement2.thingTemplate.ParentHolder is Pawn_ApparelTracker)
				{
					str = " (" + "WornBy".Translate(((Pawn)labelElement2.thingTemplate.ParentHolder.ParentHolder).LabelShort, (Pawn)labelElement2.thingTemplate.ParentHolder.ParentHolder) + ")";
				}
				else if (labelElement2.thingTemplate.ParentHolder is Pawn_EquipmentTracker)
				{
					str = " (" + "EquippedBy".Translate(((Pawn)labelElement2.thingTemplate.ParentHolder.ParentHolder).LabelShort, (Pawn)labelElement2.thingTemplate.ParentHolder.ParentHolder) + ")";
				}
				if (labelElement2.count == 1)
				{
					stringBuilder.AppendLine(prefix + labelElement2.thingTemplate.LabelCap + str);
				}
				else
				{
					stringBuilder.AppendLine(prefix + GenLabel.ThingLabel(labelElement2.thingTemplate.def, labelElement2.thingTemplate.Stuff, labelElement2.count).CapitalizeFirst() + str);
				}
			}
			GenLabel.tmpThingsLabelElements.Clear();
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0600A035 RID: 41013 RVA: 0x002EDB44 File Offset: 0x002EBD44
		public static string BestKindLabel(Pawn pawn, bool mustNoteGender = false, bool mustNoteLifeStage = false, bool plural = false, int pluralCount = -1)
		{
			if (plural && pluralCount == 1)
			{
				plural = false;
			}
			bool flag = false;
			bool flag2 = false;
			string text = null;
			switch (pawn.gender)
			{
			case Gender.None:
				if (plural && !pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.labelPlural != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.labelPlural;
					flag2 = true;
				}
				else if (!pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.label != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.label;
					flag2 = true;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text, pawn.gender, pluralCount);
					}
				}
				else
				{
					text = GenLabel.BestKindLabel(pawn.kindDef, Gender.None, out flag, plural, pluralCount);
				}
				break;
			case Gender.Male:
				if (plural && !pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.labelMalePlural != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.labelMalePlural;
					flag2 = true;
					flag = true;
				}
				else if (!pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.labelMale != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.labelMale;
					flag2 = true;
					flag = true;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text, pawn.gender, pluralCount);
					}
				}
				else if (plural && !pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.labelPlural != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.labelPlural;
					flag2 = true;
				}
				else if (!pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.label != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.label;
					flag2 = true;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text, pawn.gender, pluralCount);
					}
				}
				else
				{
					text = GenLabel.BestKindLabel(pawn.kindDef, Gender.Male, out flag, plural, pluralCount);
				}
				break;
			case Gender.Female:
				if (plural && !pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.labelFemalePlural != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.labelFemalePlural;
					flag2 = true;
					flag = true;
				}
				else if (!pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.labelFemale != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.labelFemale;
					flag2 = true;
					flag = true;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text, pawn.gender, pluralCount);
					}
				}
				else if (plural && !pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.labelPlural != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.labelPlural;
					flag2 = true;
				}
				else if (!pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.label != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.label;
					flag2 = true;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text, pawn.gender, pluralCount);
					}
				}
				else
				{
					text = GenLabel.BestKindLabel(pawn.kindDef, Gender.Female, out flag, plural, pluralCount);
				}
				break;
			}
			if (mustNoteGender && !flag && pawn.gender != Gender.None)
			{
				text = "PawnMainDescGendered".Translate(pawn.GetGenderLabel(), text, pawn.Named("PAWN"));
			}
			if (mustNoteLifeStage && !flag2 && pawn.ageTracker != null && pawn.ageTracker.CurLifeStage.visible)
			{
				text = "PawnMainDescLifestageWrap".Translate(text, pawn.ageTracker.CurLifeStage.Adjective, pawn);
			}
			return text;
		}

		// Token: 0x0600A036 RID: 41014 RVA: 0x002EDF04 File Offset: 0x002EC104
		public static string BestKindLabel(PawnKindDef kindDef, Gender gender, bool plural = false, int pluralCount = -1)
		{
			bool flag;
			return GenLabel.BestKindLabel(kindDef, gender, out flag, plural, pluralCount);
		}

		// Token: 0x0600A037 RID: 41015 RVA: 0x002EDF1C File Offset: 0x002EC11C
		public static string BestKindLabel(PawnKindDef kindDef, Gender gender, out bool genderNoted, bool plural = false, int pluralCount = -1)
		{
			if (plural && pluralCount == 1)
			{
				plural = false;
			}
			string text = null;
			genderNoted = false;
			switch (gender)
			{
			case Gender.None:
				if (plural && kindDef.labelPlural != null)
				{
					text = kindDef.labelPlural;
				}
				else
				{
					text = kindDef.label;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text, gender, pluralCount);
					}
				}
				break;
			case Gender.Male:
				if (plural && kindDef.labelMalePlural != null)
				{
					text = kindDef.labelMalePlural;
					genderNoted = true;
				}
				else if (kindDef.labelMale != null)
				{
					text = kindDef.labelMale;
					genderNoted = true;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text, gender, pluralCount);
					}
				}
				else if (plural && kindDef.labelPlural != null)
				{
					text = kindDef.labelPlural;
				}
				else
				{
					text = kindDef.label;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text, gender, pluralCount);
					}
				}
				break;
			case Gender.Female:
				if (plural && kindDef.labelFemalePlural != null)
				{
					text = kindDef.labelFemalePlural;
					genderNoted = true;
				}
				else if (kindDef.labelFemale != null)
				{
					text = kindDef.labelFemale;
					genderNoted = true;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text, gender, pluralCount);
					}
				}
				else if (plural && kindDef.labelPlural != null)
				{
					text = kindDef.labelPlural;
				}
				else
				{
					text = kindDef.label;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text, gender, pluralCount);
					}
				}
				break;
			}
			return text;
		}

		// Token: 0x0600A038 RID: 41016 RVA: 0x002EE06C File Offset: 0x002EC26C
		public static string BestGroupLabel(List<Pawn> pawns, bool definite, out Pawn singlePawn)
		{
			singlePawn = null;
			if (!pawns.Any<Pawn>())
			{
				return "";
			}
			if (pawns.Count == 1)
			{
				singlePawn = pawns[0];
				if (!definite)
				{
					return pawns[0].LabelShort;
				}
				return pawns[0].LabelDefinite();
			}
			else
			{
				GenLabel.tmpHumanlikes.Clear();
				for (int i = 0; i < pawns.Count; i++)
				{
					if (!pawns[i].AnimalOrWildMan())
					{
						GenLabel.tmpHumanlikes.Add(pawns[i]);
					}
				}
				if (GenLabel.tmpHumanlikes.Any<Pawn>())
				{
					if (GenLabel.tmpHumanlikes.Count != 1)
					{
						GenLabel.tmpHumanlikeLabels.Clear();
						for (int j = 0; j < GenLabel.tmpHumanlikes.Count; j++)
						{
							if (GenLabel.tmpHumanlikes[j].Faction != null)
							{
								string text;
								if (definite)
								{
									text = Find.ActiveLanguageWorker.WithDefiniteArticle(GenLabel.tmpHumanlikes[j].Faction.def.pawnsPlural, LanguageDatabase.activeLanguage.ResolveGender(GenLabel.tmpHumanlikes[j].Faction.def.pawnsPlural, GenLabel.tmpHumanlikes[j].Faction.def.pawnSingular), true, false);
								}
								else
								{
									text = GenLabel.tmpHumanlikes[j].Faction.def.pawnsPlural;
								}
								if (!GenLabel.tmpHumanlikeLabels.ContainsKey(text))
								{
									GenLabel.tmpHumanlikeLabels.Add(text, 1);
								}
								else
								{
									Dictionary<string, int> dictionary = GenLabel.tmpHumanlikeLabels;
									string key = text;
									int num = dictionary[key];
									dictionary[key] = num + 1;
								}
							}
							else
							{
								string text2;
								if (definite)
								{
									text2 = Find.ActiveLanguageWorker.WithDefiniteArticle(GenLabel.tmpHumanlikes[j].kindDef.GetLabelPlural(-1), LanguageDatabase.activeLanguage.ResolveGender(GenLabel.tmpHumanlikes[j].kindDef.GetLabelPlural(-1), GenLabel.tmpHumanlikes[j].kindDef.label), true, false);
								}
								else
								{
									text2 = GenLabel.tmpHumanlikes[j].kindDef.GetLabelPlural(-1);
								}
								if (!GenLabel.tmpHumanlikeLabels.ContainsKey(text2))
								{
									GenLabel.tmpHumanlikeLabels.Add(text2, 1);
								}
								else
								{
									Dictionary<string, int> dictionary2 = GenLabel.tmpHumanlikeLabels;
									string key = text2;
									int num = dictionary2[key];
									dictionary2[key] = num + 1;
								}
							}
						}
						int num2 = -1;
						string result = null;
						foreach (KeyValuePair<string, int> keyValuePair in GenLabel.tmpHumanlikeLabels)
						{
							if (keyValuePair.Value > num2)
							{
								num2 = keyValuePair.Value;
								result = keyValuePair.Key;
							}
						}
						GenLabel.tmpHumanlikeLabels.Clear();
						GenLabel.tmpHumanlikes.Clear();
						return result;
					}
					singlePawn = GenLabel.tmpHumanlikes[0];
					if (!definite)
					{
						return GenLabel.tmpHumanlikes[0].LabelShort;
					}
					return GenLabel.tmpHumanlikes[0].LabelDefinite();
				}
				else
				{
					GenLabel.tmpLabels.Clear();
					for (int k = 0; k < pawns.Count; k++)
					{
						string text3;
						if (definite)
						{
							text3 = Find.ActiveLanguageWorker.WithDefiniteArticle(pawns[k].kindDef.GetLabelPlural(-1), LanguageDatabase.activeLanguage.ResolveGender(pawns[k].kindDef.GetLabelPlural(-1), pawns[k].kindDef.label), true, false);
						}
						else
						{
							text3 = pawns[k].kindDef.GetLabelPlural(-1);
						}
						if (!GenLabel.tmpLabels.ContainsKey(text3))
						{
							GenLabel.tmpLabels.Add(text3, 1);
						}
						else
						{
							Dictionary<string, int> dictionary3 = GenLabel.tmpLabels;
							string key = text3;
							int num = dictionary3[key];
							dictionary3[key] = num + 1;
						}
					}
					int num3 = -1;
					string result2 = null;
					foreach (KeyValuePair<string, int> keyValuePair2 in GenLabel.tmpLabels)
					{
						if (keyValuePair2.Value > num3)
						{
							num3 = keyValuePair2.Value;
							result2 = keyValuePair2.Key;
						}
					}
					GenLabel.tmpLabels.Clear();
					GenLabel.tmpHumanlikes.Clear();
					if ((float)num3 / (float)pawns.Count >= 0.5f)
					{
						return result2;
					}
					if (definite)
					{
						return Find.ActiveLanguageWorker.WithDefiniteArticle("AnimalsLower".Translate(), true, false);
					}
					return "AnimalsLower".Translate();
				}
			}
		}

		// Token: 0x04006CD3 RID: 27859
		private static Dictionary<GenLabel.LabelRequest, string> labelDictionary = new Dictionary<GenLabel.LabelRequest, string>();

		// Token: 0x04006CD4 RID: 27860
		private const int LabelDictionaryMaxCount = 2000;

		// Token: 0x04006CD5 RID: 27861
		private static List<ThingCount> tmpThingCounts = new List<ThingCount>();

		// Token: 0x04006CD6 RID: 27862
		private static List<GenLabel.LabelElement> tmpThingsLabelElements = new List<GenLabel.LabelElement>();

		// Token: 0x04006CD7 RID: 27863
		private static List<Pawn> tmpHumanlikes = new List<Pawn>();

		// Token: 0x04006CD8 RID: 27864
		private static Dictionary<string, int> tmpHumanlikeLabels = new Dictionary<string, int>();

		// Token: 0x04006CD9 RID: 27865
		private static Dictionary<string, int> tmpLabels = new Dictionary<string, int>();

		// Token: 0x02001CC4 RID: 7364
		private class LabelElement
		{
			// Token: 0x04006CDA RID: 27866
			public Thing thingTemplate;

			// Token: 0x04006CDB RID: 27867
			public int count;
		}

		// Token: 0x02001CC5 RID: 7365
		private struct LabelRequest : IEquatable<GenLabel.LabelRequest>
		{
			// Token: 0x0600A03B RID: 41019 RVA: 0x0006ACF0 File Offset: 0x00068EF0
			public static bool operator ==(GenLabel.LabelRequest lhs, GenLabel.LabelRequest rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x0600A03C RID: 41020 RVA: 0x0006ACFA File Offset: 0x00068EFA
			public static bool operator !=(GenLabel.LabelRequest lhs, GenLabel.LabelRequest rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x0600A03D RID: 41021 RVA: 0x0006AD06 File Offset: 0x00068F06
			public override bool Equals(object obj)
			{
				return obj is GenLabel.LabelRequest && this.Equals((GenLabel.LabelRequest)obj);
			}

			// Token: 0x0600A03E RID: 41022 RVA: 0x002EE4F4 File Offset: 0x002EC6F4
			public bool Equals(GenLabel.LabelRequest other)
			{
				return this.entDef == other.entDef && this.stuffDef == other.stuffDef && this.stackCount == other.stackCount && this.quality == other.quality && this.health == other.health && this.maxHealth == other.maxHealth && this.wornByCorpse == other.wornByCorpse;
			}

			// Token: 0x0600A03F RID: 41023 RVA: 0x002EE568 File Offset: 0x002EC768
			public override int GetHashCode()
			{
				int num = 0;
				num = Gen.HashCombine<BuildableDef>(num, this.entDef);
				num = Gen.HashCombine<ThingDef>(num, this.stuffDef);
				ThingDef thingDef = this.entDef as ThingDef;
				if (thingDef != null)
				{
					num = Gen.HashCombineInt(num, this.stackCount);
					num = Gen.HashCombineStruct<QualityCategory>(num, this.quality);
					if (thingDef.useHitPoints)
					{
						num = Gen.HashCombineInt(num, this.health);
						num = Gen.HashCombineInt(num, this.maxHealth);
					}
					num = Gen.HashCombineInt(num, this.wornByCorpse ? 1 : 0);
				}
				return num;
			}

			// Token: 0x04006CDC RID: 27868
			public BuildableDef entDef;

			// Token: 0x04006CDD RID: 27869
			public ThingDef stuffDef;

			// Token: 0x04006CDE RID: 27870
			public int stackCount;

			// Token: 0x04006CDF RID: 27871
			public QualityCategory quality;

			// Token: 0x04006CE0 RID: 27872
			public int health;

			// Token: 0x04006CE1 RID: 27873
			public int maxHealth;

			// Token: 0x04006CE2 RID: 27874
			public bool wornByCorpse;
		}
	}
}
