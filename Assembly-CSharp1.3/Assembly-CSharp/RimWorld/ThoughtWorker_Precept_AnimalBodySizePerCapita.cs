using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200095D RID: 2397
	public class ThoughtWorker_Precept_AnimalBodySizePerCapita : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
	{
		// Token: 0x06003D1A RID: 15642 RVA: 0x001512A0 File Offset: 0x0014F4A0
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (ThoughtUtility.ThoughtNullified(p, this.def))
			{
				return false;
			}
			if (p.IsSlave)
			{
				return false;
			}
			float num = PawnUtility.PlayerAnimalBodySizePerCapita();
			if (num <= 2f && GenTicks.TicksAbs < 900000)
			{
				return false;
			}
			if (num < 4f && this.def.minExpectationForNegativeThought != null && p.MapHeld != null && ExpectationsUtility.CurrentExpectationFor(p.MapHeld).order < this.def.minExpectationForNegativeThought.order)
			{
				return false;
			}
			if (this.ThoughtStageFromAnimalDensity(num) < 0)
			{
				return false;
			}
			return ThoughtState.ActiveAtStage(this.ThoughtStageFromAnimalDensity(PawnUtility.PlayerAnimalBodySizePerCapita()));
		}

		// Token: 0x06003D1B RID: 15643 RVA: 0x0015135C File Offset: 0x0014F55C
		public override string PostProcessDescription(Pawn p, string description)
		{
			return base.PostProcessDescription(p, description) + "\n\n" + "CurrentTotalAnimalBodySizePerColonist".Translate() + ": " + PawnUtility.PlayerAnimalBodySizePerCapita().ToString("F1") + "\n" + "MinAnimalBodySizePerColonist".Translate(4f.ToString("F1"));
		}

		// Token: 0x06003D1C RID: 15644 RVA: 0x001513E0 File Offset: 0x0014F5E0
		private int ThoughtStageFromAnimalDensity(float density)
		{
			if (density < 0f)
			{
				return 0;
			}
			if (density < 1f)
			{
				return 1;
			}
			if (density < 2f)
			{
				return 2;
			}
			if (density < 4f)
			{
				return -1;
			}
			if (density < 6f)
			{
				return 3;
			}
			if (density < 8f)
			{
				return 4;
			}
			return 5;
		}

		// Token: 0x06003D1D RID: 15645 RVA: 0x0015141F File Offset: 0x0014F61F
		public IEnumerable<NamedArgument> GetDescriptionArgs()
		{
			yield return ("(" + "AnimalsBodySizePerColonist".Translate() + ": " + 1f + ")").Named("STAGE1");
			yield return ("(" + "AnimalsBodySizePerColonist".Translate() + ": " + 2f + ")").Named("STAGE2");
			yield return ("(" + "AnimalsBodySizePerColonist".Translate() + ": " + 6f + ")").Named("STAGE4");
			yield return ("(" + "AnimalsBodySizePerColonist".Translate() + ": " + 8f + ")").Named("STAGE5");
			yield break;
		}

		// Token: 0x040020C4 RID: 8388
		private const float NoAnimals = 0f;

		// Token: 0x040020C5 RID: 8389
		private const float ScarceAnimals = 1f;

		// Token: 0x040020C6 RID: 8390
		private const float FewAnimals = 2f;

		// Token: 0x040020C7 RID: 8391
		private const float NoThought = 4f;

		// Token: 0x040020C8 RID: 8392
		private const float SomeAnimals = 6f;

		// Token: 0x040020C9 RID: 8393
		private const float LotsOfAnimals = 8f;

		// Token: 0x040020CA RID: 8394
		private const int MinimumTicksBeforeFewAnimals = 900000;
	}
}
