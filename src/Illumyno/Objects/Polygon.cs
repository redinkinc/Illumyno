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

using System;
using System.Collections.Generic;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

namespace Illumyno.Objects
{
    /// <summary>
    /// A radiance polygon object
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public class Polygon : IlluminoObject
    {
        private List<Point> Points { get; }

        /// <summary>
        /// Radiance Polygon Constructor
        /// </summary>
        /// <param name="material">material of the polygon</param>
        /// <param name="name">name of the polygon</param>
        /// <param name="points">List of points that define the polygon (need to be planar)</param>
        public Polygon(string material, string name, List<Point> points)
        {
            Modifier = material;
            Type = "polygon";
            Name = name;
            Points = points;
        }

        /// <summary>
        /// Returns the full Radiance String.
        /// </summary>
        /// <returns>Radiance Polygon</returns>
        //[IsVisibleInDynamoLibrary(true)]
        public override string Write()
        {
            var value = Modifier + " " + Type + " " + Name + "\r\n" +
                        "0\r\n" +
                        "0\r\n" +
                        (Points.Count * 3);
            foreach (var point in Points)
            {
                value += " " + Math.Round(point.X, 2) + " " + Math.Round(point.Y, 2) + " " + Math.Round(point.Z, 2) + "\r\n";
            }

            return value;
        }
        
    }
}
