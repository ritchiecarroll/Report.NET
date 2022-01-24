using System;

// Creation date: 30.07.2002
// Checked: xx.07.2002
// Author: Otto Mayer (mot@root.ch)
// Version: 1.01 

// Report.NET copyright 2002-2004 root-software ag, Bürglen Switzerland - O. Mayer, S. Spirig, R. Gartenmann, all rights reserved
// This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation, version 2.1 of the License.
// This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. You
// should have received a copy of the GNU Lesser General Public License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA www.opensource.org/licenses/lgpl-license.html

namespace Root.Reports {
  /// <summary>The MatrixDOrder enumeration specifies the order of multiplication when a new matrix is multiplied by an existing matrix.</summary>
  public enum MatrixDOrder {
    /// <summary>Specifies that the new matrix is on the left and the existing matrix is on the right.</summary>
    Prepend,
    /// <summary>Specifies that the existing matrix is on the left and the new matrix is on the right.</summary>
    Append
  }

  //****************************************************************************************************
  /// <summary>Matrix Object for Double Values.</summary>
  public class MatrixD {
    /// <summary>Scaling X</summary>
    private Double _rSX = 1;

    /// <summary>Shearing Y</summary>
    private Double _rRY = 0;

    /// <summary>Shearing X</summary>
    private Double _rRX = 0;

    /// <summary>Scaling Y</summary>
    private Double _rSY = 1;

    /// <summary>Horizontal position of the report object relative to its container.</summary>
    private Double _rDX;

    /// <summary>Vertical position of the report object relative to its container.</summary>
    private Double _rDY;

