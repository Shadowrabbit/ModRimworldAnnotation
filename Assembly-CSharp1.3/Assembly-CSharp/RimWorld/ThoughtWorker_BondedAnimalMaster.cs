using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000980 RID: 2432
	public class ThoughtWorker_BondedAnimalMaster : ThoughtWorker
	{
		// Token: 0x06003D84 RID: 15748 RVA: 0x0015263C File Offset: 0x0015083C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtWorker_BondedAnimalMaster.tmpAnimals.Clear();
			this.GetAnimals(p, ThoughtWorker_BondedAnimalMaster.tmpAnimals);
			if (!ThoughtWorker_BondedAnimalMaster.tmpAnimals.Any<string>())
			{
				return false;
			}
			if (ThoughtWorker_BondedAnimalMaster.tmpAnimals.Count == 1)
			{
				return ThoughtState.ActiveAtStage(0, ThoughtWorker_BondedAnimalMaster.tmpAnimals[0]);
			}
			return ThoughtState.ActiveAtStage(1, ThoughtWorker_BondedAnimalMaster.tmpAnimals.ToCommaList(true, false));
		}

		// Token: 0x06003D85 RID: 15749 RVA: 0x001526A3 File Offset: 0x001508A3
		protected virtual bool AnimalMasterCheck(Pawn p, Pawn animal)
		{
			return animal.playerSettings.RespectedMaster == p;
		}

		// Token: 0x06003D86 RID: 15750 RVA: 0x001526B4 File Offset: 0x001508B4
		public void GetAnimals(Pawn p, List<string> outAnimals)
		{
			outAnimals.Clear();
			List<DirectPawnRelation> directRelations = p.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				DirectPawnRelation directPawnRelation = directRelations[i];
				Pawn otherPawn = directPawnRelation.otherPawn;
				if (directPawnRelation.def == PawnRelationDefOf.Bond && !otherPawn.Dead && otherPawn.Spawned && otherPawn.Faction == Faction.OfPlayer && otherPawn.training.HasLearned(TrainableDefOf.Obedience) && this.AnimalMasterCheck(p, otherPawn))
				{
					outAnimals.Add(otherPawn.LabelShort);
				}
			}
		}

		// Token: 0x06003D87 RID: 15751 RVA: 0x00152743 File Offset: 0x00150943
		public int GetAnimalsCount(Pawn p)
		{
			ThoughtWorker_BondedAnimalMaster.tmpAnimals.Clear();
			this.GetAnimals(p, ThoughtWorker_BondedAnimalMaster.tmpAnimals);
			return ThoughtWorker_BondedAnimalMaster.tmpAnimals.Count;
		}

		// Token: 0x040020E0 RID: 8416
		private static List<string> tmpAnimals = new List<string>();
	}
}
