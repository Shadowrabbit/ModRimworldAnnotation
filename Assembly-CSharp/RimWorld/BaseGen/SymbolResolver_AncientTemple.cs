using System;
using System.Linq;
using RimWorld.SketchGen;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E76 RID: 7798
	public class SymbolResolver_AncientTemple : SymbolResolver
	{
		// Token: 0x0600A7FF RID: 43007 RVA: 0x0030E8F0 File Offset: 0x0030CAF0
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			CellRect cellRect = CellRect.Empty;
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.sketch = new Sketch();
			resolveParams.monumentOpen = new bool?(false);
			resolveParams.monumentSize = new IntVec2?(new IntVec2(rp.rect.Width, rp.rect.Height));
			resolveParams.allowMonumentDoors = new bool?(false);
			resolveParams.allowWood = new bool?(false);
			resolveParams.allowFlammableWalls = new bool?(false);
			if (rp.allowedMonumentThings != null)
			{
				resolveParams.allowedMonumentThings = rp.allowedMonumentThings;
			}
			else
			{
				resolveParams.allowedMonumentThings = new ThingFilter();
				resolveParams.allowedMonumentThings.SetAllowAll(null, true);
			}
			resolveParams.allowedMonumentThings.SetAllow(ThingDefOf.Drape, false);
			Sketch sketch = SketchGen.Generate(SketchResolverDefOf.Monument, resolveParams);
			sketch.Spawn(map, rp.rect.CenterCell, null, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, true, true, null, false, true, null, null);
			CellRect rect = SketchGenUtility.FindBiggestRect(sketch, delegate(IntVec3 x)
			{
				if (sketch.TerrainAt(x) != null)
				{
					return !sketch.ThingsAt(x).Any((SketchThing y) => y.def == ThingDefOf.Wall);
				}
				return false;
			}).MovedBy(rp.rect.CenterCell);
			for (int i = 0; i < sketch.Things.Count; i++)
			{
				if (sketch.Things[i].def == ThingDefOf.Wall)
				{
					IntVec3 intVec = sketch.Things[i].pos + rp.rect.CenterCell;
					if (cellRect.IsEmpty)
					{
						cellRect = CellRect.SingleCell(intVec);
					}
					else
					{
						cellRect = CellRect.FromLimits(Mathf.Min(cellRect.minX, intVec.x), Mathf.Min(cellRect.minZ, intVec.z), Mathf.Max(cellRect.maxX, intVec.x), Mathf.Max(cellRect.maxZ, intVec.z));
					}
				}
			}
			if (!rect.IsEmpty)
			{
				ResolveParams resolveParams2 = rp;
				resolveParams2.rect = rect;
				if (rp.allowedMonumentThings != null)
				{
					resolveParams2.allowedMonumentThings = rp.allowedMonumentThings;
				}
				else
				{
					resolveParams2.allowedMonumentThings = new ThingFilter();
					resolveParams2.allowedMonumentThings.SetAllowAll(null, true);
				}
				if (ModsConfig.RoyaltyActive)
				{
					resolveParams2.allowedMonumentThings.SetAllow(ThingDefOf.Drape, false);
				}
				BaseGen.symbolStack.Push("interior_ancientTemple", resolveParams2, null);
			}
			if (rp.makeWarningLetter != null && rp.makeWarningLetter.Value)
			{
				int nextSignalTagID = Find.UniqueIDsManager.GetNextSignalTagID();
				string signalTag = "ancientTempleApproached-" + nextSignalTagID;
				SignalAction_Letter signalAction_Letter = (SignalAction_Letter)ThingMaker.MakeThing(ThingDefOf.SignalAction_Letter, null);
				signalAction_Letter.signalTag = signalTag;
				signalAction_Letter.letter = LetterMaker.MakeLetter("LetterLabelAncientShrineWarning".Translate(), "AncientShrineWarning".Translate(), LetterDefOf.ThreatBig, new TargetInfo(cellRect.CenterCell, map, false), null, null, null);
				GenSpawn.Spawn(signalAction_Letter, cellRect.CenterCell, map, WipeMode.Vanish);
				RectTrigger rectTrigger = (RectTrigger)ThingMaker.MakeThing(ThingDefOf.RectTrigger, null);
				rectTrigger.signalTag = signalTag;
				rectTrigger.Rect = cellRect.ExpandedBy(1).ClipInsideMap(map);
				rectTrigger.destroyIfUnfogged = true;
				GenSpawn.Spawn(rectTrigger, cellRect.CenterCell, map, WipeMode.Vanish);
			}
		}
	}
}
