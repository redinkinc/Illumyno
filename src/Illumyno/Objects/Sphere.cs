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
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

namespace Illumyno.Objects
{
    /// <summary>
    /// A Radiance Sphere
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public class Sphere : IlluminoObject
    {
        private Point CenterPoint { get; set; }
        private double Radius { get; set; }

        /// <summary>
        /// Creates a Radiance Sphere
        /// </summary>
        /// <param name="material">Material of the sphere</param>
        /// <param name="name">Name of the sphere</param>
        /// <param name="centerPoint">Centerpoint of the sphere</param>
        /// <param name="radius">Radius of the Sphere</param>
        public Sphere(string material, string name, Point centerPoint, double radius)
        {
            Modifier = material;
            Type = "sphere";
            Name = name;
            CenterPoint = centerPoint;
            Radius = radius;
        }

        /// <summary>
        /// Returns the full Radiance String.
        /// </summary>
        /// <returns>Radiance Sphere</returns>
        public override string Write()
        {
            var value = Modifier + " " + Type + " " + Name + "\r\n" +
                        "0\r\n" +
                        "0\r\n" +
                        "4 " + Math.Round(CenterPoint.X, 2) + " " + Math.Round(CenterPoint.Y, 2) + " " + Math.Round(CenterPoint.Z, 2) + "\r\n";
            return value;
        }
    }
}
