using System;

namespace RimWorld
{
	// Token: 0x02000AEC RID: 2796
	public class WeaponTraitWorker_Jealous : WeaponTraitWorker
	{
		// Token: 0x060041C7 RID: 16839 RVA: 0x00160708 File Offset: 0x0015E908
		public override void Notify_OtherWeaponWielded(CompBladelinkWeapon weapon)
		{
			if (weapon.CodedPawn == null || weapon.CodedPawn.Dead || weapon.CodedPawn.needs.mood == null)
			{
				return;
			}
			Thought_WeaponTrait thought_WeaponTrait = (Thought_WeaponTrait)ThoughtMaker.MakeThought(ThoughtDefOf.JealousRage);
			thought_WeaponTrait.weapon = weapon.parent;
			weapon.CodedPawn.needs.mood.thoughts.memories.TryGainMemory(thought_WeaponTrait, null);
		}
	}
}
