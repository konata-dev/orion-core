﻿// Copyright (c) 2020 Pryaxis & Orion Contributors
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
using System.Text;
using Orion.Core.DataStructures;

namespace Orion.Core.Packets.Server
{
    /// <summary>
    /// A packet sent from the server to the client to show chat.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct ServerChatPacket : IPacket
    {
        [FieldOffset(8)] private NetworkText? _message;

        /// <summary>
        /// Gets or sets the message color.
        /// </summary>
        /// <value>The message color.</value>
        [field: FieldOffset(0)] public Color3 Color { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        public NetworkText Message
        {
            get => _message ?? NetworkText.Empty;
            set => _message = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets the line width. A value of <c>-1</c> indicates that the screen width should be used.
        /// </summary>
        /// <value>The line width.</value>
        [field: FieldOffset(3)] public short LineWidth { get; set; }

        PacketId IPacket.Id => PacketId.ServerChat;

        /// <inheritdoc/>
        public int Read(Span<byte> span, PacketContext context)
        {
            var index = span.Read(ref this.AsRefByte(0), 3);
            index += span[index..].Read(Encoding.UTF8, out _message);
            return index + span[index..].Read(ref this.AsRefByte(3), 2);
        }

        /// <inheritdoc/>
        public int Write(Span<byte> span, PacketContext context)
        {
            var index = span.Write(ref this.AsRefByte(0), 3);
            index += span[index..].Write(Message, Encoding.UTF8);
            return index + span[index..].Write(ref this.AsRefByte(3), 2);
        }
    }
}
