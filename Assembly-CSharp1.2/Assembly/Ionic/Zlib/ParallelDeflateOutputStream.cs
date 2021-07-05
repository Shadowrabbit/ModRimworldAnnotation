using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Ionic.Crc;

namespace Ionic.Zlib
{
	// Token: 0x02002214 RID: 8724
	public class ParallelDeflateOutputStream : Stream
	{
		// Token: 0x0600BB63 RID: 47971 RVA: 0x000794F0 File Offset: 0x000776F0
		public ParallelDeflateOutputStream(Stream stream) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, false)
		{
		}

		// Token: 0x0600BB64 RID: 47972 RVA: 0x000794FC File Offset: 0x000776FC
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level) : this(stream, level, CompressionStrategy.Default, false)
		{
		}

		// Token: 0x0600BB65 RID: 47973 RVA: 0x00079508 File Offset: 0x00077708
		public ParallelDeflateOutputStream(Stream stream, bool leaveOpen) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
		{
		}

		// Token: 0x0600BB66 RID: 47974 RVA: 0x00079514 File Offset: 0x00077714
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, bool leaveOpen) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
		{
		}

		// Token: 0x0600BB67 RID: 47975 RVA: 0x00360750 File Offset: 0x0035E950
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, CompressionStrategy strategy, bool leaveOpen)
		{
			this._outStream = stream;
			this._compressLevel = level;
			this.Strategy = strategy;
			this._leaveOpen = leaveOpen;
			this.MaxBufferPairs = 16;
		}

		// Token: 0x17001BF5 RID: 7157
		// (get) Token: 0x0600BB68 RID: 47976 RVA: 0x00079520 File Offset: 0x00077720
		// (set) Token: 0x0600BB69 RID: 47977 RVA: 0x00079528 File Offset: 0x00077728
		public CompressionStrategy Strategy { get; private set; }

		// Token: 0x17001BF6 RID: 7158
		// (get) Token: 0x0600BB6A RID: 47978 RVA: 0x00079531 File Offset: 0x00077731
		// (set) Token: 0x0600BB6B RID: 47979 RVA: 0x00079539 File Offset: 0x00077739
		public int MaxBufferPairs
		{
			get
			{
				return this._maxBufferPairs;
			}
			set
			{
				if (value < 4)
				{
					throw new ArgumentException("MaxBufferPairs", "Value must be 4 or greater.");
				}
				this._maxBufferPairs = value;
			}
		}

		// Token: 0x17001BF7 RID: 7159
		// (get) Token: 0x0600BB6C RID: 47980 RVA: 0x00079556 File Offset: 0x00077756
		// (set) Token: 0x0600BB6D RID: 47981 RVA: 0x0007955E File Offset: 0x0007775E
		public int BufferSize
		{
			get
			{
				return this._bufferSize;
			}
			set
			{
				if (value < 1024)
				{
					throw new ArgumentOutOfRangeException("BufferSize", "BufferSize must be greater than 1024 bytes");
				}
				this._bufferSize = value;
			}
		}

		// Token: 0x17001BF8 RID: 7160
		// (get) Token: 0x0600BB6E RID: 47982 RVA: 0x0007957F File Offset: 0x0007777F
		public int Crc32
		{
			get
			{
				return this._Crc32;
			}
		}

		// Token: 0x17001BF9 RID: 7161
		// (get) Token: 0x0600BB6F RID: 47983 RVA: 0x00079587 File Offset: 0x00077787
		public long BytesProcessed
		{
			get
			{
				return this._totalBytesProcessed;
			}
		}

		// Token: 0x0600BB70 RID: 47984 RVA: 0x003607C0 File Offset: 0x0035E9C0
		private void _InitializePoolOfWorkItems()
		{
			this._toWrite = new Queue<int>();
			this._toFill = new Queue<int>();
			this._pool = new List<WorkItem>();
			int num = ParallelDeflateOutputStream.BufferPairsPerCore * Environment.ProcessorCount;
			num = Math.Min(num, this._maxBufferPairs);
			for (int i = 0; i < num; i++)
			{
				this._pool.Add(new WorkItem(this._bufferSize, this._compressLevel, this.Strategy, i));
				this._toFill.Enqueue(i);
			}
			this._newlyCompressedBlob = new AutoResetEvent(false);
			this._runningCrc = new CRC32();
			this._currentlyFilling = -1;
			this._lastFilled = -1;
			this._lastWritten = -1;
			this._latestCompressed = -1;
		}

		// Token: 0x0600BB71 RID: 47985 RVA: 0x00360878 File Offset: 0x0035EA78
		public override void Write(byte[] buffer, int offset, int count)
		{
			bool mustWait = false;
			if (this._isClosed)
			{
				throw new InvalidOperationException();
			}
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (count == 0)
			{
				return;
			}
			if (!this._firstWriteDone)
			{
				this._InitializePoolOfWorkItems();
				this._firstWriteDone = true;
			}
			for (;;)
			{
				this.EmitPendingBuffers(false, mustWait);
				mustWait = false;
				int num;
				if (this._currentlyFilling >= 0)
				{
					num = this._currentlyFilling;
					goto IL_98;
				}
				if (this._toFill.Count != 0)
				{
					num = this._toFill.Dequeue();
					this._lastFilled++;
					goto IL_98;
				}
				mustWait = true;
				IL_145:
				if (count <= 0)
				{
					return;
				}
				continue;
				IL_98:
				WorkItem workItem = this._pool[num];
				int num2 = (workItem.buffer.Length - workItem.inputBytesAvailable > count) ? count : (workItem.buffer.Length - workItem.inputBytesAvailable);
				workItem.ordinal = this._lastFilled;
				Buffer.BlockCopy(buffer, offset, workItem.buffer, workItem.inputBytesAvailable, num2);
				count -= num2;
				offset += num2;
				workItem.inputBytesAvailable += num2;
				if (workItem.inputBytesAvailable == workItem.buffer.Length)
				{
					if (!ThreadPool.QueueUserWorkItem(new WaitCallback(this._DeflateOne), workItem))
					{
						break;
					}
					this._currentlyFilling = -1;
				}
				else
				{
					this._currentlyFilling = num;
				}
				goto IL_145;
			}
			throw new Exception("Cannot enqueue workitem");
		}

		// Token: 0x0600BB72 RID: 47986 RVA: 0x003609D4 File Offset: 0x0035EBD4
		private void _FlushFinish()
		{
			byte[] array = new byte[128];
			ZlibCodec zlibCodec = new ZlibCodec();
			int num = zlibCodec.InitializeDeflate(this._compressLevel, false);
			zlibCodec.InputBuffer = null;
			zlibCodec.NextIn = 0;
			zlibCodec.AvailableBytesIn = 0;
			zlibCodec.OutputBuffer = array;
			zlibCodec.NextOut = 0;
			zlibCodec.AvailableBytesOut = array.Length;
			num = zlibCodec.Deflate(FlushType.Finish);
			if (num != 1 && num != 0)
			{
				throw new Exception("deflating: " + zlibCodec.Message);
			}
			if (array.Length - zlibCodec.AvailableBytesOut > 0)
			{
				this._outStream.Write(array, 0, array.Length - zlibCodec.AvailableBytesOut);
			}
			zlibCodec.EndDeflate();
			this._Crc32 = this._runningCrc.Crc32Result;
		}

		// Token: 0x0600BB73 RID: 47987 RVA: 0x00360A90 File Offset: 0x0035EC90
		private void _Flush(bool lastInput)
		{
			if (this._isClosed)
			{
				throw new InvalidOperationException();
			}
			if (this.emitting)
			{
				return;
			}
			if (this._currentlyFilling >= 0)
			{
				WorkItem wi = this._pool[this._currentlyFilling];
				this._DeflateOne(wi);
				this._currentlyFilling = -1;
			}
			if (lastInput)
			{
				this.EmitPendingBuffers(true, false);
				this._FlushFinish();
				return;
			}
			this.EmitPendingBuffers(false, false);
		}

		// Token: 0x0600BB74 RID: 47988 RVA: 0x0007958F File Offset: 0x0007778F
		public override void Flush()
		{
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (this._handlingException)
			{
				return;
			}
			this._Flush(false);
		}

		// Token: 0x0600BB75 RID: 47989 RVA: 0x00360AF8 File Offset: 0x0035ECF8
		public override void Close()
		{
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (this._handlingException)
			{
				return;
			}
			if (this._isClosed)
			{
				return;
			}
			this._Flush(true);
			if (!this._leaveOpen)
			{
				this._outStream.Close();
			}
			this._isClosed = true;
		}

		// Token: 0x0600BB76 RID: 47990 RVA: 0x000795C4 File Offset: 0x000777C4
		public new void Dispose()
		{
			this.Close();
			this._pool = null;
			this.Dispose(true);
		}

		// Token: 0x0600BB77 RID: 47991 RVA: 0x000795DA File Offset: 0x000777DA
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		// Token: 0x0600BB78 RID: 47992 RVA: 0x00360B5C File Offset: 0x0035ED5C
		public void Reset(Stream stream)
		{
			if (!this._firstWriteDone)
			{
				return;
			}
			this._toWrite.Clear();
			this._toFill.Clear();
			foreach (WorkItem workItem in this._pool)
			{
				this._toFill.Enqueue(workItem.index);
				workItem.ordinal = -1;
			}
			this._firstWriteDone = false;
			this._totalBytesProcessed = 0L;
			this._runningCrc = new CRC32();
			this._isClosed = false;
			this._currentlyFilling = -1;
			this._lastFilled = -1;
			this._lastWritten = -1;
			this._latestCompressed = -1;
			this._outStream = stream;
		}

		// Token: 0x0600BB79 RID: 47993 RVA: 0x00360C24 File Offset: 0x0035EE24
		private void EmitPendingBuffers(bool doAll, bool mustWait)
		{
			if (this.emitting)
			{
				return;
			}
			this.emitting = true;
			if (doAll || mustWait)
			{
				this._newlyCompressedBlob.WaitOne();
			}
			do
			{
				int num = -1;
				int num2 = doAll ? 200 : (mustWait ? -1 : 0);
				int num3 = -1;
				do
				{
					if (Monitor.TryEnter(this._toWrite, num2))
					{
						num3 = -1;
						try
						{
							if (this._toWrite.Count > 0)
							{
								num3 = this._toWrite.Dequeue();
							}
						}
						finally
						{
							Monitor.Exit(this._toWrite);
						}
						if (num3 >= 0)
						{
							WorkItem workItem = this._pool[num3];
							if (workItem.ordinal != this._lastWritten + 1)
							{
								Queue<int> toWrite = this._toWrite;
								lock (toWrite)
								{
									this._toWrite.Enqueue(num3);
								}
								if (num == num3)
								{
									this._newlyCompressedBlob.WaitOne();
									num = -1;
								}
								else if (num == -1)
								{
									num = num3;
								}
							}
							else
							{
								num = -1;
								this._outStream.Write(workItem.compressed, 0, workItem.compressedBytesAvailable);
								this._runningCrc.Combine(workItem.crc, workItem.inputBytesAvailable);
								this._totalBytesProcessed += (long)workItem.inputBytesAvailable;
								workItem.inputBytesAvailable = 0;
								this._lastWritten = workItem.ordinal;
								this._toFill.Enqueue(workItem.index);
								if (num2 == -1)
								{
									num2 = 0;
								}
							}
						}
					}
					else
					{
						num3 = -1;
					}
				}
				while (num3 >= 0);
			}
			while (doAll && this._lastWritten != this._latestCompressed);
			this.emitting = false;
		}

		// Token: 0x0600BB7A RID: 47994 RVA: 0x00360DC4 File Offset: 0x0035EFC4
		private void _DeflateOne(object wi)
		{
			WorkItem workItem = (WorkItem)wi;
			try
			{
				CRC32 crc = new CRC32();
				crc.SlurpBlock(workItem.buffer, 0, workItem.inputBytesAvailable);
				this.DeflateOneSegment(workItem);
				workItem.crc = crc.Crc32Result;
				object obj = this._latestLock;
				lock (obj)
				{
					if (workItem.ordinal > this._latestCompressed)
					{
						this._latestCompressed = workItem.ordinal;
					}
				}
				Queue<int> toWrite = this._toWrite;
				lock (toWrite)
				{
					this._toWrite.Enqueue(workItem.index);
				}
				this._newlyCompressedBlob.Set();
			}
			catch (Exception pendingException)
			{
				object obj = this._eLock;
				lock (obj)
				{
					if (this._pendingException != null)
					{
						this._pendingException = pendingException;
					}
				}
			}
		}

		// Token: 0x0600BB7B RID: 47995 RVA: 0x00360EE4 File Offset: 0x0035F0E4
		private bool DeflateOneSegment(WorkItem workitem)
		{
			ZlibCodec compressor = workitem.compressor;
			compressor.ResetDeflate();
			compressor.NextIn = 0;
			compressor.AvailableBytesIn = workitem.inputBytesAvailable;
			compressor.NextOut = 0;
			compressor.AvailableBytesOut = workitem.compressed.Length;
			do
			{
				compressor.Deflate(FlushType.None);
			}
			while (compressor.AvailableBytesIn > 0 || compressor.AvailableBytesOut == 0);
			compressor.Deflate(FlushType.Sync);
			workitem.compressedBytesAvailable = (int)compressor.TotalBytesOut;
			return true;
		}

		// Token: 0x0600BB7C RID: 47996 RVA: 0x00360F58 File Offset: 0x0035F158
		[Conditional("Trace")]
		private void TraceOutput(ParallelDeflateOutputStream.TraceBits bits, string format, params object[] varParams)
		{
			if ((bits & this._DesiredTrace) != ParallelDeflateOutputStream.TraceBits.None)
			{
				object outputLock = this._outputLock;
				lock (outputLock)
				{
					int hashCode = Thread.CurrentThread.GetHashCode();
					Console.ForegroundColor = hashCode % 8 + ConsoleColor.DarkGray;
					Console.Write("{0:000} PDOS ", hashCode);
					Console.WriteLine(format, varParams);
					Console.ResetColor();
				}
			}
		}

		// Token: 0x17001BFA RID: 7162
		// (get) Token: 0x0600BB7D RID: 47997 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001BFB RID: 7163
		// (get) Token: 0x0600BB7E RID: 47998 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001BFC RID: 7164
		// (get) Token: 0x0600BB7F RID: 47999 RVA: 0x000795E3 File Offset: 0x000777E3
		public override bool CanWrite
		{
			get
			{
				return this._outStream.CanWrite;
			}
		}

		// Token: 0x17001BFD RID: 7165
		// (get) Token: 0x0600BB80 RID: 48000 RVA: 0x0000713A File Offset: 0x0000533A
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17001BFE RID: 7166
		// (get) Token: 0x0600BB81 RID: 48001 RVA: 0x000795F0 File Offset: 0x000777F0
		// (set) Token: 0x0600BB82 RID: 48002 RVA: 0x0000713A File Offset: 0x0000533A
		public override long Position
		{
			get
			{
				return this._outStream.Position;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x0600BB83 RID: 48003 RVA: 0x0000713A File Offset: 0x0000533A
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600BB84 RID: 48004 RVA: 0x0000713A File Offset: 0x0000533A
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600BB85 RID: 48005 RVA: 0x0000713A File Offset: 0x0000533A
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x04008057 RID: 32855
		private static readonly int IO_BUFFER_SIZE_DEFAULT = 65536;

		// Token: 0x04008058 RID: 32856
		private static readonly int BufferPairsPerCore = 4;

		// Token: 0x04008059 RID: 32857
		private List<WorkItem> _pool;

		// Token: 0x0400805A RID: 32858
		private bool _leaveOpen;

		// Token: 0x0400805B RID: 32859
		private bool emitting;

		// Token: 0x0400805C RID: 32860
		private Stream _outStream;

		// Token: 0x0400805D RID: 32861
		private int _maxBufferPairs;

		// Token: 0x0400805E RID: 32862
		private int _bufferSize = ParallelDeflateOutputStream.IO_BUFFER_SIZE_DEFAULT;

		// Token: 0x0400805F RID: 32863
		private AutoResetEvent _newlyCompressedBlob;

		// Token: 0x04008060 RID: 32864
		private object _outputLock = new object();

		// Token: 0x04008061 RID: 32865
		private bool _isClosed;

		// Token: 0x04008062 RID: 32866
		private bool _firstWriteDone;

		// Token: 0x04008063 RID: 32867
		private int _currentlyFilling;

		// Token: 0x04008064 RID: 32868
		private int _lastFilled;

		// Token: 0x04008065 RID: 32869
		private int _lastWritten;

		// Token: 0x04008066 RID: 32870
		private int _latestCompressed;

		// Token: 0x04008067 RID: 32871
		private int _Crc32;

		// Token: 0x04008068 RID: 32872
		private CRC32 _runningCrc;

		// Token: 0x04008069 RID: 32873
		private object _latestLock = new object();

		// Token: 0x0400806A RID: 32874
		private Queue<int> _toWrite;

		// Token: 0x0400806B RID: 32875
		private Queue<int> _toFill;

		// Token: 0x0400806C RID: 32876
		private long _totalBytesProcessed;

		// Token: 0x0400806D RID: 32877
		private CompressionLevel _compressLevel;

		// Token: 0x0400806E RID: 32878
		private volatile Exception _pendingException;

		// Token: 0x0400806F RID: 32879
		private bool _handlingException;

		// Token: 0x04008070 RID: 32880
		private object _eLock = new object();

		// Token: 0x04008071 RID: 32881
		private ParallelDeflateOutputStream.TraceBits _DesiredTrace = ParallelDeflateOutputStream.TraceBits.EmitLock | ParallelDeflateOutputStream.TraceBits.EmitEnter | ParallelDeflateOutputStream.TraceBits.EmitBegin | ParallelDeflateOutputStream.TraceBits.EmitDone | ParallelDeflateOutputStream.TraceBits.EmitSkip | ParallelDeflateOutputStream.TraceBits.Session | ParallelDeflateOutputStream.TraceBits.Compress | ParallelDeflateOutputStream.TraceBits.WriteEnter | ParallelDeflateOutputStream.TraceBits.WriteTake;

		// Token: 0x02002215 RID: 8725
		[Flags]
		private enum TraceBits : uint
		{
			// Token: 0x04008074 RID: 32884
			None = 0U,
			// Token: 0x04008075 RID: 32885
			NotUsed1 = 1U,
			// Token: 0x04008076 RID: 32886
			EmitLock = 2U,
			// Token: 0x04008077 RID: 32887
			EmitEnter = 4U,
			// Token: 0x04008078 RID: 32888
			EmitBegin = 8U,
			// Token: 0x04008079 RID: 32889
			EmitDone = 16U,
			// Token: 0x0400807A RID: 32890
			EmitSkip = 32U,
			// Token: 0x0400807B RID: 32891
			EmitAll = 58U,
			// Token: 0x0400807C RID: 32892
			Flush = 64U,
			// Token: 0x0400807D RID: 32893
			Lifecycle = 128U,
			// Token: 0x0400807E RID: 32894
			Session = 256U,
			// Token: 0x0400807F RID: 32895
			Synch = 512U,
			// Token: 0x04008080 RID: 32896
			Instance = 1024U,
			// Token: 0x04008081 RID: 32897
			Compress = 2048U,
			// Token: 0x04008082 RID: 32898
			Write = 4096U,
			// Token: 0x04008083 RID: 32899
			WriteEnter = 8192U,
			// Token: 0x04008084 RID: 32900
			WriteTake = 16384U,
			// Token: 0x04008085 RID: 32901
			All = 4294967295U
		}
	}
}
