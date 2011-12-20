using op = BCCL.Utility.GenericOperator.Operator;
using System;

namespace BCCL.Geometry.Primitives
{
    [Serializable]
    public class Rectangle<T> where T : struct
    {
        public T X1;
        public T X2;
        public T Y1;
        public T Y2;

        public Rectangle(T x1, T y1, T x2, T y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public T Left { get { return op.GreaterThan(X2, X1) ? X1 : X2; } }
        public T Right { get { return op.GreaterThan(X2, X1) ? X2 : X1; } }
        public T Top { get { return op.GreaterThan(Y2, Y1) ? Y1 : Y2; } }
        public T Bottom { get { return op.GreaterThan(Y2, Y1) ? Y2 : Y1; } }
        public T Height { get { return op.Subtract(Right, Left); } }
        public T Width { get { return op.Subtract(Bottom, Top); } }

        public bool Contains(T x, T y)
        {
            T zero = default(T);
            T xabs = op.Subtract(x, Left);
            T yabs = op.Subtract(y, Top);
            if (op.LessThan(xabs, zero))
                return false;

            if (op.LessThan(yabs, zero))
                return false;

            return op.LessThan(xabs, Width) && op.LessThan(yabs, Height);
        }

        public static Rectangle<T> FromLrtb(T left, T right, T top, T bottom)
        {
            return new Rectangle<T>(left, top, right, bottom);
        }
    }
}