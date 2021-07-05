using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FCE RID: 4046
	public class Precept_RoleMulti : Precept_Role
	{
		// Token: 0x06005F57 RID: 24407 RVA: 0x0020A0D6 File Offset: 0x002082D6
		public override IEnumerable<Pawn> ChosenPawns()
		{
			foreach (IdeoRoleInstance ideoRoleInstance in this.chosenPawns)
			{
				yield return ideoRoleInstance.pawn;
			}
			List<IdeoRoleInstance>.Enumerator enumerator = default(List<IdeoRoleInstance>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06005F58 RID: 24408 RVA: 0x00002688 File Offset: 0x00000888
		public override Pawn ChosenPawnSingle()
		{
			return null;
		}

		// Token: 0x06005F59 RID: 24409 RVA: 0x0020A0E6 File Offset: 0x002082E6
		public override void Init(Ideo ideo, FactionDef generatingFor = null)
		{
			base.Init(ideo, generatingFor);
			this.active = true;
		}

		// Token: 0x06005F5A RID: 24410 RVA: 0x0020A0F8 File Offset: 0x002082F8
		public override void Assign(Pawn p, bool addThoughts)
		{
			if (!this.IsAssigned(p))
			{
				IdeoRoleInstance ideoRoleInstance = this.chosenPawnsCache.FirstOrDefault((IdeoRoleInstance c) => c.pawn == p);
				if (ideoRoleInstance != null)
				{
					this.chosenPawnsCache.Remove(ideoRoleInstance);
					this.chosenPawns.Add(ideoRoleInstance);
				}
				else
				{
					this.chosenPawns.Add(new IdeoRoleInstance(this)
					{
						pawn = p
					});
					this.FillOrUpdateAbilities();
				}
				base.Notify_PawnAssigned(p);
			}
		}

		// Token: 0x06005F5B RID: 24411 RVA: 0x0020A188 File Offset: 0x00208388
		public override void FillOrUpdateAbilities()
		{
			foreach (IdeoRoleInstance ideoRoleInstance in this.chosenPawns)
			{
				ideoRoleInstance.abilities = base.FillOrUpdateAbilityList(ideoRoleInstance.pawn, ideoRoleInstance.abilities);
			}
		}

		// Token: 0x06005F5C RID: 24412 RVA: 0x0020A1EC File Offset: 0x002083EC
		public override List<Ability> AbilitiesFor(Pawn p)
		{
			for (int i = 0; i < this.chosenPawns.Count; i++)
			{
				if (this.chosenPawns[i].pawn == p)
				{
					return this.chosenPawns[i].abilities;
				}
			}
			return null;
		}

		// Token: 0x06005F5D RID: 24413 RVA: 0x0020A238 File Offset: 0x00208438
		public override bool IsAssigned(Pawn p)
		{
			for (int i = 0; i < this.chosenPawns.Count; i++)
			{
				if (this.chosenPawns[i].pawn == p)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005F5E RID: 24414 RVA: 0x0020A274 File Offset: 0x00208474
		public override void Unassign(Pawn p, bool generateThoughts)
		{
			if (!this.IsAssigned(p))
			{
				return;
			}
			IdeoRoleInstance ideoRoleInstance = this.chosenPawns.FirstOrDefault((IdeoRoleInstance c) => c.pawn == p);
			if (ideoRoleInstance != null)
			{
				this.chosenPawns.Remove(ideoRoleInstance);
				this.chosenPawnsCache.Add(ideoRoleInstance);
				p.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(ThoughtDefOf.IdeoRoleLost, this), null);
				base.Notify_PawnUnassigned(p);
			}
		}

		// Token: 0x06005F5F RID: 24415 RVA: 0x0020A308 File Offset: 0x00208508
		public override void RecacheActivity()
		{
			this.pawnsToRemove.Clear();
			foreach (IdeoRoleInstance ideoRoleInstance in this.chosenPawns)
			{
				if (ideoRoleInstance.pawn == null || !base.ValidatePawn(ideoRoleInstance.pawn))
				{
					this.pawnsToRemove.Add(ideoRoleInstance);
				}
			}
			foreach (IdeoRoleInstance ideoRoleInstance2 in this.pawnsToRemove)
			{
				this.Unassign(ideoRoleInstance2.pawn, false);
				if (this.chosenPawns.Contains(ideoRoleInstance2))
				{
					this.chosenPawns.Remove(ideoRoleInstance2);
				}
			}
		}

		// Token: 0x06005F60 RID: 24416 RVA: 0x0020A3E4 File Offset: 0x002085E4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IdeoRoleInstance>(ref this.chosenPawns, "chosenPawns", LookMode.Deep, new object[]
			{
				this
			});
			Scribe_Collections.Look<IdeoRoleInstance>(ref this.chosenPawnsCache, "chosenPawnsCache", LookMode.Deep, new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.chosenPawns.RemoveAll((IdeoRoleInstance c) => c.pawn == null || !base.ValidatePawn(c.pawn));
				this.chosenPawnsCache.RemoveAll((IdeoRoleInstance c) => c.pawn == null);
				foreach (IdeoRoleInstance ideoRoleInstance in this.chosenPawns)
				{
					ideoRoleInstance.sourceRole = this;
				}
			}
		}

		// Token: 0x040036D9 RID: 14041
		public List<IdeoRoleInstance> chosenPawns = new List<IdeoRoleInstance>();

		// Token: 0x040036DA RID: 14042
		private List<IdeoRoleInstance> chosenPawnsCache = new List<IdeoRoleInstance>();

		// Token: 0x040036DB RID: 14043
		private List<IdeoRoleInstance> pawnsToRemove = new List<IdeoRoleInstance>();
	}
}
