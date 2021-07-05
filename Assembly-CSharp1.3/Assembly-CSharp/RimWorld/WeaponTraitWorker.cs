using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AEA RID: 2794
	public class WeaponTraitWorker
	{
		// Token: 0x060041BE RID: 16830 RVA: 0x001604B4 File Offset: 0x0015E6B4
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

		// Token: 0x060041BF RID: 16831 RVA: 0x00160528 File Offset: 0x0015E728
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

		// Token: 0x060041C0 RID: 16832 RVA: 0x0016059C File Offset: 0x0015E79C
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

		// Token: 0x060041C1 RID: 16833 RVA: 0x00160608 File Offset: 0x0015E808
		public virtual void Notify_KilledPawn(Pawn pawn)
		{
			if (this.def.killThought != null && pawn.needs.mood != null)
			{
				Thought_WeaponTrait thought_WeaponTrait = (Thought_WeaponTrait)ThoughtMaker.MakeThought(this.def.killThought);
				thought_WeaponTrait.weapon = pawn.equipment.Primary;
				pawn.needs.mood.thoughts.memories.TryGainMemory(thought_WeaponTrait, null);
			}
		}

		// Token: 0x060041C2 RID: 16834 RVA: 0x00160674 File Offset: 0x0015E874
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

		// Token: 0x060041C3 RID: 16835 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_OtherWeaponWielded(CompBladelinkWeapon weapon)
		{
		}

		// Token: 0x0400280E RID: 10254
		public WeaponTraitDef def;
	}
}
