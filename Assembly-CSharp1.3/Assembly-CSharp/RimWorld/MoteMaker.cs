using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010B5 RID: 4277
	public static class MoteMaker
	{
		// Token: 0x06006627 RID: 26151 RVA: 0x00227DE5 File Offset: 0x00225FE5
		public static void MakeStaticMote(IntVec3 cell, Map map, ThingDef moteDef, float scale = 1f)
		{
			MoteMaker.MakeStaticMote(cell.ToVector3Shifted(), map, moteDef, scale);
		}

		// Token: 0x06006628 RID: 26152 RVA: 0x00227DF8 File Offset: 0x00225FF8
		public static Mote MakeStaticMote(Vector3 loc, Map map, ThingDef moteDef, float scale = 1f)
		{
			if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.Saturated)
			{
				return null;
			}
			Mote mote = (Mote)ThingMaker.MakeThing(moteDef, null);
			mote.exactPosition = loc;
			mote.Scale = scale;
			GenSpawn.Spawn(mote, loc.ToIntVec3(), map, WipeMode.Vanish);
			return mote;
		}

		// Token: 0x06006629 RID: 26153 RVA: 0x00227E46 File Offset: 0x00226046
		public static void ThrowText(Vector3 loc, Map map, string text, float timeBeforeStartFadeout = -1f)
		{
			MoteMaker.ThrowText(loc, map, text, Color.white, timeBeforeStartFadeout);
		}

		// Token: 0x0600662A RID: 26154 RVA: 0x00227E58 File Offset: 0x00226058
		public static void ThrowText(Vector3 loc, Map map, string text, Color color, float timeBeforeStartFadeout = -1f)
		{
			IntVec3 intVec = loc.ToIntVec3();
			if (!intVec.InBounds(map))
			{
				return;
			}
			MoteText moteText = (MoteText)ThingMaker.MakeThing(ThingDefOf.Mote_Text, null);
			moteText.exactPosition = loc;
			moteText.SetVelocity((float)Rand.Range(5, 35), Rand.Range(0.42f, 0.45f));
			moteText.text = text;
			moteText.textColor = color;
			if (timeBeforeStartFadeout >= 0f)
			{
				moteText.overrideTimeBeforeStartFadeout = timeBeforeStartFadeout;
			}
			GenSpawn.Spawn(moteText, intVec, map, WipeMode.Vanish);
		}

		// Token: 0x0600662B RID: 26155 RVA: 0x00227ED5 File Offset: 0x002260D5
		public static Mote MakeStunOverlay(Thing stunnedThing)
		{
			Mote mote = (Mote)ThingMaker.MakeThing(ThingDefOf.Mote_Stun, null);
			mote.Attach(stunnedThing);
			GenSpawn.Spawn(mote, stunnedThing.Position, stunnedThing.Map, WipeMode.Vanish);
			return mote;
		}

		// Token: 0x0600662C RID: 26156 RVA: 0x00227F08 File Offset: 0x00226108
		public static MoteDualAttached MakeInteractionOverlay(ThingDef moteDef, TargetInfo A, TargetInfo B)
		{
			MoteDualAttached moteDualAttached = (MoteDualAttached)ThingMaker.MakeThing(moteDef, null);
			moteDualAttached.Scale = 0.5f;
			moteDualAttached.Attach(A, B);
			GenSpawn.Spawn(moteDualAttached, A.Cell, A.Map ?? B.Map, WipeMode.Vanish);
			return moteDualAttached;
		}

		// Token: 0x0600662D RID: 26157 RVA: 0x00227F58 File Offset: 0x00226158
		public static Mote MakeAttachedOverlay(Thing thing, ThingDef moteDef, Vector3 offset, float scale = 1f, float solidTimeOverride = -1f)
		{
			Mote mote = (Mote)ThingMaker.MakeThing(moteDef, null);
			mote.Attach(thing, offset);
			mote.Scale = scale;
			mote.exactPosition = thing.DrawPos + offset;
			mote.solidTimeOverride = solidTimeOverride;
			GenSpawn.Spawn(mote, thing.Position, thing.MapHeld, WipeMode.Vanish);
			return mote;
		}

		// Token: 0x0600662E RID: 26158 RVA: 0x00227FB4 File Offset: 0x002261B4
		public static void MakeColonistActionOverlay(Pawn pawn, ThingDef moteDef)
		{
			MoteThrownAttached moteThrownAttached = (MoteThrownAttached)ThingMaker.MakeThing(moteDef, null);
			moteThrownAttached.Attach(pawn);
			moteThrownAttached.exactPosition = pawn.DrawPos;
			moteThrownAttached.Scale = 1.5f;
			moteThrownAttached.SetVelocity(Rand.Range(20f, 25f), 0.4f);
			GenSpawn.Spawn(moteThrownAttached, pawn.Position, pawn.Map, WipeMode.Vanish);
		}

		// Token: 0x0600662F RID: 26159 RVA: 0x00228020 File Offset: 0x00226220
		private static MoteBubble ExistingMoteBubbleOn(Pawn pawn)
		{
			if (!pawn.Spawned)
			{
				return null;
			}
			for (int i = 0; i < 4; i++)
			{
				if ((pawn.Position + MoteMaker.UpRightPattern[i]).InBounds(pawn.Map))
				{
					List<Thing> thingList = pawn.Position.GetThingList(pawn.Map);
					for (int j = 0; j < thingList.Count; j++)
					{
						MoteBubble moteBubble = thingList[j] as MoteBubble;
						if (moteBubble != null && moteBubble.link1.Linked && moteBubble.link1.Target.HasThing && moteBubble.link1.Target == pawn)
						{
							return moteBubble;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06006630 RID: 26160 RVA: 0x002280E0 File Offset: 0x002262E0
		public static MoteBubble MakeMoodThoughtBubble(Pawn pawn, Thought thought)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return null;
			}
			if (!pawn.Spawned)
			{
				return null;
			}
			float num = thought.MoodOffset();
			if (num == 0f)
			{
				return null;
			}
			MoteBubble moteBubble = MoteMaker.ExistingMoteBubbleOn(pawn);
			if (moteBubble != null)
			{
				if (moteBubble.def == ThingDefOf.Mote_Speech)
				{
					return null;
				}
				if (moteBubble.def == ThingDefOf.Mote_ThoughtBad || moteBubble.def == ThingDefOf.Mote_ThoughtGood)
				{
					moteBubble.Destroy(DestroyMode.Vanish);
				}
			}
			MoteBubble moteBubble2 = (MoteBubble)ThingMaker.MakeThing((num > 0f) ? ThingDefOf.Mote_ThoughtGood : ThingDefOf.Mote_ThoughtBad, null);
			moteBubble2.SetupMoteBubble(thought.Icon, null, null);
			moteBubble2.Attach(pawn);
			GenSpawn.Spawn(moteBubble2, pawn.Position, pawn.Map, WipeMode.Vanish);
			return moteBubble2;
		}

		// Token: 0x06006631 RID: 26161 RVA: 0x002281A4 File Offset: 0x002263A4
		public static MoteBubble MakeThoughtBubble(Pawn pawn, string iconPath, bool maintain = false)
		{
			MoteBubble moteBubble = MoteMaker.ExistingMoteBubbleOn(pawn);
			if (moteBubble != null)
			{
				moteBubble.Destroy(DestroyMode.Vanish);
			}
			MoteBubble moteBubble2 = (MoteBubble)ThingMaker.MakeThing(maintain ? ThingDefOf.Mote_ForceJobMaintained : ThingDefOf.Mote_ForceJob, null);
			moteBubble2.SetupMoteBubble(ContentFinder<Texture2D>.Get(iconPath, true), null, null);
			moteBubble2.Attach(pawn);
			GenSpawn.Spawn(moteBubble2, pawn.Position, pawn.Map, WipeMode.Vanish);
			return moteBubble2;
		}

		// Token: 0x06006632 RID: 26162 RVA: 0x00228214 File Offset: 0x00226414
		public static MoteBubble MakeInteractionBubble(Pawn initiator, Pawn recipient, ThingDef interactionMote, Texture2D symbol, Color? iconColor = null)
		{
			MoteBubble moteBubble = MoteMaker.ExistingMoteBubbleOn(initiator);
			if (moteBubble != null)
			{
				if (moteBubble.def == ThingDefOf.Mote_Speech)
				{
					moteBubble.Destroy(DestroyMode.Vanish);
				}
				if (moteBubble.def == ThingDefOf.Mote_ThoughtBad || moteBubble.def == ThingDefOf.Mote_ThoughtGood)
				{
					moteBubble.Destroy(DestroyMode.Vanish);
				}
			}
			MoteBubble moteBubble2 = (MoteBubble)ThingMaker.MakeThing(interactionMote, null);
			moteBubble2.SetupMoteBubble(symbol, recipient, iconColor);
			moteBubble2.Attach(initiator);
			GenSpawn.Spawn(moteBubble2, initiator.Position, initiator.Map, WipeMode.Vanish);
			return moteBubble2;
		}

		// Token: 0x06006633 RID: 26163 RVA: 0x00228298 File Offset: 0x00226498
		public static MoteBubble MakeSpeechBubble(Pawn initiator, Texture2D symbol)
		{
			MoteBubble moteBubble = MoteMaker.ExistingMoteBubbleOn(initiator);
			if (moteBubble != null)
			{
				if (moteBubble.def == ThingDefOf.Mote_Speech)
				{
					moteBubble.Destroy(DestroyMode.Vanish);
				}
				if (moteBubble.def == ThingDefOf.Mote_ThoughtBad || moteBubble.def == ThingDefOf.Mote_ThoughtGood)
				{
					moteBubble.Destroy(DestroyMode.Vanish);
				}
			}
			MoteBubble moteBubble2 = (MoteBubble)ThingMaker.MakeThing(ThingDefOf.Mote_Speech, null);
			moteBubble2.SetupMoteBubble(symbol, null, null);
			moteBubble2.Attach(initiator);
			GenSpawn.Spawn(moteBubble2, initiator.Position, initiator.Map, WipeMode.Vanish);
			return moteBubble2;
		}

		// Token: 0x06006634 RID: 26164 RVA: 0x00228328 File Offset: 0x00226528
		public static void ThrowExplosionCell(IntVec3 cell, Map map, ThingDef moteDef, Color color)
		{
			if (!cell.ShouldSpawnMotesAt(map))
			{
				return;
			}
			Mote mote = (Mote)ThingMaker.MakeThing(moteDef, null);
			mote.exactRotation = (float)(90 * Rand.RangeInclusive(0, 3));
			mote.exactPosition = cell.ToVector3Shifted();
			mote.instanceColor = color;
			GenSpawn.Spawn(mote, cell, map, WipeMode.Vanish);
			if (Rand.Value < 0.7f)
			{
				FleckMaker.ThrowDustPuff(cell, map, 1.2f);
			}
		}

		// Token: 0x06006635 RID: 26165 RVA: 0x00228394 File Offset: 0x00226594
		public static void ThrowExplosionInteriorMote(Vector3 loc, Map map, ThingDef moteDef)
		{
			if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
			{
				return;
			}
			MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(moteDef, null);
			moteThrown.Scale = Rand.Range(3f, 4.5f);
			moteThrown.rotationRate = Rand.Range(-30f, 30f);
			moteThrown.exactPosition = loc;
			moteThrown.SetVelocity((float)Rand.Range(0, 360), Rand.Range(0.48f, 0.72f));
			GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map, WipeMode.Vanish);
		}

		// Token: 0x06006636 RID: 26166 RVA: 0x00228424 File Offset: 0x00226624
		public static void MakeBombardmentMote(IntVec3 cell, Map map, float scale)
		{
			Mote mote = (Mote)ThingMaker.MakeThing(ThingDefOf.Mote_Bombardment, null);
			mote.exactPosition = cell.ToVector3Shifted();
			mote.Scale = 150f * scale;
			mote.rotationRate = 1.2f;
			GenSpawn.Spawn(mote, cell, map, WipeMode.Vanish);
		}

		// Token: 0x06006637 RID: 26167 RVA: 0x00228464 File Offset: 0x00226664
		public static void MakePowerBeamMote(IntVec3 cell, Map map)
		{
			Mote mote = (Mote)ThingMaker.MakeThing(ThingDefOf.Mote_PowerBeam, null);
			mote.exactPosition = cell.ToVector3Shifted();
			mote.Scale = 90f;
			mote.rotationRate = 1.2f;
			GenSpawn.Spawn(mote, cell, map, WipeMode.Vanish);
		}

		// Token: 0x06006638 RID: 26168 RVA: 0x002284A2 File Offset: 0x002266A2
		public static void PlaceTempRoof(IntVec3 cell, Map map)
		{
			if (!cell.ShouldSpawnMotesAt(map))
			{
				return;
			}
			Mote mote = (Mote)ThingMaker.MakeThing(ThingDefOf.Mote_TempRoof, null);
			mote.exactPosition = cell.ToVector3Shifted();
			GenSpawn.Spawn(mote, cell, map, WipeMode.Vanish);
		}

		// Token: 0x06006639 RID: 26169 RVA: 0x002284D4 File Offset: 0x002266D4
		public static Mote MakeConnectingLine(Vector3 start, Vector3 end, ThingDef moteType, Map map, float width = 1f)
		{
			Vector3 vector = end - start;
			float x = vector.MagnitudeHorizontal();
			Mote mote = MoteMaker.MakeStaticMote(start + vector * 0.5f, map, moteType, 1f);
			if (mote != null)
			{
				mote.exactScale = new Vector3(x, 1f, width);
				mote.exactRotation = Mathf.Atan2(-vector.z, vector.x) * 57.29578f;
			}
			return mote;
		}

		// Token: 0x040039B3 RID: 14771
		private static IntVec3[] UpRightPattern = new IntVec3[]
		{
			new IntVec3(0, 0, 0),
			new IntVec3(1, 0, 0),
			new IntVec3(0, 0, 1),
			new IntVec3(1, 0, 1)
		};
	}
}
