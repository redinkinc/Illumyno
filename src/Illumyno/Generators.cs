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

namespace Illumyno
{
    /// <summary>
    /// The Radiance Generators.
    /// </summary>
    public static class Generators
    {
        /// <summary>
        /// Runs radiance genbox.
        /// </summary>
        /// <param name="material">material of the box</param>
        /// <param name="name">name of the box</param>
        /// <param name="length">length of the box</param>
        /// <param name="width">width of the box</param>
        /// <param name="height">height of the box</param>
        /// <returns>6 radiance polygons as a string</returns>
        public static string GenerateBox(string material, string name, double length, double width, double height)
        {
            var command = material + " " + name + " " + length + " " + width + " " + height;
            var result = Program.Execute("genbox", command);

            return result;
        }

        /// <summary>
        /// Calls Radiance's gensky with option "-defaults".
        /// </summary>
        /// <returns>gensky defaults</returns>
        public static string SkyByDefault()
        {
            var command = "-defaults";
            var result = Program.Execute("gensky", command);

            return result;
        }

        /// <summary>
        /// Calls Radiance's gensky at specified time with options.
        /// </summary>
        /// <param name="dateTime">A DateTime</param>
        /// <param name="options">gensky options</param>
        /// <returns>A specified Radiance Sky</returns>
        public static string SkyAtDateTime(DateTime dateTime, string options = "")
        {
            var command = dateTime.Month + " " + dateTime.Day + " " + dateTime.Hour +":"+dateTime.Minute + " " + options;
            var result = Program.Execute("gensky", command);

            return result;

        }

        /// <summary>
        /// Calls Radiance's gensky with sun at specified altitude and azimuth time with options.
        /// </summary>
        /// <param name="altitude">altitude of the sun</param>
        /// <param name="azimuth">azimuth of the sun</param>
        /// <param name="options">gensky options</param>
        /// <returns>A specified Radiance Sky</returns>
        public static string SkyFromSunPosition(double altitude, double azimuth, string options = "")
        {
            var command = altitude + " " + azimuth + " " + options;
            var result = Program.Execute("gensky", command);

            return result;
        }
    }
}
