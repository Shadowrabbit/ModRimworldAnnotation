using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E66 RID: 3686
	public class ApparelRequirement : IExposable
	{
		// Token: 0x17000EC3 RID: 3779
		// (get) Token: 0x06005579 RID: 21881 RVA: 0x001CEF68 File Offset: 0x001CD168
		public bool IsValid
		{
			get
			{
				if (this.isValid == null)
				{
					this.isValid = new bool?(this.AllRequiredApparel(Gender.None).Any<ThingDef>());
				}
				return this.isValid.Value;
			}
		}

		// Token: 0x0600557A RID: 21882 RVA: 0x001CEF9C File Offset: 0x001CD19C
		private bool MatchesBodyPartGroups(ThingDef apparel)
		{
			foreach (BodyPartGroupDef item in this.bodyPartGroupsMatchAny)
			{
				if (apparel.apparel.bodyPartGroups.Contains(item))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600557B RID: 21883 RVA: 0x001CF004 File Offset: 0x001CD204
		private bool HasRequiredTag(ThingDef apparel)
		{
			if (this.requiredTags != null)
			{
				foreach (string item in apparel.apparel.tags)
				{
					if (this.requiredTags.Contains(item))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x0600557C RID: 21884 RVA: 0x001CF074 File Offset: 0x001CD274
		private static bool WearsAny(Pawn p, ThingDef apparel)
		{
			using (List<Apparel>.Enumerator enumerator = p.apparel.WornApparel.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def == apparel)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600557D RID: 21885 RVA: 0x001CF0D4 File Offset: 0x001CD2D4
		public bool AllowedForPawn(Pawn p, ThingDef apparel, bool ignoreGender = false)
		{
			if (!apparel.IsApparel || apparel.apparel.tags == null || (!ignoreGender && !apparel.apparel.CorrectGenderForWearing(p.gender)))
			{
				return false;
			}
			bool flag = this.requiredDefs != null && this.requiredDefs.Contains(apparel);
			if (!flag)
			{
				foreach (string item in apparel.apparel.tags)
				{
					if ((this.requiredTags != null && this.requiredTags.Contains(item)) || (this.allowedTags != null && this.allowedTags.Contains(item)))
					{
						flag = true;
						break;
					}
				}
			}
			return flag && this.MatchesBodyPartGroups(apparel);
		}

		// Token: 0x0600557E RID: 21886 RVA: 0x001CF1AC File Offset: 0x001CD3AC
		public IEnumerable<ThingDef> AllAllowedApparelForPawn(Pawn p, bool ignoreGender = false, bool includeWorn = false)
		{
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
			{
				if (this.AllowedForPawn(p, thingDef, ignoreGender) && (includeWorn || !ApparelRequirement.WearsAny(p, thingDef)))
				{
					yield return thingDef;
				}
			}
			List<ThingDef>.Enumerator enumerator = default(List<ThingDef>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600557F RID: 21887 RVA: 0x001CF1D4 File Offset: 0x001CD3D4
		public bool RequiredForPawn(Pawn p, ThingDef apparel, bool ignoreGender = false)
		{
			return apparel.IsApparel && apparel.apparel.tags != null && (ignoreGender || apparel.apparel.CorrectGenderForWearing(p.gender)) && ((this.requiredDefs != null && this.requiredDefs.Contains(apparel)) || this.HasRequiredTag(apparel)) && this.MatchesBodyPartGroups(apparel);
		}

		// Token: 0x06005580 RID: 21888 RVA: 0x001CF239 File Offset: 0x001CD439
		public IEnumerable<ThingDef> AllRequiredApparelForPawn(Pawn p, bool ignoreGender = false, bool includeWorn = false)
		{
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
			{
				if (this.RequiredForPawn(p, thingDef, ignoreGender) && (includeWorn || !ApparelRequirement.WearsAny(p, thingDef)))
				{
					yield return thingDef;
				}
			}
			List<ThingDef>.Enumerator enumerator = default(List<ThingDef>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06005581 RID: 21889 RVA: 0x001CF25E File Offset: 0x001CD45E
		public IEnumerable<ThingDef> AllRequiredApparel(Gender gender = Gender.None)
		{
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
			{
				if (thingDef.IsApparel && thingDef.apparel.tags != null && ((this.requiredDefs != null && this.requiredDefs.Contains(thingDef)) || this.HasRequiredTag(thingDef)) && this.MatchesBodyPartGroups(thingDef) && (gender == Gender.None || thingDef.apparel.CorrectGenderForWearing(gender)))
				{
					yield return thingDef;
				}
			}
			List<ThingDef>.Enumerator enumerator = default(List<ThingDef>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06005582 RID: 21890 RVA: 0x001CF278 File Offset: 0x001CD478
		public bool ApparelMeetsRequirement(ThingDef thingDef, bool allowUnmatched = true)
		{
			bool flag = false;
			for (int i = 0; i < this.bodyPartGroupsMatchAny.Count; i++)
			{
				if (thingDef.apparel.bodyPartGroups.Contains(this.bodyPartGroupsMatchAny[i]))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return allowUnmatched;
			}
			if (this.requiredDefs != null && this.requiredDefs.Contains(thingDef))
			{
				return true;
			}
			if (this.requiredTags != null)
			{
				for (int j = 0; j < this.requiredTags.Count; j++)
				{
					if (thingDef.apparel.tags.Contains(this.requiredTags[j]))
					{
						return true;
					}
				}
			}
			if (this.allowedTags != null)
			{
				for (int k = 0; k < this.allowedTags.Count; k++)
				{
					if (thingDef.apparel.tags.Contains(this.allowedTags[k]))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06005583 RID: 21891 RVA: 0x001CF35C File Offset: 0x001CD55C
		public bool IsMet(Pawn p)
		{
			foreach (Apparel apparel in p.apparel.WornApparel)
			{
				bool flag = false;
				for (int i = 0; i < this.bodyPartGroupsMatchAny.Count; i++)
				{
					if (apparel.def.apparel.bodyPartGroups.Contains(this.bodyPartGroupsMatchAny[i]))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					if (this.requiredDefs != null && this.requiredDefs.Contains(apparel.def))
					{
						return true;
					}
					if (this.requiredTags != null)
					{
						for (int j = 0; j < this.requiredTags.Count; j++)
						{
							if (apparel.def.apparel.tags.Contains(this.requiredTags[j]))
							{
								return true;
							}
						}
					}
					if (this.allowedTags != null)
					{
						for (int k = 0; k < this.allowedTags.Count; k++)
						{
							if (apparel.def.apparel.tags.Contains(this.allowedTags[k]))
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06005584 RID: 21892 RVA: 0x001CF4C4 File Offset: 0x001CD6C4
		public bool SameApparelAs(ApparelRequirement other)
		{
			return this.requiredDefs.SetsEqual(other.requiredDefs) && this.requiredTags.SetsEqual(other.requiredTags) && this.allowedTags.SetsEqual(other.allowedTags);
		}

		// Token: 0x06005585 RID: 21893 RVA: 0x001CF4FF File Offset: 0x001CD6FF
		public bool IsActive(Pawn forPawn)
		{
			return forPawn.apparel != null && forPawn.apparel.ActiveRequirementsForReading.Contains(this);
		}

		// Token: 0x06005586 RID: 21894 RVA: 0x001CF51C File Offset: 0x001CD71C
		public ThingDef RandomRequiredApparelForPawnInGeneration(Pawn p, Func<ThingDef, bool> validator)
		{
			ThingDef result = null;
			Predicate<BodyPartGroupDef> <>9__2;
			Predicate<string> <>9__3;
			if (!DefDatabase<ThingDef>.AllDefsListForReading.Where(delegate(ThingDef a)
			{
				if (a.IsApparel && a.apparel.tags != null)
				{
					List<BodyPartGroupDef> bodyPartGroups = a.apparel.bodyPartGroups;
					Predicate<BodyPartGroupDef> predicate;
					if ((predicate = <>9__2) == null)
					{
						predicate = (<>9__2 = ((BodyPartGroupDef b) => this.bodyPartGroupsMatchAny.Contains(b)));
					}
					if (bodyPartGroups.Any(predicate))
					{
						List<string> tags = a.apparel.tags;
						Predicate<string> predicate2;
						if ((predicate2 = <>9__3) == null)
						{
							predicate2 = (<>9__3 = ((string t) => this.requiredTags.Contains(t)));
						}
						if (tags.Any(predicate2) && a.apparel.CorrectGenderForWearing(p.gender))
						{
							return validator == null || validator(a);
						}
					}
				}
				return false;
			}).TryRandomElementByWeight((ThingDef a) => a.generateCommonality, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06005587 RID: 21895 RVA: 0x001CF588 File Offset: 0x001CD788
		public override string ToString()
		{
			string text = "";
			if (this.requiredTags != null)
			{
				text += string.Join(",", this.requiredTags.ToArray());
			}
			if (this.allowedTags != null)
			{
				if (!text.NullOrEmpty())
				{
					text += "|";
				}
				text += string.Join(",", this.allowedTags.ToArray());
			}
			if (this.requiredDefs != null)
			{
				if (!text.NullOrEmpty())
				{
					text += "|";
				}
				text += string.Join<TaggedString>(",", (from d in this.requiredDefs
				select d.LabelCap).ToArray<TaggedString>());
			}
			return "(" + string.Join(",", (from a in this.bodyPartGroupsMatchAny
			select a.defName).ToArray<string>()) + ") -> " + text;
		}

		// Token: 0x06005588 RID: 21896 RVA: 0x001CF69C File Offset: 0x001CD89C
		public void ExposeData()
		{
			Scribe_Collections.Look<BodyPartGroupDef>(ref this.bodyPartGroupsMatchAny, "bodyPartGroupsMatchAny", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<ThingDef>(ref this.requiredDefs, "requiredDefs", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.requiredTags, "requiredTags", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.allowedTags, "allowedTags", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.groupLabel, "groupLabel", null, false);
		}

		// Token: 0x04003296 RID: 12950
		public List<BodyPartGroupDef> bodyPartGroupsMatchAny;

		// Token: 0x04003297 RID: 12951
		public List<string> requiredTags;

		// Token: 0x04003298 RID: 12952
		public List<string> allowedTags;

		// Token: 0x04003299 RID: 12953
		public List<ThingDef> requiredDefs;

		// Token: 0x0400329A RID: 12954
		[MustTranslate]
		public string groupLabel;

		// Token: 0x0400329B RID: 12955
		[Unsaved(false)]
		private bool? isValid;
	}
}
