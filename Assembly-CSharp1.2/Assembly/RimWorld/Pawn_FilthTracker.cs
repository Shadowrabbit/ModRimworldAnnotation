using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200150C RID: 5388
	public class Pawn_FilthTracker : IExposable
	{
		// Token: 0x170011EA RID: 4586
		// (get) Token: 0x06007426 RID: 29734 RVA: 0x00236308 File Offset: 0x00234508
		public string FilthReport
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("FilthOnFeet".Translate());
				if (this.carriedFilth.Count == 0)
				{
					stringBuilder.Append("(" + "NoneLower".Translate() + ")");
				}
				else
				{
					for (int i = 0; i < this.carriedFilth.Count; i++)
					{
						stringBuilder.AppendLine(this.carriedFilth[i].LabelCap);
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170011EB RID: 4587
		// (get) Token: 0x06007427 RID: 29735 RVA: 0x0004E4D7 File Offset: 0x0004C6D7
		private FilthSourceFlags AdditionalFilthSourceFlags
		{
			get
			{
				if (this.pawn.Faction != null || !this.pawn.RaceProps.Animal)
				{
					return FilthSourceFlags.Unnatural;
				}
				return FilthSourceFlags.Natural;
			}
		}

		// Token: 0x06007428 RID: 29736 RVA: 0x0004E4FB File Offset: 0x0004C6FB
		public Pawn_FilthTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06007429 RID: 29737 RVA: 0x002363A0 File Offset: 0x002345A0
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.lastTerrainFilthDef, "lastTerrainFilthDef");
			Scribe_Collections.Look<Filth>(ref this.carriedFilth, "carriedFilth", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.carriedFilth.RemoveAll((Filth x) => x == null) != 0)
				{
					Log.Error(this.pawn.ToStringSafe<Pawn>() + " had null carried filth after loading.", false);
				}
				if (this.carriedFilth.RemoveAll((Filth x) => x.def == null) != 0)
				{
					Log.Error(this.pawn.ToStringSafe<Pawn>() + " had carried filth with null def after loading.", false);
				}
			}
		}

		// Token: 0x0600742A RID: 29738 RVA: 0x0023646C File Offset: 0x0023466C
		public void Notify_EnteredNewCell()
		{
			if (Rand.Value < 0.05f)
			{
				this.TryDropFilth();
			}
			if (Rand.Value < 0.1f)
			{
				this.TryPickupFilth();
			}
			if (!this.pawn.RaceProps.Humanlike)
			{
				if (Rand.Value < PawnUtility.AnimalFilthChancePerCell(this.pawn.def, this.pawn.BodySize) && FilthMaker.TryMakeFilth(this.pawn.Position, this.pawn.Map, ThingDefOf.Filth_AnimalFilth, 1, this.AdditionalFilthSourceFlags))
				{
					FilthMonitor.Notify_FilthAnimalGenerated();
					return;
				}
			}
			else if (Rand.Value < PawnUtility.HumanFilthChancePerCell(this.pawn.def, this.pawn.BodySize))
			{
				ThingDef filth_Trash;
				if (this.lastTerrainFilthDef != null && Rand.Chance(0.66f))
				{
					filth_Trash = this.lastTerrainFilthDef;
				}
				else
				{
					filth_Trash = ThingDefOf.Filth_Trash;
				}
				if (FilthMaker.TryMakeFilth(this.pawn.Position, this.pawn.Map, filth_Trash, 1, this.AdditionalFilthSourceFlags))
				{
					FilthMonitor.Notify_FilthHumanGenerated();
				}
			}
		}

		// Token: 0x0600742B RID: 29739 RVA: 0x00236574 File Offset: 0x00234774
		private void TryPickupFilth()
		{
			TerrainDef terrDef = this.pawn.Map.terrainGrid.TerrainAt(this.pawn.Position);
			if (terrDef.generatedFilth != null)
			{
				for (int i = this.carriedFilth.Count - 1; i >= 0; i--)
				{
					if (this.carriedFilth[i].def.filth.TerrainSourced && this.carriedFilth[i].def != terrDef.generatedFilth)
					{
						this.ThinCarriedFilth(this.carriedFilth[i]);
					}
				}
				Filth filth = (from f in this.carriedFilth
				where f.def == terrDef.generatedFilth
				select f).FirstOrDefault<Filth>();
				if (filth == null || filth.thickness < 1)
				{
					this.GainFilth(terrDef.generatedFilth);
					FilthMonitor.Notify_FilthAccumulated();
				}
			}
			List<Thing> thingList = this.pawn.Position.GetThingList(this.pawn.Map);
			for (int j = thingList.Count - 1; j >= 0; j--)
			{
				Filth filth2 = thingList[j] as Filth;
				if (filth2 != null && filth2.CanFilthAttachNow)
				{
					this.GainFilth(filth2.def, filth2.sources);
					filth2.ThinFilth();
				}
			}
		}

		// Token: 0x0600742C RID: 29740 RVA: 0x002366CC File Offset: 0x002348CC
		private void TryDropFilth()
		{
			if (this.carriedFilth.Count == 0)
			{
				return;
			}
			for (int i = this.carriedFilth.Count - 1; i >= 0; i--)
			{
				if (this.carriedFilth[i].CanDropAt(this.pawn.Position, this.pawn.Map, FilthSourceFlags.None))
				{
					this.DropCarriedFilth(this.carriedFilth[i]);
					FilthMonitor.Notify_FilthDropped();
				}
			}
		}

		// Token: 0x0600742D RID: 29741 RVA: 0x0004E515 File Offset: 0x0004C715
		private void DropCarriedFilth(Filth f)
		{
			if (FilthMaker.TryMakeFilth(this.pawn.Position, this.pawn.Map, f.def, f.sources, this.AdditionalFilthSourceFlags))
			{
				this.ThinCarriedFilth(f);
			}
		}

		// Token: 0x0600742E RID: 29742 RVA: 0x0004E54D File Offset: 0x0004C74D
		private void ThinCarriedFilth(Filth f)
		{
			f.ThinFilth();
			if (f.thickness <= 0)
			{
				this.carriedFilth.Remove(f);
			}
		}

		// Token: 0x0600742F RID: 29743 RVA: 0x0004E56B File Offset: 0x0004C76B
		public void GainFilth(ThingDef filthDef)
		{
			if (filthDef.filth.TerrainSourced)
			{
				this.lastTerrainFilthDef = filthDef;
			}
			this.GainFilth(filthDef, null);
		}

		// Token: 0x06007430 RID: 29744 RVA: 0x00236740 File Offset: 0x00234940
		public void GainFilth(ThingDef filthDef, IEnumerable<string> sources)
		{
			if (filthDef.filth.TerrainSourced)
			{
				this.lastTerrainFilthDef = filthDef;
			}
			Filth filth = null;
			for (int i = 0; i < this.carriedFilth.Count; i++)
			{
				if (this.carriedFilth[i].def == filthDef)
				{
					filth = this.carriedFilth[i];
					break;
				}
			}
			if (filth != null)
			{
				if (filth.CanBeThickened)
				{
					filth.ThickenFilth();
					filth.AddSources(sources);
					return;
				}
			}
			else
			{
				Filth filth2 = (Filth)ThingMaker.MakeThing(filthDef, null);
				filth2.AddSources(sources);
				this.carriedFilth.Add(filth2);
			}
		}

		// Token: 0x04004CA4 RID: 19620
		private Pawn pawn;

		// Token: 0x04004CA5 RID: 19621
		private List<Filth> carriedFilth = new List<Filth>();

		// Token: 0x04004CA6 RID: 19622
		private ThingDef lastTerrainFilthDef;

		// Token: 0x04004CA7 RID: 19623
		private const float FilthPickupChance = 0.1f;

		// Token: 0x04004CA8 RID: 19624
		private const float FilthDropChance = 0.05f;

		// Token: 0x04004CA9 RID: 19625
		private const int MaxCarriedTerrainFilthThickness = 1;
	}
}
