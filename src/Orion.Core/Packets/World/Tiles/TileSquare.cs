// Copyright (c) 2020 Pryaxis & Orion Contributors
// 
// This file is part of Orion.
// 
// Orion is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Orion is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Orion.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Orion.Core.Packets.DataStructures;
using Orion.Core.Utils;
using Orion.Core.World;
using Orion.Core.World.Tiles;

namespace Orion.Core.Packets.World.Tiles
{
    /// <summary>
    /// A packet sent to set a square of tiles.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct TileSquare : IPacket
    {
        // The shifts for the tile header.
        private const int SlopeShift = 12;

        // The masks for the tile header.
        private const ushort HasBlockMask /*        */ = 0b_00000000_00000001;
        private const ushort HasWallMask /*         */ = 0b_00000000_00000100;
        private const ushort HasLiquidMask /*       */ = 0b_00000000_00001000;
        private const ushort HasRedWireMask /*      */ = 0b_00000000_00010000;
        private const ushort IsBlockHalvedMask /*   */ = 0b_00000000_00100000;
        private const ushort HasActuatorMask /*     */ = 0b_00000000_01000000;
        private const ushort IsBlockActuatedMask /* */ = 0b_00000000_10000000;
        private const ushort HasBlueWireMask /*     */ = 0b_00000001_00000000;
        private const ushort HasGreenWireMask /*    */ = 0b_00000010_00000000;
        private const ushort HasBlockColorMask /*   */ = 0b_00000100_00000000;
        private const ushort HasWallColorMask /*    */ = 0b_00001000_00000000;
        private const ushort SlopeMask /*           */ = 0b_01110000_00000000;
        private const ushort HasYellowWireMask /*   */ = 0b_10000000_00000000;
        private const byte FullbrightBlockMask /*   */ = 0b_10000000;
        private const byte FullbrightWallMask /*    */ = 0b_01000000;
        private const byte InvisibleBlockMask /*    */ = 0b_00100000;
        private const byte InvisibleWallMask /*     */ = 0b_00010000;

        [FieldOffset(0)] private byte _bytes;  // Used to obtain an interior reference.
        [FieldOffset(4)] private byte _bytes2;
        [FieldOffset(8)] private ITileSlice? _tiles;

        /// <summary>
        /// Gets or sets the top-left tile's X coordinate.
        /// </summary>
        /// <value>The top-left tile's X coordinate.</value>
        [field: FieldOffset(0)] public short X { get; set; }

        /// <summary>
        /// Gets or sets the top-left tile's Y coordinate.
        /// </summary>
        /// <value>The top-left tile's Y coordinate.</value>
        [field: FieldOffset(2)] public short Y { get; set; }

        /// <summary>
        /// Gets or sets the tilechangetype of this tilesquare.
        /// </summary>
        [field: FieldOffset(4)] public TileChangeType ChangeType { get; set; }

        /// <summary>
        /// Gets or sets the square of tiles.
        /// </summary>
        /// <value>The square of tiles.</value>
        /// <exception cref="ArgumentException"><paramref name="value"/> is not a square.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        public ITileSlice Tiles
        {
            get => _tiles ??= NetworkTileSlice.Empty;

            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _tiles = value;
            }
        }

        PacketId IPacket.Id => PacketId.TileSquare;

