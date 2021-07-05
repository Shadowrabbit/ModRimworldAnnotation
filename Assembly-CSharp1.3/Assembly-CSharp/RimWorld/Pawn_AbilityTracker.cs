using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E62 RID: 3682
	public class Pawn_AbilityTracker : IExposable
	{
		// Token: 0x17000EB0 RID: 3760
		// (get) Token: 0x06005535 RID: 21813 RVA: 0x001CD8C4 File Offset: 0x001CBAC4
		public List<Ability> AllAbilitiesForReading
		{
			get
			{
				if (this.allAbilitiesCachedDirty)
				{
					this.allAbilitiesCached.Clear();
					this.allAbilitiesCached.AddRange(this.abilities);
					if (this.pawn.royalty != null)
					{
						this.allAbilitiesCached.AddRange(this.pawn.royalty.AllAbilitiesForReading);
					}
					if (ModsConfig.IdeologyActive)
					{
						Ideo ideo = this.pawn.Ideo;
						Precept_Role precept_Role = (ideo != null) ? ideo.GetRole(this.pawn) : null;
						if (precept_Role != null && precept_Role.Active && !precept_Role.AbilitiesFor(this.pawn).NullOrEmpty<Ability>())
						{
							foreach (Ability ability in precept_Role.AbilitiesFor(this.pawn))
							{
								bool flag = false;
								if (!ability.def.requiredMemes.NullOrEmpty<MemeDef>())
								{
									foreach (MemeDef item in ability.def.requiredMemes)
									{
										if (!this.pawn.Ideo.memes.Contains(item))
										{
											flag = true;
											break;
										}
									}
								}
								if (!flag)
								{
									this.allAbilitiesCached.Add(ability);
								}
							}
						}
					}
					this.allAbilitiesCached.SortBy(delegate(Ability a)
					{
						AbilityCategoryDef category = a.def.category;
						if (category == null)
						{
							return 0;
						}
						return category.displayOrder;
					}, (Ability a) => a.def.level);
					this.allAbilitiesCachedDirty = false;
				}
				return this.allAbilitiesCached;
			}
		}

		// Token: 0x06005536 RID: 21814 RVA: 0x001CDA90 File Offset: 0x001CBC90
		public Pawn_AbilityTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06005537 RID: 21815 RVA: 0x001CDABC File Offset: 0x001CBCBC
		public void AbilitiesTick()
		{
			for (int i = 0; i < this.AllAbilitiesForReading.Count; i++)
			{
				this.AllAbilitiesForReading[i].AbilityTick();
			}
		}

		// Token: 0x06005538 RID: 21816 RVA: 0x001CDAF0 File Offset: 0x001CBCF0
		public void GainAbility(AbilityDef def)
		{
			if (!this.abilities.Any((Ability a) => a.def == def))
			{
				this.abilities.Add(AbilityUtility.MakeAbility(def, this.pawn));
			}
			this.Notify_TemporaryAbilitiesChanged();
		}

		// Token: 0x06005539 RID: 21817 RVA: 0x001CDB48 File Offset: 0x001CBD48
		public void RemoveAbility(AbilityDef def)
		{
			Ability ability = this.abilities.FirstOrDefault((Ability x) => x.def == def);
			if (ability != null)
			{
				this.abilities.Remove(ability);
			}
			this.Notify_TemporaryAbilitiesChanged();
		}

		// Token: 0x0600553A RID: 21818 RVA: 0x001CDB90 File Offset: 0x001CBD90
		public Ability GetAbility(AbilityDef def, bool includeTemporary = false)
		{
			List<Ability> list = includeTemporary ? this.AllAbilitiesForReading : this.abilities;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def == def)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x0600553B RID: 21819 RVA: 0x001CDBD8 File Offset: 0x001CBDD8
		public IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Ability ability in this.AllAbilitiesForReading)
			{
				if ((this.pawn.Drafted || ability.def.displayGizmoWhileUndrafted) && ability.GizmosVisible())
				{
					foreach (Command command in ability.GetGizmos())
					{
						yield return command;
					}
					IEnumerator<Command> enumerator2 = null;
				}
			}
			List<Ability>.Enumerator enumerator = default(List<Ability>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600553C RID: 21820 RVA: 0x001CDBE8 File Offset: 0x001CBDE8
		public void Notify_TemporaryAbilitiesChanged()
		{
			this.allAbilitiesCachedDirty = true;
		}

		// Token: 0x0600553D RID: 21821 RVA: 0x001CDBF4 File Offset: 0x001CBDF4
		public void ExposeData()
		{
			Scribe_Collections.Look<Ability>(ref this.abilities, "abilities", LookMode.Deep, new object[]
			{
				this.pawn
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.abilities.RemoveAll((Ability a) => a.def == null || a.def == AbilityDefOf.Speech);
			}
		}

		// Token: 0x0400327E RID: 12926
		public Pawn pawn;

		// Token: 0x0400327F RID: 12927
		public List<Ability> abilities = new List<Ability>();

		// Token: 0x04003280 RID: 12928
		private bool allAbilitiesCachedDirty = true;

		// Token: 0x04003281 RID: 12929
		private List<Ability> allAbilitiesCached = new List<Ability>();
	}
}
