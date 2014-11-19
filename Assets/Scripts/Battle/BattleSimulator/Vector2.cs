/*
 * RVO Library / RVO2 Library
 * 
 * Copyright © 2008-10 University of North Carolina at Chapel Hill. All rights 
 * reserved.
 * 
 * Permission to use, copy, modify, and distribute this software and its 
 * documentation for educational, research, and non-profit purposes, without 
 * fee, and without a written agreement is hereby granted, provided that the 
 * above copyright notice, this paragraph, and the following four paragraphs 
 * appear in all copies.
 * 
 * Permission to incorporate this software into commercial products may be 
 * obtained by contacting the University of North Carolina at Chapel Hill.
 * 
 * This software program and documentation are copyrighted by the University of 
 * North Carolina at Chapel Hill. The software program and documentation are 
 * supplied "as is", without any accompanying services from the University of 
 * North Carolina at Chapel Hill or the authors. The University of North 
 * Carolina at Chapel Hill and the authors do not warrant that the operation of 
 * the program will be uninterrupted or error-free. The end-user understands 
 * that the program was developed for research purposes and is advised not to 
 * rely exclusively on the program for any reason.
 * 
 * IN NO EVENT SHALL THE UNIVERSITY OF NORTH CAROLINA AT CHAPEL HILL OR ITS 
 * EMPLOYEES OR THE AUTHORS BE LIABLE TO ANY PARTY FOR DIRECT, INDIRECT, 
 * SPECIAL, INCIDENTAL, OR CONSEQUENTIAL DAMAGES, INCLUDING LOST PROFITS, 
 * ARISING OUT OF THE USE OF THIS SOFTWARE AND ITS DOCUMENTATION, EVEN IF THE 
 * UNIVERSITY OF NORTH CAROLINA AT CHAPEL HILL OR THE AUTHORS HAVE BEEN ADVISED 
 * OF THE POSSIBILITY OF SUCH DAMAGE.
 * 
 * THE UNIVERSITY OF NORTH CAROLINA AT CHAPEL HILL AND THE AUTHORS SPECIFICALLY 
 * DISCLAIM ANY WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE AND ANY 
 * STATUTORY WARRANTY OF NON-INFRINGEMENT. THE SOFTWARE PROVIDED HEREUNDER IS ON 
 * AN "AS IS" BASIS, AND THE UNIVERSITY OF NORTH CAROLINA AT CHAPEL HILL AND THE 
 * AUTHORS HAVE NO OBLIGATIONS TO PROVIDE MAINTENANCE, SUPPORT, UPDATES, 
 * ENHANCEMENTS, OR MODIFICATIONS.
 */
using System;
using System.Globalization;

namespace Battle
{
    public struct Vector2
    {
        public static Vector2 zero = new Vector2(0, 0);
        internal float x_;
        internal float y_;

        public float x()
        {
            return x_;
        }

        public float y()
        {
            return y_;
        }

        //
        // Static Properties
        //
        public static Vector2 one
        {
            get
            {
                return new Vector2(1f, 1f);
            }
        }
        
        public static Vector2 right
        {
            get
            {
                return new Vector2(1f, 0f);
            }
        }
        
        public static Vector2 up
        {
            get
            {
                return new Vector2(0f, 1f);
            }
        }

        public override string ToString()
        {
            return "(" + x_.ToString(new CultureInfo("").NumberFormat) + "," + y_.ToString(new CultureInfo("").NumberFormat) + ")";
        }

        public Vector2(float x, float y)
        {
            x_ = x;
            y_ = y;
        }

        public static Vector2 operator +(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.x_ + rhs.x_, lhs.y_ + rhs.y_);
        }

        public static Vector2 operator -(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.x_ - rhs.x_, lhs.y_ - rhs.y_);
        }

        public static float operator *(Vector2 lhs, Vector2 rhs)
        {
            return lhs.x_ * rhs.x_ + lhs.y_ * rhs.y_;
        }

        public static Vector2 operator *(float k, Vector2 u)
        {
            return u * k;
        }

        public static Vector2 operator *(Vector2 u, float k)
        {
            return new Vector2(u.x_ * k, u.y_ * k);
        }

        public static Vector2 operator /(Vector2 u, float k)
        {
            return new Vector2(u.x_ / k, u.y_ / k);
        }

        public static Vector2 operator -(Vector2 v)
        {
            return new Vector2(-v.x_, -v.y_);
        }

        public static bool IsZero(Vector2 value)
        {
            return value.x_ == 0 && value.y_ == 0;
        }

        public static float Distance(Vector2 o, Vector2 t)
        {
            return (float)Math.Sqrt((o.x_ - t.x_) * (o.x_ - t.x_) + (o.y_ - t.y_) * (o.y_ - t.y_));
        }

        public static float Magnitude(Vector2 v)
        {
            return (float)Math.Sqrt(v.x_ * v.x_ + v.y_ * v.y_);
        }
        
        public static Vector2 Normalize(Vector2 value)
        {
            float num = Vector2.Magnitude(value);
            if (num > 1E-05f)
            {
                return value / num;
            }
            return Vector2.zero;
        }

        public Vector2 Normalize()
        {
            float num = Vector2.Magnitude(this);
            if (num > 1E-05f)
            {
                return this / num;
            }
            return Vector2.zero;
        }

        public static Vector2 LerpTo(Vector2 from, Vector2 to, float speed, float time)
        {
            if (Distance(from, to) <= time * speed)
            {
                return to;
            }
            return from + Vector2.Normalize(to - from) * time * speed;
        }

        public static float Angle(Vector2 from, Vector2 to)
        {
            return (float)Math.Acos(BattleMath.Clamp(Vector2.Dot(from.normalized, to.normalized), -1f, 1f)) * 57.29578f;
        }

        public static float Dot(Vector2 lhs, Vector2 rhs)
        {
            return lhs.x_ * rhs.x_ + lhs.y_ * rhs.y_;
        }

        public Vector2 normalized
        {
            get
            {
                return this.Normalize();
            }
        }
    }
}
