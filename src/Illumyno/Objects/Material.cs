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
    /// The Radiance Material.
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public class Material : IlluminoObject
    {
        private double Red { get; set; }
        private double Green { get; set; }
        private double Blue { get; set; }
        private double? Spec { get; set; }
        private double? Rough { get; set; }

        /// <summary>
        /// Radiance Material Constructor
        /// </summary>
        /// <param name="modifier">void or another material name</param>
        /// <param name="type">Material Type [plastic, metal, glass, trans, light, source, brightfunc, illum, glow, spotlight, ...]</param>
        /// <param name="name">The name of the material</param>
        /// <param name="red">red value [W/m²/sr]</param>
        /// <param name="green">green value [W/m²/sr]</param>
        /// <param name="blue">blue value [W/m²/sr]</param>
        /// <param name="specularity">for plastic and metal, otherwise null [0..1]</param>
        /// <param name="roughness">for plastic and metal, otherwise null [0..1]</param>
        public Material(string modifier, string type, string name, double red, double green, double blue, double? specularity = null, double? roughness = null)
        {
            Modifier = modifier;
            Type = type;
            Name = name;
            Red = red;
            Green = green;
            Blue = blue;
            Spec = specularity;
            Rough = roughness;
        }

        /// <summary>
        /// Returns the full Radiance String.
        /// </summary>
        /// <returns>Radiance Material</returns>
        public override string Write()
        {
            var value = Modifier + " " + Type + " " + Name + "\r\n" +
                        "0\r\n" +
                        "0\r\n";
            if (Spec != null && Rough != null)
            {
                value += "5 " + Red + " " + Green + " " + Blue + " " + Spec + " " + Rough + "\r\n"; 
            }
            else
            {
                value += "3 " + Red + " " + Green + " " + Blue + "\r\n";
            }
            
            return value;
        }
    }
}
