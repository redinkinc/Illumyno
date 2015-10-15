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
using System.Diagnostics;
using System.IO;
using Autodesk.DesignScript.Runtime;

namespace Illumyno
{
    /// <summary>
    /// Process Handling.
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public static class Program
    {
        /// <summary>
        /// Runs a process with a command.
        /// </summary>
        /// <param name="program">Programm to execute</param>
        /// <param name="command">additional commands and options</param>
        /// <returns></returns>
        public static string Execute(string program, string command)
        {
            var proc = new Process()
            {
                StartInfo = new ProcessStartInfo(program, command)
                {
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }
            };
            proc.Start();

            proc.StandardInput.Flush();
            proc.StandardInput.Close();

            var result = proc.StandardOutput.ReadToEnd();

            proc.WaitForExit();
            proc.Close();

            return result;
        }

        /// <summary>
        /// Runs a process with command and stdInput
        /// </summary>
        /// <param name="program">Programm to execute</param>
        /// <param name="command">additional commands and options</param>
        /// <param name="input">string written to stadard input</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public static string Execute(string program, string command, string input)
        {
            var proc = new Process()
            {
                StartInfo = new ProcessStartInfo(program, command)
                {
                    //CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                }
            };
            proc.Start();

            //StreamWriter utf8Writer = new StreamWriter(proc.StandardInput.BaseStream, Encoding.ASCII);
            //utf8Writer.Write(input);
            //utf8Writer.Write((char) 26); // EOF
            //utf8Writer.Close();

            proc.StandardInput.WriteLine(input);
            proc.StandardInput.WriteLine((char)26); // EOF
            proc.StandardInput.Flush();
            proc.StandardInput.Close();

            var e = proc.StandardError.ReadToEnd();
            if (e != "")
            {
                throw new ArgumentException(e);
            }

            var result = proc.StandardOutput.ReadToEnd();

            proc.WaitForExit();
            proc.Close();

            return result;
        }

    }
}
