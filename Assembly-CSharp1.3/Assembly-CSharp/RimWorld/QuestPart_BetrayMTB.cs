using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B3F RID: 2879
	public class QuestPart_BetrayMTB : QuestPart_MTB
	{
		// Token: 0x17000BC6 RID: 3014
		// (get) Token: 0x06004352 RID: 17234 RVA: 0x00167290 File Offset: 0x00165490
		protected override float MTBDays
		{
			get
			{
				Map map = null;
				bool flag = false;
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.CanBetray(this.pawns[i]))
					{
						flag = true;
						map = this.pawns[i].MapHeld;
						break;
					}
				}
				if (!flag)
				{
					return -1f;
				}
				int num = 0;
				int num2 = 0;
				List<Pawn> list = map.mapPawns.PawnsInFaction(Faction.OfPlayer);
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].IsColonist)
					{
						num++;
						if (list[j].Downed)
						{
							num2++;
						}
					}
				}
				if (num <= 1)
				{
					return QuestPart_BetrayMTB.DownedColonistsExcept1PctToMTBDaysCurve.Evaluate(1f);
				}
				return QuestPart_BetrayMTB.DownedColonistsExcept1PctToMTBDaysCurve.Evaluate((float)num2 / (float)(num - 1));
			}
		}

		// Token: 0x06004353 RID: 17235 RVA: 0x00167369 File Offset: 0x00165569
		private bool CanBetray(Pawn p)
		{
			return !p.DestroyedOrNull() && !p.Downed && p.IsFreeColonist && !p.InMentalState && !p.IsBurning() && !p.Suspended && p.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x06004354 RID: 17236 RVA: 0x001673A4 File Offset: 0x001655A4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x040028EC RID: 10476
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x040028ED RID: 10477
		private static readonly SimpleCurve DownedColonistsExcept1PctToMTBDaysCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 120f),
				true
			},
			{
				new CurvePoint(1f, 15f),
				true
			}
		};
	}
}
