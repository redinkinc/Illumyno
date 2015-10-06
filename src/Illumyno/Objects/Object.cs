/*
 *  This file is part of Illumyno.
 *  
 *  Copyright (c) 2014-2015 Technische Universitaet Muenchen, 
 *  Chair of Computational Modeling and Simulation (https://www.cms.bgu.tum.de/)
 *  LEONHARD OBERMEYER CENTER (https://www.loc.tum.de)
 *  
 *  Illumyno by Fabian Ritter (mailto:mail@redinkinc.de)
 * 
 *  Illumyno is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *  
 *  Illumyno is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *  
 *  You should have received a copy of the GNU General Public License
 *  along with Illumyno. If not, see <http://www.gnu.org/licenses/>.
 */

using Autodesk.DesignScript.Runtime;

namespace Illumyno.Objects
{
    /// <summary>
    /// 
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public abstract class IlluminoObject
    {
        protected string Modifier;
        protected string Type;
        protected string Name;

        /// <summary>
        /// Returns the full Radiance String.
        /// </summary>
        /// <returns>String for Radiance</returns>
        public abstract string Write();

        /// <summary>
        /// Returns the name of the object.
        /// </summary>
        /// <returns>Name</returns>
        public override string ToString()
        {
            return Name;
        }

    }
}
