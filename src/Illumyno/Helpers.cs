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
using System.Linq;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using Dynamo;
using Illumyno.Objects;
using Math = System.Math;

namespace Illumyno
{
    /// <summary>
    /// Modifiers for preparing Dynamo RadObject for radiance.
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Adds Surfaces on a given set of surfaces that represent windows in a size specified through the glazing percentage factor
        /// </summary>
        /// <param name="surfaces">Set of surfaces to place the windows on</param>
        /// <param name="percentage">Glazing percentage [0.05 - 0.95]</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static List<Surface> ByGlazingPercentage(List<Surface> surfaces, double percentage = 0.5)
        {
            // check for the right percentage value and throw an exeption if fails
            if (percentage < 0.05 || percentage > 0.95)
            {
                throw new ArgumentException("The glazing percentage has to be between 0.05 and 0.95");
            }

            var fenestrationsurfaceList = new List<Surface>();

            // iterate over the surfaces
            foreach (var surface in surfaces)
            {
                //surface.Tessellate(null, null);

                //var edge1 = surface.Edges.First().CurveGeometry.Length;
                //var edge2 = surface.Edges.Last();

                var vec1 = Vector.ByTwoPoints(surface.Vertices.First().PointGeometry, surface.Vertices[1].PointGeometry);
                var vec2 = Vector.ByTwoPoints(surface.Vertices[0].PointGeometry, surface.Vertices[3].PointGeometry);
                //calculate the distance from the border (offset) from the given surface
                var length = vec1.Length;
                var height = vec2.Length;
                var dist = ((height + length) - Math.Sqrt(Math.Pow(height, 2) + Math.Pow(length, 2) + (4 * percentage - 2) * height * length)) / 4;

                //calculate new points for the FenestrationSurface
                var points = new List<Point>();
                // var coordSystem = surface.CoordinateSystemAtParameter(0.0, 0.0); // may have Problems of orientation ?!?
                var coordSystem = CoordinateSystem.ByOriginVectors(surface.Vertices[0].PointGeometry, vec1, vec2);
                points.Add(Point.ByCartesianCoordinates(coordSystem, dist, dist));
                points.Add(Point.ByCartesianCoordinates(coordSystem, length - dist, dist));
                points.Add(Point.ByCartesianCoordinates(coordSystem, length - dist, height - dist));
                points.Add(Point.ByCartesianCoordinates(coordSystem, dist, height - dist));

                var fenestrationSurface = Surface.ByPerimeterPoints(points);
                //add to List
                fenestrationsurfaceList.Add(fenestrationSurface);
            }
            //return List
            return fenestrationsurfaceList;
        }

        /// <summary>
        /// Given a surface and the windows placed on it, the windows are cut out of the surface to generate a wall with holes.
        /// </summary>
        /// <param name="surface">Surface that contains the windows</param>
        /// <param name="windows">List of surfaces that represent the windows</param>
        [IsVisibleInDynamoLibrary(false)] //does not work as intended!
        public static Surface CutWindows(Surface surface, List<Surface> windows)
        {
            foreach (var window in windows)
            {
                var sub = window.Thicken(5000.0, true);
                surface.SubtractFrom(sub);
            }

            return surface;
        }

        /// <summary>
        /// Tesselates a Dynamo geometry and returns the vertices as Point lists. Needed to generate Radiance geometry from freeform or not closed geometry.
        /// </summary>
        /// <param name="geometry">the dynamo geometry</param>
        /// <param name="tolerance">tesselation tolerance</param>
        /// <returns></returns>
        public static List<List<Point>> TesselateSurface(Geometry geometry, double tolerance)
        {
            var triangleList = new List<List<Point>>();

            var t = new DefaultRenderPackageFactory
            {
                TessellationParameters =
                {
                    MaxTessellationDivisions = 500,
                    Tolerance = tolerance
                }
            };
            var rp = t.CreateRenderPackage();
            geometry.Tessellate(rp, t.TessellationParameters);

            var vertexArray = rp.MeshVertices.ToArray();
            for (var i = 0; i < vertexArray.Length; i += 9)
            {
                var trianglePoints = new List<Point>
                {
                    Point.ByCoordinates(vertexArray[i],
                        vertexArray[i + 1], vertexArray[i + 2]),
                    Point.ByCoordinates(vertexArray[i + 3], vertexArray[i + 4],
                        vertexArray[i + 5]),
                    Point.ByCoordinates(vertexArray[i + 6], vertexArray[i + 7],
                        vertexArray[i + 8])
                };
                
                triangleList.Add(trianglePoints);
            }

            return triangleList;
        }

        /// <summary>
        /// Creates an evaluation pointlist.
        /// </summary>
        /// <param name="points">Points to evaluate</param>
        /// <param name="direction">direction to evaluate (normaly z-direction)</param>
        /// <returns>pointList</returns>
        public static string EvaluationPointsList(List<Point> points, Vector direction)
        {
            var n = points.Count - 1;
            var pointList = "";
            for (var i = 0; i < n ; i++)
            {
                pointList += Math.Round(points[i].X, 2) + " " + Math.Round(points[i].Y, 2) + " " + Math.Round(points[i].Z, 2) + " " + Math.Round(direction.X) + " " + Math.Round(direction.Y) + " " + Math.Round(direction.Z)+"\n";
            }
            pointList += Math.Round(points[n].X, 2) + " " + Math.Round(points[n].Y, 2) + " " + Math.Round(points[n].Z, 2) + " " + Math.Round(direction.X) + " " + Math.Round(direction.Y) + " " + Math.Round(direction.Z);

            return pointList;
        }

        /// <summary>
        /// Convert Illumino Objects to string.
        /// </summary>
        /// <param name="illumynoObjects">List of Objects from Illumino to write to a string.</param>
        /// <returns>string that can be used by radiance or be written to a *.rad file</returns>
        public static string ObjectsToString(List<IlluminoObject> illumynoObjects)
        {
            string result = "";
            foreach (var illumynoObject in illumynoObjects)
                result = result + illumynoObject.Write() + "\r\n";
            return result;
        }
    }
}
