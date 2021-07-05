using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E69 RID: 3689
	public class Pawn_FilthTracker : IExposable
	{
		// Token: 0x17000EC5 RID: 3781
		// (get) Token: 0x06005599 RID: 21913 RVA: 0x001CFD68 File Offset: 0x001CDF68
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

		// Token: 0x17000EC6 RID: 3782
		// (get) Token: 0x0600559A RID: 21914 RVA: 0x001CFDFE File Offset: 0x001CDFFE
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

		// Token: 0x0600559B RID: 21915 RVA: 0x001CFE22 File Offset: 0x001CE022
		public Pawn_FilthTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600559C RID: 21916 RVA: 0x001CFE3C File Offset: 0x001CE03C
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.lastTerrainFilthDef, "lastTerrainFilthDef");
			Scribe_Collections.Look<Filth>(ref this.carriedFilth, "carriedFilth", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.carriedFilth.RemoveAll((Filth x) => x == null) != 0)
				{
					Log.Error(this.pawn.ToStringSafe<Pawn>() + " had null carried filth after loading.");
				}
				if (this.carriedFilth.RemoveAll((Filth x) => x.def == null) != 0)
				{
					Log.Error(this.pawn.ToStringSafe<Pawn>() + " had carried filth with null def after loading.");
				}
			}
		}

		// Token: 0x0600559D RID: 21917 RVA: 0x001CFF08 File Offset: 0x001CE108
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
			if (Rand.Value < this.pawn.GetStatValue(StatDefOf.FilthRate, true) * 0.005f)
			{
				if (this.pawn.RaceProps.Humanlike)
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
					if (FilthMaker.TryMakeFilth(this.pawn.Position, this.pawn.Map, filth_Trash, 1, this.AdditionalFilthSourceFlags | FilthSourceFlags.Pawn))
					{
						FilthMonitor.Notify_FilthHumanGenerated();
						return;
					}
				}
				else if (FilthMaker.TryMakeFilth(this.pawn.Position, this.pawn.Map, ThingDefOf.Filth_AnimalFilth, 1, this.AdditionalFilthSourceFlags | FilthSourceFlags.Pawn))
				{
					FilthMonitor.Notify_FilthAnimalGenerated();
				}
			}
		}

		// Token: 0x0600559E RID: 21918 RVA: 0x001CFFEC File Offset: 0x001CE1EC
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

		// Token: 0x0600559F RID: 21919 RVA: 0x001D0144 File Offset: 0x001CE344
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

		// Token: 0x060055A0 RID: 21920 RVA: 0x001D01B8 File Offset: 0x001CE3B8
		private void DropCarriedFilth(Filth f)
		{
			if (FilthMaker.TryMakeFilth(this.pawn.Position, this.pawn.Map, f.def, f.sources, this.AdditionalFilthSourceFlags))
			{
				this.ThinCarriedFilth(f);
			}
		}

		// Token: 0x060055A1 RID: 21921 RVA: 0x001D01F0 File Offset: 0x001CE3F0
		private void ThinCarriedFilth(Filth f)
		{
			f.ThinFilth();
			if (f.thickness <= 0)
			{
				this.carriedFilth.Remove(f);
			}
		}

		// Token: 0x060055A2 RID: 21922 RVA: 0x001D020E File Offset: 0x001CE40E
		public void GainFilth(ThingDef filthDef)
		{
			if (filthDef.filth.TerrainSourced)
			{
				this.lastTerrainFilthDef = filthDef;
			}
			this.GainFilth(filthDef, null);
		}

		// Token: 0x060055A3 RID: 21923 RVA: 0x001D022C File Offset: 0x001CE42C
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

		// Token: 0x0400329E RID: 12958
		private Pawn pawn;

		// Token: 0x0400329F RID: 12959
		private List<Filth> carriedFilth = new List<Filth>();

		// Token: 0x040032A0 RID: 12960
		private ThingDef lastTerrainFilthDef;

		// Token: 0x040032A1 RID: 12961
		private const float FilthPickupChance = 0.1f;

		// Token: 0x040032A2 RID: 12962
		private const float FilthDropChance = 0.05f;

		// Token: 0x040032A3 RID: 12963
		private const int MaxCarriedTerrainFilthThickness = 1;

		// Token: 0x040032A4 RID: 12964
		private const float BaseChanceToSpreadPerCell = 0.005f;
	}
}
