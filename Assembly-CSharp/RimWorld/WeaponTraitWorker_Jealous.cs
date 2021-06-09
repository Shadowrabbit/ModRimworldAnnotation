using System;

namespace RimWorld
{
	// Token: 0x02001012 RID: 4114
	public class WeaponTraitWorker_Jealous : WeaponTraitWorker
	{
		// Token: 0x060059C0 RID: 22976 RVA: 0x001D3214 File Offset: 0x001D1414
		public override void Notify_OtherWeaponWielded(CompBladelinkWeapon weapon)
		{
			if (weapon.bondedPawn == null || weapon.bondedPawn.Dead || weapon.bondedPawn.needs.mood == null)
			{
				return;
			}
			Thought_WeaponTrait thought_WeaponTrait = (Thought_WeaponTrait)ThoughtMaker.MakeThought(ThoughtDefOf.JealousRage);
			thought_WeaponTrait.weapon = weapon.parent;
			weapon.bondedPawn.needs.mood.thoughts.memories.TryGainMemory(thought_WeaponTrait, null);
		}
	}
}
