using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200034B RID: 843
	public class Graphic_Appearances : Graphic
	{
		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06001808 RID: 6152 RVA: 0x0008EFDB File Offset: 0x0008D1DB
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[(int)StuffAppearanceDefOf.Smooth.index].MatSingle;
			}
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x0008EFF3 File Offset: 0x0008D1F3
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			return this.SubGraphicFor(thing).MatAt(rot, thing);
		}

		// Token: 0x0600180A RID: 6154 RVA: 0x0008F004 File Offset: 0x0008D204
		public override void Init(GraphicRequest req)
		{
			this.data = req.graphicData;
			this.path = req.path;
			this.color = req.color;
			this.drawSize = req.drawSize;
			List<StuffAppearanceDef> allDefsListForReading = DefDatabase<StuffAppearanceDef>.AllDefsListForReading;
			this.subGraphics = new Graphic[allDefsListForReading.Count];
			for (int i = 0; i < this.subGraphics.Length; i++)
			{
				StuffAppearanceDef stuffAppearance = allDefsListForReading[i];
				string text = req.path;
				if (!stuffAppearance.pathPrefix.NullOrEmpty())
				{
					text = stuffAppearance.pathPrefix + "/" + text.Split(new char[]
					{
						'/'
					}).Last<string>();
				}
				Texture2D texture2D = (from x in ContentFinder<Texture2D>.GetAllInFolder(text)
				where x.name.EndsWith(stuffAppearance.defName)
				select x).FirstOrDefault<Texture2D>();
				if (texture2D != null)
				{
					this.subGraphics[i] = GraphicDatabase.Get<Graphic_Single>(text + "/" + texture2D.name, req.shader, this.drawSize, this.color);
				}
			}
			for (int j = 0; j < this.subGraphics.Length; j++)
			{
				if (this.subGraphics[j] == null)
				{
					this.subGraphics[j] = this.subGraphics[(int)StuffAppearanceDefOf.Smooth.index];
				}
			}
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x0008F15E File Offset: 0x0008D35E
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			if (newColorTwo != Color.white)
			{
				Log.ErrorOnce("Cannot use Graphic_Appearances.GetColoredVersion with a non-white colorTwo.", 9910251);
			}
			return GraphicDatabase.Get<Graphic_Appearances>(this.path, newShader, this.drawSize, newColor, Color.white, this.data, null);
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x0008F19B File Offset: 0x0008D39B
		public override Material MatSingleFor(Thing thing)
		{
			return this.SubGraphicFor(thing).MatSingleFor(thing);
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x0008F1AA File Offset: 0x0008D3AA
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			this.SubGraphicFor(thing).DrawWorker(loc, rot, thingDef, thing, extraRotation);
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x0008F1C0 File Offset: 0x0008D3C0
		public Graphic SubGraphicFor(Thing thing)
		{
			StuffAppearanceDef smooth = StuffAppearanceDefOf.Smooth;
			if (thing != null)
			{
				return this.SubGraphicFor(thing.Stuff);
			}
			return this.subGraphics[(int)smooth.index];
		}

		// Token: 0x0600180F RID: 6159 RVA: 0x0008F1F0 File Offset: 0x0008D3F0
		public Graphic SubGraphicFor(ThingDef stuff)
		{
			StuffAppearanceDef stuffAppearanceDef = StuffAppearanceDefOf.Smooth;
			if (stuff != null && stuff.stuffProps.appearance != null)
			{
				stuffAppearanceDef = stuff.stuffProps.appearance;
			}
			return this.subGraphics[(int)stuffAppearanceDef.index];
		}

		// Token: 0x06001810 RID: 6160 RVA: 0x0008F22C File Offset: 0x0008D42C
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Appearance(path=",
				this.path,
				", color=",
				this.color,
				", colorTwo=unsupported)"
			});
		}

		// Token: 0x0400107C RID: 4220
		protected Graphic[] subGraphics;
	}
}
