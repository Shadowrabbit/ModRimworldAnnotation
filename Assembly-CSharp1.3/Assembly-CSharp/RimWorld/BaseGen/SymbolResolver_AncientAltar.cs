using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015D1 RID: 5585
	public class SymbolResolver_AncientAltar : SymbolResolver
	{
		// Token: 0x17001604 RID: 5636
		// (get) Token: 0x06008364 RID: 33636 RVA: 0x002ED0A4 File Offset: 0x002EB2A4
		public static IntVec2 Size
		{
			get
			{
				return new IntVec2(SymbolResolver_AncientAltar.MainRoomSize.x + 22, SymbolResolver_AncientAltar.MainRoomSize.z + 22);
			}
		}

		// Token: 0x06008365 RID: 33637 RVA: 0x002ED0C8 File Offset: 0x002EB2C8
		public override void Resolve(ResolveParams rp)
		{
			rp.floorDef = (Rand.Bool ? TerrainDefOf.FlagstoneSandstone : TerrainDefOf.TileSandstone);
			rp.wallStuff = BaseGenUtility.RandomCheapWallStuff(rp.faction, true);
			CellRect cellRect = CellRect.CenteredOn(rp.rect.CenterCell, SymbolResolver_AncientAltar.MainRoomSize.x, SymbolResolver_AncientAltar.MainRoomSize.z);
			CellRect cellRect2 = new CellRect(cellRect.CenterCell.x - 5, cellRect.minZ + 2, 1, cellRect.Height - 4);
			CellRect cellRect3 = new CellRect(cellRect.CenterCell.x + 5, cellRect.minZ + 2, 1, cellRect.Height - 4);
			CellRect cellRect4 = cellRect.ExpandedBy(-1);
			CellRect rect = new CellRect(cellRect4.minX, cellRect4.minZ + 3, 1, cellRect4.Height - 6);
			CellRect rect2 = new CellRect(cellRect4.maxX, cellRect4.minZ + 3, 1, cellRect4.Height - 6);
			ResolveParams resolveParams = rp;
			resolveParams.rect = rect;
			resolveParams.singleThingStuff = rp.wallStuff;
			if (rp.exteriorThreatPoints != null && Faction.OfMechanoids != null)
			{
				ResolveParams resolveParams2 = rp;
				resolveParams2.rect = rp.rect.ExpandedBy(10);
				resolveParams2.sleepingMechanoidsWakeupSignalTag = rp.triggerSecuritySignal;
				resolveParams2.sendWokenUpMessage = new bool?(false);
				resolveParams2.threatPoints = new float?(rp.exteriorThreatPoints.Value);
				BaseGen.symbolStack.Push("sleepingMechanoids", resolveParams2, null);
			}
			int num = Rand.Range(2, 6);
			for (int i = 0; i < num; i++)
			{
				resolveParams.singleThingDef = (Rand.Bool ? ThingDefOf.Sarcophagus : ThingDefOf.Urn);
				resolveParams.thingRot = new Rot4?(Rot4.North);
				BaseGen.symbolStack.Push("edgeThing", resolveParams, null);
			}
			num = Rand.Range(1, 6);
			resolveParams.rect = rect2;
			for (int j = 0; j < num; j++)
			{
				resolveParams.singleThingDef = (Rand.Bool ? ThingDefOf.Sarcophagus : ThingDefOf.Urn);
				resolveParams.thingRot = new Rot4?(Rot4.North);
				BaseGen.symbolStack.Push("edgeThing", resolveParams, null);
			}
			CellRect cellRect5 = new CellRect(cellRect.minX - 11 + 1, cellRect.minZ + 5, 11, 11);
			CellRect cellRect6 = new CellRect(cellRect.maxX, cellRect.minZ + 5, 11, 11);
			CellRect cellRect7 = new CellRect(cellRect.minX + cellRect.Width / 2 - 5, cellRect.maxZ, 11, 11);
			CellRect[] array = new CellRect[]
			{
				cellRect5,
				cellRect6,
				cellRect7
			};
			ResolveParams resolveParams3 = rp;
			resolveParams3.rect = cellRect.ContractedBy(3);
			resolveParams3.unfoggedSignalTag = "UnfoggedSignal" + Find.UniqueIDsManager.GetNextSignalTagID();
			BaseGen.symbolStack.Push("unfoggedTrigger", resolveParams3, null);
			ResolveParams resolveParams4 = rp;
			resolveParams4.sound = SoundDefOf.AncientRelicRoomReveal;
			resolveParams4.soundOneShotActionSignalTag = resolveParams3.unfoggedSignalTag;
			BaseGen.symbolStack.Push("soundOneShotAction", resolveParams4, null);
			ResolveParams resolveParams5 = rp;
			resolveParams5.singleThingDef = ThingDefOf.SteleLarge;
			IntVec2 size = ThingDefOf.SteleLarge.size;
			resolveParams5.rect = new CellRect(cellRect2.minX + 1, cellRect.maxZ - size.x, size.x, size.z);
			BaseGen.symbolStack.Push("thing", resolveParams5, null);
			resolveParams5.rect = new CellRect(cellRect3.maxX - size.x, cellRect.maxZ - size.x, size.x, size.z);
			BaseGen.symbolStack.Push("thing", resolveParams5, null);
			ResolveParams resolveParams6 = rp;
			resolveParams6.singleThingDef = ThingDefOf.Column;
			resolveParams6.singleThingStuff = rp.wallStuff;
			resolveParams6.fillWithThingsPadding = new int?(2);
			resolveParams6.rect = cellRect2;
			BaseGen.symbolStack.Push("fillWithThings", resolveParams6, null);
			resolveParams6.rect = cellRect3;
			BaseGen.symbolStack.Push("fillWithThings", resolveParams6, null);
			Thing item = rp.relicThing ?? ThingMaker.MakeThing(ThingDefOf.Beer, null);
			Thing thing = ThingMaker.MakeThing(ThingDefOf.Reliquary, BaseGenUtility.CheapStuffFor(ThingDefOf.Reliquary, rp.faction));
			thing.TryGetComp<CompThingContainer>().innerContainer.TryAdd(item, true);
			ResolveParams resolveParams7 = rp;
			resolveParams7.sound = SoundDefOf.AncientRelicTakenAlarm;
			resolveParams7.soundOneShotActionSignalTag = rp.triggerSecuritySignal;
			BaseGen.symbolStack.Push("soundOneShotAction", resolveParams7, null);
			CellRect rect3 = CellRect.CenteredOn(cellRect.CenterCell, thing.def.Size.x, thing.def.Size.z);
			ResolveParams resolveParams8 = rp;
			resolveParams8.rect = rect3;
			resolveParams8.triggerContainerEmptiedSignalTag = rp.triggerSecuritySignal;
			resolveParams8.triggerContainerEmptiedThing = thing;
			BaseGen.symbolStack.Push("containerEmptiedTrigger", resolveParams8, null);
			ResolveParams resolveParams9 = rp;
			resolveParams9.rect = rect3;
			resolveParams9.thingRot = new Rot4?(Rot4.South);
			resolveParams9.singleThingToSpawn = thing;
			BaseGen.symbolStack.Push("thing", resolveParams9, null);
			CellRect rect4 = resolveParams9.rect.ExpandedBy(2);
			foreach (IntVec3 center in rect4.Corners)
			{
				ResolveParams resolveParams10 = rp;
				resolveParams10.faction = Faction.OfAncients;
				resolveParams10.singleThingDef = ThingDefOf.AncientLamp;
				resolveParams10.rect = CellRect.CenteredOn(center, 1, 1);
				BaseGen.symbolStack.Push("thing", resolveParams10, null);
			}
			ResolveParams resolveParams11 = rp;
			resolveParams11.rect = rect4;
			resolveParams11.floorDef = TerrainDefOf.PavedTile;
			BaseGen.symbolStack.Push("floor", resolveParams11, null);
			ResolveParams resolveParams12 = rp;
			resolveParams12.floorDef = TerrainDefOf.Gravel;
			BaseGen.symbolStack.Push("outdoorsPath", resolveParams12, null);
			ResolveParams resolveParams13 = rp;
			CellRect cellRect8 = new CellRect(cellRect.minX + cellRect.Width / 2, cellRect.minZ, 1, 1);
			resolveParams13.rect = cellRect8;
			resolveParams13.singleThingDef = ThingDefOf.Door;
			resolveParams13.singleThingStuff = rp.wallStuff;
			BaseGen.symbolStack.Push("thing", resolveParams13, null);
			ResolveParams resolveParams14 = rp;
			resolveParams14.rect = cellRect8;
			resolveParams14.singleThingDef = ThingDefOf.AncientLamp;
			resolveParams14.rect = new CellRect(cellRect8.minX - 1, cellRect8.minZ - 1, 1, 1);
			BaseGen.symbolStack.Push("thing", resolveParams14, null);
			BaseGen.symbolStack.Push("clear", resolveParams14, null);
			resolveParams14.rect = new CellRect(cellRect8.maxX + 1, cellRect8.minZ - 1, 1, 1);
			BaseGen.symbolStack.Push("thing", resolveParams14, null);
			BaseGen.symbolStack.Push("clear", resolveParams14, null);
			ResolveParams resolveParams15 = resolveParams13;
			resolveParams15.rect = new CellRect(cellRect.minX - 1, cellRect5.minZ + 3, 2, cellRect5.Height - 6);
			BaseGen.symbolStack.Push("floor", resolveParams15, null);
			BaseGen.symbolStack.Push("clear", resolveParams15, null);
			ResolveParams resolveParams16 = resolveParams13;
			resolveParams16.rect = new CellRect(cellRect.maxX, cellRect6.minZ + 3, 2, cellRect6.Height - 6);
			BaseGen.symbolStack.Push("floor", resolveParams16, null);
			BaseGen.symbolStack.Push("clear", resolveParams16, null);
			ResolveParams resolveParams17 = resolveParams13;
			resolveParams17.rect = new CellRect(cellRect7.minX + 3, cellRect.maxZ, cellRect7.Width - 6, 2);
			BaseGen.symbolStack.Push("floor", resolveParams17, null);
			BaseGen.symbolStack.Push("clear", resolveParams17, null);
			resolveParams13.rect = cellRect;
			resolveParams13.cornerRadius = new int?(3);
			BaseGen.symbolStack.Push("emptyRoomRounded", resolveParams13, null);
			resolveParams13.rect = new CellRect(cellRect8.minX, cellRect8.minZ - 3, 1, 3);
			BaseGen.symbolStack.Push("clear", resolveParams13, null);
			if (rp.interiorThreatPoints != null)
			{
				ResolveParams resolveParams18 = rp;
				resolveParams18.threatPoints = new float?(rp.interiorThreatPoints.Value / (float)array.Length);
				foreach (CellRect cellRect9 in array)
				{
					resolveParams18.sleepingMechanoidsWakeupSignalTag = rp.triggerSecuritySignal;
					resolveParams18.ancientCryptosleepCasketOpenSignalTag = rp.triggerSecuritySignal;
					resolveParams18.rect = cellRect9.ContractedBy(3);
					resolveParams18.sendWokenUpMessage = new bool?(false);
					BaseGen.symbolStack.Push("ancientComplex_interior_sleepingMechanoids", resolveParams18, null);
				}
			}
			ResolveParams resolveParams19 = rp;
			resolveParams19.cornerRadius = new int?(3);
			foreach (CellRect rect5 in array)
			{
				resolveParams19.rect = rect5;
				BaseGen.symbolStack.Push("emptyRoomRounded", resolveParams19, null);
			}
		}

		// Token: 0x04005206 RID: 20998
		private const int AncillaryRoomSize = 11;

		// Token: 0x04005207 RID: 20999
		private const int AncillaryRoomCornerRadius = 3;

		// Token: 0x04005208 RID: 21000
		private const int EntrancePathLength = 3;

		// Token: 0x04005209 RID: 21001
		private static readonly IntVec2 MainRoomSize = new IntVec2(17, 21);

		// Token: 0x0400520A RID: 21002
		private const string UnfoggedSignalPrefix = "UnfoggedSignal";
	}
}
