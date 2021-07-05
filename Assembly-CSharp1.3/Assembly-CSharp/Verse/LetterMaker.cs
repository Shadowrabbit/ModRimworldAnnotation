using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000403 RID: 1027
	public static class LetterMaker
	{
		// Token: 0x06001EBD RID: 7869 RVA: 0x000C040A File Offset: 0x000BE60A
		public static Letter MakeLetter(LetterDef def)
		{
			Letter letter = (Letter)Activator.CreateInstance(def.letterClass);
			letter.def = def;
			letter.ID = Find.UniqueIDsManager.GetNextLetterID();
			return letter;
		}

		// Token: 0x06001EBE RID: 7870 RVA: 0x000C0434 File Offset: 0x000BE634
		public static ChoiceLetter MakeLetter(TaggedString label, TaggedString text, LetterDef def, Faction relatedFaction = null, Quest quest = null)
		{
			if (!typeof(ChoiceLetter).IsAssignableFrom(def.letterClass))
			{
				Log.Error(def + " is not a choice letter.");
				return null;
			}
			ChoiceLetter choiceLetter = (ChoiceLetter)LetterMaker.MakeLetter(def);
			choiceLetter.label = label;
			choiceLetter.text = text;
			choiceLetter.relatedFaction = relatedFaction;
			choiceLetter.quest = quest;
			return choiceLetter;
		}

		// Token: 0x06001EBF RID: 7871 RVA: 0x000C0492 File Offset: 0x000BE692
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