    //----------------------------------------------------------------------------------------------------x
    /// <summary></summary>
    /// <param name="rSX"></param>
    /// <param name="rRY"></param>
    /// <param name="rRX"></param>
    /// <param name="rSY"></param>
    /// <param name="rDX"></param>
    /// <param name="rDY"></param>
    internal MatrixD(Double rSX, Double rRY, Double rRX, Double rSY, Double rDX, Double rDY) {
      _rSX = rSX;
      _rRY = rRY;
      _rRX = rRX;
      _rSY = rSY;
      _rDX = rDX;
      _rDY = rDY;
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary></summary>
    internal Boolean bComplex {
      get { return _rSX != 1 || _rRY != 0 || _rRX != 0 || _rSY != 1; }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets or sets the horizontal position of this report object relative to its container.</summary>
    /// <value>Gets or sets the horizontal position of this report object relative to its container.</value>
    public Double rDX {
      set { _rDX = value; }
      get { return _rDX; }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Vertical position of this report object relative to its container.</summary>
    public Double rDY {
      set { _rDY = value; }
      get { return _rDY; }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Shearing X</summary>
    public Double rRX {
      get { return _rRX; }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Shearing Y</summary>
    public Double rRY {
      get { return _rRY; }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Scaling X</summary>
    public Double rSX {
      get { return _rSX; }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Scaling Y</summary>
    public Double rSY {
      get { return _rSY; }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates an exact copy of this Matrix object.</summary>
    /// <returns>Copy of this matrix</returns>
    public MatrixD Clone() {
      return new MatrixD(_rSX, _rRY, _rRX, _rSY, _rDX, _rDY);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Multiplies this matrix with the specified matrix values.</summary>
    /// <param name="rSXp"></param>
    /// <param name="rRYp"></param>
    /// <param name="rRXp"></param>
    /// <param name="rSYp"></param>
    /// <param name="rDXp"></param>
    /// <param name="rDYp"></param>
    /// <param name="matrixDOrder"></param>
    internal void Multiply(Double rSXp, Double rRYp, Double rRXp, Double rSYp, Double rDXp, Double rDYp, MatrixDOrder matrixDOrder) {
      Double rSXn, rRYn, rRXn, rSYn, rDXn, rDYn;
      if (matrixDOrder == MatrixDOrder.Prepend) {
        rSXn = rSXp * _rSX + rRYp * _rRX;
        rRYn = rSXp * _rRY + rRYp * _rSY;
        rRXn = rRXp * _rSX + rSYp * _rRX;
        rSYn = rRXp * _rRY + rSYp * _rSY;
        rDXn = rDXp * _rSX + rDYp * _rRX + _rDX ;
        rDYn = rDXp * _rRY + rDYp * _rSY + _rDY ;
      }
      else {
        //System.Drawing.Drawing2D.Matrix m1 = new System.Drawing.Drawing2D.Matrix((Single)_rSX, (Single)_rRY, (Single)_rRX, (Single)_rSY, (Single)_rDX, (Single)_rDY);
        //System.Drawing.Drawing2D.Matrix ma1 = new System.Drawing.Drawing2D.Matrix((Single)m._rSX, (Single)m._rRY, (Single)m._rRX, (Single)m._rSY, (Single)m._rDX, (Single)m._rDY);
        //m1.Multiply(ma1, System.Drawing.Drawing2D.MatrixOrder.Append);  // es wird Append ausgeführt !!!
        rSXn = _rSX * rSXp + _rRY * rRXp;
        rRYn = _rSX * rRYp + _rRY * rSYp;
        rRXn = _rRX * rSXp + _rSY * rRXp;
        rSYn = _rRX * rRYp + _rSY * rSYp;
        rDXn = _rDX * rSXp + _rDY * rRXp + rDXp ;
        rDYn = _rDX * rRYp + _rDY * rSYp + rDYp ;
      }
      _rSX = rSXn;
      _rRY = rRYn;
      _rRX = rRXn;
      _rSY = rSYn;
      _rDX = rDXn;
      _rDY = rDYn;
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Multiplies this matrix with the specified matrix values.</summary>
    /// <param name="rSXp"></param>
    /// <param name="rRYp"></param>
    /// <param name="rRXp"></param>
    /// <param name="rSYp"></param>
    /// <param name="rDXp"></param>
    /// <param name="rDYp"></param>
    internal void Multiply(Double rSXp, Double rRYp, Double rRXp, Double rSYp, Double rDXp, Double rDYp) {
      Multiply(rSXp, rRYp, rRXp, rSYp, rDXp, rDYp, MatrixDOrder.Prepend);
    }
    
    //----------------------------------------------------------------------------------------------------x
    /// <summary>Multiplies this matrix with the specified matrix values.</summary>
    /// <param name="m"></param>
    /// <param name="matrixDOrder"></param>
    internal void Multiply(MatrixD m, MatrixDOrder matrixDOrder) {
      Multiply(m._rSX, m._rRY, m._rRX, m._rSY, m._rDX, m._rDY, matrixDOrder);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary></summary>
    /// <param name="m"></param>
    internal void Multiply(MatrixD m) {
      Multiply(m, MatrixDOrder.Prepend);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Applies the specified rotation to the transformation matrix of this report object.</summary>
    /// <param name="rAngle">Angle of rotation in degrees</param>
    internal void Rotate(Double rAngle) {
      //System.Drawing.Drawing2D.Matrix m1 = new System.Drawing.Drawing2D.Matrix((Single)_rSX, (Single)_rRY, (Single)_rRX, (Single)_rSY, (Single)_rDX, (Single)_rDY);
      //m1.Rotate((float)rAngle);

      Multiply(new MatrixD(Math.Cos(rAngle / 180 * Math.PI), Math.Sin(rAngle / 180 * Math.PI),  -Math.Sin(rAngle / 180 * Math.PI), Math.Cos(rAngle / 180 * Math.PI), 0, 0));
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary></summary>
    /// <param name="rX"></param>
    /// <param name="rY"></param>
    /// <returns></returns>
    internal Double rTransformX(Double rX, Double rY) {
      return rX * _rSX + rY * _rRX + _rDX;
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary></summary>
    /// <param name="rX"></param>
    /// <param name="rY"></param>
    /// <returns></returns>
    internal Double rTransformY(Double rX, Double rY) {
      return rX * _rRY + rY * _rSY + _rDY;
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Scales this report object.</summary>
    /// <param name="rScaleX">The value by which to scale this Matrix in the x-axis direction.</param>
    /// <param name="rScaleY">The value by which to scale this Matrix in the y-axis direction.</param>
    internal void Scale(Double rScaleX, Double rScaleY) {
      Multiply(rScaleX, 0, 0, rScaleY, 0, 0, MatrixDOrder.Prepend);
    }

  }
}
