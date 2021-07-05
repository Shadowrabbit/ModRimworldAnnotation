using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200034A RID: 842
	public static class GraphicUtility
	{
		// Token: 0x06001806 RID: 6150 RVA: 0x0008EF50 File Offset: 0x0008D150
		public static Graphic ExtractInnerGraphicFor(this Graphic outerGraphic, Thing thing)
		{
			Graphic_Random graphic_Random = outerGraphic as Graphic_Random;
			if (graphic_Random != null)
			{
				return graphic_Random.SubGraphicFor(thing);
			}
			Graphic_Appearances graphic_Appearances = outerGraphic as Graphic_Appearances;
			if (graphic_Appearances != null)
			{
				return graphic_Appearances.SubGraphicFor(thing);
			}
			return outerGraphic;
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x0008EF84 File Offset: 0x0008D184
		public static Graphic_Linked WrapLinked(Graphic subGraphic, LinkDrawerType linkDrawerType)
		{
			switch (linkDrawerType)
			{
			case LinkDrawerType.None:
				return null;
			case LinkDrawerType.Basic:
				return new Graphic_Linked(subGraphic);
			case LinkDrawerType.CornerFiller:
				return new Graphic_LinkedCornerFiller(subGraphic);
			case LinkDrawerType.Transmitter:
				return new Graphic_LinkedTransmitter(subGraphic);
			case LinkDrawerType.TransmitterOverlay:
				return new Graphic_LinkedTransmitterOverlay(subGraphic);
			case LinkDrawerType.Asymmetric:
				return new Graphic_LinkedAsymmetric(subGraphic);
			default:
				throw new ArgumentException();
			}
		}
	}
}
