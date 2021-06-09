using System;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E7A RID: 7802
	public class SymbolResolver_EdgeDefense : SymbolResolver
	{
		// Token: 0x0600A809 RID: 43017 RVA: 0x0030EDAC File Offset: 0x0030CFAC
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			Faction faction = rp.faction ?? Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			int num = rp.edgeDefenseGuardsCount ?? 0;
			int width;
			if (rp.edgeDefenseWidth != null)
			{
				width = rp.edgeDefenseWidth.Value;
			}
			else if (rp.edgeDefenseMortarsCount != null && rp.edgeDefenseMortarsCount.Value > 0)
			{
				width = 4;
			}
			else
			{
				width = (Rand.Bool ? 2 : 4);
			}
			width = Mathf.Clamp(width, 1, Mathf.Min(rp.rect.Width, rp.rect.Height) / 2);
			int num2;
			int num3;
			bool flag;
			bool flag2;
			bool flag3;
			switch (width)
			{
			case 1:
				num2 = (rp.edgeDefenseTurretsCount ?? 0);
				num3 = 0;
				flag = false;
				flag2 = true;
				flag3 = true;
				break;
			case 2:
				num2 = (rp.edgeDefenseTurretsCount ?? (rp.rect.EdgeCellsCount / 30));
				num3 = 0;
				flag = false;
				flag2 = false;
				flag3 = true;
				break;
			case 3:
				num2 = (rp.edgeDefenseTurretsCount ?? (rp.rect.EdgeCellsCount / 30));
				num3 = (rp.edgeDefenseMortarsCount ?? (rp.rect.EdgeCellsCount / 75));
				flag = (num3 == 0);
				flag2 = false;
				flag3 = true;
				break;
			default:
				num2 = (rp.edgeDefenseTurretsCount ?? (rp.rect.EdgeCellsCount / 30));
				num3 = (rp.edgeDefenseMortarsCount ?? (rp.rect.EdgeCellsCount / 75));
				flag = true;
				flag2 = false;
				flag3 = false;
				break;
			}
			if (faction != null && faction.def.techLevel < TechLevel.Industrial)
			{
				num2 = 0;
				num3 = 0;
			}
			if (num > 0)
			{
				Lord singlePawnLord = rp.singlePawnLord ?? LordMaker.MakeNewLord(faction, new LordJob_DefendBase(faction, rp.rect.CenterCell), map, null);
				Predicate<IntVec3> <>9__0;
				for (int i = 0; i < num; i++)
				{
					PawnGenerationRequest value = new PawnGenerationRequest(faction.RandomPawnKind(), faction, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, true, 1f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null);
					ResolveParams rp2 = rp;
					rp2.faction = faction;
					rp2.singlePawnLord = singlePawnLord;
					rp2.singlePawnGenerationRequest = new PawnGenerationRequest?(value);
					Predicate<IntVec3> singlePawnSpawnCellExtraPredicate;
					if ((singlePawnSpawnCellExtraPredicate = rp2.singlePawnSpawnCellExtraPredicate) == null && (singlePawnSpawnCellExtraPredicate = <>9__0) == null)
					{
						singlePawnSpawnCellExtraPredicate = (<>9__0 = delegate(IntVec3 x)
						{
							CellRect cellRect = rp.rect;
							for (int m = 0; m < width; m++)
							{
								if (cellRect.IsOnEdge(x))
								{
									return true;
								}
								cellRect = cellRect.ContractedBy(1);
							}
							return true;
						});
					}
					rp2.singlePawnSpawnCellExtraPredicate = singlePawnSpawnCellExtraPredicate;
					BaseGen.symbolStack.Push("pawn", rp2, null);
				}
			}
			CellRect rect = rp.rect;
			for (int j = 0; j < width; j++)
			{
				if (j % 2 == 0)
				{
					ResolveParams rp3 = rp;
					rp3.faction = faction;
					rp3.rect = rect;
					BaseGen.symbolStack.Push("edgeSandbags", rp3, null);
					if (!flag)
					{
						break;
					}
				}
				rect = rect.ContractedBy(1);
			}
			CellRect rect2 = flag3 ? rp.rect : rp.rect.ContractedBy(1);
			for (int k = 0; k < num3; k++)
			{
				ResolveParams rp4 = rp;
				rp4.faction = faction;
				rp4.rect = rect2;
				BaseGen.symbolStack.Push("edgeMannedMortar", rp4, null);
			}
			CellRect rect3 = flag2 ? rp.rect : rp.rect.ContractedBy(1);
			for (int l = 0; l < num2; l++)
			{
				ResolveParams rp5 = rp;
				rp5.faction = faction;
				rp5.singleThingDef = ThingDefOf.Turret_MiniTurret;
				rp5.rect = rect3;
				rp5.edgeThingAvoidOtherEdgeThings = new bool?(rp.edgeThingAvoidOtherEdgeThings ?? true);
				BaseGen.symbolStack.Push("edgeThing", rp5, null);
			}
		}

		// Token: 0x0400720C RID: 29196
		private const int DefaultCellsPerTurret = 30;

		// Token: 0x0400720D RID: 29197
		private const int DefaultCellsPerMortar = 75;
	}
}
