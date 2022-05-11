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

namespace PGENLib;

/// <summary>
/// This abstract class represents a pigment with a function that associates a color with
/// each point on a parametric surface (u,v).
/// </summary>
public class Pigment
{
    /// <summary>
    /// Calculate a color (type color) associated with a (u,v) coordinate.
    /// </summary>
    /// <param name="uv"> 2D vector </param>
    /// <returns></returns>
    public Color GetColor(Vec2d uv)
    {
        Color pigm = new Color();
        return pigm;
    }
}
/// <summary>
/// A uniform pigment.
/// </summary>
public class UniformPigment
{
    public Color UColor = new();
    public Color GetColor(Vec2d uv)
    {
        Color pigm = new Color();
        return pigm;
    }
}
/// <summary>
/// A checkered pigment.
/// The number of rows/columns in the checkered pattern is tunable, but you cannot have a different number of
/// repetitions along the u/v directions.
/// </summary>
public class CheckeredPigment
{
    private readonly Color _col1;
    private readonly Color _col2;
    private int NumStep;
    public CheckeredPigment(Color col1, Color col2, int numStep = 10)
    {
        _col1 = col1;
        _col2 = col2;
        NumStep = numStep;
    }

    public Color getColor(Vec2d uv)
    {
        double intU = Math.Floor(uv.u * NumStep); 
        double intV = Math.Floor(uv.u * NumStep);
        if (Math.Abs((intU % 2) - (intV % 2)) < 1E-5)
        {
            return _col1;
        }
        else
        {
            return _col2;
        }
    }
}

class ImagePigment
{
    private HdrImage _image;
    public ImagePigment(HdrImage image)
    {
        _image = image;
    }

    public Color getColor(Vec2d uv)
    {
        int col = (int)uv.u * _image.Width;
        int row = (int)uv.v * _image.Height;
        if (col >= _image.Width)
        {
            col = _image.Width - 1;
        }

        if (row >= _image.Height)
        {
            row = _image.Height - 1;
        }

        return _image.GetPixel(col, row);
    }
}










