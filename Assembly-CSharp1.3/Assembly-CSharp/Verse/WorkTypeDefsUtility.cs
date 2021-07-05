using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000121 RID: 289
	public static class WorkTypeDefsUtility
	{
		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060007B8 RID: 1976 RVA: 0x00023CB4 File Offset: 0x00021EB4
		public static IEnumerable<WorkTypeDef> WorkTypeDefsInPriorityOrder
		{
			get
			{
				return from wt in DefDatabase<WorkTypeDef>.AllDefs
				orderby wt.naturalPriority descending
				select wt;
			}
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x00023CE0 File Offset: 0x00021EE0
		public static string LabelTranslated(this WorkTags tags)
		{
			if (tags <= WorkTags.Crafting)
			{
				if (tags <= WorkTags.Social)
				{
					if (tags <= WorkTags.Violent)
					{
						switch (tags)
						{
						case WorkTags.None:
							return "WorkTagNone".Translate();
						case (WorkTags)1:
						case (WorkTags)3:
							break;
						case WorkTags.ManualDumb:
							return "WorkTagManualDumb".Translate();
						case WorkTags.ManualSkilled:
							return "WorkTagManualSkilled".Translate();
						default:
							if (tags == WorkTags.Violent)
							{
								return "WorkTagViolent".Translate();
							}
							break;
						}
					}
					else
					{
						if (tags == WorkTags.Caring)
						{
							return "WorkTagCaring".Translate();
						}
						if (tags == WorkTags.Social)
						{
							return "WorkTagSocial".Translate();
						}
					}
				}
				else if (tags <= WorkTags.Intellectual)
				{
					if (tags == WorkTags.Commoner)
					{
						return "WorkTagCommoner".Translate();
					}
					if (tags == WorkTags.Intellectual)
					{
						return "WorkTagIntellectual".Translate();
					}
				}
				else
				{
					if (tags == WorkTags.Animals)
					{
						return "WorkTagAnimals".Translate();
					}
					if (tags == WorkTags.Artistic)
					{
						return "WorkTagArtistic".Translate();
					}
					if (tags == WorkTags.Crafting)
					{
						return "WorkTagCrafting".Translate();
					}
				}
			}
			else if (tags <= WorkTags.PlantWork)
			{
				if (tags <= WorkTags.Firefighting)
				{
					if (tags == WorkTags.Cooking)
					{
						return "WorkTagCooking".Translate();
					}
					if (tags == WorkTags.Firefighting)
					{
						return "WorkTagFirefighting".Translate();
					}
				}
				else
				{
					if (tags == WorkTags.Cleaning)
					{
						return "WorkTagCleaning".Translate();
					}
					if (tags == WorkTags.Hauling)
					{
						return "WorkTagHauling".Translate();
					}
					if (tags == WorkTags.PlantWork)
					{
						return "WorkTagPlantWork".Translate();
					}
				}
			}
			else if (tags <= WorkTags.Hunting)
			{
				if (tags == WorkTags.Mining)
				{
					return "WorkTagMining".Translate();
				}
				if (tags == WorkTags.Hunting)
				{
					return "WorkTagHunting".Translate();
				}
			}
			else
			{
				if (tags == WorkTags.Constructing)
				{
					return "WorkTagConstructing".Translate();
				}
				if (tags == WorkTags.Shooting)
				{
					return "WorkTagShooting".Translate();
				}
				if (tags == WorkTags.AllWork)
				{
					return "WorkTagAllWork".Translate();
				}
			}
			Log.Error("Unknown or mixed worktags for naming: " + (int)tags);
			return "Worktag";
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x00023F88 File Offset: 0x00022188
		public static bool OverlapsWithOnAnyWorkType(this WorkTags a, WorkTags b)
		{
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[i];
				if ((workTypeDef.workTags & a) != WorkTags.None && (workTypeDef.workTags & b) != WorkTags.None)
				{
					return true;
				}
			}
			return false;
		}
	}
}
