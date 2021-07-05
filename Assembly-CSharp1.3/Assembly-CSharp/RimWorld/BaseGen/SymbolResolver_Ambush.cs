using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015D0 RID: 5584
	public class SymbolResolver_Ambush : SymbolResolver
	{
		// Token: 0x06008361 RID: 33633 RVA: 0x002ECF88 File Offset: 0x002EB188
		public override bool CanResolve(ResolveParams rp)
		{
			return !rp.ambushSignalTag.NullOrEmpty();
		}

		// Token: 0x06008362 RID: 33634 RVA: 0x002ECF98 File Offset: 0x002EB198
		public override void Resolve(ResolveParams rp)
		{
			SignalAction_Ambush signalAction_Ambush = (SignalAction_Ambush)ThingMaker.MakeThing(ThingDefOf.SignalAction_Ambush, null);
			signalAction_Ambush.signalTag = rp.ambushSignalTag;
			signalAction_Ambush.points = (rp.ambushPoints ?? this.DefaultAmbushPoints);
			signalAction_Ambush.ambushType = (rp.ambushType ?? SignalActionAmbushType.Normal);
			signalAction_Ambush.spawnNear = (rp.spawnNear ?? IntVec3.Invalid);
			signalAction_Ambush.spawnAround = (rp.spawnAround ?? default(CellRect));
			signalAction_Ambush.spawnPawnsOnEdge = (rp.spawnPawnsOnEdge ?? false);
			GenSpawn.Spawn(signalAction_Ambush, rp.rect.CenterCell, BaseGen.globalSettings.map, WipeMode.Vanish);
		}

		// Token: 0x04005205 RID: 20997
		private float DefaultAmbushPoints = 200f;
	}
}
