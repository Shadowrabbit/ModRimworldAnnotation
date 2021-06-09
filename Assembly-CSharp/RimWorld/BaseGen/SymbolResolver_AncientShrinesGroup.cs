using System;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E75 RID: 7797
	public class SymbolResolver_AncientShrinesGroup : SymbolResolver
	{
		// Token: 0x0600A7FC RID: 43004 RVA: 0x0030E6F8 File Offset: 0x0030C8F8
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			int num = (rp.rect.Width + Mathf.Max(-1, 0)) / (SymbolResolver_AncientShrinesGroup.StandardAncientShrineSize.x + -1);
			int num2 = (rp.rect.Height + Mathf.Max(-1, 0)) / (SymbolResolver_AncientShrinesGroup.StandardAncientShrineSize.z + -1);
			IntVec3 bottomLeft = rp.rect.BottomLeft;
			PodContentsType? podContentsType = rp.podContentsType;
			if (podContentsType == null)
			{
				float value = Rand.Value;
				if (value < 0.5f)
				{
					podContentsType = null;
				}
				else if (value < 0.7f)
				{
					podContentsType = new PodContentsType?(PodContentsType.Slave);
				}
				else
				{
					podContentsType = new PodContentsType?(PodContentsType.AncientHostile);
				}
			}
			int value2 = rp.ancientCryptosleepCasketGroupID ?? Find.UniqueIDsManager.GetNextAncientCryptosleepCasketGroupID();
			int num3 = 0;
			for (int i = 0; i < num2; i++)
			{
				for (int j = 0; j < num; j++)
				{
					if (!Rand.Chance(0.25f))
					{
						if (num3 >= 6)
						{
							break;
						}
						CellRect cellRect = new CellRect(bottomLeft.x + j * (SymbolResolver_AncientShrinesGroup.StandardAncientShrineSize.x + -1), bottomLeft.z + i * (SymbolResolver_AncientShrinesGroup.StandardAncientShrineSize.z + -1), SymbolResolver_AncientShrinesGroup.StandardAncientShrineSize.x, SymbolResolver_AncientShrinesGroup.StandardAncientShrineSize.z);
						if (cellRect.FullyContainedWithin(rp.rect))
						{
							IntVec3 center = new IntVec3(cellRect.minX + cellRect.Width / 2 - 1, 0, cellRect.minZ + cellRect.Height / 2);
							if (ThingUtility.InteractionCellWhenAt(ThingDefOf.AncientCryptosleepCasket, center, Rot4.East, map).Standable(map))
							{
								ResolveParams resolveParams = rp;
								resolveParams.rect = cellRect;
								resolveParams.ancientCryptosleepCasketGroupID = new int?(value2);
								resolveParams.podContentsType = podContentsType;
								BaseGen.symbolStack.Push("ancientShrine", resolveParams, null);
								num3++;
							}
						}
					}
				}
			}
		}

		// Token: 0x04007204 RID: 29188
		public static readonly IntVec2 StandardAncientShrineSize = new IntVec2(4, 3);

		// Token: 0x04007205 RID: 29189
		private const int MaxNumCaskets = 6;

		// Token: 0x04007206 RID: 29190
		private const float SkipShrineChance = 0.25f;

		// Token: 0x04007207 RID: 29191
		public const int MarginCells = -1;
	}
}
