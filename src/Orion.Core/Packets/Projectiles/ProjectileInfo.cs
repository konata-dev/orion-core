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
using System.Xml.Schema;
using Orion.Core.Packets.DataStructures;
using Orion.Core.Utils;

namespace Orion.Core.Packets.Projectiles
{
    /// <summary>
    /// A packet sent to update projectile information.
    /// </summary>
    public struct ProjectileInfo : IPacket
    {
        private const int MaxAi = 3;

        private Flags8 _flags;
        private Flags8 _flags2;
        private float[]? _ai;

        /// <summary>
        /// Gets or sets the identity, i.e, the projectile index.
        /// </summary>
        public short Identity { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public Vector2f Position { get; set; }

        /// <summary>
        /// Gets or sets the velocity.
        /// </summary>
        public Vector2f Velocity { get; set; }

        /// <summary>
        /// Gets or sets the owner index.
        /// </summary>
        public byte OwnerIndex { get; set; }

        /// <summary>
        /// Gets or sets the projectile type.
        /// </summary>
        public short Type { get; set; }

        /// <summary>
        /// Gets or sets additional information.
        /// </summary>
        public float[] AdditionalInformation { get => _ai ??= new float[MaxAi]; set => _ai = value; }

        /// <summary>
        /// Gets or sets the damage.
        /// </summary>
        public short Damage { get; set; }

        /// <summary>
        /// Gets or sets the knockback.
        /// </summary>
        public float Knockback { get; set; }

        /// <summary>
        /// Gets or sets the original damage.
        /// </summary>
        public short OriginalDamage { get; set; }

        /// <summary>
        /// Gets or sets the UUID.
        /// </summary>
        public short Uuid { get; set; }

        /// <summary>
        /// Gets or sets the banner ID.
        /// </summary>
        public ushort BannerId { get; set; }

        PacketId IPacket.Id => PacketId.ProjectileInfo;

        int IPacket.ReadBody(Span<byte> span, PacketContext context)
        {
            var length = 22;
            Identity = span.Read<short>();
            Position = span.Read<Vector2f>();
            Velocity = span.Read<Vector2f>();
            OwnerIndex = span.Read<byte>();
            Type = span.Read<short>();
            _flags = span.Read<Flags8>();

            if (_flags[2])
                _flags2 = span.Read<Flags8>();

            if (_flags[0]) // replaced the for loop with this because the 3rd ai value is wrote at the end of the packet
            {
                AdditionalInformation[0] = span.Read<float>();
                length += 4;
            }

            if (_flags[1])
            {
                AdditionalInformation[1] = span.Read<float>();
                length += 4;
            }

            if (_flags[3])
            {
                BannerId = span.Read<ushort>();
                length += 2;
            }

            if (_flags[4])
            {
                Damage = span.Read<short>();
                length += 2;
            }

            if (_flags[5])
            {
                Knockback = span.Read<float>();
                length += 4;
            }

            if (_flags[6])
            {
                OriginalDamage = span.Read<short>();
                length += 2;
            }

            if (_flags[7])
            {
                Uuid = span.Read<short>();
                length += 2;
            }

            if (_flags2[0])
            {
                AdditionalInformation[2] = span.Read<float>();
                length += 4;
            }

            return length;
        }

        int IPacket.WriteBody(Span<byte> span, PacketContext context)
        {
            var length = span.Write(Identity);
            length += span[length..].Write(Position);
            length += span[length..].Write(Velocity);
            length += span[length..].Write(OwnerIndex);
            length += span[length..].Write(Type);

            var flagsOffset = length++;
            var flags2Offset = AdditionalInformation[2] == 0 ? -1 : length++; // shit solution but im making the bad assumption there will be no more uses of the flag

            if (flags2Offset != -1)
                _flags[2] = true;

            if (AdditionalInformation[0] != 0)
            {
                _flags[0] = true;
                length += span[length..].Write(AdditionalInformation[0]);
            }

            if (AdditionalInformation[1] != 0)
            {
                _flags[1] = true;
                length += span[length..].Write(AdditionalInformation[1]);
            }

            if (BannerId > 0)
            {
                _flags[3] = true;
                length += span[length..].Write(BannerId);
            }

            if (Damage > 0)
            {
                _flags[4] = true;
                length += span[length..].Write(Damage);
            }

            if (Knockback > 0)
            {
                _flags[5] = true;
                length += span[length..].Write(Knockback);
            }

            if (OriginalDamage > 0)
            {
                _flags[6] = true;
                length += span[length..].Write(OriginalDamage);
            }

            if (Uuid > 0)
            {
                _flags[7] = true;
                length += span[length..].Write(Uuid);
            }

            if (AdditionalInformation[2] != 0)
            {
                _flags2[0] = true;
                length += span[length..].Write(AdditionalInformation[2]);
            }

            span[flagsOffset] = Unsafe.As<Flags8, byte>(ref _flags);

            if (_flags[2])
                span[flags2Offset] = Unsafe.As<Flags8, byte>(ref _flags2);

            return length;
        }
    }
}
