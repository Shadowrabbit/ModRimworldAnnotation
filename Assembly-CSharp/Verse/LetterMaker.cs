using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000723 RID: 1827
	public static class LetterMaker
	{
		// Token: 0x06002E10 RID: 11792 RVA: 0x000243C3 File Offset: 0x000225C3
		public static Letter MakeLetter(LetterDef def)
		{
			Letter letter = (Letter)Activator.CreateInstance(def.letterClass);
			letter.def = def;
			letter.ID = Find.UniqueIDsManager.GetNextLetterID();
			return letter;
		}

		// Token: 0x06002E11 RID: 11793 RVA: 0x00136850 File Offset: 0x00134A50
		public static ChoiceLetter MakeLetter(TaggedString label, TaggedString text, LetterDef def, Faction relatedFaction = null, Quest quest = null)
		{
			if (!typeof(ChoiceLetter).IsAssignableFrom(def.letterClass))
			{
				Log.Error(def + " is not a choice letter.", false);
				return null;
			}
			ChoiceLetter choiceLetter = (ChoiceLetter)LetterMaker.MakeLetter(def);
			choiceLetter.label = label;
			choiceLetter.text = text;
			choiceLetter.relatedFaction = relatedFaction;
			choiceLetter.quest = quest;
			return choiceLetter;
		}

		// Token: 0x06002E12 RID: 11794 RVA: 0x000243EC File Offset: 0x000225EC
		public static ChoiceLetter MakeLetter(TaggedString label, TaggedString text, LetterDef def, LookTargets lookTargets, Faction relatedFaction = null, Quest quest = null, List<ThingDef> hyperlinkThingDefs = null)
		{
			ChoiceLetter choiceLetter = LetterMaker.MakeLetter(label, text, def, null, null);
			choiceLetter.lookTargets = lookTargets;
			choiceLetter.relatedFaction = relatedFaction;
			choiceLetter.quest = quest;
			choiceLetter.hyperlinkThingDefs = hyperlinkThingDefs;
			return choiceLetter;
		}
	}
}
