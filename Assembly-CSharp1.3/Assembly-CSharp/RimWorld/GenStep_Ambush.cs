using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CAF RID: 3247
	public abstract class GenStep_Ambush : GenStep
	{
		// Token: 0x06004BB3 RID: 19379 RVA: 0x00193774 File Offset: 0x00191974
		public override void Generate(Map map, GenStepParams parms)
		{
			CellRect rectToDefend;
			IntVec3 root;
			if (!SiteGenStepUtility.TryFindRootToSpawnAroundRectOfInterest(out rectToDefend, out root, map))
			{
				return;
			}
			this.SpawnTrigger(rectToDefend, root, map, parms);
		}

		// Token: 0x06004BB4 RID: 19380 RVA: 0x00193798 File Offset: 0x00191998
		private void SpawnTrigger(CellRect rectToDefend, IntVec3 root, Map map, GenStepParams parms)
		{
			int nextSignalTagID = Find.UniqueIDsManager.GetNextSignalTagID();
			string signalTag = "ambushActivated-" + nextSignalTagID;
			CellRect rect;
			if (root.IsValid)
			{
				rect = CellRect.CenteredOn(root, 17);
			}
			else
			{
				rect = rectToDefend.ExpandedBy(12);
			}
			SignalAction_Ambush signalAction_Ambush = this.MakeAmbushSignalAction(rectToDefend, root, parms);
			signalAction_Ambush.signalTag = signalTag;
			GenSpawn.Spawn(signalAction_Ambush, rect.CenterCell, map, WipeMode.Vanish);
			RectTrigger rectTrigger = this.MakeRectTrigger();
			rectTrigger.signalTag = signalTag;
			rectTrigger.Rect = rect;
			GenSpawn.Spawn(rectTrigger, rect.CenterCell, map, WipeMode.Vanish);
			TriggerUnfogged triggerUnfogged = (TriggerUnfogged)ThingMaker.MakeThing(ThingDefOf.TriggerUnfogged, null);
			triggerUnfogged.signalTag = signalTag;
			GenSpawn.Spawn(triggerUnfogged, rect.CenterCell, map, WipeMode.Vanish);
		}

		// Token: 0x06004BB5 RID: 19381 RVA: 0x00193848 File Offset: 0x00191A48
		protected virtual RectTrigger MakeRectTrigger()
		{
			return (RectTrigger)ThingMaker.MakeThing(ThingDefOf.RectTrigger, null);
		}

		// Token: 0x06004BB6 RID: 19382 RVA: 0x0019385C File Offset: 0x00191A5C
		protected virtual SignalAction_Ambush MakeAmbushSignalAction(CellRect rectToDefend, IntVec3 root, GenStepParams parms)
		{
			SignalAction_Ambush signalAction_Ambush = (SignalAction_Ambush)ThingMaker.MakeThing(ThingDefOf.SignalAction_Ambush, null);
			if (parms.sitePart != null)
			{
				signalAction_Ambush.points = parms.sitePart.parms.threatPoints;
			}
			else
			{
				signalAction_Ambush.points = this.defaultPointsRange.RandomInRange;
			}
			int num = Rand.RangeInclusive(0, 2);
			if (num == 0)
			{
				signalAction_Ambush.ambushType = SignalActionAmbushType.Manhunters;
			}
			else if (num == 1 && PawnGroupMakerUtility.CanGenerateAnyNormalGroup(Faction.OfMechanoids, signalAction_Ambush.points))
			{
				signalAction_Ambush.ambushType = SignalActionAmbushType.Mechanoids;
			}
			else
			{
				signalAction_Ambush.ambushType = SignalActionAmbushType.Normal;
			}
			return signalAction_Ambush;
		}

		// Token: 0x04002DDE RID: 11742
		public FloatRange defaultPointsRange = new FloatRange(180f, 340f);
	}
}
