/*
PhotoGENius : photorealistic images generation.
Copyright (C) 2022  Lamorte Teresa, Salteri Francesca, Zanetti Martino

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */


namespace PGENLib
{
    public class InvalidPfmFileFormat : Exception
    {
        public InvalidPfmFileFormat(string errMessage)
        {
            Console.WriteLine(errMessage);
        }
    }
    public class RuntimeError : Exception
    {
        public RuntimeError(string errMessage)
        {
            Console.WriteLine(errMessage);
        }
    }

    /// <summary>
    /// An error found by the lexer/parser while reading a scene file.
    /// The fields of this type are the following:
    /// <list type="table">
    /// <item>
    ///     <term>Message</term>
    ///     <description> a user-frendly error message</description>
    /// </item>
    /// <item>
    ///     <term>sourceLocation</term>
    ///     <description> a sourceLocation object pointing out the name of the file, the line number and the column
    ///     number where the error occured</description>
    /// </item>
    /// </list>
    /// </summary>
    class GrammarErrorException : Exception
    {
        public SourceLocation SourceLocation;

        public GrammarErrorException() : base()
        {
            SourceLocation = new SourceLocation();
        }

        public GrammarErrorException(string msg, SourceLocation sourceLocation) : base(msg)
        {
            SourceLocation = sourceLocation;
        }
    }
}