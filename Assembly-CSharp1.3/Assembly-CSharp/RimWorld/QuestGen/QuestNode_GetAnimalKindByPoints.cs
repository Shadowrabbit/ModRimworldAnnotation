using System;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001667 RID: 5735
	public class QuestNode_GetAnimalKindByPoints : QuestNode
	{
		// Token: 0x060085A0 RID: 34208 RVA: 0x002FF07D File Offset: 0x002FD27D
		protected override bool TestRunInt(Slate slate)
		{
			return this.SetVars(slate);
		}

		// Token: 0x060085A1 RID: 34209 RVA: 0x002FF086 File Offset: 0x002FD286
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x060085A2 RID: 34210 RVA: 0x002FF094 File Offset: 0x002FD294
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
