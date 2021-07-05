using System;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E38 RID: 7736
	public class SymbolResolver_BasePart_Outdoors_Division_Split : SymbolResolver
	{
		// Token: 0x0600A73A RID: 42810 RVA: 0x0030A428 File Offset: 0x00308628
		public override bool CanResolve(ResolveParams rp)
		{
			int num;
			int num2;
			return base.CanResolve(rp) && (this.TryFindSplitPoint(false, rp.rect, out num, out num2) || this.TryFindSplitPoint(true, rp.rect, out num2, out num));
		}

		// Token: 0x0600A73B RID: 42811 RVA: 0x0030A46C File Offset: 0x0030866C
		public override void Resolve(ResolveParams rp)
		{
			bool @bool = Rand.Bool;
			int num;
			int num2;
			bool flag;
			if (this.TryFindSplitPoint(@bool, rp.rect, out num, out num2))
			{
				flag = @bool;
			}
			else
			{
				if (!this.TryFindSplitPoint(!@bool, rp.rect, out num, out num2))
				{
					Log.Warning("Could not find split point.", false);
					return;
				}
				flag = !@bool;
			}
			TerrainDef floorDef = rp.pathwayFloorDef ?? BaseGenUtility.RandomBasicFloorDef(rp.faction, false);
			ResolveParams resolveParams3;
			ResolveParams resolveParams5;
			if (flag)
			{
				ResolveParams resolveParams = rp;
				resolveParams.rect = new CellRect(rp.rect.minX, rp.rect.minZ + num, rp.rect.Width, num2);
				resolveParams.floorDef = floorDef;
				resolveParams.streetHorizontal = new bool?(true);
				BaseGen.symbolStack.Push("street", resolveParams, null);
				ResolveParams resolveParams2 = rp;
				resolveParams2.rect = new CellRect(rp.rect.minX, rp.rect.minZ, rp.rect.Width, num);
				resolveParams3 = resolveParams2;
				ResolveParams resolveParams4 = rp;
				resolveParams4.rect = new CellRect(rp.rect.minX, rp.rect.minZ + num + num2, rp.rect.Width, rp.rect.Height - num - num2);
				resolveParams5 = resolveParams4;
			}
			else
			{
				ResolveParams resolveParams6 = rp;
				resolveParams6.rect = new CellRect(rp.rect.minX + num, rp.rect.minZ, num2, rp.rect.Height);
				resolveParams6.floorDef = floorDef;
				resolveParams6.streetHorizontal = new bool?(false);
				BaseGen.symbolStack.Push("street", resolveParams6, null);
				ResolveParams resolveParams7 = rp;
				resolveParams7.rect = new CellRect(rp.rect.minX, rp.rect.minZ, num, rp.rect.Height);
				resolveParams3 = resolveParams7;
				ResolveParams resolveParams8 = rp;
				resolveParams8.rect = new CellRect(rp.rect.minX + num + num2, rp.rect.minZ, rp.rect.Width - num - num2, rp.rect.Height);
				resolveParams5 = resolveParams8;
			}
			if (Rand.Bool)
			{
				BaseGen.symbolStack.Push("basePart_outdoors", resolveParams3, null);
				BaseGen.symbolStack.Push("basePart_outdoors", resolveParams5, null);
				return;
			}
			BaseGen.symbolStack.Push("basePart_outdoors", resolveParams5, null);
			BaseGen.symbolStack.Push("basePart_outdoors", resolveParams3, null);
		}

		// Token: 0x0600A73C RID: 42812 RVA: 0x0030A6E8 File Offset: 0x003088E8
		private bool TryFindSplitPoint(bool horizontal, CellRect rect, out int splitPoint, out int spaceBetween)
		{
			int num = horizontal ? rect.Height : rect.Width;
			spaceBetween = SymbolResolver_BasePart_Outdoors_Division_Split.SpaceBetweenRange.RandomInRange;
			spaceBetween = Mathf.Min(spaceBetween, num - 10);
			if (spaceBetween < SymbolResolver_BasePart_Outdoors_Division_Split.SpaceBetweenRange.min)
			{
				splitPoint = -1;
				return false;
			}
			splitPoint = Rand.RangeInclusive(5, num - 5 - spaceBetween);
			return true;
		}

		// Token: 0x040071AF RID: 29103
		private const int MinLengthAfterSplit = 5;

		// Token: 0x040071B0 RID: 29104
		private static readonly IntRange SpaceBetweenRange = new IntRange(1, 2);
	}
}