        int IPacket.ReadBody(Span<byte> span, PacketContext context)
        {
            var length = 3 + span.Read(ref _bytes, 4);
            var width = span.At(4);
            var height = span.At(5);

            ChangeType = (TileChangeType)span.At(6);
            Tiles = new NetworkTileSlice(width, height);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    length += ReadTile(span[length..], ref Tiles[i, j]);
                }
            }

            return length;
        }

        int IPacket.WriteBody(Span<byte> span, PacketContext context)
        {
            var length = 2 + span.Write(ref _bytes, 4);
            span.At(4) = (byte)Tiles.Width;
            span.At(5) = (byte)Tiles.Height;
            span.At(6) = (byte)ChangeType;

            for (int i = 0; i < Tiles.Width; i++)
            {
                for (int j = 0; j < Tiles.Height; j++)
                {
                    length += WriteTile(span[length..], ref Tiles[i, j]);
                }
            }

            return length;
        }

        private int ReadTile(Span<byte> span, ref Tile tile)
        {
            ref var header = ref Unsafe.As<byte, ushort>(ref span.At(0));
            ref var header2 = ref span.At(2);
            var length = 3;

            tile.HasRedWire /*      */ = (header & HasRedWireMask) != 0;
            tile.HasActuator /*     */ = (header & HasActuatorMask) != 0;
            tile.IsBlockActuated /* */ = (header & IsBlockActuatedMask) != 0;
            tile.HasBlueWire /*     */ = (header & HasBlueWireMask) != 0;
            tile.HasGreenWire /*    */ = (header & HasGreenWireMask) != 0;
            tile.HasYellowWire /*   */ = (header & HasYellowWireMask) != 0;
            tile.IsBlockFullbright     = (header2 & FullbrightBlockMask) != 0;
            tile.IsWallFullbright /**/ = (header2 & FullbrightWallMask) != 0;
            tile.IsBlockInvisible /**/ = (header2 & InvisibleBlockMask) != 0;
            tile.IsWallInvisible /* */ = (header2 & InvisibleWallMask) != 0;

            if ((header & HasBlockColorMask) != 0)
            {
                tile.BlockColor = (PaintColor)span.At(length++);
            }

            if ((header & HasWallColorMask) != 0)
            {
                tile.WallColor = (PaintColor)span.At(length++);
            }

            if ((header & HasBlockMask) != 0)
            {
                tile.BlockId = Unsafe.ReadUnaligned<BlockId>(ref span.At(length)) + 1;
                length += 2;

                if (tile.BlockId.HasFrames())
                {
                    Unsafe.CopyBlockUnaligned(ref Unsafe.Add(ref tile.AsByte(), 4), ref span.At(length), 4);
                    length += 4;
                }

                if ((header & IsBlockHalvedMask) != 0)
                {
                    tile.BlockShape = BlockShape.Halved;
                }
                else
                {
                    var slope = header & SlopeMask;
                    if (slope > 0)
                    {
                        tile.BlockShape = (BlockShape)((slope >> SlopeShift) + 1);
                    }
                }
            }

            if ((header & HasWallMask) != 0)
            {
                tile.WallId = Unsafe.ReadUnaligned<WallId>(ref span.At(length));
                length += 2;
            }

            if ((header & HasLiquidMask) != 0)
            {
                var amount = span.At(length++);
                var type = (LiquidType)span.At(length++);
                tile.Liquid = new Liquid(type, amount);
            }

            return length;
        }

        private int WriteTile(Span<byte> span, ref Tile tile)
        {
            ref var header = ref Unsafe.As<byte, ushort>(ref span.At(0));
            ref var header2 = ref span.At(2);
            header = 0;
            var length = 3;

            if (tile.HasRedWire) /*      */ header |= HasRedWireMask;
            if (tile.HasActuator) /*     */ header |= HasActuatorMask;
            if (tile.IsBlockActuated) /* */ header |= IsBlockActuatedMask;
            if (tile.HasBlueWire) /*     */ header |= HasBlueWireMask;
            if (tile.HasGreenWire) /*    */ header |= HasGreenWireMask;
            if (tile.HasYellowWire) /*   */ header |= HasYellowWireMask;
            if (tile.IsBlockFullbright)    header2 |= FullbrightBlockMask;
            if (tile.IsWallFullbright)     header2 |= FullbrightWallMask;
            if (tile.IsBlockInvisible)     header2 |= InvisibleBlockMask;
            if (tile.IsWallInvisible) /**/ header2 |= InvisibleWallMask;

            if (tile.BlockColor != PaintColor.None)
            {
                span.At(length++) = (byte)tile.BlockColor;

                header |= HasBlockColorMask;
            }

            if (tile.WallColor != PaintColor.None)
            {
                span.At(length++) = (byte)tile.WallColor;

                header |= HasWallColorMask;
            }

            if (tile.BlockId != BlockId.None)
            {
                Unsafe.WriteUnaligned(ref span.At(length), tile.BlockId - 1);
                length += 2;

                if (tile.BlockId.HasFrames())
                {
                    Unsafe.CopyBlockUnaligned(ref span.At(length), ref Unsafe.Add(ref tile.AsByte(), 4), 4);
                    length += 4;
                }

                if (tile.BlockShape == BlockShape.Halved)
                {
                    header |= IsBlockHalvedMask;
                }
                else
                {
                    if (tile.BlockShape != BlockShape.Normal)
                    {
                        header |= (ushort)((int)(tile.BlockShape - 1) << SlopeShift);
                    }
                }

                header |= HasBlockMask;
            }

            if (tile.WallId != WallId.None)
            {
                Unsafe.WriteUnaligned(ref span.At(length), tile.WallId);
                length += 2;

                header |= HasWallMask;
            }

            if (!tile.Liquid.IsEmpty)
            {
                var liquid = tile.Liquid;
                span.At(length++) = liquid.Amount;
                span.At(length++) = (byte)liquid.Type;

                header |= HasLiquidMask;
            }

            return length;
        }

        /// <summary>
        /// Represents the tile change type (as of 1.4.4.7 only used for telling clients to play specific sounds).
        /// </summary>
        public enum TileChangeType : byte
        {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            None,
            LavaWater,
            HoneyWater,
            HoneyLava,
            ShimmerWater,
            ShimmerLava,
            ShimmerHoney,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        }
    }
}
