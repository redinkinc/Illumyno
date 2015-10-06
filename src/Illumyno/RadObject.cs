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

namespace Illumyno
{
    /// <summary>
    /// Radiance Objects.
    /// </summary>
    public static class RadObject
    {
        /// <summary>
        /// Creates a Radiance Polygon.
        /// </summary>
        /// <param name="material">Material of the Polygon</param>
        /// <param name="name">Name of the Polygon</param>
        /// <param name="points">List of Points that define the Polygon (need to be planar).</param>
        /// <returns></returns>
        public static Objects.Polygon Polygon(string material, string name, List<Point> points )
        {
            //Check if points are planar
            var pcurve = PolyCurve.ByPoints(points);
            if (!pcurve.IsPlanar)
            {
                throw new ArgumentException("Points do not define a planar polygon!");
            }

            //Create new Polygon.
            return new Objects.Polygon(material, name, points);
        }

        /// <summary>
        /// Creates a Radiance Material.
        /// </summary>
        /// <param name="modifier">"void" or another material name</param>
        /// <param name="type">Material Type [plastic, metal, glass, trans, light, source, brightfunc, illum, glow, spotlight, ...]</param>
        /// <param name="name">The name of the material</param>
        /// <param name="red">red value [W/m²/sr]</param>
        /// <param name="green">green value [W/m²/sr]</param>
        /// <param name="blue">blue value [W/m²/sr]</param>
        /// <param name="specularity">for plastic and metal, otherwise null [0..1]</param>
        /// <param name="roughness">for plastic and metal, otherwise null [0..1]</param>
        /// <returns></returns>
        public static Objects.Material Material(string modifier, string type, string name, double red, double green, double blue, double? specularity = null, double? roughness = null)
        {
            return new Objects.Material(modifier, type, name, red, green, blue, specularity, roughness);
        }

        /// <summary>
        /// Creates a Radiance Sphere.
        /// </summary>
        /// <param name="material">Material of the sphere</param>
        /// <param name="name">Name of the sphere</param>
        /// <param name="centerPoint">Centerpoint of the sphere</param>
        /// <param name="radius">Radius of the Sphere</param>
        /// <returns></returns>
        public static Objects.Sphere Sphere(string material, string name, Point centerPoint, double radius)
        {
            return new Objects.Sphere(material, name, centerPoint, radius);
        }
    }
}
