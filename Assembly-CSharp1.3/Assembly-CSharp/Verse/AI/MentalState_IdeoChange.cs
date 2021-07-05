using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse.AI
{
	// Token: 0x020005DF RID: 1503
	public class MentalState_IdeoChange : MentalState
	{
		// Token: 0x06002B7D RID: 11133 RVA: 0x00103924 File Offset: 0x00101B24
		public override void PreStart()
		{
			base.PreStart();
			this.oldIdeo = this.pawn.Ideo;
			this.oldRole = this.oldIdeo.GetRole(this.pawn);
			this.newIdeo = Find.IdeoManager.IdeosListForReading.RandomElementByWeight((Ideo x) => this.GetIdeoWeight(x));
			if (this.pawn.ideo.IdeoConversionAttempt(0.5f, this.newIdeo))
			{
				this.changedIdeo = true;
			}
			this.newCertainty = this.pawn.ideo.Certainty;
		}

		// Token: 0x06002B7E RID: 11134 RVA: 0x001039BC File Offset: 0x00101BBC
		public override TaggedString GetBeginLetterText()
		{
			TaggedString taggedString = this.def.beginLetter.Formatted(this.pawn.LabelShort, this.pawn.Named("PAWN"), this.pawn.Ideo.Named("NEWIDEO"), this.oldIdeo.Named("OLDIDEO")).CapitalizeFirst() + "\n\n";
			if (this.changedIdeo)
			{
				taggedString += "LetterIdeoChangeConverted".Translate(this.pawn.Named("PAWN"), this.newIdeo.Named("NEWIDEO"), this.oldIdeo.Named("OLDIDEO")).CapitalizeFirst();
			}
			else
			{
				taggedString += "LetterIdeoChangeNotConverted".Translate(this.pawn.Named("PAWN"), this.oldIdeo.Named("OLDIDEO"), this.newCertainty.ToStringPercent().Named("NEWCERTAINTY")).CapitalizeFirst();
			}
			MentalStateDef wanderToOwnRoomStateOrFallback = MentalStateUtility.GetWanderToOwnRoomStateOrFallback(this.pawn);
			if (wanderToOwnRoomStateOrFallback == MentalStateDefOf.Wander_OwnRoom)
			{
				taggedString += "\n\n" + "LetterIdeoChangedWanderOwnRoom".Translate(this.pawn.Named("PAWN"));
			}
			else if (wanderToOwnRoomStateOrFallback == MentalStateDefOf.Wander_Sad)
			{
				taggedString += "\n\n" + "LetterIdeoChangedSadWander".Translate(this.pawn.Named("PAWN"));
			}
			if (this.changedIdeo && this.oldRole != null)
			{
				taggedString += "\n\n" + "LetterRoleLostLetterIdeoChangedPostfix".Translate(this.pawn.Named("PAWN"), this.oldRole.Named("ROLE"), this.oldIdeo.Named("OLDIDEO"));
			}
			return taggedString;
		}

		// Token: 0x06002B7F RID: 11135 RVA: 0x00103BA2 File Offset: 0x00101DA2
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			MentalStateUtility.TryTransitionToWanderOwnRoom(this);
		}

		// Token: 0x06002B80 RID: 11136 RVA: 0x00103BB4 File Offset: 0x00101DB4
		private float GetIdeoWeight(Ideo ideo)
		{
			if (ideo == this.pawn.Ideo)
			{
				return 0f;
			}
			float num = 1f;
			if (this.pawn.Faction != null)
			{
				foreach (Pawn pawn in PawnsFinder.AllMaps_SpawnedPawnsInFaction(this.pawn.Faction))
				{
					if (pawn.ideo != null && pawn.Ideo == ideo)
					{
						num += 1f;
						break;
					}
				}
				foreach (Faction faction in Find.FactionManager.AllFactionsVisible)
				{
					if (faction != this.pawn.Faction && faction.RelationKindWith(this.pawn.Faction) == FactionRelationKind.Ally && faction.ideos.IsPrimary(ideo))
					{
						num += 1f;
						break;
					}
				}
			}
			if (this.pawn.Spawned)
			{
				using (List<Pawn>.Enumerator enumerator = this.pawn.Map.mapPawns.AllPawnsSpawned.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Pawn pawn2 = enumerator.Current;
						if (pawn2.Faction != null && pawn2.ideo != null && pawn2.Ideo == ideo && (pawn2.Faction == this.pawn.Faction || !pawn2.Faction.HostileTo(this.pawn.Faction)))
						{
							num += 1f;
							break;
						}
					}
					return num;
				}
			}
			Caravan caravan = this.pawn.GetCaravan();
			if (caravan != null)
			{
				foreach (Pawn pawn3 in caravan.PawnsListForReading)
				{
					if (pawn3.ideo != null && pawn3.Ideo == ideo)
					{
						num += 1f;
						break;
					}
				}
			}
			return num;
		}

		// Token: 0x06002B81 RID: 11137 RVA: 0x00103DE0 File Offset: 0x00101FE0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Ideo>(ref this.oldIdeo, "oldIdeo", false);
			Scribe_References.Look<Ideo>(ref this.newIdeo, "newIdeo", false);
			Scribe_References.Look<Precept_Role>(ref this.oldRole, "oldRole", false);
			Scribe_Values.Look<bool>(ref this.changedIdeo, "changedIdeo", false, false);
			Scribe_Values.Look<float>(ref this.newCertainty, "newCertainty", 0f, false);
		}

		// Token: 0x04001A87 RID: 6791
		private Ideo oldIdeo;

		// Token: 0x04001A88 RID: 6792
		private Ideo newIdeo;

		// Token: 0x04001A89 RID: 6793
		private Precept_Role oldRole;

		// Token: 0x04001A8A RID: 6794
		private bool changedIdeo;

		// Token: 0x04001A8B RID: 6795
		private float newCertainty;

		// Token: 0x04001A8C RID: 6796
		private const float ConversionChance = 0.5f;
	}
}
