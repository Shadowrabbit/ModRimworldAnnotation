using System;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F10 RID: 7952
	public class QuestNode_GetAnimalKindByPoints : QuestNode
	{
		// Token: 0x0600AA32 RID: 43570 RVA: 0x0006F90D File Offset: 0x0006DB0D
		protected override bool TestRunInt(Slate slate)
		{
			return this.SetVars(slate);
		}

		// Token: 0x0600AA33 RID: 43571 RVA: 0x0006F916 File Offset: 0x0006DB16
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600AA34 RID: 43572 RVA: 0x0031B0C8 File Offset: 0x003192C8
		private bool SetVars(Slate slate)
		{
			float points = slate.Get<float>("points", 0f, false);
			PawnKindDef var;
			if ((from x in DefDatabase<PawnKindDef>.AllDefs
			where x.RaceProps.Animal && x.combatPower < points
			select x).TryRandomElement(out var))
			{
				slate.Set<PawnKindDef>("animalKindDef", var, false);
				return true;
			}
			return false;
		}
	}
}
