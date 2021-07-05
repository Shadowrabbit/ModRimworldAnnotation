using System;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen
{
	// Token: 0x020015EA RID: 5610
	public class SymbolResolver_RandomMechanoidGroup : SymbolResolver
	{
		// Token: 0x060083B1 RID: 33713 RVA: 0x002F0F6C File Offset: 0x002EF16C
		public override void Resolve(ResolveParams rp)
		{
			int num = rp.mechanoidsCount ?? SymbolResolver_RandomMechanoidGroup.DefaultMechanoidCountRange.RandomInRange;
			Lord lord = rp.singlePawnLord;
			if (lord == null && num > 0)
			{
				Map map = BaseGen.globalSettings.map;
				IntVec3 point;
				LordJob lordJob;
				if (Rand.Bool && (from x in rp.rect.Cells
				where !x.Impassable(map)
				select x).TryRandomElement(out point))
				{
					lordJob = new LordJob_DefendPoint(point, null, false, true);
				}
				else
				{
					lordJob = new LordJob_AssaultColony(Faction.OfMechanoids, false, false, false, false, false, false, false);
				}
				lord = LordMaker.MakeNewLord(Faction.OfMechanoids, lordJob, map, null);
			}
			for (int i = 0; i < num; i++)
			{
				PawnKindDef pawnKindDef = rp.singlePawnKindDef;
				if (pawnKindDef == null)
				{
					pawnKindDef = DefDatabase<PawnKindDef>.AllDefsListForReading.Where(new Func<PawnKindDef, bool>(MechClusterGenerator.MechKindSuitableForCluster)).RandomElementByWeight((PawnKindDef kind) => 1f / kind.combatPower);
				}
				ResolveParams resolveParams = rp;
				resolveParams.singlePawnKindDef = pawnKindDef;
				resolveParams.singlePawnLord = lord;
				resolveParams.faction = Faction.OfMechanoids;
				BaseGen.symbolStack.Push("pawn", resolveParams, null);
			}
		}

		// Token: 0x04005230 RID: 21040
		private static readonly IntRange DefaultMechanoidCountRange = new IntRange(1, 5);
	}
}
