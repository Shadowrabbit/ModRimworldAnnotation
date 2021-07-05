using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015DB RID: 5595
	public class SymbolResolver_EdgeMannedMortar : SymbolResolver
	{
		// Token: 0x06008382 RID: 33666 RVA: 0x002EF800 File Offset: 0x002EDA00
		public override bool CanResolve(ResolveParams rp)
		{
			CellRect cellRect;
			return base.CanResolve(rp) && this.TryFindRandomInnerRectTouchingEdge(rp.rect, out cellRect);
		}

		// Token: 0x06008383 RID: 33667 RVA: 0x002EF828 File Offset: 0x002EDA28
		public override void Resolve(ResolveParams rp)
		{
			CellRect rect;
			if (!this.TryFindRandomInnerRectTouchingEdge(rp.rect, out rect))
			{
				return;
			}
			Rot4 value;
			if (rect.Cells.Any((IntVec3 x) => x.x == rp.rect.minX))
			{
				value = Rot4.West;
			}
			else if (rect.Cells.Any((IntVec3 x) => x.x == rp.rect.maxX))
			{
				value = Rot4.East;
			}
			else if (rect.Cells.Any((IntVec3 x) => x.z == rp.rect.minZ))
			{
				value = Rot4.South;
			}
			else
			{
				value = Rot4.North;
			}
			ResolveParams rp2 = rp;
			rp2.rect = rect;
			rp2.thingRot = new Rot4?(value);
			BaseGen.symbolStack.Push("mannedMortar", rp2, null);
		}

		// Token: 0x06008384 RID: 33668 RVA: 0x002EF8F4 File Offset: 0x002EDAF4
		private bool TryFindRandomInnerRectTouchingEdge(CellRect rect, out CellRect mortarRect)
		{
			Map map = BaseGen.globalSettings.map;
			IntVec2 size = new IntVec2(3, 3);
			Func<IntVec3, bool> <>9__2;
			Func<IntVec3, bool> <>9__3;
			return rect.TryFindRandomInnerRectTouchingEdge(size, out mortarRect, delegate(CellRect x)
			{
				IEnumerable<IntVec3> cells = x.Cells;
				Func<IntVec3, bool> predicate;
				if ((predicate = <>9__2) == null)
				{
					predicate = (<>9__2 = ((IntVec3 y) => y.Standable(map) && y.GetEdifice(map) == null));
				}
				return cells.All(predicate) && GenConstruct.TerrainCanSupport(x, map, ThingDefOf.Turret_Mortar);
			}) || rect.TryFindRandomInnerRectTouchingEdge(size, out mortarRect, delegate(CellRect x)
			{
				IEnumerable<IntVec3> cells = x.Cells;
				Func<IntVec3, bool> predicate;
				if ((predicate = <>9__3) == null)
				{
					predicate = (<>9__3 = ((IntVec3 y) => y.Standable(map) && y.GetEdifice(map) == null));
				}
				return cells.All(predicate);
			});
		}
	}
}
