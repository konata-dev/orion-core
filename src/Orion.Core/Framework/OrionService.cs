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
using Serilog;

namespace Orion.Core.Framework
{
    /// <summary>
    /// Provides the base class for an Orion service.
    /// </summary>
    public abstract class OrionService : OrionExtension
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrionService"/> class with the specified
        /// <paramref name="kernel"/> and <paramref name="log"/>.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        /// <param name="log">The service-specific log to log to.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="kernel"/> or <paramref name="log"/> are <see langword="null"/>.
        /// </exception>
        protected OrionService(OrionKernel kernel, ILogger log) : base(kernel, log) { }
    }
}
