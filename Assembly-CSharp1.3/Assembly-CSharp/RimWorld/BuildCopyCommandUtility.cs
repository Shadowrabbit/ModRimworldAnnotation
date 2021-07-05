using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020013E8 RID: 5096
	public static class BuildCopyCommandUtility
	{
		// Token: 0x06007BF5 RID: 31733 RVA: 0x002BC513 File Offset: 0x002BA713
		public static Command BuildCopyCommand(BuildableDef buildable, ThingDef stuff)
		{
			return BuildCopyCommandUtility.BuildCommand(buildable, stuff, "CommandBuildCopy".Translate(), "CommandBuildCopyDesc".Translate(), true);
		}

		// Token: 0x06007BF6 RID: 31734 RVA: 0x002BC53C File Offset: 0x002BA73C
		public static Command BuildCommand(BuildableDef buildable, ThingDef stuff, string label, string description, bool allowHotKey)
		{
			Designator_Build des = BuildCopyCommandUtility.FindAllowedDesignator(buildable, true);
			if (des == null)
			{
				return null;
			}
			if (buildable.MadeFromStuff && stuff == null)
			{
				return des;
			}
			Command_Action command_Action = new Command_Action();
			command_Action.action = delegate()
			{
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				Find.DesignatorManager.Select(des);
			};
			command_Action.defaultLabel = label;
			command_Action.defaultDesc = description;
			des.SetStuffDef(stuff);
			command_Action.icon = des.ResolvedIcon();
			command_Action.iconProportions = des.iconProportions;
			command_Action.iconDrawScale = des.iconDrawScale;
			command_Action.iconTexCoords = des.iconTexCoords;
			command_Action.iconAngle = des.iconAngle;
			command_Action.iconOffset = des.iconOffset;
			command_Action.order = 10f;
			command_Action.SetColorOverride(des.IconDrawColor);
			if (stuff != null)
			{
				command_Action.defaultIconColor = buildable.GetColorForStuff(stuff);
			}
			else
			{
				command_Action.defaultIconColor = buildable.uiIconColor;
			}
			if (allowHotKey)
			{
				command_Action.hotKey = KeyBindingDefOf.Misc11;
			}
			return command_Action;
		}

		// Token: 0x06007BF7 RID: 31735 RVA: 0x002BC65C File Offset: 0x002BA85C
		public static Designator_Build FindAllowedDesignator(BuildableDef buildable, bool mustBeVisible = true)
		{
			Game game = Current.Game;
			if (game != null)
			{
				if (BuildCopyCommandUtility.lastCacheTick != game.tickManager.TicksGame)
				{
					BuildCopyCommandUtility.cache.Clear();
					BuildCopyCommandUtility.lastCacheTick = game.tickManager.TicksGame;
				}
				if (BuildCopyCommandUtility.cache.ContainsKey(buildable))
				{
					return BuildCopyCommandUtility.cache[buildable];
				}
			}
			else
			{
				BuildCopyCommandUtility.cache.Clear();
			}
			List<DesignationCategoryDef> allDefsListForReading = DefDatabase<DesignationCategoryDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				foreach (Designator designator in allDefsListForReading[i].AllResolvedAndIdeoDesignators)
				{
					Designator_Build designator_Build = BuildCopyCommandUtility.FindAllowedDesignatorRecursive(designator, buildable, mustBeVisible);
					if (designator_Build != null)
					{
						if (!BuildCopyCommandUtility.cache.ContainsKey(buildable))
						{
							BuildCopyCommandUtility.cache.Add(buildable, designator_Build);
						}
						return designator_Build;
					}
				}
			}
			if (!BuildCopyCommandUtility.cache.ContainsKey(buildable))
			{
				BuildCopyCommandUtility.cache.Add(buildable, null);
			}
			return null;
		}

		// Token: 0x06007BF8 RID: 31736 RVA: 0x002BC760 File Offset: 0x002BA960
		public static Designator FindAllowedDesignatorRoot(BuildableDef buildable, bool mustBeVisible = true)
		{
			List<Designator> allResolvedDesignators = buildable.designationCategory.AllResolvedDesignators;
			for (int i = 0; i < allResolvedDesignators.Count; i++)
			{
				if (BuildCopyCommandUtility.FindAllowedDesignatorRecursive(allResolvedDesignators[i], buildable, mustBeVisible) != null)
				{
					return allResolvedDesignators[i];
				}
			}
			return null;
		}

		// Token: 0x06007BF9 RID: 31737 RVA: 0x002BC7A4 File Offset: 0x002BA9A4
		private static Designator_Build FindAllowedDesignatorRecursive(Designator designator, BuildableDef buildable, bool mustBeVisible)
		{
			if (!Current.Game.Rules.DesignatorAllowed(designator))
			{
				return null;
			}
			if (mustBeVisible && !designator.Visible)
			{
				return null;
			}
			Designator_Build designator_Build = designator as Designator_Build;
			if (designator_Build != null && designator_Build.PlacingDef == buildable)
			{
				return designator_Build;
			}
			Designator_Dropdown designator_Dropdown = designator as Designator_Dropdown;
			if (designator_Dropdown != null)
			{
				for (int i = 0; i < designator_Dropdown.Elements.Count; i++)
				{
					Designator_Build designator_Build2 = BuildCopyCommandUtility.FindAllowedDesignatorRecursive(designator_Dropdown.Elements[i], buildable, mustBeVisible);
					if (designator_Build2 != null)
					{
						return designator_Build2;
					}
				}
			}
			return null;
		}

		// Token: 0x040044AF RID: 17583
		private static Dictionary<BuildableDef, Designator_Build> cache = new Dictionary<BuildableDef, Designator_Build>();

		// Token: 0x040044B0 RID: 17584
		private static int lastCacheTick = -1;
	}
}
