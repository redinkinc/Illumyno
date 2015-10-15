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
using System.IO;
using Autodesk.DesignScript.Runtime;

namespace Illumyno
{
    /// <summary>
    /// Runs the Radiance Functions.
    /// </summary>
    public static class Simulation
    {
        /// <summary>
        /// Calculates the LUX of a scene at specified evaluation points (using rtrace and rcalc).
        /// </summary>
        /// <param name="evalPoints">the evaluation points as string</param>
        /// <param name="sceneFile">Path to scene file</param>
        /// <returns>LUX values at given points</returns>
        public static string CalculateLux(string evalPoints, string sceneFile)
        {
            //Run rtrace to calculate irradiance in rgb at given points
            var command = "-I -ab 1 -h -oov " + @sceneFile;
            var irradiance = Program.Execute("rtrace", command, evalPoints);

            //Run rcalc to convert rgb-irradiance to lux
            const string command2 = "-e \"$1=$1;$2=$2;$3=179*(.265*$4+.670*$5+.065*$6)\" ";
            var lux = Program.Execute("rcalc", command2, irradiance);

            return lux;
        }

        /// <summary>
        /// Creates a Radiance Scene from input using oconv.
        /// </summary>
        /// <param name="filepath">name of the output file to be generated</param>
        /// <param name="input">All objects as string</param>
        /// <returns>Path to the generated sceneFile.</returns>
        [IsVisibleInDynamoLibrary(true)]
        public static string CreateSceneFromString(string filepath, string input)
        {
            return CreateSceneFromStrings(filepath, null, null, input);
        }

        /// <summary>
        /// Takes the Sky, Materials and Objects as string to create a Radiance Scene using oconv.
        /// </summary>
        /// <param name="filepath">name of the output file to be generated</param>
        /// <param name="sky">Sky parameters as string</param>
        /// <param name="materials">All materials as string</param>
        /// <param name="objects">All objects as string</param>
        /// <returns>Path to the generated sceneFile.</returns>
        [IsVisibleInDynamoLibrary(true)]
        public static string CreateSceneFromStrings(string filepath, string sky, string materials, string objects)
        {
            var path = Path.GetDirectoryName(filepath) + @"\" + Path.GetFileNameWithoutExtension(filepath);

            var command = "";
            
            if (sky != null)
            {
                var skypath = path + "_sky.rad";
                File.WriteAllText(skypath, sky);
                command += skypath + " ";
            }
            if (materials != null)
            {
                var matpath = path + "_materials.rad";
                File.WriteAllText(matpath, materials);
                command += matpath + " ";
            }
            if (objects != null)
            {
                var objpath = path + "_objects.rad";
                File.WriteAllText(objpath, objects);
                command += objpath + " ";
            }

            const string program = "oconv";
            
            var scene = Program.Execute(program, command);
            if (scene.Length == 0) throw new ArgumentException("empty file");
            File.WriteAllText(filepath, scene);

            return filepath;
        }

        /// <summary>
        /// Creates a Radiance Scene from files using oconv
        /// </summary>
        /// <param name="filepath">name of the output file to be generated</param>
        /// <param name="skyFile">Path to file containing a sky</param>
        /// <param name="materialFile">Path to file containing all materials</param>
        /// <param name="objectFile">Path to file containing all objects</param>
        /// <returns>Path to the generated sceneFile.</returns>
        public static string CreateSceneFromFiles(string filepath, string skyFile = "", string materialFile = "", string objectFile = "")
        {
            const string program = "oconv";
            var command = skyFile + " " + materialFile + " " + objectFile;
            
            var scene = Program.Execute(program, command);
            File.WriteAllText(filepath, scene);

            return filepath;
        }

    }
}
