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
   /// <summary>
   /// A specific position in a source file.
   /// </summary>
   public class SourceLocation
   {
      public string FileName;
      public int LineNum;
      public int ColNum;
      public SourceLocation(string fileName, int lineNum = 0, int colNum= 0)
      {
          FileName = fileName;
          LineNum = lineNum;
          ColNum = colNum;
      }
      public SourceLocation(int lineNum = 0, int colNum= 0)
      {
          FileName = new string("");
          LineNum = lineNum;
          ColNum = colNum;
      }
   }
   //A lexical token, used when parsing a scene file
   public class Token
   {
       public SourceLocation Location;

       public Token(SourceLocation location)
       {
           Location = location;
       }
   }

   public class StopToken : Token
   {
       public StopToken(SourceLocation location) : base(location) {}
   }

   public enum Keyword
   {
       New = 1,
       Material = 2,
       Plane = 3,
       Sphere = 4,
       Diffuse = 5,
       Specular = 6,
       Uniform = 7,
       Checkered = 8,
       Image = 9,
       Identity = 10,
       Translation = 11,
       RotationX = 12,
       RotationY = 13,
       RotationZ = 14,
       Scaling = 15,
       Camera = 16,
       Orthogonal =17,
       Perspective = 18,
       Float = 19
   }
   
   
   
   
   }