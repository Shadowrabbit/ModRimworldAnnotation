using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020014FF RID: 5375
	public class Pawn_AbilityTracker : IExposable
	{
		// Token: 0x060073BF RID: 29631 RVA: 0x0004E09B File Offset: 0x0004C29B
		public Pawn_AbilityTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060073C0 RID: 29632 RVA: 0x00234E38 File Offset: 0x00233038
		public void AbilitiesTick()
		{
			for (int i = 0; i < this.abilities.Count; i++)
			{
				this.abilities[i].AbilityTick();
			}
		}

		// Token: 0x060073C1 RID: 29633 RVA: 0x00234E6C File Offset: 0x0023306C
		public void GainAbility(AbilityDef def)
		{
			if (!this.abilities.Any((Ability a) => a.def == def))
			{
				this.abilities.Add(Activator.CreateInstance(def.abilityClass, new object[]
				{
					this.pawn,
					def
				}) as Ability);
			}
		}

		// Token: 0x060073C2 RID: 29634 RVA: 0x00234ED8 File Offset: 0x002330D8
		public void RemoveAbility(AbilityDef def)
		{
			Ability ability = this.abilities.FirstOrDefault((Ability x) => x.def == def);
			if (ability != null)
			{
				this.abilities.Remove(ability);
			}
		}

		// Token: 0x060073C3 RID: 29635 RVA: 0x00234F1C File Offset: 0x0023311C
		public Ability GetAbility(AbilityDef def)
		{
			for (int i = 0; i < this.abilities.Count; i++)
			{
				if (this.abilities[i].def == def)
				{
					return this.abilities[i];
				}
			}
			return null;
		}

		// Token: 0x060073C4 RID: 29636 RVA: 0x0004E0B5 File Offset: 0x0004C2B5
		public IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Ability ability in this.abilities.OrderBy(delegate(Ability a)
			{
				if (a.def.category == null)
				{
					return 0;
				}
				return a.def.category.displayOrder;
			}).ThenBy((Ability a) => a.def.level))
			{
				if (this.pawn.Drafted || ability.def.displayGizmoWhileUndrafted)
				{
					foreach (Command command in ability.GetGizmos())
					{
						yield return command;
					}
					IEnumerator<Command> enumerator2 = null;
				}
			}
			IEnumerator<Ability> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060073C5 RID: 29637 RVA: 0x00234F64 File Offset: 0x00233164
		public void ExposeData()
		{
			Scribe_Collections.Look<Ability>(ref this.abilities, "abilities", LookMode.Deep, new object[]
			{
				this.pawn
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.abilities.RemoveAll((Ability a) => a.def == null);
			}
		}

		// Token: 0x04004C77 RID: 19575
		public Pawn pawn;

		// Token: 0x04004C78 RID: 19576
		public List<Ability> abilities = new List<Ability>();
	}
}
