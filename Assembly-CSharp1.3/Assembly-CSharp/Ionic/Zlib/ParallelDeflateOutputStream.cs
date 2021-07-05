using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Ionic.Crc;

namespace Ionic.Zlib
{
	// Token: 0x02001831 RID: 6193
	public class ParallelDeflateOutputStream : Stream
	{
		// Token: 0x060091AB RID: 37291 RVA: 0x003466A2 File Offset: 0x003448A2
		public ParallelDeflateOutputStream(Stream stream) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, false)
		{
		}

		// Token: 0x060091AC RID: 37292 RVA: 0x003466AE File Offset: 0x003448AE
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level) : this(stream, level, CompressionStrategy.Default, false)
		{
		}

		// Token: 0x060091AD RID: 37293 RVA: 0x003466BA File Offset: 0x003448BA
		public ParallelDeflateOutputStream(Stream stream, bool leaveOpen) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
		{
		}

		// Token: 0x060091AE RID: 37294 RVA: 0x003466C6 File Offset: 0x003448C6
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, bool leaveOpen) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
		{
		}

		// Token: 0x060091AF RID: 37295 RVA: 0x003466D4 File Offset: 0x003448D4
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, CompressionStrategy strategy, bool leaveOpen)
		{
			this._outStream = stream;
			this._compressLevel = level;
			this.Strategy = strategy;
			this._leaveOpen = leaveOpen;
			this.MaxBufferPairs = 16;
		}

		// Token: 0x17001805 RID: 6149
		// (get) Token: 0x060091B0 RID: 37296 RVA: 0x00346743 File Offset: 0x00344943
		// (set) Token: 0x060091B1 RID: 37297 RVA: 0x0034674B File Offset: 0x0034494B
		public CompressionStrategy Strategy { get; private set; }

		// Token: 0x17001806 RID: 6150
		// (get) Token: 0x060091B2 RID: 37298 RVA: 0x00346754 File Offset: 0x00344954
		// (set) Token: 0x060091B3 RID: 37299 RVA: 0x0034675C File Offset: 0x0034495C
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

		// Token: 0x17001807 RID: 6151
		// (get) Token: 0x060091B4 RID: 37300 RVA: 0x00346779 File Offset: 0x00344979
		// (set) Token: 0x060091B5 RID: 37301 RVA: 0x00346781 File Offset: 0x00344981
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

		// Token: 0x17001808 RID: 6152
		// (get) Token: 0x060091B6 RID: 37302 RVA: 0x003467A2 File Offset: 0x003449A2
		public int Crc32
		{
			get
			{
				return this._Crc32;
			}
		}

		// Token: 0x17001809 RID: 6153
		// (get) Token: 0x060091B7 RID: 37303 RVA: 0x003467AA File Offset: 0x003449AA
		public long BytesProcessed
		{
			get
			{
				return this._totalBytesProcessed;
			}
		}

		// Token: 0x060091B8 RID: 37304 RVA: 0x003467B4 File Offset: 0x003449B4
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

		// Token: 0x060091B9 RID: 37305 RVA: 0x0034686C File Offset: 0x00344A6C
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

		// Token: 0x060091BA RID: 37306 RVA: 0x003469C8 File Offset: 0x00344BC8
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

		// Token: 0x060091BB RID: 37307 RVA: 0x00346A84 File Offset: 0x00344C84
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

		// Token: 0x060091BC RID: 37308 RVA: 0x00346AEB File Offset: 0x00344CEB
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

		// Token: 0x060091BD RID: 37309 RVA: 0x00346B20 File Offset: 0x00344D20
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

		// Token: 0x060091BE RID: 37310 RVA: 0x00346B83 File Offset: 0x00344D83
		public new void Dispose()
		{
			this.Close();
			this._pool = null;
			this.Dispose(true);
		}

		// Token: 0x060091BF RID: 37311 RVA: 0x00346B99 File Offset: 0x00344D99
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		// Token: 0x060091C0 RID: 37312 RVA: 0x00346BA4 File Offset: 0x00344DA4
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

		// Token: 0x060091C1 RID: 37313 RVA: 0x00346C6C File Offset: 0x00344E6C
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

		// Token: 0x060091C2 RID: 37314 RVA: 0x00346E0C File Offset: 0x0034500C
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

		// Token: 0x060091C3 RID: 37315 RVA: 0x00346F2C File Offset: 0x0034512C
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

		// Token: 0x060091C4 RID: 37316 RVA: 0x00346FA0 File Offset: 0x003451A0
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

		// Token: 0x1700180A RID: 6154
		// (get) Token: 0x060091C5 RID: 37317 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700180B RID: 6155
		// (get) Token: 0x060091C6 RID: 37318 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700180C RID: 6156
		// (get) Token: 0x060091C7 RID: 37319 RVA: 0x00347018 File Offset: 0x00345218
		public override bool CanWrite
		{
			get
			{
				return this._outStream.CanWrite;
			}
		}

		// Token: 0x1700180D RID: 6157
		// (get) Token: 0x060091C8 RID: 37320 RVA: 0x00347025 File Offset: 0x00345225
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700180E RID: 6158
		// (get) Token: 0x060091C9 RID: 37321 RVA: 0x0034702C File Offset: 0x0034522C
		// (set) Token: 0x060091CA RID: 37322 RVA: 0x00347025 File Offset: 0x00345225
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

		// Token: 0x060091CB RID: 37323 RVA: 0x00347025 File Offset: 0x00345225
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060091CC RID: 37324 RVA: 0x00347025 File Offset: 0x00345225
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060091CD RID: 37325 RVA: 0x00347025 File Offset: 0x00345225
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x04005BCE RID: 23502
		private static readonly int IO_BUFFER_SIZE_DEFAULT = 65536;

		// Token: 0x04005BCF RID: 23503
		private static readonly int BufferPairsPerCore = 4;

		// Token: 0x04005BD0 RID: 23504
		private List<WorkItem> _pool;

		// Token: 0x04005BD1 RID: 23505
		private bool _leaveOpen;

		// Token: 0x04005BD2 RID: 23506
		private bool emitting;

		// Token: 0x04005BD3 RID: 23507
		private Stream _outStream;

		// Token: 0x04005BD4 RID: 23508
		private int _maxBufferPairs;

		// Token: 0x04005BD5 RID: 23509
		private int _bufferSize = ParallelDeflateOutputStream.IO_BUFFER_SIZE_DEFAULT;

		// Token: 0x04005BD6 RID: 23510
		private AutoResetEvent _newlyCompressedBlob;

		// Token: 0x04005BD7 RID: 23511
		private object _outputLock = new object();

		// Token: 0x04005BD8 RID: 23512
		private bool _isClosed;

		// Token: 0x04005BD9 RID: 23513
		private bool _firstWriteDone;

		// Token: 0x04005BDA RID: 23514
		private int _currentlyFilling;

		// Token: 0x04005BDB RID: 23515
		private int _lastFilled;

		// Token: 0x04005BDC RID: 23516
		private int _lastWritten;

		// Token: 0x04005BDD RID: 23517
		private int _latestCompressed;

		// Token: 0x04005BDE RID: 23518
		private int _Crc32;

		// Token: 0x04005BDF RID: 23519
		private CRC32 _runningCrc;

		// Token: 0x04005BE0 RID: 23520
		private object _latestLock = new object();

		// Token: 0x04005BE1 RID: 23521
		private Queue<int> _toWrite;

		// Token: 0x04005BE2 RID: 23522
		private Queue<int> _toFill;

		// Token: 0x04005BE3 RID: 23523
		private long _totalBytesProcessed;

		// Token: 0x04005BE4 RID: 23524
		private CompressionLevel _compressLevel;

		// Token: 0x04005BE5 RID: 23525
		private volatile Exception _pendingException;

		// Token: 0x04005BE6 RID: 23526
		private bool _handlingException;

		// Token: 0x04005BE7 RID: 23527
		private object _eLock = new object();

		// Token: 0x04005BE8 RID: 23528
		private ParallelDeflateOutputStream.TraceBits _DesiredTrace = ParallelDeflateOutputStream.TraceBits.EmitLock | ParallelDeflateOutputStream.TraceBits.EmitEnter | ParallelDeflateOutputStream.TraceBits.EmitBegin | ParallelDeflateOutputStream.TraceBits.EmitDone | ParallelDeflateOutputStream.TraceBits.EmitSkip | ParallelDeflateOutputStream.TraceBits.Session | ParallelDeflateOutputStream.TraceBits.Compress | ParallelDeflateOutputStream.TraceBits.WriteEnter | ParallelDeflateOutputStream.TraceBits.WriteTake;

		// Token: 0x02002A9F RID: 10911
		[Flags]
		private enum TraceBits : uint
		{
			// Token: 0x0400A08C RID: 41100
			None = 0U,
			// Token: 0x0400A08D RID: 41101
			NotUsed1 = 1U,
			// Token: 0x0400A08E RID: 41102
			EmitLock = 2U,
			// Token: 0x0400A08F RID: 41103
			EmitEnter = 4U,
			// Token: 0x0400A090 RID: 41104
			EmitBegin = 8U,
			// Token: 0x0400A091 RID: 41105
			EmitDone = 16U,
			// Token: 0x0400A092 RID: 41106
			EmitSkip = 32U,
			// Token: 0x0400A093 RID: 41107
			EmitAll = 58U,
			// Token: 0x0400A094 RID: 41108
			Flush = 64U,
			// Token: 0x0400A095 RID: 41109
			Lifecycle = 128U,
			// Token: 0x0400A096 RID: 41110
			Session = 256U,
			// Token: 0x0400A097 RID: 41111
			Synch = 512U,
			// Token: 0x0400A098 RID: 41112
			Instance = 1024U,
			// Token: 0x0400A099 RID: 41113
			Compress = 2048U,
			// Token: 0x0400A09A RID: 41114
			Write = 4096U,
			// Token: 0x0400A09B RID: 41115
			WriteEnter = 8192U,
			// Token: 0x0400A09C RID: 41116
			WriteTake = 16384U,
			// Token: 0x0400A09D RID: 41117
			All = 4294967295U
		}
	}
}
