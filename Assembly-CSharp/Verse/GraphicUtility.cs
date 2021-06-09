using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020004F1 RID: 1265
	public static class GraphicUtility
	{
		// Token: 0x06001F84 RID: 8068 RVA: 0x00100414 File Offset: 0x000FE614
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

		// Token: 0x06001F85 RID: 8069 RVA: 0x00100448 File Offset: 0x000FE648
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
			default:
				throw new ArgumentException();
			}
		}
	}
}
