using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000967 RID: 2407
	public class ThoughtWorker_VeneratedAnimalOnMapOrCaravan : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
	{
		// Token: 0x06003D3D RID: 15677 RVA: 0x001519B4 File Offset: 0x0014FBB4
		public override string PostProcessLabel(Pawn p, string label)
		{
			Pawn pawn = PawnUtility.FirstVeneratedAnimalOnMapOrCaravan(p);
			if (pawn == null)
			{
				return label;
			}
			return label.Formatted(pawn.kindDef.Named("ANIMAL"));
		}

		// Token: 0x06003D3E RID: 15678 RVA: 0x001519E8 File Offset: 0x0014FBE8
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (p.IsSlave)
			{
				return false;
			}
			int num = this.ThoughtStageFromAnimalDensity(PawnUtility.PlayerVeneratedAnimalBodySizePerCapitaOnMapOrCaravan(p));
			if (num > 0)
			{
				return ThoughtState.ActiveAtStage(num);
			}
			return false;
		}

		// Token: 0x06003D3F RID: 15679 RVA: 0x00151A22 File Offset: 0x0014FC22
		public IEnumerable<NamedArgument> GetDescriptionArgs()
		{
			yield return ("(" + "VeneratedAnimalsBodySizePerColonist".Translate() + ": " + 1f + ")").Named("STAGE1");
			yield return ("(" + "VeneratedAnimalsBodySizePerColonist".Translate() + ": " + 2f + ")").Named("STAGE2");
			yield return ("(" + "VeneratedAnimalsBodySizePerColonist".Translate() + ": " + 4f + ")").Named("STAGE3");
			yield return ("(" + "VeneratedAnimalsBodySizePerColonist".Translate() + ": " + 6f + ")").Named("STAGE4");
			yield break;
		}

		// Token: 0x06003D40 RID: 15680 RVA: 0x00151A2B File Offset: 0x0014FC2B
		private int ThoughtStageFromAnimalDensity(float density)
		{
			if (density <= 0f)
			{
				return -1;
			}
			if (density < 1f)
			{
				return 0;
			}
			if (density < 2f)
			{
				return 1;
			}
			if (density < 4f)
			{
				return 2;
			}
			if (density < 6f)
			{
				return 3;
			}
			return 4;
		}

		// Token: 0x040020D0 RID: 8400
		private const float FewAnimals = 1f;

		// Token: 0x040020D1 RID: 8401
		private const float SomeAnimals = 2f;

		// Token: 0x040020D2 RID: 8402
		private const float ManyAnimals = 4f;

		// Token: 0x040020D3 RID: 8403
		private const float LotsOfAnimals = 6f;
	}
}
