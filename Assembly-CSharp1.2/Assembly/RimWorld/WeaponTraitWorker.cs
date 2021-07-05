using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001010 RID: 4112
	public class WeaponTraitWorker
	{
		// Token: 0x060059B7 RID: 22967 RVA: 0x001D2FE8 File Offset: 0x001D11E8
		public virtual void Notify_Bonded(Pawn pawn)
		{
			if (!this.def.bondedHediffs.NullOrEmpty<HediffDef>())
			{
				for (int i = 0; i < this.def.bondedHediffs.Count; i++)
				{
					pawn.health.AddHediff(this.def.bondedHediffs[i], pawn.health.hediffSet.GetBrain(), null, null);
				}
			}
		}

		// Token: 0x060059B8 RID: 22968 RVA: 0x001D305C File Offset: 0x001D125C
		public virtual void Notify_Equipped(Pawn pawn)
		{
			if (!this.def.equippedHediffs.NullOrEmpty<HediffDef>())
			{
				for (int i = 0; i < this.def.equippedHediffs.Count; i++)
				{
					pawn.health.AddHediff(this.def.equippedHediffs[i], pawn.health.hediffSet.GetBrain(), null, null);
				}
			}
		}

		// Token: 0x060059B9 RID: 22969 RVA: 0x001D30D0 File Offset: 0x001D12D0
		public virtual void Notify_EquipmentLost(Pawn pawn)
		{
			if (!this.def.equippedHediffs.NullOrEmpty<HediffDef>())
			{
				for (int i = 0; i < this.def.equippedHediffs.Count; i++)
				{
					Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(this.def.equippedHediffs[i], false);
					if (firstHediffOfDef != null)
					{
						pawn.health.RemoveHediff(firstHediffOfDef);
					}
				}
			}
		}

		// Token: 0x060059BA RID: 22970 RVA: 0x001D313C File Offset: 0x001D133C
		public virtual void Notify_KilledPawn(Pawn pawn)
		{
			if (this.def.killThought != null && pawn.needs.mood != null)
			{
				Thought_WeaponTrait thought_WeaponTrait = (Thought_WeaponTrait)ThoughtMaker.MakeThought(this.def.killThought);
				thought_WeaponTrait.weapon = pawn.equipment.Primary;
				pawn.needs.mood.thoughts.memories.TryGainMemory(thought_WeaponTrait, null);
			}
		}

		// Token: 0x060059BB RID: 22971 RVA: 0x001D31A8 File Offset: 0x001D13A8
		public virtual void Notify_Unbonded(Pawn pawn)
		{
			if (!this.def.bondedHediffs.NullOrEmpty<HediffDef>())
			{
				for (int i = 0; i < this.def.bondedHediffs.Count; i++)
				{
					Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(this.def.bondedHediffs[i], false);
					if (firstHediffOfDef != null)
					{
						pawn.health.RemoveHediff(firstHediffOfDef);
					}
				}
			}
		}

		// Token: 0x060059BC RID: 22972 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_OtherWeaponWielded(CompBladelinkWeapon weapon)
		{
		}

		// Token: 0x04003C65 RID: 15461
		public WeaponTraitDef def;
	}
}
